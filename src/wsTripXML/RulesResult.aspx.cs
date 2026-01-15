using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace wsTripXML
{
    public partial class RulesResult : Page
    {
        public RulesResult()
        {
            Load += Page_Load;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["RulesNodes"] is not null)
                {
                    XmlNodeList nodeList = (XmlNodeList)Session["RulesNodes"];
                    RPTHeader.DataSource = nodeList;
                    RPTHeader.DataBind();
                    Session["RulesNodes"] = null;
                }
                else
                {

                }
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