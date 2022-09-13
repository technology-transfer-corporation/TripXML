using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripXMLTools
{
    public class TripXMLProviderDTO
    {
        public List<Server> Servers { get; set; }
    }

    public class Server
    {
        public string Address { get; set; }
        public List<Customerserver> CustomerServers { get; set; }
    }

    public class Customerserver
    {
        public Customer Customer { get; set; }
    }

    public class Customer
    {
        public string RequestorId { get; set; }
        public bool Active { get; set; }
        public List<Txuser> TxUsers { get; set; }
    }

    public class Txuser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<Provideruser> ProviderUsers { get; set; }
    }

    public class Provideruser
    {
        public Provider Provider { get; set; }
    }

    public class Provider
    {
        public string Name { get; set; }
        public System System { get; set; }
        public List<Pcc> PCCs { get; set; }
    }

    public class System
    {
        public string URL { get; set; }
        public string SOAP4Url { get; set; }
        public string Environment { get; set; }
    }

    public class Pcc
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public object AmadeusWs { get; set; }
        public bool SessionPool { get; set; }
        public string SOAPType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<Profile> Profiles { get; set; }
        public List<Wsdlschema> Wsdlschemas { get; set; }
        public List<OpenType> OpenTypes { get; set; }
    }

    public class OpenType
    {
        public string OfficeID { get; set; }
        public string Agent { get; set; }
        public string SignIn { get; set; }
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
        public string LanguageCode { get; set; }
        public string TravelAgentID { get; set; }
    }

    public class Profile
    {
        public string Origin { get; set; }
        public string Text { get; set; }
        public string Tkt { get; set; }
        public string Cryptic { get; set; }
        public string XML { get; set; }
    }

    public class WsdlschemaObject
    {
        public List<Wsdlschema> Wsdlschemas { get; set; }
    }

    public class Wsdlschema
    {
        public string Name { get; set; }
        public string Version { get; set; }        
    }


}
