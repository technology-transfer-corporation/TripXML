﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace Galileo.wsGalileoProd {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="XmlSelectSoap", Namespace="http://webservices.galileo.com")]
    public partial class XmlSelect : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback SubmitXmlOperationCompleted;
        
        private System.Threading.SendOrPostCallback MultiSubmitXmlOperationCompleted;
        
        private System.Threading.SendOrPostCallback BeginSessionOperationCompleted;
        
        private System.Threading.SendOrPostCallback EndSessionOperationCompleted;
        
        private System.Threading.SendOrPostCallback SubmitXmlOnSessionOperationCompleted;
        
        private System.Threading.SendOrPostCallback SubmitTerminalTransactionOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetIdentityInfoOperationCompleted;
        
        private System.Threading.SendOrPostCallback SubmitCruiseTransactionOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public XmlSelect() {
            this.Url = "https://americas.webservices.travelport.com/B2BGateway/service/XmlSelect";
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event SubmitXmlCompletedEventHandler SubmitXmlCompleted;
        
        /// <remarks/>
        public event MultiSubmitXmlCompletedEventHandler MultiSubmitXmlCompleted;
        
        /// <remarks/>
        public event BeginSessionCompletedEventHandler BeginSessionCompleted;
        
        /// <remarks/>
        public event EndSessionCompletedEventHandler EndSessionCompleted;
        
        /// <remarks/>
        public event SubmitXmlOnSessionCompletedEventHandler SubmitXmlOnSessionCompleted;
        
        /// <remarks/>
        public event SubmitTerminalTransactionCompletedEventHandler SubmitTerminalTransactionCompleted;
        
        /// <remarks/>
        public event GetIdentityInfoCompletedEventHandler GetIdentityInfoCompleted;
        
        /// <remarks/>
        public event SubmitCruiseTransactionCompletedEventHandler SubmitCruiseTransactionCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://webservices.galileo.com/SubmitXml", RequestNamespace="http://webservices.galileo.com", ResponseNamespace="http://webservices.galileo.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlElement SubmitXml(string Profile, System.Xml.XmlElement Request, System.Xml.XmlElement Filter) {
            object[] results = this.Invoke("SubmitXml", new object[] {
                        Profile,
                        Request,
                        Filter});
            return ((System.Xml.XmlElement)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginSubmitXml(string Profile, System.Xml.XmlElement Request, System.Xml.XmlElement Filter, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("SubmitXml", new object[] {
                        Profile,
                        Request,
                        Filter}, callback, asyncState);
        }
        
        /// <remarks/>
        public System.Xml.XmlElement EndSubmitXml(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((System.Xml.XmlElement)(results[0]));
        }
        
        /// <remarks/>
        public void SubmitXmlAsync(string Profile, System.Xml.XmlElement Request, System.Xml.XmlElement Filter) {
            this.SubmitXmlAsync(Profile, Request, Filter, null);
        }
        
        /// <remarks/>
        public void SubmitXmlAsync(string Profile, System.Xml.XmlElement Request, System.Xml.XmlElement Filter, object userState) {
            if ((this.SubmitXmlOperationCompleted == null)) {
                this.SubmitXmlOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSubmitXmlOperationCompleted);
            }
            this.InvokeAsync("SubmitXml", new object[] {
                        Profile,
                        Request,
                        Filter}, this.SubmitXmlOperationCompleted, userState);
        }
        
        private void OnSubmitXmlOperationCompleted(object arg) {
            if ((this.SubmitXmlCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SubmitXmlCompleted(this, new SubmitXmlCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://webservices.galileo.com/MultiSubmitXml", RequestNamespace="http://webservices.galileo.com", ResponseNamespace="http://webservices.galileo.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Responses")]
        public System.Xml.XmlElement MultiSubmitXml(string Profile, System.Xml.XmlElement Requests) {
            object[] results = this.Invoke("MultiSubmitXml", new object[] {
                        Profile,
                        Requests});
            return ((System.Xml.XmlElement)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginMultiSubmitXml(string Profile, System.Xml.XmlElement Requests, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("MultiSubmitXml", new object[] {
                        Profile,
                        Requests}, callback, asyncState);
        }
        
        /// <remarks/>
        public System.Xml.XmlElement EndMultiSubmitXml(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((System.Xml.XmlElement)(results[0]));
        }
        
        /// <remarks/>
        public void MultiSubmitXmlAsync(string Profile, System.Xml.XmlElement Requests) {
            this.MultiSubmitXmlAsync(Profile, Requests, null);
        }
        
        /// <remarks/>
        public void MultiSubmitXmlAsync(string Profile, System.Xml.XmlElement Requests, object userState) {
            if ((this.MultiSubmitXmlOperationCompleted == null)) {
                this.MultiSubmitXmlOperationCompleted = new System.Threading.SendOrPostCallback(this.OnMultiSubmitXmlOperationCompleted);
            }
            this.InvokeAsync("MultiSubmitXml", new object[] {
                        Profile,
                        Requests}, this.MultiSubmitXmlOperationCompleted, userState);
        }
        
        private void OnMultiSubmitXmlOperationCompleted(object arg) {
            if ((this.MultiSubmitXmlCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.MultiSubmitXmlCompleted(this, new MultiSubmitXmlCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://webservices.galileo.com/BeginSession", RequestNamespace="http://webservices.galileo.com", ResponseNamespace="http://webservices.galileo.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string BeginSession(string Profile) {
            object[] results = this.Invoke("BeginSession", new object[] {
                        Profile});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginBeginSession(string Profile, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("BeginSession", new object[] {
                        Profile}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndBeginSession(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void BeginSessionAsync(string Profile) {
            this.BeginSessionAsync(Profile, null);
        }
        
        /// <remarks/>
        public void BeginSessionAsync(string Profile, object userState) {
            if ((this.BeginSessionOperationCompleted == null)) {
                this.BeginSessionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnBeginSessionOperationCompleted);
            }
            this.InvokeAsync("BeginSession", new object[] {
                        Profile}, this.BeginSessionOperationCompleted, userState);
        }
        
        private void OnBeginSessionOperationCompleted(object arg) {
            if ((this.BeginSessionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.BeginSessionCompleted(this, new BeginSessionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://webservices.galileo.com/EndSession", RequestNamespace="http://webservices.galileo.com", ResponseNamespace="http://webservices.galileo.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void EndSession(string Token) {
            this.Invoke("EndSession", new object[] {
                        Token});
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginEndSession(string Token, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("EndSession", new object[] {
                        Token}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EndEndSession(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
        
        /// <remarks/>
        public void EndSessionAsync(string Token) {
            this.EndSessionAsync(Token, null);
        }
        
        /// <remarks/>
        public void EndSessionAsync(string Token, object userState) {
            if ((this.EndSessionOperationCompleted == null)) {
                this.EndSessionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnEndSessionOperationCompleted);
            }
            this.InvokeAsync("EndSession", new object[] {
                        Token}, this.EndSessionOperationCompleted, userState);
        }
        
        private void OnEndSessionOperationCompleted(object arg) {
            if ((this.EndSessionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.EndSessionCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://webservices.galileo.com/SubmitXmlOnSession", RequestNamespace="http://webservices.galileo.com", ResponseNamespace="http://webservices.galileo.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlElement SubmitXmlOnSession(string Token, System.Xml.XmlElement Request, System.Xml.XmlElement Filter) {
            object[] results = this.Invoke("SubmitXmlOnSession", new object[] {
                        Token,
                        Request,
                        Filter});
            return ((System.Xml.XmlElement)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginSubmitXmlOnSession(string Token, System.Xml.XmlElement Request, System.Xml.XmlElement Filter, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("SubmitXmlOnSession", new object[] {
                        Token,
                        Request,
                        Filter}, callback, asyncState);
        }
        
        /// <remarks/>
        public System.Xml.XmlElement EndSubmitXmlOnSession(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((System.Xml.XmlElement)(results[0]));
        }
        
        /// <remarks/>
        public void SubmitXmlOnSessionAsync(string Token, System.Xml.XmlElement Request, System.Xml.XmlElement Filter) {
            this.SubmitXmlOnSessionAsync(Token, Request, Filter, null);
        }
        
        /// <remarks/>
        public void SubmitXmlOnSessionAsync(string Token, System.Xml.XmlElement Request, System.Xml.XmlElement Filter, object userState) {
            if ((this.SubmitXmlOnSessionOperationCompleted == null)) {
                this.SubmitXmlOnSessionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSubmitXmlOnSessionOperationCompleted);
            }
            this.InvokeAsync("SubmitXmlOnSession", new object[] {
                        Token,
                        Request,
                        Filter}, this.SubmitXmlOnSessionOperationCompleted, userState);
        }
        
        private void OnSubmitXmlOnSessionOperationCompleted(object arg) {
            if ((this.SubmitXmlOnSessionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SubmitXmlOnSessionCompleted(this, new SubmitXmlOnSessionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://webservices.galileo.com/SubmitTerminalTransaction", RequestNamespace="http://webservices.galileo.com", ResponseNamespace="http://webservices.galileo.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SubmitTerminalTransaction(string Token, string Request, string IntermediateResponse) {
            object[] results = this.Invoke("SubmitTerminalTransaction", new object[] {
                        Token,
                        Request,
                        IntermediateResponse});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginSubmitTerminalTransaction(string Token, string Request, string IntermediateResponse, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("SubmitTerminalTransaction", new object[] {
                        Token,
                        Request,
                        IntermediateResponse}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndSubmitTerminalTransaction(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void SubmitTerminalTransactionAsync(string Token, string Request, string IntermediateResponse) {
            this.SubmitTerminalTransactionAsync(Token, Request, IntermediateResponse, null);
        }
        
        /// <remarks/>
        public void SubmitTerminalTransactionAsync(string Token, string Request, string IntermediateResponse, object userState) {
            if ((this.SubmitTerminalTransactionOperationCompleted == null)) {
                this.SubmitTerminalTransactionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSubmitTerminalTransactionOperationCompleted);
            }
            this.InvokeAsync("SubmitTerminalTransaction", new object[] {
                        Token,
                        Request,
                        IntermediateResponse}, this.SubmitTerminalTransactionOperationCompleted, userState);
        }
        
        private void OnSubmitTerminalTransactionOperationCompleted(object arg) {
            if ((this.SubmitTerminalTransactionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SubmitTerminalTransactionCompleted(this, new SubmitTerminalTransactionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://webservices.galileo.com/GetIdentityInfo", RequestNamespace="http://webservices.galileo.com", ResponseNamespace="http://webservices.galileo.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlElement GetIdentityInfo(string Profile) {
            object[] results = this.Invoke("GetIdentityInfo", new object[] {
                        Profile});
            return ((System.Xml.XmlElement)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetIdentityInfo(string Profile, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetIdentityInfo", new object[] {
                        Profile}, callback, asyncState);
        }
        
        /// <remarks/>
        public System.Xml.XmlElement EndGetIdentityInfo(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((System.Xml.XmlElement)(results[0]));
        }
        
        /// <remarks/>
        public void GetIdentityInfoAsync(string Profile) {
            this.GetIdentityInfoAsync(Profile, null);
        }
        
        /// <remarks/>
        public void GetIdentityInfoAsync(string Profile, object userState) {
            if ((this.GetIdentityInfoOperationCompleted == null)) {
                this.GetIdentityInfoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetIdentityInfoOperationCompleted);
            }
            this.InvokeAsync("GetIdentityInfo", new object[] {
                        Profile}, this.GetIdentityInfoOperationCompleted, userState);
        }
        
        private void OnGetIdentityInfoOperationCompleted(object arg) {
            if ((this.GetIdentityInfoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetIdentityInfoCompleted(this, new GetIdentityInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://webservices.galileo.com/SubmitCruiseTransaction", RequestNamespace="http://webservices.galileo.com", ResponseNamespace="http://webservices.galileo.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Response")]
        public System.Xml.XmlElement SubmitCruiseTransaction(string Profile, ref string CorrelationToken, System.Xml.XmlElement Transactions) {
            object[] results = this.Invoke("SubmitCruiseTransaction", new object[] {
                        Profile,
                        CorrelationToken,
                        Transactions});
            CorrelationToken = ((string)(results[1]));
            return ((System.Xml.XmlElement)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginSubmitCruiseTransaction(string Profile, string CorrelationToken, System.Xml.XmlElement Transactions, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("SubmitCruiseTransaction", new object[] {
                        Profile,
                        CorrelationToken,
                        Transactions}, callback, asyncState);
        }
        
        /// <remarks/>
        public System.Xml.XmlElement EndSubmitCruiseTransaction(System.IAsyncResult asyncResult, out string CorrelationToken) {
            object[] results = this.EndInvoke(asyncResult);
            CorrelationToken = ((string)(results[1]));
            return ((System.Xml.XmlElement)(results[0]));
        }
        
        /// <remarks/>
        public void SubmitCruiseTransactionAsync(string Profile, string CorrelationToken, System.Xml.XmlElement Transactions) {
            this.SubmitCruiseTransactionAsync(Profile, CorrelationToken, Transactions, null);
        }
        
        /// <remarks/>
        public void SubmitCruiseTransactionAsync(string Profile, string CorrelationToken, System.Xml.XmlElement Transactions, object userState) {
            if ((this.SubmitCruiseTransactionOperationCompleted == null)) {
                this.SubmitCruiseTransactionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSubmitCruiseTransactionOperationCompleted);
            }
            this.InvokeAsync("SubmitCruiseTransaction", new object[] {
                        Profile,
                        CorrelationToken,
                        Transactions}, this.SubmitCruiseTransactionOperationCompleted, userState);
        }
        
        private void OnSubmitCruiseTransactionOperationCompleted(object arg) {
            if ((this.SubmitCruiseTransactionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SubmitCruiseTransactionCompleted(this, new SubmitCruiseTransactionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    public delegate void SubmitXmlCompletedEventHandler(object sender, SubmitXmlCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SubmitXmlCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SubmitXmlCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlElement Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlElement)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    public delegate void MultiSubmitXmlCompletedEventHandler(object sender, MultiSubmitXmlCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class MultiSubmitXmlCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal MultiSubmitXmlCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlElement Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlElement)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    public delegate void BeginSessionCompletedEventHandler(object sender, BeginSessionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class BeginSessionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal BeginSessionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    public delegate void EndSessionCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    public delegate void SubmitXmlOnSessionCompletedEventHandler(object sender, SubmitXmlOnSessionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SubmitXmlOnSessionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SubmitXmlOnSessionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlElement Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlElement)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    public delegate void SubmitTerminalTransactionCompletedEventHandler(object sender, SubmitTerminalTransactionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SubmitTerminalTransactionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SubmitTerminalTransactionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    public delegate void GetIdentityInfoCompletedEventHandler(object sender, GetIdentityInfoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetIdentityInfoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetIdentityInfoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlElement Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlElement)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    public delegate void SubmitCruiseTransactionCompletedEventHandler(object sender, SubmitCruiseTransactionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SubmitCruiseTransactionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SubmitCruiseTransactionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlElement Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlElement)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public string CorrelationToken {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
    }
}

#pragma warning restore 1591