using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        static SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        public static void TripXMLLoadObject(bool isRefresh = false)
        {
            if (UsersObject == null || isRefresh)
            {
                UsersObject = GetProvidersObject(new { id = new Guid(WebConfigurationManager.AppSettings["ServerGuid"]) });
            }
        }

        public static async Task<UpdateCacheResponse> UpdateCachedObjects()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                TripXMLLoadObject(true);
                GetDecodingTables(true);

                return new UpdateCacheResponse
                {
                    Updated = true
                };
            }
            catch (Exception ex)
            {
                return new UpdateCacheResponse
                {
                    Updated = false,
                    Message = ex.Message
                };
            }
            finally
            {
                _semaphoreSlim.Release();
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

        public static void GetDecodingTables(bool isRefresh = false)
        {
            if (DecodingTables == null || isRefresh)
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
                var providerPcc = credentials.Providers.Count().Equals(0)
                    ? provider.Provider.PCCs[0]
                    : provider.Provider.PCCs.Find(p => p.Code == credentials.Providers.First().PCC || p.OpenTypes.Exists(ot => ot.OfficeID.Equals(credentials.Providers.First().PCC)));
                //  : provider.Provider.PCCs.Select(p => new Pcc { Code = p.OpenTypes.Find(ot => ot.OfficeID.Equals(credentials.Providers.First().PCC)).OfficeID });
                switch (credentials.Providers[0].Name)
                {
                    case "Sabre":
                    case "Galileo":
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
                ttProviderSystem.SOAP2 = providerPcc.SOAPType == null ? false : providerPcc.SOAPType.Equals("SOAP2");
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
                    var _code = DecodingTables.Airlines.FirstOrDefault(c => c.Name == code)?.Code;
                    if (string.IsNullOrEmpty(_code))
                        _code = EvaluateName(code);

                    return _code;
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

        private static string EvaluateName(string code)
        {

            var _code = string.Empty;

            if (string.IsNullOrEmpty(code))
                return string.Empty;

            if (code.Contains("OPERATED BY"))
                code = code.Replace("OPERATED BY ", "");

            if (DecodingTables.Airlines.FindAll(c => c.Name == code).Count.Equals(1))
            {
                _code = DecodingTables.Airlines.FirstOrDefault(c => c.Name == code)?.Code;
                if (!string.IsNullOrEmpty(_code))
                    return _code;
            }

            if (DecodingTables.Airlines.FindAll(c => c.Name.Contains(code)).Count.Equals(1))
            {
                _code = DecodingTables.Airlines.FirstOrDefault(c => c.Name.Contains(code))?.Code;
                if (!string.IsNullOrEmpty(_code))
                    return _code;
            }

            if (DecodingTables.Airlines.FindAll(c => c.Name.StartsWith(code)).Count.Equals(1))
            {
                _code = DecodingTables.Airlines.FirstOrDefault(c => c.Name.StartsWith(code))?.Code;
                if (!string.IsNullOrEmpty(_code))
                    return _code;
            }
            
            if (code.Length.Equals(2))
                return code;

            /******************************
            'Try to cut Airline Name
            'Example: Trans American Airlines F Ta
            'But we need to look only at: Trans American Airlines
            '******************************/
            code = code.ToUpper().Replace("AIR LINES", "AIRLINES");
            var _airlines = code.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var shortName = string.Empty;

            if (code.ToUpper().Contains(" AS "))
            {
                var airlines = code.Split(new[] { " AS " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                _code = DecodingTables.Airlines.FirstOrDefault(c => c.Name == airlines.First())?.Code;
                if (!string.IsNullOrEmpty(_code))
                    return _code;

                for (int i = 0; i < airlines.Count; i++)
                {
                    if (airlines[i].Contains("AIRWAYS"))
                        airlines[i] = airlines[i].Replace("AIRWAYS", "AIRLINE");
                }

                _code = DecodingTables.Airlines.FirstOrDefault(c => c.Name.Contains(airlines.First()))?.Code;
                if (!string.IsNullOrEmpty(_code))
                    return _code;

                foreach (var word in _airlines)
                {
                    if (word.Contains(" AS "))
                        break;
                    _code = DecodingTables.Airlines.FirstOrDefault(c => c.Name == word)?.Code;
                    if (!string.IsNullOrEmpty(_code))
                        return _code;

                    _code = DecodingTables.Airlines.FirstOrDefault(c => c.Name.StartsWith(word.TrimEnd()))?.Code;
                    if (!string.IsNullOrEmpty(_code))
                        return _code;

                    _code = DecodingTables.Airlines.FirstOrDefault(c => c.Name.Contains(word.TrimEnd()))?.Code;
                    if (!string.IsNullOrEmpty(_code))
                        return _code;
                }
            }

            if (code.ToUpper().Contains(" FOR "))
            {
                _airlines = code.Split(new[] { " FOR " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (var word in _airlines)
                {
                    _code = DecodingTables.Airlines.FirstOrDefault(c => c.Name == word)?.Code;
                    if (!string.IsNullOrEmpty(_code))
                        return _code;

                    _code = DecodingTables.Airlines.FirstOrDefault(c => c.Name.Contains(word))?.Code;
                    if (!string.IsNullOrEmpty(_code))
                        return _code;

                }
            }

            if (code.ToUpper().Contains(" - "))
            {
                _airlines = code.Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (var word in _airlines)
                {
                    _code = DecodingTables.Airlines.FirstOrDefault(c => c.Name == word)?.Code;
                    if (!string.IsNullOrEmpty(_code))
                        return _code;

                    _code = DecodingTables.Airlines.FirstOrDefault(c => c.Name.Contains(word))?.Code;
                    if (!string.IsNullOrEmpty(_code))
                        return _code;
                }
            }

            if (!code.ToUpper().Contains(" AS "))
            {
                var lastIndex = _airlines.Count() - 1;
                if (Char.IsDigit(_airlines.Last(), 0) && _airlines[lastIndex - 1].Length.Equals(2))
                    return _airlines[lastIndex - 1];

                if (!Char.IsDigit(_airlines[lastIndex], 0) && _airlines[lastIndex].Length.Equals(2))
                    return _airlines.Last();
            }

            if (code.ToUpper().Contains(" DBA "))
            {
                _airlines = code.Split(new[] { " DBA " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (var word in _airlines)
                {
                    _code = DecodingTables.Airlines.FirstOrDefault(c => c.Name == word)?.Code;
                    if (!string.IsNullOrEmpty(_code))
                        return _code;

                    _code = DecodingTables.Airlines.FirstOrDefault(c => c.Name.Contains(word))?.Code;
                    if (!string.IsNullOrEmpty(_code))
                        return _code;
                }
            }

            _code = DecodingTables.Airlines.FirstOrDefault(c => c.Name.Contains(_airlines.First()))?.Code;
            if (!string.IsNullOrEmpty(_code))
                return _code;

            return _code ?? string.Empty;
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





