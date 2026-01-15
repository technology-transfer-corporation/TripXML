using System;
using System.Threading;
using System.Web.Services.Protocols;
using TripXMLMain;


namespace wsTripXML.wsTravelTalk
{


    public class cSoapRQ : SoapExtension
    {

        private string mstrSoapEnvelope = "";
        private string mstrSoapException = "";
        private System.IO.StreamReader sr;

        public override object GetInitializer(Type serviceType)
        {
            return null;
        }

        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return null;
        }

        public override void Initialize(object initializer)
        {

        }

        public override void ProcessMessage(SoapMessage message)
        {
            try
            {
                switch (message.Stage)
                {
                    case SoapMessageStage.BeforeDeserialize:
                        {
                            GetSoapEnvelope(ref message);
                            break;
                        }
                    case SoapMessageStage.AfterSerialize:
                        {
                            if (sr is not null)
                            {
                                sr.Close();
                                sr = null;
                            }
                            GC.Collect();
                            break;
                        }
                }

                if (message.Exception is not null)
                {

                    mstrSoapException = message.Exception.Message;

                    var oLofThread = new Thread(new ThreadStart(LogSoapException));

                    oLofThread.Start();

                    throw message.Exception;

                }
            }
            catch (SoapException ex)
            {
                throw ex;
            }

        }

        public void GetSoapEnvelope(ref SoapMessage myMessage)
        {

            try
            {
                sr = new System.IO.StreamReader(myMessage.Stream);

                if (sr.BaseStream.CanSeek)
                {
                    if (sr.BaseStream.CanRead)
                        mstrSoapEnvelope = sr.ReadToEnd();
                    myMessage.Stream.Position = 0L;
                }
            }
            finally
            {

            }

        }

        private void LogSoapException()
        {
            cDA oDA = null;

            try
            {
                oDA = new cDA();

                oDA.AddSoapException(ref mstrSoapException, ref mstrSoapEnvelope);
            }

            catch (Exception ex)
            {
                modMain.LogSoapExceptionToFile(ref mstrSoapException, ref mstrSoapEnvelope, ex.Message);
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

    }

}