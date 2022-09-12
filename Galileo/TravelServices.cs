using System;
using System.Globalization;
using System.Threading;
using System.Xml;
using TripXMLMain;

namespace Galileo
{
    public class TravelServices : GalileoBase
    {
        public string Warnings { get; set; }

        public string Errors { get; set; }

        public string Message { get; set; }

        public TravelServices()
        {
            Request = string.Empty;
            ConversationID = string.Empty;
            Warnings = string.Empty;
            Errors = string.Empty;
        }

        public string TravelBuild()
        {
            string strResponse;

            try
            {
                string strFIC = "";
                string strError;
                string strRecloc = "";
                bool autoTicketing = false;
                string iEndPrice = "";
                string strMandatory = "";
                string strET = "";
                string strETR = "";
                string strRules = "";
                string strDoubleET = "";
                string strCarAvail = "";
                string strPriceAgain = "";
                string strAlternateCur = "";
                string strMessage = "";

                // *****************************************************************
                // Transform OTA Travel Build Request into Native Galileo Request *
                // ***************************************************************** 
                DateTime requestTime = DateTime.Now;
                string strRequest = SetRequest("Galileo_TravelBuildRQ.xsl");

                // **********************
                // Create Session
                // **********************
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);

                #region  Send Transformed Request to the Galileo Adapter and Getting Native Response  

                var oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                var oRoot = oDoc.DocumentElement;
                if (oRoot.SelectSingleNode("CarAvail") != null)
                    strCarAvail = oRoot.SelectSingleNode("CarAvail").InnerXml;

                if (oRoot.SelectSingleNode("PNRBF") == null)
                {
                    throw new Exception("Request is missing mandatory elements.");
                }
                else
                {
                    strMandatory = oRoot.SelectSingleNode("PNRBF").InnerXml;
                    strET = oRoot.SelectSingleNode("ET").InnerXml;
                }

                if (oRoot.SelectSingleNode("Rules") != null)
                {
                    strRules = oRoot.SelectSingleNode("Rules").InnerXml;
                    strETR = oRoot.SelectSingleNode("ETR").InnerXml;
                }

                if (oRoot.SelectSingleNode("PriceAgain") != null)
                {
                    strPriceAgain = oRoot.SelectSingleNode("PriceAgain").InnerXml;
                }

                var iStartPrice = strRequest.IndexOf("<StorePriceMods>").ToString();
                iEndPrice = (strRequest.IndexOf("</StorePriceMods>") + 17).ToString();

                var strPricingReq = Convert.ToDouble(iStartPrice) != -1
                    ? strRequest.Substring(Convert.ToInt32(iStartPrice), (int)Math.Round(Convert.ToDouble(iEndPrice) - Convert.ToDouble(iStartPrice)))
                    : "";

                var oDocReqOTA = new XmlDocument();
                oDocReqOTA.LoadXml(Request);
                var oRootReqOTA = oDocReqOTA.DocumentElement;
                if (oRootReqOTA.SelectSingleNode("POS/Source/@ISOCurrency") != null)
                {
                    strAlternateCur = $":{oRootReqOTA.SelectSingleNode("POS/Source/@ISOCurrency").InnerXml}";
                }

                if (oRootReqOTA.SelectSingleNode("POS/Source/RequestorID/CompanyName/@Code") != null)
                {
                    if (!string.IsNullOrEmpty(oRootReqOTA.SelectSingleNode("POS/Source/RequestorID/CompanyName/@Code").InnerText))
                    {
                        strResponse = ttGA.SendCrypticMessage($"CMT/{oRootReqOTA.SelectSingleNode("POS/Source/RequestorID/CompanyName/@Code").InnerText}//", ConversationID);
                    }
                }

                if (!string.IsNullOrEmpty(strCarAvail))
                {
                    strResponse = ttGA.SendMessage(strCarAvail, ConversationID);
                    // strNative = strMandatory & strResponse

                    strMessage = $"{strCarAvail}\r\n{strResponse}";
                    strError = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_TB_Errors.xsl");
                    if (strError.Contains("<Error"))
                    {
                        strResponse = $"<PNRBFManagement_53>{strError}</PNRBFManagement_53>";
                        strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_PNRReadRS.xsl");
                        return strResponse;
                    }

                    XmlDocument oDocCar = null;
                    XmlElement oRootCar = null;
                    XmlNode oNodeCar = null;
                    oDocCar = new XmlDocument();
                    oDocCar.LoadXml(strResponse);
                    oRootCar = oDocCar.DocumentElement;
                    oNodeCar = oRootCar.SelectSingleNode("CarAvailDetail/DataQual/CarDetailAry/CarDetail[1]/YieldMgmt");
                    if (oNodeCar != null)
                    {
                        if (strMandatory.Contains("</YieldMgmtNum>"))
                        {
                            strMandatory = strMandatory.Replace("</YieldMgmtNum>", $"{oNodeCar.InnerText}</YieldMgmtNum>");
                        }
                        else
                        {
                            strMandatory = strMandatory.Replace("<YieldMgmtNum />", $"<YieldMgmtNum>{oNodeCar.InnerText}</YieldMgmtNum>");
                        }
                    }

                    oNodeCar = oRootCar.SelectSingleNode("CarAvailDetail/DataQual/CarDetailAry/CarDetail[1]/RateDBKey");
                    if (oNodeCar != null)
                    {
                        if (strMandatory.Contains("</RefDBKey>"))
                        {
                            strMandatory = strMandatory.Replace("</RefDBKey>", $"{oNodeCar.InnerText}</RefDBKey>");
                        }
                        else
                        {
                            strMandatory = strMandatory.Replace("<RefDBKey />", $"<RefDBKey>{oNodeCar.InnerText}</RefDBKey>");
                        }
                    }
                }

                strResponse = ttGA.SendMessage(strMandatory, ConversationID);
                strMessage += $"\r\n{strMandatory}\r\n{strResponse}";

                strError = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_TB_Errors.xsl");

                if (strError.Contains("<Error"))
                {
                    strResponse = CoreLib.TransformXML($"<PNRBFManagement_53>{strError}</PNRBFManagement_53>", XslPath, $"{Version}Galileo_PNRReadRS.xsl");
                    return strResponse;
                }

                strDoubleET = "<PNRBFManagement_53><EndTransactionMods><EndTransactRequest><ETInd>R</ETInd><RcvdFrom>TRIPXML</RcvdFrom></EndTransactRequest></EndTransactionMods><FareRedisplayMods></FareRedisplayMods></PNRBFManagement_53>";
                // "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>1</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods>").Append( _
                // "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>2</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods>").Append( _
                // "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>3</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods>").Append( _
                // "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>4</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods>").Append( _
                if (strError.Contains("<Error") & !string.IsNullOrEmpty(strPriceAgain))
                {
                    if (strError.Contains("NO VALID FARE FOR INPUT CRITERIA"))
                    {
                        strResponse = ttGA.SendMessage(strPriceAgain, ConversationID);
                        strMessage += $"\r\n{strPriceAgain}\r\n{strResponse}";
                        strError = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_TB_Errors.xsl");

                        if (strError.Contains("<Error"))
                        {
                            strResponse = CoreLib.TransformXML($"<PNRBFManagement_53>{strError}</PNRBFManagement_53>", XslPath, $"{Version}Galileo_PNRReadRS.xsl");
                            return strResponse;
                        }
                        else
                        {
                            strResponse = ttGA.SendMessage(strDoubleET, ConversationID);
                        }
                    }
                    else
                    {
                        strResponse = CoreLib.TransformXML($"<PNRBFManagement_53>{strError}</PNRBFManagement_53>", XslPath, $"{Version}Galileo_PNRReadRS.xsl");
                        return strResponse;
                    }
                }
                else
                {
                    strResponse = ttGA.SendMessage(strDoubleET, ConversationID);
                }
                #endregion

                var oDocResp = new XmlDocument();
                oDocResp.LoadXml(strResponse);
                var oRootResp = oDocResp.DocumentElement;
                var oNodeResp = oRootResp.SelectSingleNode("PNRBFRetrieve/GenPNRInfo/RecLoc");

                if (oNodeResp == null)
                {
                    oNodeResp = oRootResp.SelectSingleNode("EndTransaction/EndTransactMessage[TypeInd = 'W']");
                    if (oNodeResp != null)
                    {
                        strDoubleET = "<PNRBFManagement_53><EndTransactionMods><EndTransactRequest><ETInd>R</ETInd><RcvdFrom>TRIPXML</RcvdFrom></EndTransactRequest></EndTransactionMods><FareRedisplayMods></FareRedisplayMods></PNRBFManagement_53>";
                        // "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>1</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods>").Append( _
                        // "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>2</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods>").Append( _
                        // "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>3</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods>").Append( _
                        // "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>4</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods>").Append( _

                        strResponse = ttGA.SendMessage(strDoubleET, ConversationID);
                        oDocResp.LoadXml(strResponse);
                        oRootResp = oDocResp.DocumentElement;
                        oNodeResp = oRootResp.SelectSingleNode("EndTransaction/EndTransactMessage[TypeInd = 'W']");
                        if (oNodeResp != null)
                        {
                            strResponse = ttGA.SendMessage(strDoubleET, ConversationID);
                            oDocResp.LoadXml(strResponse);
                            oRootResp = oDocResp.DocumentElement;
                        }

                        oNodeResp = oRootResp.SelectSingleNode("PNRBFRetrieve/GenPNRInfo/RecLoc");
                    }
                    else if (oRootResp.SelectSingleNode("EndTransaction/EndTransactResponse[RecLoc != '']") != null)
                    {
                        strRequest = $"<PNRBFManagement_53><PNRBFRetrieveMods><PNRAddr><FileAddr/><CodeCheck/><RecLoc>{oRootResp.SelectSingleNode("EndTransaction/EndTransactResponse/RecLoc").InnerText}</RecLoc></PNRAddr></PNRBFRetrieveMods><FareRedisplayMods></FareRedisplayMods></PNRBFManagement_53>";
                        // "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>1</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods>").Append( _
                        // "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>2</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods>").Append( _
                        // "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>3</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods>").Append( _
                        // "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>4</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods>").Append( _
                        strResponse = ttGA.SendMessage(strRequest, ConversationID);
                        oDocResp.LoadXml(strResponse);
                        oRootResp = oDocResp.DocumentElement;
                        oNodeResp = oRootResp.SelectSingleNode("PNRBFRetrieve/GenPNRInfo/RecLoc");
                    }
                    else
                    {
                        oNodeResp = oRootResp.SelectSingleNode("CustomCheckRuleExecute");
                        if (oNodeResp != null & string.IsNullOrEmpty(strRules))
                        {
                            strRequest = $"<PNRBFManagement_53>{strPricingReq}<EndTransactionMods><EndTransactRequest><ETInd>R</ETInd><RcvdFrom>TRIPXML</RcvdFrom></EndTransactRequest></EndTransactionMods><FareRedisplayMods></FareRedisplayMods></PNRBFManagement_53>";
                            // "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>1</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods>").Append( _
                            // "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>2</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods>").Append( _
                            // "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>3</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods>").Append( _
                            // "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>4</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods>").Append( _
                            strResponse = ttGA.SendMessage(strRequest, ConversationID);
                            oDocResp.LoadXml(strResponse);
                            oRootResp = oDocResp.DocumentElement;
                        }
                        oNodeResp = oRootResp.SelectSingleNode("PNRBFRetrieve/GenPNRInfo/RecLoc");
                    }
                }

                if (oNodeResp != null)
                {
                    if (!string.IsNullOrEmpty(oNodeResp.InnerText))
                    {
                        XmlNodeList oFareNodes;
                        XmlNode oFareNode;
                        bool bFQ = false;
                        int i = 0;
                        string strFQResp = "";
                        string strSegNum = "";

                        // beacause of simultaneous change issues we ignore the session
                        // then we wait 1 second so hopefully the PNR gets updated by the airline
                        // then we retrieve again to continue the process

                        strResponse = ttGA.SendCrypticMessage("I", ConversationID);
                        Thread.Sleep(1000);

                        strRequest = $"<PNRBFManagement_53><PNRBFRetrieveMods><PNRAddr><FileAddr/><CodeCheck/><RecLoc>{oNodeResp.InnerText}</RecLoc></PNRAddr></PNRBFRetrieveMods><FareRedisplayMods></FareRedisplayMods></PNRBFManagement_53>";
                        // "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>1</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods>").Append( _
                        // "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>2</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods>").Append( _
                        // "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>3</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods>").Append( _
                        // "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>4</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods>").Append( _
                        strResponse = ttGA.SendMessage(strRequest, ConversationID);
                        oDocResp.LoadXml(strResponse);
                        oRootResp = oDocResp.DocumentElement;
                        oNodeResp = oRootResp.SelectSingleNode("PNRBFRetrieve/VndRecLocs/RecLocInfoAry/RecLocInfo/RecLoc");
                        if (oNodeResp == null)
                        {
                            Thread.Sleep(2000);

                            strResponse = ttGA.SendMessage(strRequest, ConversationID);
                            oDocResp.LoadXml(strResponse);
                            oRootResp = oDocResp.DocumentElement;
                            oNodeResp = oRootResp.SelectSingleNode("PNRBFRetrieve/VndRecLocs/RecLocInfoAry/RecLocInfo/RecLoc");
                            if (oNodeResp == null)
                            {
                                Thread.Sleep(3000);

                                strResponse = ttGA.SendMessage(strRequest, ConversationID);
                                oDocResp.LoadXml(strResponse);
                                oRootResp = oDocResp.DocumentElement;
                                oNodeResp = oRootResp.SelectSingleNode("PNRBFRetrieve/VndRecLocs/RecLocInfoAry/RecLocInfo/RecLoc");
                                if (oNodeResp == null)
                                {
                                    Warnings += "<Warning>AIRLINE RECORD LOCATOR NOT IN PNR</Warning>";
                                }
                            }
                        }

                        oNodeResp = oRootResp.SelectSingleNode("PNRBFRetrieve/GenPNRInfo/RecLoc");
                        oFareNodes = oRootResp.SelectNodes("DocProdDisplayStoredQuote[GenQuoteDetails]");
                        foreach (XmlNode currentOFareNode in oFareNodes)
                        {
                            oFareNode = currentOFareNode;
                            if (oFareNode.SelectSingleNode("GenQuoteDetails/QuoteType").InnerText != "G" & oFareNode.SelectSingleNode("GenQuoteDetails/QuoteType").InnerText != "A" & oFareNode.SelectSingleNode("GenQuoteDetails/QuoteType").InnerText != "P" & oFareNode.SelectSingleNode("GenQuoteDetails/QuoteType").InnerText != "C")
                            {
                                bFQ = true;
                                strSegNum = "";
                                string strCorpCode = "";
                                foreach (XmlNode oSegNode in oFareNode.SelectNodes("AssocSegs"))
                                {
                                    if (!string.IsNullOrEmpty(strSegNum))
                                    {
                                        strSegNum += ".";
                                    }

                                    strSegNum = strSegNum + oSegNode.SelectSingleNode("SegNumAry/SegNum").InnerText;
                                }

                                // If oFareNode.SelectSingleNode("GenQuoteDetails/QuoteType").InnerText = "P" _
                                // And Not oRootReqOTA.SelectSingleNode("TPA_Extensions/PriceData[@FlightRefNumberRPHList='" & strSegNum.Replace(".", " ") & "']/NegoFares/PriceRequestInformation/NegotiatedFareCode/@SecondaryCode") Is Nothing Then
                                // strCorpCode = "/C" & oFareNode.SelectSingleNode("PlatingAirVMod/AirV").InnerText & "-" & oRootReqOTA.SelectSingleNode("TPA_Extensions/PriceData[@FlightRefNumberRPHList='" & strSegNum.Replace(".", " ") & "']/NegoFares/PriceRequestInformation/NegotiatedFareCode/@SecondaryCode").InnerText
                                // End If

                                strResponse = ttGA.SendCrypticMessage("FQS" + strSegNum + strCorpCode + strAlternateCur, ConversationID);
                            }
                        }

                        for (i = oFareNodes.Count - 1; i >= 0; i -= 1)
                        {
                            oFareNode = oFareNodes.Item(i);
                            if (oFareNode.SelectSingleNode("GenQuoteDetails/QuoteType").InnerText != "G" & oFareNode.SelectSingleNode("GenQuoteDetails/QuoteType").InnerText != "A" & oFareNode.SelectSingleNode("GenQuoteDetails/QuoteType").InnerText != "P" & oFareNode.SelectSingleNode("GenQuoteDetails/QuoteType").InnerText != "C")
                                strResponse = ttGA.SendCrypticMessage($"FX{oFareNode.SelectSingleNode("FareNumInfo/FareNumAry/FareNum").InnerText}", ConversationID);
                        }

                        if (bFQ)
                        {
                            strResponse = ttGA.SendCrypticMessage("R.TRIPXML", ConversationID);
                            strResponse = ttGA.SendCrypticMessage("ER", ConversationID);
                            if (!(strResponse.Substring(6, 1) == "/" & strResponse.Contains("1.1")))
                                strResponse = ttGA.SendCrypticMessage("ER", ConversationID);

                            strRequest = "<PNRBFManagement_53><PNRBFRetrieveMods><CurrentPNR/></PNRBFRetrieveMods><FareRedisplayMods></FareRedisplayMods></PNRBFManagement_53>";
                            strResponse = ttGA.SendMessage(strRequest, ConversationID);
                        }

                        var oDocReq = new XmlDocument();
                        oDocReq.LoadXml(Request);
                        var oRootReq = oDocReq.DocumentElement;

                        // Get AutoTicketing Flag
                        var oNodeReq = oRootReq.SelectSingleNode("TPA_Extensions/PriceData");
                        if (oNodeReq != null)
                        {
                            if (oNodeReq.Attributes["AutoTicketing"] != null)
                                autoTicketing = string.Compare(oNodeReq.Attributes["AutoTicketing"].Value, "true") == 0;
                        }

                        oNodeReq = oRootReq.SelectSingleNode("TPA_Extensions/PriceData/FareDiscount/BaseFare");
                        if (oNodeReq != null)
                        {
                            #region Send manual price Request to the Galileo Adapter and Getting Native Response *

                            strRequest = $"<TTReprice>{Request}{strResponse}</TTReprice>";
                            strRequest = CoreLib.TransformXML(strRequest, XslPath, $"{Version}Galileo_TravelBuild1RQ.xsl");
                            CoreLib.SendTrace(ProviderSystems.UserID, "ttGalileoService", "OTA Transformed Reprice Request", strRequest, ProviderSystems.LogUUID);


                            if (string.IsNullOrEmpty(strRequest))
                                throw new Exception("Transformation produced empty xml.");

                            #endregion

                            strResponse = ttGA.SendMessage(strRequest, ConversationID);
                            strMessage += $"\r\n{strRequest}\r\n{strResponse}";

                            // *******************************************************************************
                            // if manual price update fails, cancel booked PNR and return error message     *
                            // ******************************************************************************* 
                            if (strResponse.Contains("<TransactionErrorCode>"))
                            {
                                strError = strResponse.Substring(strResponse.IndexOf("<Text>") + 6, strResponse.IndexOf("</Text>") - (strResponse.IndexOf("<Text>") + 6));
                                strRequest = "<PNRBFManagement_53><SegCancelMods><CancelSegAry><CancelSeg><Tok>01</Tok><SegNum>FF</SegNum></CancelSeg></CancelSegAry></SegCancelMods><EndTransactionMods><EndTransactRequest><ETInd>E</ETInd><RcvdFrom>TRIPXML</RcvdFrom></EndTransactRequest></EndTransactionMods></PNRBFManagement_53>";
                                strResponse = ttGA.SendMessage(strRequest, ConversationID);
                                strMessage += $"\r\n{strRequest}\r\n{strResponse}";

                                if (strResponse.Contains("<EndTransactMessage><TypeInd>W</TypeInd>"))
                                {
                                    strRequest = "<PNRBFManagement_53><EndTransactionMods><EndTransactRequest><ETInd>E</ETInd><RcvdFrom>TRIPXML</RcvdFrom></EndTransactRequest></EndTransactionMods></PNRBFManagement_53>";
                                    strResponse = ttGA.SendMessage(strRequest, ConversationID);
                                    strMessage += $"\r\n{strRequest}\r\n{strResponse}";
                                }

                                throw new Exception($"PNR {strRecloc} CREATED AND CANCELLED - {strError}");
                            }

                            strRequest = $"<PNRBFManagement_53><EndTransactionMods><EndTransactRequest><ETInd>R</ETInd><RcvdFrom>RI</RcvdFrom></EndTransactRequest></EndTransactionMods><FareRedisplayMods></FareRedisplayMods></PNRBFManagement_53>";

                            // *******************************************************************************
                            // Send end pnr Request to the Galileo Adapter and Getting Native Response      *
                            // ******************************************************************************* 
                            strMessage += $"\r\n{strRequest}\r\n{strResponse}";
                        }
                    }
                }

                if (autoTicketing)
                {
                    var strTicketRequest = $"<DocProdFareManipulation_4_0><TicketingMods><ElectronicTicketFailed><CancelInd>Y</CancelInd><IssuePaperTkInd/><IssuePaperTkToSTP/><STPlocation/></ElectronicTicketFailed>" +
                                           "<FareNumInfo><FareNumAry><FareNum>1</FareNum></FareNumAry></FareNumInfo><TicketingControl><TransType>TK</TransType></TicketingControl></TicketingMods></DocProdFareManipulation_4_0>";

                    var strTicketResponse = ttGA.SendMessage(strTicketRequest, ConversationID);
                    strMessage += $"\r\n{strMessage}\r\n{strTicketRequest}\r\n{strTicketResponse}";

                    if (strTicketResponse.Contains("SIMULTANEOUS CHANGES/IGNORE") | strTicketResponse.Contains("SIMULT CHGS TO PNR"))
                    {
                        strRequest = "<PNRBFManagement_53><IgnoreMods/></PNRBFManagement_53>";
                        var strResp = ttGA.SendMessage(strRequest, ConversationID);
                        strMessage += $"\r\n{strMessage}\r\n{strRequest}\r\n{strResp}";

                        strRequest = $"<PNRBFManagement_53><PNRBFRetrieveMods><PNRAddr><FileAddr/><CodeCheck/><RecLoc>{strRecloc}</RecLoc></PNRAddr></PNRBFRetrieveMods>" +
                        "<FareRedisplayMods></FareRedisplayMods>" +
                        "</PNRBFManagement_53>";
                        strResp = ttGA.SendMessage(strRequest, ConversationID);

                        strMessage += $"\r\n{strMessage}\r\n{strRequest}\r\n{strResp}";
                        strTicketResponse = ttGA.SendMessage(strTicketRequest, ConversationID);
                        strMessage += $"\r\n{strMessage}\r\n{strTicketRequest}\r\n{strTicketResponse}";
                    }

                    strResponse = strResponse.Insert(strResponse.IndexOf("</PNRBFManagement_53>"), strTicketResponse.Replace(" xmlns=\"\"", ""));
                }

                if (!string.IsNullOrEmpty(strRules))
                {
                    strResponse = ttGA.SendMessage(strRules, ConversationID);

                    strMessage += $"\r\n{strMessage}\r\n{strRules}\r\n{strResponse}";
                    if (!string.IsNullOrEmpty(strDoubleET))
                    {
                        strResponse = ttGA.SendMessage(strDoubleET, ConversationID);
                        strMessage += $"\r\n{strMessage}\r\n{strDoubleET}\r\n{strResponse}";
                        strDoubleET = strDoubleET.Replace("<ETInd>R</ETInd>", "<ETInd>E</ETInd>");
                        strResponse = ttGA.SendMessage(strDoubleET, ConversationID);

                        strMessage += $"\r\n{strMessage}\r\n{strDoubleET}\r\n{strResponse}";
                        oDocResp.LoadXml(strResponse);
                        oRootResp = oDocResp.DocumentElement;
                        oNodeResp = oRootResp.SelectSingleNode("EndTransaction/EndTransactResponse/RecLoc");
                        if (oNodeResp != null)
                        {
                            if (!string.IsNullOrEmpty(oNodeResp.InnerText))
                            {
                                strRecloc = oNodeResp.InnerText;
                                if (strETR.Contains("<ETInd>Q</ETInd>"))
                                {
                                    strRequest = $"<PNRBFManagement_53><PNRBFRetrieveMods><PNRAddr><FileAddr/><CodeCheck/><RecLoc>{strRecloc}</RecLoc></PNRAddr></PNRBFRetrieveMods></PNRBFManagement_53>";
                                    strResponse = ttGA.SendMessage(strRequest, ConversationID);
                                    strMessage += $"\r\n{strRequest}\r\n{strResponse}";
                                    strResponse = ttGA.SendMessage(strETR, ConversationID);
                                    strMessage += $"\r\n{strMessage}\r\n{strETR}\r\n{strResponse}";
                                }
                                // *********************************************************
                                // BELOW GIVEN CODE BLOCK IS NOT THERE IN TEH LOCAL CODE
                                // ---------------------------------------------------------
                                strRequest = $"<PNRBFManagement_53><PNRBFRetrieveMods><PNRAddr><FileAddr/><CodeCheck/><RecLoc>{strRecloc}</RecLoc></PNRAddr></PNRBFRetrieveMods>" +
                                "<FareRedisplayMods></FareRedisplayMods></PNRBFManagement_53>";
                                strResponse = ttGA.SendMessage(strRequest, ConversationID);

                                strMessage += $"\r\n{strRequest}\r\n{strResponse}";
                            }
                        }
                        else if (strResponse.Contains("RULE - END WARNING"))
                        {
                            strResponse = ttGA.SendMessage(strDoubleET, ConversationID);
                            strMessage += $"\r\n{strMessage}\r\n{strDoubleET}\r\n{strResponse}";
                            oDocResp.LoadXml(strResponse);
                            oRootResp = oDocResp.DocumentElement;
                            oNodeResp = oRootResp.SelectSingleNode("EndTransaction/EndTransactResponse/RecLoc");
                            if (oNodeResp != null & !string.IsNullOrEmpty(oNodeResp.InnerText))
                            {
                                strRecloc = oNodeResp.InnerText;
                                // ----------------------------------------------------
                                // ****************************************************
                                strRequest = $"<PNRBFManagement_53><PNRBFRetrieveMods><PNRAddr><FileAddr/><CodeCheck/><RecLoc>{strRecloc}</RecLoc></PNRAddr></PNRBFRetrieveMods>" +
                                "<FareRedisplayMods></FareRedisplayMods></PNRBFManagement_53>";
                                strResponse = ttGA.SendMessage(strRequest, ConversationID);
                                strMessage += $"\r\n{strRequest}\r\n{strResponse}";
                            }
                        }
                    }
                    else
                    {
                        strET = strET.Replace("<EndTransactionMods>", $"{strPricingReq}<EndTransactionMods>");
                        strResponse = ttGA.SendMessage(strET, ConversationID);
                        strMessage += $"\r\n{strMessage}\r\n{strET}\r\n{strResponse}";
                        oDocResp.LoadXml(strResponse);
                        oRootResp = oDocResp.DocumentElement;
                        oNodeResp = oRootResp.SelectSingleNode("EndTransaction/EndTransactResponse/RecLoc");
                        if (oNodeResp != null)
                        {
                            if (!string.IsNullOrEmpty(oNodeResp.InnerText) & !strET.Contains("<ETInd>R</ETInd>"))
                            {
                                strRecloc = oNodeResp.InnerText;
                                strRequest = $"<PNRBFManagement_53><PNRBFRetrieveMods><PNRAddr><FileAddr/><CodeCheck/><RecLoc>{strRecloc}</RecLoc></PNRAddr></PNRBFRetrieveMods>" +
                                "<FareRedisplayMods></FareRedisplayMods></PNRBFManagement_53>";
                                strResponse = ttGA.SendMessage(strRequest, ConversationID);

                                strMessage += $"\r\n{strRequest}\r\n{strResponse}";
                            }
                        }
                    }
                }

                if (oNodeResp != null & strET.Contains("<ETInd>Q</ETInd>"))
                {
                    string strResponse1 = ttGA.SendMessage(strET, ConversationID);
                }

                // *****************************************************************
                // Transform Native Galileo PNRRead Response into OTA Response     *
                // ***************************************************************** 
                try
                {
                    strResponse = strResponse.Replace("</PNRBFManagement_53>", $"{Warnings}</PNRBFManagement_53>");
                    strResponse = inSession
                        ? strResponse.Replace("</PNRBFManagement_53>", $"<ConversationID>{ConversationID}</ConversationID></PNRBFManagement_53>")
                        : strResponse;

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_PNRReadRS.xsl");

                    if (ProviderSystems.LogNative)
                    {
                        TripXMLTools.TripXMLLog.LogMessage("TravelBuild", ref strMessage, requestTime, DateTime.Now, "Native", ProviderSystems.Provider, ProviderSystems.System, ProviderSystems.UserName);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttGA.CloseSession(ConversationID);
                        ConversationID = null;
                        ttGA = null;
                    }
                }
            }
            catch (Exception exx)
            {
                AddLog($"<M>{Request}<BL/>", ProviderSystems.UserID);
                Errors = exx.Message;
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.TravelBuild, exx.Message, ProviderSystems, "");
            }
            finally
            {
                GC.Collect();
            }

