using System;
using System.Text;
using System.Threading;
using System.Web.Configuration;
using TripXMLMain;


namespace wsTripXML.wsTravelTalk
{


    public class cLogData
    {
        private string mstrRequest = "";
        private string mstrResponse = "";

        private StringBuilder sb = new StringBuilder();

        public void LogDataDeals(string strRequest, string strResponse)
        {

            mstrRequest = strRequest;
            mstrResponse = strResponse;

            if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["DataDatabase"]))
            {
                var oLofThread = new Thread(new ThreadStart(LogDeals));

                oLofThread.Start();
            }

        }

        private void LogDeals()
        {
            cDA oDA = null;

            try
            {
                oDA = new cDA("DataDatabase");
                oDA.AddDeals(mstrRequest, mstrResponse);
            }

            catch (Exception ex)
            {
            }
            finally
            {
                if (oDA is not null)
                {
                    oDA.Dispose();
                }
            }

        }

        public string GetDataDeals(string strRequest)
        {
            cDA oDA = null;
            string strResponse = "";

            try
            {
                if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["DataDatabase"]))
                {
                    oDA = new cDA("DataDatabase");
                    strResponse = oDA.GetDeals(strRequest);
                }

                return strResponse;
            }

            catch (Exception ex)
            {
            }
            finally
            {
                if (oDA is not null)
                {
                    oDA.Dispose();
                }
            }
            return string.Empty;
        }

    }

}