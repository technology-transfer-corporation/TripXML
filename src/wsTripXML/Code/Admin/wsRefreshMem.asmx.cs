using System;
using System.Diagnostics;
using System.Text;
using System.Web.Services;

namespace wsTripXML.wsTravelTalk
{


    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsRefreshMem", Name = "wsRefreshMem", Description = "A TripXML Web Service to Refresh Application Variables.")]


    public class wsRefreshMem : WebService
    {

        #region  Web Services Designer Generated Code 

        public wsRefreshMem() : base()
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
        private StringBuilder sb = new StringBuilder();

        [WebMethod(Description = "Refresh Application Variables.")]
        public string wmRefreshMem()
        {
            try
            {
                var argoApplication = Application;
                modMain.TripXMLStartUp(ref argoApplication);
                return "Succes. Application Variables Reloaded.";
            }
            catch (Exception ex)
            {
                return sb.Append("Error Reloading Application Variables. ").Append(ex.Message).ToString();
            }
        }

    }

}