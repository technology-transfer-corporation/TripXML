using System;
using System.Diagnostics;
using static System.IO.File;
using System.Text;
using System.Threading;
using System.Web.Services;
using System.Xml;
using Microsoft.VisualBasic;
using TripXMLMain;


namespace wsTripXML.wsTravelTalk
{


    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsImportLog", Name = "wsImportLog", Description = "A TripXML Web Service to Import Log from Log File.")]


    public class wsImportLog : WebService
    {

        #region  Web Services Designer Generated Code 

        public wsImportLog() : base()
        {

            // This call is required by the Web Services Designer.
            InitializeComponent();

            // Add your own initialization code after the InitializeComponent() call

        }

        // Required by the Web Services Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Web Services Designer
        // It can be modified using the Web Services Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        protected override void Dispose(bool disposing)
        {
            // CODEGEN: This procedure is required by the Web Services Designer
            // Do not modify it using the code editor.
            if (disposing)
            {
                if (components is not null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion
        private StringBuilder sb = new StringBuilder();

        private void ImportLog()
        {
            cLog oLog = null;

            try
            {

                oLog = new cLog();

                oLog.ImportLog();
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsImportLog", "Error Importing Log.", ex.Message, string.Empty);
            }
            finally
            {

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

        [WebMethod(Description = "Import Log from Log File. Progress is sent to the Trace.")]
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

        [WebMethod(Description = "View the first 200 errors Log from Log File.")]
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