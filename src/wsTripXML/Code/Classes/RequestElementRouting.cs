using System;
using System.Collections.Generic;
using CoreWCF;
using CoreWCF.Channels;
using CoreWCF.Description;
using CoreWCF.Dispatcher;

namespace wsTripXML.wsTravelTalk
{
    /// <summary>
    /// Reproduces ASMX SoapDocumentService(RoutingStyle = SoapServiceRoutingStyle.RequestElement):
    /// the operation is selected by the first element of the SOAP body, not by SOAPAction.
    /// Legacy clients that send an empty or non-standard SOAPAction keep working.
    /// </summary>
    public class RequestElementRoutingBehavior : IServiceBehavior
    {
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) { }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
            System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters) { }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (var channelDispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                if (channelDispatcherBase is not ChannelDispatcher channelDispatcher) continue;

                foreach (var endpointDispatcher in channelDispatcher.Endpoints)
                {
                    if (endpointDispatcher.IsSystemEndpoint) continue;

                    var operationNames = new HashSet<string>(StringComparer.Ordinal);
                    foreach (var op in endpointDispatcher.DispatchRuntime.Operations)
                    {
                        operationNames.Add(op.Name);
                    }

                    // The default ContractFilter matches on SOAPAction and rejects empty/odd
                    // actions before operation selection ever runs — ASMX RequestElement
                    // routing ignores the action entirely, so accept everything here and
                    // let the operation selector route by body element.
                    endpointDispatcher.ContractFilter = new MatchAllMessageFilter();
                    endpointDispatcher.DispatchRuntime.OperationSelector =
                        new RequestElementOperationSelector(operationNames);
                }
            }
        }
    }

    public class RequestElementOperationSelector : IDispatchOperationSelector
    {
        private readonly HashSet<string> _operationNames;

        public RequestElementOperationSelector(HashSet<string> operationNames)
        {
            _operationNames = operationNames;
        }

        public string SelectOperation(ref Message message)
        {
            // Peek the first body element without consuming the message.
            var buffer = message.CreateBufferedCopy(int.MaxValue);
            message = buffer.CreateMessage();

            string elementName;
            using (var probe = buffer.CreateMessage())
            {
                if (probe.IsEmpty) return string.Empty;
                using var reader = probe.GetReaderAtBodyContents();
                elementName = reader.LocalName;
            }

            // Wrapped doc/literal: the request wrapper element name equals the operation name.
            // Unknown names are returned as-is so CoreWCF raises the standard dispatch fault.
            return _operationNames.Contains(elementName) ? elementName : elementName;
        }
    }
}
