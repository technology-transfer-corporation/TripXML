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

namespace AmadeusWS.wsBlackList {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="wsFlightBlackListSoap", Namespace="http://admintalk/wsFlightBlackList")]
    public partial class wsFlightBlackList : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback wmFlightDisplayOperationCompleted;
        
        private System.Threading.SendOrPostCallback wmFlightAddOperationCompleted;
        
        private System.Threading.SendOrPostCallback wmFlightDeleteOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public wsFlightBlackList() {
            this.Url = global::AmadeusWS.Properties.Settings.Default.AmadeusWS_wsBlackList_wsFlightBlackList;
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
        public event wmFlightDisplayCompletedEventHandler wmFlightDisplayCompleted;
        
        /// <remarks/>
        public event wmFlightAddCompletedEventHandler wmFlightAddCompleted;
        
        /// <remarks/>
        public event wmFlightDeleteCompletedEventHandler wmFlightDeleteCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://admintalk/wsFlightBlackList/wmFlightDisplay", RequestNamespace="http://admintalk/wsFlightBlackList", ResponseNamespace="http://admintalk/wsFlightBlackList", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string wmFlightDisplay(string Airline) {
            object[] results = this.Invoke("wmFlightDisplay", new object[] {
                        Airline});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void wmFlightDisplayAsync(string Airline) {
            this.wmFlightDisplayAsync(Airline, null);
        }
        
        /// <remarks/>
        public void wmFlightDisplayAsync(string Airline, object userState) {
            if ((this.wmFlightDisplayOperationCompleted == null)) {
                this.wmFlightDisplayOperationCompleted = new System.Threading.SendOrPostCallback(this.OnwmFlightDisplayOperationCompleted);
            }
            this.InvokeAsync("wmFlightDisplay", new object[] {
                        Airline}, this.wmFlightDisplayOperationCompleted, userState);
        }
        
        private void OnwmFlightDisplayOperationCompleted(object arg) {
            if ((this.wmFlightDisplayCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.wmFlightDisplayCompleted(this, new wmFlightDisplayCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://admintalk/wsFlightBlackList/wmFlightAdd", RequestNamespace="http://admintalk/wsFlightBlackList", ResponseNamespace="http://admintalk/wsFlightBlackList", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string wmFlightAdd(string Airline, string FlightNo, System.DateTime Departure, string COS) {
            object[] results = this.Invoke("wmFlightAdd", new object[] {
                        Airline,
                        FlightNo,
                        Departure,
                        COS});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void wmFlightAddAsync(string Airline, string FlightNo, System.DateTime Departure, string COS) {
            this.wmFlightAddAsync(Airline, FlightNo, Departure, COS, null);
        }
        
        /// <remarks/>
        public void wmFlightAddAsync(string Airline, string FlightNo, System.DateTime Departure, string COS, object userState) {
            if ((this.wmFlightAddOperationCompleted == null)) {
                this.wmFlightAddOperationCompleted = new System.Threading.SendOrPostCallback(this.OnwmFlightAddOperationCompleted);
            }
            this.InvokeAsync("wmFlightAdd", new object[] {
                        Airline,
                        FlightNo,
                        Departure,
                        COS}, this.wmFlightAddOperationCompleted, userState);
        }
        
        private void OnwmFlightAddOperationCompleted(object arg) {
            if ((this.wmFlightAddCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.wmFlightAddCompleted(this, new wmFlightAddCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://admintalk/wsFlightBlackList/wmFlightDelete", RequestNamespace="http://admintalk/wsFlightBlackList", ResponseNamespace="http://admintalk/wsFlightBlackList", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string wmFlightDelete(System.DateTime DeleteDate) {
            object[] results = this.Invoke("wmFlightDelete", new object[] {
                        DeleteDate});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void wmFlightDeleteAsync(System.DateTime DeleteDate) {
            this.wmFlightDeleteAsync(DeleteDate, null);
        }
        
        /// <remarks/>
        public void wmFlightDeleteAsync(System.DateTime DeleteDate, object userState) {
            if ((this.wmFlightDeleteOperationCompleted == null)) {
                this.wmFlightDeleteOperationCompleted = new System.Threading.SendOrPostCallback(this.OnwmFlightDeleteOperationCompleted);
            }
            this.InvokeAsync("wmFlightDelete", new object[] {
                        DeleteDate}, this.wmFlightDeleteOperationCompleted, userState);
        }
        
        private void OnwmFlightDeleteOperationCompleted(object arg) {
            if ((this.wmFlightDeleteCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.wmFlightDeleteCompleted(this, new wmFlightDeleteCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void wmFlightDisplayCompletedEventHandler(object sender, wmFlightDisplayCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class wmFlightDisplayCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal wmFlightDisplayCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void wmFlightAddCompletedEventHandler(object sender, wmFlightAddCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class wmFlightAddCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal wmFlightAddCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void wmFlightDeleteCompletedEventHandler(object sender, wmFlightDeleteCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class wmFlightDeleteCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal wmFlightDeleteCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
}

#pragma warning restore 1591