using System;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXML.Core.Enums;
using TripXML.Core.Models;
using TripXML.Core.Models.Base;
using TripXML.Core.Utils;
using TripXMLMain;
using static TripXMLMain.modCore;
using wsTripXML.wsTravelTalk.wmCancelVirtualCardLoad;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsVirtualCreditCard
    {
        public TripXML tXML;

        private readonly modMain _modMain;

        public wsVirtualCreditCard(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Process Service Request All GDS 
        private StringBuilder sb = new StringBuilder();

        private T ServiceRequest<T>(VirtualCardRQBase request, ttServices ttServiceID)
        {
            string strResponse = "";
            TravelTalkCredential ttCredential = default;
            TripXMLProviderSystems ttProviderSystems = default;
            bool validateXSDOut;
            var startTime = default(DateTime);
            string uuid = "";
            T responseObj = default;

            try
            {
                startTime = DateTime.Now;
                string strRequest = SerializeRequest(request);
                _modMain.PreServiceRequest(ref strRequest, ref ttCredential, ref ttProviderSystems, startTime, (int)ttServiceID, Environment.MachineName, ref uuid);
                switch (request.BankSource)
                {
                    case VirtualCardSourceType.ConnexPay:
                        {
                            responseObj = modMain.SendPaymentRequest<T>(ttServiceID, ref ttCredential, ref ttProviderSystems, ref request);
                            strResponse = TripXMLSerializer.Serialize(responseObj);
                            break;
                        }
                    case VirtualCardSourceType.Airwallex:
                        {
                            responseObj = modMain.SendPaymentRequest<T>(ttServiceID, ref ttCredential, ref ttProviderSystems, ref request);
                            strResponse = TripXMLSerializer.Serialize(responseObj);
                            break;
                        }
                    case VirtualCardSourceType.USBank:
                        {
                            switch (ttCredential.Providers[0].Name.ToLower() ?? "")
                            {
                                case "amadeusws":
                                    {
                                        strResponse = modMain.SendPaymentRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                                        responseObj = TripXMLSerializer.Deserialize<T>(strResponse);
                                        break;
                                    }

                                default:
                                    {
                                        TripXMLProviderSystems ttDefProvider = default;
                                        _modMain.PreServiceRequest(ref strRequest, ref ttCredential, ref ttDefProvider, startTime, (int)ttServiceID, Environment.MachineName, ref uuid, "", true);

                                        strResponse = modMain.SendPaymentRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttDefProvider, ref strRequest);
                                        responseObj = TripXMLSerializer.Deserialize<T>(strResponse);
                                        break;
                                    }
                            }

                            break;
                        }
                }
                validateXSDOut = Conversions.ToBoolean(TripXMLMain.AppState.Get($"XSD{ttCredential.UserID}Out"));
                modMain.PostServiceRequest(ref strResponse, validateXSDOut, (int)ttServiceID, ttCredential.UserID);
            }
            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers[0].Name);
            }
            finally
            {
                _modMain.LogResponse(ref strResponse, ref ttCredential, startTime, (int)ttServiceID, Environment.MachineName, ref uuid);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsGenerateVirtualCard", "============= OTA Response ============= ", strResponse, uuid);
            }

            return responseObj;
        }

        #endregion

        #region  Web Methods 
        public PAY_GenerateVirtualCardRS GenerateVirtualCard(PAY_GenerateVirtualCardRQ PAY_GenerateVirtualCardRQ)
        {
            PAY_GenerateVirtualCardRS oVCCRS = null;
            try
            {
                oVCCRS = ServiceRequest<PAY_GenerateVirtualCardRS>(PAY_GenerateVirtualCardRQ, ttServices.GenerateVirtualCard);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "GenerateVirtualCard", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }
            return oVCCRS;
        }
        public PAY_CancelVirtualCardLoadRS CancelVirtualCardLoad(PAY_CancelVirtualCardLoadRQ PAY_CancelVirtualCardLoadRQ)
        {
            PAY_CancelVirtualCardLoadRS oVCCRS = null;

            try
            {
                oVCCRS = ServiceRequest<PAY_CancelVirtualCardLoadRS>(PAY_CancelVirtualCardLoadRQ, ttServices.CancelVirtualCardLoad);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "CancelVirtualCardLoad", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oVCCRS;

        }
        public PAY_DeleteVirtualCardRS DeleteVirtualCard(PAY_DeleteVirtualCardRQ PAY_DeleteVirtualCardRQ)
        {
            PAY_DeleteVirtualCardRS oVCCRS = null;

            try
            {
                oVCCRS = ServiceRequest<PAY_DeleteVirtualCardRS>(PAY_DeleteVirtualCardRQ, ttServices.DeleteVirtualCard);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "DeleteVirtualCard", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oVCCRS;

        }
        public PAY_GetVirtualCardDetailsRS GetVirtualCardDetails(PAY_GetVirtualCardDetailsRQ PAY_GetVirtualCardDetailsRQ)
        {
            PAY_GetVirtualCardDetailsRS oVCCRS = null;

            try
            {
                oVCCRS = ServiceRequest<PAY_GetVirtualCardDetailsRS>(PAY_GetVirtualCardDetailsRQ, ttServices.GetVirtualCardDetails);
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace("", "GetVirtualCardDetails", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oVCCRS;

        }
        public PAY_ListVirtualCardsRS ListVirtualCards(PAY_ListVirtualCardsRQ PAY_ListVirtualCardsRQ)
        {
            PAY_ListVirtualCardsRS oVCCRS = null;

            try
            {
                oVCCRS = ServiceRequest<PAY_ListVirtualCardsRS>(PAY_ListVirtualCardsRQ, ttServices.ListVirtualCards);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "ListVirtualCards", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oVCCRS;

        }

        #endregion

        #region Serializer / Desirializer
        public string SerializeRequest(object request)
        {
            try
            {
                var oSerializer = new XmlSerializer(request.GetType());
                var oWriter = new System.IO.StringWriter(new StringBuilder());
                oSerializer.Serialize(oWriter, request);
                string xmlMessage = oWriter.ToString();
                xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
                xmlMessage = xmlMessage.Replace(" xmlns=\"http://www.opentravel.org/OTA/2003/05\"", "");
                xmlMessage = xmlMessage.Replace(" xmlns=\"http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2\"", "");

                return xmlMessage;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

        }
        #endregion

    }
}