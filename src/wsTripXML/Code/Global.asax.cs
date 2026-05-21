//using System;
//using System.Diagnostics;

//using TripXMLMain;

//namespace wsTripXML.wsTravelTalk
//{

//    public class Global : HttpApplication
//    {

//        #region  Component Designer Generated Code 

//        public Global() : base()
//        {

//            // This call is required by the Component Designer.
//            InitializeComponent();

//            // Add any initialization after the InitializeComponent() call

//        }

//        // Required by the Component Designer
//        private System.ComponentModel.IContainer components;

//        // NOTE: The following procedure is required by the Component Designer
//        // It can be modified using the Component Designer.
//        // Do not modify it using the code editor.
//        [DebuggerStepThrough()]
//        private void InitializeComponent()
//        {
//            components = new System.ComponentModel.Container();
//        }

//        #endregion

//        public void Application_Start(object sender, EventArgs e)
//        {

//            try
//            {
//                var argoApplication = Application;
//                modMain.TripXMLStartUp(ref argoApplication);
//            }
//            catch (Exception ex)
//            {
//                CoreLib.SendTrace("", "Global.asax", "Error Starting", ex.Message, string.Empty);
//            }

//        }

//        public void Session_Start(object sender, EventArgs e)
//        {
//            // Fires when the session is started
//        }

//        public void Application_BeginRequest(object sender, EventArgs e)
//        {
//            // Fires at the beginning of each request
//        }

//        public void Application_AuthenticateRequest(object sender, EventArgs e)
//        {
//            // Fires upon attempting to authenticate the use
//        }

//        public void Application_Error(object sender, EventArgs e)
//        {
//            // Fires when an error occurs
//        }

//        public void Session_End(object sender, EventArgs e)
//        {
//            // Fires when the session ends
//        }

//        public void Application_End(object sender, EventArgs e)
//        {
//            // Fires when an end occurs
//        }

//    }

//}