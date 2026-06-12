using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.Services;
using System.Xml;
using TripXMLMain;

namespace wsTripXML.wsTravelTalk
{

    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    // <System.Web.Script.Services.ScriptService()> _
    [WebService(Namespace = "http://tripxml.com/wsUpdateMarkups")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class wsUpdateMarkups : WebService
    {
        private StringBuilder sb = new StringBuilder();

        private string ServiceRequest(string strRequest)
        {
            string strResponse = "";
            FileInfo markUp;
            XmlTextWriter writer = null;
            XmlDocument xmlDoc;
            XmlDocument oDoc;
            XmlElement oRoot;
            string newstrRequest = "";

            try
            {
                if (modCore.Trace)
                    CoreLib.SendTrace("", "wsUpdateMarkups", "============= Request ============= ", strRequest, string.Empty);

                oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                oRoot = oDoc.DocumentElement;

                foreach (XmlNode oNode in oRoot.SelectNodes("Promotion"))
                {
                    var piNode = oNode.SelectSingleNode("Id");
                    var childPINode = oDoc.CreateNode("element", "PromotionId", "");
                    childPINode.InnerText = piNode.InnerText;
                    oNode.ReplaceChild(childPINode, piNode);

                    // Dim childAMNode As XmlNode = oDoc.CreateNode("element", "AppliedMarkup", "")
                    // childAMNode.InnerText = "Base"
                    // oNode.AppendChild(childAMNode)

                    var ftNode = oNode.SelectSingleNode("FareTypes");
                    XmlNode childFTNode;
                    childFTNode = oDoc.CreateNode("element", "FareType", "");
                    if (ftNode.SelectSingleNode("Id") is not null)
                    {
                        childFTNode.InnerText = ftNode.SelectSingleNode("Id").InnerText;
                    }
                    else
                    {
                        childFTNode.InnerText = "";
                    }
                    oNode.ReplaceChild(childFTNode, ftNode);

                    var scNode = oNode.SelectSingleNode("SupplierCodes");

                    if (scNode.SelectNodes("Id").Count > 0)
                    {
                        int i = 0;
                        XmlNode firstNode;
                        firstNode = oDoc.CreateNode("element", "SupplierCode", "");
                        firstNode.InnerText = scNode.SelectNodes("Id").Item(0).InnerText;

                        foreach (XmlNode idNode in scNode.SelectNodes("Id"))
                        {
                            XmlNode childNode;
                            childNode = oDoc.CreateNode("element", "SupplierCode", "");
                            childNode.InnerText = idNode.InnerText;

                            if (i > 0)
                            {
                                var pNode = oDoc.CreateNode("element", "Promotion", "");
                                pNode.InnerXml = oNode.InnerXml;
                                pNode.ReplaceChild(childNode, pNode.SelectSingleNode("SupplierCodes"));
                                oRoot.InsertAfter(pNode, oNode);
                            }
                            i = i + 1;
                        }

                        oNode.ReplaceChild(firstNode, scNode);
                    }
                    else
                    {
                        XmlNode childNode;
                        childNode = oDoc.CreateNode("element", "SupplierCode", "");
                        childNode.InnerText = "";
                        oNode.ReplaceChild(childNode, scNode);
                    }
                }

                strRequest = oDoc.OuterXml;

                xmlDoc = new XmlDocument();
                markUp = new FileInfo(ConfigurationManager.AppSettings["TripXMLFolder"] + @"\Xsl\Aggregation\Markups.xml");
                writer = new XmlTextWriter(markUp.FullName, null);
                xmlDoc.LoadXml(strRequest);
                xmlDoc.Save(writer);
                strResponse = "Success";
            }
            catch (Exception ex)
            {
                strResponse = ex.Message;
            }
            finally
            {
                if (writer is not null)
                {
                    writer.Close();
                }
                if (modCore.Trace)
                    CoreLib.SendTrace("", "wsUpdateMarkups", "============= Response ============= ", strResponse, string.Empty);
            }

            return strResponse;
            sb = null;
        }

        [WebMethod(Description = "Update Markups.")]
        public string UpdateMarkups(string xmlRequest)
        {
            return ServiceRequest(xmlRequest);
        }

    }
}