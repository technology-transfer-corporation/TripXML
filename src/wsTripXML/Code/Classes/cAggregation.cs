using System;
using System.Text;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{


    public class cAggregation
    {


        public static void Aggregate(ttServices MessageID, string XslPath, string Version, ref string Response)
        {
            string strXslName = "";
            var sb = new StringBuilder();

            XslPath += @"Aggregation\";

            if (Version.Length > 0)
            {
                strXslName = sb.Append(Version).Append("_").ToString();
                sb.Remove(0, sb.Length);
            }

            switch (MessageID)
            {
                case ttServices.AirAvail:
                    {
                        strXslName = "Aggregation_AirAvailRS.xsl";
                        break;
                    }
                case ttServices.LowFare:
                    {
                        strXslName = "Aggregation_LowFareRS.xsl";
                        break;
                    }
                case ttServices.LowFarePlus:
                    {
                        strXslName = "Aggregation_LowFareRS.xsl";
                        break;
                    }
                case ttServices.LowFareSchedule:
                    {
                        strXslName = "Aggregation_LowFareRS.xsl";
                        break;
                    }
                case ttServices.CarAvail:
                    {
                        strXslName = "Aggregation_CarAvailRS.xsl";
                        break;
                    }
                case ttServices.HotelAvail:
                    {
                        strXslName = "Aggregation_HotelAvailRS.xsl";
                        break;
                    }
                case ttServices.HotelSearch:
                    {
                        strXslName = "Aggregation_HotelSearchRS.xsl";
                        break;
                    }

                default:
                    {
                        strXslName = "NoSupported.xsl";
                        break;
                    }
            }

            try
            {
                Response = CoreLib.TransformXML(Response, XslPath, strXslName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            sb = null;
        }

        public static void ProcessMarkup(string XslPath, string Version, ref string Response)
        {
            string strXslName = "";
            var sb = new StringBuilder();

            XslPath += @"Aggregation\";

            if (Version.Length > 0)
            {
                strXslName = sb.Append(Version).Append("_").ToString();
                sb.Remove(0, sb.Length);
            }

            strXslName = "Markups_LowFareRS.xsl";

            CoreLib.SendTrace("", "cAggregation", "markup", Response.Substring(0, 2000), string.Empty);

            try
            {
                Response = CoreLib.TransformXML(Response, XslPath, strXslName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            sb = null;
        }

    }

}