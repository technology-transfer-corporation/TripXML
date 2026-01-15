using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;
using static TripXMLTools.TripXMLLoad;

namespace wsTripXML.wsTravelTalk
{


    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsCarAvail", Name = "wsCarAvail_v03", Description = "A TripXML Web Service to Process Car Availability Messages Request version 03.")]
    public class wsCarAvail_v03 : WebService
    {
        private StringBuilder sb = new StringBuilder();

        #region  Web Services Designer Generated Code 

        public wsCarAvail_v03() : base()
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

        #region  Decode Function 

        private string DecodeCarAvail(string strResponse, string UserID)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            DataView ttAirports;
            DataView ttCars;
            DataView ttCarTypes;
            XmlNode oNode = null;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                ttAirports = (DataView)Application.Get("ttAirports");
                ttCars = (DataView)Application.Get("ttCars");
                ttCarTypes = (DataView)Application.Get("ttCarTypes");

                foreach (XmlNode currentONode in oRoot.SelectNodes("VehAvailRSCore/VehRentalCore"))
                {
                    oNode = currentONode;
                    // *******************
                    // Decode Airports   *
                    // *******************
                    if (oNode.SelectSingleNode("PickUpLocation") is not null)
                    {
                        oNode.SelectSingleNode("PickUpLocation").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("PickUpLocation").Attributes["LocationCode"].Value);
                    }
                    if (oNode.SelectSingleNode("ReturnLocation") is not null)
                    {
                        oNode.SelectSingleNode("ReturnLocation").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("ReturnLocation").Attributes["LocationCode"].Value);
                    }
                }
                foreach (XmlNode currentONode1 in oRoot.SelectNodes("VehAvailRSCore/VehVendorAvails/VehVendorAvail"))
                {
                    oNode = currentONode1;
                    // *******************
                    // Decode Cars   *
                    // *******************
                    if (oNode.SelectSingleNode("Vendor") is not null)
                    {
                        oNode.SelectSingleNode("Vendor").InnerText = DecodeValue(DecodingType.CarCompany, oNode.SelectSingleNode("Vendor").Attributes["Code"].Value);
                    }
                }
                foreach (XmlNode currentONode2 in oRoot.SelectNodes("VehAvailRSCore/VehVendorAvails/VehVendorAvail/VehAvails/VehAvail/VehAvailCore/Vehicle"))
                {
                    oNode = currentONode2;
                    // *******************
                    // Decode CarTypes *
                    // *******************
                    if (oNode.SelectSingleNode("VehType") is not null)
                    {
                        oNode.SelectSingleNode("VehType").InnerText = DecodeValue(DecodingType.CarType, oNode.SelectSingleNode("VehType").Attributes["VehicleCategory"].Value);
                    }
                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsCarAvail", "Error *** Decoding CarAvail Response", ex.Message, string.Empty);
            }
            return strResponse;
        }

        #endregion

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
            // Dim DoAmadeusSearches(99) As SearchCarAmadeus
            var DoAmadeusWSSearches = new SearchCarAmadeusWS[100];
            var DoGalileoSearches = new SearchCarGalileo[100];
            var DoSabreSearches = new SearchCarSabre[100];
            // Dim DoWorldspanSearches(99) As SearchHotelWorldspan
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            XmlNode oNodeNewVendorPrefs = null;
            XmlDocument oDocVendorPrefs = null;
            XmlElement oRootVendorPrefs = null;
            XmlNode oNodeVendorPrefs = null;
            string strVendorPrefs;
            int j = 0;

            try
            {
                StartTime = DateTime.Now;

                var argoApp = Application;
                modMain.PreServiceRequestPool(ref strRequest, ref argoApp, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Server.MachineName, ref UUID);
                ValidateXSDOut = Conversions.ToBoolean(Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                oRoot = oDoc.DocumentElement;
                oNodeNewVendorPrefs = oRoot.SelectSingleNode("VehAvailRQCore/VendorPrefs");
                strVendorPrefs = oNodeNewVendorPrefs.OuterXml;
                oDocVendorPrefs = new XmlDocument();
                oDocVendorPrefs.LoadXml(strVendorPrefs);
                oRootVendorPrefs = oDocVendorPrefs.DocumentElement;
                oNodeNewVendorPrefs.RemoveAll();

                {
                    ref var withBlock = ref ttCredential;
                    var loopTo = withBlock.Providers.Length - 1;
                    for (i = 0; i <= loopTo; i++)
                    {
                        foreach (XmlNode oNodeCriterion in oRootVendorPrefs.SelectNodes("VendorPref"))
                        {
                            oNodeNewVendorPrefs.InnerXml = oNodeCriterion.OuterXml;
                            strRequest = oRoot.OuterXml;
                            oNodeNewVendorPrefs.RemoveAll();

                            switch (withBlock.Providers[i].Name.ToLower() ?? "")
                            {
                                case "amadeus":
                                case "amadeusws":
                                    {
                                        try
                                        {
                                            // Dim ttAA As AmadeusAPIAdapter
                                            withBlock.Providers[i].Name = withBlock.Providers[i].Name.Replace("AmadeusWS", "Amadeus");
                                            // ttAA = Application.Get(sb.Append("API").Append(.UserID).Append(.System).Append(.Providers(i).PCC).ToString())
                                            // sb.Remove(0, sb.Length())
                                            // If ttAA Is Nothing Then
                                            // ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(i).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(i).PCC).ToString())
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
                                                // Dim oThreadAmadeusWS As New Thread(New ThreadStart(AddressOf oAmadeusWS.SendCarRequest))
                                                oAmadeusWS.GotResponse += GotResponse;

                                                {
                                                    ref var withBlock1 = ref oAmadeusWS;
                                                    withBlock1.ServiceID = (int)ttServiceID;
                                                    withBlock1.Request = strRequest;
                                                    withBlock1.ttProviderSystems = ttProviderSystems;
                                                    withBlock1.Version = "";
                                                }
                                                // oThreadAmadeusWS.Start()
                                                DoAmadeusWSSearches[j] = new SearchCarAmadeusWS(withBlock.Providers[i].PCC, withBlock.UserID, withBlock.System, ref ttProviderSystems, ref oAmadeusWS);
                                                DoAmadeusWSSearches[j].Request = strRequest;
                                                DoAmadeusWSSearches[j].ServiceID = ((int)ttServiceID).ToString();
                                                DoAmadeusWSSearches[j].BeginSearch();
                                                ttProviderSystems = default;
                                                // Else
                                                // ttProviderSystems = ttAA.ttProviderSystems
                                                // If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                                // ttAA.SourcePCC = ttCredential.Providers(i).PCC
                                                // End If

                                                // Dim oAmadeus As New cServiceAmadeus
                                                // 'Dim oThreadAmadeus As New Thread(New ThreadStart(AddressOf oAmadeus.SendCarRequest))
                                                // AddHandler oAmadeus.GotResponse, AddressOf GotResponse

                                                // With oAmadeus
                                                // .ServiceID = ttServiceID
                                                // .Request = strRequest
                                                // .ttAA = ttAA
                                                // .Version = ""
                                                // End With
                                                // 'oThreadAmadeus.Start()

                                                // DoAmadeusSearches(j) = New SearchCarAmadeus(.Providers(i).PCC, .UserID, .System, ttAA, oAmadeus)
                                                // DoAmadeusSearches(j).Request = strRequest
                                                // DoAmadeusSearches(j).ServiceID =CInt(ttServiceID).ToString()
                                                // DoAmadeusSearches(j).BeginSearch()

                                                // Application.Set(sb.Append("API").Append(.UserID).Append(.System).ToString(), ttAA)
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
                                            ttProviderSystems = (TripXMLProviderSystems)Application.Get(sb.Append("PS").Append(withBlock.Providers[i].Name).Append(withBlock.UserID).Append(withBlock.System).Append(withBlock.Providers[i].PCC).ToString());
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
                                            // Dim oThreadGalileo As New Thread(New ThreadStart(AddressOf oGalileo.SendCarRequest))
                                            oGalileo.GotResponse += GotResponse;

                                            {
                                                ref var withBlock2 = ref oGalileo;
                                                withBlock2.ServiceID = (int)ttServiceID;
                                                withBlock2.Request = strRequest;
                                                withBlock2.ProviderSystems = ttProviderSystems;
                                                withBlock2.Version = "";
                                            }
                                            // oThreadGalileo.Start()
                                            DoGalileoSearches[j] = new SearchCarGalileo(withBlock.Providers[i].PCC, withBlock.UserID, withBlock.System, ref ttProviderSystems, ref oGalileo);
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
                                            ttProviderSystems = (TripXMLProviderSystems)Application.Get(sb.Append("PS").Append(withBlock.Providers[i].Name).Append(withBlock.UserID).Append(withBlock.System).Append(withBlock.Providers[i].PCC).ToString());
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
                                            // Dim oThreadSabre As New Thread(New ThreadStart(AddressOf oSabre.SendCarRequest))
                                            oSabre.GotResponse += GotResponse;

                                            {
                                                ref var withBlock3 = ref oSabre;
                                                withBlock3.ServiceID = ttServiceID;
                                                withBlock3.Request = strRequest;
                                                withBlock3.ProviderSystems = ttProviderSystems;
                                                withBlock3.Version = "";
                                            }
                                            // oThreadSabre.Start()
                                            DoSabreSearches[j] = new SearchCarSabre(withBlock.Providers[i].PCC, withBlock.UserID, withBlock.System, ref ttProviderSystems, ref oSabre);
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
                strResponse = DecodeCarAvail(strResponse, ttCredential.UserID);
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
                modMain.LogResponse(ref strResponse, ref ttCredential, StartTime, (int)ttServiceID, Server.MachineName, ref UUID);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsCarAvail_v03", "============= OTA Response ============= ", strResponse, UUID);
            }

            return strResponse;
            sb = null;
        }

        #endregion

        #region  Web Methods 

        [WebMethod(Description = "Process Car Availability Messages Request.")]
        public wmCarAvailOut.OTA_VehAvailRateRS wmCarAvail(wmCarAvailIn.OTA_VehAvailRateRQ OTA_VehAvailRateRQ)
        {
            string xmlMessage = "";
            wmCarAvailOut.OTA_VehAvailRateRS oCarAvailRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmCarAvailIn.OTA_VehAvailRateRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_VehAvailRateRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.CarAvail);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmCarAvailOut.OTA_VehAvailRateRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oCarAvailRS = (wmCarAvailOut.OTA_VehAvailRateRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsCarAvail", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oCarAvailRS;

        }

        [WebMethod(Description = "Process Car Availability Xml Messages Request.")]
        public string wmCarAvailXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.CarAvail);
        }

        #endregion

    }

    #region Search Amadeus
    // Public Class SearchCarAmadeus
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
    // oAmadeus.SendCarRequest()
    // oAmadeus = Nothing
    // End Sub
    // End Class
    #endregion

    #region Search AmadeusWS
    public class SearchCarAmadeusWS
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

        public SearchCarAmadeusWS(string _pcc, string _userid, string _System, ref TripXMLProviderSystems _ttProviderSystems, ref cServiceAmadeusWS _oAmadeusWS)
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
            oAmadeusWS.SendCarRequest();
            oAmadeusWS = null;
        }
    }
    #endregion

    #region Search Galileo
    public class SearchCarGalileo
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
        public SearchCarGalileo(string _pcc, string _userid, string _System, ref TripXMLProviderSystems _ttProviderSystems, ref cServiceGalileo _oGalileo)
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
            oGalileo.SendCarRequest();
            oGalileo = null;
        }
    }
    #endregion

    #region Search Sabre
    public class SearchCarSabre
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

        public SearchCarSabre(string _pcc, string _userid, string _System, ref TripXMLProviderSystems _ttProviderSystems, ref cServiceSabre _oSabre)
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
            oSabre.SendCarRequest();
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