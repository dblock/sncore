﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 2.0.50727.4927.
// 
#pragma warning disable 1591

namespace SnCore.Web.Soap.Tests.WebTagWordService {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WebTagWordServiceSoap", Namespace="http://www.vestris.com/sncore/ns/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TransitServiceOfAccount))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TransitServiceOfTagWord))]
    public partial class WebTagWordService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback CreateOrUpdateTagWordOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetTagWordByIdOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetTagWordsOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetTagWordsCountOperationCompleted;
        
        private System.Threading.SendOrPostCallback DeleteTagWordOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetTagWordAccountsByIdOperationCompleted;
        
        private System.Threading.SendOrPostCallback SearchTagWordAccountsOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WebTagWordService() {
            this.Url = global::SnCore.Web.Soap.Tests.Properties.Settings.Default.SnCore_Web_Soap_Tests_WebTagWordService_WebTagWordService;
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
        public event CreateOrUpdateTagWordCompletedEventHandler CreateOrUpdateTagWordCompleted;
        
        /// <remarks/>
        public event GetTagWordByIdCompletedEventHandler GetTagWordByIdCompleted;
        
        /// <remarks/>
        public event GetTagWordsCompletedEventHandler GetTagWordsCompleted;
        
        /// <remarks/>
        public event GetTagWordsCountCompletedEventHandler GetTagWordsCountCompleted;
        
        /// <remarks/>
        public event DeleteTagWordCompletedEventHandler DeleteTagWordCompleted;
        
        /// <remarks/>
        public event GetTagWordAccountsByIdCompletedEventHandler GetTagWordAccountsByIdCompleted;
        
        /// <remarks/>
        public event SearchTagWordAccountsCompletedEventHandler SearchTagWordAccountsCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.vestris.com/sncore/ns/CreateOrUpdateTagWord", RequestNamespace="http://www.vestris.com/sncore/ns/", ResponseNamespace="http://www.vestris.com/sncore/ns/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int CreateOrUpdateTagWord(string ticket, TransitTagWord tagword) {
            object[] results = this.Invoke("CreateOrUpdateTagWord", new object[] {
                        ticket,
                        tagword});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void CreateOrUpdateTagWordAsync(string ticket, TransitTagWord tagword) {
            this.CreateOrUpdateTagWordAsync(ticket, tagword, null);
        }
        
        /// <remarks/>
        public void CreateOrUpdateTagWordAsync(string ticket, TransitTagWord tagword, object userState) {
            if ((this.CreateOrUpdateTagWordOperationCompleted == null)) {
                this.CreateOrUpdateTagWordOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreateOrUpdateTagWordOperationCompleted);
            }
            this.InvokeAsync("CreateOrUpdateTagWord", new object[] {
                        ticket,
                        tagword}, this.CreateOrUpdateTagWordOperationCompleted, userState);
        }
        
