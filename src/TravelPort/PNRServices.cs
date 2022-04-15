using System.Xml;
using TripXMLMain;
using System.Text;
using System;
using System.Globalization;
using System.IO;

namespace Travelport
{
    public class PNRServices
    {
        public modCore.TripXMLProviderSystems ttProviderSystems;
        private StringBuilder sb = new StringBuilder();
        private string mstrVersion = "";
        private string mstrXslPath = "";
        private string _tracerID = "";

        public string Request { get; set; } = "";

        public string Version
        {
            get { return mstrVersion; }
            set
            {
                mstrVersion = value;
                if (mstrVersion.Length > 0) mstrVersion += "_";
            }
        }

        public string XslPath
        {
            get { return mstrXslPath; }
            set
            {
                mstrXslPath = sb.Append(value).Append("TravelPort\\").ToString();
                sb.Remove(0, sb.Length);
            }
        }

        public string PNRRead()
        {
            string strResponse;
            DateTime RequestTime = DateTime.Now;
            //*****************************************************************
            // Transform OTA PNRRead Request into Native Amadeus Request     *
            //***************************************************************** 

            try
            {
                string strRequest ;
                string strRetrieve;
                string strSearch ;
                string strImport ;
                string strProviderRecLoc ;
                try
                {
                    #region Get Tracer ID

                    XmlDocument otaDoc = new XmlDocument();
                    XmlElement otaElement;
                    otaDoc.LoadXml(Request);
                    otaElement = otaDoc.DocumentElement;
                    if (otaElement != null && otaElement.HasAttribute("EchoToken") && (otaElement).Attributes["EchoToken"].Value != null)
                    {
                        _tracerID = (otaElement).Attributes["EchoToken"].Value;
                    }
                    else
                    { _tracerID = ""; }

                    strProviderRecLoc = otaElement.SelectSingleNode("UniqueID/@ID").InnerText;
                    otaDoc = null;
                    otaElement = null;

                    #endregion

                    Request = Request.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "").Replace("<?xml version=\"1.0\"?>", "");
                    strRequest = Request;

                    strRequest = CoreLib.TransformXML(strRequest, mstrXslPath, sb.Append(mstrVersion).Append("Travelport_PNRReadRQ.xsl").ToString(), false);
                    sb.Remove(0, sb.Length);

                    var natDoc = new XmlDocument();
                    natDoc.LoadXml(strRequest);
                    var natElement = natDoc.DocumentElement;

                    strRetrieve = natElement.SelectSingleNode("RetrieveReq").InnerXml;
                    strSearch = natElement.SelectSingleNode("SearchReq").InnerXml;
                    strImport = natElement.SelectSingleNode("ImportReq").InnerXml;
                }
                catch (Exception ex)
                {
                    throw new Exception(sb.Append(sb.Append("Error Transforming OTA Request. ").Append(ex.Message).ToString()).ToString());
                }

                if (strRequest.Length == 0)
                {
                    throw new Exception("Transformation produced empty xml.");
                }

                TravelPortWSAdapter ttTP;
                //*******************************************************************************
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                //******************************************************************************* 

                try
                {
                    ttTP = new TravelPortWSAdapter(ttProviderSystems) { TracerID = _tracerID };
                    // send retrieve universal record (UR)
                    strResponse = ttTP.SendMessage(strRetrieve, TravelPortWSAdapter.enRequestType.UniversalRecordService);

                    if (strResponse.Contains("Record locator not found"))
                    {
                        // search UR by GDS locator
                        strResponse = ttTP.SendMessage(strSearch, TravelPortWSAdapter.enRequestType.UniversalRecordService);

                        if (strResponse.Contains("No matching records found for the given parameters"))
                        {
                            // GDS locator does not exist as UR, so import it
                            strResponse = ttTP.SendMessage(strImport, TravelPortWSAdapter.enRequestType.UniversalRecordService);
                        }
                        //------------------------------------------------------------
                        // It appears that no need for any prior manipulations with message
                        //------------------------------------------------------------
                        //else
                        //{
                        //    // get UR locator and retrieve it
                        //    XmlDocument otaDoc = new XmlDocument();
                        //    otaDoc.LoadXml(strResponse);
                        //    var otaElement = otaDoc.DocumentElement;

                        //    var nsmgr = new XmlNamespaceManager(otaDoc.NameTable);
                        //    nsmgr.AddNamespace("un", "http://www.travelport.com/schema/universal_v27_0");

                        //    var strURRecLoc = otaElement.SelectSingleNode("un:UniversalRecordSearchResult/@UniversalRecordLocatorCode", nsmgr).InnerText;

                        //    strRetrieve = strRetrieve.Replace(strProviderRecLoc, strURRecLoc);

                        //    strResponse = ttTP.SendMessage(strRetrieve, TravelPortWSAdapter.enRequestType.UniversalRecordService);
                        //}
                    }

                    //strResponse = strResponse.Replace(" xmlns=\"http://xml.amadeus.com/" + ttProviderSystems.TravelportSchema.PNR_RetrieveByRecLocReply + "\"", "");
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                //*****************************************************************
                // Transform Native Amadeus PNRRead Response into OTA Response   *
                //***************************************************************** 

                ttTP = null;

                try
                {
                    strResponse = CoreLib.TransformXML(strResponse, mstrXslPath, sb.Append(mstrVersion).Append("Travelport_PNRReadRS.xsl").ToString());
                    sb.Remove(0, sb.Length);
                    return strResponse;
                }

                catch (Exception ex)
                {
                    throw new Exception(sb.Append(sb.Append("Error Transforming Native Response.").Append("\r\n").Append(ex.Message)).ToString());

                }
            }
            catch (Exception exx)
            {
                addLog("<EXOR><M>" + Request + "<BL/>", ttProviderSystems.UserID);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.PNRRead, exx.Message, ttProviderSystems);
            }
            finally
            {
                sb.Remove(0, sb.Length);
            }

