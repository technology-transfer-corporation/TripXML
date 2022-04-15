using System;
using System.Xml;
using TripXMLMain;

namespace Galileo
{
    public class CarServices : GalileoBase
    {
        public CarServices()
        {
            Request = string.Empty;
            Version = string.Empty;
        }

        public string CarAvail()
        {
            string strResponse;

            // ************************************************************************
            // Save Pickup, Return Location and Dates to echo it back to the Response 
            // ************************************************************************
            try
            {
                var ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);

                // *****************************************************************
                // Transform OTA CarAvail Request into Native Galileo Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Galileo_CarAvailRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                var oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(strRequest);
                var oRoot = oReqDoc.DocumentElement;
                var PickUpLocation = oRoot.SelectSingleNode("VehAvailRQCore/VehRentalCore/PickUpLocation/@LocationCode").InnerText;
                var ReturnLocation = oRoot.SelectSingleNode("VehAvailRQCore/VehRentalCore/ReturnLocation/@LocationCode").InnerText;
                var PickUpDate = oRoot.SelectSingleNode("VehAvailRQCore/VehRentalCore/@PickUpDateTime").InnerText;
                var ReturnDate = oRoot.SelectSingleNode("VehAvailRQCore/VehRentalCore/@ReturnDateTime").InnerText;

                // *******************************************************************************
                //  Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                // ******************************************************************************* 
                strResponse = ttGA.SendMessage(strRequest, ConversationID);

                // *****************************************************************
                // Transform Native Galileo CarAvail Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    var tagToReplace = "</Car_AvailReply>";

                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_CarAvailRS.xsl");

                    // ********************************************************
                    // Add Pickup, Return Location and Dates to the Response 
                    // ********************************************************
                    oReqDoc = new XmlDocument();
                    oReqDoc.LoadXml(strResponse);
                    oRoot = oReqDoc.DocumentElement;
                    var oNode = oRoot.SelectSingleNode("VehAvailRSCore/VehRentalCore");
                    if (oNode != null)
                    {
                        if (PickUpDate.Length > 0)
                        {
                            oNode.Attributes["PickUpDateTime"].Value = PickUpDate;
                        }

                        if (ReturnDate.Length > 0)
                        {
                            oNode.Attributes["ReturnDateTime"].Value = ReturnDate;
                        }

                        if (PickUpLocation.Length > 0)
                        {
                            oNode.SelectSingleNode("PickUpLocation/@LocationCode").InnerText = PickUpLocation;
                        }

                        if (ReturnLocation.Length > 0)
                        {
                            oNode.SelectSingleNode("ReturnLocation/@LocationCode").InnerText = ReturnLocation;
                        }

                        strResponse = oReqDoc.OuterXml;
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
                        ConversationID = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CarAvail, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string CarInfo()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA CarInfo Request into Native Galileo Request     *
            // ***************************************************************** 
            try
            {
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);

                string strRequest = SetRequest("Galileo_CarInfoRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *****************************************************
                // Add StartDate Information to Galileo Native Request 
                // *****************************************************
                var oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(strRequest);
                var oRoot = oReqDoc.DocumentElement;

                // Start Date 
                var oNode = oRoot.SelectSingleNode("CarDescMods/StartDt");
                oNode.InnerText = DateTime.Now.AddDays(7).ToString("yyyyMMdd");

                // New Document
                strRequest = oReqDoc.OuterXml;

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                strResponse = ttGA.SendMessage(strRequest);

                // *****************************************************************
                // Transform Native Galileo CarInfo Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    var tagToReplace = "</Car_InfoReply>";

                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_CarInfoRS.xsl");
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
                        ConversationID = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CarInfo, ex.Message, ProviderSystems);
            }

            return strResponse;
        }
    }
}