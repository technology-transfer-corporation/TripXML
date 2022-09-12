using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;

namespace TripXML.Library
{
    public class cDA
    {
        private SqlConnection moConn;

        public cDA(string CnxString = "ConnectionString")
        {
            OpenConnection(CnxString);
        }

        public void Dispose()
        {
            if (moConn is object)
            {
                if (moConn.State == ConnectionState.Open)
                    moConn.Close();
                moConn.Dispose();
                moConn = null;
            }
        }

        private bool OpenConnection(string CnxString)
        {
            string server;
            string database;
            string user;
            string password;
            string cnnString;
            StringBuilder sb;
            try
            {
                if (CnxString == "BEConnectionString")
                {

/* Unmerged change from project 'TripXML.Library (net6.0)'
Before:
                    cnnString = Core.modCore.config.GetSection("BEConnectionString").ToString().Trim();
After:
                    cnnString = modCore.config.GetSection("BEConnectionString").ToString().Trim();
*/
                    cnnString = Library.modCore.config.GetSection("BEConnectionString").ToString().Trim();
                }
                else if (CnxString == "DataDatabase")
                {

/* Unmerged change from project 'TripXML.Library (net6.0)'
Before:
                    cnnString = Core.modCore.config.GetSection("DataDatabase").ToString().Trim();
After:
                    cnnString = modCore.config.GetSection("DataDatabase").ToString().Trim();
*/
                    cnnString = Library.modCore.config.GetSection("DataDatabase").ToString().Trim();
                }
                else
                {

/* Unmerged change from project 'TripXML.Library (net6.0)'
Before:
                    cnnString = Core.modCore.config.GetSection("ConnectionString").ToString().Trim();
After:
                    cnnString = modCore.config.GetSection("ConnectionString").ToString().Trim();
*/
                    cnnString = Library.modCore.config.GetSection("ConnectionString").ToString().Trim();
                    if (cnnString.Length == 0)
                    {

/* Unmerged change from project 'TripXML.Library (net6.0)'
Before:
                        server = Core.modCore.config.GetSection("Server").ToString().Trim(); ;
                        database = Core.modCore.config.GetSection("Database").ToString().Trim(); ;
                        user = Core.modCore.config.GetSection("User").ToString().Trim(); ;
                        password = Core.modCore.config.GetSection("Password").ToString().Trim(); ;
After:
                        server = modCore.config.GetSection("Server").ToString().Trim(); ;
                        database = modCore.config.GetSection("Database").ToString().Trim(); ;
                        user = modCore.config.GetSection("User").ToString().Trim(); ;
                        password = modCore.config.GetSection("Password").ToString().Trim(); ;
*/
                        server = Library.modCore.config.GetSection("Server").ToString().Trim(); ;
                        database = Library.modCore.config.GetSection("Database").ToString().Trim(); ;
                        user = Library.modCore.config.GetSection("User").ToString().Trim(); ;
                        password = Library.modCore.config.GetSection("Password").ToString().Trim(); ;
                        sb = new StringBuilder();
                        sb.Append("data source=").Append(server).Append(";initial Catalog=").Append(database).Append(";User ID=").Append(user).Append(";Password=").Append(password);
                        cnnString = sb.ToString();
                    }
                }

                moConn = new SqlConnection();
                moConn.ConnectionString = cnnString;
                moConn.Open();
            }
            catch (Exception ex)
            {
                throw;
            }

            return true;
        }

        public void AddMessageLog(int LogTypeID, ref string UUID, ref string WebServer, ref string Customer, ref string UserName, ref string Provider, int MessageID, ref string Message, DateTime MessageDate, int ResponseTime)


        {
            SqlCommand oCommand = null;
            string logMessage = Message;
            try
            {
                oCommand = new SqlCommand("uspAddMessageLog", moConn);
                oCommand.CommandType = CommandType.StoredProcedure;
                oCommand.CommandTimeout = 60;
                {
                    var withBlock = oCommand.Parameters;
                    withBlock.Add(new SqlParameter("@LogTypeID", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, LogTypeID));
                    withBlock.Add(new SqlParameter("@UUID", SqlDbType.Char, 35, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, UUID));
                    withBlock.Add(new SqlParameter("@WebServer", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, WebServer));
                    withBlock.Add(new SqlParameter("@Customer", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, Customer));
                    withBlock.Add(new SqlParameter("@UserName", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, UserName));
                    withBlock.Add(new SqlParameter("@Provider", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, Provider));
                    withBlock.Add(new SqlParameter("@MessageID", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, MessageID));
                    withBlock.Add(new SqlParameter("@MessageDate", SqlDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, MessageDate));
                    withBlock.Add(new SqlParameter("@ResponseTime", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, ResponseTime));
                    withBlock.Add(new SqlParameter("@Message", logMessage));
                }

                oCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (oCommand is object)
                {
                    oCommand.Dispose();
                }
            }
        }