        private void OnCreateOrUpdateTagWordOperationCompleted(object arg) {
            if ((this.CreateOrUpdateTagWordCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CreateOrUpdateTagWordCompleted(this, new CreateOrUpdateTagWordCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.vestris.com/sncore/ns/GetTagWordById", RequestNamespace="http://www.vestris.com/sncore/ns/", ResponseNamespace="http://www.vestris.com/sncore/ns/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public TransitTagWord GetTagWordById(string ticket, int id) {
            object[] results = this.Invoke("GetTagWordById", new object[] {
                        ticket,
                        id});
            return ((TransitTagWord)(results[0]));
        }
        
        /// <remarks/>
        public void GetTagWordByIdAsync(string ticket, int id) {
            this.GetTagWordByIdAsync(ticket, id, null);
        }
        
        /// <remarks/>
        public void GetTagWordByIdAsync(string ticket, int id, object userState) {
            if ((this.GetTagWordByIdOperationCompleted == null)) {
                this.GetTagWordByIdOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetTagWordByIdOperationCompleted);
            }
            this.InvokeAsync("GetTagWordById", new object[] {
                        ticket,
                        id}, this.GetTagWordByIdOperationCompleted, userState);
        }
        
        private void OnGetTagWordByIdOperationCompleted(object arg) {
            if ((this.GetTagWordByIdCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetTagWordByIdCompleted(this, new GetTagWordByIdCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.vestris.com/sncore/ns/GetTagWords", RequestNamespace="http://www.vestris.com/sncore/ns/", ResponseNamespace="http://www.vestris.com/sncore/ns/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public TransitTagWord[] GetTagWords(string ticket, TransitTagWordQueryOptions queryoptions, ServiceQueryOptions options) {
            object[] results = this.Invoke("GetTagWords", new object[] {
                        ticket,
                        queryoptions,
                        options});
            return ((TransitTagWord[])(results[0]));
        }
        
        /// <remarks/>
        public void GetTagWordsAsync(string ticket, TransitTagWordQueryOptions queryoptions, ServiceQueryOptions options) {
            this.GetTagWordsAsync(ticket, queryoptions, options, null);
        }
        
        /// <remarks/>
        public void GetTagWordsAsync(string ticket, TransitTagWordQueryOptions queryoptions, ServiceQueryOptions options, object userState) {
            if ((this.GetTagWordsOperationCompleted == null)) {
                this.GetTagWordsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetTagWordsOperationCompleted);
            }
            this.InvokeAsync("GetTagWords", new object[] {
                        ticket,
                        queryoptions,
                        options}, this.GetTagWordsOperationCompleted, userState);
        }
        
        private void OnGetTagWordsOperationCompleted(object arg) {
            if ((this.GetTagWordsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetTagWordsCompleted(this, new GetTagWordsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.vestris.com/sncore/ns/GetTagWordsCount", RequestNamespace="http://www.vestris.com/sncore/ns/", ResponseNamespace="http://www.vestris.com/sncore/ns/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int GetTagWordsCount(string ticket, TransitTagWordQueryOptions queryoptions) {
            object[] results = this.Invoke("GetTagWordsCount", new object[] {
                        ticket,
                        queryoptions});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void GetTagWordsCountAsync(string ticket, TransitTagWordQueryOptions queryoptions) {
            this.GetTagWordsCountAsync(ticket, queryoptions, null);
        }
        
        /// <remarks/>
        public void GetTagWordsCountAsync(string ticket, TransitTagWordQueryOptions queryoptions, object userState) {
            if ((this.GetTagWordsCountOperationCompleted == null)) {
                this.GetTagWordsCountOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetTagWordsCountOperationCompleted);
            }
            this.InvokeAsync("GetTagWordsCount", new object[] {
                        ticket,
                        queryoptions}, this.GetTagWordsCountOperationCompleted, userState);
        }
        
        private void OnGetTagWordsCountOperationCompleted(object arg) {
            if ((this.GetTagWordsCountCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetTagWordsCountCompleted(this, new GetTagWordsCountCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.vestris.com/sncore/ns/DeleteTagWord", RequestNamespace="http://www.vestris.com/sncore/ns/", ResponseNamespace="http://www.vestris.com/sncore/ns/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void DeleteTagWord(string ticket, int id) {
            this.Invoke("DeleteTagWord", new object[] {
                        ticket,
                        id});
        }
        
        /// <remarks/>
        public void DeleteTagWordAsync(string ticket, int id) {
            this.DeleteTagWordAsync(ticket, id, null);
        }
        
        /// <remarks/>
        public void DeleteTagWordAsync(string ticket, int id, object userState) {
            if ((this.DeleteTagWordOperationCompleted == null)) {
                this.DeleteTagWordOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteTagWordOperationCompleted);
            }
            this.InvokeAsync("DeleteTagWord", new object[] {
                        ticket,
                        id}, this.DeleteTagWordOperationCompleted, userState);
        }
        
        private void OnDeleteTagWordOperationCompleted(object arg) {
            if ((this.DeleteTagWordCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteTagWordCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.vestris.com/sncore/ns/GetTagWordAccountsById", RequestNamespace="http://www.vestris.com/sncore/ns/", ResponseNamespace="http://www.vestris.com/sncore/ns/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public TransitAccount[] GetTagWordAccountsById(string ticket, int id, ServiceQueryOptions options) {
            object[] results = this.Invoke("GetTagWordAccountsById", new object[] {
                        ticket,
                        id,
                        options});
            return ((TransitAccount[])(results[0]));
        }
        
        /// <remarks/>
        public void GetTagWordAccountsByIdAsync(string ticket, int id, ServiceQueryOptions options) {
            this.GetTagWordAccountsByIdAsync(ticket, id, options, null);
        }
        
        /// <remarks/>
        public void GetTagWordAccountsByIdAsync(string ticket, int id, ServiceQueryOptions options, object userState) {
            if ((this.GetTagWordAccountsByIdOperationCompleted == null)) {
                this.GetTagWordAccountsByIdOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetTagWordAccountsByIdOperationCompleted);
            }
            this.InvokeAsync("GetTagWordAccountsById", new object[] {
                        ticket,
                        id,
                        options}, this.GetTagWordAccountsByIdOperationCompleted, userState);
        }
        
        private void OnGetTagWordAccountsByIdOperationCompleted(object arg) {
            if ((this.GetTagWordAccountsByIdCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetTagWordAccountsByIdCompleted(this, new GetTagWordAccountsByIdCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.vestris.com/sncore/ns/SearchTagWordAccounts", RequestNamespace="http://www.vestris.com/sncore/ns/", ResponseNamespace="http://www.vestris.com/sncore/ns/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public TransitAccount[] SearchTagWordAccounts(string ticket, string search, ServiceQueryOptions options) {
            object[] results = this.Invoke("SearchTagWordAccounts", new object[] {
                        ticket,
                        search,
                        options});
            return ((TransitAccount[])(results[0]));
        }
        
        /// <remarks/>
        public void SearchTagWordAccountsAsync(string ticket, string search, ServiceQueryOptions options) {
            this.SearchTagWordAccountsAsync(ticket, search, options, null);
        }
        
        /// <remarks/>
        public void SearchTagWordAccountsAsync(string ticket, string search, ServiceQueryOptions options, object userState) {
            if ((this.SearchTagWordAccountsOperationCompleted == null)) {
                this.SearchTagWordAccountsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSearchTagWordAccountsOperationCompleted);
            }
            this.InvokeAsync("SearchTagWordAccounts", new object[] {
                        ticket,
                        search,
                        options}, this.SearchTagWordAccountsOperationCompleted, userState);
        }
        
        private void OnSearchTagWordAccountsOperationCompleted(object arg) {
            if ((this.SearchTagWordAccountsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SearchTagWordAccountsCompleted(this, new SearchTagWordAccountsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.vestris.com/sncore/ns/")]
    public partial class TransitTagWord : TransitServiceOfTagWord {
        
        private int frequencyField;
        
        private string wordField;
        
        private bool promotedField;
        
        private bool excludedField;
        
        /// <remarks/>
        public int Frequency {
            get {
                return this.frequencyField;
            }
            set {
                this.frequencyField = value;
            }
        }
        
        /// <remarks/>
        public string Word {
            get {
                return this.wordField;
            }
            set {
                this.wordField = value;
            }
        }
        
        /// <remarks/>
        public bool Promoted {
            get {
                return this.promotedField;
            }
            set {
                this.promotedField = value;
            }
        }
        
        /// <remarks/>
        public bool Excluded {
            get {
                return this.excludedField;
            }
            set {
                this.excludedField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TransitTagWord))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.vestris.com/sncore/ns/")]
    public abstract partial class TransitServiceOfTagWord {
        
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TransitAccount))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.vestris.com/sncore/ns/")]
    public abstract partial class TransitServiceOfAccount {
        
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.vestris.com/sncore/ns/")]
    public partial class TransitAccount : TransitServiceOfAccount {
        
        private bool isPasswordExpiredField;
        
        private System.DateTime createdField;
        
        private bool isAdministratorField;
        
        private string nameField;
        
        private System.DateTime birthdayField;
        
        private System.DateTime lastLoginField;
        
        private int pictureIdField;
        
        private string stateField;
        
        private string countryField;
        
        private string cityField;
        
        private int timeZoneField;
        
        private string signatureField;
        
        private int lCIDField;
        
        private string cultureField;
        
        private string passwordField;
        
        /// <remarks/>
        public bool IsPasswordExpired {
            get {
                return this.isPasswordExpiredField;
            }
            set {
                this.isPasswordExpiredField = value;
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
        public bool IsAdministrator {
            get {
                return this.isAdministratorField;
            }
            set {
                this.isAdministratorField = value;
            }
        }
        
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
        public System.DateTime Birthday {
            get {
                return this.birthdayField;
            }
            set {
                this.birthdayField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime LastLogin {
            get {
                return this.lastLoginField;
            }
            set {
                this.lastLoginField = value;
            }
        }
        
        /// <remarks/>
        public int PictureId {
            get {
                return this.pictureIdField;
            }
            set {
                this.pictureIdField = value;
            }
        }
        
        /// <remarks/>
        public string State {
            get {
                return this.stateField;
            }
            set {
                this.stateField = value;
            }
        }
        
        /// <remarks/>
        public string Country {
            get {
                return this.countryField;
            }
            set {
                this.countryField = value;
            }
        }
        
        /// <remarks/>
        public string City {
            get {
                return this.cityField;
            }
            set {
                this.cityField = value;
            }
        }
        
        /// <remarks/>
        public int TimeZone {
            get {
                return this.timeZoneField;
            }
            set {
                this.timeZoneField = value;
            }
        }
        
        /// <remarks/>
        public string Signature {
            get {
                return this.signatureField;
            }
            set {
                this.signatureField = value;
            }
        }
        
        /// <remarks/>
        public int LCID {
            get {
                return this.lCIDField;
            }
            set {
                this.lCIDField = value;
            }
        }
        
        /// <remarks/>
        public string Culture {
            get {
                return this.cultureField;
            }
            set {
                this.cultureField = value;
            }
        }
        
        /// <remarks/>
        public string Password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.vestris.com/sncore/ns/")]
    public enum TransitTagWordQueryOptions {
        
        /// <remarks/>
        Promoted,
        
        /// <remarks/>
        Excluded,
        
        /// <remarks/>
        New,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void CreateOrUpdateTagWordCompletedEventHandler(object sender, CreateOrUpdateTagWordCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CreateOrUpdateTagWordCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CreateOrUpdateTagWordCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void GetTagWordByIdCompletedEventHandler(object sender, GetTagWordByIdCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetTagWordByIdCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetTagWordByIdCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public TransitTagWord Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((TransitTagWord)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void GetTagWordsCompletedEventHandler(object sender, GetTagWordsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetTagWordsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetTagWordsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public TransitTagWord[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((TransitTagWord[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void GetTagWordsCountCompletedEventHandler(object sender, GetTagWordsCountCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetTagWordsCountCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetTagWordsCountCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void DeleteTagWordCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void GetTagWordAccountsByIdCompletedEventHandler(object sender, GetTagWordAccountsByIdCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetTagWordAccountsByIdCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetTagWordAccountsByIdCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public TransitAccount[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((TransitAccount[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void SearchTagWordAccountsCompletedEventHandler(object sender, SearchTagWordAccountsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SearchTagWordAccountsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SearchTagWordAccountsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public TransitAccount[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((TransitAccount[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591