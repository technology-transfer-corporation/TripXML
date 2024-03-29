﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Galileo.wsGalileoProd {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://webservices.galileo.com", ConfigurationName="wsGalileoProd.XmlSelectSoap")]
    public interface XmlSelectSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="https://webservices.galileo.com/SubmitXml", ReplyAction="*")]
        Galileo.wsGalileoProd.SubmitXmlResponse SubmitXml(Galileo.wsGalileoProd.SubmitXmlRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://webservices.galileo.com/SubmitXml", ReplyAction="*")]
        System.Threading.Tasks.Task<Galileo.wsGalileoProd.SubmitXmlResponse> SubmitXmlAsync(Galileo.wsGalileoProd.SubmitXmlRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://webservices.galileo.com/MultiSubmitXml", ReplyAction="*")]
        Galileo.wsGalileoProd.MultiSubmitXmlResponse MultiSubmitXml(Galileo.wsGalileoProd.MultiSubmitXmlRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://webservices.galileo.com/MultiSubmitXml", ReplyAction="*")]
        System.Threading.Tasks.Task<Galileo.wsGalileoProd.MultiSubmitXmlResponse> MultiSubmitXmlAsync(Galileo.wsGalileoProd.MultiSubmitXmlRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://webservices.galileo.com/BeginSession", ReplyAction="*")]
        Galileo.wsGalileoProd.BeginSessionResponse BeginSession(Galileo.wsGalileoProd.BeginSessionRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://webservices.galileo.com/BeginSession", ReplyAction="*")]
        System.Threading.Tasks.Task<Galileo.wsGalileoProd.BeginSessionResponse> BeginSessionAsync(Galileo.wsGalileoProd.BeginSessionRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://webservices.galileo.com/EndSession", ReplyAction="*")]
        Galileo.wsGalileoProd.EndSessionResponse EndSession(Galileo.wsGalileoProd.EndSessionRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://webservices.galileo.com/EndSession", ReplyAction="*")]
        System.Threading.Tasks.Task<Galileo.wsGalileoProd.EndSessionResponse> EndSessionAsync(Galileo.wsGalileoProd.EndSessionRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://webservices.galileo.com/SubmitXmlOnSession", ReplyAction="*")]
        Galileo.wsGalileoProd.SubmitXmlOnSessionResponse SubmitXmlOnSession(Galileo.wsGalileoProd.SubmitXmlOnSessionRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://webservices.galileo.com/SubmitXmlOnSession", ReplyAction="*")]
        System.Threading.Tasks.Task<Galileo.wsGalileoProd.SubmitXmlOnSessionResponse> SubmitXmlOnSessionAsync(Galileo.wsGalileoProd.SubmitXmlOnSessionRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://webservices.galileo.com/SubmitTerminalTransaction", ReplyAction="*")]
        Galileo.wsGalileoProd.SubmitTerminalTransactionResponse SubmitTerminalTransaction(Galileo.wsGalileoProd.SubmitTerminalTransactionRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://webservices.galileo.com/SubmitTerminalTransaction", ReplyAction="*")]
        System.Threading.Tasks.Task<Galileo.wsGalileoProd.SubmitTerminalTransactionResponse> SubmitTerminalTransactionAsync(Galileo.wsGalileoProd.SubmitTerminalTransactionRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://webservices.galileo.com/GetIdentityInfo", ReplyAction="*")]
        Galileo.wsGalileoProd.GetIdentityInfoResponse GetIdentityInfo(Galileo.wsGalileoProd.GetIdentityInfoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://webservices.galileo.com/GetIdentityInfo", ReplyAction="*")]
        System.Threading.Tasks.Task<Galileo.wsGalileoProd.GetIdentityInfoResponse> GetIdentityInfoAsync(Galileo.wsGalileoProd.GetIdentityInfoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://webservices.galileo.com/SubmitCruiseTransaction", ReplyAction="*")]
        Galileo.wsGalileoProd.SubmitCruiseTransactionResponse SubmitCruiseTransaction(Galileo.wsGalileoProd.SubmitCruiseTransactionRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://webservices.galileo.com/SubmitCruiseTransaction", ReplyAction="*")]
        System.Threading.Tasks.Task<Galileo.wsGalileoProd.SubmitCruiseTransactionResponse> SubmitCruiseTransactionAsync(Galileo.wsGalileoProd.SubmitCruiseTransactionRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SubmitXmlRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SubmitXml", Namespace="http://webservices.galileo.com", Order=0)]
        public Galileo.wsGalileoProd.SubmitXmlRequestBody Body;
        
        public SubmitXmlRequest() {
        }
        
        public SubmitXmlRequest(Galileo.wsGalileoProd.SubmitXmlRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://webservices.galileo.com")]
    public partial class SubmitXmlRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Profile;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public System.Xml.XmlElement Request;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public System.Xml.XmlElement Filter;
        
        public SubmitXmlRequestBody() {
        }
        
        public SubmitXmlRequestBody(string Profile, System.Xml.XmlElement Request, System.Xml.XmlElement Filter) {
            this.Profile = Profile;
            this.Request = Request;
            this.Filter = Filter;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SubmitXmlResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SubmitXmlResponse", Namespace="http://webservices.galileo.com", Order=0)]
        public Galileo.wsGalileoProd.SubmitXmlResponseBody Body;
        
        public SubmitXmlResponse() {
        }
        
        public SubmitXmlResponse(Galileo.wsGalileoProd.SubmitXmlResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://webservices.galileo.com")]
    public partial class SubmitXmlResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public System.Xml.XmlElement SubmitXmlResult;
        
        public SubmitXmlResponseBody() {
        }
        
        public SubmitXmlResponseBody(System.Xml.XmlElement SubmitXmlResult) {
            this.SubmitXmlResult = SubmitXmlResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class MultiSubmitXmlRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="MultiSubmitXml", Namespace="http://webservices.galileo.com", Order=0)]
        public Galileo.wsGalileoProd.MultiSubmitXmlRequestBody Body;
        
        public MultiSubmitXmlRequest() {
        }
        
        public MultiSubmitXmlRequest(Galileo.wsGalileoProd.MultiSubmitXmlRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://webservices.galileo.com")]
    public partial class MultiSubmitXmlRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Profile;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public System.Xml.XmlElement Requests;
        
        public MultiSubmitXmlRequestBody() {
        }
        
        public MultiSubmitXmlRequestBody(string Profile, System.Xml.XmlElement Requests) {
            this.Profile = Profile;
            this.Requests = Requests;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class MultiSubmitXmlResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="MultiSubmitXmlResponse", Namespace="http://webservices.galileo.com", Order=0)]
        public Galileo.wsGalileoProd.MultiSubmitXmlResponseBody Body;
        
        public MultiSubmitXmlResponse() {
        }
        
        public MultiSubmitXmlResponse(Galileo.wsGalileoProd.MultiSubmitXmlResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://webservices.galileo.com")]
    public partial class MultiSubmitXmlResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public System.Xml.XmlElement Responses;
        
        public MultiSubmitXmlResponseBody() {
        }
        
        public MultiSubmitXmlResponseBody(System.Xml.XmlElement Responses) {
            this.Responses = Responses;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class BeginSessionRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="BeginSession", Namespace="http://webservices.galileo.com", Order=0)]
        public Galileo.wsGalileoProd.BeginSessionRequestBody Body;
        
        public BeginSessionRequest() {
        }
        
        public BeginSessionRequest(Galileo.wsGalileoProd.BeginSessionRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://webservices.galileo.com")]
    public partial class BeginSessionRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Profile;
        
        public BeginSessionRequestBody() {
        }
        
        public BeginSessionRequestBody(string Profile) {
            this.Profile = Profile;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class BeginSessionResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="BeginSessionResponse", Namespace="http://webservices.galileo.com", Order=0)]
        public Galileo.wsGalileoProd.BeginSessionResponseBody Body;
        
        public BeginSessionResponse() {
        }
        
        public BeginSessionResponse(Galileo.wsGalileoProd.BeginSessionResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://webservices.galileo.com")]
    public partial class BeginSessionResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string BeginSessionResult;
        
        public BeginSessionResponseBody() {
        }
        
        public BeginSessionResponseBody(string BeginSessionResult) {
            this.BeginSessionResult = BeginSessionResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class EndSessionRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="EndSession", Namespace="http://webservices.galileo.com", Order=0)]
        public Galileo.wsGalileoProd.EndSessionRequestBody Body;
        
        public EndSessionRequest() {
        }
        
        public EndSessionRequest(Galileo.wsGalileoProd.EndSessionRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://webservices.galileo.com")]
    public partial class EndSessionRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Token;
        
        public EndSessionRequestBody() {
        }
        
        public EndSessionRequestBody(string Token) {
            this.Token = Token;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class EndSessionResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="EndSessionResponse", Namespace="http://webservices.galileo.com", Order=0)]
        public Galileo.wsGalileoProd.EndSessionResponseBody Body;
        
        public EndSessionResponse() {
        }
        
        public EndSessionResponse(Galileo.wsGalileoProd.EndSessionResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class EndSessionResponseBody {
        
        public EndSessionResponseBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SubmitXmlOnSessionRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SubmitXmlOnSession", Namespace="http://webservices.galileo.com", Order=0)]
        public Galileo.wsGalileoProd.SubmitXmlOnSessionRequestBody Body;
        
        public SubmitXmlOnSessionRequest() {
        }
        
        public SubmitXmlOnSessionRequest(Galileo.wsGalileoProd.SubmitXmlOnSessionRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://webservices.galileo.com")]
    public partial class SubmitXmlOnSessionRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Token;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public System.Xml.XmlElement Request;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public System.Xml.XmlElement Filter;
        
        public SubmitXmlOnSessionRequestBody() {
        }
        
        public SubmitXmlOnSessionRequestBody(string Token, System.Xml.XmlElement Request, System.Xml.XmlElement Filter) {
            this.Token = Token;
            this.Request = Request;
            this.Filter = Filter;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SubmitXmlOnSessionResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SubmitXmlOnSessionResponse", Namespace="http://webservices.galileo.com", Order=0)]
        public Galileo.wsGalileoProd.SubmitXmlOnSessionResponseBody Body;
        
        public SubmitXmlOnSessionResponse() {
        }
        
        public SubmitXmlOnSessionResponse(Galileo.wsGalileoProd.SubmitXmlOnSessionResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://webservices.galileo.com")]
    public partial class SubmitXmlOnSessionResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public System.Xml.XmlElement SubmitXmlOnSessionResult;
        
        public SubmitXmlOnSessionResponseBody() {
        }
        
        public SubmitXmlOnSessionResponseBody(System.Xml.XmlElement SubmitXmlOnSessionResult) {
            this.SubmitXmlOnSessionResult = SubmitXmlOnSessionResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SubmitTerminalTransactionRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SubmitTerminalTransaction", Namespace="http://webservices.galileo.com", Order=0)]
        public Galileo.wsGalileoProd.SubmitTerminalTransactionRequestBody Body;
        
        public SubmitTerminalTransactionRequest() {
        }
        
        public SubmitTerminalTransactionRequest(Galileo.wsGalileoProd.SubmitTerminalTransactionRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://webservices.galileo.com")]
    public partial class SubmitTerminalTransactionRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Profile;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string Token;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string Request;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string IntermediateResponse;
        
        public SubmitTerminalTransactionRequestBody() {
        }
        
        public SubmitTerminalTransactionRequestBody(string Profile, string Token, string Request, string IntermediateResponse) {
            this.Profile = Profile;
            this.Token = Token;
            this.Request = Request;
            this.IntermediateResponse = IntermediateResponse;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SubmitTerminalTransactionResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SubmitTerminalTransactionResponse", Namespace="http://webservices.galileo.com", Order=0)]
        public Galileo.wsGalileoProd.SubmitTerminalTransactionResponseBody Body;
        
        public SubmitTerminalTransactionResponse() {
        }
        
        public SubmitTerminalTransactionResponse(Galileo.wsGalileoProd.SubmitTerminalTransactionResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://webservices.galileo.com")]
    public partial class SubmitTerminalTransactionResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string SubmitTerminalTransactionResult;
        
        public SubmitTerminalTransactionResponseBody() {
        }
        
        public SubmitTerminalTransactionResponseBody(string SubmitTerminalTransactionResult) {
            this.SubmitTerminalTransactionResult = SubmitTerminalTransactionResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetIdentityInfoRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetIdentityInfo", Namespace="http://webservices.galileo.com", Order=0)]
        public Galileo.wsGalileoProd.GetIdentityInfoRequestBody Body;
        
        public GetIdentityInfoRequest() {
        }
        
        public GetIdentityInfoRequest(Galileo.wsGalileoProd.GetIdentityInfoRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://webservices.galileo.com")]
    public partial class GetIdentityInfoRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Profile;
        
        public GetIdentityInfoRequestBody() {
        }
        
        public GetIdentityInfoRequestBody(string Profile) {
            this.Profile = Profile;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetIdentityInfoResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetIdentityInfoResponse", Namespace="http://webservices.galileo.com", Order=0)]
        public Galileo.wsGalileoProd.GetIdentityInfoResponseBody Body;
        
        public GetIdentityInfoResponse() {
        }
        
        public GetIdentityInfoResponse(Galileo.wsGalileoProd.GetIdentityInfoResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://webservices.galileo.com")]
    public partial class GetIdentityInfoResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public System.Xml.XmlElement GetIdentityInfoResult;
        
        public GetIdentityInfoResponseBody() {
        }
        
        public GetIdentityInfoResponseBody(System.Xml.XmlElement GetIdentityInfoResult) {
            this.GetIdentityInfoResult = GetIdentityInfoResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SubmitCruiseTransactionRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SubmitCruiseTransaction", Namespace="http://webservices.galileo.com", Order=0)]
        public Galileo.wsGalileoProd.SubmitCruiseTransactionRequestBody Body;
        
        public SubmitCruiseTransactionRequest() {
        }
        
        public SubmitCruiseTransactionRequest(Galileo.wsGalileoProd.SubmitCruiseTransactionRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://webservices.galileo.com")]
    public partial class SubmitCruiseTransactionRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Profile;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string CorrelationToken;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public System.Xml.XmlElement Transactions;
        
        public SubmitCruiseTransactionRequestBody() {
        }
        
        public SubmitCruiseTransactionRequestBody(string Profile, string CorrelationToken, System.Xml.XmlElement Transactions) {
            this.Profile = Profile;
            this.CorrelationToken = CorrelationToken;
            this.Transactions = Transactions;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SubmitCruiseTransactionResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SubmitCruiseTransactionResponse", Namespace="http://webservices.galileo.com", Order=0)]
        public Galileo.wsGalileoProd.SubmitCruiseTransactionResponseBody Body;
        
        public SubmitCruiseTransactionResponse() {
        }
        
        public SubmitCruiseTransactionResponse(Galileo.wsGalileoProd.SubmitCruiseTransactionResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://webservices.galileo.com")]
    public partial class SubmitCruiseTransactionResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public System.Xml.XmlElement Response;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string CorrelationToken;
        
        public SubmitCruiseTransactionResponseBody() {
        }
        
        public SubmitCruiseTransactionResponseBody(System.Xml.XmlElement Response, string CorrelationToken) {
            this.Response = Response;
            this.CorrelationToken = CorrelationToken;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface XmlSelectSoapChannel : Galileo.wsGalileoProd.XmlSelectSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class XmlSelectSoapClient : System.ServiceModel.ClientBase<Galileo.wsGalileoProd.XmlSelectSoap>, Galileo.wsGalileoProd.XmlSelectSoap {
        
        public XmlSelectSoapClient() {
        }
        
        public XmlSelectSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public XmlSelectSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public XmlSelectSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public XmlSelectSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Galileo.wsGalileoProd.SubmitXmlResponse SubmitXml(Galileo.wsGalileoProd.SubmitXmlRequest request) {
            return base.Channel.SubmitXml(request);
        }
        
        public System.Threading.Tasks.Task<Galileo.wsGalileoProd.SubmitXmlResponse> SubmitXmlAsync(Galileo.wsGalileoProd.SubmitXmlRequest request) {
            return base.Channel.SubmitXmlAsync(request);
        }
        
        public Galileo.wsGalileoProd.MultiSubmitXmlResponse MultiSubmitXml(Galileo.wsGalileoProd.MultiSubmitXmlRequest request) {
            return base.Channel.MultiSubmitXml(request);
        }
        
        public System.Threading.Tasks.Task<Galileo.wsGalileoProd.MultiSubmitXmlResponse> MultiSubmitXmlAsync(Galileo.wsGalileoProd.MultiSubmitXmlRequest request) {
            return base.Channel.MultiSubmitXmlAsync(request);
        }
        
        public Galileo.wsGalileoProd.BeginSessionResponse BeginSession(Galileo.wsGalileoProd.BeginSessionRequest request) {
            return base.Channel.BeginSession(request);
        }
        
        public System.Threading.Tasks.Task<Galileo.wsGalileoProd.BeginSessionResponse> BeginSessionAsync(Galileo.wsGalileoProd.BeginSessionRequest request) {
            return base.Channel.BeginSessionAsync(request);
        }
        
        public Galileo.wsGalileoProd.EndSessionResponse EndSession(Galileo.wsGalileoProd.EndSessionRequest request) {
            return base.Channel.EndSession(request);
        }
        
        public System.Threading.Tasks.Task<Galileo.wsGalileoProd.EndSessionResponse> EndSessionAsync(Galileo.wsGalileoProd.EndSessionRequest request) {
            return base.Channel.EndSessionAsync(request);
        }
        
        public Galileo.wsGalileoProd.SubmitXmlOnSessionResponse SubmitXmlOnSession(Galileo.wsGalileoProd.SubmitXmlOnSessionRequest request) {
            return base.Channel.SubmitXmlOnSession(request);
        }
        
        public System.Threading.Tasks.Task<Galileo.wsGalileoProd.SubmitXmlOnSessionResponse> SubmitXmlOnSessionAsync(Galileo.wsGalileoProd.SubmitXmlOnSessionRequest request) {
            return base.Channel.SubmitXmlOnSessionAsync(request);
        }
        
        public Galileo.wsGalileoProd.SubmitTerminalTransactionResponse SubmitTerminalTransaction(Galileo.wsGalileoProd.SubmitTerminalTransactionRequest request) {
            return base.Channel.SubmitTerminalTransaction(request);
        }
        
        public System.Threading.Tasks.Task<Galileo.wsGalileoProd.SubmitTerminalTransactionResponse> SubmitTerminalTransactionAsync(Galileo.wsGalileoProd.SubmitTerminalTransactionRequest request) {
            return base.Channel.SubmitTerminalTransactionAsync(request);
        }
        
        public Galileo.wsGalileoProd.GetIdentityInfoResponse GetIdentityInfo(Galileo.wsGalileoProd.GetIdentityInfoRequest request) {
            return base.Channel.GetIdentityInfo(request);
        }
        
        public System.Threading.Tasks.Task<Galileo.wsGalileoProd.GetIdentityInfoResponse> GetIdentityInfoAsync(Galileo.wsGalileoProd.GetIdentityInfoRequest request) {
            return base.Channel.GetIdentityInfoAsync(request);
        }
        
        public Galileo.wsGalileoProd.SubmitCruiseTransactionResponse SubmitCruiseTransaction(Galileo.wsGalileoProd.SubmitCruiseTransactionRequest request) {
            return base.Channel.SubmitCruiseTransaction(request);
        }
        
        public System.Threading.Tasks.Task<Galileo.wsGalileoProd.SubmitCruiseTransactionResponse> SubmitCruiseTransactionAsync(Galileo.wsGalileoProd.SubmitCruiseTransactionRequest request) {
            return base.Channel.SubmitCruiseTransactionAsync(request);
        }
    }
}
