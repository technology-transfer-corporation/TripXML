using System;
using System.Data;
using System.Text;
using System.Xml;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{

    public class cServiceTravelport
    {

        public event GotResponseEventHandler GotResponse;

        public delegate void GotResponseEventHandler(string Response);

        #region  Properties 
        private StringBuilder sb = new StringBuilder();
        private TripXMLProviderSystems ttProviderSystems;

        public ttServices ServiceID { get; set; }

        public string Request { get; set; } = "";

        public string Version { get; set; } = "";

        public TripXMLProviderSystems ProviderSystems
        {
            get
            {
                return ttProviderSystems;
            }
            set
            {
                ttProviderSystems = value;
            }
        }

        public DataView ttCities { get; set; }

        #endregion

        public void SendAirRequest()
        {
            string strResponse = "";
            Travelport.AirServices ttService = null;
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            string strMsg = "";

            try
            {
                ttService = new Travelport.AirServices();

                {
                    ref var withBlock = ref ttService;
                    withBlock.Version = Version;
                    withBlock.XslPath = XslPath;
                    withBlock.ProviderSystems = ttProviderSystems;
                    withBlock.Request = Request;

                    switch (ServiceID)
                    {
                        case ttServices.LowFare:
                            {
                                strResponse = withBlock.LowFare();
                                break;
                            }
                        case ttServices.LowFarePlus:
                            {
                                strResponse = withBlock.LowFarePlus();
                                break;
                            }

                        default:
                            {
                                throw new Exception("Invalid request or message not supported by Travelport air services.");
                            }
                    }
                    if (ttProviderSystems.PCC.Length > 0)
                    {
                        strResponse = strResponse.Replace("TransactionIdentifier=\"Travelport", sb.Append("TransactionIdentifier=\"Travelport").Append("-").Append(ttProviderSystems.PCC).ToString());
                        sb.Remove(0, sb.Length);
                    }

                    if (!string.IsNullOrEmpty(withBlock.ProviderSystems.BLFile))
                    {
                        oDoc = new XmlDocument();
                        // Load Access Control List into memory
                        try
                        {
                            oDoc.Load(withBlock.ProviderSystems.BLFile);
                        }
                        catch (Exception exr)
                        {
                            CoreLib.SendTrace("", "cServiceTravelport", "Error Loading business logic file", exr.Message, ttProviderSystems.LogUUID);
                            throw exr;
                        }

                        oRoot = oDoc.DocumentElement;
                        oNode = oRoot.SelectSingleNode(sb.Append("Message[@Name='").Append(strMsg).Append("'][@Direction='Out']").ToString());
                        sb.Remove(0, sb.Length);

                        if (oNode is not null)
                        {
                            oNode = oNode.SelectSingleNode(sb.Append("ProviderBL[@Name='Travelport'][@System='").Append(withBlock.ProviderSystems.System).Append("'][@PCC='").Append(withBlock.ProviderSystems.PCC.ToUpper()).Append("']").ToString());
                            sb.Remove(0, sb.Length);

                            if (oNode is not null)
                            {
                                strResponse = BusinessLogic(strResponse, oNode.OuterXml, sb.Append(XslPath).Append(@"BL\").ToString(), strMsg);
                                sb.Remove(0, sb.Length);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ServiceID, ex.Message, "Travelport", "", false, Version);
            }
            finally
            {
                if (ttService is not null)
                    ttService = null;
                GotResponse?.Invoke(strResponse);
            }
            sb = null;
        }

        public void SendPNRRequest()
        {
            string strResponse = "";
            Travelport.PNRServices ttService = null;

            try
            {
                ttService = new Travelport.PNRServices();

                {
                    ref var withBlock = ref ttService;
                    withBlock.Version = Version;
                    withBlock.XslPath = XslPath;
                    withBlock.ProviderSystems = ttProviderSystems;
                    withBlock.Request = Request;

                    switch (ServiceID)
                    {
                        case ttServices.PNRRead:
                            {
                                strResponse = withBlock.PNRRead();
                                break;
                            }
                        // strResponse = .PNRCancel()
                        case ttServices.PNRCancel:
                            {
                                break;
                            }

                        default:
                            {
                                throw new Exception("Invalid request or message not supported by Travelport PNR services.");
                            }
                    }

                }
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ServiceID, ex.Message, "Travelport", "", false, Version);
            }
            finally
            {
                if (ttService is not null)
                    ttService = null;
                GotResponse?.Invoke(strResponse);
            }

        }

        public void SendTravelRequest()
        {
            string strResponse = "";
            Travelport.TravelServices ttService = null;

            try
            {
                ttService = new Travelport.TravelServices();

                {
                    ref var withBlock = ref ttService;
                    withBlock.Version = Version;
                    withBlock.XslPath = XslPath;
                    withBlock.ProviderSystems = ttProviderSystems;
                    withBlock.Request = Request;

                    switch (ServiceID)
                    {
                        // strResponse = .TravelBuild()
                        case ttServices.TravelBuild:
                            {
                                break;
                            }

                        default:
                            {
                                throw new Exception("Invalid request or message not supported by Travelport Travel services.");
                            }
                    }

                }
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ServiceID, ex.Message, "Travelport", "", false, Version);
            }
            finally
            {
                if (ttService is not null)
                    ttService = null;
                GotResponse?.Invoke(strResponse);
            }

        }

        public string BusinessLogic(string strResponse, string strBusiness, string xslPath, string strMsg)
        {

            if (strResponse.IndexOf("<Success />") != -1 || strResponse.IndexOf("<Success></Success>") != -1)
            {
                strResponse = strResponse.Replace("<Success />", sb.Append(strBusiness).Append("<Success />").ToString());
                sb.Remove(0, sb.Length);
                strResponse = strResponse.Replace("<Success></Success>", sb.Append(strBusiness).Append("<Success></Success>").ToString());
                sb.Remove(0, sb.Length);
                CoreLib.SendTrace("", "cServiceTravelport", sb.Append("Before ").Append(strMsg).Append(" business logic").ToString(), strResponse, ttProviderSystems.LogUUID);
                sb.Remove(0, sb.Length);
                strResponse = CoreLib.TransformXML(strResponse, xslPath, sb.Append(Version).Append("BL_").Append(strMsg).Append("RS.xsl").ToString());
                sb.Remove(0, sb.Length);
            }
            sb = null;
            return strResponse;
        }

    }

}