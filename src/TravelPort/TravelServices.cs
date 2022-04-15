using System.Xml;
using TripXMLMain;
using System.Text;
using System;

namespace Travelport
{
    public class TravelServices : TravelportBase
    {           
        string _tracerID = "";
        public saveInDBData saveDbData;
       
        public struct saveInDBData
        {
            public string TravelBuildRQ;
            public string TravelBuildRS;
            public string portalSession;
        }

        public string TravelBuild()
        {
            string strResponse;

            // *******************************************************************
            // Transform OTA Travel Build Request into Native Worldspan Request *
            // ******************************************************************* 

            try
            {
                XmlNode oNode;
                Version = "";
                var strRequest = SetRequest("Travelport_TravelBuildRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                CoreLib.SendTrace(ProviderSystems.UserID, "ttWorldspanService", "OTA Transformed Request", strRequest,
                    ProviderSystems.LogUUID);


                // *************************
                // Get Multiple Requests  *
                // *************************
                var oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                var oRoot = oDoc.DocumentElement;
                var strBPC = oRoot.SelectSingleNode("TTBPC").InnerXml;
                var strRMC = oRoot.SelectSingleNode("TTRMC") != null ? oRoot.SelectSingleNode("TTRMC").InnerXml : "";
                var strUPC = oRoot.SelectSingleNode("TTUPC") != null ? oRoot.SelectSingleNode("TTUPC").InnerXml : "";


                // *******************************************************************************
                // Send Transformed Request to the Worldspan Adapter and Getting Native Response*
                // ******************************************************************************* 
                var ttWA = SetAdapter(ProviderSystems);
                strResponse = ttWA.SendMessage(strBPC, TravelPortWSAdapter.enRequestType.UniversalRecordService);
                var strNative = $"{strBPC}{strResponse}";

                // **************************************************
                // Get record locator                              *
                // ************************************************** 
                var oDocResp = new XmlDocument();
                oDocResp.LoadXml(strResponse);
                var oRootResp = oDocResp.DocumentElement;
                var oNodeResp = oRootResp.SelectSingleNode("PNR_RLOC");
                if (oNodeResp != null)
                {
                    // **************************************
                    // check if any SSR and send if yes *
                    // **************************************
                    if (!string.IsNullOrEmpty(strUPC))
                    {
                        oNode = oRoot.SelectSingleNode("TTUPC/UPC7/PNR_RLOC");
                        oNode.InnerText = oNodeResp.InnerText;
                        strRequest = oRoot.SelectSingleNode("TTUPC").InnerXml;

                        strResponse = ttWA.SendMessage(strRequest, TravelPortWSAdapter.enRequestType.Message);
                        strNative += $"{strRequest}{strResponse}";
                    }

                    // **************************************
                    // check if any Remark and send if yes *
                    // **************************************
                    if (!string.IsNullOrEmpty(strRMC))
                    {
                        oNode = oRoot.SelectSingleNode("TTRMC/RMC2/PNR_RLOC");
                        oNode.InnerText = oNodeResp.InnerText;
                        strRequest = oRoot.SelectSingleNode("TTRMC").InnerXml;

                        strResponse = ttWA.SendMessage(strRequest, TravelPortWSAdapter.enRequestType.Message);
                        strNative += $"{strRequest}{strResponse}";
                    }

                    // ********************
                    // Send retrieve PNR *
                    // ******************** 
                    oNode = oRoot.SelectSingleNode("TTDPC/DPC8/REC_LOC");
                    oNode.InnerText = oNodeResp.InnerText;
                    strRequest = oRoot.SelectSingleNode("TTDPC").InnerXml;

                    strResponse = ttWA.SendMessage(strRequest, TravelPortWSAdapter.enRequestType.Message);
                    strNative += $"{strRequest}{strResponse}";
                }

                // ************************************************
                // calculate year in all dates and arrival date  *
                // ************************************************

                try
                {
                    oDoc.LoadXml(strResponse);
                    oRoot = oDoc.DocumentElement;
                    if (oRoot.SelectNodes("AIR_SEGMENT_INFO") != null)
                    {
                        foreach (XmlNode currentONode in oRoot.SelectNodes("AIR_SEGMENT_INFO/AIR_ITEM"))
                        {
                            oNode = currentONode;
                            DateTime dtDepartureDate =
                                Convert.ToDateTime(
                                    $"{oNode.SelectSingleNode("DEP_DATE/DEP_DAY").InnerText}{oNode.SelectSingleNode("DEP_DATE/DEP_MONTH").InnerText}{DateTime.Now.Year}");

                            DateTime dtArrivalDate =
                                Convert.ToDateTime(
                                    $"{oNode.SelectSingleNode("ARR_DATE/ARR_DAY").InnerText}{oNode.SelectSingleNode("ARR_DATE/ARR_MONTH").InnerText}{DateTime.Now.Year}");

                            if (DateTime.Now.DayOfYear > dtDepartureDate.DayOfYear)
                            {
                                dtDepartureDate = dtDepartureDate.AddYears(1);
                            }

                            oNode.SelectSingleNode("DEP_DATE").InnerText =
                                dtDepartureDate.ToString("yyyy-MM-dd");
                            if (DateTime.Now.DayOfYear > dtArrivalDate.DayOfYear)
                            {
                                dtArrivalDate = dtArrivalDate.AddYears(1);
                            }

                            oNode.SelectSingleNode("ARR_DATE").InnerText = dtArrivalDate.ToString("yyyy-MM-dd");
                        }

                        strResponse = oRoot.OuterXml;
                    }

                    // *****************************************************************
                    // Transform Native Worldspan PNRRead Response into OTA Response  *
                    // ***************************************************************** 
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Worldspan_PNRReadRS.xsl");

                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }

            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.TravelBuild, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string UpdateSessioned()
        {
            string strResponse = "";            
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

                var ttTP = SetAdapter(ProviderSystems);

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
                var strErrEvent = "Modify PNR - Insert elements Error.";

                if (oRoot.SelectSingleNode("Position/Element[@Operation='insert']") != null)
                {
                    //******************************************************************* 
                    //* Transform OTA Modify Request into Amadeus Native Insert Request * 
                    //******************************************************************* 
                    strRequest = CoreLib.TransformXML($"<UpdateInsert>{Request}</UpdateInsert>", XslPath, $"{Version}Travelport_UpdateInsertRQ.xsl");

                    try
                    {
                        strResponse = ttTP.SendMessage(strRequest, TravelPortWSAdapter.enRequestType.UniversalRecordService);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error in Native Response.\r\n{ex.Message}");
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

                //***************************************************************** 
                // Transform Native Amadeus TravelBuild Response into OTA Response * 
                //***************************************************************** 
                strErrEvent = "Travelport_PNRReadRS.xsl Error.";
                Version = Request.Contains("v03") ? "v03_" : "v03_";
                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Travelport_PNRReadRS.xsl");                
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.UpdateSessioned, ex.Message, ProviderSystems);
            }

            return strResponse;
        }
    }
}