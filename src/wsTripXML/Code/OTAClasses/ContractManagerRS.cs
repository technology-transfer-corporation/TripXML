
namespace wsTripXML.wsTravelTalk.wmContractManager
{
    public class ContractManagerRS
    {
        // <remarks/>
        public Success Success;

        // <remarks/>
        [System.Xml.Serialization.XmlArrayItem("Warning", IsNullable = false)]
        public string[] Warnings;

        // <remarks/>
        [System.Xml.Serialization.XmlArrayItem("Error", IsNullable = false)]
        public string[] Errors;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class Success
    {
    }
}