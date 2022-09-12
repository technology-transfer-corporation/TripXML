using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TripXMLMain;
using System.Web.Configuration;

namespace SITA
{
    public class OtherServices
    {
        private StringBuilder sb = new StringBuilder();
        public modCore.TripXMLProviderSystems ttProviderSystems;

        private string mstrRequest = "";
        private string mstrVersion = "";
        private string mstrXslPath = "";

        public string Request
        {
            get
            {
                return mstrRequest;
            }
            set
            {
                mstrRequest = value;
            }
        }

        public string Version
        {
            get
            {
                return mstrVersion;
            }
            set
            {
                mstrVersion = value;
                if ((mstrVersion.Length > 0))
                {
                    mstrVersion = mstrVersion + "_";
                }
            }
        }

        public string XslPath
        {
            get
            {
                return mstrXslPath;
            }
            set
            {
                mstrXslPath = sb.Append(value).Append("SITA\\").ToString();
                sb.Remove(0, sb.Length);
            }
        }

        public string CreateSession()
        {
            Sita.Sws.SITAAdapter ttSI = null;
            string ConversationID = "";
            string strResponse = "";
            try
            {
                Uri SitaUri = new Uri(ttProviderSystems.URL);
                ttSI = new Sita.Sws.SITAAdapter(SitaUri, ttProviderSystems);
                ConversationID = ttSI.CreateSession();
                //}

                //  Build Response.
                strResponse = sb.Append("<SessionCreateRS Version=\'1.001\'><Success/>").Append("<ConversationID>").Append(ConversationID).Append("</ConversationID></SessionCreateRS>").ToString();
                sb.Remove(0, sb.Length);
                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(sb.Append("Session was not Created.").Append("\r\n").Append(ex.Message).ToString());
                sb.Remove(0, sb.Length);
            }
            finally
            {
                if (!(ttSI == null))
                {
                    ttSI = null;
                }
                sb = null;
            }
        }

        public string CloseSession()
        {
            Sita.Sws.SITAAdapter ttSI = null;
            XmlDocument oReqDoc = null;
            XmlElement oRoot = null;
            string ConversationID = "";
            string strRequest = "";
            string strResponse = "";
            try
            {
                strRequest = mstrRequest;
                oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(strRequest);
                oRoot = oReqDoc.DocumentElement;
                ConversationID = oRoot.SelectSingleNode("POS/TPA_Extensions/ConversationID").InnerText;
            }
            catch (Exception ex)
            {
                throw new Exception(sb.Append("ConversationID is missing in the Request.").Append("\r\n").Append(ex.Message).ToString());
                sb.Remove(0, sb.Length);
            }
            finally
            {
                oRoot = null;
                oReqDoc = null;
            }
            if ((ConversationID.Length == 0))
            {
                throw new Exception("ConversationID is missing in the Request.");
            }
            //  *******************
            //  Close Session    *
            //  *******************
            try
            {
                Uri SitaUri = new Uri(ttProviderSystems.URL);
                ttSI = new Sita.Sws.SITAAdapter(SitaUri, ttProviderSystems);
                strResponse = ttSI.CloseSession(ConversationID);

                //  Build Response.
                strResponse = "<SessionCloseRS Version=\'1.001\'><Success/></SessionCloseRS>";
                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(sb.Append("Session was not Closed.").Append("\r\n").Append(ex.Message).ToString());
                sb.Remove(0, sb.Length);
            }
            finally
            {
                if (!(ttSI == null))
                {
                    ttSI = null;
                }
                oReqDoc = null;
                oRoot = null;
                sb = null;
            }
        }

