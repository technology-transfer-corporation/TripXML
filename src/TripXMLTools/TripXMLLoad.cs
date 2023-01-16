using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Configuration;
using System.Xml.Linq;
using static TripXMLMain.modCore;

namespace TripXMLTools
{
    public class TripXMLLoad
    {
        public static List<Txuser> UsersObject { get; set; }
        private static string Requestor { get; set; }
        public static Decoding DecodingTables { get; set; }
        
        public static void TripXMLLoadObject()
        {
            if (UsersObject == null)
            {
                UsersObject = GetProvidersObject(new { id = new Guid(WebConfigurationManager.AppSettings["ServerGuid"]) });
            }
        }

        private static List<Txuser> GetProvidersObject(object rqObject)
        {
            if (rqObject is null)
            {
                throw new ArgumentNullException(nameof(rqObject));
            }
            
            var body = JsonConvert.SerializeObject(rqObject);
            var _response = GetServerData($"{WebConfigurationManager.AppSettings["HasuraEndpoint"]}/tripxmlload", body);

            var providers = JsonConvert.DeserializeObject<TripXMLProviderDTO>(_response);

            if (providers != null)
            {
                Requestor = providers.Servers[0].CustomerServers[0].Customer.RequestorId;
                return providers.Servers[0].CustomerServers[0].Customer.TxUsers;
            }
            else
                return default;
        }

        public static void GetDecodingTables()
        {
            if (DecodingTables == null)
            {
                var _response = GetServerData($"{WebConfigurationManager.AppSettings["HasuraEndpoint"]}/decoding");
                var decoded = JsonConvert.DeserializeObject<Decoding>(_response);                

                DecodingTables = decoded;
            }
        }

