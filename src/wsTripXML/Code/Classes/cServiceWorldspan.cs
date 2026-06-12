using System;
using System.Data;
using System.Text;
using System.Xml;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{

    public class cServiceWorldspan
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
            Worldspan.AirServices ttService = null;
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            string strMsg = "";

            try
            {
                ttService = new Worldspan.AirServices();

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
                                var argttCities = ttCities;
                                strResponse = withBlock.LowFare(ref argttCities);
                                ttCities = argttCities;
                                break;
                            }
                        case ttServices.LowFarePlus:
                            {
                                var argttCities1 = ttCities;
                                strResponse = withBlock.LowFarePlus(ref argttCities1);
                                ttCities = argttCities1;
                                break;
                            }

                        default:
                            {
                                throw new Exception("Invalid request or message not supported by Worldspan air services.");
                            }
                    }
                    if (ttProviderSystems.PCC.Length > 0)
                    {
                        strResponse = strResponse.Replace("TransactionIdentifier=\"Worldspan", sb.Append("TransactionIdentifier=\"Worldspan").Append("-").Append(ttProviderSystems.PCC).ToString());
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
                            CoreLib.SendTrace("", "cServiceWorldspan", "Error Loading business logic file", exr.Message, ttProviderSystems.LogUUID);
                            throw exr;
                        }

                        oRoot = oDoc.DocumentElement;
                        oNode = oRoot.SelectSingleNode(sb.Append("Message[@Name='").Append(strMsg).Append("'][@Direction='Out']").ToString());
                        sb.Remove(0, sb.Length);

                        if (oNode is not null)
                        {
                            oNode = oNode.SelectSingleNode(sb.Append("ProviderBL[@Name='Worldspan'][@System='").Append(withBlock.ProviderSystems.System).Append("'][@PCC='").Append(withBlock.ProviderSystems.PCC.ToUpper()).Append("']").ToString());
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
                strResponse = FormatErrorMessage(ServiceID, ex.Message, "Worldspan", "", false, Version);
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
            Worldspan.PNRServices ttService = null;

            try
            {
                ttService = new Worldspan.PNRServices();

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
                        case ttServices.PNRCancel:
                            {
                                strResponse = withBlock.PNRCancel();
                                break;
                            }
                        case var @case when @case == ttServices.PNRCancel:
                            {
                                strResponse = withBlock.PNREnd();
                                break;
                            }

                        default:
                            {
                                throw new Exception("Invalid request or message not supported by Worldspan PNR services.");
                            }
                    }

                }
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ServiceID, ex.Message, "Worldspan", "", false, Version);
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
            Worldspan.TravelServices ttService = null;

            try
            {
                ttService = new Worldspan.TravelServices();

                {
                    ref var withBlock = ref ttService;
                    withBlock.Version = Version;
                    withBlock.XslPath = XslPath;
                    withBlock.ProviderSystems = ttProviderSystems;
                    withBlock.Request = Request;

                    switch (ServiceID)
                    {
                        case ttServices.TravelBuild:
                            {
                                strResponse = withBlock.TravelBuild();
                                break;
                            }

                        default:
                            {
                                throw new Exception("Invalid request or message not supported by Worldspan Travel services.");
                            }
                    }

                }
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ServiceID, ex.Message, "Worldspan", "", false, Version);
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
                CoreLib.SendTrace("", "cServiceWorldspan", sb.Append("Before ").Append(strMsg).Append(" business logic").ToString(), strResponse, ttProviderSystems.LogUUID);
                sb.Remove(0, sb.Length);
                strResponse = CoreLib.TransformXML(strResponse, xslPath, sb.Append(Version).Append("BL_").Append(strMsg).Append("RS.xsl").ToString());
                sb.Remove(0, sb.Length);
            }
            sb = null;
            return strResponse;
        }

    }

}