        public string Cryptic()
        {
            Sita.Sws.SITAAdapter ttSI = null;
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            string ConversationID = "";
            string strRequest = "";
            string strResponse = "";
            string strScreen = "";
            string resp = "";
            try
            {
                strRequest = mstrRequest;
                oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                oRoot = oDoc.DocumentElement;
                if (!(oRoot.SelectSingleNode("POS/TPA_Extensions/ConversationID") == null))
                {
                    ConversationID = oRoot.SelectSingleNode("POS/TPA_Extensions/ConversationID").InnerText;
                }

                strRequest = oRoot.SelectSingleNode("Entry").InnerText;
            }
            catch (Exception ex)
            {
                throw new Exception(sb.Append("Invalid Request.").Append("\r\n").Append(ex.Message).ToString());
                sb.Remove(0, sb.Length);
            }
            finally
            {
                oRoot = null;
                oDoc = null;
            }

            // *******************************************************************************
            //  Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
            // ******************************************************************************* 
            try
            {
                sb.Append("<OTA_ScreenTextRQ xmlns=\"http://www.opentravel.org/OTA/2003/05\" TransactionIdentifier=\"" + ConversationID + "\">");
                sb.Append("<POS><Source ERSP_UserID=\"").Append(ttProviderSystems.UserName).Append("\" AgentSine=\"").Append(ttProviderSystems.Password).Append("\" PseudoCityCode=\"");
                sb.Append(ttProviderSystems.PCC).Append("\" AgentDutyCode=\"17\" ISOCountry=\"EC\" AirlineVendorID=\"EQ\" AirportCode=\"");
                sb.Append(ttProviderSystems.PCC.Substring(0, 3)).Append("\"/></POS><ScreenEntry>" + strRequest + "</ScreenEntry></OTA_ScreenTextRQ>");
                strRequest = sb.ToString();
                sb.Remove(0, sb.Length);

                Uri SitaUri = new Uri(ttProviderSystems.URL);
                ttSI = new Sita.Sws.SITAAdapter(SitaUri, ttProviderSystems);
                resp = ttSI.Send(strRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ttSI = null;
            }
            // ****************************************************************
            //  Transform Native SITA Cryptic Response into OTA Response   *
            // **************************************************************** 
            try
            {
                oDoc = new XmlDocument();
                oDoc.LoadXml(resp);
                oRoot = oDoc.DocumentElement;

                strResponse = "<CrypticRS Version=\"1.0\"><Response></Response></CrypticRS>";

                if (!(oRoot.SelectSingleNode("TextScreens") == null))
                {
                    strScreen = oRoot.SelectSingleNode("TextScreens/TextScreen").InnerXml;
                    strScreen = strScreen.Replace("TextData>","Line>");

                    if (strScreen.Length > 0)
                    {
                        strResponse = strResponse.Replace("</CrypticRS>", "<Screen>" + strScreen + "</Screen></CrypticRS>");

                        strScreen = strScreen.Replace("<Line>", "").Replace("</Line>","\r\n");
                        strResponse = strResponse.Replace("</Response>", strScreen + "</Response>");
                    }
                }
                //  Insert ConversationID
                if (ConversationID.Length > 0)
                {
                    ConversationID = string.Concat("<ConversationID>", ConversationID, "</ConversationID>");
                    strResponse = strResponse.Insert(strResponse.IndexOf("</CrypticRS>"), ConversationID);
                }
                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                oRoot = null;
                oDoc = null;
                sb = null;
            }
        }

        public string Native()
        {
            Sita.Sws.SITAAdapter ttSI = null;
            XmlDocument oReqDoc = null;
            XmlElement oRoot = null;
            string strRequest = "";
            string strResponse = "";
            try
            {
                strRequest = mstrRequest;
                oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(strRequest);
                oRoot = oReqDoc.DocumentElement;
                if (oRoot.SelectSingleNode("Native") == null)
                {
                    throw new Exception("Native Message is missing in the Request.");
                }
                else
                {
                    strRequest = oRoot.SelectSingleNode("Native").InnerXml;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(sb.Append("Invalid Request.").Append("\r\n").Append(ex.Message).ToString());
                sb.Remove(0, sb.Length);
            }
            finally
            {
                oRoot = null;
                oReqDoc = null;
            }
            // *******************************************************************************
            //  Send Native Request to the SITA Adapter and Getting Native Response  *
            // ******************************************************************************* 
            try
            {
                Uri SitaUri = new Uri(ttProviderSystems.URL);

                ttSI = new Sita.Sws.SITAAdapter(SitaUri, ttProviderSystems);

                strResponse = ttSI.Send(strRequest);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ttSI = null;
            }
            // ********************
            //  Build Response    *
            // ********************
            try
            {
                strResponse = sb.Append("<NativeRS><Success/><Response>").Append(strResponse.Replace("<", "&lt;").Replace(">", "&gt;")).Append("</Response>").Append("</NativeRS>").ToString();
                sb.Remove(0, sb.Length);
                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(sb.Append("Error in Native Response.").Append("\r\n").Append(ex.Message).ToString());
                sb.Remove(0, sb.Length);
            }
            finally
            {
                sb = null;
            }
        }

        public string InventoryManagement()
        {
            Sita.Sws.SITAAdapter ttSI = null;
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            string ConversationID = "";
            string strRequest = "";
            string strResponse = "";
            string resp = "";
            try
            {
                strRequest = mstrRequest;
                oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                oRoot = oDoc.DocumentElement;
            }
            catch (Exception ex)
            {
                throw new Exception(sb.Append("Invalid Request.").Append("\r\n").Append(ex.Message).ToString());
                sb.Remove(0, sb.Length);
            }
            finally
            {
                oRoot = null;
                oDoc = null;
            }

            // ****************************************************************
            //  Transform OTA ShowMileage Request into Native Amadeus Request *
            // **************************************************************** 
            try
            {
                strRequest = CoreLib.TransformXML(strRequest, mstrXslPath, sb.Append(mstrVersion).Append("SITA_InventoryManagementRQ.xsl").ToString(), false);
                sb.Remove(0, sb.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(sb.Append(sb.Append("Error Transforming TXML Request. ").Append(ex.Message).ToString()).ToString());
                sb.Remove(0, sb.Length);
            }

            if (strRequest.Length == 0)
            {
                throw new Exception("Transformation produced empty xml.");
            }

            Uri SitaUri = new Uri(ttProviderSystems.URL);
            ttSI = new Sita.Sws.SITAAdapter(SitaUri, ttProviderSystems);
            ConversationID = ttSI.CreateSession();

            strRequest = strRequest.Replace("SESSION", ConversationID);

            // *******************************************************************************
            //  Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
            // ******************************************************************************* 
            try
            {
                ttSI = new Sita.Sws.SITAAdapter(SitaUri, ttProviderSystems);
                resp = ttSI.Send(strRequest);
                resp = resp.Replace(" xmlns=\"http://www.opentravel.org/OTA/2003/05\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"","");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //  *******************
            //  Close Session    *
            //  *******************
            try
            {
                ttSI = new Sita.Sws.SITAAdapter(SitaUri, ttProviderSystems);
                strResponse = ttSI.CloseSession(ConversationID);
            }
            catch (Exception ex)
            {
                throw new Exception(sb.Append("Session was not Closed.").Append("\r\n").Append(ex.Message).ToString());
                sb.Remove(0, sb.Length);
            }

            // ****************************************************************
            //  Transform Native SITA Cryptic Response into OTA Response   *
            // **************************************************************** 
            try
            {
                resp = resp.Replace("</OTA_ScreenTextRS>", "<NowDate>" + DateTime.Now.ToString("yyyy-MM-dd") + "</NowDate></OTA_ScreenTextRS>");
                strResponse = CoreLib.TransformXML(resp, mstrXslPath, sb.Append(mstrVersion).Append("SITA_InventoryManagementRS.xsl").ToString(), false);
                sb.Remove(0, sb.Length);
                
                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                oRoot = null;
                oDoc = null;
                sb = null;
            }
        }
    }
}
