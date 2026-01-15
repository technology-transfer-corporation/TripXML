
namespace wsTripXML
{

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    [System.Xml.Serialization.XmlRoot(Namespace = "", IsNullable = false)]
    public partial class SessionCreateRS
    {
        /// <remarks/>
        public object Success { get; set; }

        /// <remarks/>
        public ConversationID ConversationID { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public decimal Version { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    public partial class ConversationID
    {
        /// <remarks/>
        public Errors Errors { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    public partial class Errors
    {
        /// <remarks/>
        public string Error { get; set; }
    }
}