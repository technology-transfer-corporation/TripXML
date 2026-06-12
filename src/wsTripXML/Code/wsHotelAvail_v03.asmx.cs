using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsHotelAvail_v03
    {

        private readonly modMain _modMain;

        public wsHotelAvail_v03(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Function 

        private string DecodeHotelAvail(string strResponse, string UserID)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            DataView ttHotels;
            DataView ttHotelAmenities;
            DataView ttHotelAreas;
            DataView ttHotelSubTitles;
            XmlNode oNode = null;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                ttHotels = (DataView)TripXMLMain.AppState.Get("ttHotels");

                foreach (XmlNode currentONode in oRoot.SelectNodes("RoomStays/RoomStay/BasicPropertyInfo"))
                {
                    oNode = currentONode;
                    // *******************
                    // Decode Hotels   *
                    // *******************
                    if (!(oNode.Attributes["ChainCode"] is null | oNode.Attributes["ChainName"] is null))
                    {
                        string argstrCode = oNode.Attributes["ChainCode"].Value;
                        oNode.Attributes["ChainName"].Value = modMain.GetDecodeValue(ref ttHotels, ref argstrCode);
                        oNode.Attributes["ChainCode"].Value = argstrCode;
                    }
                    // *******************************
                    // Decode Hotels for OutriggerR  *
                    // *******************************
                    if (!(oNode.Attributes["HotelCode"] is null | oNode.Attributes["HotelName"] is null))
                    {
                        if (oNode.Attributes["HotelName"].Value.Length == 0)
                        {
                            string argstrCode1 = oNode.Attributes["HotelCode"].Value;
                            oNode.Attributes["HotelName"].Value = modMain.GetDecodeValue(ref ttHotels, ref argstrCode1);
                            oNode.Attributes["HotelCode"].Value = argstrCode1;
                        }
                    }
                }

                if (oRoot.SelectNodes("RoomStays/RoomStay/RoomTypes/RoomType/Amenities/Amenity") is not null | oRoot.SelectNodes("Criteria/Criterion/HotelAmenity") is not null)
                {
                    ttHotelAmenities = (DataView)TripXMLMain.AppState.Get("ttHotelAmenities");

                    foreach (XmlNode currentONode1 in oRoot.SelectNodes("RoomStays/RoomStay/RoomTypes/RoomType/Amenities/Amenity"))
                    {
                        oNode = currentONode1;
                        if (oNode.Attributes["RoomAmenity"] is not null)
                        {
                            string argstrCode2 = oNode.Attributes["RoomAmenity"].Value;
                            oNode.InnerText = modMain.GetDecodeValue(ref ttHotelAmenities, ref argstrCode2);
                            oNode.Attributes["RoomAmenity"].Value = argstrCode2;
                        }
                    }

                    foreach (XmlNode currentONode2 in oRoot.SelectNodes("Criteria/Criterion/HotelAmenity"))
                    {
                        oNode = currentONode2;
                        if (oNode.Attributes["Code"] is not null)
                        {
                            string hacode = "";
                            if (oNode.Attributes["Code"].Value.Contains(" "))
                            {
                                hacode = ":" + oNode.Attributes["Code"].Value.Substring(oNode.Attributes["Code"].Value.IndexOf(" ")).Trim();
                                oNode.Attributes["Code"].Value = oNode.Attributes["Code"].Value.Substring(0, oNode.Attributes["Code"].Value.IndexOf(" ") - 1);
                            }
                            string localGetDecodeValue() { string argstrCode = oNode.Attributes["Code"].Value; var ret = modMain.GetDecodeValue(ref ttHotelAmenities, ref argstrCode); oNode.Attributes["Code"].Value = argstrCode; return ret; }

                            oNode.InnerText = localGetDecodeValue() + hacode;
                        }
                    }
                }

                if (oRoot.SelectNodes("Areas/Area") is not null)
                {
                    ttHotelAreas = (DataView)TripXMLMain.AppState.Get("ttHotelAreas");

                    foreach (XmlNode currentONode3 in oRoot.SelectNodes("Areas/Area"))
                    {
                        oNode = currentONode3;
                        if (oNode.Attributes["AreaID"] is not null)
                        {
                            string argstrCode3 = oNode.Attributes["AreaID"].Value;
                            oNode.SelectSingleNode("AreaDescription/Text").InnerText = modMain.GetDecodeValue(ref ttHotelAreas, ref argstrCode3);
                            oNode.Attributes["AreaID"].Value = argstrCode3;
                        }
                    }
                }

                if (oRoot.SelectNodes("RoomStays/RoomStay/BasicPropertyInfo/VendorMessages/VendorMessages[@InfoType='Text']") is not null)
                {
                    ttHotelSubTitles = (DataView)TripXMLMain.AppState.Get("ttHotelSubTitles");

                    foreach (XmlNode currentONode4 in oRoot.SelectNodes("RoomStays/RoomStay/BasicPropertyInfo/VendorMessages/VendorMessages[@InfoType='Text']/SubSection"))
                    {
                        oNode = currentONode4;
                        if (oNode.Attributes["SubCode"] is not null)
                        {
                            string argstrCode4 = oNode.Attributes["SubCode"].Value;
                            oNode.Attributes["SubTitle"].Value = modMain.GetDecodeValue(ref ttHotelSubTitles, ref argstrCode4);
                            oNode.Attributes["SubCode"].Value = argstrCode4;
                        }
                    }
                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsHotelAvail_v03", "Error *** Decoding HotelAvail Response", ex.Message, string.Empty);
            }

            return strResponse;

        }

        #endregion
        private StringBuilder sb = new StringBuilder();

        private string mstrResponse = "";
        private int mintProviders = 0;

        private void GotResponse(string Response)
        {
            mstrResponse += Response;
            mintProviders += 1;
        }

        #region  Process Service Request All GDS 

        private string ServiceRequest(string strRequest, ttServices ttServiceID)
        {
            string strResponse = "";
            TravelTalkCredential ttCredential = default;
            TripXMLProviderSystems ttProviderSystems = default;
            bool ValidateXSDOut;
            var StartTime = default(DateTime);
            string UUID = "";
            int i;
            // Dim DoAmadeusSearches(99) As SearchHotelAmadeus
            var DoAmadeusWSSearches = new SearchHotelAmadeusWS[100];
            var DoGalileoSearches = new SearchHotelGalileo[100];
            var DoSabreSearches = new SearchHotelSabre[100];
            // Dim DoWorldspanSearches(99) As SearchHotelWorldspan
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            XmlNode oNodeNewCriterion = null;
            XmlDocument oDocCriterion = null;
            XmlElement oRootCriterion = null;
            string strCriterion;
            int j = 0;

            try
            {
                StartTime = DateTime.Now;
                _modMain.PreServiceRequestPool(ref strRequest, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Environment.MachineName, ref UUID);
                ValidateXSDOut = Conversions.ToBoolean(TripXMLMain.AppState.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                oRoot = oDoc.DocumentElement;
                oNodeNewCriterion = oRoot.SelectSingleNode("AvailRequestSegments/AvailRequestSegment/HotelSearchCriteria/Criterion");
                strCriterion = oNodeNewCriterion.OuterXml;
                oDocCriterion = new XmlDocument();
                oDocCriterion.LoadXml(strCriterion);
                oRootCriterion = oDocCriterion.DocumentElement;
                oNodeNewCriterion.RemoveAll();

                {
                    ref var withBlock = ref ttCredential;
                    var loopTo = withBlock.Providers.Length - 1;
                    for (i = 0; i <= loopTo; i++)
                    {
                        foreach (XmlNode oNodeCriterion in oRootCriterion.SelectNodes("HotelRef"))
                        {
                            oNodeNewCriterion.InnerXml = oNodeCriterion.OuterXml;
                            strRequest = oRoot.OuterXml;
                            oNodeNewCriterion.RemoveAll();

                            switch (withBlock.Providers[i].Name.ToLower() ?? "")
                            {
                                case "amadeus":
                                case "amadeusws":
                                    {
                                        try
                                        {
                                            // Dim ttAA As AmadeusAPIAdapter
                                            withBlock.Providers[i].Name = withBlock.Providers[i].Name.Replace("AmadeusWS", "Amadeus");
                                            // ttAA = TripXMLMain.AppState.Get(sb.Append("API").Append(.UserID).Append(.System).Append(.Providers(i).PCC).ToString())
                                            // sb.Remove(0, sb.Length())
                                            // If ttAA Is Nothing Then
                                            // ttProviderSystems = TripXMLMain.AppState.Get(sb.Append("PS").Append(ttCredential.Providers(i).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(i).PCC).ToString())
                                            // sb.Remove(0, sb.Length())

                                            // If ttProviderSystems.AmadeusWS = False Then
                                            // GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), .Providers(i).Name))
                                            // sb.Remove(0, sb.Length())
                                            // Exit Select
                                            // End If
                                            // End If

                                            if (ttProviderSystems.AmadeusWS == true)
                                            {
                                                if (ttCredential.Providers[i].PCC.Trim().Length > 0)
                                                {
                                                    ttProviderSystems.PCC = ttCredential.Providers[i].PCC;
                                                }

                                                ttCredential.Providers[0].Name = "AmadeusWS";

                                                if (ttCredential.System == "Test")
                                                {
                                                    ttProviderSystems.URL = "https://test.webservices.amadeus.com";
                                                }
                                                else if (ttCredential.System == "Training")
                                                {
                                                    ttProviderSystems.URL = "https://production.webservices.amadeus.com";
                                                }
                                                else
                                                {
                                                    ttProviderSystems.URL = "https://production.webservices.amadeus.com";
                                                }

                                                var oAmadeusWS = new cServiceAmadeusWS();
                                                // Dim oThreadAmadeusWS As New Thread(New ThreadStart(AddressOf oAmadeusWS.SendHotelRequest))
                                                oAmadeusWS.GotResponse += GotResponse;

                                                {
                                                    ref var withBlock1 = ref oAmadeusWS;
                                                    withBlock1.ServiceID = (int)ttServiceID;
                                                    withBlock1.Request = strRequest;
                                                    withBlock1.ttProviderSystems = ttProviderSystems;
                                                    withBlock1.Version = "";
                                                }
                                                // oThreadAmadeusWS.Start()
                                                DoAmadeusWSSearches[j] = new SearchHotelAmadeusWS(withBlock.Providers[i].PCC, withBlock.UserID, withBlock.System, ref ttProviderSystems, ref oAmadeusWS);
                                                DoAmadeusWSSearches[j].Request = strRequest;
                                                DoAmadeusWSSearches[j].ServiceID = ((int)ttServiceID).ToString();
                                                DoAmadeusWSSearches[j].BeginSearch();
                                                ttProviderSystems = default;
                                                // Else
                                                // ttProviderSystems = ttAA.ttProviderSystems

                                                // If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                                // ttAA.SourcePCC = ttCredential.Providers(i).PCC
                                                // Else
                                                // ttAA.SourcePCC = ttAA.ttProviderSystems.PCC
                                                // End If

                                                // Dim oAmadeus As New cServiceAmadeus
                                                // 'Dim oThreadAmadeus As New Thread(New ThreadStart(AddressOf oAmadeus.SendHotelRequest))
                                                // AddHandler oAmadeus.GotResponse, AddressOf GotResponse

                                                // With oAmadeus
                                                // .ServiceID = ttServiceID
                                                // .Request = strRequest
                                                // .ttAA = ttAA
                                                // .Version = ""
                                                // End With

                                                // 'oThreadAmadeus.Start()

                                                // DoAmadeusSearches(j) = New SearchHotelAmadeus(.Providers(i).PCC, .UserID, .System, ttAA, oAmadeus)
                                                // DoAmadeusSearches(j).Request = strRequest
                                                // DoAmadeusSearches(j).ServiceID =CInt(ttServiceID).ToString()
                                                // DoAmadeusSearches(j).BeginSearch()

                                                // TripXMLMain.AppState.Set(sb.Append("API").Append(.UserID).Append(.System).ToString(), ttAA)
                                                // sb.Remove(0, sb.Length())
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            GotResponse(FormatErrorMessage(ttServiceID, e.Message, withBlock.Providers[i].Name));
                                        }

                                        break;
                                    }
                                case "apollo":
                                case "galileo":
                                    {
                                        try
                                        {
                                            ttProviderSystems = (TripXMLProviderSystems)TripXMLMain.AppState.Get(sb.Append("PS").Append(withBlock.Providers[i].Name).Append(withBlock.UserID).Append(withBlock.System).Append(withBlock.Providers[i].PCC).ToString());
                                            sb.Remove(0, sb.Length);
                                            if (ttProviderSystems.System is null)
                                            {
                                                GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(withBlock.Providers[i].Name).Append(" - ").Append(withBlock.System).Append(" system. Or invalid provider.").ToString(), withBlock.Providers[i].Name));
                                                sb.Remove(0, sb.Length);
                                                break;
                                            }

                                            if (ttCredential.Providers[i].PCC.Trim().Length > 0)
                                            {
                                                ttProviderSystems.PCC = ttCredential.Providers[i].PCC;
                                            }

                                            var oGalileo = new cServiceGalileo();
                                            // Dim oThreadGalileo As New Thread(New ThreadStart(AddressOf oGalileo.SendHotelRequest))
                                            oGalileo.GotResponse += GotResponse;

                                            {
                                                ref var withBlock2 = ref oGalileo;
                                                withBlock2.ServiceID = (int)ttServiceID;
                                                withBlock2.Request = strRequest;
                                                withBlock2.ProviderSystems = ttProviderSystems;
                                                withBlock2.Version = "";
                                            }

                                            // oThreadGalileo.Start()
                                            DoGalileoSearches[j] = new SearchHotelGalileo(withBlock.Providers[i].PCC, withBlock.UserID, withBlock.System, ref ttProviderSystems, ref oGalileo);
                                            DoGalileoSearches[j].Request = strRequest;
                                            DoGalileoSearches[j].ServiceID = ((int)ttServiceID).ToString();
                                            DoGalileoSearches[j].BeginSearch();
                                        }

                                        catch (Exception e)
                                        {
                                            GotResponse(FormatErrorMessage(ttServiceID, e.Message, withBlock.Providers[i].Name));
                                        }

                                        break;
                                    }
                                case "sabre":
                                    {
                                        try
                                        {
                                            ttProviderSystems = (TripXMLProviderSystems)TripXMLMain.AppState.Get(sb.Append("PS").Append(withBlock.Providers[i].Name).Append(withBlock.UserID).Append(withBlock.System).Append(withBlock.Providers[i].PCC).ToString());
                                            sb.Remove(0, sb.Length);
                                            if (ttProviderSystems.System is null)
                                            {
                                                GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(withBlock.Providers[i].Name).Append(" - ").Append(withBlock.System).Append(" system. Or invalid provider.").ToString(), withBlock.Providers[i].Name));
                                                sb.Remove(0, sb.Length);
                                                break;
                                            }

                                            ttProviderSystems.AAAPCC = withBlock.Providers[i].PCC;

                                            // If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                            // ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                                            // End If

                                            var oSabre = new cServiceSabre();
                                            // Dim oThreadSabre As New Thread(New ThreadStart(AddressOf oSabre.SendHotelRequest))
                                            oSabre.GotResponse += GotResponse;

                                            {
                                                ref var withBlock3 = ref oSabre;
                                                withBlock3.ServiceID = ttServiceID;
                                                withBlock3.Request = strRequest;
                                                withBlock3.ProviderSystems = ttProviderSystems;
                                                withBlock3.Version = "";
                                            }

                                            // oThreadSabre.Start()
                                            DoSabreSearches[j] = new SearchHotelSabre(withBlock.Providers[i].PCC, withBlock.UserID, withBlock.System, ref ttProviderSystems, ref oSabre);
                                            DoSabreSearches[j].Request = strRequest;
                                            DoSabreSearches[j].ServiceID = ((int)ttServiceID).ToString();
                                            DoSabreSearches[j].BeginSearch();
                                        }
                                        catch (Exception e)
                                        {
                                            GotResponse(FormatErrorMessage(ttServiceID, e.Message, withBlock.Providers[i].Name));
                                        }

                                        break;
                                    }

                                default:
                                    {
                                        GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Provider ").Append(withBlock.Providers[i].Name).Append(" Not Currently Supported.").ToString(), withBlock.Providers[i].Name));
                                        sb.Remove(0, sb.Length);
                                        break;
                                    }
                            }
                            j += 1;
                        }
                    }
                }

                var StartCounter = DateTime.Now;

                // Do While mintProviders < ttCredential.Providers.Length
                while (mintProviders < j)
                {
                    if ((int)Math.Round(DateTime.Now.Subtract(StartCounter).TotalSeconds) > modMain.CPrdTimeOut)
                        break;
                    System.Threading.Thread.Sleep(10);
                }

                // If ttCredential.Providers.Length > 1 Then
                if (j > 1)
                {
                    strResponse = string.Concat("<SuperRS>", mstrResponse, "</SuperRS>");
                    // Aggregate
                    cAggregation.Aggregate(ttServiceID, XslPath, "", ref strResponse);
                }
                else
                {
                    strResponse = mstrResponse;
                }

                StartCounter = DateTime.Now;
                strResponse = DecodeHotelAvail(strResponse, ttCredential.UserID);
                CoreLib.SendTrace(ttCredential.UserID, "Performance", sb.Append("Decoding = ").Append((int)Math.Round(DateTime.Now.Subtract(StartCounter).TotalMilliseconds)).ToString(), "", UUID);
                sb.Remove(0, sb.Length);

                modMain.PostServiceRequest(ref strResponse, ValidateXSDOut, (int)ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, "");
            }
            finally
            {
                _modMain.LogResponse(ref strResponse, ref ttCredential, StartTime, (int)ttServiceID, Environment.MachineName, ref UUID);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsHotelAvail_v03", "============= OTA Response ============= ", strResponse, UUID);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Web Methods 
        public wmHotelAvailOut_v03.OTA_HotelAvailRS wmHotelAvail(wmHotelAvailIn_v03.OTA_HotelAvailRQ OTA_HotelAvailRQ)
        {
            string xmlMessage = "";
            wmHotelAvailOut_v03.OTA_HotelAvailRS oHotelAvailRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;
            oSerializer = new XmlSerializer(typeof(wmHotelAvailIn_v03.OTA_HotelAvailRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_HotelAvailRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.HotelAvail);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmHotelAvailOut_v03.OTA_HotelAvailRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oHotelAvailRS = (wmHotelAvailOut_v03.OTA_HotelAvailRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsHotelAvail_v03", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oHotelAvailRS;

        }
        public string wmHotelAvailXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.HotelAvail);
        }

        #endregion

    }

    #region Search Amadeus
    // Public Class SearchHotelAmadeus
    // Private Delegate Sub StartSearch_Delegate()
    // Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoAmadeusSearch)
    // Private pcc As String = ""
    // Private userid As String = ""
    // Private System As String = ""
    // Private ttAA As AmadeusAPIAdapter
    // Private _ServiceID As String = ""
    // Private _Request As String = ""
    // Private oAmadeus As cServiceAmadeus

    // Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttAA As AmadeusAPIAdapter, ByRef _oAmadeus As cServiceAmadeus)
    // Me.pcc = _pcc
    // Me.userid = _userid
    // Me.System = _System
    // Me.ttAA = _ttAA
    // Me.oAmadeus = _oAmadeus
    // End Sub
    // Public Property ServiceID() As String
    // Get
    // Return _ServiceID
    // End Get
    // Set(ByVal value As String)
    // _ServiceID = value
    // End Set
    // End Property
    // Public Property Request() As String
    // Get
    // Return _Request
    // End Get
    // Set(ByVal value As String)
    // _Request = value
    // End Set
    // End Property
    // Public Sub BeginSearch()
    // Dim cbr As AsyncCallback = New AsyncCallback(AddressOf EndSearch)
    // Dim arr As IAsyncResult = StartSearch_Wrapper.BeginInvoke(cbr, Nothing)
    // End Sub
    // Private Sub EndSearch(ByVal asy As IAsyncResult)
    // StartSearch_Wrapper.EndInvoke(asy)
    // asy.AsyncWaitHandle.Close()
    // End Sub
    // Private Sub DoAmadeusSearch()
    // ttAA.SourcePCC = Me.pcc
    // oAmadeus.SendHotelRequest()
    // oAmadeus = Nothing
    // End Sub
    // End Class
    #endregion

    #region Search AmadeusWS
    public class SearchHotelAmadeusWS
    {
        private delegate void StartSearch_Delegate();
        private StartSearch_Delegate StartSearch_Wrapper;
        private string pcc = "";
        private string userid = "";
        private string System = "";
        private TripXMLProviderSystems ttProviderSystems;
        private string _ServiceID = "";
        private string _Request = "";
        private cServiceAmadeusWS oAmadeusWS;

        public SearchHotelAmadeusWS(string _pcc, string _userid, string _System, ref TripXMLProviderSystems _ttProviderSystems, ref cServiceAmadeusWS _oAmadeusWS)
        {
            StartSearch_Wrapper = new StartSearch_Delegate(DoAmadeusSearchWS);
            pcc = _pcc;
            userid = _userid;
            System = _System;
            ttProviderSystems = _ttProviderSystems;
            oAmadeusWS = _oAmadeusWS;
        }
        public string ServiceID
        {
            get
            {
                return _ServiceID;
            }
            set
            {
                _ServiceID = value;
            }
        }
        public string Request
        {
            get
            {
                return _Request;
            }
            set
            {
                _Request = value;
            }
        }
        public void BeginSearch()
        {
            var cbr = new AsyncCallback(EndSearch);
            var arr = StartSearch_Wrapper.BeginInvoke(cbr, null);
        }
        private void EndSearch(IAsyncResult asy)
        {
            StartSearch_Wrapper.EndInvoke(asy);
            asy.AsyncWaitHandle.Close();
        }
        private void DoAmadeusSearchWS()
        {

            ttProviderSystems.PCC = pcc;
            oAmadeusWS.SendHotelRequest();
            oAmadeusWS = null;
        }
    }
    #endregion

    #region Search Galileo
    public class SearchHotelGalileo
    {
        private delegate void StartSearch_Delegate();
        private StartSearch_Delegate StartSearch_Wrapper;
        private string pcc = "";
        private string userid = "";
        private string System = "";
        private TripXMLProviderSystems ttProviderSystems;
        private string _ServiceID = "";
        private string _Request = "";
        private cServiceGalileo oGalileo;
        public SearchHotelGalileo(string _pcc, string _userid, string _System, ref TripXMLProviderSystems _ttProviderSystems, ref cServiceGalileo _oGalileo)
        {
            StartSearch_Wrapper = new StartSearch_Delegate(DoGalileoSearch);
            pcc = _pcc;
            userid = _userid;
            System = _System;
            ttProviderSystems = _ttProviderSystems;
            oGalileo = _oGalileo;
        }
        public string ServiceID
        {
            get
            {
                return _ServiceID;
            }
            set
            {
                _ServiceID = value;
            }
        }
        public string Request
        {
            get
            {
                return _Request;
            }
            set
            {
                _Request = value;
            }
        }
        public void BeginSearch()
        {
            var cbr = new AsyncCallback(EndSearch);
            var arr = StartSearch_Wrapper.BeginInvoke(cbr, null);
        }
        private void EndSearch(IAsyncResult asy)
        {
            StartSearch_Wrapper.EndInvoke(asy);
            asy.AsyncWaitHandle.Close();
        }
        private void DoGalileoSearch()
        {
            ttProviderSystems.PCC = pcc;
            oGalileo.SendHotelRequest();
            oGalileo = null;
        }
    }
    #endregion

    #region Search Sabre
    public class SearchHotelSabre
    {
        private delegate void StartSearch_Delegate();
        private StartSearch_Delegate StartSearch_Wrapper;
        private string pcc = "";
        private string userid = "";
        private string System = "";
        private TripXMLProviderSystems ttProviderSystems;
        private string _ServiceID = "";
        private string _Request = "";
        private cServiceSabre oSabre;

        public SearchHotelSabre(string _pcc, string _userid, string _System, ref TripXMLProviderSystems _ttProviderSystems, ref cServiceSabre _oSabre)
        {
            StartSearch_Wrapper = new StartSearch_Delegate(DoSabreSearchWS);
            pcc = _pcc;
            userid = _userid;
            System = _System;
            ttProviderSystems = _ttProviderSystems;
            oSabre = _oSabre;
        }
        public string ServiceID
        {
            get
            {
                return _ServiceID;
            }
            set
            {
                _ServiceID = value;
            }
        }
        public string Request
        {
            get
            {
                return _Request;
            }
            set
            {
                _Request = value;
            }
        }
        public void BeginSearch()
        {
            var cbr = new AsyncCallback(EndSearch);
            var arr = StartSearch_Wrapper.BeginInvoke(cbr, null);
        }
        private void EndSearch(IAsyncResult asy)
        {
            StartSearch_Wrapper.EndInvoke(asy);
            asy.AsyncWaitHandle.Close();
        }
        private void DoSabreSearchWS()
        {
            ttProviderSystems.PCC = pcc;
            oSabre.SendHotelRequest();
            oSabre = null;
        }
    }
    #endregion

    #region Search Worldspan
    // Public Class SearchHotelWorldspan
    // Private Delegate Sub StartSearch_Delegate()
    // Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoAmadeusSearchWS)
    // Private pcc As String = ""
    // Private userid As String = ""
    // Private System As String = ""
    // Private ttProviderSystems As TripXMLProviderSystems
    // Private _ServiceID As String = ""
    // Private _Request As String = ""
    // Private oWorldspan As cServiceWorldspan

    // Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oWorldspan As cServiceWorldspan)
    // Me.pcc = _pcc
    // Me.userid = _userid
    // Me.System = _System
    // Me.ttProviderSystems = _ttProviderSystems
    // Me.oWorldspan = _oWorldspan
    // End Sub
    // Public Property ServiceID() As String
    // Get
    // Return _ServiceID
    // End Get
    // Set(ByVal value As String)
    // _ServiceID = value
    // End Set
    // End Property
    // Public Property Request() As String
    // Get
    // Return _Request
    // End Get
    // Set(ByVal value As String)
    // _Request = value
    // End Set
    // End Property
    // Public Sub BeginSearch()
    // Dim cbr As AsyncCallback = New AsyncCallback(AddressOf EndSearch)
    // Dim arr As IAsyncResult = StartSearch_Wrapper.BeginInvoke(cbr, Nothing)
    // End Sub
    // Private Sub EndSearch(ByVal asy As IAsyncResult)
    // StartSearch_Wrapper.EndInvoke(asy)
    // asy.AsyncWaitHandle.Close()
    // End Sub
    // Private Sub DoAmadeusSearchWS()
    // ttProviderSystems.PCC = Me.pcc
    // oWorldspan.SendHotelRequest()
    // oWorldspan = Nothing
    // End Sub
    // End Class
    #endregion


}