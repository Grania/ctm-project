﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CTMF_Desktop_App.ServiceReference {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference.WebServiceSoap")]
    public interface WebServiceSoap {
        
        // CODEGEN: Generating message contract since message GetAccountRequest has headers
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetAccount", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        CTMF_Desktop_App.ServiceReference.GetAccountResponse GetAccount(CTMF_Desktop_App.ServiceReference.GetAccountRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetAccount", ReplyAction="*")]
        System.Threading.Tasks.Task<CTMF_Desktop_App.ServiceReference.GetAccountResponse> GetAccountAsync(CTMF_Desktop_App.ServiceReference.GetAccountRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class AuthSoapHeader : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string authStringField;
        
        private System.Xml.XmlAttribute[] anyAttrField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string authString {
            get {
                return this.authStringField;
            }
            set {
                this.authStringField = value;
                this.RaisePropertyChanged("authString");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr {
            get {
                return this.anyAttrField;
            }
            set {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetAccount", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetAccountRequest {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        public CTMF_Desktop_App.ServiceReference.AuthSoapHeader AuthSoapHeader;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string username;
        
        public GetAccountRequest() {
        }
        
        public GetAccountRequest(CTMF_Desktop_App.ServiceReference.AuthSoapHeader AuthSoapHeader, string username) {
            this.AuthSoapHeader = AuthSoapHeader;
            this.username = username;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetAccountResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetAccountResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public System.Data.DataTable GetAccountResult;
        
        public GetAccountResponse() {
        }
        
        public GetAccountResponse(System.Data.DataTable GetAccountResult) {
            this.GetAccountResult = GetAccountResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface WebServiceSoapChannel : CTMF_Desktop_App.ServiceReference.WebServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WebServiceSoapClient : System.ServiceModel.ClientBase<CTMF_Desktop_App.ServiceReference.WebServiceSoap>, CTMF_Desktop_App.ServiceReference.WebServiceSoap {
        
        public WebServiceSoapClient() {
        }
        
        public WebServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WebServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        CTMF_Desktop_App.ServiceReference.GetAccountResponse CTMF_Desktop_App.ServiceReference.WebServiceSoap.GetAccount(CTMF_Desktop_App.ServiceReference.GetAccountRequest request) {
            return base.Channel.GetAccount(request);
        }
        
        public System.Data.DataTable GetAccount(CTMF_Desktop_App.ServiceReference.AuthSoapHeader AuthSoapHeader, string username) {
            CTMF_Desktop_App.ServiceReference.GetAccountRequest inValue = new CTMF_Desktop_App.ServiceReference.GetAccountRequest();
            inValue.AuthSoapHeader = AuthSoapHeader;
            inValue.username = username;
            CTMF_Desktop_App.ServiceReference.GetAccountResponse retVal = ((CTMF_Desktop_App.ServiceReference.WebServiceSoap)(this)).GetAccount(inValue);
            return retVal.GetAccountResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CTMF_Desktop_App.ServiceReference.GetAccountResponse> CTMF_Desktop_App.ServiceReference.WebServiceSoap.GetAccountAsync(CTMF_Desktop_App.ServiceReference.GetAccountRequest request) {
            return base.Channel.GetAccountAsync(request);
        }
        
        public System.Threading.Tasks.Task<CTMF_Desktop_App.ServiceReference.GetAccountResponse> GetAccountAsync(CTMF_Desktop_App.ServiceReference.AuthSoapHeader AuthSoapHeader, string username) {
            CTMF_Desktop_App.ServiceReference.GetAccountRequest inValue = new CTMF_Desktop_App.ServiceReference.GetAccountRequest();
            inValue.AuthSoapHeader = AuthSoapHeader;
            inValue.username = username;
            return ((CTMF_Desktop_App.ServiceReference.WebServiceSoap)(this)).GetAccountAsync(inValue);
        }
    }
}
