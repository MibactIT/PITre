﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.269.
// 
#pragma warning disable 1591

namespace MessageEngine.SimpleProxy {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="MessageEngineSimpleServicesSoap", Namespace="http://tempuri.org/")]
    public partial class MessageEngineSimpleServices : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback SendMessageOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public MessageEngineSimpleServices() {
            this.Url = global::MessageEngine.Properties.Settings.Default.MessageEngine_SimpleProxy_MessageEngineSimpleServices;
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
        public event SendMessageCompletedEventHandler SendMessageCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendMessage", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void SendMessage(string p_Interface, string p_Action, string p_Recipient_Type, string p_Message_Type, string p_From, string[] p_Recipients_List, string p_Subject, string p_Body) {
            this.Invoke("SendMessage", new object[] {
                        p_Interface,
                        p_Action,
                        p_Recipient_Type,
                        p_Message_Type,
                        p_From,
                        p_Recipients_List,
                        p_Subject,
                        p_Body});
        }
        
        /// <remarks/>
        public void SendMessageAsync(string p_Interface, string p_Action, string p_Recipient_Type, string p_Message_Type, string p_From, string[] p_Recipients_List, string p_Subject, string p_Body) {
            this.SendMessageAsync(p_Interface, p_Action, p_Recipient_Type, p_Message_Type, p_From, p_Recipients_List, p_Subject, p_Body, null);
        }
        
        /// <remarks/>
        public void SendMessageAsync(string p_Interface, string p_Action, string p_Recipient_Type, string p_Message_Type, string p_From, string[] p_Recipients_List, string p_Subject, string p_Body, object userState) {
            if ((this.SendMessageOperationCompleted == null)) {
                this.SendMessageOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendMessageOperationCompleted);
            }
            this.InvokeAsync("SendMessage", new object[] {
                        p_Interface,
                        p_Action,
                        p_Recipient_Type,
                        p_Message_Type,
                        p_From,
                        p_Recipients_List,
                        p_Subject,
                        p_Body}, this.SendMessageOperationCompleted, userState);
        }
        
        private void OnSendMessageOperationCompleted(object arg) {
            if ((this.SendMessageCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendMessageCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void SendMessageCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
}

#pragma warning restore 1591