        public void AddDeals(string strRequest, string strResponse)
        {
            SqlCommand oCommand = null;
            // Dim strSQL As String = ""
            string logMessage;
            XmlDocument oDoc;
            XmlElement oRoot;
            XmlNodeList oNodes;
            string departureCity;
            string arrivalCity;
            string departureDate;
            string returnDate = "";
            decimal fareAmount;
            decimal markup = 0m;
            string fareType;
            string airline = "";
            string officeID;
            DateTime insertDate;
            var tripType = default(string);
            try
            {
                logMessage = "<LogMessage>" + strRequest.Replace("OTA_AirLowFareSearchPlusRQ", "OTA_AirLowFareSearchRQ").Replace("OTA_AirLowFareSearchScheduleRQ", "OTA_AirLowFareSearchRQ").Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "") + strResponse.Replace("OTA_AirLowFareSearchPlusRS", "OTA_AirLowFareSearchRS").Replace("OTA_AirLowFareSearchScheduleRS", "OTA_AirLowFareSearchRS") + "</LogMessage>";
                oDoc = new XmlDocument();
                oDoc.LoadXml(logMessage);
                oRoot = oDoc.DocumentElement;
                if (oRoot.SelectSingleNode("OTA_AirLowFareSearchRS/Success") is object)
                {
                    oNodes = oRoot.SelectNodes("OTA_AirLowFareSearchRQ/OriginDestinationInformation");
                    if (oNodes.Count < 3)
                    {
                        if (oNodes.Count == 1)
                        {
                            tripType = "O";
                        }
                        else if (oNodes.Count == 2)
                        {
                            tripType = "R";
                        }

                        officeID = oRoot.SelectSingleNode("OTA_AirLowFareSearchRQ/POS/Source/@PseudoCityCode").InnerText;
                        departureCity = oRoot.SelectSingleNode("OTA_AirLowFareSearchRQ/OriginDestinationInformation[1]/OriginLocation/@LocationCode").InnerText;
                        arrivalCity = oRoot.SelectSingleNode("OTA_AirLowFareSearchRQ/OriginDestinationInformation[1]/DestinationLocation/@LocationCode").InnerText;
                        departureDate = oRoot.SelectSingleNode("OTA_AirLowFareSearchRQ/OriginDestinationInformation[1]/DepartureDateTime").InnerText.Substring(0, 10);
                        if (oNodes.Count == 2)
                        {
                            returnDate = oRoot.SelectSingleNode("OTA_AirLowFareSearchRQ/OriginDestinationInformation[position()=2]/DepartureDateTime").InnerText.Substring(0, 10);
                        }

                        fareAmount = Convert.ToDecimal(oRoot.SelectSingleNode("OTA_AirLowFareSearchRS/PricedItineraries/PricedItinerary[1]/AirItineraryPricingInfo/PTC_FareBreakdowns/PTC_FareBreakdown[1]/PassengerFare/TotalFare/@Amount").InnerText);
                        int nip = Convert.ToInt32(oRoot.SelectSingleNode("OTA_AirLowFareSearchRS/PricedItineraries/PricedItinerary[1]/AirItineraryPricingInfo/PTC_FareBreakdowns/PTC_FareBreakdown[1]/PassengerTypeQuantity/@Quantity").InnerText);
                        fareAmount = fareAmount / nip / 100m;
                        var oFeeNode = oRoot.SelectSingleNode("OTA_AirLowFareSearchRS/PricedItineraries/PricedItinerary[1]/AirItineraryPricingInfo/PTC_FareBreakdowns/PTC_FareBreakdown[1]/PassengerFare/Fees/Fee/@Amount");
                        if (oFeeNode is object)
                        {
                            markup = Convert.ToDecimal(oFeeNode.InnerText);
                            markup = markup / nip / 100m;
                        }

                        fareType = oRoot.SelectSingleNode("OTA_AirLowFareSearchRS/PricedItineraries/PricedItinerary[1]/AirItineraryPricingInfo/@PricingSource").InnerText;
                        if (fareType == "Published")
                        {
                            fareType = "P";
                        }
                        else
                        {
                            fareType = "U";
                        }

                        var oAirNodes = oRoot.SelectNodes("OTA_AirLowFareSearchRS/PricedItineraries/PricedItinerary[1]/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment");
                        foreach (XmlNode oAirNode in oAirNodes)
                            airline = airline + oAirNode.SelectSingleNode("OperatingAirline/@Code").InnerText + "/";
                        string airtemp = airline.Replace(airline.Substring(0, 3), "");
                        if (string.IsNullOrEmpty(airtemp))
                        {
                            airline = airline.Substring(0, 2);
                        }
                        else
                        {
                            airline = "";
                        }

                        insertDate = DateTime.Now;
                        oCommand = new SqlCommand("uspAddDeals", moConn);
                        oCommand.CommandType = CommandType.StoredProcedure;
                        oCommand.CommandTimeout = 60;
                        {
                            var withBlock = oCommand.Parameters;
                            withBlock.Add(new SqlParameter("@DepartureCity", SqlDbType.Char, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, departureCity));
                            withBlock.Add(new SqlParameter("@ArrivalCity", SqlDbType.Char, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, arrivalCity));
                            withBlock.Add(new SqlParameter("@DepartureDate", SqlDbType.Char, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, departureDate));
                            withBlock.Add(new SqlParameter("@ReturnDate", SqlDbType.Char, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, returnDate));
                            withBlock.Add(new SqlParameter("@TripType", SqlDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, tripType));
                            withBlock.Add(new SqlParameter("@FareAmount", SqlDbType.Decimal, 18, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, fareAmount));
                            withBlock.Add(new SqlParameter("@Markup", SqlDbType.Decimal, 18, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, markup));
                            withBlock.Add(new SqlParameter("@FareType", SqlDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, fareType));
                            withBlock.Add(new SqlParameter("@Airline", SqlDbType.Char, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, airline));
                            withBlock.Add(new SqlParameter("@OfficeID", SqlDbType.VarChar, 9, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, officeID));
                            withBlock.Add(new SqlParameter("@InsertDate", SqlDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, insertDate));
                        }

                        oCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (oCommand is object)
                {
                    oCommand.Dispose();
                }
            }
        }

        public string GetDeals(string strRequest)
        {
            SqlCommand oCommand = null;
            // Dim strSQL As String = ""
            XmlDocument oDoc;
            XmlElement oRoot;
            XmlNode oNode = null;
            // Dim oNodes As XmlNodeList = Nothing
            string strResponse;
            string departureCity;
            string arrivalCity = null;
            string departureDate;
            string returnDate = null;
            string fareType = null;
            string airline = null;
            string officeID;
            string tripType = null;
            SqlDataReader dr;
            StringBuilder sb;
            int iRPH = 0;
            try
            {
                oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                oRoot = oDoc.DocumentElement;
                strResponse = "<TXML_GetDealsRS Version=\"1.0\">";
                if (oRoot.SelectSingleNode("OriginDestinationInformation/DepartureDate") is object & oRoot.SelectSingleNode("OriginDestinationInformation/OriginLocation/@LocationCode") is object & oRoot.SelectSingleNode("OriginDestinationInformation/@TripType") is object)
                {
                    if (oRoot.SelectSingleNode("OriginDestinationInformation/@TripType").InnerText == "OneWay")
                    {
                        tripType = "O";
                    }
                    else if (oRoot.SelectSingleNode("OriginDestinationInformation/@TripType").InnerText == "RoundTrip")
                    {
                        tripType = "R";
                    }

                    officeID = oRoot.SelectSingleNode("POS/Source/@PseudoCityCode").InnerText;
                    departureCity = oRoot.SelectSingleNode("OriginDestinationInformation/OriginLocation/@LocationCode").InnerText;
                    if (oRoot.SelectSingleNode("OriginDestinationInformation/DestinationLocation/@LocationCode") is object)
                    {
                        arrivalCity = oRoot.SelectSingleNode("OriginDestinationInformation/DestinationLocation/@LocationCode").InnerText;
                    }

                    departureDate = oRoot.SelectSingleNode("OriginDestinationInformation/DepartureDate").InnerText.Substring(0, 10);
                    if (oRoot.SelectSingleNode("OriginDestinationInformation/ReturnDate") is object)
                    {
                        returnDate = oRoot.SelectSingleNode("OriginDestinationInformation/ReturnDate").InnerText.Substring(0, 10);
                    }

                    if (oRoot.SelectSingleNode("FareType") is object)
                    {
                        fareType = oRoot.SelectSingleNode("FareType").InnerText;
                        if (fareType == "Published")
                        {
                            fareType = "P";
                        }
                        else
                        {
                            fareType = "U";
                        }
                    }

                    if (oRoot.SelectSingleNode("VendorPref/@Code") is object)
                    {
                        airline = oRoot.SelectSingleNode("VendorPref/@Code").InnerText;
                    }

                    oNode = null;
                    oCommand = new SqlCommand("uspGetDeals", moConn);
                    oCommand.CommandType = CommandType.StoredProcedure;
                    oCommand.CommandTimeout = 60;
                    {
                        var withBlock = oCommand.Parameters;
                        withBlock.Add(new SqlParameter("@DepartureCity", SqlDbType.Char, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, departureCity));
                        withBlock.Add(new SqlParameter("@ArrivalCity", SqlDbType.Char, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, arrivalCity));
                        withBlock.Add(new SqlParameter("@DepartureDate", SqlDbType.Char, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, departureDate));
                        withBlock.Add(new SqlParameter("@ReturnDate", SqlDbType.Char, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, returnDate));
                        withBlock.Add(new SqlParameter("@TripType", SqlDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, tripType));
                        withBlock.Add(new SqlParameter("@FareType", SqlDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, fareType));
                        withBlock.Add(new SqlParameter("@Airline", SqlDbType.Char, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, airline));
                        withBlock.Add(new SqlParameter("@OfficeID", SqlDbType.VarChar, 9, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, officeID));
                    }

                    dr = oCommand.ExecuteReader();
                }
                else
                {
                    throw new Exception("Manadatory elements missing");
                }

                sb = new StringBuilder();
                sb.Append("<Success/><Deals>");
                do
                {
                    while (dr.Read())
                    {
                        iRPH = iRPH + 1;
                        sb.Append("<Deal RPH=\"").Append(iRPH.ToString());
                        sb.Append("\"><OriginDestinationInformation>");
                        sb.Append("<DepartureDate>");
                        sb.Append(dr.GetValue(3).ToString());
                        sb.Append("</DepartureDate>");
                        if (dr.GetValue(5).ToString() == "R")
                        {
                            sb.Append("<ReturnDate>");
                            sb.Append(dr.GetValue(4).ToString());
                            sb.Append("</ReturnDate>");
                        }

                        sb.Append("<OriginLocation LocationCode=\"");
                        sb.Append(dr.GetValue(1).ToString());
                        sb.Append("\"/>");
                        sb.Append("<DestinationLocation LocationCode=\"");
                        sb.Append(dr.GetValue(2).ToString());
                        sb.Append("\"/>");
                        if (dr.GetValue(9).ToString() != "  ")
                        {
                            sb.Append("<MarketingAirline Code=\"");
                            sb.Append(dr.GetValue(9).ToString());
                            sb.Append("\"/>");
                        }

                        sb.Append("</OriginDestinationInformation>");
                        sb.Append("<FareInfo FareType=\"");
                        if (dr.GetValue(8).ToString() == "P")
                        {
                            sb.Append("Published");
                        }
                        else
                        {
                            sb.Append("Private");
                        }

                        sb.Append("\">");
                        sb.Append("<TotalAmount>");
                        sb.Append(dr.GetValue(6).ToString());
                        sb.Append("</TotalAmount>");
                        sb.Append("<IncludedMarkup>");
                        sb.Append(dr.GetValue(7).ToString());
                        sb.Append("</IncludedMarkup></FareInfo>");
                        sb.Append("</Deal>");
                    }

                    if (dr.NextResult() == default)
                    {
                        break;
                    }
                }
                while (true);
                sb.Append("</Deals>");
                if (sb.ToString().Contains("<Deal RPH"))
                {
                    strResponse = strResponse + sb.ToString();
                }
                else
                {
                    strResponse = strResponse + "<Errors><Error>No deals available</Error></Errors>";
                }

                dr.Close();
                strResponse = strResponse + "</TXML_GetDealsRS>";

/* Unmerged change from project 'TripXML.Library (net6.0)'
Before:
                strResponse = Core.CoreLib.TransformXML(strResponse, @"C:\\TRIPXML\\XSL\\TXML\\", "TXML_GetDealsRS.xsl");
After:
                strResponse = Library.CoreLib.TransformXML(strResponse, @"C:\\TRIPXML\\XSL\\TXML\\", "TXML_GetDealsRS.xsl");
*/
                strResponse = CoreLib.TransformXML(strResponse, @"C:\\TRIPXML\\XSL\\TXML\\", "TXML_GetDealsRS.xsl");
                return strResponse;
            }
            catch (Exception ex)
            {
                strResponse = $"<TXML_GetDealsRS><Errors><Error>{ex.Message}</Error></Errors></TXML_GetDealsRS>";
                return strResponse;
            }
            finally
            {
                if (oCommand is object)
                {
                    oCommand.Dispose();
                }
            }
        }

        public bool CheckSession(string SessionID, bool isSOAP2) // Check conditions when closing - by Shashin - 23-02-2010
        {
            var oCommand = new SqlCommand();
            string strSQL;
            SqlDataAdapter oAdapter = null;
            var dtTest1 = new DataTable();
            var dtTest2 = new DataTable();
            int cnt = 0;

            try
            {
                string tSession = "";
                try
                {
                    if (isSOAP2)
                    {
                        tSession = SessionID.Substring(0, SessionID.LastIndexOf("|")); // ignore the sequence number
                    }
                    else
                    {
                        tSession = SessionID.Substring(0, SessionID.IndexOf("|"));
                    } // ignore the sequence number
                }
                catch (Exception ex)
                {
                    tSession = SessionID;
                }

                strSQL = "select sessionid from tblSessionPool where sessionid like '" + tSession + "%'";
                oCommand.CommandText = strSQL;
                oCommand.Connection = moConn;
                oAdapter = new SqlDataAdapter(oCommand);
                oAdapter.Fill(dtTest1);
                if (dtTest1.Rows.Count == 0)
                {
                    return false;
                }

                strSQL = "select sessionID from tblSessionPool where sessionid like '" + tSession + "%' and ToBeDeleted='Y'";
                oCommand.CommandText = strSQL;
                oAdapter.Fill(dtTest2);
                if (dtTest2.Rows.Count > 0)
                {
                    if (!(moConn.State == ConnectionState.Open))
                    {
                        moConn.Open();
                    }

                    strSQL = "delete from tblSessionPool where sessionid like '" + tSession + "%'";
                    oCommand.CommandText = strSQL;
                    cnt = oCommand.ExecuteNonQuery();
                    strSQL = "update tblPCCBlocks set sessionsUsed=sessionsUsed-1 where PCC=(SELECT top 1  [PCC]() FROM tblSessionPool where sessionid like '" + tSession + "%')";
                    oCommand.CommandText = strSQL;
                    cnt = oCommand.ExecuteNonQuery();
                    return false;
                }

                if (!(moConn.State == ConnectionState.Open))
                {
                    moConn.Open();
                }

                strSQL = "update tblSessionPool set LastMessageTime=getdate(),IsUse='N' where sessionid like '" + tSession + "%'";
                oCommand.CommandText = strSQL;
                cnt = oCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                dtTest1.Dispose();
                dtTest2.Dispose();
                oAdapter.Dispose();
                oCommand.Dispose();
            }
        }

        public DataTable ToBeDeleted(string SessionID)
        {
            var oCommand = new SqlCommand();
            string strSQL;
            SqlDataAdapter oAdapter = null;
            var dtTest1 = new DataTable();
            var dtTest2 = new DataTable();
            int cnt = 0;
            DataTable strRet;

            try
            {

                string tSession = "";
                try
                {
                    tSession = SessionID.Substring(0, SessionID.IndexOf("|")); // ignore the sequence number
                }
                catch (Exception ex)
                {
                    tSession = SessionID;
                }

                strSQL = "select sessionid, BlockID, IsInitialBlock from tblSessionPool where sessionid like '%" + tSession + "%'";
                oCommand.CommandText = strSQL;
                oCommand.Connection = moConn;
                oAdapter = new SqlDataAdapter(oCommand);
                oAdapter.Fill(dtTest1);
                if (dtTest1.Rows.Count == 0)
                {
                    return dtTest1;
                }
                else
                {
                    strRet = dtTest1;
                }

                if (!(moConn.State == ConnectionState.Open))
                {
                    moConn.Open();
                }

                strSQL = "delete from tblSessionPool where SessionID like '%" + tSession + "%'";
                oCommand.CommandText = strSQL;
                cnt = oCommand.ExecuteNonQuery();
                return strRet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtTest1.Dispose();
                dtTest2.Dispose();
                oAdapter.Dispose();
                oCommand.Dispose();
            }
        }

        public bool CheckSessionWithOutSequence(string SessionID) // Check conditions when closing - by Shashin - 23-02-2010
        {
            var oCommand = new SqlCommand();
            SqlDataAdapter oAdapter = null;
            var dtTest1 = new DataTable();
            var dtTest2 = new DataTable();
            var oCon = new SqlConnection();
            try
            {

/* Unmerged change from project 'TripXML.Library (net6.0)'
Before:
                string Server = Core.modCore.config.GetSection("Server").ToString();
                string Database = Core.modCore.config.GetSection("Database").ToString();
                string User = Core.modCore.config.GetSection("User").ToString();
                string Password = Core.modCore.config.GetSection("Password").ToString();
After:
                string Server = modCore.config.GetSection("Server").ToString();
                string Database = modCore.config.GetSection("Database").ToString();
                string User = modCore.config.GetSection("User").ToString();
                string Password = modCore.config.GetSection("Password").ToString();
*/
                string Server = Library.modCore.config.GetSection("Server").ToString();
                string Database = Library.modCore.config.GetSection("Database").ToString();
                string User = Library.modCore.config.GetSection("User").ToString();
                string Password = Library.modCore.config.GetSection("Password").ToString();
                StringBuilder sb = new StringBuilder();
                sb.Append("data source=").Append(Server).Append(";initial Catalog=").Append(Database).Append(";User ID=").Append(User).Append(";Password=").Append(Password);
                string CnnString = sb.ToString();
                oCon.ConnectionString = CnnString;
                string tSession = "";
                tSession = SessionID;
                string strSQL = "select sessionid from tblSessionPool where sessionid = '" + tSession + "'";
                oCommand.CommandText = strSQL;
                oCommand.Connection = oCon;
                oAdapter = new SqlDataAdapter(oCommand);
                oAdapter.Fill(dtTest1);
                if (dtTest1.Rows.Count == 0)
                {
                    return false;
                }

                strSQL = "select sessionID from tblSessionPool where sessionid = '" + tSession + "' and ToBeDeleted='Y'";
                oCommand.CommandText = strSQL;
                oAdapter.Fill(dtTest2);
                int cnt;
                if (dtTest2.Rows.Count > 0)
                {
                    if (!(oCon.State == ConnectionState.Open))
                    {
                        oCon.Open();
                    }

                    strSQL = "delete from tblSessionPool where sessionid = '" + tSession + "'";
                    oCommand.CommandText = strSQL;
                    cnt = oCommand.ExecuteNonQuery();
                    strSQL = "update tblPCCBlocks set sessionsUsed=sessionsUsed-1 where PCC=(SELECT top 1  [PCC]() FROM tblSessionPool where sessionid = '" + tSession + "')";
                    oCommand.CommandText = strSQL;
                    cnt = oCommand.ExecuteNonQuery();
                    oCon.Close();
                    return false;
                }

                if (!(oCon.State == ConnectionState.Open))
                {
                    oCon.Open();
                }

                strSQL = "update tblSessionPool set LastMessageTime=getdate(),IsUse='N',sessionid='" + tSession + "' where sessionid like '" + tSession + "%'";

                // strSQL = "update tblSessionPool set LastMessageTime=getdate(),IsUse='N' where sessionid like '" & tSession & "%'"


                oCommand.CommandText = strSQL;
                cnt = oCommand.ExecuteNonQuery();
                oCon.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtTest1.Dispose();
                dtTest2.Dispose();
                oAdapter.Dispose();
                oCommand.Dispose();
                oCon.Close();
                oCon.Dispose();
            }
        }

        public void AddSoapException(ref string SoapException, ref string SoapEnvelope)
        {
            SqlCommand oCommand = null;
            try
            {
                oCommand = new SqlCommand("uspAddSoapException", moConn);
                {
                    ref var withBlock = ref oCommand;
                    withBlock.CommandType = CommandType.StoredProcedure;
                    withBlock.CommandTimeout = 60;
                    {
                        var withBlock1 = oCommand.Parameters;
                        withBlock1.Add(new SqlParameter("@SoapException", SqlDbType.VarChar, 2000, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, SoapException));
                        withBlock1.Add(new SqlParameter("@SoapEnvelope", SoapEnvelope));
                    }

                    withBlock.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oCommand is object)
                {
                    oCommand.Dispose();
                }
            }
        }

        public void ImportBooking(string Customer, string UserName, string RecLoc, DateTime FlightDate)
        {
            SqlCommand oCommand = null;
            try
            {
                oCommand = new SqlCommand("uspImportBookings", moConn);
                {
                    ref var withBlock = ref oCommand;
                    withBlock.CommandType = CommandType.StoredProcedure;
                    withBlock.CommandTimeout = 60;
                    {
                        var withBlock1 = oCommand.Parameters;
                        withBlock1.Add(new SqlParameter("@Customer", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, Customer));
                        withBlock1.Add(new SqlParameter("@UserName", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, UserName));
                        withBlock1.Add(new SqlParameter("@RecLoc", SqlDbType.Char, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, RecLoc));
                        withBlock1.Add(new SqlParameter("@FlightDate", SqlDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, FlightDate));
                    }

                    withBlock.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oCommand is object)
                {
                    oCommand.Dispose();
                }
            }
        }

        public void UpdateBookingStatus(string RecLoc, char Status)
        {
            SqlCommand oCommand = null;
            try
            {
                oCommand = new SqlCommand("uspUpdateBookingStatus", moConn);
                {
                    ref var withBlock = ref oCommand;
                    withBlock.CommandType = CommandType.StoredProcedure;
                    withBlock.CommandTimeout = 60;
                    {
                        var withBlock1 = oCommand.Parameters;
                        withBlock1.Add(new SqlParameter("@RecLoc", SqlDbType.Char, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, RecLoc));
                        withBlock1.Add(new SqlParameter("@Status", SqlDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, Status));
                    }

                    withBlock.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oCommand is object)
                {
                    oCommand.Dispose();
                }
            }
        }

        public string GetMarkups(int siteItemID, string databaseName, int languageId, string currencyId, string supplierId, string externalId, string destinationId, string brandId, string departureCity, DateTime departureStartDate, DateTime departureEndDate, string countryId, string stateId, int promotionTypeId, int classTypeId, int seniorCount, int adultCount, int childCount, DateTime BookingDate, int PackageID, int FareCodeSiteItemID, int ProductTypeId, string fromCity, string toCity, string fromCountry, string toCountry, string FareType, int Affiliate, int JourneyType, string OfficeID, string ApplicationTypeId, string RoutingCity, string BookingClass, string FlightNumber)
        {
            SqlCommand oCommand = null;
            var dr = default(SqlDataReader);
            var sb = new StringBuilder();
            try
            {
                oCommand = new SqlCommand("SitePromotionsSearchNew", moConn);
                {
                    ref var withBlock = ref oCommand;
                    withBlock.CommandType = CommandType.StoredProcedure;
                    withBlock.CommandTimeout = 60;
                    {
                        var withBlock1 = oCommand.Parameters;
                        withBlock1.Add(new SqlParameter("@siteItemID", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, siteItemID));
                        withBlock1.Add(new SqlParameter("@databaseName", SqlDbType.Char, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, databaseName));
                        withBlock1.Add(new SqlParameter("@languageId", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, languageId));
                        withBlock1.Add(new SqlParameter("@currencyId", SqlDbType.Char, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, currencyId));
                        withBlock1.Add(new SqlParameter("@supplierId", SqlDbType.Char, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, supplierId));
                        withBlock1.Add(new SqlParameter("@externalId", SqlDbType.Char, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, externalId));
                        withBlock1.Add(new SqlParameter("@destinationId", SqlDbType.Char, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, destinationId));
                        withBlock1.Add(new SqlParameter("@brandId", SqlDbType.Char, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, brandId));
                        withBlock1.Add(new SqlParameter("@departureCity", SqlDbType.Char, 200, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, departureCity));
                        withBlock1.Add(new SqlParameter("@departureStartDate", SqlDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, departureStartDate));
                        withBlock1.Add(new SqlParameter("@departureEndDate", SqlDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, departureEndDate));
                        withBlock1.Add(new SqlParameter("@countryId", SqlDbType.Char, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, countryId));
                        withBlock1.Add(new SqlParameter("@stateId", SqlDbType.Char, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, stateId));
                        withBlock1.Add(new SqlParameter("@promotionTypeId", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, promotionTypeId));
                        withBlock1.Add(new SqlParameter("@classTypeId", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, classTypeId));
                        withBlock1.Add(new SqlParameter("@seniorCount", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, seniorCount));
                        withBlock1.Add(new SqlParameter("@adultCount", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, adultCount));
                        withBlock1.Add(new SqlParameter("@childCount", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, childCount));
                        withBlock1.Add(new SqlParameter("@BookingDate", SqlDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, BookingDate));
                        withBlock1.Add(new SqlParameter("@PackageID", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, PackageID));
                        withBlock1.Add(new SqlParameter("@FareCodeSiteItemID", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, FareCodeSiteItemID));
                        withBlock1.Add(new SqlParameter("@ProductTypeId", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, ProductTypeId));
                        withBlock1.Add(new SqlParameter("@fromCity", SqlDbType.Char, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, fromCity));
                        withBlock1.Add(new SqlParameter("@toCity", SqlDbType.Char, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, toCity));
                        withBlock1.Add(new SqlParameter("@fromCountry", SqlDbType.Char, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, fromCountry));
                        withBlock1.Add(new SqlParameter("@toCountry", SqlDbType.Char, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, toCountry));
                        withBlock1.Add(new SqlParameter("@FareType", SqlDbType.Char, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, FareType));
                        withBlock1.Add(new SqlParameter("@Affiliate", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, Affiliate));
                        withBlock1.Add(new SqlParameter("@JourneyType", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, JourneyType));
                        withBlock1.Add(new SqlParameter("@OfficeID", SqlDbType.Char, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, OfficeID));
                        withBlock1.Add(new SqlParameter("@ApplicationTypeId", SqlDbType.Char, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, ApplicationTypeId));
                        withBlock1.Add(new SqlParameter("@RoutingCity", SqlDbType.Char, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, RoutingCity));
                        withBlock1.Add(new SqlParameter("@BookingClass", SqlDbType.Char, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, BookingClass));
                        withBlock1.Add(new SqlParameter("@FlightNumber", SqlDbType.Char, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, FlightNumber));
                    }

                    // .ExecuteXmlReader()
                }

                // markupsXML = oCommand.ExecuteXmlReader
                dr = oCommand.ExecuteReader();
                int i = 0;
                sb.Append("<SearchPromotionsResponse>");
                do
                {
                    while (dr.Read())
                    {
                        i = i + 1;
                        sb.Append("<Promotion>");
                        for (int j = 0, loopTo = dr.FieldCount - 1; j <= loopTo; j++)
                        {
                            sb.Append("<");
                            sb.Append(dr.GetName(j));
                            sb.Append(">");
                            sb.Append(dr[j]);
                            sb.Append("</");
                            sb.Append(dr.GetName(j));
                            sb.Append(">");
                        }

                        sb.Append("</Promotion>");
                    }
                }
                while (dr.NextResult());
                sb.Append("</SearchPromotionsResponse>");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dr.Close();
                if (oCommand is object)
                {
                    oCommand.Dispose();
                }
            }
        }
        // Check AvailableSessions in the pool
        public bool CheckAvailableSessions(string PCC, string System, string UserID)
        {
            var oCommand = new SqlCommand();
            string strSQL = "";
            bool AvailableSessions;
            // Dim reader As SqlDataReader = Nothing
            SqlDataReader reader = null;
            try
            {
                strSQL = "Select * from dbo.tblSessionPool where PCC like '" + PCC + "' AND system like '" + System + "' AND UserID like '" + UserID + "' AND ToBeDeleted='N' AND IsUse='N';";

                // oCommand = New SqlCommand("uspCheckAvailableSessions", moConn)
                // oCommand = New SqlCommand(strSQL, moConn)
                // strSQL = "select sessionid from tblSessionPool where sessionid like '" & tSession & "%'"

                oCommand.CommandText = strSQL;
                oCommand.Connection = moConn;
                reader = oCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    AvailableSessions = true;
                }
                else
                {
                    AvailableSessions = false;
                }

                return AvailableSessions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oCommand is object)
                {
                    reader.Close();
                    oCommand.Dispose();
                }
            }
        }

