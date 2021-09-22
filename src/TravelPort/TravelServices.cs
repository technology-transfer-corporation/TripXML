using System.Xml;
using TripXMLMain;
using System.Text;
using System;

namespace TravelPort
{
    public class TravelServices
    {
        public modCore.TripXMLProviderSystems ttProviderSystems;
        private StringBuilder sb = new StringBuilder();
        private string mstrVersion = "";
        private string mstrXslPath = "";       
        string _tracerID = "";
        public saveInDBData saveDbData;
       
        public struct saveInDBData
        {
            public string TravelBuildRQ;
            public string TravelBuildRS;
            public string portalSession;
        }

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
                mstrXslPath = sb.Append(value).Append("Travelport\\").ToString();
                sb.Remove(0, sb.Length);
            }
        }

         public string UpdateSessioned()
        {
            string strResponse = "";
            StringBuilder sbu = null;
            string strErrEvent = "";

            try
            {
                #region Get Tracer ID

                XmlDocument otaDoc = new XmlDocument();
                otaDoc.LoadXml(Request);
                var otaElement = otaDoc.DocumentElement;
                if (otaElement != null && otaElement.HasAttribute("EchoToken") && (otaElement).Attributes["EchoToken"].Value != null)
                {
                    _tracerID = otaElement.Attributes["EchoToken"].Value;
                }
                else
                { _tracerID = ""; }


                #endregion

                string strRequest = Request.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "").Replace("<?xml version=\"1.0\"?>", "");
                var oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                var oRoot = oDoc.DocumentElement;

                var ttTP = new TravelPortWSAdapter(ttProviderSystems) {TracerID = _tracerID};

                var strMessage = strRequest;

                #region Commented Code
                //******************************* 
                // Modify PNR - Delete elements * 
                //******************************* 
                // strErrEvent = "Modify PNR - Delete elements Error.";

                //if ((oRoot.SelectSingleNode("Position/Element[@Operation='delete']") != null))
                //{
                //    //******************************** 
                //    //* Build PNR Retrieve xml msg * 
                //    //******************************** 
                //    sbu = new StringBuilder();
                //    sbu.Append("<UpdateDelete>");
                //    sbu.Append(mstrRequest);
                //    sbu.Append(strNativePNRReply);
                //    sbu.Append("</UpdateDelete>");

                //    //******************************************************************* 
                //    //* Transform OTA Modify Request into Amadeus Native Delete Request * 
                //    //******************************************************************* 
                //    strRequest = CoreLib.TransformXML(sbu.ToString(), mstrXslPath, sb.Append(mstrVersion).Append("Travelport_UpdateDeleteRQ.xsl").ToString(), false);
                //    sbu.Remove(0, sbu.Length);
                //    sb.Remove(0, sb.Length);

                //    if (strRequest.IndexOf("<Error>") != -1)
                //    {
                //        //return BuildOTAResponse(strRequest.Substring(strRequest.IndexOf("<Error>"), (strRequest.IndexOf("</Error>") + 8) - strRequest.IndexOf("<Error>")));
                //    }

                //    //************************************** 
                //    //* Send Amadeus Native Delete Request * 
                //    //************************************** 
                //    oDocTemp = new XmlDocument();
                //    oDocTemp.LoadXml(strRequest);
                //    oRootTemp = oDocTemp.DocumentElement;

                //    strErrorResp = SendRequestSegment(oRootTemp.SelectSingleNode("Cancel").InnerXml, "Delete", "http://webservices.amadeus.com/" + ttProviderSystems.Profile + "/" + ttProviderSystems.TravelportSchema.PNR_Cancel, "http://xml.amadeus.com/" + ttProviderSystems.TravelportSchema.PNR_Reply);
                //    strNativePNRReply = strNativeResp.Replace("PNR_Reply", "PNR_RetrieveByRecLocReply");

                //    //******************** 
                //    //* Check for Errors * 
                //    //******************** 
                //    if (strErrorResp.Length > 0)
                //    {
                //        if (strErrorResp.IndexOf("<Error") >= 0)
                //        {
                //            // Fatal Error 
                //            return BuildOTAResponse(strErrorResp);
                //        }
                //        else if (strErrorResp.IndexOf("<Warning>") >= 0)
                //        {
                //            strWarnings += strErrorResp;
                //        }
                //    }

                //    strErrorResp = SendRequestSegment(oRootTemp.SelectSingleNode("RF").InnerXml, "ReceivedFrom", "http://webservices.amadeus.com/" + ttProviderSystems.Profile + "/" + ttProviderSystems.TravelportSchema.PNR_AddMultiElements, "http://xml.amadeus.com/" + ttProviderSystems.TravelportSchema.PNR_Reply);

                //    oDocTemp = null;
                //}
                #endregion

                //******************************* 
                // Modify PNR - Insert elements * 
                //******************************* 
                strErrEvent = "Modify PNR - Insert elements Error.";

                if (oRoot.SelectSingleNode("Position/Element[@Operation='insert']") != null)
                {
                    //******************************************************************* 
                    //* Transform OTA Modify Request into Amadeus Native Insert Request * 
                    //******************************************************************* 

                    sbu = new StringBuilder();
                    sbu.Append("<UpdateInsert>");
                    sbu.Append(Request);
                    sbu.Append("</UpdateInsert>");

                    strRequest = CoreLib.TransformXML(sbu.ToString(), mstrXslPath, sb.Append(mstrVersion).Append("Travelport_UpdateInsertRQ.xsl").ToString());
                    sbu.Remove(0, sbu.Length);
                    sb.Remove(0, sb.Length);

                    try
                    {
                        strResponse = ttTP.SendMessage(strRequest, TravelPortWSAdapter.enRequestType.UniversalRecordService);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(sbu.Append("Error Loading Transformed Request XML Document.").Append("\r\n").Append(ex.Message).ToString());
                    }

                }

                //******************************* 
                // Modify PNR - Modify elements * 
                //******************************* 
                //strErrEvent = "Modify PNR - Modify elements Error.";

                //if ((oRoot.SelectSingleNode("Position/Element[@Operation='modify']") != null))
                //{
                //    //******************************** 
                //    //* Build PNR Retrieve xml msg * 
                //    //******************************** 
                //    sbu = new StringBuilder();
                //    sbu.Append("<UpdateModify>");
                //    sbu.Append(mstrRequest);
                //    sbu.Append(strNativePNRReply);
                //    sbu.Append("</UpdateModify>");

                //    //******************************************************************* 
                //    //* Transform OTA Modify Request into Amadeus Native Delete Request * 
                //    //******************************************************************* 
                //    strRequest = CoreLib.TransformXML(sbu.ToString(), mstrXslPath, sb.Append(mstrVersion).Append("Travelport_UpdateModifyRQ.xsl").ToString(), false);
                //    sbu.Remove(0, sbu.Length);
                //    sb.Remove(0, sb.Length);

                //    oDocResp = new XmlDocument();
                //    oDocResp.LoadXml(strRequest);
                //    oRootResp = oDocResp.DocumentElement;

                //    //************************************** 
                //    //* Send Amadeus Native Delete Request * 
                //    //************************************** 
                //    foreach (XmlNode oNodeResp1 in oRootResp.ChildNodes)
                //    {
                //        if (oNodeResp1.OuterXml.Contains("Command_Cryptic"))
                //        {
                //            strErrorResp = SendRequestSegment(oNodeResp1.OuterXml, "Air", "http://webservices.amadeus.com/" + ttProviderSystems.Profile + "/" + ttProviderSystems.TravelportSchema.Command_Cryptic, "http://xml.amadeus.com/" + ttProviderSystems.TravelportSchema.Command_Cryptic);
                //        }
                //        else if (oNodeResp1.OuterXml.Contains("PNR_Cancel"))
                //        {
                //            strErrorResp = SendRequestSegment(oNodeResp1.OuterXml, "Air", "http://webservices.amadeus.com/" + ttProviderSystems.Profile + "/" + ttProviderSystems.TravelportSchema.PNR_Cancel, "http://xml.amadeus.com/" + ttProviderSystems.TravelportSchema.PNR_Cancel);
                //        }
                //        else
                //        {
                //            strErrorResp = SendRequestSegment(oNodeResp1.OuterXml, "Air", "http://webservices.amadeus.com/" + ttProviderSystems.Profile + "/" + ttProviderSystems.TravelportSchema.PNR_AddMultiElements, "http://xml.amadeus.com/" + ttProviderSystems.TravelportSchema.PNR_AddMultiElements);
                //        }

                //        //******************** 
                //        //* Check for Errors * 
                //        //******************** 
                //        if (strErrorResp.Length > 0)
                //        {
                //            if (strErrorResp.IndexOf("<Error") >= 0)
                //            {
                //                // Fatal Error 
                //                return BuildOTAResponse(strErrorResp);
                //            }
                //            else if (strErrorResp.IndexOf("<Warning>") >= 0)
                //            {
                //                strWarnings += strErrorResp;
                //            }
                //        }

                //    }
                //}

                ttTP = null;

                //***************************************************************** 
                // Transform Native Amadeus TravelBuild Response into OTA Response * 
                //***************************************************************** 
                strErrEvent = "Travelport_PNRReadRS.xsl Error.";

                mstrVersion = Request.Contains("v03") ? "v03_" : "v03_";

                strResponse = CoreLib.TransformXML(strResponse, mstrXslPath, sb.Append(mstrVersion).Append("Travelport_PNRReadRS.xsl").ToString());
                sb.Remove(0, sb.Length);


                return strResponse;
            }
            catch (Exception exx)
            {
                throw new Exception(sbu.Append(strErrEvent).Append("\r\n").Append(exx.Message).ToString());
            }
        }
    }
}