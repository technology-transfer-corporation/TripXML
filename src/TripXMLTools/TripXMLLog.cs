using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TripXMLTools
{
    public sealed class TripXMLLog
    {
        private static readonly Lazy<TripXMLLog> Lazy = new Lazy<TripXMLLog>(() => new TripXMLLog());

        public static string ErrorMessage { get; set; }

        public static TripXMLLog Instance { get { return Lazy.Value; } }

        private TripXMLLog()
        {
            ErrorMessage = string.Empty;
        }

        public static void LogMessage(string msgType, ref string message, DateTime requestTime, DateTime responseTime, String logCategory, String providerName, String systemName, String userName)
        {
            //var sb = new StringBuilder();
            //dynamic logWriter = EnterpriseLibraryContainer.Current.GetInstance<LogWriter>();
            try
            {
                DateTimeFormatInfo myDTFI = new CultureInfo("en-US", true).DateTimeFormat;
                TimeSpan dur = responseTime - requestTime;

                var msg = new JObject(
                    new JProperty("Type", msgType),
                    new JProperty("RequestTime", requestTime.ToString("dd MMM yyyy HH:mm:ss")),
                    new JProperty("ResponseTime", responseTime.ToString("dd MMM yyyy HH:mm:ss")),
                    new JProperty("Duration(Sec)", dur.TotalSeconds.ToString(CultureInfo.InvariantCulture)),
                    new JProperty("Provider", providerName),
                    new JProperty("System", systemName),
                    new JProperty("User Name", userName),
                    new JProperty("GMT", DateTime.UtcNow.ToString(myDTFI).Substring(11)),
                    new JProperty("Message", message)
                );
                string strLine = JsonConvert.SerializeObject(msg);

                //sb.Append("<Message").Append(" Type='").Append(msgType).Append("'").Append(" RequestTime='");
                //sb.Append(requestTime.ToString("dd MMM yyyy HH:mm:ss")).Append("'").Append(" ResponseTime='");
                //sb.Append(responseTime.ToString("dd MMM yyyy HH:mm:ss")).Append("'");
                //sb.Append(" Duration(Sec)='").Append(dur.TotalSeconds.ToString(CultureInfo.InvariantCulture)).Append("'");
                //sb.Append(" Provider='").Append(providerName).Append("'");
                //sb.Append(" System='").Append(systemName).Append("'");
                //sb.Append(" User Name='").Append(userName).Append("'");
                //sb.Append(" GMT='").Append(DateTime.UtcNow.ToString(myDTFI).Substring(11)).Append("'>");
                //sb.Append(Environment.NewLine).Append("<").Append(providerName).Append("Message>").Append(Environment.NewLine).Append(message);
                //sb.Append(Environment.NewLine).Append("</").Append(providerName).Append("Message>");
                //sb.Append(Environment.NewLine).Append("</Message>");
                //string strLine = sb.ToString();
                //sb.Remove(0, sb.Length);

                //logWriter.Write(strLine, logCategory);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                //logWriter.Write(ErrorMessage, "LogCategory:ERROR");
            }
            finally
            {
                //logWriter.Dispose();
            }
        }

        public static void LogSoapMessage(string message, DateTime requestTime, DateTime responseTime, String userName, string tracerID)
        {
            //var sb = new StringBuilder();
            //dynamic logWriter = EnterpriseLibraryContainer.Current.GetInstance<LogWriter>();

            try
            {
                DateTimeFormatInfo myDTFI = new CultureInfo("en-US", true).DateTimeFormat;
                TimeSpan dur = responseTime - requestTime;

                var msg = new JObject(
                        new JProperty("RequestTime", requestTime.ToString("dd MMM yyyy HH:mm:ss")),
                        new JProperty("ResponseTime", responseTime.ToString("dd MMM yyyy HH:mm:ss")),
                        new JProperty("Duration(Sec)", dur.TotalSeconds.ToString(CultureInfo.InvariantCulture)),
                        new JProperty("User Name", userName),
                        new JProperty("Tracer ID", tracerID),
                        new JProperty("GMT", DateTime.UtcNow.ToString(myDTFI).Substring(11)),
                        new JProperty("Message", message)
                        );
                string strLine = JsonConvert.SerializeObject(msg);

                //sb.Append("<Message").Append(" RequestTime='");
                //sb.Append(requestTime.ToString("dd MMM yyyy HH:mm:ss")).Append("'").Append(" ResponseTime='");
                //sb.Append(responseTime.ToString("dd MMM yyyy HH:mm:ss")).Append("'");
                //sb.Append(" Duration(Sec)='").Append(dur.TotalSeconds.ToString(CultureInfo.InvariantCulture)).Append("'");
                //sb.Append(" User Name='").Append(userName).Append("'");
                //sb.Append(" Tracer ID='").Append(tracerID).Append("'");
                //sb.Append(" GMT='").Append(DateTime.UtcNow.ToString(myDTFI).Substring(11)).Append("'>");
                //sb.Append(Environment.NewLine).Append(message).Append(Environment.NewLine).Append("</Message>");

                //string strLine = sb.ToString();
                //sb.Remove(0, sb.Length);

                //logWriter.Write(strLine, "SoapLog");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                //logWriter.Write(ErrorMessage, "SoapLog:ERROR");
            }
            finally
            {
                //logWriter.Dispose();
            }
        }

        public static void LogErrorMessage(string message,String userName, String tracerID)
        {
            //var sb = new StringBuilder();
            //dynamic logWriter = EnterpriseLibraryContainer.Current.GetInstance<LogWriter>();
            try
            {
                DateTimeFormatInfo myDTFI = new CultureInfo("en-US", true).DateTimeFormat;

                var msg = new JObject(
                    new JProperty("User Name", userName),
                    new JProperty("Tracer ID", tracerID),
                    new JProperty("GMT", DateTime.UtcNow.ToString(myDTFI).Substring(11)),
                    new JProperty("Message", message)
                );
                string strLine = JsonConvert.SerializeObject(msg);

                //sb.Append("<Message");
                //sb.Append(" User Name='").Append(userName).Append("'");
                //sb.Append(" Tracer ID='").Append(tracerID).Append("'");
                //sb.Append(" GMT='").Append(DateTime.UtcNow.ToString(myDTFI).Substring(11)).Append("'>");
                //sb.Append(Environment.NewLine).Append(message).Append(Environment.NewLine).Append("</Message>");
                //string strLine = sb.ToString();
                //sb.Remove(0, sb.Length);

                //logWriter.Write(strLine, "SoapError");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                //logWriter.Write(ErrorMessage, "SoapError:ERROR");
            }
            finally
            {
                //logWriter.Dispose();
            }
        }
    }
}