        private static string GetServerData(string url, string body = "")
        {
            try
            {
                var hasuraKey = WebConfigurationManager.AppSettings["HasuraKey"];

                var client = new HttpClient();
                var request = string.IsNullOrEmpty(body) ? new HttpRequestMessage(HttpMethod.Get, url) : new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("x-hasura-admin-secret", hasuraKey);

                if (!string.IsNullOrEmpty(body))
                    request.Content = new StringContent(body, Encoding.UTF8, "application/json");

                var response = client.SendAsync(request);
                var _response = response.Result.Content.ReadAsStringAsync().Result;

                return _response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static TravelTalkCredential GetTravelTalkCredential(string strRequest, int id)
        {
            if (UsersObject == null)
                TripXMLLoadObject();

            if (string.IsNullOrEmpty(strRequest))
                return default;

            var credentials = new TravelTalkCredential();
            credentials.Providers = new TripXMLMain.modCore.Provider[1];
            var doc = XDocument.Parse(strRequest);
            var pos = doc.Descendants("POS");

            var source = pos.Descendants("Source").ToList().First();
            TravelTalkCredential.RequestorID = source.Element("RequestorID").Attribute("ID").Value;
            credentials.Providers[0].PCC = source.Attribute("PseudoCityCode").Value;

            var provider = pos.Descendants("Provider").ToList().First();
            credentials.UserID = provider.Element("Userid").Value;
            credentials.Password = provider.Element("Password").Value;
            credentials.System = provider.Element("System").Value;
            credentials.Providers[0].Name = provider.Element("Name").Value;

            return credentials;
        }

        public static void GetProviderSystem(ref TripXMLProviderSystems ttProviderSystem, TravelTalkCredential credentials)
        {
            try
            {
                var user = UsersObject.Find(p => p.Username == credentials.UserID && p.Password == credentials.Password);
                var provider = user.ProviderUsers.FirstOrDefault(u => u.Provider.System.Environment == credentials.System
                    && u.Provider.Name == credentials.Providers[0].Name);
                var providerPcc = provider.Provider.PCCs[0];

                switch (credentials.Providers[0].Name)
                {
                    case "Sabre":
                        ttProviderSystem.PCC = providerPcc.Code;
                        ttProviderSystem.AAAPCC = providerPcc.Code;
                        break;
                    case "Amadeus":
                        ttProviderSystem.AmadeusWS = credentials.Providers[0].Name == "Amadeus";
                        ttProviderSystem.AmadeusWSSchema = new Dictionary<enAmadeusWSSchema, string>();
                        foreach (var schema in providerPcc?.Wsdlschemas)
                        {
                            Enum.TryParse(schema.Name, out enAmadeusWSSchema name);
                            if (ttProviderSystem.AmadeusWSSchema.ContainsKey(name))
                                continue;
                            ttProviderSystem.AmadeusWSSchema.Add(name, schema.Version);
                        }
                        break;
                    default:
                        ttProviderSystem.PCC = credentials.Providers[0].PCC;
                        break;
                }

                ttProviderSystem.System = credentials.System;
                ttProviderSystem.Provider = provider.Provider.Name;
                ttProviderSystem.UserID = credentials.UserID;
                ttProviderSystem.Password = providerPcc.Password;
                ttProviderSystem.UserName = providerPcc.Username;
                ttProviderSystem.SOAP2 = providerPcc.SOAPType.Equals("SOAP2");
                ttProviderSystem.SOAP4 = !ttProviderSystem.SOAP2;
                ttProviderSystem.Profile.Origin = providerPcc.Profile.Origin;
                ttProviderSystem.Profile.Xml = providerPcc.Profile.Xml;
                ttProviderSystem.Profile.Text = providerPcc.Profile.Text;
                ttProviderSystem.Profile.Cryptic = providerPcc.Profile.Cryptic;
                ttProviderSystem.Profile.Ticketing = providerPcc.Profile.Ticketing;
                ttProviderSystem.GReqID = Requestor;
                ttProviderSystem.AggFilter = true;
                ttProviderSystem.FareMessage = "VP";
                ttProviderSystem.URL = provider.Provider.System.URL;
                ttProviderSystem.SOAP4URL = provider.Provider.System.SOAP4Url;
                ttProviderSystem.ProxyURL = "";
                ttProviderSystem.BLFile = "";
                ttProviderSystem.System = provider.Provider.System.Environment;
                //Default Values
                ttProviderSystem.GetStoredFares = true;
                ttProviderSystem.CheckBookedFare = false;
                ttProviderSystem.AmadeusTrace = false;
                ttProviderSystem.RebookNextFlight = false;
                ttProviderSystem.LogNative = false;
                ttProviderSystem.SessionPool = false;
                ttProviderSystem.AddLog = false;
                ttProviderSystem.HotelMedia = false;
                ttProviderSystem.SendEmailToAgency = false;
                ttProviderSystem.CreateInRHAdmin = false;
                ttProviderSystem.LFPLight = false;
                ttProviderSystem.CouponStatus = false;
                ttProviderSystem.AddLFPStat = false;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string EncodeValue(DecodingType type, string code)
        {
            if (DecodingTables == null)
                GetDecodingTables();

            switch (type)
            {
                case DecodingType.Airline:
                    return DecodingTables.Airlines.FirstOrDefault(c => c.Name == code)?.Code;
                case DecodingType.Airport:
                    return DecodingTables.Airports.FirstOrDefault(c => c.Name == code)?.Code;
                case DecodingType.Equipment:
                    return DecodingTables.Equipments.FirstOrDefault(c => c.Name == code)?.Code;
                case DecodingType.CreditCard:
                    return DecodingTables.Creditcards.FirstOrDefault(c => c.Name == code)?.Code;
                case DecodingType.Hotel:
                    return DecodingTables.Hotels.FirstOrDefault(c => c.Name == code)?.Code;
                case DecodingType.HotelAmenity:
                    return DecodingTables.Hotelamenity.FirstOrDefault(c => c.Name == code)?.Code;
                case DecodingType.HotelArea:
                    return DecodingTables.Hotelarea.FirstOrDefault(c => c.Name == code)?.Code;
                case DecodingType.HotelRoom:
                    return DecodingTables.Hotelrooms.FirstOrDefault(c => c.Name == code)?.Code;
                case DecodingType.HotelSubtitle:
                    return DecodingTables.Hotelsubtitle.FirstOrDefault(c => c.Name == code)?.Code;
                default:
                    return string.Empty;
            }
        }

        public static string DecodeValue(DecodingType type, string code)
        {
            if (DecodingTables == null)
                GetDecodingTables();

            switch (type)
            {
                case DecodingType.Airline:
                    return DecodingTables.Airlines.FirstOrDefault(c => c.Code == code)?.Name;
                case DecodingType.Airport:
                    return DecodingTables.Airports.FirstOrDefault(c => c.Code == code)?.Name;
                case DecodingType.Equipment:
                    return DecodingTables.Equipments.FirstOrDefault(c => c.Code == code)?.Name;
                case DecodingType.CreditCard:
                    return DecodingTables.Creditcards.FirstOrDefault(c => c.Code == code)?.Name;
                case DecodingType.Hotel:
                    return DecodingTables.Hotels.FirstOrDefault(c => c.Code == code)?.Name;
                case DecodingType.HotelAmenity:
                    return DecodingTables.Hotelamenity.FirstOrDefault(c => c.Code == code)?.Name;
                case DecodingType.HotelArea:
                    return DecodingTables.Hotelarea.FirstOrDefault(c => c.Code == code)?.Name;
                case DecodingType.HotelRoom:
                    return DecodingTables.Hotelrooms.FirstOrDefault(c => c.Code == code)?.Name;
                case DecodingType.HotelSubtitle:
                    return DecodingTables.Hotelsubtitle.FirstOrDefault(c => c.Code == code)?.Name;
                default:
                    return string.Empty;
            }
        }

        public enum DecodingType
        {
            Airline,
            City,
            Airport,
            CarCompany,
            CarType,
            CreditCard,
            CruiseAdvisory,
            CruiseBadConfiguration,
            CruiseCabinFilter,
            CruiseCity,
            CruiseCurrency,
            CruiseInsuarence,
            CruiseLine,
            CruisePriceItem,
            CruiseMot,
            CruiseOccupation,
            CruisePaxTitle,
            CruiseProfile,
            CruiseRegion,
            CruiseShip,
            Equipment,
            HotelAmenity,
            HotelArea,
            HotelRoom,
            Hotel,
            HotelSubtitle,
            WsCruiseCity,
            WsCruisePaxTitle
        }
        public class Decoding
        {
            public List<Airline> Airlines { get; set; }
            public List<Airport> Airports { get; set; }
            public List<City> Cities { get; set; }
            public List<Equipment> Equipments { get; set; }
            public List<Creditcard> Creditcards { get; set; }
            public List<Hotelamenity> Hotelamenity { get; set; }
            public List<Hotelarea> Hotelarea { get; set; }
            public List<Hotelroom> Hotelrooms { get; set; }
            public List<Hotelsubtitle> Hotelsubtitle { get; set; }
            public List<Hotel> Hotels { get; set; }
        }
        public class Airline : DecodingBase
        {
            public string ICAO { get; set; }
        }
        public class Airport : DecodingBase { }
        public class City
        {
            public string CityAirport { get; set; }
        }
        public class Equipment : DecodingBase { }
        public class Creditcard : DecodingBase { }
        public class Hotelamenity : DecodingBase { }
        public class Hotelarea : DecodingBase { }
        public class Hotelroom : DecodingBase { }
        public class Hotelsubtitle : DecodingBase { }
        public class Hotel : DecodingBase { }

        public abstract class DecodingBase
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }
    }
}





