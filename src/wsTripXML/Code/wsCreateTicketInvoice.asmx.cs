using System.ComponentModel;
using System.Web.Services;

namespace wsTripXML.wsTravelTalk
{

    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    // <System.Web.Script.Services.ScriptService()> _
    [WebService(Namespace = "http://tripxml.com/wsUpdateMarkups")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class wsCreateTicketInvoice : WebService
    {

        private string ServiceRequest(string strRequest)
        {
            string strResponse = "";

            // strResponse = AdminStatusManager.Program.CreateTicket(strRequest)

            return strResponse;
        }

        [WebMethod(Description = "Update Markups.")]
        public string CreateTicketInvoice(string xmlRequest)
        {
            return ServiceRequest(xmlRequest);
        }

    }
}