        // Update an available session
        public string SessionUpdate1()
        {
            SqlCommand oCommand = null;
            var LastMessageTime = DateTime.Now;
            // Dim dr As SqlDataReader
            string SessionID = "";
            try
            {
                oCommand = new SqlCommand("uspSessionPoolUpdate", moConn);
                {
                    ref var withBlock = ref oCommand;
                    withBlock.CommandType = CommandType.StoredProcedure;
                    withBlock.CommandTimeout = 60;
                    {
                        var withBlock1 = oCommand.Parameters;
                        withBlock1.Add(new SqlParameter("@LastMessageTime", SqlDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, LastMessageTime));
                        var SessionIDParam = new SqlParameter("@SessionID", SqlDbType.NVarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, SessionID);
                        SessionIDParam.Direction = ParameterDirection.Output;
                        withBlock1.Add(SessionIDParam);
                    }

                    withBlock.ExecuteNonQuery();
                }
                return SessionID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oCommand is object)
                {
                    oCommand.Dispose();
                    oCommand = null;
                }
            }
        }

        public string SessionUpdate(string PCC, string System, string UserId, bool isSOAP2)
        {
            var oCommand = new SqlCommand();
            string uSessionID = "";
            try
            {
                oCommand = new SqlCommand("SessionUpdate", moConn);

                // strSQL = "SELECT TOP 1 SessionID,SequenceNo FROM tblSessionPool WITH (XLOCK, PAGLOCK) where PCC like '" & PCC & "' AND system like '" & System & "' AND UserID like '" & UserId & "' AND ToBeDeleted='N' AND IsUse='N' ORDER by LastMessageTime;"

                var SessionIDParam = new SqlParameter("@Session", SqlDbType.NVarChar, 450, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, uSessionID);
                {
                    ref var withBlock = ref oCommand;
                    withBlock.CommandType = CommandType.StoredProcedure;
                    withBlock.CommandTimeout = 60;
                    {
                        var withBlock1 = oCommand.Parameters;
                        withBlock1.Add(new SqlParameter("@PCC", SqlDbType.NVarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, PCC));
                        withBlock1.Add(new SqlParameter("@System", SqlDbType.NVarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, System));
                        withBlock1.Add(new SqlParameter("@UserID", SqlDbType.NVarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, UserId));
                        SessionIDParam.Direction = ParameterDirection.Output;
                        withBlock1.Add(SessionIDParam);
                    }

                    withBlock.ExecuteNonQuery();
                    uSessionID = SessionIDParam.Value.ToString();
                    int iseq = 0;
                    string seq = "";
                    if (isSOAP2)
                    {
                        seq = uSessionID.Substring(uSessionID.IndexOf("|") + 1);
                        seq = seq.Substring(seq.IndexOf("|") + 1);
                    }
                    else
                    {
                        seq = uSessionID.Substring(uSessionID.IndexOf("|") + 1);
                    }

                    iseq = Convert.ToInt16(seq) - 1;
                    uSessionID = uSessionID.Substring(0, uSessionID.Length - seq.Length) + iseq.ToString();
                    return uSessionID;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oCommand is object)
                {
                    oCommand.Dispose();
                }
            }

        }

        public string SessionUpdate(string PCC, string System, string UserId, string GDS)
        {
            var oCommand = new SqlCommand();
            string uSessionID = "";
            try
            {
                oCommand = new SqlCommand("SessionUpdate", moConn);

                // strSQL = "SELECT TOP 1 SessionID,SequenceNo FROM tblSessionPool WITH (XLOCK, PAGLOCK) where PCC like '" & PCC & "' AND system like '" & System & "' AND UserID like '" & UserId & "' AND ToBeDeleted='N' AND IsUse='N' ORDER by LastMessageTime;"

                var SessionIDParam = new SqlParameter("@Session", SqlDbType.NVarChar, 450, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, uSessionID);
                {
                    ref var withBlock = ref oCommand;
                    withBlock.CommandType = CommandType.StoredProcedure;
                    withBlock.CommandTimeout = 60;
                    {
                        var withBlock1 = oCommand.Parameters;
                        withBlock1.Add(new SqlParameter("@PCC", SqlDbType.NVarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, PCC));
                        withBlock1.Add(new SqlParameter("@System", SqlDbType.NVarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, System));
                        withBlock1.Add(new SqlParameter("@UserID", SqlDbType.NVarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, UserId));
                        SessionIDParam.Direction = ParameterDirection.Output;
                        withBlock1.Add(SessionIDParam);
                    }

                    withBlock.ExecuteNonQuery();
                    uSessionID = SessionIDParam.Value.ToString();
                    return uSessionID;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oCommand is object)
                {
                    oCommand.Dispose();
                }
            }

        }


