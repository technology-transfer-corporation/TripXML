using System;
using System.IO;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Xml;
using CoreWCF;
using CoreWCF.Channels;
using CoreWCF.Description;
using CoreWCF.Dispatcher;
using Microsoft.Extensions.Logging;
using TripXMLMain;

namespace wsTripXML.wsTravelTalk
{
    public class cSoapRQ : IDispatchMessageInspector
    {
        private readonly ILogger<cSoapRQ> _logger;

        public cSoapRQ(ILogger<cSoapRQ> logger)
        {
            _logger = logger;
        }

        // Equivalent to SoapMessageStage.BeforeDeserialize
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            string soapEnvelope = CaptureEnvelope(ref request);
            // The returned object is handed back to BeforeSendReply as correlationState
            return soapEnvelope;
        }

        // Equivalent to SoapMessageStage.AfterSerialize (+ exception handling)
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            string soapEnvelope = correlationState as string ?? string.Empty;

            if (reply is not null && reply.IsFault)
            {
                string faultText = ExtractFaultMessage(ref reply);
                _ = Task.Run(() => LogSoapException(faultText, soapEnvelope));
            }
        }

        private static string CaptureEnvelope(ref Message request)
        {
            // Message bodies can only be read once, so buffer and replace it
            using MessageBuffer buffer = request.CreateBufferedCopy(int.MaxValue);
            request = buffer.CreateMessage();

            using Message copy = buffer.CreateMessage();
            using var sw = new StringWriter();
            using var xw = XmlWriter.Create(sw, new XmlWriterSettings { OmitXmlDeclaration = true });
            copy.WriteMessage(xw);
            xw.Flush();
            return sw.ToString();
        }

        private static string ExtractFaultMessage(ref Message reply)
        {
            using MessageBuffer buffer = reply.CreateBufferedCopy(int.MaxValue);
            reply = buffer.CreateMessage();

            using Message copy = buffer.CreateMessage();
            MessageFault fault = MessageFault.CreateFault(copy, int.MaxValue);
            return fault.Reason?.GetMatchingTranslation()?.Text ?? "Unknown SOAP fault";
        }

        private void LogSoapException(string soapException, string soapEnvelope)
        {
            try
            {
                using var oDA = new cDA();
                oDA.AddSoapException(ref soapException, ref soapEnvelope);
            }
            catch (Exception ex)
            {
                modMain.LogSoapExceptionToFile(ref soapException, ref soapEnvelope, ex.Message);
                _logger.LogError(ex, "Failed to persist SOAP exception");
            }
        }
    }

    // Attach the inspector to every endpoint via a service behavior
    public class SoapRequestInspectorBehavior : IServiceBehavior
    {
        private readonly ILogger<cSoapRQ> _logger;

        public SoapRequestInspectorBehavior(ILogger<cSoapRQ> logger)
        {
            _logger = logger;
        }

        public void AddBindingParameters(ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase,
            System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        { }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) { }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            var inspector = new cSoapRQ(_logger);

            foreach (ChannelDispatcherBase cdb in serviceHostBase.ChannelDispatchers)
            {
                if (cdb is ChannelDispatcher cd)
                {
                    foreach (EndpointDispatcher ed in cd.Endpoints)
                    {
                        ed.DispatchRuntime.MessageInspectors.Add(inspector);
                    }
                }
            }
        }
    }
}