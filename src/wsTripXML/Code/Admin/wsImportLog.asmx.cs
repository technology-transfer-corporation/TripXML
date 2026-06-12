using System;
using System.Diagnostics;
using static System.IO.File;
using System.Text;
using System.Threading;
using System.Xml;
using Microsoft.VisualBasic;
using TripXMLMain;


namespace wsTripXML.wsTravelTalk
{
    public partial class wsImportLog
    {

        private readonly modMain _modMain;

        public wsImportLog(modMain modMain)
        {
            _modMain = modMain;
        }
        private StringBuilder sb = new StringBuilder();

        private void ImportLog()
        {
            try
            {
                // cLog.ImportLog() was left commented out (half-refactored VB FileSystem code)
                // by the VB->C# conversion; this admin endpoint is not deployed in production.
                // Restore once cLog.ImportLog is finished.
                CoreLib.SendTrace("", "wsImportLog", "ImportLog is not available in this build (cLog.ImportLog pending port).", "", string.Empty);
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsImportLog", "Error Importing Log.", ex.Message, string.Empty);
            }
        }

        private string ViewErrorLog()
        {
            string xmlLog = "";
            XmlDocument oDoc = null;
            XmlElement oRoot = null;

            try
            {

                xmlLog = ReadLogFile(false);

                oDoc = new XmlDocument();
                oDoc.LoadXml(xmlLog);
                oRoot = oDoc.DocumentElement;

                xmlLog = "<ErrorLog>";

                foreach (XmlNode oNode in oRoot.SelectNodes("Line[LogType='1']"))
                {
                    xmlLog += oNode.SelectSingleNode("ExError").OuterXml;
                    xmlLog += oNode.SelectSingleNode("MessageID").OuterXml;
                    xmlLog += oNode.SelectSingleNode("Message").OuterXml;
                    xmlLog += oNode.SelectSingleNode("MessageDate").OuterXml;
                }

                return sb.Append(xmlLog).Append("</ErrorLog>").ToString();
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            sb = null;
        }

        private string ReadLogFile(bool DeleteLog)
        {
            var FileNumber = default(int);
            string strLine = "";
            var i = default(int);
            var sb2 = new StringBuilder();
            try
            {
                CoreLib.SendTrace("", "wsImportLog", sb.Append("Reading Log File ").Append(modCore.LogPath).Append(string.Format("{0}_Log.txt", DateTime.Now.ToShortDateString())).ToString(), "", string.Empty);
                sb.Remove(0, sb.Length);

                if (Exists(sb.Append(modCore.LogPath).Append(string.Format("{0}_Log.txt", DateTime.Now.ToShortDateString())).ToString()))
                {
                    sb.Remove(0, sb.Length);
                    if (DeleteLog)
                    {
                        Move(sb.Append(modCore.LogPath).Append(string.Format("{0}_Log.txt", DateTime.Now.ToShortDateString())).ToString(), sb2.Append(modCore.LogPath).Append("LogImport.txt").ToString());
                        sb.Remove(0, sb.Length);
                        sb2.Remove(0, sb2.Length);
                    }
                    else
                    {
                        Copy(sb.Append(modCore.LogPath).Append(string.Format("{0}_Log.txt", DateTime.Now.ToShortDateString())).ToString(), sb2.Append(modCore.LogPath).Append("LogImport.txt").ToString(), true);
                        sb.Remove(0, sb.Length);
                        sb2.Remove(0, sb2.Length);
                    }
                }
                else
                {
                    throw new Exception(sb.Append("Log File ").Append(modCore.LogPath).Append("Log.txt Not found.").ToString());
                }

                FileNumber = FileSystem.FreeFile();

                FileSystem.FileOpen(FileNumber, sb.Append(modCore.LogPath).Append("LogImport.txt").ToString(), OpenMode.Input, OpenAccess.Read, OpenShare.LockWrite);
                sb.Remove(0, sb.Length);

                while (!FileSystem.EOF(FileNumber))
                {
                    strLine += FileSystem.LineInput(FileNumber);
                    i += 1;
                    if (i > 400)
                        break;
                }

                return sb.Append("<LogFile>").Append(strLine).Append("</LogFile>").ToString();
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                FileSystem.FileClose(FileNumber);
                sb.Remove(0, sb.Length);
                if (Exists(sb.Append(modCore.LogPath).Append("LogImport.txt").ToString()))
                {
                    sb.Remove(0, sb.Length);
                    Delete(sb.Append(modCore.LogPath).Append("LogImport.txt").ToString());
                    sb.Remove(0, sb.Length);
                }
            }
            sb = null;
        }
        public string wmImportLog()
        {

            try
            {
                var oLofThread = new Thread(new ThreadStart(ImportLog));

                oLofThread.Start();

                return "Import Log Process was started. Monitor the Trace Tool to view the progress.";
            }

            catch (Exception ex)
            {
                return sb.Append("Error Importing Log File. ").Append(ex.Message).ToString();
                sb.Remove(0, sb.Length);
            }
            sb = null;
        }
        public string wmViewErrorLog()
        {

            try
            {
                return ViewErrorLog();
            }
            catch (Exception ex)
            {
                return sb.Append("Error Viewing Log File. ").Append(ex.Message).ToString();
                sb.Remove(0, sb.Length);
            }
            sb = null;
        }

    }

}