/* Unmerged change from project 'TripXML.Library (net6.0)'
Before:
        public Core.modCore.TripXMLProviderSystems SetPCCBlock(Core.modCore.TripXMLProviderSystems Provider)
After:
        public modCore.TripXMLProviderSystems SetPCCBlock(modCore.TripXMLProviderSystems Provider)
*/
        public Library.modCore.TripXMLProviderSystems SetPCCBlock(Library.modCore.TripXMLProviderSystems Provider)
        {
            var oCommand = new SqlCommand();
            SqlDataReader reader = null;
            string PCC = Provider.PCC;
            try
            {
                string strSQL = "SELECT * FROM tblPCCBlocks where PCC like '" + PCC + "%' and UserID='" + Provider.UserID + "'";
                oCommand.CommandText = strSQL;
                oCommand.Connection = moConn;
                reader = oCommand.ExecuteReader();
                while (reader.Read())
                {
                    Provider.ProviderSession.InitialBlockSize = Convert.ToInt32(reader["InitialBlock"]);
                    Provider.ProviderSession.MaximumCount = Convert.ToInt32(reader["MaxSessions"]);
                    Provider.ProviderSession.NextBlockSize = Convert.ToInt32(reader["NextBlock"]);
                    Provider.ProviderSession.SessionsUsed = Convert.ToInt32(reader["SessionsUsed"]);
                }

                reader.Close();
                return Provider;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oCommand is object)
                {
                    reader.Close();
                    oCommand.Dispose();
                }
            }
        }


