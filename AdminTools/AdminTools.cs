using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Data.SqlClient;
using System.Net.Mail;

namespace AdminStatusManager
{
    public class Program
    {
        static public string CreateTicket(string strRequest)
        {
            string strResponse = "<Tickets>";
            var sb = new StringBuilder();
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            var startDate = new DateTime();
            var currentDate = new DateTime();
            currentDate = DateTime.Now;
            string curDate = currentDate.ToString("ddMMMyy").ToUpper();
            //curDate = "09MAR12";
            //startDate = Convert.ToDateTime("2011-06-30 00:00:00");
            startDate = DateTime.Now.AddDays(-50);
            const string cRefund = "38";
            const string cPartialRefund = "45";
            const string cInvoluntryRefund = "48";
            const string cInvoluntaryPartialRefund = "49";
            const string cExchange = "40";
            const string cVoid = "42";
            const string cEvenExchange = "44";
            const string cPartialExchange = "47";
            const string cEpay = "23";

            StreamWriter sw = new StreamWriter("C:\\tripxml\\log\\adminprocess.txt", true, Encoding.ASCII);
            sw.Write($"\r\nStart process: {currentDate}\r\n");
            sw.Write("==================================== \r\n");
            sw.Close();

            //var moConn = new SqlConnection
            //{
            //    ConnectionString = "Data Source=8.8.248.115\\DEDICATED8-VM6,1433;Initial Catalog=BBTrip;User ID=sa;Password=vLUIpUpTq8mx"
            //};

            oDoc = new XmlDocument();
            oDoc.LoadXml(strRequest);
            oRoot = oDoc.DocumentElement;
            
            AdminTools.PNRRead.wmPNRReadRequest pnr = new AdminTools.PNRRead.wmPNRReadRequest();
            AdminTools.PNRRead.wmPNRReadResponse pnrresp = new AdminTools.PNRRead.wmPNRReadResponse();
            AdminTools.PNRRead.OTA_TravelItineraryRS pnrrespota = new AdminTools.PNRRead.OTA_TravelItineraryRS();
            AdminTools.PNRRead.wsPNRReadSoapClient pnrsaop = new AdminTools.PNRRead.wsPNRReadSoapClient();
            AdminTools.PNRRead.OTA_ReadRQ pnrota = new AdminTools.PNRRead.OTA_ReadRQ();
            AdminTools.PNRRead.POS pnrpos = new AdminTools.PNRRead.POS();
            AdminTools.PNRRead.Source[] pnrsource = new AdminTools.PNRRead.Source[1];
            pnrsource[0] = new AdminTools.PNRRead.Source();
            AdminTools.PNRRead.RequestorID pnrreqid = new AdminTools.PNRRead.RequestorID();
            AdminTools.PNRRead.TPA_Extensions pnrtpa = new AdminTools.PNRRead.TPA_Extensions();
            AdminTools.PNRRead.Provider pnrprovider = new AdminTools.PNRRead.Provider();
            AdminTools.PNRRead.UniqueIDRQ pnrid = new AdminTools.PNRRead.UniqueIDRQ();

            pnrsource[0].PseudoCityCode = "NYC1S211F"; //"ATL1S2157";
            pnrreqid.ID = "DTT";// "RussiaHouse";
            pnrreqid.Type = "21";
            pnrsource[0].RequestorID = pnrreqid;
            pnrprovider.Name = "Amadeus";
            pnrprovider.System = "Production";
            pnrprovider.Userid = "Downtown";// "morqua";
            pnrprovider.Password = "tr@v3l";// "sm1rN0ff";
            pnrtpa.Provider = pnrprovider;
            pnrpos.Source = pnrsource;
            pnrpos.TPA_Extensions = pnrtpa;
            pnrota.POS = pnrpos;

            try
            {
                foreach (XmlNode oNode in oRoot.SelectNodes("PNR"))
                {
                    pnrid.ID = oNode.InnerText;
                    pnrota.UniqueID = pnrid;
                    pnr.OTA_ReadRQ = pnrota;
                    string pnrNumber = oNode.InnerText;
                    string status = oNode.SelectSingleNode("@Status").InnerText;
                    var dInvoiceDate = new DateTime();
                    dInvoiceDate = DateTime.Now;

                    if (status != "")
                    {
                        pnrresp.OTA_TravelItineraryRS = pnrsaop.wmPNRRead(null, pnr.OTA_ReadRQ);

                        sw = new StreamWriter("C:\\tripxml\\log\\adminprocess.txt", true, Encoding.ASCII);
                        sw.Write("\r\n " + oNode.InnerText);

                        string transactionid = oNode.SelectSingleNode("@ID").InnerText;
                        string customer = oNode.SelectSingleNode("@Customer").InnerText;
                        double airlineFee = 0;
                        double agencyFee = 0;

                        if (oNode.SelectSingleNode("@AirlineFee").InnerText != "")
                            airlineFee = Convert.ToDouble(oNode.SelectSingleNode("@AirlineFee").InnerText);

                        if (oNode.SelectSingleNode("@AgencyFee").InnerText != "")
                            agencyFee = Convert.ToDouble(oNode.SelectSingleNode("@AgencyFee").InnerText);

                        if (oNode.SelectSingleNode("@InvoiceDate").InnerText != "")
                        {
                            dInvoiceDate = Convert.ToDateTime(oNode.SelectSingleNode("@InvoiceDate").InnerText);
                        }

                        strResponse += $"<TransactionID>{transactionid}</TransactionID><ModifierID>36</ModifierID>";

                        if (pnrresp.OTA_TravelItineraryRS.Errors != null)
                        {
                            sw.Write($" Amadeus error:{pnrresp.OTA_TravelItineraryRS.Errors[0].Value}");
                            strResponse += $"<Error>{pnrresp.OTA_TravelItineraryRS.Errors[0].Value}</Error>";
                        }
                        else if ((pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.TPA_Extensions != null
                                  && pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.TPA_Extensions.IssuedTickets != null
                                  && pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.TPA_Extensions.IssuedTickets.IssuedTicket != null)
                                 || status == cRefund || status == cInvoluntryRefund || status == cPartialRefund ||
                                 status == cInvoluntaryPartialRefund || status == cVoid)
                        {
                            string tktnum = "";

                            int totalnip = pnrresp.OTA_TravelItineraryRS.TravelItinerary.CustomerInfos.CustomerInfo
                                .Length;

                            if (status == cRefund || status == cInvoluntryRefund || status == cPartialRefund ||
                                status == cInvoluntaryPartialRefund || status == cVoid)
                            {
                                if (pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.TPA_Extensions != null
                                    && pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.TPA_Extensions
                                        .IssuedTickets != null
                                    && pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.TPA_Extensions
                                        .IssuedTickets.ElectronicTicket != null)
                                {
                                    DateTime dinvoiceDate = new DateTime();
                                    dinvoiceDate = DateTime.Now;

                                    foreach (AdminTools.PNRRead.ElectronicTicket eTkt in pnrresp.OTA_TravelItineraryRS
                                        .TravelItinerary.ItineraryInfo.TPA_Extensions.IssuedTickets.ElectronicTicket)
                                    {
                                        double dbaseFare = 0;
                                        double dtaxes = 0;
                                        double dcommAmount = 0;
                                        double dcommPercent = 0;
                                        double dtotalFare = 0;
                                        double dmarkup = 0;
                                        double dfinalComm = 0;
                                        double dagencyComm = 0;
                                        double damountDue = 0;

                                        if (eTkt.Value.Substring(3, 1) == "-")
                                            tktnum = eTkt.Value.Substring(0, 14);
                                        else
                                            tktnum = eTkt.Value.Substring(4, 14);

                                        if (customer == "21")
                                        {
                                            AdminTools.Cryptic.CrypticRS crypticRS = new AdminTools.Cryptic.CrypticRS();

                                            crypticRS = CrypticCommand(tktnum);

                                            if (crypticRS.Screen != null)
                                            {
                                                foreach (AdminTools.Cryptic.Line strLine in crypticRS.Screen)
                                                {
                                                    // ********************************************
                                                    //following 'if' condition was not in local code
                                                    //**********************************************

                                                    if (strLine.Value != null)
                                                    {
                                                        //------------------------------------------------
                                                        if (strLine.Value.StartsWith("FARE"))
                                                        {
                                                            string bf = strLine.Value.Substring(12).Trim();

                                                            // 'if' condition was didfferent in local code
                                                            //  if (bf != "IT")
                                                            if (bf != "IT" && bf != "BT")
                                                                //--------------------------------------------
                                                                dbaseFare = Convert.ToDouble(bf);
                                                            else
                                                            {
                                                                if (pnrresp.OTA_TravelItineraryRS.TravelItinerary
                                                                        .ItineraryInfo.SpecialRequestDetails.Remarks !=
                                                                    null
                                                                    && pnrresp.OTA_TravelItineraryRS.TravelItinerary
                                                                        .ItineraryInfo.SpecialRequestDetails.Remarks
                                                                        .Remark[0] != null)
                                                                {
                                                                    foreach (AdminTools.PNRRead.Remark rmk in pnrresp
                                                                        .OTA_TravelItineraryRS.TravelItinerary
                                                                        .ItineraryInfo.SpecialRequestDetails.Remarks
                                                                        .Remark)
                                                                    {
                                                                        string rmkText = rmk.Value;
                                                                        if (rmkText.IndexOf(
                                                                                "TOTAL AMOUNT IN GDS CURRENCY") != -1)
                                                                        {
                                                                            rmkText = rmkText.Replace(" ", "")
                                                                                .Substring(30);
                                                                            dtotalFare =
                                                                                Convert.ToDouble(rmkText.Substring(0,
                                                                                    rmkText.IndexOf("USD")));
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else if (strLine.Value.StartsWith("EQUIV"))
                                                            dbaseFare = Convert.ToDouble(strLine.Value.Substring(12, 13)
                                                                .Trim());
                                                        else if (strLine.Value.StartsWith("TAX"))
                                                            dtaxes += Convert.ToDouble(strLine.Value.Substring(12, 13)
                                                                .Trim());
                                                        else if (strLine.Value.StartsWith("TOTALTAX"))
                                                            dtaxes = Convert.ToDouble(
                                                                strLine.Value.Substring(12).Trim());
                                                        else if (strLine.Value.StartsWith("TOTAL") && dtotalFare == 0)
                                                        {
                                                            dtotalFare = Convert.ToDouble(strLine.Value.Substring(12)
                                                                .Trim());
                                                            break;
                                                        }
                                                    }
                                                }
                                            }

                                            if (pnrresp.OTA_TravelItineraryRS.TravelItinerary.TPA_Extensions != null &&
                                                pnrresp.OTA_TravelItineraryRS.TravelItinerary.TPA_Extensions.Items !=
                                                null && pnrresp.OTA_TravelItineraryRS.TravelItinerary.TPA_Extensions
                                                    .Items[0] != null)
                                            {
                                                Type comtype;
                                                comtype = pnrresp.OTA_TravelItineraryRS.TravelItinerary.TPA_Extensions
                                                    .Items[0].GetType();
                                                if (comtype.Name == "AgencyCommission")
                                                {
                                                    AdminTools.PNRRead.AgencyCommission acom =
                                                        new AdminTools.PNRRead.AgencyCommission();
                                                    acom = (AdminTools.PNRRead.AgencyCommission)pnrresp
                                                        .OTA_TravelItineraryRS.TravelItinerary.TPA_Extensions.Items[0];

                                                    dcommPercent = Convert.ToDouble(acom.Percent);
                                                    dcommAmount = Convert.ToDouble(acom.Amount);
                                                }
                                            }

                                            if (pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo
                                                    .SpecialRequestDetails.Remarks != null
                                                && pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo
                                                    .SpecialRequestDetails.Remarks.Remark[0] != null)
                                            {
                                                bool noPerPaxMarkup = true;
                                                string rmkText = "";

                                                foreach (AdminTools.PNRRead.Remark rmk in pnrresp.OTA_TravelItineraryRS
                                                    .TravelItinerary.ItineraryInfo.SpecialRequestDetails.Remarks.Remark)
                                                {
                                                    rmkText = rmk.Value;
                                                    if (rmkText.StartsWith("*THR** "))
                                                    {
                                                        rmkText = rmkText.Replace(" ", "").Substring(6);
                                                        dmarkup = Convert.ToDouble(rmkText.Substring(0,
                                                            rmkText.IndexOf("MARKUP")));
                                                        noPerPaxMarkup = false;
                                                        break;
                                                    }
                                                }

                                                if (noPerPaxMarkup)
                                                {
                                                    foreach (AdminTools.PNRRead.Remark rmk in pnrresp
                                                        .OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo
                                                        .SpecialRequestDetails.Remarks.Remark)
                                                    {
                                                        rmkText = rmk.Value;
                                                        if (rmkText.IndexOf("MARKUP WAS APPLIED") != -1)
                                                        {
                                                            rmkText = rmkText.Replace(" ", "").Substring(6);
                                                            dmarkup = Convert.ToDouble(rmkText.Substring(0,
                                                                rmkText.IndexOf("TOTAL")));

                                                            AdminTools.PNRRead.ItineraryRef recloc = pnrresp
                                                                .OTA_TravelItineraryRS.TravelItinerary.ItineraryRef;

                                                            if (recloc.Instance != null)
                                                            {
                                                                int iSPpax = Int16.Parse(recloc.Instance.Substring(11));

                                                                dmarkup = dmarkup / (totalnip + iSPpax);
                                                            }
                                                            else
                                                            {
                                                                dmarkup = dmarkup / totalnip;
                                                            }

                                                            break;
                                                        }
                                                    }
                                                }
                                            }

                                            if (dbaseFare == 0 && dtotalFare > 0)
                                            {
                                                dtotalFare = dtotalFare - dmarkup;
                                                dbaseFare = Math.Round(dtotalFare - dtaxes, 2);
                                            }

                                            if (dcommPercent > 0)
                                            {
                                                dfinalComm = dbaseFare * dcommPercent / 200;
                                            }
                                            else if (dcommAmount > 0)
                                            {
                                                dfinalComm = dcommAmount / 2;
                                            }

                                            dbaseFare *= -1;
                                            dtaxes *= -1;
                                            dtotalFare *= -1;

                                            if (status == cVoid)
                                                agencyFee = 30;

                                            if (status == cRefund || status == cPartialRefund ||
                                                status == cInvoluntryRefund || status == cInvoluntaryPartialRefund)
                                                dinvoiceDate = dInvoiceDate;

                                            dmarkup = agencyFee - dmarkup;

                                            dbaseFare = Math.Round(dbaseFare, 2);
                                            dtaxes = Math.Round(dtaxes, 2);
                                            dtotalFare = Math.Round(dtotalFare, 2);
                                            dmarkup = Math.Round(dmarkup, 2);
                                            dfinalComm = Math.Round(dfinalComm + 0.001, 2);
                                            dagencyComm = Math.Round(dagencyComm + 0.001, 2);
                                            damountDue = dtotalFare + dmarkup + dfinalComm + airlineFee;
                                            damountDue = Math.Round(damountDue, 2);
                                            dfinalComm *= -1;

                                            strResponse +=
                                                $"<Ticket><InvoiceDate>{dinvoiceDate.ToString("yyyy-MM-dd")}</InvoiceDate>" +
                                                $"<Airline>{tktnum.Substring(0, 3)}</Airline>" +
                                                $"<TicketNumber>{tktnum.Substring(4)}</TicketNumber>" +
                                                $"<BaseFare>{dbaseFare.ToString("0.00")}</BaseFare>" +
                                                $"<TaxAmount>{dtaxes.ToString("0.00")}</TaxAmount>" +
                                                $"<TotalFare>{dtotalFare.ToString("0.00")}</TotalFare>" +
                                                $"<Standard>{dmarkup.ToString("0.00")}</Standard>" +
                                                $"<Agency>{dagencyComm.ToString("0.00")}</Agency>" +
                                                $"<Affiliate>{dfinalComm.ToString("0.00")}</Affiliate>" +
                                                $"<AirlineFee>{airlineFee.ToString("0.00")}</AirlineFee>" +
                                                $"<TotalDue>{damountDue.ToString("0.00")}</TotalDue>" +
                                                $"<Percent>{dcommPercent.ToString()}</Percent></Ticket>";

                                            sw.Write(" " + tktnum);

                                            if (status == cRefund)
                                            {
                                                sw.Write(" refund");
                                            }
                                            else if (status == cPartialRefund)
                                            {
                                                sw.Write(" partial refund");
                                            }
                                            else if (status == cInvoluntryRefund)
                                            {
                                                sw.Write("involuntry refund");
                                            }
                                            else if (status == cInvoluntaryPartialRefund)
                                            {
                                                sw.Write("involuntary partial refund");
                                            }
                                            else if (status == cVoid)
                                            {
                                                sw.Write(" void");
                                            }
                                        }
                                    }
                                }
                                else if (pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.TPA_Extensions !=
                                         null
                                         && pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.TPA_Extensions
                                             .IssuedTickets != null
                                         && pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.TPA_Extensions
                                             .IssuedTickets.IssuedTicket != null)
                                {

                                    DateTime dinvoiceDate = new DateTime();
                                    dinvoiceDate = DateTime.Now;

                                    foreach (AdminTools.PNRRead.IssuedTicket issuedTkt in pnrresp.OTA_TravelItineraryRS
                                        .TravelItinerary.ItineraryInfo.TPA_Extensions.IssuedTickets.IssuedTicket)
                                    {
                                        if (issuedTkt.Value.Substring(7, 1) == "-")
                                        {
                                            double dbaseFare = 0;
                                            double dtaxes = 0;
                                            double dcommAmount = 0;
                                            double dcommPercent = 0;
                                            double dtotalFare = 0;
                                            double dmarkup = 0;
                                            double dfinalComm = 0;
                                            double dagencyComm = 0;
                                            double damountDue = 0;

                                            if (issuedTkt.Value.Substring(18, 3) == "/EV"
                                                && status == cVoid)
                                            {
                                                tktnum = issuedTkt.Value.Substring(4, 14);
                                            }
                                            else
                                            {
                                                if (issuedTkt.Value.Substring(3, 1) == "-")
                                                    tktnum = issuedTkt.Value.Substring(0, 14);
                                                else
                                                    tktnum = issuedTkt.Value.Substring(4, 14);
                                            }

                                            if (customer == "21")
                                            {
                                                AdminTools.Cryptic.CrypticRS crypticRS =
                                                    new AdminTools.Cryptic.CrypticRS();

                                                crypticRS = CrypticCommand(tktnum);

                                                if (crypticRS.Screen != null)
                                                {
                                                    foreach (AdminTools.Cryptic.Line strLine in crypticRS.Screen)
                                                    {

                                                        //******************************************************
                                                        //the following 'if' condition was not in the local code 
                                                        if (strLine.Value != null)
                                                        {
                                                            //------------------------------------------------------
                                                            if (strLine.Value.StartsWith("FARE"))
                                                            {
                                                                string bf = strLine.Value.Substring(12).Trim();
                                                                if (bf != "IT" && bf != "BT")
                                                                    dbaseFare = Convert.ToDouble(bf);
                                                                else
                                                                {
                                                                    if (pnrresp.OTA_TravelItineraryRS.TravelItinerary
                                                                            .ItineraryInfo.SpecialRequestDetails
                                                                            .Remarks != null
                                                                        && pnrresp.OTA_TravelItineraryRS.TravelItinerary
                                                                            .ItineraryInfo.SpecialRequestDetails.Remarks
                                                                            .Remark[0] != null)
                                                                    {
                                                                        foreach (AdminTools.PNRRead.Remark rmk in
                                                                            pnrresp.OTA_TravelItineraryRS
                                                                                .TravelItinerary.ItineraryInfo
                                                                                .SpecialRequestDetails.Remarks.Remark)
                                                                        {
                                                                            string rmkText = rmk.Value;
                                                                            if (rmkText.IndexOf(
                                                                                    "TOTAL AMOUNT IN GDS CURRENCY") !=
                                                                                -1)
                                                                            {
                                                                                rmkText = rmkText.Replace(" ", "")
                                                                                    .Substring(30);
                                                                                dtotalFare =
                                                                                    Convert.ToDouble(rmkText.Substring(
                                                                                        0, rmkText.IndexOf("USD")));
                                                                                break;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else if (strLine.Value.StartsWith("EQUIV"))
                                                                dbaseFare = Convert.ToDouble(strLine.Value
                                                                    .Substring(12, 13).Trim());
                                                            else if (strLine.Value.StartsWith("TAX"))
                                                                dtaxes += Convert.ToDouble(strLine.Value
                                                                    .Substring(12, 13).Trim());
                                                            else if (strLine.Value.StartsWith("TOTALTAX"))
                                                                dtaxes = Convert.ToDouble(strLine.Value.Substring(12)
                                                                    .Trim());
                                                            else if (strLine.Value.StartsWith("TOTAL") &&
                                                                     dtotalFare == 0)
                                                            {
                                                                dtotalFare = Convert.ToDouble(strLine.Value
                                                                    .Substring(12).Trim());
                                                                break;
                                                            }
                                                            else if (strLine.Value.Contains("SECURED ETKT RECORD")
                                                                     || strLine.Value.Contains(
                                                                         "TICKET NUMBER NOT FOUND"))
                                                            {
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }

                                                if (pnrresp.OTA_TravelItineraryRS.TravelItinerary.TPA_Extensions !=
                                                    null && pnrresp.OTA_TravelItineraryRS.TravelItinerary.TPA_Extensions
                                                        .Items != null &&
                                                    pnrresp.OTA_TravelItineraryRS.TravelItinerary.TPA_Extensions
                                                        .Items[0] != null)
                                                {
                                                    Type comtype;
                                                    comtype = pnrresp.OTA_TravelItineraryRS.TravelItinerary
                                                        .TPA_Extensions.Items[0].GetType();
                                                    if (comtype.Name == "AgencyCommission")
                                                    {
                                                        AdminTools.PNRRead.AgencyCommission acom =
                                                            new AdminTools.PNRRead.AgencyCommission();
                                                        acom = (AdminTools.PNRRead.AgencyCommission)pnrresp
                                                            .OTA_TravelItineraryRS.TravelItinerary.TPA_Extensions
                                                            .Items[0];

                                                        dcommPercent = Convert.ToDouble(acom.Percent);
                                                        dcommAmount = Convert.ToDouble(acom.Amount);
                                                    }
                                                }

                                                if (pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo
                                                        .SpecialRequestDetails.Remarks != null
                                                    && pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo
                                                        .SpecialRequestDetails.Remarks.Remark[0] != null)
                                                {
                                                    bool noPerPaxMarkup = true;
                                                    string rmkText = "";

                                                    foreach (AdminTools.PNRRead.Remark rmk in pnrresp
                                                        .OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo
                                                        .SpecialRequestDetails.Remarks.Remark)
                                                    {
                                                        rmkText = rmk.Value;
                                                        if (rmkText.StartsWith("*THR** "))
                                                        {
                                                            rmkText = rmkText.Replace(" ", "").Substring(6);
                                                            dmarkup = Convert.ToDouble(rmkText.Substring(0,
                                                                rmkText.IndexOf("MARKUP")));
                                                            noPerPaxMarkup = false;
                                                            break;
                                                        }
                                                    }

                                                    if (noPerPaxMarkup)
                                                    {
                                                        foreach (AdminTools.PNRRead.Remark rmk in pnrresp
                                                            .OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo
                                                            .SpecialRequestDetails.Remarks.Remark)
                                                        {
                                                            rmkText = rmk.Value;
                                                            if (rmkText.IndexOf("MARKUP WAS APPLIED") != -1)
                                                            {
                                                                rmkText = rmkText.Replace(" ", "").Substring(6);
                                                                dmarkup = Convert.ToDouble(rmkText.Substring(0,
                                                                    rmkText.IndexOf("TOTAL")));

                                                                AdminTools.PNRRead.ItineraryRef recloc = pnrresp
                                                                    .OTA_TravelItineraryRS.TravelItinerary.ItineraryRef;

                                                                if (recloc.Instance != null)
                                                                {
                                                                    int iSPpax =
                                                                        Int16.Parse(recloc.Instance.Substring(11));

                                                                    dmarkup = dmarkup / (totalnip + iSPpax);
                                                                }
                                                                else
                                                                {
                                                                    dmarkup = dmarkup / totalnip;
                                                                }

                                                                break;
                                                            }
                                                        }
                                                    }
                                                }

                                                if (dbaseFare == 0 && dtotalFare > 0)
                                                {
                                                    dtotalFare = dtotalFare - dmarkup;
                                                    dbaseFare = Math.Round(dtotalFare - dtaxes, 2);
                                                }

                                                if (dcommPercent > 0)
                                                {
                                                    dfinalComm = dbaseFare * dcommPercent / 200;
                                                }
                                                else if (dcommAmount > 0)
                                                {
                                                    dfinalComm = dcommAmount / 2;
                                                }

                                                dbaseFare *= -1;
                                                dtaxes *= -1;
                                                dtotalFare *= -1;

                                                if (status == cVoid)
                                                    agencyFee = 30;

                                                if (status == cRefund || status == cPartialRefund ||
                                                    status == cInvoluntryRefund || status == cInvoluntaryPartialRefund)
                                                    dinvoiceDate = dInvoiceDate;

                                                dmarkup = agencyFee - dmarkup;

                                                dbaseFare = Math.Round(dbaseFare, 2);
                                                dtaxes = Math.Round(dtaxes, 2);
                                                dtotalFare = Math.Round(dtotalFare, 2);
                                                dmarkup = Math.Round(dmarkup, 2);
                                                dfinalComm = Math.Round(dfinalComm + 0.001, 2);
                                                dagencyComm = Math.Round(dagencyComm + 0.001, 2);
                                                damountDue = dtotalFare + dmarkup + dfinalComm + airlineFee;
                                                damountDue = Math.Round(damountDue, 2);
                                                dfinalComm *= -1;

                                                strResponse += $"<Ticket><InvoiceDate>{dinvoiceDate.ToString("yyyy-MM-dd")}</InvoiceDate>" +
                                                $"<Airline>{tktnum.Substring(0, 3)}</Airline>" +
                                                $"<TicketNumber>{tktnum.Substring(4)}</TicketNumber>" +
                                                $"<BaseFare>{dbaseFare.ToString("0.00")}</BaseFare>" +
                                                $"<TaxAmount>{dtaxes.ToString("0.00")}</TaxAmount>" +
                                                $"<TotalFare>{dtotalFare.ToString("0.00")}</TotalFare>" +
                                                $"<Standard>{dmarkup.ToString("0.00")}</Standard>" +
                                                $"<Agency>{dagencyComm.ToString("0.00")}</Agency>" +
                                                $"<Affiliate>{dfinalComm.ToString("0.00")}</Affiliate>" +
                                                $"<AirlineFee>{airlineFee.ToString("0.00")}</AirlineFee>" +
                                                $"<TotalDue>{damountDue.ToString("0.00")}</TotalDue>" +
                                                $"<Percent>{dcommPercent}</Percent></Ticket>";

                                                sw.Write(" " + tktnum);

                                                if (status == cRefund)
                                                {
                                                    sw.Write(" refund");
                                                }
                                                else if (status == cInvoluntryRefund)
                                                {
                                                    sw.Write(" involuntry refund");
                                                }
                                                else if (status == cVoid)
                                                {
                                                    sw.Write(" void");
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (customer == "AnyWayAnyDay"
                                         && pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo
                                             .SpecialRequestDetails.Remarks != null
                                         && pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo
                                             .SpecialRequestDetails.Remarks.Remark[0] != null)
                                {
                                    bool ePayPNR = false;
                                    string rmkText = "";

                                    foreach (AdminTools.PNRRead.Remark rmk in pnrresp.OTA_TravelItineraryRS
                                        .TravelItinerary.ItineraryInfo.SpecialRequestDetails.Remarks.Remark)
                                    {
                                        if (rmk.Value.Contains("EPAY"))
                                        {
                                            ePayPNR = true;
                                            break;
                                        }
                                    }

                                    if (ePayPNR)
                                    {
                                        // process refund EPAY pnr

                                        foreach (AdminTools.PNRRead.CustomerInfoRS passenger in pnrresp
                                            .OTA_TravelItineraryRS.TravelItinerary.CustomerInfos.CustomerInfo)
                                        {
                                            bool infTkt = false;
                                            string passengerType = passenger.Customer.PersonName.NameType;

                                            if (passengerType == "INF")
                                                infTkt = true;

                                            //new process to create ticket records in transactionTickets table
                                            string paxref = passenger.RPH;
                                            string strTicketNo = "EPAY " + passengerType + " " + paxref;
                                            double dbaseFare = 0;
                                            double dtaxes = 0;
                                            double dcommAmount = 0;
                                            double dcommPercent = 0;
                                            double dtotalFare = 0;
                                            double dmarkup = 0;
                                            string sfareType = "";
                                            double dfinalComm = 0;
                                            double dagencyComm = 0;
                                            double damountDue = 0;
                                            double dairlineFee = 0;
                                            DateTime dinvoiceDate = new DateTime();
                                            dinvoiceDate = DateTime.Now;

                                            if (pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo
                                                    .ReservationItems.ItemPricing != null)
                                            {
                                                foreach (AdminTools.PNRRead.PTC_FareBreakdownRS ptcFare in pnrresp
                                                    .OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo
                                                    .ReservationItems.ItemPricing[0].AirFareInfo.PTC_FareBreakdowns)
                                                {
                                                    int nip = ptcFare.PassengerTypeQuantity.Quantity;
                                                    string paxType = ptcFare.PassengerTypeQuantity.Code;

                                                    if ((infTkt == true && paxType == "INF" &&
                                                         ptcFare.TravelerRefNumberRPHList.IndexOf(paxref) != -1)
                                                        || (infTkt == false && paxType != "INF" &&
                                                            ptcFare.TravelerRefNumberRPHList.IndexOf(paxref) != -1))
                                                    {
                                                        dbaseFare = ptcFare.PassengerFare.BaseFare.Amount / (100 * nip);
                                                        dtotalFare =
                                                            ptcFare.PassengerFare.TotalFare.Amount / (100 * nip);
                                                        dtaxes = dtotalFare - dbaseFare;
                                                        sfareType = ptcFare.PricingSource.ToString();

                                                        if (pnrresp.OTA_TravelItineraryRS.TravelItinerary
                                                                .TPA_Extensions != null &&
                                                            pnrresp.OTA_TravelItineraryRS.TravelItinerary.TPA_Extensions
                                                                .Items != null && pnrresp.OTA_TravelItineraryRS
                                                                .TravelItinerary.TPA_Extensions.Items[0] != null)
                                                        {
                                                            Type comtype;
                                                            comtype = pnrresp.OTA_TravelItineraryRS.TravelItinerary
                                                                .TPA_Extensions.Items[0].GetType();
                                                            if (comtype.Name == "AgencyCommission")
                                                            {
                                                                AdminTools.PNRRead.AgencyCommission acom =
                                                                    new AdminTools.PNRRead.AgencyCommission();
                                                                acom = (AdminTools.PNRRead.AgencyCommission) pnrresp
                                                                    .OTA_TravelItineraryRS.TravelItinerary
                                                                    .TPA_Extensions.Items[0];

                                                                dcommPercent = Convert.ToDouble(acom.Percent);
                                                                dcommAmount = Convert.ToDouble(acom.Amount);
                                                            }
                                                        }

                                                        if (pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo
                                                                .SpecialRequestDetails.Remarks != null
                                                            && pnrresp.OTA_TravelItineraryRS.TravelItinerary
                                                                .ItineraryInfo.SpecialRequestDetails.Remarks
                                                                .Remark[0] != null)
                                                        {
                                                            bool noPerPaxMarkup = true;
                                                            rmkText = "";

                                                            foreach (AdminTools.PNRRead.Remark rmk in pnrresp
                                                                .OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo
                                                                .SpecialRequestDetails.Remarks.Remark)
                                                            {
                                                                rmkText = rmk.Value;
                                                                if (rmkText.StartsWith("*THR** "))
                                                                {
                                                                    rmkText = rmkText.Replace(" ", "").Substring(6);
                                                                    dmarkup = Convert.ToDouble(rmkText.Substring(0,
                                                                        rmkText.IndexOf("MARKUP")));
                                                                    noPerPaxMarkup = false;
                                                                    break;
                                                                }
                                                            }

                                                            if (noPerPaxMarkup)
                                                            {
                                                                foreach (AdminTools.PNRRead.Remark rmk in pnrresp
                                                                    .OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo
                                                                    .SpecialRequestDetails.Remarks.Remark)
                                                                {
                                                                    rmkText = rmk.Value;
                                                                    if (rmkText.IndexOf("MARKUP WAS APPLIED") != -1)
                                                                    {
                                                                        rmkText = rmkText.Replace(" ", "").Substring(6);
                                                                        dmarkup = Convert.ToDouble(rmkText.Substring(0,
                                                                            rmkText.IndexOf("TOTAL")));

                                                                        AdminTools.PNRRead.ItineraryRef recloc = pnrresp
                                                                            .OTA_TravelItineraryRS.TravelItinerary
                                                                            .ItineraryRef;

                                                                        if (recloc.Instance != null)
                                                                        {
                                                                            int iSPpax = Int16.Parse(recloc.Instance
                                                                                .Substring(11));

                                                                            dmarkup = dmarkup / (totalnip + iSPpax);
                                                                        }
                                                                        else
                                                                        {
                                                                            dmarkup = dmarkup / totalnip;
                                                                        }

                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        if (dcommPercent > 0)
                                                        {
                                                            dfinalComm = dbaseFare * dcommPercent / 200;
                                                            dagencyComm = dfinalComm;
                                                        }
                                                        else if (dcommAmount > 0)
                                                        {
                                                            dfinalComm = dcommAmount / 2;
                                                            dagencyComm = dfinalComm;
                                                        }

                                                        dbaseFare *= -1;
                                                        dtaxes *= -1;
                                                        dtotalFare *= -1;

                                                        if (status == cRefund || status == cPartialRefund ||
                                                            status == cInvoluntryRefund || status ==
                                                            cInvoluntaryPartialRefund)
                                                            dinvoiceDate = dInvoiceDate;

                                                        dbaseFare = Math.Round(dbaseFare, 2);
                                                        dtaxes = Math.Round(dtaxes, 2);
                                                        dtotalFare = Math.Round(dtotalFare, 2);
                                                        dmarkup = Math.Round(dmarkup, 2);
                                                        dfinalComm = Math.Round(dfinalComm + 0.001, 2);
                                                        dagencyComm = Math.Round(dagencyComm + 0.001, 2);
                                                        damountDue = dtotalFare + dmarkup - dfinalComm;
                                                        damountDue = Math.Round(damountDue, 2);

                                                        strResponse +=
                                                            "<Ticket><InvoiceDate>" +
                                                            dinvoiceDate.ToString("yyyy-MM-dd") +
                                                            "</InvoiceDate><Airline></Airline><TicketNumber></TicketNumber><BaseFare>" +
                                                            dbaseFare.ToString("0.00") + "</BaseFare><TaxAmount>" +
                                                            dtaxes.ToString("0.00") + "</TaxAmount><TotalFare>" +
                                                            dtotalFare.ToString("0.00") + "</TotalFare><Standard>" +
                                                            dmarkup.ToString("0.00") + "</Standard><Agency>" +
                                                            dagencyComm.ToString("0.00") + "</Agency><Affiliate>" +
                                                            dfinalComm.ToString("0.00") + "</Affiliate><AirlineFee>" +
                                                            airlineFee.ToString("0.00") + "</AirlineFee><TotalDue>" +
                                                            damountDue.ToString("0.00") + "</TotalDue><Percent>" +
                                                            dcommPercent.ToString() + "</Percent><Epay>" + strTicketNo +
                                                            "</Epay></Ticket>";

                                                        sw.Write(" " + strTicketNo);

                                                        break;
                                                    }
                                                }
                                            }

                                            sw.Write(" processed EPAY refund");
                                        }
                                    }
                                }
                                else
                                {
                                    // add notification to booking
                                    sw.Write(" no ticket number in PNR");
                                }
                            }
                            else
                            {
                                int issuedTktLen = 0;
                                string issuedTickets = "";
                                foreach (AdminTools.PNRRead.IssuedTicket issuedTkt in pnrresp.OTA_TravelItineraryRS
                                    .TravelItinerary.ItineraryInfo.TPA_Extensions.IssuedTickets.IssuedTicket)
                                {
                                    if (issuedTkt.Value.Substring(7, 1) == "-")
                                    {
                                        if (!issuedTickets.Contains(issuedTkt.Value.Substring(4, 14))
                                            || (issuedTickets.Contains(issuedTkt.Value.Substring(4, 14)) &&
                                                issuedTkt.Value.Substring(0, 3) == "INF"))
                                        {
                                            issuedTktLen++;
                                            issuedTickets = issuedTickets + "/" + issuedTkt.Value.Substring(4, 14);
                                        }
                                    }
                                }

                                issuedTickets = "";
                                int iTktIssued = 1;
                                foreach (AdminTools.PNRRead.IssuedTicket issuedTkt in pnrresp.OTA_TravelItineraryRS
                                    .TravelItinerary.ItineraryInfo.TPA_Extensions.IssuedTickets.IssuedTicket)
                                {
                                    if (status == cExchange || status == cEvenExchange || status == cPartialExchange)
                                        curDate = dInvoiceDate.ToString("ddMMMyy").ToUpper();

                                    if (issuedTkt.Value.Substring(7, 1) == "-"
                                        && (!issuedTickets.Contains(issuedTkt.Value.Substring(4, 14)) ||
                                            (issuedTickets.Contains(issuedTkt.Value.Substring(4, 14)) &&
                                             issuedTkt.Value.Substring(0, 3) == "INF"))
                                        && ((status != cExchange && status != cEvenExchange &&
                                             status != cPartialExchange)
                                            || (status == cExchange || status == cEvenExchange ||
                                                status == cPartialExchange)
                                            && (issuedTktLen == totalnip || issuedTkt.Value.Contains(curDate))))
                                    {
                                        bool infTkt = false;

                                        if (tktnum != "")
                                            tktnum = tktnum + "/" + issuedTkt.Value.Substring(4, 14);
                                        else
                                            tktnum = issuedTkt.Value.Substring(4, 14);

                                        if (issuedTkt.Value.Substring(0, 3) == "INF")
                                            infTkt = true;

                                        if (customer == "21")
                                        {
                                            //new process to create ticket records in transactionTickets table
                                            string strTicketNo = issuedTkt.Value.Substring(4, 14);
                                            string paxref = issuedTkt.TravelerRefNumberRPHList;
                                            string segref = issuedTkt.FlightRefNumberRPHList;
                                            string[] segs = segref.Split(' ');
                                            string[] paxs = paxref.Split(' ');
                                            double dbaseFare = 0;
                                            double dtaxes = 0;
                                            double dcommAmount = 0;
                                            double dcommPercent = 0;
                                            double dtotalFare = 0;
                                            double dmarkup = 0;
                                            string sfareType = "";
                                            double dfinalComm = 0;
                                            double dagencyComm = 0;
                                            double damountDue = 0;
                                            double dairlineFee = 0;
                                            DateTime dinvoiceDate = new DateTime();
                                            dinvoiceDate = DateTime.Now;

                                            if (pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo
                                                    .ReservationItems.ItemPricing != null)
                                            {
                                                foreach (AdminTools.PNRRead.PTC_FareBreakdownRS ptcFare in pnrresp
                                                    .OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo
                                                    .ReservationItems.ItemPricing[0].AirFareInfo.PTC_FareBreakdowns)
                                                {
                                                    int nip = ptcFare.PassengerTypeQuantity.Quantity;
                                                    string paxType = ptcFare.PassengerTypeQuantity.Code;

                                                    bool segInRef = true;

                                                    foreach (string seg in segs)
                                                    {
                                                        if (ptcFare.FlightRefNumberRPHList.IndexOf(seg) == -1)
                                                        {
                                                            segInRef = false;
                                                            break;
                                                        }
                                                    }

                                                    bool paxInRef = true;

                                                    foreach (string pax in paxs)
                                                    {
                                                        if (ptcFare.TravelerRefNumberRPHList.IndexOf(pax) == -1)
                                                        {
                                                            segInRef = false;
                                                            break;
                                                        }
                                                    }

                                                    if ((infTkt == true && paxType == "INF" && paxInRef && segInRef)
                                                        || (infTkt == false && paxType != "INF" && paxInRef && segInRef)
                                                    )
                                                    {
                                                        dbaseFare = ptcFare.PassengerFare.BaseFare.Amount / (100 * nip);
                                                        dtotalFare =
                                                            ptcFare.PassengerFare.TotalFare.Amount / (100 * nip);
                                                        dtaxes = dtotalFare - dbaseFare;
                                                        sfareType = ptcFare.PricingSource.ToString();

                                                        if (pnrresp.OTA_TravelItineraryRS.TravelItinerary
                                                                .TPA_Extensions != null &&
                                                            pnrresp.OTA_TravelItineraryRS.TravelItinerary.TPA_Extensions
                                                                .Items != null && pnrresp.OTA_TravelItineraryRS
                                                                .TravelItinerary.TPA_Extensions.Items[0] != null)
                                                        {
                                                            Type comtype;
                                                            comtype = pnrresp.OTA_TravelItineraryRS.TravelItinerary
                                                                .TPA_Extensions.Items[0].GetType();
                                                            if (comtype.Name == "AgencyCommission")
                                                            {
                                                                AdminTools.PNRRead.AgencyCommission acom =
                                                                    new AdminTools.PNRRead.AgencyCommission();
                                                                acom = (AdminTools.PNRRead.AgencyCommission) pnrresp
                                                                    .OTA_TravelItineraryRS.TravelItinerary
                                                                    .TPA_Extensions.Items[0];

                                                                dcommPercent = Convert.ToDouble(acom.Percent);
                                                                dcommAmount = Convert.ToDouble(acom.Amount);
                                                            }
                                                        }

                                                        if (pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo
                                                                .SpecialRequestDetails.Remarks != null
                                                            && pnrresp.OTA_TravelItineraryRS.TravelItinerary
                                                                .ItineraryInfo.SpecialRequestDetails.Remarks
                                                                .Remark[0] != null)
                                                        {
                                                            bool noPerPaxMarkup = true;
                                                            string rmkText = "";

                                                            foreach (AdminTools.PNRRead.Remark rmk in pnrresp
                                                                .OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo
                                                                .SpecialRequestDetails.Remarks.Remark)
                                                            {
                                                                rmkText = rmk.Value;
                                                                if (rmkText.StartsWith("*THR** "))
                                                                {
                                                                    rmkText = rmkText.Replace(" ", "").Substring(6);
                                                                    dmarkup = Convert.ToDouble(rmkText.Substring(0,
                                                                        rmkText.IndexOf("MARKUP")));
                                                                    noPerPaxMarkup = false;
                                                                    break;
                                                                }
                                                            }

                                                            if (noPerPaxMarkup)
                                                            {
                                                                foreach (AdminTools.PNRRead.Remark rmk in pnrresp
                                                                    .OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo
                                                                    .SpecialRequestDetails.Remarks.Remark)
                                                                {
                                                                    rmkText = rmk.Value;
                                                                    if (rmkText.IndexOf("MARKUP WAS APPLIED") != -1)
                                                                    {
                                                                        rmkText = rmkText.Replace(" ", "").Substring(6);
                                                                        dmarkup = Convert.ToDouble(rmkText.Substring(0,
                                                                            rmkText.IndexOf("TOTAL")));

                                                                        AdminTools.PNRRead.ItineraryRef recloc = pnrresp
                                                                            .OTA_TravelItineraryRS.TravelItinerary
                                                                            .ItineraryRef;

                                                                        if (recloc.Instance != null)
                                                                        {
                                                                            int iSPpax = Int16.Parse(recloc.Instance
                                                                                .Substring(11));

                                                                            dmarkup = dmarkup / (totalnip + iSPpax);
                                                                        }
                                                                        else
                                                                        {
                                                                            dmarkup = dmarkup / totalnip;
                                                                        }

                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        if (dcommPercent > 0)
                                                        {
                                                            dfinalComm = dbaseFare * dcommPercent / 200;
                                                            dagencyComm = dfinalComm;
                                                        }
                                                        else if (dcommAmount > 0)
                                                        {
                                                            dfinalComm = dcommAmount / 2;
                                                            dagencyComm = dfinalComm;
                                                        }

                                                        if (status == cExchange || status == cEvenExchange ||
                                                            status == cPartialExchange)
                                                        {
                                                            dmarkup = agencyFee;
                                                            dairlineFee = airlineFee;

                                                            double exbaseFare = 0;
                                                            double extaxes = 0;
                                                            double extotalFare = 0;
                                                            double exfareDiff = 0;

                                                            if (pnrresp.OTA_TravelItineraryRS.TravelItinerary
                                                                    .ItineraryInfo.TPA_Extensions.IssuedTickets
                                                                    .ExchangeDocument == null)
                                                            {
                                                                dbaseFare = 0;
                                                                dtaxes = 0;
                                                                dtotalFare = 0;
                                                                dfinalComm = 0;
                                                                dagencyComm = 0;
                                                            }
                                                            else
                                                            {
                                                                bool NoExchTkt = true;
                                                                bool SkipExchTkt = false;
                                                                int iExchTkt = 0;
                                                                int exchTktLen = pnrresp.OTA_TravelItineraryRS
                                                                    .TravelItinerary.ItineraryInfo.TPA_Extensions
                                                                    .IssuedTickets.ExchangeDocument.Length;

                                                                foreach (AdminTools.PNRRead.ExchangeDocument exchangeTkt
                                                                    in pnrresp.OTA_TravelItineraryRS.TravelItinerary
                                                                        .ItineraryInfo.TPA_Extensions.IssuedTickets
                                                                        .ExchangeDocument)
                                                                {
                                                                    iExchTkt++;
                                                                    
                                                                    string exchSegRef = "";

                                                                    if (exchangeTkt.FlightRefNumberRPHList != null)
                                                                        exchSegRef = exchangeTkt.FlightRefNumberRPHList;

                                                                    if (exchSegRef != "")
                                                                    {
                                                                        segInRef = true;

                                                                        foreach (string seg in segs)
                                                                        {
                                                                            if (exchangeTkt.FlightRefNumberRPHList
                                                                                    .IndexOf(seg) == -1)
                                                                            {
                                                                                segInRef = false;
                                                                                break;
                                                                            }
                                                                        }
                                                                    }

                                                                    paxInRef = true;

                                                                    foreach (string pax in paxs)
                                                                    {
                                                                        if (exchangeTkt.TravelerRefNumberRPHList
                                                                                .IndexOf(pax) == -1)
                                                                        {
                                                                            paxInRef = false;
                                                                            break;
                                                                        }
                                                                    }
                                                                    /**/
                                                                    if ((infTkt == false && paxType != "INF" &&
                                                                         paxInRef && (exchSegRef == "" || segInRef))
                                                                        || (infTkt == true && paxType == "INF" &&
                                                                            paxInRef && exchangeTkt.Infant &&
                                                                            (exchSegRef == "" || segInRef)))
                                                                    {
                                                                        NoExchTkt = false;

                                                                        if (exchangeTkt.OriginalTicket.BaseFare != null)
                                                                            exbaseFare = Convert.ToDouble(exchangeTkt
                                                                                .OriginalTicket.BaseFare);
                                                                        else
                                                                            exbaseFare = dbaseFare;

                                                                        if (exchangeTkt.OriginalTicket.Tax != null)
                                                                            extaxes = Convert.ToDouble(exchangeTkt
                                                                                .OriginalTicket.Tax);
                                                                        else
                                                                            extaxes = dtaxes;

                                                                        extotalFare = Math.Round(exbaseFare + extaxes,
                                                                            2);

                                                                        exfareDiff = dtotalFare - extotalFare;

                                                                        dbaseFare = exfareDiff;
                                                                        dtaxes = 0;
                                                                        dtotalFare = exfareDiff;

                                                                        if (exchangeTkt.Penalty != null)
                                                                            airlineFee = Convert.ToDouble(exchangeTkt
                                                                                .Penalty);
                                                                        else if (exchangeTkt.Infant)
                                                                            airlineFee = 0;
                                                                        else
                                                                            airlineFee = dairlineFee;
                                                                        
                                                                        if (dcommPercent > 0 || dcommAmount > 0)
                                                                        {
                                                                            dfinalComm = 0;
                                                                            dagencyComm = 0;
                                                                        }

                                                                        // get the ticket number of the exchanged ticket (same issue date as today)
                                                                        string todayDate =
                                                                            DateTime.Now.Date.ToString("ddMMMyy");
                                                                        todayDate = todayDate.Substring(0, 2) +
                                                                                    todayDate.Substring(2, 3)
                                                                                        .ToUpper() +
                                                                                    todayDate.Substring(5);
                                                                        string paxRef = exchangeTkt
                                                                            .TravelerRefNumberRPHList;

                                                                        foreach (AdminTools.PNRRead.IssuedTicket
                                                                            issuedTktComp in pnrresp
                                                                            .OTA_TravelItineraryRS.TravelItinerary
                                                                            .ItineraryInfo.TPA_Extensions.IssuedTickets
                                                                            .IssuedTicket)
                                                                        {
                                                                            if (issuedTktComp
                                                                                    .TravelerRefNumberRPHList == paxRef
                                                                                && issuedTktComp.Value.Contains(
                                                                                    todayDate))
                                                                            {
                                                                                strTicketNo = issuedTktComp.Value
                                                                                    .Substring(4, 14);
                                                                                break;
                                                                            }
                                                                        }


                                                                        break;
                                                                    }
                                                                    else if (iExchTkt == exchTktLen)
                                                                    {
                                                                        SkipExchTkt = true;
                                                                    }
                                                                    /**/
                                                                }

                                                                if (SkipExchTkt)
                                                                    break;

                                                                if (NoExchTkt)
                                                                {
                                                                    dbaseFare = 0;
                                                                    dtaxes = 0;
                                                                    dtotalFare = 0;
                                                                    dfinalComm = 0;
                                                                    dagencyComm = 0;

                                                                    if (infTkt == true)
                                                                        airlineFee = 0;
                                                                    else
                                                                        airlineFee = dairlineFee;
                                                                }
                                                            }

                                                            dtotalFare = Math.Round(dtotalFare, 2);
                                                            dmarkup = Math.Round(dmarkup, 2);
                                                            dfinalComm = Math.Round(dfinalComm + 0.001, 2);
                                                            airlineFee = Math.Round(airlineFee, 2);
                                                            damountDue = dtotalFare + dmarkup + dfinalComm + airlineFee;
                                                        }
                                                        else
                                                        {
                                                            string titkt = "";

                                                            if (issuedTkt.Value.IndexOf("/ET") != -1)
                                                                titkt = issuedTkt.Value.Substring(
                                                                    issuedTkt.Value.IndexOf("/ET") + 6);
                                                            else if (issuedTkt.Value.IndexOf("/EV") != -1)
                                                                titkt = issuedTkt.Value.Substring(
                                                                    issuedTkt.Value.IndexOf("/EV") + 6);
                                                            else if (issuedTkt.Value.IndexOf("/ER") != -1)
                                                                titkt = issuedTkt.Value.Substring(
                                                                    issuedTkt.Value.IndexOf("/ER") + 6);
                                                            else if (issuedTkt.Value.IndexOf("/PT") != -1)
                                                                titkt = issuedTkt.Value.Substring(
                                                                    issuedTkt.Value.IndexOf("/PT") + 6);
                                                            else if (issuedTkt.Value.IndexOf("/PV") != -1)
                                                                titkt = issuedTkt.Value.Substring(
                                                                    issuedTkt.Value.IndexOf("/PV") + 6);
                                                            else if (issuedTkt.Value.IndexOf("/PR") != -1)
                                                                titkt = issuedTkt.Value.Substring(
                                                                    issuedTkt.Value.IndexOf("/PR") + 6);
                                                            else if (issuedTkt.Value.IndexOf("/VT") != -1)
                                                                titkt = issuedTkt.Value.Substring(
                                                                    issuedTkt.Value.IndexOf("/VT") + 6);
                                                            else if (issuedTkt.Value.IndexOf("/VV") != -1)
                                                                titkt = issuedTkt.Value.Substring(
                                                                    issuedTkt.Value.IndexOf("/VV") + 6);
                                                            else if (issuedTkt.Value.IndexOf("/VR") != -1)
                                                                titkt = issuedTkt.Value.Substring(
                                                                    issuedTkt.Value.IndexOf("/VR") + 6);
                                                            else if (issuedTkt.Value.IndexOf("/DT") != -1)
                                                                titkt = issuedTkt.Value.Substring(
                                                                    issuedTkt.Value.IndexOf("/DT") + 6);
                                                            else if (issuedTkt.Value.IndexOf("/DV") != -1)
                                                                titkt = issuedTkt.Value.Substring(
                                                                    issuedTkt.Value.IndexOf("/DV") + 6);
                                                            else
                                                                titkt = issuedTkt.Value.Substring(
                                                                    issuedTkt.Value.IndexOf("/DR") + 6);

                                                            double Num;
                                                            bool isNum = double.TryParse(titkt.Substring(0, 1),
                                                                out Num);

                                                            if (isNum)
                                                                dinvoiceDate =
                                                                    Convert.ToDateTime(titkt.Substring(0, 7));
                                                            else
                                                                dinvoiceDate =
                                                                    Convert.ToDateTime(titkt.Substring(
                                                                        titkt.IndexOf("/") + 1, 7));

                                                            dtotalFare = Math.Round(dtotalFare, 2);
                                                            dmarkup = Math.Round(dmarkup, 2);
                                                            dfinalComm = Math.Round(dfinalComm + 0.001, 2);
                                                            damountDue = dtotalFare + dmarkup - dfinalComm;
                                                        }

                                                        dbaseFare = Math.Round(dbaseFare, 2);
                                                        dtaxes = Math.Round(dtaxes, 2);
                                                        dagencyComm = Math.Round(dagencyComm + 0.001, 2);
                                                        damountDue = Math.Round(damountDue, 2);

                                                        strResponse +=
                                                            "<Ticket><InvoiceDate>" +
                                                            dinvoiceDate.ToString("yyyy-MM-dd") +
                                                            "</InvoiceDate><Airline>" + strTicketNo.Substring(0, 3) +
                                                            "</Airline><TicketNumber>" + strTicketNo.Substring(4) +
                                                            "</TicketNumber><BaseFare>" + dbaseFare.ToString("0.00") +
                                                            "</BaseFare><TaxAmount>" + dtaxes.ToString("0.00") +
                                                            "</TaxAmount><TotalFare>" + dtotalFare.ToString("0.00") +
                                                            "</TotalFare><Standard>" + dmarkup.ToString("0.00") +
                                                            "</Standard><Agency>" + dagencyComm.ToString("0.00") +
                                                            "</Agency><Affiliate>" + dfinalComm.ToString("0.00") +
                                                            "</Affiliate><AirlineFee>" + airlineFee.ToString("0.00") +
                                                            "</AirlineFee><TotalDue>" + damountDue.ToString("0.00") +
                                                            "</TotalDue><Percent>" + dcommPercent.ToString() +
                                                            "</Percent></Ticket>";

                                                        sw.Write(" " + strTicketNo);

                                                        if (status == cExchange)
                                                        {
                                                            sw.Write(" exchange diff:" + dbaseFare.ToString());
                                                        }
                                                        else if (status == cEvenExchange)
                                                        {
                                                            sw.Write(" even exchange:" + dbaseFare.ToString());
                                                        }
                                                        else if (status == cPartialExchange)
                                                        {
                                                            sw.Write(" partial exchange:" + dbaseFare.ToString());
                                                        }

                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    iTktIssued += 1;
                                }
                            }

                            #region Comments

                            //string tktnum = pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.TPA_Extensions.IssuedTickets.IssuedTicket[0].Value.Substring(4, 14);
                            //double baseFare = 0;
                            //double taxes = 0;
                            //double commAmount = 0;
                            //double commPercent = 0;
                            //double totalFare = 0;
                            //double markup = 0;
                            //string fareType = "";
                            //double finalComm = 0;
                            //double amountDue = 0;

                            //if (customer == "21")
                            //{
                            //    if (pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo != null
                            //        && pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.ReservationItems != null
                            //        && pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.ReservationItems.ItemPricing != null)
                            //    {
                            //        baseFare = pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.ReservationItems.ItemPricing[0].AirFareInfo.ItinTotalFare.BaseFare.Amount / 100;
                            //        taxes = pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.ReservationItems.ItemPricing[0].AirFareInfo.ItinTotalFare.Taxes.Amount / 100;
                            //        totalFare = pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.ReservationItems.ItemPricing[0].AirFareInfo.ItinTotalFare.TotalFare.Amount / 100;
                            //        fareType = pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.ReservationItems.ItemPricing[0].AirFareInfo.PricingSource.ToString();
                            //    }

                            //    if (pnrresp.OTA_TravelItineraryRS.TravelItinerary.TPA_Extensions != null && pnrresp.OTA_TravelItineraryRS.TravelItinerary.TPA_Extensions.Items != null && pnrresp.OTA_TravelItineraryRS.TravelItinerary.TPA_Extensions.Items[0] != null)
                            //    {
                            //        Type comtype;
                            //        comtype = pnrresp.OTA_TravelItineraryRS.TravelItinerary.TPA_Extensions.Items[0].GetType();
                            //        if (comtype.Name == "AgencyCommission")
                            //        {
                            //            AdminTools.PNRRead.AgencyCommission acom = new AdminTools.PNRRead.AgencyCommission();
                            //            acom = (AdminTools.PNRRead.AgencyCommission)pnrresp.OTA_TravelItineraryRS.TravelItinerary.TPA_Extensions.Items[0];

                            //            commPercent = Convert.ToDouble(acom.Percent);
                            //            commAmount = Convert.ToDouble(acom.Amount);
                            //        }
                            //    }

                            //    if (pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.SpecialRequestDetails.Remarks != null
                            //        && pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.SpecialRequestDetails.Remarks.Remark[0] != null)
                            //    {
                            //        bool noPerPaxMarkup = true;
                            //        string rmkText = "";

                            //        foreach (AdminTools.PNRRead.Remark rmk in pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.SpecialRequestDetails.Remarks.Remark)
                            //        {
                            //            rmkText = rmk.Value;
                            //            if (rmkText.StartsWith("*THR** "))
                            //            {
                            //                rmkText = rmkText.Replace(" ", "").Substring(6);
                            //                markup = Convert.ToDouble(rmkText.Substring(0, rmkText.IndexOf("MARKUP")));
                            //                noPerPaxMarkup = false;
                            //                break;
                            //            }
                            //        }

                            //        if (noPerPaxMarkup)
                            //        {
                            //            foreach (AdminTools.PNRRead.Remark rmk in pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.SpecialRequestDetails.Remarks.Remark)
                            //            {
                            //                rmkText = rmk.Value;
                            //                if (rmkText.IndexOf("MARKUP WAS APPLIED") != -1)
                            //                {
                            //                    rmkText = rmkText.Replace(" ", "").Substring(6);
                            //                    markup = Convert.ToDouble(rmkText.Substring(0, rmkText.IndexOf("TOTAL")));
                            //                    markup = markup / totalnip;
                            //                    break;
                            //                }
                            //            }
                            //        }
                            //    }

                            //    if (commPercent > 0)
                            //    {
                            //        finalComm = baseFare * commPercent / 200;
                            //    }
                            //    else if (commAmount > 0)
                            //    {
                            //        finalComm = commAmount / 2;
                            //    }

                            //    amountDue = totalFare + markup - finalComm;

                            //    // for a ticketed PNR remove FOP from PNR if FOP is a credit card
                            //    if (status != cRefund && status != cInvoluntryRefund && status != cExchange && status != cVoid && status != cPartialExchange && status != cEvenExchange && status != cPartialRefund && status != cInvoluntaryPartialRefund)
                            //    {
                            //        if (pnrresp.OTA_TravelItineraryRS.TravelItinerary.TravelCost != null
                            //            && pnrresp.OTA_TravelItineraryRS.TravelItinerary.TravelCost.FormOfPayment != null
                            //            && pnrresp.OTA_TravelItineraryRS.TravelItinerary.TravelCost.FormOfPayment[0].PaymentCard != null)
                            //        {
                            //            string fopRPH = pnrresp.OTA_TravelItineraryRS.TravelItinerary.TravelCost.FormOfPayment[0].RPH;

                            //            AdminTools.Update.OTA_TravelItineraryRS updateRS = new AdminTools.Update.OTA_TravelItineraryRS();

                            //            updateRS = Update(fopRPH);
                            //        }
                            //    }

                            //}

                            //string strSQL = "";
                            //string historyText = "";

                            //if (status == cRefund || status == cInvoluntryRefund || status == cExchange || status == cVoid || status == cEvenExchange || status == cPartialExchange || status == cPartialRefund || status == cInvoluntaryPartialRefund)
                            //{
                            //    strSQL = "UPDATE transactions SET statusID = " + Convert.ToInt32(status) + ", modID = 36, modDateTime = '" + DateTime.Now + "', baseFare = '" + baseFare.ToString() + "' , taxes = '" + taxes.ToString() + "' , totalFare = '" + totalFare.ToString() + "' , markup = '" + markup.ToString() + "' , commissionAffiliate = '" + finalComm.ToString() + "' , commissionAgency = '" + finalComm + "' , commissionPercent = '" + commPercent.ToString() + "' , promoID2 = '" + amountDue.ToString() + "' , amountDue = '" + amountDue.ToString() + "' , TicketNumber = '" + tktnum + "' , wasModified = 'true' WHERE transactionID = " + transactionid;

                            //    if (status == cRefund)
                            //        historyText = "Processed Refund";
                            //    else if (status == cInvoluntryRefund)
                            //        historyText = "Ivoluntry Refund";
                            //    else if (status == cExchange)
                            //        historyText = "Processed Exchange";
                            //    else if (status == cVoid)
                            //        historyText = "Processed Void";
                            //    else if (status == cEvenExchange)
                            //        historyText = "Processed Even Exchange";
                            //    else if (status == cPartialExchange)
                            //        historyText = "Processed Partial Exchange";
                            //    else if (status == cPartialRefund)
                            //        historyText = "Processed Partial Refund";
                            //    else if (status == cInvoluntaryPartialRefund)
                            //        historyText = "Involuntary Partial Refund";
                            //}
                            //else
                            //{
                            //    strSQL = "UPDATE transactions SET statusID = 21, modID = 36, modDateTime = '" + DateTime.Now + "', baseFare = '" + baseFare.ToString() + "' , taxes = '" + taxes.ToString() + "' , totalFare = '" + totalFare.ToString() + "' , markup = '" + markup.ToString() + "' , commissionAffiliate = '" + finalComm.ToString() + "' , commissionAgency = '" + finalComm.ToString() + "' , commissionPercent = '" + commPercent.ToString() + "' , promoID2 = '" + amountDue.ToString() + "' , amountDue = '" + amountDue.ToString() + "' , TicketNumber = '" + tktnum + "' , wasModified = 'false' WHERE transactionID = " + transactionid;
                            //    historyText = "Change Status to E-Ticketed";
                            //}

                            //SqlConnection moConn1 = new SqlConnection();
                            ////moConn1.ConnectionString = "Data Source=RASTKOELLEIPSIS\\RASTKOTHOMALEX;Initial Catalog=BBTCustomers;Integrated Security=True";
                            //moConn1.ConnectionString = "Data Source=8.8.248.115\\DEDICATED8-VM6,1433;Initial Catalog=BBTCustomers;User ID=sa;Password=vLUIpUpTq8mx";
                            //moConn1.Open();
                            //SqlCommand oCommand1 = new SqlCommand();
                            //oCommand1.CommandType = CommandType.Text;
                            //oCommand1.CommandTimeout = 60;
                            //oCommand1.CommandText = strSQL;
                            //oCommand1.Connection = moConn1;
                            //int cnt = oCommand1.ExecuteNonQuery();

                            //strSQL = "INSERT INTO transactionHistory ([transactionId],[booking],[description],[insId],[insDateTime]) VALUES (" + transactionid + ", NULL , '" + historyText + "' ,36 ,'" + DateTime.Now + "')";

                            //oCommand1.CommandText = strSQL;
                            //cnt = oCommand1.ExecuteNonQuery();
                            //moConn1.Close();

                            //if (status != cRefund && status != cInvoluntryRefund && status != cExchange && status != cVoid && status != cEvenExchange && status != cPartialExchange && status != cPartialRefund && status != cInvoluntaryPartialRefund)
                            //{
                            //    AdminTools.Queue.OTA_QueueRS queueRS = new AdminTools.Queue.OTA_QueueRS();

                            //    queueRS = QueuePlace(pnrNumber);
                            //}

                            //sw.Write(" ticketed - amount due: $" + amountDue.ToString() + " - " + transactionid);

                            #endregion
                        }
                        else if (status == cEpay)
                        {
                            int totalnip = pnrresp.OTA_TravelItineraryRS.TravelItinerary.CustomerInfos.CustomerInfo
                                .Length;

                            foreach (AdminTools.PNRRead.CustomerInfoRS passenger in pnrresp.OTA_TravelItineraryRS
                                .TravelItinerary.CustomerInfos.CustomerInfo)
                            {
                                bool infTkt = false;
                                string passengerType = passenger.Customer.PersonName.NameType;

                                if (passengerType == "INF")
                                    infTkt = true;

                                if (customer == "21")
                                {
                                    //new process to create ticket records in transactionTickets table
                                    string paxref = passenger.RPH;
                                    string strTicketNo = "EPAY " + passengerType + " " + paxref;
                                    double dbaseFare = 0;
                                    double dtaxes = 0;
                                    double dcommAmount = 0;
                                    double dcommPercent = 0;
                                    double dtotalFare = 0;
                                    double dmarkup = 0;
                                    string sfareType = "";
                                    double dfinalComm = 0;
                                    double dagencyComm = 0;
                                    double damountDue = 0;
                                    double dairlineFee = 0;
                                    DateTime dinvoiceDate = new DateTime();
                                    dinvoiceDate = DateTime.Now;

                                    if (pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.ReservationItems
                                            .ItemPricing != null)
                                    {
                                        foreach (AdminTools.PNRRead.PTC_FareBreakdownRS ptcFare in pnrresp
                                            .OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.ReservationItems
                                            .ItemPricing[0].AirFareInfo.PTC_FareBreakdowns)
                                        {
                                            int nip = ptcFare.PassengerTypeQuantity.Quantity;
                                            string paxType = ptcFare.PassengerTypeQuantity.Code;

                                            if ((infTkt == true && paxType == "INF" &&
                                                 ptcFare.TravelerRefNumberRPHList.IndexOf(paxref) != -1)
                                                || (infTkt == false && paxType != "INF" &&
                                                    ptcFare.TravelerRefNumberRPHList.IndexOf(paxref) != -1))
                                            {
                                                dbaseFare = ptcFare.PassengerFare.BaseFare.Amount / (100 * nip);
                                                dtotalFare = ptcFare.PassengerFare.TotalFare.Amount / (100 * nip);
                                                dtaxes = dtotalFare - dbaseFare;
                                                sfareType = ptcFare.PricingSource.ToString();

                                                if (pnrresp.OTA_TravelItineraryRS.TravelItinerary.TPA_Extensions !=
                                                    null && pnrresp.OTA_TravelItineraryRS.TravelItinerary.TPA_Extensions
                                                        .Items != null &&
                                                    pnrresp.OTA_TravelItineraryRS.TravelItinerary.TPA_Extensions
                                                        .Items[0] != null)
                                                {
                                                    Type comtype;
                                                    comtype = pnrresp.OTA_TravelItineraryRS.TravelItinerary
                                                        .TPA_Extensions.Items[0].GetType();
                                                    if (comtype.Name == "AgencyCommission")
                                                    {
                                                        AdminTools.PNRRead.AgencyCommission acom = new AdminTools.PNRRead.AgencyCommission();
                                                        acom = (AdminTools.PNRRead.AgencyCommission) pnrresp.OTA_TravelItineraryRS.TravelItinerary.TPA_Extensions.Items[0];

                                                        dcommPercent = Convert.ToDouble(acom.Percent);
                                                        dcommAmount = Convert.ToDouble(acom.Amount);
                                                    }
                                                }

                                                if (pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.SpecialRequestDetails.Remarks != null
                                                    && pnrresp.OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo.SpecialRequestDetails.Remarks.Remark[0] != null)
                                                {
                                                    bool noPerPaxMarkup = true;
                                                    string rmkText;

                                                    foreach (AdminTools.PNRRead.Remark rmk in pnrresp
                                                        .OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo
                                                        .SpecialRequestDetails.Remarks.Remark)
                                                    {
                                                        rmkText = rmk.Value;
                                                        if (rmkText.StartsWith("*THR** "))
                                                        {
                                                            rmkText = rmkText.Replace(" ", "").Substring(6);
                                                            dmarkup = Convert.ToDouble(rmkText.Substring(0,
                                                                rmkText.IndexOf("MARKUP")));
                                                            noPerPaxMarkup = false;
                                                            break;
                                                        }
                                                    }

                                                    if (noPerPaxMarkup)
                                                    {
                                                        foreach (AdminTools.PNRRead.Remark rmk in pnrresp
                                                            .OTA_TravelItineraryRS.TravelItinerary.ItineraryInfo
                                                            .SpecialRequestDetails.Remarks.Remark)
                                                        {
                                                            rmkText = rmk.Value;
                                                            if (rmkText.IndexOf("MARKUP WAS APPLIED") != -1)
                                                            {
                                                                rmkText = rmkText.Replace(" ", "").Substring(6);
                                                                dmarkup = Convert.ToDouble(rmkText.Substring(0,
                                                                    rmkText.IndexOf("TOTAL")));

                                                                AdminTools.PNRRead.ItineraryRef recloc = pnrresp
                                                                    .OTA_TravelItineraryRS.TravelItinerary.ItineraryRef;

                                                                if (recloc.Instance != null)
                                                                {
                                                                    int iSPpax =
                                                                        Int16.Parse(recloc.Instance.Substring(11));

                                                                    dmarkup = dmarkup / (totalnip + iSPpax);
                                                                }
                                                                else
                                                                {
                                                                    dmarkup = dmarkup / totalnip;
                                                                }

                                                                break;
                                                            }
                                                        }
                                                    }
                                                }

                                                if (dcommPercent > 0)
                                                {
                                                    dfinalComm = dbaseFare * dcommPercent / 200;
                                                    dagencyComm = dfinalComm;
                                                }
                                                else if (dcommAmount > 0)
                                                {
                                                    dfinalComm = dcommAmount / 2;
                                                    dagencyComm = dfinalComm;
                                                }

                                                dinvoiceDate =
                                                    pnrresp.OTA_TravelItineraryRS.TravelItinerary.UpdatedBy
                                                        .CreateDateTime.AddHours(-4);

                                                dbaseFare = Math.Round(dbaseFare, 2);
                                                dtaxes = Math.Round(dtaxes, 2);
                                                dtotalFare = Math.Round(dtotalFare, 2);
                                                dmarkup = Math.Round(dmarkup, 2);
                                                dfinalComm = Math.Round(dfinalComm + 0.001, 2);
                                                dagencyComm = Math.Round(dagencyComm + 0.001, 2);
                                                damountDue = dtotalFare + dmarkup - dfinalComm;
                                                damountDue = Math.Round(damountDue, 2);

                                                strResponse +=
                                                    $"<Ticket><InvoiceDate>{dinvoiceDate:yyyy-MM-dd}</InvoiceDate><Airline></Airline><TicketNumber></TicketNumber>" +
                                                    $"<BaseFare>{dbaseFare:0.00}</BaseFare><TaxAmount>{dtaxes:0.00}</TaxAmount><TotalFare>{dtotalFare:0.00}</TotalFare>" + 
                                                    $"<Standard>{dmarkup:0.00}</Standard><Agency>{dagencyComm:0.00}</Agency><Affiliate>{dfinalComm:0.00}</Affiliate>" + 
                                                    $"<AirlineFee>{airlineFee:0.00}</AirlineFee><TotalDue>{damountDue:0.00}</TotalDue><Percent>{dcommPercent}</Percent><Epay>{strTicketNo}</Epay></Ticket>";

                                                sw.Write(" " + strTicketNo);

                                                break;
                                            }
                                        }
                                    }
                                }

                                if (customer == "21")
                                {
                                    sw.Write(" processed EPAY");
                                }
                            }
                        }

                        sw.Close();
                    }
                }

                strResponse += "</Tickets>";
                return strResponse;
            }
            catch (Exception ex)
            {
                var mail = new MailMessage("adminmanager@downtowntravel.com", "itadmin@downtowntravel.com", "AdminManager exception error", $"PNR:{pnrid.ID}  {ex.Message}");
                mail.IsBodyHtml = false;
                var smtp = new SmtpClient("maillist.anaxanet.com", 25);
                smtp.UseDefaultCredentials = false;

                smtp.Send(mail);
                return strResponse;
            }
            finally
            {
                sw = new StreamWriter("C:\\tripxml\\log\\adminprocess.txt", true, Encoding.ASCII);
                sw.Write($"\r\nPNRs processed: {oRoot.SelectNodes("PNR").Count}\r\n");
                sw.Write($"\r\nEnd process: {DateTime.Now}\r\n");
                sw.Close();
            }
        }

        static public AdminTools.Cryptic.CrypticRS CrypticCommand(string entry)
        {
            var cryptic = new AdminTools.Cryptic.wmCrypticRequest();
            var crypticresp = new AdminTools.Cryptic.wmCrypticResponse();
            //var crypticrespota = new AdminTools.Cryptic.CrypticRS();
            var crypticsaop = new AdminTools.Cryptic.wsCrypticSoapClient();
            var crypticota = new AdminTools.Cryptic.CrypticRQ();
            var crypticpos = new AdminTools.Cryptic.POS();
            var crypticsource = new AdminTools.Cryptic.Source[1];
            crypticsource[0] = new AdminTools.Cryptic.Source();
            var crypticreqid = new AdminTools.Cryptic.RequestorID();
            var crypticprovider = new AdminTools.Cryptic.Provider[1];
            crypticprovider[0] = new AdminTools.Cryptic.Provider();
            crypticsource[0].PseudoCityCode = "NYC1S211F";
            crypticreqid.ID = "DowntownTravel";
            //crypticreqid.Type = "21";
            crypticsource[0].RequestorID = crypticreqid;
            crypticprovider[0].Name = "Amadeus";
            crypticprovider[0].System = "Production";
            crypticprovider[0].Userid = "DTT";
            crypticprovider[0].Password = "sm1rN0ff";
            crypticpos.Source = crypticsource;
            var tpaext = new AdminTools.Cryptic.TPA_Extensions {Provider = crypticprovider[0]};
            //tpaext.ConversationID = _sessionID;
            crypticota.POS = crypticpos;
            crypticpos.TPA_Extensions = tpaext;
            crypticota.Entry = "TWD/TKT" + entry;
            cryptic.CrypticRQ = crypticota;

            crypticresp.CrypticRS = crypticsaop.wmCryptic(null, cryptic.CrypticRQ);

            return crypticresp.CrypticRS;
        }

        static public AdminTools.Update.OTA_TravelItineraryRS Update(string elemRPH)
        {
            var update = new AdminTools.Update.wmUpdateRequest();
            var updateresp = new AdminTools.Update.wmUpdateResponse();
            var updaterespota = new AdminTools.Update.OTA_TravelItineraryRS();
            var updatesaop = new AdminTools.Update.wsUpdateSoapClient();
            var updateota = new AdminTools.Update.OTA_UpdateRQ();
            var updatepos = new AdminTools.Update.POS_Type();
            var updatesource = new AdminTools.Update.SourceType[1];
            updatesource[0] = new AdminTools.Update.SourceType();
            var updatereqid = new AdminTools.Update.SourceTypeRequestorID();
            var updateprovider = new AdminTools.Update.Provider();
            updatesource[0].PseudoCityCode = "NYC1S211F";
            updatereqid.ID = "DowntownTravel";
            updatereqid.Type = "21";
            updatesource[0].RequestorID = updatereqid;
            updateprovider.Name = "Amadeus";
            updateprovider.System = "Production";
            updateprovider.Userid = "DTT";
            updateprovider.Password = "Tr@v3l";
            updatepos.Source = updatesource;
            var updatetpa = new AdminTools.Update.TPA_Extensions();
            updatetpa.Provider = updateprovider;
            updatepos.TPA_Extensions = updatetpa;
            updateota.POS = updatepos;
            var updatePosition = new AdminTools.Update.UpdatePositionType[1];
            updatePosition[0] = new AdminTools.Update.UpdatePositionType();
            updatePosition[0].XPath = "OTA_TravelItineraryRS/TravelItinerary/TravelCost";
            var updateElement = new AdminTools.Update.Element();
            updateElement.Operation = AdminTools.Update.ElementOperation.delete;
            updateElement.Child = "TravelCost";

            var oDoc = new XmlDocument();
            XmlElement elemTravelCost = oDoc.CreateElement("TravelCost");
            XmlNode fop = oDoc.CreateNode(XmlNodeType.Element, "FormOfPayment", "");
            XmlAttribute rph = oDoc.CreateAttribute("RPH");
            rph.Value = elemRPH;
            fop.Attributes.Append(rph);
            elemTravelCost.AppendChild(fop);
            updateElement.Any = new []{ elemTravelCost };

            var oItems = new object[1];
            oItems[0] = updateElement;
            updatePosition[0].Items = oItems;
            updateota.Position = updatePosition;

            update.OTA_UpdateRQ = updateota;

            updateresp.OTA_TravelItineraryRS = updatesaop.wmUpdate(null, update.OTA_UpdateRQ);

            return updateresp.OTA_TravelItineraryRS;
        }

        static public AdminTools.Queue.OTA_QueueRS QueuePlace(string pnrNumber)
        {
            AdminTools.Queue.wmQueueRequest queue = new AdminTools.Queue.wmQueueRequest();
            AdminTools.Queue.wmQueueResponse queueresp = new AdminTools.Queue.wmQueueResponse();
            AdminTools.Queue.OTA_QueueRS queuerespota = new AdminTools.Queue.OTA_QueueRS();
            AdminTools.Queue.wsQueueSoapClient queuesaop = new AdminTools.Queue.wsQueueSoapClient();
            AdminTools.Queue.OTA_QueueRQ queueota = new AdminTools.Queue.OTA_QueueRQ();
            AdminTools.Queue.POS queuepos = new AdminTools.Queue.POS();
            AdminTools.Queue.Source[] queuesource = new AdminTools.Queue.Source[1];
            queuesource[0] = new AdminTools.Queue.Source();
            AdminTools.Queue.RequestorID queuereqid = new AdminTools.Queue.RequestorID();
            AdminTools.Queue.Provider[] queueprovider = new AdminTools.Queue.Provider[1];
            queueprovider[0] = new AdminTools.Queue.Provider();
            //QueueRead.UniqueID queueid = new QueueRead.UniqueID();
            queuesource[0].PseudoCityCode = "NYC1S211F";
            queuereqid.ID = "DowntownTravel";
            queuereqid.Type = "21";
            queuesource[0].RequestorID = queuereqid;
            queueprovider[0].Name = "Amadeus";
            queueprovider[0].System = "Production";
            queueprovider[0].Userid = "DTT";
            queueprovider[0].Password = "Tr@v3l";
            queuepos.Source = queuesource;
            queuepos.TPA_Extensions = queueprovider;
            queueota.POS = queuepos;
            AdminTools.Queue.PlaceQueue[] queueplace = new AdminTools.Queue.PlaceQueue[1];
            queueplace[0] = new AdminTools.Queue.PlaceQueue();
            queueplace[0].Number = "55";
            queueplace[0].Category = "1";
            queueplace[0].PseudoCityCode = "NYC1S211F";
            AdminTools.Queue.UniqueID queueuniqueid = new AdminTools.Queue.UniqueID();
            queueuniqueid.ID = pnrNumber;
            queueuniqueid.Type = "21";
            queueplace[0].UniqueID = queueuniqueid;
            queueota.PlaceQueue = queueplace;
            queue.OTA_QueueRQ = queueota;

            queueresp.OTA_QueueRS = queuesaop.wmQueue(null, queue.OTA_QueueRQ);

            return queueresp.OTA_QueueRS;
        }
    }
}

