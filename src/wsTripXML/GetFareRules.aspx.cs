using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Microsoft.VisualBasic.CompilerServices;

namespace wsTripXML
{
    public partial class GetFareRules : Page
    {
        public Repeater RPTDetails;

        public GetFareRules()
        {
            Load += Page_Load;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                try
                {
                    txtPNR.Text = Request.QueryString["pnr"];

                    if (txtPNR.Text.Trim().Length > 0)
                    {
                        processPNR(txtPNR.Text);
                        Response.Redirect("RulesResult.aspx", true);
                    }
                }


                catch (Exception ex)
                {

                }
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            if (txtPNR.Text.Trim().Length > 0)
            {
                processPNR(txtPNR.Text);
                var sb2 = new StringBuilder();

                // sb2 = sb2.Append("<script type=""text/javascript"">")
                sb2 = sb2.Append("window.open('RulesResult.aspx', '', '');");
                // sb2 = sb2.Append("</script>")
                Page.ClientScript.RegisterStartupScript(typeof(string), "MyRules", sb2.ToString(), true);
            }

        }
        private void processPNR(string PNR)
        {
            string req = "";
            var sb = new StringBuilder();
            sb = sb.Append("<OTA_ReadRQ>");
            sb = sb.Append("<POS><Source PseudoCityCode=\"");
            sb = sb.Append("ATL1S2157");
            sb = sb.Append("\"><RequestorID Type=\"21\" ID=\"");
            sb = sb.Append("RussiaHouse");
            sb = sb.Append("\"/> </Source><TPA_Extensions><Provider><Name>");
            sb = sb.Append("Amadeus");
            sb = sb.Append("</Name><System>");
            sb = sb.Append("Production");
            sb = sb.Append("</System><Userid>");
            sb = sb.Append("morqua");
            sb = sb.Append("</Userid> <Password>");
            sb = sb.Append("sm1rN0ff");
            sb = sb.Append("</Password></Provider></TPA_Extensions></POS><UniqueID ID=\"");
            sb = sb.Append(PNR.Trim());
            sb = sb.Append("\"  /></OTA_ReadRQ>");

            req = sb.ToString();
            sb = sb.Remove(0, sb.Length);

            string strResponse;
            var PNRRead = new wsTravelTalk.wsPNRRead_v03();
            strResponse = PNRRead.wmPNRReadXml(req);

            XmlDocument oDoc = null;
            XmlElement oRoot = null;

            XmlNode oNode = null;

            var sbFlights = new StringBuilder();
            int seats = 0;
            int AdtCount = 0;
            int CHDCount = 0;
            int INFCount = 0;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;


                foreach (XmlNode currentONode in oRoot.SelectNodes("TravelItinerary/ItineraryInfo/ReservationItems/Item"))
                {
                    oNode = currentONode;
                    foreach (XmlNode oNode2 in oNode.SelectNodes("Air"))
                    {
                        sbFlights = sbFlights.Append("<FlightSegment DepartureDateTime=\"").Append(oNode2.Attributes["DepartureDateTime"].Value).Append("\" ArrivalDateTime=\"").Append(oNode2.Attributes["ArrivalDateTime"].Value).Append("\" FlightNumber=\"").Append(oNode2.Attributes["FlightNumber"].Value).Append("\" ResBookDesigCode=\"").Append(oNode2.Attributes["ResBookDesigCode"].Value).Append("\">");
                        sbFlights = sbFlights.Append("<DepartureAirport LocationCode=\"").Append(oNode2.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value).Append("\" />");
                        sbFlights = sbFlights.Append("<ArrivalAirport LocationCode=\"").Append(oNode2.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value).Append("\" />");
                        sbFlights = sbFlights.Append("<MarketingAirline Code=\"").Append(oNode2.SelectSingleNode("MarketingAirline").Attributes["Code"].Value).Append("\" />");
                        sbFlights = sbFlights.Append("</FlightSegment>");
                        seats = Conversions.ToInteger(oNode2.Attributes["NumberInParty"].Value);
                    }

                    // If Not oNode.SelectSingleNode("DepartureAirport") Is Nothing Then

                    // End If
                    // If Not oNode.SelectSingleNode("ArrivalAirport") Is Nothing Then
                    // ' oNode.SelectSingleNode("ArrivalAirport").InnerText = GetDecodeValue(ttAirports, oNode.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value)
                    // End If


                }

                foreach (XmlNode currentONode1 in oRoot.SelectNodes("TravelItinerary/CustomerInfos/CustomerInfo"))
                {
                    oNode = currentONode1;

                    if (oNode.SelectSingleNode("Customer").SelectSingleNode("PersonName").Attributes["NameType"].Value == "ADT")
                    {
                        AdtCount = AdtCount + 1;
                    }
                    if (oNode.SelectSingleNode("Customer").SelectSingleNode("PersonName").Attributes["NameType"].Value == "CHD")
                    {
                        CHDCount = CHDCount + 1;
                    }
                    if (oNode.SelectSingleNode("Customer").SelectSingleNode("PersonName").Attributes["NameType"].Value == "INF")
                    {
                        INFCount = INFCount + 1;
                    }

                }

                sb = sb.Append("<OTA_AirRulesRQ>");
                sb = sb.Append("<POS><Source PseudoCityCode=\"");
                sb = sb.Append("ATL1S2157");
                sb = sb.Append("\"><RequestorID Type=\"21\" ID=\"");
                sb = sb.Append("RussiaHouse");
                sb = sb.Append("\"/> </Source><TPA_Extensions><Provider><Name>");
                sb = sb.Append("Amadeus");
                sb = sb.Append("</Name><System>");
                sb = sb.Append("Production");
                sb = sb.Append("</System><Userid>");
                sb = sb.Append("morqua");
                sb = sb.Append("</Userid> <Password>");
                sb = sb.Append("sm1rN0ff");
                sb = sb.Append("</Password></Provider></TPA_Extensions></POS>");
                sb = sb.Append("<AirItinerary>");
                sb = sb.Append("<OriginDestinationOptions>");
                sb = sb.Append("<OriginDestinationOption>");
                sb = sb.Append(sbFlights.ToString());
                sb = sb.Append("</OriginDestinationOption>");
                sb = sb.Append("</OriginDestinationOptions>");

                sb = sb.Append("</AirItinerary>");
                sb = sb.Append("<TravelerInfoSummary>");
                sb = sb.Append("<SeatsRequested>").Append(seats.ToString()).Append("</SeatsRequested>");
                sb = sb.Append("<AirTravelerAvail>");
                if (AdtCount > 0)
                {
                    sb = sb.Append("<PassengerTypeQuantity Code=\"ADT\" Quantity=\"").Append(AdtCount.ToString()).Append("\" />");
                }
                if (CHDCount > 0)
                {
                    sb = sb.Append("<PassengerTypeQuantity Code=\"CHD\" Quantity=\"").Append(CHDCount.ToString()).Append("\" />");
                }
                if (INFCount > 0)
                {
                    sb = sb.Append("<PassengerTypeQuantity Code=\"INF\" Quantity=\"").Append(INFCount.ToString()).Append("\" />");
                }
                sb = sb.Append("</AirTravelerAvail>");

                try
                {

                    if (oRoot.SelectSingleNode("TravelItinerary/ItineraryInfo/ReservationItems/ItemPricing") is not null)
                    {

                        if (oRoot.SelectSingleNode("TravelItinerary/ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo") is not null)
                        {
                            if (oRoot.SelectSingleNode("TravelItinerary/ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo").Attributes["PricingSource"] is not null)
                            {
                                sb = sb.Append("<PriceRequestInformation PricingSource=\"").Append(oRoot.SelectSingleNode("TravelItinerary/ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo").Attributes["PricingSource"].Value).Append("\" />");
                            }
                            else
                            {
                                sb = sb.Append("<PriceRequestInformation PricingSource=\"Published\" />");
                            }
                        }
                        else
                        {
                            sb = sb.Append("<PriceRequestInformation PricingSource=\"Published\" />");
                        }
                    }
                    else
                    {
                        sb = sb.Append("<PriceRequestInformation PricingSource=\"Published\" />");
                    }
                }




                catch (Exception ex)
                {

                }


                sb = sb.Append("</TravelerInfoSummary>");
                sb = sb.Append("</OTA_AirRulesRQ>");

                req = sb.ToString();
                sb = sb.Remove(0, sb.Length);

                var AirRule = new wsTravelTalk.wsAirRules_v03();
                strResponse = AirRule.wmAirRulesXml(req);

                // Dim reader As StreamReader = New StreamReader(Server.MapPath("AirRulesRS2.xml"))
                // strResponse = reader.ReadToEnd()

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                var nodeList = oDoc.GetElementsByTagName("SubSection");
                // RPTHeader.DataSource = nodeList
                // RPTHeader.DataBind()
                Session["RulesNodes"] = nodeList;
            }



            catch (Exception ex)
            {

            }

        }

        protected void RPTHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            XmlNode node;
            node = (XmlNode)e.Item.DataItem;

            Label lbl = (Label)e.Item.FindControl("lblTitle");
            lbl.Text = node.Attributes["SubTitle"].Value;
            var ndList = node.SelectNodes("Paragraph");



            Table tb = (Table)e.Item.FindControl("tb");
            foreach (XmlNode oNode in ndList)
            {

                var row = new TableRow();
                var cell = new TableCell();
                cell.Text = oNode.SelectSingleNode("Text").InnerText;
                row.Cells.Add(cell);
                tb.Rows.Add(row);
            }
            // RPTDetails = CType(e.Item.FindControl("RPTDetails"), Repeater)



            // RPTDetails.DataSource = ndList
            // RPTDetails.DataBind()





        }








    }
}