/* Unmerged change from project 'TripXML.Library (net6.0)'
Before:
        public Core.modCore.TripXMLProviderSystems SetPCCBlock(Core.modCore.TripXMLProviderSystems Provider, string version)
After:
        public modCore.TripXMLProviderSystems SetPCCBlock(modCore.TripXMLProviderSystems Provider, string version)
*/
        public Library.modCore.TripXMLProviderSystems SetPCCBlock(Library.modCore.TripXMLProviderSystems Provider, string version)
        {
            var oCommand = new SqlCommand();
            string strSQL;
            string pcc;
            SqlDataReader reader = null;
            pcc = Provider.PCC;
            try
            {
                strSQL = "SELECT * FROM tblPCCBlocks where PCC like '" + pcc + "%' and UserID='" + Provider.UserID + "' and system='" + Provider.System + "'";
                oCommand.CommandText = strSQL;
                oCommand.Connection = moConn;
                reader = oCommand.ExecuteReader();
                while (reader.Read())
                {
                    Provider.ProviderSession.InitialBlockSize = Convert.ToInt32(reader["InitialBlock"]);
                    Provider.ProviderSession.MaximumCount = Convert.ToInt32(reader["MaxSessions"]);
                    Provider.ProviderSession.NextBlockSize = Convert.ToInt32(reader["NextBlock"]);
                    Provider.ProviderSession.SessionsUsed = Convert.ToInt32(reader["SessionsUsed"]);
                }

                reader.Close();
                return Provider;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oCommand is object)
                {
                    reader.Close();
                    oCommand.Dispose();
                }
            }
        }

        public bool CheckInitialPool(string PCC, string UserID)
        {
            // Public Function CheckInitialPool(ByVal PCC As String, ByVal UserID As String, ByVal system As String) As Boolean
            var oCommand = new SqlCommand();
            string strSQL = "";
            SqlDataReader reader = null;
            int SessionsUsed = 1;
            int Creating = 1;
            var tran = moConn.BeginTransaction();
            try
            {
                strSQL = "SELECT SessionsUsed,CreatingInit FROM tblPCCBlocks where PCC like '" + PCC + "%' AND UserID='" + UserID + "'";
                oCommand.CommandText = strSQL;
                oCommand.Connection = moConn;
                oCommand.Transaction = tran;
                reader = oCommand.ExecuteReader();
                while (reader.Read())
                {
                    SessionsUsed = Convert.ToInt32(reader["SessionsUsed"].ToString());
                    Creating = Convert.ToInt32(reader["CreatingInit"].ToString());
                }

                reader.Close();
                strSQL = "UPDATE tblPCCBlocks SET CreatingInit=1 where PCC like '" + PCC + "%' AND UserID='" + UserID + "' AND InitialBlock !=0";
                oCommand.CommandText = strSQL;
                oCommand.Connection = moConn;
                oCommand.ExecuteNonQuery();
                tran.Commit();
                if (SessionsUsed == 0 & Creating == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally
            {
                if (oCommand is object)
                {
                    oCommand.Dispose();
                }
            }
        }

        public bool CheckInitialPool(string PCC, string UserID, string system)
        {
            var oCommand = new SqlCommand();
            string strSQL = "";
            SqlDataReader reader = null;
            int SessionsUsed = 1;
            int Creating = 1;
            var tran = moConn.BeginTransaction();
            try
            {

                // strSQL = "SELECT SessionsUsed,CreatingInit FROM tblPCCBlocks where PCC like '" & PCC & "%' AND UserID='" & UserID & "'"

                strSQL = "SELECT SessionsUsed,CreatingInit FROM tblPCCBlocks where PCC like '" + PCC + "%' AND UserID='" + UserID + "' AND system='" + system + "'";
                oCommand.CommandText = strSQL;
                oCommand.Connection = moConn;
                oCommand.Transaction = tran;
                reader = oCommand.ExecuteReader();
                while (reader.Read())
                {
                    SessionsUsed = Convert.ToInt32(reader["SessionsUsed"].ToString());
                    Creating = Convert.ToInt32(reader["CreatingInit"].ToString());
                }

                reader.Close();

                // strSQL = "UPDATE tblPCCBlocks SET CreatingInit=1 where PCC like '" & PCC & "%' AND UserID='" & UserID & "' AND InitialBlock !=0"

                strSQL = "UPDATE tblPCCBlocks SET CreatingInit=1 where PCC like '" + PCC + "%' AND UserID='" + UserID + "' AND system='" + system + "'AND InitialBlock !=0";
                oCommand.CommandText = strSQL;
                oCommand.Connection = moConn;
                oCommand.ExecuteNonQuery();
                tran.Commit();

                return SessionsUsed == 0 & Creating == 0;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally
            {
                if (oCommand is object)
                {
                    oCommand.Dispose();
                }
            }
        }

        // Public Sub UpdatePCCSessions(ByVal PCC As String, ByVal NewSessions As Integer)
        public void UpdatePCCSessions(string PCC, int NewSessions)
        {
            var oCommand = new SqlCommand();
            var SessionsUsed = default(int);
            try
            {
                // strSQL = "UPDATE tblPCCBlocks SET SessionsUsed='" & SessionCount & "' WHERE  PCC like '" & PCC & "%'"

                // strSQL = "SELECT SessionsUsed FROM tblPCCBlocks where PCC like '" & PCC & "%'"
                string strSQL = "SELECT SessionsUsed FROM tblPCCBlocks where PCC like '" + PCC + "%'";
                oCommand.CommandText = strSQL;
                oCommand.Connection = moConn;
                SqlDataReader reader = oCommand.ExecuteReader();
                while (reader.Read())
                    SessionsUsed = Convert.ToInt32(reader["SessionsUsed"].ToString());
                reader.Close();
                NewSessions = NewSessions + SessionsUsed;
                strSQL = "UPDATE tblPCCBlocks SET SessionsUsed='" + NewSessions + "' WHERE  PCC like '" + PCC + "%'";
                oCommand.CommandText = strSQL;
                oCommand.Connection = moConn;
                oCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oCommand is object)
                {
                    oCommand.Dispose();
                }
            }
        }

        public void UpdatePCCSessions(string PCC, int NewSessions, string userID)
        {
            var oCommand = new SqlCommand();

            try
            {
                // strSQL = "UPDATE tblPCCBlocks SET SessionsUsed='" & SessionCount & "' WHERE  PCC like '" & PCC & "%'"
                string strSQL = "SELECT SessionsUsed FROM tblPCCBlocks where PCC like '" + PCC + "%' and UserID='" + userID + "'";
                oCommand.CommandText = strSQL;
                oCommand.Connection = moConn;

                strSQL = "UPDATE tblPCCBlocks SET SessionsUsed=SessionsUsed+" + NewSessions + " WHERE  PCC like '" + PCC + "%' and UserID='" + userID + "'";
                oCommand.CommandText = strSQL;
                oCommand.Connection = moConn;
                oCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oCommand is object)
                {
                    oCommand.Dispose();
                }
            }
        }

        public void UpdatePCCSessions(string PCC, int NewSessions, string userID, string system)
        {
            var oCommand = new SqlCommand();

            try
            {
                // strSQL = "UPDATE tblPCCBlocks SET SessionsUsed='" & SessionCount & "' WHERE  PCC like '" & PCC & "%'"

                // strSQL = "SELECT SessionsUsed FROM tblPCCBlocks where PCC like '" & PCC & "%'"
                string strSQL = "SELECT SessionsUsed FROM tblPCCBlocks where PCC like '" + PCC + "%'AND UserID='" + userID + "' AND system='" + system + "'";
                oCommand.CommandText = strSQL;
                oCommand.Connection = moConn;

                strSQL = "UPDATE tblPCCBlocks SET SessionsUsed=SessionsUsed+" + NewSessions + " WHERE  PCC like '" + PCC + "%' and UserID='" + userID + "'AND system='" + system + "'";
                oCommand.CommandText = strSQL;
                oCommand.Connection = moConn;
                oCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oCommand is object)
                {
                    oCommand.Dispose();
                }
            }
        }


        // Insert a Session to the Pool
        public void InsertNewSession(string SessionID, int SequenceNo, string GDS, DateTime CreatedTime, DateTime LastMessageTime, string UserName, string UserID, string Status, char IsUse, char TobeDeleted, string URL, string BlockId, char IsInitialBlock, string PCC, string Profile, string System, string Password)
        {
            SqlCommand oCommand = null;
            try
            {
                oCommand = new SqlCommand("uspInsertNewSession", moConn);
                {
                    ref var withBlock = ref oCommand;
                    withBlock.CommandType = CommandType.StoredProcedure;
                    withBlock.CommandTimeout = 60;
                    {
                        var withBlock1 = oCommand.Parameters;
                        withBlock1.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar, 450, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, SessionID));
                        withBlock1.Add(new SqlParameter("@SequenceNo", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, SequenceNo));
                        withBlock1.Add(new SqlParameter("@GDS", SqlDbType.NVarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, GDS));
                        withBlock1.Add(new SqlParameter("@CreatedTime", SqlDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, CreatedTime));
                        withBlock1.Add(new SqlParameter("@LastMessageTime", SqlDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, LastMessageTime));
                        withBlock1.Add(new SqlParameter("@UserName", SqlDbType.NVarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, UserName));
                        withBlock1.Add(new SqlParameter("@UserID", SqlDbType.NVarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, UserID));
                        withBlock1.Add(new SqlParameter("@Status", SqlDbType.NVarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, Status));
                        withBlock1.Add(new SqlParameter("@ToBeDeleted", SqlDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, TobeDeleted));
                        withBlock1.Add(new SqlParameter("@IsUse", SqlDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, IsUse));
                        withBlock1.Add(new SqlParameter("@URL", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, URL));
                        withBlock1.Add(new SqlParameter("@BlockId", SqlDbType.NVarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, BlockId));
                        withBlock1.Add(new SqlParameter("@IsInitialBlock", SqlDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, IsInitialBlock));
                        withBlock1.Add(new SqlParameter("@PCC", SqlDbType.NVarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, PCC));
                        withBlock1.Add(new SqlParameter("@Profile", SqlDbType.NVarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, Profile));
                        withBlock1.Add(new SqlParameter("@System", SqlDbType.NVarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, System));
                        withBlock1.Add(new SqlParameter("@Password", SqlDbType.NVarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, Password));
                    }

                    int numberOfRows = withBlock.ExecuteNonQuery();
                    int a = numberOfRows;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oCommand is object)
                {
                    oCommand.Dispose();
                }
            }
        }

        public void InsertNewSession(string SessionID, int SequenceNo, string GDS, DateTime CreatedTime, DateTime LastMessageTime, string UserName, string Status, char IsUse, char TobeDeleted, string URL, string BlockId, char IsInitialBlock, string PCC, string Profile, string System, string Password)
        {
            SqlCommand oCommand = null;
            try
            {
                oCommand = new SqlCommand("uspInsertNewSession", moConn);
                {
                    ref var withBlock = ref oCommand;
                    withBlock.CommandType = CommandType.StoredProcedure;
                    withBlock.CommandTimeout = 60;
                    {
                        var withBlock1 = oCommand.Parameters;
                        withBlock1.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar, 4000, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, SessionID));
                        withBlock1.Add(new SqlParameter("@SequenceNo", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, SequenceNo));
                        withBlock1.Add(new SqlParameter("@GDS", SqlDbType.NVarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, GDS));
                        withBlock1.Add(new SqlParameter("@CreatedTime", SqlDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, CreatedTime));
                        withBlock1.Add(new SqlParameter("@LastMessageTime", SqlDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, LastMessageTime));
                        withBlock1.Add(new SqlParameter("@UserName", SqlDbType.NVarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, UserName));
                        withBlock1.Add(new SqlParameter("@Status", SqlDbType.NVarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, Status));
                        withBlock1.Add(new SqlParameter("@ToBeDeleted", SqlDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, TobeDeleted));
                        withBlock1.Add(new SqlParameter("@IsUse", SqlDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, IsUse));
                        withBlock1.Add(new SqlParameter("@URL", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, URL));
                        withBlock1.Add(new SqlParameter("@BlockId", SqlDbType.NVarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, BlockId));
                        withBlock1.Add(new SqlParameter("@IsInitialBlock", SqlDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, IsInitialBlock));
                        withBlock1.Add(new SqlParameter("@PCC", SqlDbType.NVarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, PCC));
                        withBlock1.Add(new SqlParameter("@Profile", SqlDbType.NVarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, Profile));
                        withBlock1.Add(new SqlParameter("@System", SqlDbType.NVarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, System));
                        withBlock1.Add(new SqlParameter("@Password", SqlDbType.NVarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, Password));
                    }

                    withBlock.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oCommand is object)
                {
                    oCommand.Dispose();
                }
            }
        }
    }

    public class UpdateTACount
    {
        private delegate void TAIncrementDelegate();

        private TAIncrementDelegate TAIncrementWrapper;
        private int _TAIncrement = 0;
        private string _TASupplierID = "";
        private string _TAOwnerID = "";

        public UpdateTACount()
        {
            TAIncrementWrapper = new TAIncrementDelegate(TACount_Method);
        }

        public void BeginTACount()
        {
            var cbr = new AsyncCallback(EndTACount);
            var arr = TAIncrementWrapper.BeginInvoke(cbr, null);
            // TACount_Method()
        }

        private void EndTACount(IAsyncResult aBr)
        {
            TAIncrementWrapper.EndInvoke(aBr);
            aBr.AsyncWaitHandle.Close();
        }

        private void TACount_Method()
        {

/* Unmerged change from project 'TripXML.Library (net6.0)'
Before:
            string CnnString = Core.modCore.config.GetSection("ConnectionString").ToString().Trim();
After:
            string CnnString = modCore.config.GetSection("ConnectionString").ToString().Trim();
*/
            string CnnString = Library.modCore.config.GetSection("ConnectionString").ToString().Trim();
            if (CnnString.Length == 0)
            {

/* Unmerged change from project 'TripXML.Library (net6.0)'
Before:
                string Server = Core.modCore.config.GetSection("Server").ToString();
                string Database = Core.modCore.config.GetSection("Database").ToString();
                string User = Core.modCore.config.GetSection("User").ToString();
                string Password = Core.modCore.config.GetSection("Password").ToString();
After:
                string Server = modCore.config.GetSection("Server").ToString();
                string Database = modCore.config.GetSection("Database").ToString();
                string User = modCore.config.GetSection("User").ToString();
                string Password = modCore.config.GetSection("Password").ToString();
*/
                string Server = Library.modCore.config.GetSection("Server").ToString();
                string Database = Library.modCore.config.GetSection("Database").ToString();
                string User = Library.modCore.config.GetSection("User").ToString();
                string Password = Library.modCore.config.GetSection("Password").ToString();
                StringBuilder sb = new StringBuilder();
                sb.Append("data source=").Append(Server).Append(";initial Catalog=").Append(Database).Append(";User ID=").Append(User).Append(";Password=").Append(Password);
                CnnString = sb.ToString();
                // CnnString = "data source=8.8.248.115\DEDICATED8-VM6,1433;initial Catalog=Traveltalk;User ID=ttlog;Password=traveltalk"
            }

            var cmd = new SqlCommand();
            var conn = new SqlConnection(CnnString);
            try
            {
                conn.Open();
                var cmdBuild = new StringBuilder("<MessageContent>");
                cmdBuild.Append("<TA_Add>" + TAIncrement + "</TA_Add>");
                cmdBuild.Append("<TAUser>" + TAOwnerID + "</TAUser>");
                cmdBuild.Append("<TAOwner>" + TASupplierID + "</TAOwner>");
                cmdBuild.Append("</MessageContent>");
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_BeginTACount";
                cmd.Parameters.Add("@Message", SqlDbType.VarBinary);
                cmd.Parameters["@Message"].Value = Encoding.Unicode.GetBytes(cmdBuild.ToString());
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                if (!(conn.State == ConnectionState.Closed))
                    conn.Close();
            }
            catch (Exception ex)
            {
                if (!(conn.State == ConnectionState.Closed))
                    conn.Close();
                // Log or handle Error here
            }
        }

        public string TASupplierID
        {
            get
            {
                return _TASupplierID;
            }

            set
            {
                _TASupplierID = value;
            }
        }

        public string TAOwnerID
        {
            get
            {
                return _TAOwnerID;
            }

            set
            {
                _TAOwnerID = value;
            }
        }

        public int TAIncrement
        {
            get
            {
                return _TAIncrement;
            }

            set
            {
                _TAIncrement = value;
            }
        }
    }
}