            sb = null;
            return strResponse;
        }

        public string PNRReprice()
        {
            string strResponse;
            DateTime RequestTime;
            //*****************************************************************
            // Transform OTA PNRReprice Request into Native Travelport Request     *
            //***************************************************************** 
            RequestTime = DateTime.Now;
            try
            {
                string strRequest;
                try
                {
                    #region Get Tracer ID

                    XmlDocument otaDoc = new XmlDocument();
                    XmlElement otaElement;
                    otaDoc.LoadXml(Request);
                    otaElement = otaDoc.DocumentElement;
                    if (otaElement != null && otaElement.HasAttribute("EchoToken") && (otaElement).Attributes["EchoToken"].Value != null)
                    {
                        _tracerID = (otaElement).Attributes["EchoToken"].Value;
                    }
                    else
                    { _tracerID = ""; }

                    #endregion

                    Request = Request.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "").Replace("<?xml version=\"1.0\"?>", "");
                    strRequest = Request;

                    strRequest = CoreLib.TransformXML(strRequest, mstrXslPath, sb.Append(mstrVersion).Append("Travelport_PNRRepriceRQ.xsl").ToString(), false);
                    sb.Remove(0, sb.Length);
                }
                catch (Exception ex)
                {
                    throw new Exception(sb.Append(sb.Append("Error Transforming OTA Request. ").Append(ex.Message).ToString()).ToString());
                }

                if (strRequest.Length == 0)
                {
                    throw new Exception("Transformation produced empty xml.");
                }

                //*******************************************************************************
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                //******************************************************************************* 

                string strRetrieve;
                TravelPortWSAdapter ttTP;
                try
                {
                    ttTP = new TravelPortWSAdapter(ttProviderSystems);
                    ttTP.TracerID = _tracerID;

                    // send retrieve universal record (UR)
                    strResponse = ttTP.SendMessage(strRequest, TravelPortWSAdapter.enRequestType.UniversalRecordService);
                    strRetrieve = strResponse;

                    if (strResponse.Contains("universal:UniversalRecord LocatorCode="))
                    {
                        // create and send pricing message
                        strRequest = Request.Replace("</OTA_PNRRepriceRQ>", "<Response>" + strResponse + "</Response></OTA_PNRRepriceRQ>");
                        strResponse = CoreLib.TransformXML(strRequest, mstrXslPath, sb.Append(mstrVersion).Append("Travelport_PNRRepriceRQ.xsl").ToString(), false);
                        sb.Remove(0, sb.Length);

                        strResponse = ttTP.SendMessage(strResponse, TravelPortWSAdapter.enRequestType.AirService);
                        strRetrieve = strRetrieve.Replace("</universal:UniversalRecordRetrieveRsp>", strResponse + "</universal:UniversalRecordRetrieveRsp>");

                        if(Request.Contains("StoreFare='true'"))
                        {
                            // store new pricing in UR
                            strRequest = Request.Replace("</OTA_PNRRepriceRQ>", "<NewPrice>" + strResponse + "</NewPrice></OTA_PNRRepriceRQ>");
                            strResponse = CoreLib.TransformXML(strRequest, mstrXslPath, sb.Append(mstrVersion).Append("Travelport_PNRRepriceRQ.xsl").ToString(), false);
                            sb.Remove(0, sb.Length);

                            strResponse = ttTP.SendMessage(strResponse, TravelPortWSAdapter.enRequestType.UniversalRecordService);
                            strRetrieve = strRetrieve.Replace("</universal:UniversalRecordRetrieveRsp>", strResponse + "</universal:UniversalRecordRetrieveRsp>");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                //*****************************************************************
                // Transform Native Travelport PNRReprice Response into OTA Response   *
                //***************************************************************** 

                ttTP = null;

                try
                {
                    strResponse = CoreLib.TransformXML(strRetrieve, mstrXslPath, sb.Append(mstrVersion).Append("Travelport_PNRRepriceRS.xsl").ToString());
                    sb.Remove(0, sb.Length);

                    return strResponse;
                }

                catch (Exception ex)
                {
                    throw new Exception(sb.Append(sb.Append("Error Transforming Native Response.").Append("\r\n").Append(ex.Message)).ToString());

                }
            }
            catch (Exception exx)
            {
                addLog("<EXOR><M>" + Request + "<BL/>", ttProviderSystems.UserID);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.PNRRead, exx.Message, ttProviderSystems);
            }
            finally
            {
                sb.Remove(0, sb.Length);
            }

            sb = null;
            return strResponse;
        }

        public string Queue()
        {
            TravelPortWSAdapter ttTP ;
            string strRequest;
            string strResponse = "";
            string ConversationID = "";
            string strBranch = "";
            string strHost = "";
            XmlDocument otaDoc ;
            XmlElement otaElement;


            //*****************************************************************
            // Transform OTA Queue Request into Native TravelPort Request     *
            //***************************************************************** 

            try
            {
                #region Get Tracer ID

                otaDoc = new XmlDocument();
                otaDoc.LoadXml(Request);
                otaElement = otaDoc.DocumentElement;
                if (otaElement != null && otaElement.HasAttribute("EchoToken") && (otaElement).Attributes["EchoToken"].Value != null)
                {
                    _tracerID = (otaElement).Attributes["EchoToken"].Value;
                }
                else
                { _tracerID = ""; }

                if (otaElement.SelectSingleNode("ListQueue/@PseudoCityCode") != null)
                    strBranch = otaElement.SelectSingleNode("ListQueue/@PseudoCityCode").InnerText;
                else if (otaElement.SelectSingleNode("PlaceQueue/@PseudoCityCode") != null)
                    strBranch = otaElement.SelectSingleNode("PlaceQueue/@PseudoCityCode").InnerText;
                else if (otaElement.SelectSingleNode("RemoveQueue/@PseudoCityCode") != null)
                    strBranch = otaElement.SelectSingleNode("RemoveQueue/@PseudoCityCode").InnerText;

                if (otaElement.HasAttribute("Target"))
                {
                    switch (otaElement.Attributes["Target"].Value)
                    {
                        case "WSP":
                            strHost = "1P";
                            break;
                        case "GAL":
                            strHost = "1G";
                            break;
                        default:
                            strHost = "1V";
                            break;
                    }
                }

                otaDoc = null;
                otaElement = null;

                #endregion

                strRequest = Request;
                strRequest = CoreLib.TransformXML(strRequest, mstrXslPath, sb.Append(mstrVersion).Append("Travelport_QueueRQ.xsl").ToString());
                sb.Remove(0, sb.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(sb.Append(sb.Append("Error Transforming OTA Request. ").Append(ex.Message)).ToString());
            }

            if (strRequest.Length == 0)
            {
                throw new Exception("Transformation produced empty xml.");
            }

            //*******************************************************************************
            // Send Transformed Request to the TravelPort Adapter and Getting Native Response  *
            //******************************************************************************* 

            try
            {

                ttTP = new TravelPortWSAdapter(ttProviderSystems) {TracerID = _tracerID};

                if (Request.IndexOf("ListQueue") != -1)
                {
                    if (strRequest.Contains("QLD"))
                    {
                        // process Worldspan queue
                        ConversationID = ttTP.CreateTerminalSession(strBranch, strHost);

                        strResponse = ttTP.SubmitTerminalTransaction(strRequest, strBranch, strHost, ConversationID);

                        if (strResponse.Contains(")"))
                        {
                            int i = 0;
                            var strResponseQL = strResponse;

                            while(i < 10 && strResponse.Contains(")"))
                            {
                                strResponse = ttTP.SubmitTerminalTransaction("MD", strBranch, strHost, ConversationID);
                                strResponseQL += strResponse;

                                i++;
                            }

                            strResponse = strResponseQL;
                        }

                        strResponse = "<ListQueue>" + strResponse + "</ListQueue>";

                        ConversationID = ttTP.CloseTerminalSession(strBranch, strHost, ConversationID);
                    }
                    else
                    {
                        // process Galileo queue (TBD)
                    }
                }
                else if (Request.IndexOf("PlaceQueue") != -1)
                {
                    if (strRequest.Contains("QEP/"))
                    {
                        // process Worldspan queue
                        ConversationID = ttTP.CreateTerminalSession(strBranch, strHost);

                        // send *pnrloc
                        strResponse = ttTP.SubmitTerminalTransaction(strRequest.Substring(0,7), strBranch, strHost, ConversationID);

                        if (!strResponse.Contains("INVALID") && !strResponse.Contains("INVLD ADDRESS"))
                        {
                            strResponse = ttTP.SubmitTerminalTransaction(strRequest.Substring(7), strBranch, strHost, ConversationID);
                        }

                        strResponse = "<PlaceQueue>" + strResponse + "</PlaceQueue>";

                        ConversationID = ttTP.CloseTerminalSession(strBranch, strHost, ConversationID);
                    }
                    else
                    {
                        // process Galileo queue (TBD)
                    }
                }
                else if (Request.IndexOf("RemoveQueue") != -1)
                {
                    if (strRequest.Contains("QRQ/"))
                    {
                        // process Worldspan queue
                        ConversationID = ttTP.CreateTerminalSession(strBranch, strHost);

                        // send *pnrloc
                        strResponse = ttTP.SubmitTerminalTransaction(strRequest.Substring(0, 7), strBranch, strHost, ConversationID);

                        if (!strResponse.Contains("INVALID") && !strResponse.Contains("INVLD ADDRESS"))
                        {
                            strResponse = ttTP.SubmitTerminalTransaction(strRequest.Substring(7), strBranch, strHost, ConversationID);
                        }

                        strResponse = "<RemoveQueue>" + strResponse + "</RemoveQueue>";

                        ConversationID = ttTP.CloseTerminalSession(strBranch, strHost, ConversationID);
                    }
                    else
                    {
                        // process Galileo queue (TBD)
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //*******************************************************************************
            // check if message is queue lists and if need need to scroll the response      *
            //******************************************************************************* 
            try
            {
                ConversationID = "";
                ttTP = null;

                try
                {

                    strResponse = CoreLib.TransformXML(strResponse, mstrXslPath, sb.Append(mstrVersion).Append("Travelport_QueueRS.xsl").ToString(), false);
                    sb.Remove(0, sb.Length);

                    return strResponse;
                }

                catch (Exception ex)
                {
                    throw new Exception(sb.Append(sb.Append("Error Transforming Native Response.").Append("\r\n").Append(ex.Message).ToString()).ToString());
                }
            }

            catch (Exception ex)
            {
                if (ConversationID != "")
                {
                    ttTP.CloseTerminalSession(strBranch,strHost,ConversationID);
                }

                throw ex;
            }
            finally
            {
                sb = null;
            }
            
        }

        public static void addLog(string msg, string username)
        {
            try
            {
                string FilePath = "log\\" + username + "_" + DateTime.Today.ToString("dd-MM-yyyy");
                string DirPath = "C:\\TripXML\\log";
                FilePath = "C:\\TripXML\\" + FilePath + ".txt";

                if (!Directory.Exists(DirPath))
                {
                    Directory.CreateDirectory(DirPath);
                }
                if (!File.Exists(FilePath))
                {
                    using (StreamWriter sw = File.CreateText(FilePath))
                    {
                        sw.WriteLine("created On - " + DateTime.Now + "\r\n");
                        sw.Flush();
                        sw.Close();
                    }
                }
                using (StreamWriter sw = File.AppendText(FilePath))
                {
                    DateTimeFormatInfo myDTFI = new CultureInfo("fr-FR", true).DateTimeFormat;

                    sw.WriteLine(DateTime.UtcNow.ToString(myDTFI).Substring(11) + " GMT - " + msg + "\r\n");
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
