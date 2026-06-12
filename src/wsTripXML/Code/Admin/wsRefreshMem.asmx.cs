using System;
using System.Diagnostics;
using System.Text;
namespace wsTripXML.wsTravelTalk
{
    public partial class wsRefreshMem
    {

        private readonly modMain _modMain;

        public wsRefreshMem(modMain modMain)
        {
            _modMain = modMain;
        }
        private StringBuilder sb = new StringBuilder();
        public string wmRefreshMem()
        {
            try
            {
                modMain.TripXMLStartUp();
                TripXMLTools.TripXMLLoad.BuildDecodingDataViews();
                TripXMLMain.CoreLib.ClearXslCache();
                return "Succes. Application Variables Reloaded.";
            }
            catch (Exception ex)
            {
                return sb.Append("Error Reloading Application Variables. ").Append(ex.Message).ToString();
            }
        }

    }

}