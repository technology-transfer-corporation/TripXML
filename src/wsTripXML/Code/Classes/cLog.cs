using System;
using static System.IO.File;
using System.Text;
using System.Threading;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{


    public class cLog
    {
        private int mintLogType;
        private string mstrServerName = "";
        private string mstrCustomer = "";
        private string mstrUUID = "";
        private string mstrUserName = "";
        private string mstrProvider = "";
        private int mintMessageID;
        private string mstrMessage = "";
        private DateTime mdtMessageDate;
        private int mintResponseTime;
        private StringBuilder sb = new StringBuilder();

        public string LogRequest(string ServerName, string Customer, string UserName, string Provider, int MessageID, ref string Message, DateTime StartTime)
        {


            try
            {
                mstrUUID = modMain.CreateUUID();

                mintLogType = (int)modMain.enLogType.Request;
                mstrServerName = ServerName;
                mstrCustomer = Customer;
                mstrUserName = UserName;
                mstrProvider = Provider;
                mintMessageID = MessageID;
                mstrMessage = Message;
                mdtMessageDate = StartTime;
                mintResponseTime = 0;

                var oLofThread = new Thread(new ThreadStart(LogMessage));

                oLofThread.Start();
            }

            catch (Exception ex)
            {

            }

            return mstrUUID;

        }

        public void LogResponse(string UUID, ref string ServerName, string Customer, string UserName, string Provider, int MessageID, ref string Message, DateTime StartTime)
        {

            mdtMessageDate = DateTime.Now;
            mintLogType = (int)modMain.enLogType.Response;
            mstrUUID = UUID;
            mstrServerName = ServerName;
            mstrCustomer = Customer;
            mstrUserName = UserName;
            mstrProvider = Provider;
            mintMessageID = MessageID;
            mstrMessage = Message;
            mintResponseTime = (int)Math.Round(mdtMessageDate.Subtract(StartTime).TotalMilliseconds);

            var oLofThread = new Thread(new ThreadStart(LogMessage));

            oLofThread.Start();

        }

        private void LogMessage()
        {
            cDA oDA = null;
            string[] arAttributes;

            if (mstrUUID is null)
                mstrUUID = "";
            if (mstrCustomer is null)
                mstrCustomer = "";
            if (mstrServerName is null)
                mstrServerName = "";
            if (mstrUserName is null)
                mstrUserName = "";
            if (mstrProvider is null)
                mstrProvider = "";

            switch (mintMessageID)
            {
                case (int)ttServices.TravelBuild:
                case (int)ttServices.CruiseCreateBooking:
                    {
                        arAttributes = new string[] { "CardNumber", "BankAcctNumber", "DocID" };
                        CoreLib.MaskPrivateData(ref mstrMessage, arAttributes);
                        break;
                    }
            }

            do
            {
                try
                {
                    if (mstrUUID.Length == 0)
                    {
                        modMain.LogMessageToFile(mintLogType, ref mstrUUID, ref mstrServerName, ref mstrCustomer, ref mstrUserName, ref mstrProvider, mintMessageID, ref mstrMessage, mdtMessageDate, mintResponseTime, "UUID is Missing");
                        break;
                    }
                    else if (mstrCustomer.Length == 0)
                    {
                        modMain.LogMessageToFile(mintLogType, ref mstrUUID, ref mstrServerName, ref mstrCustomer, ref mstrUserName, ref mstrProvider, mintMessageID, ref mstrMessage, mdtMessageDate, mintResponseTime, "Customer is Missing");
                        break;
                    }

                    oDA = new cDA();

                    oDA.AddMessageLog(mintLogType, ref mstrUUID, ref mstrServerName, ref mstrCustomer, ref mstrUserName, ref mstrProvider, mintMessageID, ref mstrMessage, mdtMessageDate, mintResponseTime);
                }

                catch (Exception ex)
                {
                    modMain.LogMessageToFile(mintLogType, ref mstrUUID, ref mstrServerName, ref mstrCustomer, ref mstrUserName, ref mstrProvider, mintMessageID, ref mstrMessage, mdtMessageDate, mintResponseTime, ex.Message);
                }
                finally
                {
                    if (oDA is not null)
                    {
                        oDA.Dispose();
                        oDA = null;
                    }
                }
            }
            while (false);

        }

        private string GetNodeInnerText(ref string xmlData, string sNode)
        {
            int intStart;
            int intLength;

            if (xmlData.IndexOf(sb.Append("<").Append(sNode).Append(">").ToString()) == -1)
            {
                sb.Remove(0, sb.Length);
                return "";
            }
            sb.Remove(0, sb.Length);
            intStart = xmlData.IndexOf(sb.Append("<").Append(sNode).Append(">").ToString()) + sNode.Length + 2;
            sb.Remove(0, sb.Length);

            intLength = xmlData.IndexOf(sb.Append("</").Append(sNode).Append(">").ToString()) - intStart;
            sb.Remove(0, sb.Length);

            return xmlData.Substring(intStart, intLength).Replace(Constants.vbCr, "").Replace(Constants.vbLf, "").Trim();
            sb = null;
        }

        public void ImportLog()
        {
            var fileNumber = default(int);
            string strLine;
            cDA oDA = null;
            var startCounter = default(DateTime);
            string strLogType = "";

            int logType;
            string webServer;
            string customer;
            string UUID;
            string userName;
            string provider;
            int messageID;
            string message;
            DateTime messageDate;
            int responseTime;
            string recLoc;
            string flightDate;
            int intStart;
            int intLength;
            var sb2 = new StringBuilder();
            try
            {
                if (Exists(sb.Append(LogPath).Append(string.Format("{0}_Log.txt", DateTime.Now.ToShortDateString())).ToString()))
                {
                    sb.Remove(0, sb.Length);
                    Move(sb.Append(LogPath).Append(string.Format("{0}_Log.txt", DateTime.Now.ToShortDateString())).ToString(), sb2.Append(LogPath).Append("LogImport.txt").ToString());
                    sb.Remove(0, sb.Length);
                    sb2.Remove(0, sb2.Length);
                }
                else
                {
                    sb.Remove(0, sb.Length);
                    throw new Exception(sb.Append("Log File ").Append(LogPath).Append("Log.txt Not found.").ToString());
                }

                fileNumber = FileSystem.FreeFile();

                FileSystem.FileOpen(fileNumber, sb.Append(LogPath).Append("LogImport.txt").ToString(), OpenMode.Input, OpenAccess.Read, OpenShare.LockWrite);
                sb.Remove(0, sb.Length);

                oDA = new cDA();

                while (!FileSystem.EOF(fileNumber))
                {
                    strLine = FileSystem.LineInput(fileNumber);

                    logType = Conversions.ToInteger(GetNodeInnerText(ref strLine, "LogType"));
                    UUID = GetNodeInnerText(ref strLine, "UUID");
                    webServer = GetNodeInnerText(ref strLine, "WebServer");
                    customer = GetNodeInnerText(ref strLine, "Customer");
                    userName = GetNodeInnerText(ref strLine, "UserName");
                    provider = GetNodeInnerText(ref strLine, "Provider");
                    messageID = Conversions.ToInteger(GetNodeInnerText(ref strLine, "MessageID"));
                    message = GetNodeInnerText(ref strLine, "Message");
                    messageDate = Conversions.ToDate(GetNodeInnerText(ref strLine, "MessageDate"));
                    responseTime = Conversions.ToInteger(GetNodeInnerText(ref strLine, "ResponseTime"));

                    switch (logType)
                    {
                        case (int)modMain.enLogType.Request:
                            {
                                strLogType = "Log Request = ";
                                break;
                            }
                        case (int)modMain.enLogType.Response:
                            {
                                strLogType = "Log Response = ";
                                break;
                            }
                    }

                    try
                    {
                        startCounter = DateTime.Now;

                        oDA.AddMessageLog(logType, ref UUID, ref webServer, ref customer, ref userName, ref provider, messageID, ref message, messageDate, responseTime);

                        CoreLib.SendTrace("", "cLog", sb.Append("Logging Entry from Log File ").Append(strLogType).Append(((int)Math.Round(DateTime.Now.Subtract(startCounter).TotalMilliseconds)).ToString()).ToString(), "", string.Empty);
                        sb.Remove(0, sb.Length);

                        if (logType == (int)modMain.enLogType.Response & provider.IndexOf("Production") > 0)
                        {
                            switch (messageID)
                            {
                                case (int)ttServices.TravelBuild:
                                    {
                                        // Import New Bookings
                                        if (message.IndexOf("<Success />") > 0)
                                        {
                                            intStart = message.IndexOf("ID=") + 4;
                                            intLength = message.IndexOf(">", intStart) - (intStart + 1);
                                            recLoc = message.Substring(intStart, intLength);

                                            intStart = message.IndexOf("DepartureDateTime=") + 19;
                                            flightDate = message.Substring(intStart, 10);

                                            oDA.ImportBooking(customer, userName, recLoc, Conversions.ToDate(flightDate));

                                        }

                                        break;
                                    }
                                case (int)ttServices.PNRCancel:
                                    {
                                        // Cancel Bookings
                                        if (message.IndexOf("<Success />") > 0)
                                        {
                                            intStart = message.IndexOf("<UniqueID ID=") + 14;
                                            intLength = message.IndexOf(">", intStart) - (intStart + 1);
                                            recLoc = message.Substring(intStart, intLength);

                                            oDA.UpdateBookingStatus(recLoc, 'C');

                                        }

                                        break;
                                    }
                            }
                        }
                    }

                    catch (Exception exx)
                    {
                        CoreLib.SendTrace("", "cLog", sb.Append("Error Logging Entry from Log File ").Append(strLogType).Append(((int)Math.Round(DateTime.Now.Subtract(startCounter).TotalMilliseconds)).ToString()).ToString(), exx.Message, string.Empty);
                        sb.Remove(0, sb.Length);
                    }

                }

                CoreLib.SendTrace("", "cLog", sb.Append("Log File Imported. ").Append(DateTime.Now.ToString()).ToString(), "", string.Empty);
                sb.Remove(0, sb.Length);
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace("", "cLog", "Error Importing Log File.", ex.Message, string.Empty);
            }
            finally
            {
                if (oDA is not null)
                {
                    oDA.Dispose();
                }
                FileSystem.FileClose(fileNumber);
                if (Exists(sb.Append(LogPath).Append("LogImport.txt").ToString()))
                {
                    sb.Remove(0, sb.Length);
                    Delete(sb.Append(LogPath).Append("LogImport.txt").ToString());
                    sb.Remove(0, sb.Length);
                }
            }
            sb = null;
        }

    }

}