            return strResponse;
        }

        public string TravelModify()
        {

            string strResponse = "";

            try
            {
                string strNative = "";
                DateTime RequestTime = DateTime.Now;
                string strRequest = SetRequest("Galileo_TravelModifyRQ.xsl");

                // **********************
                // Create Session
                // **********************
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // ************************************************
                // check if request is to add or modify elements *
                // ************************************************ 
                var oDocReqN = new XmlDocument();
                oDocReqN.LoadXml(strRequest);
                var oRootReqN = oDocReqN.DocumentElement;

                if (oRootReqN.SelectSingleNode("Modify") == null)
                {
                    bool AutoTicketing = false;
                    // *******************************************************************************
                    // this is an add to existing booking                                           *
                    // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                    // *******************************************************************************                     
                    strResponse = ttGA.SendMessage(strRequest, ConversationID);
                    strNative += $"{strRequest}\r\n{strResponse}";

                    var oDocResp = new XmlDocument();
                    oDocResp.LoadXml(strResponse);
                    var oRootResp = oDocResp.DocumentElement;
                    var oNodeResp = oRootResp.SelectSingleNode("PNRBFRetrieve/GenPNRInfo/FileAddr");
                    if (oRootResp.SelectSingleNode("EndTransaction/EndTransactMessage[TypeInd = 'W']") != null)
                    {
                        var iStartPrice = strRequest.IndexOf("<StorePriceMods>").ToString();
                        var iEndPrice = (strRequest.IndexOf("</StorePriceMods>") + 17).ToString();

                        var strPricingReq = Convert.ToDouble(iStartPrice) != -1
                                ? strRequest.Substring(Convert.ToInt32(iStartPrice), (int)Math.Round(Convert.ToDouble(iEndPrice) - Convert.ToDouble(iStartPrice)))
                                : "";

                        strRequest = $"<PNRBFManagement_53>{strPricingReq}<EndTransactionMods><EndTransactRequest><ETInd>R</ETInd><RcvdFrom>TRIPXML</RcvdFrom></EndTransactRequest></EndTransactionMods><FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>1</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods></PNRBFManagement_53>";
                        strResponse = ttGA.SendMessage(strRequest, ConversationID);
                        oDocResp.LoadXml(strResponse);
                        oRootResp = oDocResp.DocumentElement;
                        oNodeResp = oRootResp.SelectSingleNode("PNRBFRetrieve/GenPNRInfo/FileAddr");
                    }
                    var strRecloc = oRootResp.SelectSingleNode("PNRBFRetrieve/GenPNRInfo/RecLoc").InnerText;

                    if (oNodeResp != null)
                    {
                        if (!string.IsNullOrEmpty(oNodeResp.InnerText))
                        {
                            var oDocReq = new XmlDocument();
                            oDocReq.LoadXml(Request);
                            var oRootReq = oDocReq.DocumentElement;

                            // Get AutoTicketing Flag
                            var oNodeReq = oRootReq.SelectSingleNode("TPA_Extensions/PriceData");

                            if (oNodeReq != null)
                            {
                                if (oNodeReq.Attributes["AutoTicketing"] != null)
                                {
                                    AutoTicketing = string.Compare(oNodeReq.Attributes["AutoTicketing"].Value, "true") == 0;
                                }
                            }

                            oNodeReq = oRootReq.SelectSingleNode("TPA_Extensions/PriceData/FareDiscount/BaseFare");

                            if (oNodeReq != null)
                            {

                                // *******************************************************************************
                                // Send manual price Request to the Galileo Adapter and Getting Native Response *
                                // ******************************************************************************* 

                                strRequest = $"<TTReprice>{Request}{strResponse}</TTReprice>";
                                strRequest = CoreLib.TransformXML(strRequest, XslPath, $"{Version}Galileo_TravelModify1RQ.xsl");
                                CoreLib.SendTrace(ProviderSystems.UserID, "ttGalileoService", "OTA Transformed Reprice Request", strRequest, ProviderSystems.LogUUID);

                                if (string.IsNullOrEmpty(strRequest))
                                    throw new Exception("Transformation produced empty xml.");

                                strResponse = ttGA.SendMessage(strRequest, ConversationID);
                                strNative += $"{strRequest}\r\n{strResponse}";

                                // *******************************************************************************
                                // if manual price update fails, cancel booked PNR and return error message     *
                                // ******************************************************************************* 
                                if (strResponse.Contains("<TransactionErrorCode>"))
                                {
                                    var strError = strResponse.Substring(strResponse.IndexOf("<Text>") + 6, strResponse.IndexOf("</Text>") - (strResponse.IndexOf("<Text>") + 6));
                                    strRequest = "<PNRBFManagement_53><SegCancelMods><CancelSegAry><CancelSeg><Tok>01</Tok><SegNum>FF</SegNum></CancelSeg></CancelSegAry></SegCancelMods><EndTransactionMods><EndTransactRequest><ETInd>E</ETInd><RcvdFrom>TRIPXML</RcvdFrom></EndTransactRequest></EndTransactionMods></PNRBFManagement_53>";
                                    // <PNRBFRetrieveMods><PNRAddr><FileAddr /><CodeCheck /><RecLoc>").Append(strRecloc).Append("</RecLoc></PNRAddr></PNRBFRetrieveMods>").Append(_
                                    strResponse = ttGA.SendMessage(strRequest, ConversationID);
                                    strNative += $"{strRequest}\r\n{strResponse}";

                                    if (strResponse.Contains("<EndTransactMessage><TypeInd>W</TypeInd>"))
                                    {
                                        strRequest = "<PNRBFManagement_53><EndTransactionMods><EndTransactRequest><ETInd>E</ETInd><RcvdFrom>TRIPXML</RcvdFrom></EndTransactRequest></EndTransactionMods></PNRBFManagement_53>";
                                        strResponse = ttGA.SendMessage(strRequest, ConversationID);
                                        strNative += $"{strRequest}\r\n{strResponse}";
                                    }

                                    throw new Exception($"PNR {strRecloc} CREATED AND CANCELLED - {strError}");
                                }

                                strRequest = "<PNRBFManagement_53><EndTransactionMods><EndTransactRequest><ETInd>R</ETInd><RcvdFrom>RI</RcvdFrom></EndTransactRequest></EndTransactionMods><FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>1</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods></PNRBFManagement_53>";

                                // *******************************************************************************
                                // Send end pnr Request to the Galileo Adapter and Getting Native Response      *
                                // ******************************************************************************* 
                                strResponse = ttGA.SendMessage(strRequest, ConversationID);
                                strNative += $"{strRequest}\r\n{strResponse}";

                            }
                        }
                    }

                    if (AutoTicketing)
                    {
                        var strTicketRequest = "<DocProdFareManipulation_4_0><TicketingMods><ElectronicTicketFailed><CancelInd>Y</CancelInd><IssuePaperTkInd/><IssuePaperTkToSTP/><STPlocation/></ElectronicTicketFailed>" +
                        "<FareNumInfo><FareNumAry><FareNum>1</FareNum></FareNumAry></FareNumInfo><TicketingControl><TransType>TK</TransType></TicketingControl></TicketingMods></DocProdFareManipulation_4_0>";


                        var strTicketResponse = ttGA.SendMessage(strTicketRequest, ConversationID);
                        strNative += $"{strRequest}\r\n{strResponse}";

                        if (strTicketResponse.Contains("SIMULTANEOUS CHANGES/IGNORE") | strTicketResponse.Contains("SIMULT CHGS TO PNR"))
                        {
                            strRequest = "<PNRBFManagement_53><IgnoreMods/></PNRBFManagement_53>";
                            var strResp = ttGA.SendMessage(strRequest, ConversationID);
                            strNative += $"{strRequest}\r\n{strResp}";

                            strRequest = $"<PNRBFManagement_53><PNRBFRetrieveMods><PNRAddr><FileAddr/><CodeCheck/><RecLoc>{strRecloc}</RecLoc></PNRAddr></PNRBFRetrieveMods>";
                            strRequest += "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>1</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods></PNRBFManagement_53>";
                            strResp = ttGA.SendMessage(strRequest, ConversationID);
                            strNative += $"{strRequest}\r\n{strResp}";

                            strTicketResponse = ttGA.SendMessage(strTicketRequest, ConversationID);
                            strNative += $"{strTicketRequest}\r\n{strTicketResponse}";
                        }

                        strResponse = strResponse.Insert(strResponse.IndexOf("</PNRBFManagement_53>"), strTicketResponse.Replace(" xmlns=\"\"", ""));
                    }
                }
                else
                {
                    // *******************************************************************************
                    // this is a modify of existing booking                                         *
                    // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                    // ******************************************************************************* 
                    var oNodeReqN = oRootReqN.SelectSingleNode("Retrieve");
                    strResponse = ttGA.SendMessage(oNodeReqN.InnerXml, ConversationID);
                    strNative += $"{oNodeReqN.InnerXml}\r\n{strResponse}";

                    var oDocResp = new XmlDocument();
                    oDocResp.LoadXml(strResponse);
                    var oRootResp = oDocResp.DocumentElement;
                    var strRecloc = oRootResp.SelectSingleNode("PNRBFRetrieve/GenPNRInfo/RecLoc").InnerText;
                    var oNodeResp = oRootResp.SelectSingleNode("PNRBFRetrieve/GenPNRInfo/FileAddr");

                    if (oNodeResp == null)
                        throw new Exception($"PNR {strRecloc} does not exist");

                    oNodeReqN = oRootReqN.SelectSingleNode("Modify");
                    strResponse = ttGA.SendMessage(oNodeReqN.InnerXml, ConversationID);
                    strNative = $"{oNodeReqN.InnerXml}\r\n{strResponse}";

                    oNodeReqN = oRootReqN.SelectSingleNode("ET");
                    strResponse = ttGA.SendMessage(oNodeReqN.InnerXml, ConversationID);
                    strNative = $"{oNodeReqN.InnerXml}\r\n{strResponse}";

                    oNodeReqN = oRootReqN.SelectSingleNode("VerifyATFQ");
                    strResponse = ttGA.SendMessage(oNodeReqN.InnerXml, ConversationID);
                    strNative = $"{oNodeReqN.InnerXml}\r\n{strResponse}";

                    oNodeReqN = oRootReqN.SelectSingleNode("ET");
                    strResponse = ttGA.SendMessage(oNodeReqN.InnerXml, ConversationID);
                    strNative = $"{oNodeReqN.InnerXml}\r\n{strResponse}";
                }

                // *****************************************************************
                // Transform Native Galileo PNRRead Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    strResponse = inSession
                        ? strResponse.Replace("</PNRBFManagement_53>", $"<ConversationID>{ConversationID}</ConversationID></PNRBFManagement_53>")
                        : strResponse;

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_PNRReadRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttGA.CloseSession(ConversationID);
                        ConversationID = null;
                        ttGA = null;
                    }
                }
            }
            catch (Exception exx)
            {
                AddLog($"<M>{Request}<BL/>", ProviderSystems.UserID);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.TravelModify, exx.Message, ProviderSystems, "");
            }
            finally
            {
                GC.Collect();
            }

            return strResponse;
        }

        public string IssueTicket()
        {
            string strResponse = "";

            try
            {

                DateTime RequestTime = DateTime.Now;
                string strRequest = SetRequest("Galileo_IssueTicketRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                string strRead;
                // *************************
                // Get Multiple Requests  *
                // *************************
                string strEmail = "";
                string strVerifyATFQ = "";
                string strExistingBF = "";
                string strNewBF = "";
                string strAlternateCur = "";
                string strFOP = "";
                string strTicket = "";
                string strCheckPrt = "";
                string strChangeTkt = "";
                string strET = "";
                string strCrypticRULA = "";
                string strGetTickets = "";
                string strMessage = "";
                XmlElement oRoot;
                try
                {
                    var oDoc = new XmlDocument();
                    oDoc.LoadXml(strRequest);
                    oRoot = oDoc.DocumentElement;
                    strRead = oRoot.SelectSingleNode("PNRRead").InnerXml;
                                        
                    strTicket = oRoot.SelectSingleNode("Ticket").InnerXml;
                    strVerifyATFQ = oRoot.SelectSingleNode("VerifyATFQ") != null ? oRoot.SelectSingleNode("VerifyATFQ").InnerXml : "";
                    strET = oRoot.SelectSingleNode("ET") != null ? oRoot.SelectSingleNode("ET").InnerXml : "";
                    strCrypticRULA = oRoot.SelectSingleNode("CrypticRULA") != null ? oRoot.SelectSingleNode("CrypticRULA").InnerXml : "";
                    strGetTickets = oRoot.SelectSingleNode("GetTickets") != null ? oRoot.SelectSingleNode("GetTickets").InnerXml : "";

                    var oDocReqOTA = new XmlDocument();
                    oDocReqOTA.LoadXml(Request);
                    var oRootReqOTA = oDocReqOTA.DocumentElement;
                    // **********************************************************
                    // below given 'if else' block is not  there in the locl code
                    // ----------------------------------------------------------
                    if (oRootReqOTA.SelectSingleNode("Fulfillment/PaymentDetails/PaymentDetail/PaymentCard") != null)
                    {
                        oRootReqOTA.SelectSingleNode("Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardNumber").InnerText = "****************";
                        Request = oDocReqOTA.OuterXml;
                    }

                    strEmail += Request;

                    // -----------------------------------------------------------

                    if (oRootReqOTA.SelectSingleNode("POS/Source/@ISOCurrency") != null)
                    {
                        strAlternateCur = $":{oRootReqOTA.SelectSingleNode("POS/Source/@ISOCurrency").InnerXml}";
                    }
                }
                catch (Exception ex)
                {
                    // this catch was empty in local code
                    strEmail += ex.Message;
                    // -----------------------------------------------------------
                    throw new Exception($"Error Loading Transformed Request XML Document. {ex.Message}");

                }

                // **********************
                // Create Session      *
                // **********************
                var ttGA = SetAdapter();
                bool inSession = false;
                try
                {
                    inSession = SetConversationID(ttGA);
                }
                catch (Exception ex)
                {
                    strEmail += ex.Message;
                    throw ex;
                }

                // ********************
                // Retrieve the PNR  *
                // ******************** 
                try
                {
                    strResponse = ttGA.SendMessage(strRead, ConversationID);
                    strEmail += $"{strRead}\r\n{strResponse}";

                    strMessage = $"{strRead}\r\n{strResponse}";
                }
                catch (Exception ex)
                {
                    // this catch was empty in local code
                    strEmail += ex.Message;
                    throw ex;
                }

                // Check for Errors
                if (strResponse.Contains("<PNRBFRetrieve><ErrorCode>"))
                    throw new Exception("Cannot retrieve PNR to ticket");

                // Check if stored fares exist
                if (strResponse.Contains("<DocProdDisplayStoredQuote />") | strResponse.Contains("<Text>NO FARES</Text>"))
                    throw new Exception("Cannot issue ticket - no stored fares in PNR");

                // get existing fare
                var oDocResp = new XmlDocument();
                oDocResp.LoadXml(strResponse);
                var oRootResp = oDocResp.DocumentElement;
                XmlNodeList oFareNodes;
                XmlNode oFareNode;
                XmlNode oSegNode;
                bool bFQ = false;
                int i = 0;
                string strFQResp = "";
                string strSegNum = "";
                int iPrevNum = 0;
                int iCurNum = 0;
                int iRangeNum = 0;

                foreach (XmlNode currentONode in oRoot.SelectNodes("CrypticTMU"))
                {
                    var oNode = currentONode;
                    strResponse = ttGA.SendCrypticMessage(oNode.InnerText, ConversationID);

                    // below given code block was not there in the local code
                    string strCCInfo = $"{oNode.InnerText.Substring(0, 5)}****************{oNode.InnerText.Substring(oNode.InnerText.IndexOf("*D"))}";
                    strEmail += $"{strCCInfo}\r\n{strResponse}";
                    // ------------------------------------------------------

                    if (strResponse.StartsWith("ERROR") & !strResponse.Contains("MODIFIER ALREADY EXISTS"))
                        throw new Exception(strResponse.Replace("&gt;&lt;", "").Replace("><", ""));
                }

                if (!string.IsNullOrEmpty(strCrypticRULA))
                {
                    strResponse = ttGA.SendCrypticMessage(strCrypticRULA, ConversationID);
                    strEmail += $"{strCrypticRULA}\r\n{strResponse}";
                }

                if (!string.IsNullOrEmpty(strET))
                {
                    strResponse = ttGA.SendMessage(strET, ConversationID);
                    strEmail += $"{strET}\r\n{strResponse}";
                    strMessage += $"\r\n{strET}\r\n{strResponse}";
                }

                foreach (XmlNode currentONode1 in oRoot.SelectNodes("CrypticFF"))
                {
                    var oNode = currentONode1;
                    strResponse = ttGA.SendCrypticMessage(oNode.InnerText, ConversationID);
                    strEmail += $"{oNode.InnerText}\r\n{strResponse}";
                }


                try
                {
                    strResponse = ttGA.SendMessage(strTicket, ConversationID);

                    // send cryptic issue ticket command and format response screen
                    // strResponse = ttGA.SendCrypticMessage(strTicket, Token)
                    // strResponse = strResponse.Replace("&gt;", "")
                    // strResponse = strResponse.Replace("&lt;", "")
                    // strResponse = strResponse.Replace(Chr(13), sb.Append(Chr(13)).Append(Chr(10)).ToString())
                    // sb.Remove(0, sb.Length())
                    // strResponse = strResponse.Replace(Chr(10), sb.Append(Chr(13)).Append(Chr(10)).ToString())
                    // sb.Remove(0, sb.Length())
                    // strResponse = formatGalileo(strResponse)

                    strEmail += $"{strTicket}\r\n{strResponse}";
                    strMessage += $"\r\n{strTicket}\r\n{strResponse}";

                    if (!string.IsNullOrEmpty(strGetTickets) & !strResponse.Contains("TransactionErrorCode") & strResponse.Contains("<TicketingControl><TransType>OK</TransType></TicketingControl>"))
                    {
                        string strResponse1 = ttGA.SendMessage(strRead, ConversationID);
                        strMessage += $"\r\n{strRead}\r\n{strResponse1}";

                        strResponse = ttGA.SendMessage(strGetTickets, ConversationID);
                        strResponse = strResponse.Replace("</DocProdFareManipulation_10>", $"{strResponse1}</DocProdFareManipulation_10>");
                        strEmail += $"{strGetTickets}\r\n{strResponse}";
                        strMessage += $"\r\n{strGetTickets}\r\n{strResponse1}";
                    }
                }
                catch (Exception ex)
                {
                    strEmail += ex.Message;
                    throw ex;
                }

                // *****************************************************************
                // Transform Native Galileo IssueTicket Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    strResponse = inSession
                       ? strResponse.Replace("</PNRBFManagement_53>", $"<ConversationID>{ConversationID}</ConversationID></PNRBFManagement_53>")
                       : strResponse;

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_IssueTicketRS.xsl");

                    strEmail += strResponse;


                    if (ProviderSystems.LogNative)
                        TripXMLTools.TripXMLLog.LogMessage("IssueTicket", ref strMessage, RequestTime, DateTime.Now, "Native", ProviderSystems.Provider, ProviderSystems.System, ProviderSystems.UserName);

                    // ****************************************************
                    // below given if condition was not there in local code
                    // ----------------------------------------------------
                    if (Request.Contains("<System>Production</System>") & !strResponse.Contains("<Success"))
                    {
                        string argMessage = $"<Ticket>{strEmail}</Ticket>";
                        LogMessageToFile("TravelBuild", ref argMessage, DateTime.Now, DateTime.Now);
                    }
                    // ----------------------------------------------------
                }
                catch (Exception ex)
                {
                    strEmail += ex.Message;
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttGA.CloseSession(ConversationID);
                        ConversationID = null;
                        ttGA = null;
                    }
                }

            }
            catch (Exception exx)
            {
                AddLog($"<M>{Request}<BL/>", ProviderSystems.UserID);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.IssueTicket, exx.Message, ProviderSystems, "");
            }
            finally
            {
                GC.Collect();
            }

            return strResponse;
        }

        public string IssueTicketSessioned()
        {
            string strResponse = "";
            Version = string.IsNullOrEmpty(Version) ? "v03_" : Version;
            string strEmail = string.Empty;

            try
            {
                string strRequest = SetRequest("Galileo_IssueTicketRQ.xsl");
                DateTime RequestTime = DateTime.Now;
                string strReSetPrt;
                var strReplacementTag = "</PNRBFManagement_53>";

                // *************************
                // Get Multiple Requests  *
                // *************************
                var oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                XmlElement oRoot = oDoc.DocumentElement;
                string strRead = oRoot.SelectSingleNode("PNRRead").InnerXml;
                string strCurrentRead = oRoot.SelectSingleNode("PNRCurrentRead").InnerXml;
                string strTicket = oRoot.SelectSingleNode("Ticket").InnerXml;
                string strVerifyATFQ = oRoot.SelectSingleNode("VerifyATFQ") != null ? oRoot.SelectSingleNode("VerifyATFQ").InnerXml : "";
                string strET = oRoot.SelectSingleNode("ET") != null ? oRoot.SelectSingleNode("ET").InnerXml : "";
                string strCrypticRULA = oRoot.SelectSingleNode("CrypticRULA") != null ? oRoot.SelectSingleNode("CrypticRULA").InnerXml : "";
                string strGetTickets = oRoot.SelectSingleNode("GetTickets") != null ? oRoot.SelectSingleNode("GetTickets").InnerXml : "";

                var oDocReqOTA = new XmlDocument();
                oDocReqOTA.LoadXml(Request);
                var oRootReqOTA = oDocReqOTA.DocumentElement;
                // **********************************************************
                // below given 'if else' block is not  there in the locl code
                // ----------------------------------------------------------
                if (oRootReqOTA.SelectSingleNode("Fulfillment/PaymentDetails/PaymentDetail/PaymentCard") != null)
                {
                    oRootReqOTA.SelectSingleNode("Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardNumber").InnerText = "****************";
                    Request = oDocReqOTA.OuterXml;
                }

                strEmail += Request;

                /******  Why would you need this?  *****************
                if (oRootReqOTA.SelectSingleNode("POS/Source/@ISOCurrency") != null)
                {
                    string strAlternateCur = $":{oRootReqOTA.SelectSingleNode("POS / Source / @ISOCurrency").InnerXml}";
                }
                ****************************************************/

                string strCheckPrt = oRoot.SelectSingleNode("CheckPRT") != null ? oRoot.SelectSingleNode("CheckPRT").InnerXml : "";
                // TODO: following for possible future use
                string strReLinkPrt = oRoot.SelectSingleNode("SetPRT") != null ? oRoot.SelectSingleNode("SetPRT").InnerXml : "";

                // **********************
                // Create Session      *
                // **********************
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                string strMessage;

                // *****************************************
                // Check if printer is up and running     *
                // ***************************************** 
                if (!string.IsNullOrEmpty(strCheckPrt))
                {
                    oDoc = new XmlDocument();
                    strResponse = ttGA.SendMessage(strCheckPrt, ConversationID);

                    oDoc.LoadXml(strResponse);
                    oRoot = oDoc.DocumentElement;
                    var oNd = oRoot.SelectSingleNode("LinkageDisplay/PrinterParameters[Type='T']");
                    if (oNd == null)
                    {
                        strResponse = ReLinkPrinters(strReLinkPrt, ref ttGA, ConversationID);
                        if (strResponse.Contains("Error"))
                        {
                            throw new Exception("No ticket printer linked");
                        }
                    }
                    else
                    {
                        switch (oNd.SelectSingleNode("Status").InnerText ?? "")
                        {
                            case "D":
                                {
                                    throw new Exception($"Printer {oNd.SelectSingleNode("LNIATA").InnerText} is Down");
                                    break;
                                }

                            case "B":
                                {
                                    // Printer can be reset with status U: HMOM{oNd.SelectSingleNode("LNIATA").InnerText}–U(HMOMF82303–U)
                                    strResponse = ReLinkPrinters(strReLinkPrt, ref ttGA, ConversationID);
                                    if (strResponse.Contains("Error"))
                                    {
                                        throw new Exception($"Printer {oNd.SelectSingleNode("LNIATA").InnerText} is Busy");
                                    }

                                    break;
                                }
                        }
                    }
                }
                else
                {
                    // ********************
                    // Retrieve the PNR  *
                    // ********************                     
                    strResponse = ttGA.SendMessage(strCurrentRead, ConversationID);
                    strEmail += $"{strCurrentRead}\r\n{strResponse}";
                    strMessage = $"{strCurrentRead}\r\n{strResponse}";

                    // Check for Errors
                    if (strResponse.Contains("<PNRBFRetrieve><ErrorCode>") && !strResponse.Contains("<DocProdDisplayStoredQuote><FareNumInfo>"))
                        throw new Exception("Cannot retrieve PNR to ticket");

                    // Check if stored fares exist
                    if (strResponse.Contains("<DocProdDisplayStoredQuote />") | strResponse.Contains("<Text>NO FARES</Text>"))
                        throw new Exception("Cannot issue ticket - no stored fares in PNR");

                    #region Get existing fare
                    var oDocResp = new XmlDocument();
                    oDocResp.LoadXml(strResponse);
                    var oRootResp = oDocResp.DocumentElement;
                    XmlNodeList oFareNodes;
                    XmlNode oFareNode;
                    XmlNode oSegNode;
                    bool bFQ = false;
                    int i = 0;
                    string strFQResp = "";
                    string strSegNum = "";
                    int iPrevNum = 0;
                    int iCurNum = 0;
                    int iRangeNum = 0;

                    if (!string.IsNullOrEmpty(strCrypticRULA))
                    {
                        strResponse = ttGA.SendCrypticMessage(strCrypticRULA, ConversationID);
                        strEmail += $"{strCrypticRULA}\r\n{strResponse}";
                    }

                    if (!string.IsNullOrEmpty(strET))
                    {
                        strResponse = ttGA.SendMessage(strET, ConversationID);
                        strEmail += $"{strET}\r\n{strResponse}";
                        strMessage += $"{strET}\r\n{strResponse}";
                    }


                    strResponse = ttGA.SendMessage(strTicket, ConversationID);
                    // send cryptic issue ticket command and format response screen
                    // strResponse = ttGA.SendCrypticMessage(strTicket, Token)
                    // strResponse = strResponse.Replace("&gt;", "")
                    // strResponse = strResponse.Replace("&lt;", "")
                    // strResponse = strResponse.Replace(Chr(13), sb.Append(Chr(13)).Append(Chr(10)).ToString())
                    // sb.Remove(0, sb.Length())
                    // strResponse = strResponse.Replace(Chr(10), sb.Append(Chr(13)).Append(Chr(10)).ToString())
                    // sb.Remove(0, sb.Length())
                    // strResponse = formatGalileo(strResponse)

                    if (strResponse.Contains("<ErrText><Err>"))
                    {
                        oDocResp.LoadXml(strResponse);
                        oRootResp = oDocResp.DocumentElement;
                        var oErrorNodes = oRootResp.SelectSingleNode("Ticketing/ErrText/Text");
                        throw new Exception(oErrorNodes.InnerText);
                    }

                    strEmail += $"{strTicket}\r\n{strResponse}";
                    strMessage += $"{strTicket}\r\n{strResponse}";
                    if (!string.IsNullOrEmpty(strGetTickets) & !strResponse.Contains("TransactionErrorCode") & strResponse.Contains("<TicketingControl><TransType>OK</TransType></TicketingControl>"))
                    {

                        // ***********************************************
                        // Prior for Ticket request we have to reRead PNR
                        // ***********************************************
                        strResponse = ttGA.SendCrypticMessage("I", ConversationID);
                        string strResp = ttGA.SendMessage(strRead, ConversationID);

                        strMessage += $"\r\n{strRead}\r\n{strResp}";

                        strResponse = ttGA.SendMessage(strGetTickets, ConversationID);
                        strResponse = strResponse.Replace("</DocProdFareManipulation_29>", strResponse + "</DocProdFareManipulation_29>");
                        strEmail += $"{strGetTickets}\r\n{strResponse}";
                        strMessage += $"\r\n{strGetTickets}\r\n{strResponse}";
                        strReplacementTag = "</DocProdFareManipulation_29>";
                    }


                    #endregion

                }

                // *****************************************************************
                // Transform Native Galileo IssueTicket Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    strResponse = inSession
                        ? strResponse.Replace(strReplacementTag, $"<ConversationID>{ConversationID}</ConversationID>{strReplacementTag}")
                        : strResponse;

                    // If String.IsNullOrEmpty(strCheckPrt) Then
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_IssueTicketRS.xsl");
                    strEmail += strResponse;
                }
                catch (Exception ex)
                {
                    strEmail += ex.Message;
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttGA.CloseSession(ConversationID);
                        ConversationID = null;
                        ttGA = null;
                    }
                }


                if (ProviderSystems.LogNative)
                    TripXMLTools.TripXMLLog.LogMessage("IssueTicket", ref strResponse, RequestTime, DateTime.Now, "Native", ProviderSystems.Provider, ProviderSystems.System, ProviderSystems.UserName);

            }
            catch (Exception exx)
            {
                AddLog($"<M>{Request}<BL/>", ProviderSystems.UserID);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.IssueTicketSessioned, exx.Message, ProviderSystems, "");
            }
            finally
            {
                // ****************************************************
                // below given if condition was not there in local code
                // ----------------------------------------------------
                if (Request.Contains("<System>Production</System>") & !strResponse.Contains("<Success"))
                {
                    // CoreLib.SendEmail("Galileo ticketing failure", strEmail, "Nexus")
                    string argMessage = $"<Ticket>{strEmail}</Ticket>";
                    LogMessageToFile("TravelBuild", ref argMessage, DateTime.Now, DateTime.Now);
                }
                // ----------------------------------------------------

                GC.Collect();
            }

            return strResponse;

        }

        public string Update()
        {
            string strResponse = "";

            try
            {
                string strRequest = SetRequest("Galileo_PNRReadRQ.xsl");
                DateTime RequestTime = DateTime.Now;


                // *********************
                // * Create Session    *
                // *********************
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);

                if (!string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation of OTA PNRRead Request produced empty xml.");

                // ********************************************
                // * Get Galileo Native PNR Retrieve response *
                // ********************************************
                var strNativePNRReply = ttGA.SendMessage(strRequest, ConversationID);
                var strMessage = $"{strRequest}\r\n{strNativePNRReply}";
                var strErrEvent = "Error sending Galileo PNR Retrieve Request.";

                if (!strNativePNRReply.Contains("TransactionErrorCode") & !(strNativePNRReply.Contains("ErrText") | strNativePNRReply.Contains("PNRBFRetrieve")))
                {
                    // *******************************
                    // Load OTA Modify XML document  *
                    // ******************************* 
                    var oDoc = new XmlDocument();
                    oDoc.LoadXml(Request);
                    var oRoot = oDoc.DocumentElement;

                    // *******************************
                    // Modify PNR - Insert elements *
                    // ******************************* 
                    strErrEvent = "Modify PNR - Insert elements Error.";
                    var strEndTransaction = oRoot.SelectSingleNode("ET").InnerXml;

                    if (oRoot.SelectSingleNode("Position/Element[@Operation='insert']") != null)
                    {
                        // *******************************************************************
                        // * Transform OTA Modify Request into Galileo Native Insert Request *
                        // *******************************************************************
                        strRequest = CoreLib.TransformXML($"<UpdateInsert>{Request}{strNativePNRReply}</UpdateInsert>", XslPath, $"{Version}Galileo_UpdateInsertRQ.xsl");

                        // ********************
                        // Get All Requests  * 
                        // ********************
                        var oDocTemp = new XmlDocument();
                        oDocTemp.LoadXml(strRequest);
                        var oRootTemp = oDocTemp.DocumentElement;
                        strEndTransaction = oRootTemp.SelectSingleNode("ET").InnerXml;

                        // insert other elements
                        if (oRootTemp.SelectSingleNode("MultiElements") != null)
                        {
                            var strMultiElements = oRootTemp.SelectSingleNode("MultiElements").InnerXml;
                            strNativePNRReply = ttGA.SendMessage(strMultiElements, ConversationID);
                            strMessage = $"\r\n{strMultiElements}\r\n{strNativePNRReply}";
                        }
                    }


                    // *********************************
                    // * Send End Transaction Request  *
                    // *********************************
                    strResponse = ttGA.SendMessage(strEndTransaction, ConversationID);
                    strMessage = $"\r\n{strEndTransaction}\r\n{strResponse}";
                    var oDocResp = new XmlDocument();
                    oDocResp.LoadXml(strResponse);
                    var oRootResp = oDocResp.DocumentElement;
                    var oNodeResp = oRootResp.SelectSingleNode("EndTransaction/EndTransactResponse/RecLoc");
                    if (oNodeResp != null)
                    {
                        var RecordLocator = oNodeResp.InnerText;
                        if (!string.IsNullOrEmpty(RecordLocator))
                        {
                            // Send Retreive Request
                            var strRTV = $"<PNRBFManagement_53><PNRBFRetrieveMods><PNRAddr><FileAddr/><CodeCheck/><RecLoc>{RecordLocator}</RecLoc></PNRAddr></PNRBFRetrieveMods>" +
                            "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>1</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods></PNRBFManagement_53>";
                            strResponse = ttGA.SendMessage(strRTV, ConversationID);
                            strMessage = $"\r\n{strRTV}\r\n{strResponse}";
                        }
                    }


                    // ****************************************************************************
                    // Add Previous Errors and Warnings To Galileo Native End Transact Response  *
                    // ****************************************************************************
                    strNativePNRReply = strResponse;
                }

                // *****************************************************************
                // Transform Native Galileo TravelBuild Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    strNativePNRReply = inSession
                        ? strResponse.Replace("</PNRBFManagement_53>", $"<ConversationID>{ConversationID}</ConversationID></PNRBFManagement_53>")
                        : strResponse;

                    strResponse = CoreLib.TransformXML(strNativePNRReply, XslPath, $"{Version}Galileo_PNRReadRS.xsl");

                    if (ProviderSystems.LogNative)
                        TripXMLTools.TripXMLLog.LogMessage("Update", ref strMessage, RequestTime, DateTime.Now, "Native", ProviderSystems.Provider, ProviderSystems.System, ProviderSystems.UserName);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttGA.CloseSession(ConversationID);
                        ConversationID = null;
                        ttGA = null;
                    }
                }
            }
            catch (Exception exx)
            {
                AddLog($"<M>{Request}<BL/>", ProviderSystems.UserID);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.Update, exx.Message, ProviderSystems, "");
            }
            finally
            {
                GC.Collect();
            }

            return strResponse;
        }

        public string UpdateSessioned()
        {
            string strResponse = "";
            try
            {
                var RequestTime = DateTime.Now;
                string strRequest = SetRequest("Galileo_UpdateInsertRQ.xsl");

                // *********************
                // * Create Session    *
                // *********************
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation of OTA PNRRead Request produced empty xml.");

                // ****************************
                // Retrieve existing PNR     *
                // **************************** 
                var oDoc = new XmlDocument();
                oDoc.LoadXml(Request);
                var oRoot = oDoc.DocumentElement;
                var strRecLocator = oRoot.SelectSingleNode("UniqueID/@ID").InnerText;
                string strCurrentPNR = $"<PNRBFManagement_53><PNRBFRetrieveMods><PNRAddr><FileAddr /><CodeCheck /><RecLoc>{strRecLocator}</RecLoc></PNRAddr></PNRBFRetrieveMods><FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>1</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods><FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>2</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods><FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>3</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods><FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>4</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods></PNRBFManagement_53>";
                
                // ********************************************
                // * Get Galileo Native PNR Retrieve response *
                // ********************************************
                string strNativePNRReply = ttGA.SendMessage(strCurrentPNR, ConversationID);
                string strMessage = $"{strCurrentPNR}\r\n{strNativePNRReply}";
                var strErrEvent = "Error sending Galileo PNR Retrieve Request.";

                if (!strNativePNRReply.Contains("TransactionErrorCode") & (!strNativePNRReply.Contains("ErrText") | strNativePNRReply.Contains("PNRBFRetrieve")))
                {
                    // *******************************
                    // Load OTA Modify XML document  *
                    // ******************************* 
                    oDoc = new XmlDocument();
                    oDoc.LoadXml(strRequest);
                    oRoot = oDoc.DocumentElement;
                    var strEndTransaction = oRoot.SelectSingleNode("ET").InnerXml;

                    // *******************************
                    // Modify PNR - Insert elements *
                    // ******************************* 
                    strErrEvent = "Modify PNR - Insert elements Error.";
                    oDoc.LoadXml(Request);
                    oRoot = oDoc.DocumentElement;
                    if (oRoot.SelectSingleNode("Position/Element[@Operation='insert']") != null)
                    {
                        // *******************************************************************
                        // * Transform OTA Modify Request into Galileo Native Insert Request *
                        // *******************************************************************
                        strRequest = CoreLib.TransformXML($"<UpdateInsert>{Request}{strNativePNRReply}</UpdateInsert>", XslPath, $"{Version}Galileo_UpdateInsertRQ.xsl");

                        XmlDocument oDocTemp = null;

                        // ********************
                        // Get All Requests  * 
                        // ********************                        
                        oDocTemp = new XmlDocument();
                        oDocTemp.LoadXml(strRequest);
                        var oRootTemp = oDocTemp.DocumentElement;
                        strEndTransaction = oRootTemp.SelectSingleNode("ET").InnerXml;

                        // insert other elements
                        if (oRootTemp.SelectSingleNode("MultiElements") != null)
                        {
                            string strMultiElements = oRootTemp.SelectSingleNode("MultiElements").InnerXml;
                            strNativePNRReply = ttGA.SendMessage(strMultiElements, ConversationID);
                            if (strNativePNRReply.Contains("CHECK CONTINUITY SEGMENT"))
                                strNativePNRReply = ttGA.SendMessage(strEndTransaction, ConversationID);

                            strMessage = $"{strMultiElements}\r\n{strNativePNRReply}";
                        }
                    }

                    strResponse = strNativePNRReply;
                    strMessage = $"{strEndTransaction}\r\n{strResponse}";
                    strResponse = ttGA.SendMessage(strCurrentPNR, ConversationID);

                    var oDocResp = new XmlDocument();
                    oDocResp.LoadXml(strResponse);
                    var oRootResp = oDocResp.DocumentElement;
                    var oNodeResp = oRootResp.SelectSingleNode("EndTransaction/EndTransactResponse/RecLoc");
                    if (oNodeResp != null)
                    {
                        string RecordLocator = oNodeResp.InnerText;
                        if (!string.IsNullOrEmpty(RecordLocator))
                        {
                            // Send Retreive Request
                            string strRTV = $"<PNRBFManagement_53><PNRBFRetrieveMods><PNRAddr><FileAddr/><CodeCheck/><RecLoc>{RecordLocator}</RecLoc></PNRAddr></PNRBFRetrieveMods>" +
                            "<FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>1</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods></PNRBFManagement_53>";
                            strResponse = ttGA.SendMessage(strRTV, ConversationID);
                            strMessage = $"{strRTV}\r\n{strResponse}";
                        }
                    }

                    // ****************************************************************************
                    // Add Previous Errors and Warnings To Galileo Native End Transact Response   *
                    // ****************************************************************************
                    strNativePNRReply = strResponse;
                    oDocResp = null;
                }

                // *****************************************************************
                // Transform Native Galileo TravelBuild Response into OTA Response *
                // ***************************************************************** 
                try
                {

                    Version = string.IsNullOrEmpty(Version) ? "v03_" : Version;

                    strNativePNRReply = inSession
                        ? strResponse.Replace("</PNRBFManagement_53>", $"<ConversationID>{ConversationID}</ConversationID></PNRBFManagement_53>")
                        : strResponse;

                    //CoreLib.SendTrace(ProviderSystems.UserID, "QRead", "Final response", strResponse, ProviderSystems.LogUUID);
                    CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", $"Final response size for version {Version}", strResponse.Length.ToString(CultureInfo.InvariantCulture), ProviderSystems.LogUUID);
                    if (strNativePNRReply.Length > 5500)
                    {
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response I", strNativePNRReply.Substring(0, strNativePNRReply.Length / 2), ProviderSystems.LogUUID);
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response II", strNativePNRReply.Substring(strNativePNRReply.Length / 2), ProviderSystems.LogUUID);
                    }
                    else
                    {
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response I", strResponse, ProviderSystems.LogUUID);
                    }
                    strResponse = CoreLib.TransformXML(strNativePNRReply, XslPath, $"{Version}Galileo_PNRReadRS.xsl");

                    if (ProviderSystems.LogNative)
                        TripXMLTools.TripXMLLog.LogMessage("Update", ref strMessage, RequestTime, DateTime.Now, "Native", ProviderSystems.Provider, ProviderSystems.System, ProviderSystems.UserName);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttGA.CloseSession(ConversationID);
                        ConversationID = null;
                        ttGA = null;
                    }
                }
            }
            catch (Exception exx)
            {
                AddLog($"<M>{Request}<BL/>", ProviderSystems.UserID);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.Update, exx.Message, ProviderSystems, "");
            }
            finally
            {
                GC.Collect();
            }

            return strResponse;
        }

        public string VoidTicket()
        {
            string strResponse;
            try
            {
                string strFinalResp = "";
                var RequestTime = DateTime.Now;
                string strRequest = SetRequest("Galileo_VoidTicketRQ.xsl");

                // *********************
                // * Create Session    *
                // *********************
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);

                if (!string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation of OTA PNRRead Request produced empty xml.");

                var oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                var oRoot = oDoc.DocumentElement;

                foreach (XmlNode nd in oRoot.SelectNodes("TicketVoid_2"))
                {
                    try
                    {
                        strRequest = nd.OuterXml;
                        strResponse = ttGA.SendMessage(strRequest, ConversationID);
                        var oDocTicket = new XmlDocument();
                        oDocTicket.LoadXml(strRequest);
                        var oRootTicket = oDocTicket.DocumentElement;
                        strFinalResp += strResponse.Contains("ErrText")
                            ? $"<Ticket Number=\"{nd.SelectSingleNode("VoidTicketMods").SelectSingleNode("TicketNumberRange").SelectSingleNode("AirNumeric").InnerText + "-" + nd.SelectSingleNode("VoidTicketMods").SelectSingleNode("TicketNumberRange").SelectSingleNode("TkStockNum").InnerText}\" Status=\"NotVoid\"/>"
                            : $"<Ticket Number=\"{nd.SelectSingleNode("VoidTicketMods").SelectSingleNode("TicketNumberRange").SelectSingleNode("AirNumeric").InnerText + "-" + nd.SelectSingleNode("VoidTicketMods").SelectSingleNode("TicketNumberRange").SelectSingleNode("TkStockNum").InnerText}\" Status=\"Void\"/>";
                    }
                    catch (Exception ex)
                    {
                        strFinalResp += $"<Ticket Number=\"{nd.SelectSingleNode("VoidTicketMods").SelectSingleNode("TicketNumberRange").SelectSingleNode("AirNumeric").InnerText + "-" + nd.SelectSingleNode("VoidTicketMods").SelectSingleNode("TicketNumberRange").SelectSingleNode("TkStockNum").InnerText}\" Status=\"NotVoid\"/>";
                    }
                }

                // ***************************************************************** 
                // Transform Native Amadeus IssueTicket Response into OTA Response * 
                // ***************************************************************** 
                try
                {
                    // ***********************************************
                    // First if block was not there in local code
                    // ***********************************************                    
                    strResponse = string.IsNullOrEmpty(strFinalResp)
                        ? "<TT_VoidTicketRS Version=\"1.0\"><Warnings><Warning>Ticket not void</Warning></Warnings></TT_VoidTicketRS>"
                        : strFinalResp.Contains("NotVoid")
                            ? $"<TT_VoidTicketRS Version=\"1.0\"><Warnings><Warning>Ticket not void</Warning></Warnings>{strFinalResp}</TT_VoidTicketRS>"
                            : $"<TT_VoidTicketRS Version=\"1.0\"><Success/>{strFinalResp}</TT_VoidTicketRS>";

                    strResponse = inSession
                        ? strResponse.Replace("</TT_VoidTicketRS>", $"<ConversationID>{ConversationID}</ConversationID></TT_VoidTicketRS>")
                        : strResponse;

                    // strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_IssueTicketRS.xsl", false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttGA.CloseSession(ConversationID);
                        ConversationID = null;
                        ttGA = null;
                    }
                }

            }
            catch (Exception exx)
            {
                AddLog($"<M>{Request}<BL/>", ProviderSystems.UserID);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.TicketVoid, exx.Message, ProviderSystems, "");
            }
            finally
            {
                GC.Collect();
            }

            return strResponse;
        }

        public string ReLinkPrinters(string strReq, ref GalileoAdapter ttGA, string session)
        {
            try
            {
                var oDoc = new XmlDocument();
                string strResponse = ttGA.SendMessage(strReq, session);

                oDoc.LoadXml(strResponse);
                var oRoot = oDoc.DocumentElement;
                var oNd = oRoot.SelectSingleNode("LinkageUpdate/PrinterParameters[Type='T']");
                if (oNd == null)
                {
                    throw new Exception("No ticket printer linked");
                }
                else
                {
                    switch (oNd.SelectSingleNode("Status").InnerText ?? "")
                    {
                        case "D":
                            {
                                throw new Exception($"Printer {oNd.SelectSingleNode("LNIATA").InnerText} is Down");
                                break;
                            }

                        case "B":
                            {
                                throw new Exception($"Printer {oNd.SelectSingleNode("LNIATA").InnerText} is Busy");
                                break;
                            }
                    }
                }

                return strResponse;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public string ReSetPrinters(string strReq, ref GalileoAdapter ttGA, string session)
        {
            try
            {
                var oDoc = new XmlDocument();
                string strResponse = ttGA.SendCrypticMessage(strReq, session);
                if (!strResponse.Contains(" IS NOW UP"))
                {
                    throw new Exception(strResponse.Trim());
                }

                return strResponse;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

    }
}