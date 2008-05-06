﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1434
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 2.0.50727.1434.
// 
#pragma warning disable 1591

namespace SnCore.Web.Soap.Tests.WebLicenseService {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WebLicenseServiceSoap", Namespace="http://www.vestris.com/sncore/ns/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TransitServiceOfAccountLicense))]
    public partial class WebLicenseService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetAccountLicensesOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetAccountLicensesCountOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetAccountLicenseByIdOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetAccountLicenseByAccountIdOperationCompleted;
        
        private System.Threading.SendOrPostCallback CreateOrUpdateAccountLicenseOperationCompleted;
        
        private System.Threading.SendOrPostCallback DeleteAccountLicenseOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WebLicenseService() {
            this.Url = global::SnCore.Web.Soap.Tests.Properties.Settings.Default.SnCore_Web_Soap_Tests_WebLicenseService_WebLicenseService;
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
        public event GetAccountLicensesCompletedEventHandler GetAccountLicensesCompleted;
        
        /// <remarks/>
        public event GetAccountLicensesCountCompletedEventHandler GetAccountLicensesCountCompleted;
        
        /// <remarks/>
        public event GetAccountLicenseByIdCompletedEventHandler GetAccountLicenseByIdCompleted;
        
        /// <remarks/>
        public event GetAccountLicenseByAccountIdCompletedEventHandler GetAccountLicenseByAccountIdCompleted;
        
        /// <remarks/>
        public event CreateOrUpdateAccountLicenseCompletedEventHandler CreateOrUpdateAccountLicenseCompleted;
        
        /// <remarks/>
        public event DeleteAccountLicenseCompletedEventHandler DeleteAccountLicenseCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.vestris.com/sncore/ns/GetAccountLicenses", RequestNamespace="http://www.vestris.com/sncore/ns/", ResponseNamespace="http://www.vestris.com/sncore/ns/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public TransitAccountLicense[] GetAccountLicenses(string ticket, int id, ServiceQueryOptions options) {
            object[] results = this.Invoke("GetAccountLicenses", new object[] {
                        ticket,
                        id,
                        options});
            return ((TransitAccountLicense[])(results[0]));
        }
        
        /// <remarks/>
        public void GetAccountLicensesAsync(string ticket, int id, ServiceQueryOptions options) {
            this.GetAccountLicensesAsync(ticket, id, options, null);
        }
        
