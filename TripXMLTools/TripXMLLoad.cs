using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private static RestClient _restClient => new RestClient(WebConfigurationManager.AppSettings["HasuraEndpoint"]);
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

            var request = new RestRequest("tripxmlload");
            request.AddJsonBody(rqObject);

            var response = _restClient.AddDefaultHeader("x-hasura-admin-secret", WebConfigurationManager.AppSettings["HasuraKey"]).Post(request);
            if (response.ErrorMessage != null)
                throw new Exception(response.ErrorMessage);

            var providers = JsonConvert.DeserializeObject<TripXMLProviderDTO>(response.Content);

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
                var client = _restClient;
                var request = new RestRequest("decoding");

                var response = client.AddDefaultHeader("x-hasura-admin-secret", WebConfigurationManager.AppSettings["HasuraKey"]).Get<Decoding>(request);
                if (response.ErrorMessage != null)
                    throw new Exception(response.ErrorMessage);

                DecodingTables = client.Get<Decoding>(request).Data;
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
                var providerPcc = provider.Provider.PCCs.FindAll(p=> p.Code.Equals(credentials.Providers[0].PCC)).FirstOrDefault();

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
                ttProviderSystem.Profile = providerPcc.Profiles.FirstOrDefault().Text;
                ttProviderSystem.Password = providerPcc.Password;
                ttProviderSystem.UserName = providerPcc.Username;
                ttProviderSystem.SOAP2 = providerPcc.SOAPType.Equals("SOAP2");
                ttProviderSystem.SOAP4 = !ttProviderSystem.SOAP2;
                ttProviderSystem.Origin = "NMC-US";
                ttProviderSystem.GReqID = Requestor;
                ttProviderSystem.AggFilter = true;
                ttProviderSystem.FareMessage = "VP";
                ttProviderSystem.URL = provider.Provider.System.URL;
                ttProviderSystem.SOAP4URL = provider.Provider.System.SOAP4Url;
                ttProviderSystem.ProxyURL = "";
                ttProviderSystem.BLFile = "";
                ttProviderSystem.System = provider.Provider.System.Environment;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
        public class Hotelsubtitle : DecodingBase {}
        public class Hotel : DecodingBase { }

        public abstract class DecodingBase
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }
    }
}





