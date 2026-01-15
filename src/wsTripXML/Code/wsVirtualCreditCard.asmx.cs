using System;
using System.Diagnostics;
using System.Text;
using System.Web.Services;
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
    [System.Web.Services.Protocols.SoapDocumentService(RoutingStyle = System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement)]
    [WebService(Namespace = "http://tripxml.downtowntravel.com/wsVirtualCreditCard", Name = "wsVirtualCreditCard", Description = "A TripXML Web Service to Process Virtual Card Requests.")]
    public class wsVirtualCreditCard : WebService
    {
        public TripXML tXML;

        #region  Web Services Designer Generated Code 

        public wsVirtualCreditCard() : base()
        {

            // This call is required by the Web Services Designer.
            InitializeComponent();

            // Add your own initialization code after the InitializeComponent() call

        }

        // Required by the Web Services Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Web Services Designer
        // It can be modified using the Web Services Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        protected override void Dispose(bool disposing)
        {
            // CODEGEN: This procedure is required by the Web Services Designer
            // Do not modify it using the code editor.
            if (disposing)
            {
                if (components is not null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

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
                var argoApp = Application;
                modMain.PreServiceRequest(ref strRequest, ref argoApp, ref ttCredential, ref ttProviderSystems, startTime, (int)ttServiceID, Server.MachineName, ref uuid);
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
                                        var argoApp1 = Application;
                                        modMain.PreServiceRequest(ref strRequest, ref argoApp1, ref ttCredential, ref ttDefProvider, startTime, (int)ttServiceID, Server.MachineName, ref uuid, "", true);

                                        strResponse = modMain.SendPaymentRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttDefProvider, ref strRequest);
                                        responseObj = TripXMLSerializer.Deserialize<T>(strResponse);
                                        break;
                                    }
                            }

                            break;
                        }
                }
                validateXSDOut = Conversions.ToBoolean(Application.Get($"XSD{ttCredential.UserID}Out"));
                modMain.PostServiceRequest(ref strResponse, validateXSDOut, (int)ttServiceID, ttCredential.UserID);
            }
            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers[0].Name);
            }
            finally
            {
                modMain.LogResponse(ref strResponse, ref ttCredential, startTime, (int)ttServiceID, Server.MachineName, ref uuid);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsGenerateVirtualCard", "============= OTA Response ============= ", strResponse, uuid);
            }

            return responseObj;
        }

        #endregion

        #region  Web Methods 

        [CompressionExtension.CompressionExtension]
        [WebMethod(Description = "Process Generate Virtual Card Messages Request.")]
        [System.Web.Services.Protocols.SoapHeader("tXML")]
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

        [CompressionExtension.CompressionExtension]
        [WebMethod(Description = "Process Cancel Virtual Card Load Messages Request.")]
        [System.Web.Services.Protocols.SoapHeader("tXML")]
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

        [CompressionExtension.CompressionExtension]
        [WebMethod(Description = "Process Delete Virtual Card Messages Request.")]
        [System.Web.Services.Protocols.SoapHeader("tXML")]
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

        [CompressionExtension.CompressionExtension]
        [WebMethod(Description = "Process Detail Virtual Card Messages Request.")]
        [System.Web.Services.Protocols.SoapHeader("tXML")]
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

        [CompressionExtension.CompressionExtension]
        [WebMethod(Description = "Process List Virtual Credit Cards Request.")]
        [System.Web.Services.Protocols.SoapHeader("tXML")]
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