        /// <remarks/>
        public void GetAccountLicensesAsync(string ticket, int id, ServiceQueryOptions options, object userState) {
            if ((this.GetAccountLicensesOperationCompleted == null)) {
                this.GetAccountLicensesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAccountLicensesOperationCompleted);
            }
            this.InvokeAsync("GetAccountLicenses", new object[] {
                        ticket,
                        id,
                        options}, this.GetAccountLicensesOperationCompleted, userState);
        }
        
        private void OnGetAccountLicensesOperationCompleted(object arg) {
            if ((this.GetAccountLicensesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAccountLicensesCompleted(this, new GetAccountLicensesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.vestris.com/sncore/ns/GetAccountLicensesCount", RequestNamespace="http://www.vestris.com/sncore/ns/", ResponseNamespace="http://www.vestris.com/sncore/ns/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int GetAccountLicensesCount(string ticket, int id) {
            object[] results = this.Invoke("GetAccountLicensesCount", new object[] {
                        ticket,
                        id});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void GetAccountLicensesCountAsync(string ticket, int id) {
            this.GetAccountLicensesCountAsync(ticket, id, null);
        }
        
        /// <remarks/>
        public void GetAccountLicensesCountAsync(string ticket, int id, object userState) {
            if ((this.GetAccountLicensesCountOperationCompleted == null)) {
                this.GetAccountLicensesCountOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAccountLicensesCountOperationCompleted);
            }
            this.InvokeAsync("GetAccountLicensesCount", new object[] {
                        ticket,
                        id}, this.GetAccountLicensesCountOperationCompleted, userState);
        }
        
        private void OnGetAccountLicensesCountOperationCompleted(object arg) {
            if ((this.GetAccountLicensesCountCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAccountLicensesCountCompleted(this, new GetAccountLicensesCountCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.vestris.com/sncore/ns/GetAccountLicenseById", RequestNamespace="http://www.vestris.com/sncore/ns/", ResponseNamespace="http://www.vestris.com/sncore/ns/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public TransitAccountLicense GetAccountLicenseById(string ticket, int id) {
            object[] results = this.Invoke("GetAccountLicenseById", new object[] {
                        ticket,
                        id});
            return ((TransitAccountLicense)(results[0]));
        }
        
        /// <remarks/>
        public void GetAccountLicenseByIdAsync(string ticket, int id) {
            this.GetAccountLicenseByIdAsync(ticket, id, null);
        }
        
        /// <remarks/>
        public void GetAccountLicenseByIdAsync(string ticket, int id, object userState) {
            if ((this.GetAccountLicenseByIdOperationCompleted == null)) {
                this.GetAccountLicenseByIdOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAccountLicenseByIdOperationCompleted);
            }
            this.InvokeAsync("GetAccountLicenseById", new object[] {
                        ticket,
                        id}, this.GetAccountLicenseByIdOperationCompleted, userState);
        }
        
        private void OnGetAccountLicenseByIdOperationCompleted(object arg) {
            if ((this.GetAccountLicenseByIdCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAccountLicenseByIdCompleted(this, new GetAccountLicenseByIdCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.vestris.com/sncore/ns/GetAccountLicenseByAccountId", RequestNamespace="http://www.vestris.com/sncore/ns/", ResponseNamespace="http://www.vestris.com/sncore/ns/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public TransitAccountLicense GetAccountLicenseByAccountId(string ticket, int id) {
            object[] results = this.Invoke("GetAccountLicenseByAccountId", new object[] {
                        ticket,
                        id});
            return ((TransitAccountLicense)(results[0]));
        }
        
        /// <remarks/>
        public void GetAccountLicenseByAccountIdAsync(string ticket, int id) {
            this.GetAccountLicenseByAccountIdAsync(ticket, id, null);
        }
        
        /// <remarks/>
        public void GetAccountLicenseByAccountIdAsync(string ticket, int id, object userState) {
            if ((this.GetAccountLicenseByAccountIdOperationCompleted == null)) {
                this.GetAccountLicenseByAccountIdOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAccountLicenseByAccountIdOperationCompleted);
            }
            this.InvokeAsync("GetAccountLicenseByAccountId", new object[] {
                        ticket,
                        id}, this.GetAccountLicenseByAccountIdOperationCompleted, userState);
        }
        
        private void OnGetAccountLicenseByAccountIdOperationCompleted(object arg) {
            if ((this.GetAccountLicenseByAccountIdCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAccountLicenseByAccountIdCompleted(this, new GetAccountLicenseByAccountIdCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.vestris.com/sncore/ns/CreateOrUpdateAccountLicense", RequestNamespace="http://www.vestris.com/sncore/ns/", ResponseNamespace="http://www.vestris.com/sncore/ns/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int CreateOrUpdateAccountLicense(string ticket, TransitAccountLicense license) {
            object[] results = this.Invoke("CreateOrUpdateAccountLicense", new object[] {
                        ticket,
                        license});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void CreateOrUpdateAccountLicenseAsync(string ticket, TransitAccountLicense license) {
            this.CreateOrUpdateAccountLicenseAsync(ticket, license, null);
        }
        
        /// <remarks/>
        public void CreateOrUpdateAccountLicenseAsync(string ticket, TransitAccountLicense license, object userState) {
            if ((this.CreateOrUpdateAccountLicenseOperationCompleted == null)) {
                this.CreateOrUpdateAccountLicenseOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreateOrUpdateAccountLicenseOperationCompleted);
            }
            this.InvokeAsync("CreateOrUpdateAccountLicense", new object[] {
                        ticket,
                        license}, this.CreateOrUpdateAccountLicenseOperationCompleted, userState);
        }
        
        private void OnCreateOrUpdateAccountLicenseOperationCompleted(object arg) {
            if ((this.CreateOrUpdateAccountLicenseCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CreateOrUpdateAccountLicenseCompleted(this, new CreateOrUpdateAccountLicenseCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.vestris.com/sncore/ns/DeleteAccountLicense", RequestNamespace="http://www.vestris.com/sncore/ns/", ResponseNamespace="http://www.vestris.com/sncore/ns/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void DeleteAccountLicense(string ticket, int id) {
            this.Invoke("DeleteAccountLicense", new object[] {
                        ticket,
                        id});
        }
        
        /// <remarks/>
        public void DeleteAccountLicenseAsync(string ticket, int id) {
            this.DeleteAccountLicenseAsync(ticket, id, null);
        }
        
        /// <remarks/>
        public void DeleteAccountLicenseAsync(string ticket, int id, object userState) {
            if ((this.DeleteAccountLicenseOperationCompleted == null)) {
                this.DeleteAccountLicenseOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteAccountLicenseOperationCompleted);
            }
            this.InvokeAsync("DeleteAccountLicense", new object[] {
                        ticket,
                        id}, this.DeleteAccountLicenseOperationCompleted, userState);
        }
        
        private void OnDeleteAccountLicenseOperationCompleted(object arg) {
            if ((this.DeleteAccountLicenseCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteAccountLicenseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1434")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.vestris.com/sncore/ns/")]
    public partial class ServiceQueryOptions {
        
        private int pageSizeField;
        
        private int pageNumberField;
        
        /// <remarks/>
        public int PageSize {
            get {
                return this.pageSizeField;
            }
            set {
                this.pageSizeField = value;
            }
        }
        
        /// <remarks/>
        public int PageNumber {
            get {
                return this.pageNumberField;
            }
            set {
                this.pageNumberField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TransitAccountLicense))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1434")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.vestris.com/sncore/ns/")]
    public abstract partial class TransitServiceOfAccountLicense {
        
        private int idField;
        
        /// <remarks/>
        public int Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1434")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.vestris.com/sncore/ns/")]
    public partial class TransitAccountLicense : TransitServiceOfAccountLicense {
        
        private string nameField;
        
        private string licenseUrlField;
        
        private System.DateTime createdField;
        
        private System.DateTime modifiedField;
        
        private int accountIdField;
        
        private string imageUrlField;
        
        /// <remarks/>
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        public string LicenseUrl {
            get {
                return this.licenseUrlField;
            }
            set {
                this.licenseUrlField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime Created {
            get {
                return this.createdField;
            }
            set {
                this.createdField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime Modified {
            get {
                return this.modifiedField;
            }
            set {
                this.modifiedField = value;
            }
        }
        
        /// <remarks/>
        public int AccountId {
            get {
                return this.accountIdField;
            }
            set {
                this.accountIdField = value;
            }
        }
        
        /// <remarks/>
        public string ImageUrl {
            get {
                return this.imageUrlField;
            }
            set {
                this.imageUrlField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    public delegate void GetAccountLicensesCompletedEventHandler(object sender, GetAccountLicensesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAccountLicensesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetAccountLicensesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public TransitAccountLicense[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((TransitAccountLicense[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    public delegate void GetAccountLicensesCountCompletedEventHandler(object sender, GetAccountLicensesCountCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAccountLicensesCountCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetAccountLicensesCountCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    public delegate void GetAccountLicenseByIdCompletedEventHandler(object sender, GetAccountLicenseByIdCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAccountLicenseByIdCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetAccountLicenseByIdCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public TransitAccountLicense Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((TransitAccountLicense)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    public delegate void GetAccountLicenseByAccountIdCompletedEventHandler(object sender, GetAccountLicenseByAccountIdCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAccountLicenseByAccountIdCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetAccountLicenseByAccountIdCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public TransitAccountLicense Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((TransitAccountLicense)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    public delegate void CreateOrUpdateAccountLicenseCompletedEventHandler(object sender, CreateOrUpdateAccountLicenseCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CreateOrUpdateAccountLicenseCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CreateOrUpdateAccountLicenseCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    public delegate void DeleteAccountLicenseCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
}

#pragma warning restore 1591