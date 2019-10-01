﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.296
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.296.
// 
#pragma warning disable 1591

namespace Subscriber.AlboTelematico.tkn {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="BasicHttpBinding_IToken", Namespace="http://nttdata.com/2012/Pi3")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Response))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Request))]
    public partial class Token : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetAuthenticationTokenOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Token() {
            this.Url = "http://pis.test2.pitre.tn.it/infotn_coll-ws/VtDocsWS/WebServices/Token.svc/basic";
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
        public event GetAuthenticationTokenCompletedEventHandler GetAuthenticationTokenCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://nttdata.com/2012/Pi3/IToken/GetAuthenticationToken", RequestNamespace="http://nttdata.com/2012/Pi3", ResponseNamespace="http://nttdata.com/2012/Pi3", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public GetAuthenticationTokenResponse GetAuthenticationToken([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] GetAuthenticationTokenRequest request) {
            object[] results = this.Invoke("GetAuthenticationToken", new object[] {
                        request});
            return ((GetAuthenticationTokenResponse)(results[0]));
        }
        
        /// <remarks/>
        public void GetAuthenticationTokenAsync(GetAuthenticationTokenRequest request) {
            this.GetAuthenticationTokenAsync(request, null);
        }
        
        /// <remarks/>
        public void GetAuthenticationTokenAsync(GetAuthenticationTokenRequest request, object userState) {
            if ((this.GetAuthenticationTokenOperationCompleted == null)) {
                this.GetAuthenticationTokenOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAuthenticationTokenOperationCompleted);
            }
            this.InvokeAsync("GetAuthenticationToken", new object[] {
                        request}, this.GetAuthenticationTokenOperationCompleted, userState);
        }
        
        private void OnGetAuthenticationTokenOperationCompleted(object arg) {
            if ((this.GetAuthenticationTokenCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAuthenticationTokenCompleted(this, new GetAuthenticationTokenCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/VtDocsWS.Services.Token.GetAuthentication" +
        "Token")]
    public partial class GetAuthenticationTokenRequest : Request {
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetAuthenticationTokenRequest))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/VtDocsWS.Services")]
    public partial class Request {
        
        private string authenticationTokenField;
        
        private string codeAdmField;
        
        private string codeApplicationField;
        
        private string codeRoleLoginField;
        
        private string userNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string AuthenticationToken {
            get {
                return this.authenticationTokenField;
            }
            set {
                this.authenticationTokenField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string CodeAdm {
            get {
                return this.codeAdmField;
            }
            set {
                this.codeAdmField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string CodeApplication {
            get {
                return this.codeApplicationField;
            }
            set {
                this.codeApplicationField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string CodeRoleLogin {
            get {
                return this.codeRoleLoginField;
            }
            set {
                this.codeRoleLoginField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string UserName {
            get {
                return this.userNameField;
            }
            set {
                this.userNameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetAuthenticationTokenResponse))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/VtDocsWS.Services")]
    public partial class Response {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/VtDocsWS.Services.Token.GetAuthentication" +
        "Token")]
    public partial class GetAuthenticationTokenResponse : Response {
        
        private string authenticationTokenField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string AuthenticationToken {
            get {
                return this.authenticationTokenField;
            }
            set {
                this.authenticationTokenField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetAuthenticationTokenCompletedEventHandler(object sender, GetAuthenticationTokenCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAuthenticationTokenCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetAuthenticationTokenCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public GetAuthenticationTokenResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((GetAuthenticationTokenResponse)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591