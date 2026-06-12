using System.ComponentModel;
namespace wsTripXML.wsTravelTalk
{

    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    // <System.Web.Script.Services.ScriptService()> _
    [ToolboxItem(false)]
    public partial class wsCreateTicketInvoice
    {

        private string ServiceRequest(string strRequest)
        {
            string strResponse = "";

            // strResponse = AdminStatusManager.Program.CreateTicket(strRequest)

            return strResponse;
        }
        public string CreateTicketInvoice(string xmlRequest)
        {
            return ServiceRequest(xmlRequest);
        }

    }
}