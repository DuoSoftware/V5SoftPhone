﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DuoSoftware.DuoSoftPhone.RefUserAuth {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="UserAuth", Namespace="http://schemas.datacontract.org/2004/07/DuoAuthSvr")]
    [System.SerializableAttribute()]
    public partial class UserAuth : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private DuoSoftware.DuoSoftPhone.RefUserAuth.AccountContracts[] AccountContractsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool AccountRelatedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ApplicationField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int CompanyIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int[] CompanyIDsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.Dictionary<string, string> DataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IgnoreViweObjField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int ObjectIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SecurityTokenField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int TenantIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int[] TenantIDsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime TokenExpireOnField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UserNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool WriteField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal[] guUserGrpIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private long guUserIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int[] viweObjectIDsField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public DuoSoftware.DuoSoftPhone.RefUserAuth.AccountContracts[] AccountContracts {
            get {
                return this.AccountContractsField;
            }
            set {
                if ((object.ReferenceEquals(this.AccountContractsField, value) != true)) {
                    this.AccountContractsField = value;
                    this.RaisePropertyChanged("AccountContracts");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool AccountRelated {
            get {
                return this.AccountRelatedField;
            }
            set {
                if ((this.AccountRelatedField.Equals(value) != true)) {
                    this.AccountRelatedField = value;
                    this.RaisePropertyChanged("AccountRelated");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Application {
            get {
                return this.ApplicationField;
            }
            set {
                if ((object.ReferenceEquals(this.ApplicationField, value) != true)) {
                    this.ApplicationField = value;
                    this.RaisePropertyChanged("Application");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int CompanyID {
            get {
                return this.CompanyIDField;
            }
            set {
                if ((this.CompanyIDField.Equals(value) != true)) {
                    this.CompanyIDField = value;
                    this.RaisePropertyChanged("CompanyID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int[] CompanyIDs {
            get {
                return this.CompanyIDsField;
            }
            set {
                if ((object.ReferenceEquals(this.CompanyIDsField, value) != true)) {
                    this.CompanyIDsField = value;
                    this.RaisePropertyChanged("CompanyIDs");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.Dictionary<string, string> Data {
            get {
                return this.DataField;
            }
            set {
                if ((object.ReferenceEquals(this.DataField, value) != true)) {
                    this.DataField = value;
                    this.RaisePropertyChanged("Data");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IgnoreViweObj {
            get {
                return this.IgnoreViweObjField;
            }
            set {
                if ((this.IgnoreViweObjField.Equals(value) != true)) {
                    this.IgnoreViweObjField = value;
                    this.RaisePropertyChanged("IgnoreViweObj");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ObjectID {
            get {
                return this.ObjectIDField;
            }
            set {
                if ((this.ObjectIDField.Equals(value) != true)) {
                    this.ObjectIDField = value;
                    this.RaisePropertyChanged("ObjectID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SecurityToken {
            get {
                return this.SecurityTokenField;
            }
            set {
                if ((object.ReferenceEquals(this.SecurityTokenField, value) != true)) {
                    this.SecurityTokenField = value;
                    this.RaisePropertyChanged("SecurityToken");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int TenantID {
            get {
                return this.TenantIDField;
            }
            set {
                if ((this.TenantIDField.Equals(value) != true)) {
                    this.TenantIDField = value;
                    this.RaisePropertyChanged("TenantID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int[] TenantIDs {
            get {
                return this.TenantIDsField;
            }
            set {
                if ((object.ReferenceEquals(this.TenantIDsField, value) != true)) {
                    this.TenantIDsField = value;
                    this.RaisePropertyChanged("TenantIDs");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime TokenExpireOn {
            get {
                return this.TokenExpireOnField;
            }
            set {
                if ((this.TokenExpireOnField.Equals(value) != true)) {
                    this.TokenExpireOnField = value;
                    this.RaisePropertyChanged("TokenExpireOn");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Type {
            get {
                return this.TypeField;
            }
            set {
                if ((object.ReferenceEquals(this.TypeField, value) != true)) {
                    this.TypeField = value;
                    this.RaisePropertyChanged("Type");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UserName {
            get {
                return this.UserNameField;
            }
            set {
                if ((object.ReferenceEquals(this.UserNameField, value) != true)) {
                    this.UserNameField = value;
                    this.RaisePropertyChanged("UserName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Write {
            get {
                return this.WriteField;
            }
            set {
                if ((this.WriteField.Equals(value) != true)) {
                    this.WriteField = value;
                    this.RaisePropertyChanged("Write");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal[] guUserGrpID {
            get {
                return this.guUserGrpIDField;
            }
            set {
                if ((object.ReferenceEquals(this.guUserGrpIDField, value) != true)) {
                    this.guUserGrpIDField = value;
                    this.RaisePropertyChanged("guUserGrpID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long guUserId {
            get {
                return this.guUserIdField;
            }
            set {
                if ((this.guUserIdField.Equals(value) != true)) {
                    this.guUserIdField = value;
                    this.RaisePropertyChanged("guUserId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int[] viweObjectIDs {
            get {
                return this.viweObjectIDsField;
            }
            set {
                if ((object.ReferenceEquals(this.viweObjectIDsField, value) != true)) {
                    this.viweObjectIDsField = value;
                    this.RaisePropertyChanged("viweObjectIDs");
                }
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="AccountContracts", Namespace="http://schemas.datacontract.org/2004/07/DuoAuthSvr")]
    [System.SerializableAttribute()]
    public partial class AccountContracts : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AccountNoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal GUAccountIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal GUPromotionIDField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AccountNo {
            get {
                return this.AccountNoField;
            }
            set {
                if ((object.ReferenceEquals(this.AccountNoField, value) != true)) {
                    this.AccountNoField = value;
                    this.RaisePropertyChanged("AccountNo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal GUAccountID {
            get {
                return this.GUAccountIDField;
            }
            set {
                if ((this.GUAccountIDField.Equals(value) != true)) {
                    this.GUAccountIDField = value;
                    this.RaisePropertyChanged("GUAccountID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal GUPromotionID {
            get {
                return this.GUPromotionIDField;
            }
            set {
                if ((this.GUPromotionIDField.Equals(value) != true)) {
                    this.GUPromotionIDField = value;
                    this.RaisePropertyChanged("GUPromotionID");
                }
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ViewObjects", Namespace="http://schemas.datacontract.org/2004/07/DuoAuthSvr")]
    [System.SerializableAttribute()]
    public partial class ViewObjects : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ObjectNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int parentIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int viweObjectIDField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ObjectName {
            get {
                return this.ObjectNameField;
            }
            set {
                if ((object.ReferenceEquals(this.ObjectNameField, value) != true)) {
                    this.ObjectNameField = value;
                    this.RaisePropertyChanged("ObjectName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int parentID {
            get {
                return this.parentIDField;
            }
            set {
                if ((this.parentIDField.Equals(value) != true)) {
                    this.parentIDField = value;
                    this.RaisePropertyChanged("parentID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int viweObjectID {
            get {
                return this.viweObjectIDField;
            }
            set {
                if ((this.viweObjectIDField.Equals(value) != true)) {
                    this.viweObjectIDField = value;
                    this.RaisePropertyChanged("viweObjectID");
                }
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="RefUserAuth.Iauth")]
    public interface Iauth {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/getMD5Hash", ReplyAction="http://tempuri.org/Iauth/getMD5HashResponse")]
        string getMD5Hash(string input);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/getMD5Hash", ReplyAction="http://tempuri.org/Iauth/getMD5HashResponse")]
        System.Threading.Tasks.Task<string> getMD5HashAsync(string input);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/ClearLocks", ReplyAction="http://tempuri.org/Iauth/ClearLocksResponse")]
        void ClearLocks(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/ClearLocks", ReplyAction="http://tempuri.org/Iauth/ClearLocksResponse")]
        System.Threading.Tasks.Task ClearLocksAsync(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/login", ReplyAction="http://tempuri.org/Iauth/loginResponse")]
        DuoSoftware.DuoSoftPhone.RefUserAuth.UserAuth login(string username, string password, int CompanyID, string Appliaction);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/login", ReplyAction="http://tempuri.org/Iauth/loginResponse")]
        System.Threading.Tasks.Task<DuoSoftware.DuoSoftPhone.RefUserAuth.UserAuth> loginAsync(string username, string password, int CompanyID, string Appliaction);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/Processlogin", ReplyAction="http://tempuri.org/Iauth/ProcessloginResponse")]
        DuoSoftware.DuoSoftPhone.RefUserAuth.UserAuth Processlogin(string SecurityToken, string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/Processlogin", ReplyAction="http://tempuri.org/Iauth/ProcessloginResponse")]
        System.Threading.Tasks.Task<DuoSoftware.DuoSoftPhone.RefUserAuth.UserAuth> ProcessloginAsync(string SecurityToken, string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/Applogin", ReplyAction="http://tempuri.org/Iauth/ApploginResponse")]
        DuoSoftware.DuoSoftPhone.RefUserAuth.UserAuth Applogin(string AppliactionKey, int CompanyID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/Applogin", ReplyAction="http://tempuri.org/Iauth/ApploginResponse")]
        System.Threading.Tasks.Task<DuoSoftware.DuoSoftPhone.RefUserAuth.UserAuth> ApploginAsync(string AppliactionKey, int CompanyID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/GetAccess", ReplyAction="http://tempuri.org/Iauth/GetAccessResponse")]
        DuoSoftware.DuoSoftPhone.RefUserAuth.UserAuth GetAccess(string SecurityToken, string AccessCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/GetAccess", ReplyAction="http://tempuri.org/Iauth/GetAccessResponse")]
        System.Threading.Tasks.Task<DuoSoftware.DuoSoftPhone.RefUserAuth.UserAuth> GetAccessAsync(string SecurityToken, string AccessCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/SetAccessCompany", ReplyAction="http://tempuri.org/Iauth/SetAccessCompanyResponse")]
        string SetAccessCompany(string SecurityToken, int Comapany);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/SetAccessCompany", ReplyAction="http://tempuri.org/Iauth/SetAccessCompanyResponse")]
        System.Threading.Tasks.Task<string> SetAccessCompanyAsync(string SecurityToken, int Comapany);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/SetWriteAccess", ReplyAction="http://tempuri.org/Iauth/SetWriteAccessResponse")]
        string SetWriteAccess(string SecurityToken, decimal[] Records);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/SetWriteAccess", ReplyAction="http://tempuri.org/Iauth/SetWriteAccessResponse")]
        System.Threading.Tasks.Task<string> SetWriteAccessAsync(string SecurityToken, decimal[] Records);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/ProcessloginForWrite", ReplyAction="http://tempuri.org/Iauth/ProcessloginForWriteResponse")]
        DuoSoftware.DuoSoftPhone.RefUserAuth.UserAuth ProcessloginForWrite(string SecurityToken, string username, string password, decimal[] Records);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/ProcessloginForWrite", ReplyAction="http://tempuri.org/Iauth/ProcessloginForWriteResponse")]
        System.Threading.Tasks.Task<DuoSoftware.DuoSoftPhone.RefUserAuth.UserAuth> ProcessloginForWriteAsync(string SecurityToken, string username, string password, decimal[] Records);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/GetViewObjectList", ReplyAction="http://tempuri.org/Iauth/GetViewObjectListResponse")]
        DuoSoftware.DuoSoftPhone.RefUserAuth.ViewObjects[] GetViewObjectList(int CompanyID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/GetViewObjectList", ReplyAction="http://tempuri.org/Iauth/GetViewObjectListResponse")]
        System.Threading.Tasks.Task<DuoSoftware.DuoSoftPhone.RefUserAuth.ViewObjects[]> GetViewObjectListAsync(int CompanyID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/getAccountContracts", ReplyAction="http://tempuri.org/Iauth/getAccountContractsResponse")]
        DuoSoftware.DuoSoftPhone.RefUserAuth.AccountContracts[] getAccountContracts(string SecurityToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/getAccountContracts", ReplyAction="http://tempuri.org/Iauth/getAccountContractsResponse")]
        System.Threading.Tasks.Task<DuoSoftware.DuoSoftPhone.RefUserAuth.AccountContracts[]> getAccountContractsAsync(string SecurityToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/LogOut", ReplyAction="http://tempuri.org/Iauth/LogOutResponse")]
        void LogOut(string SecurityToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/LogOut", ReplyAction="http://tempuri.org/Iauth/LogOutResponse")]
        System.Threading.Tasks.Task LogOutAsync(string SecurityToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/SaveLicense", ReplyAction="http://tempuri.org/Iauth/SaveLicenseResponse")]
        void SaveLicense(System.IO.Stream file);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/SaveLicense", ReplyAction="http://tempuri.org/Iauth/SaveLicenseResponse")]
        System.Threading.Tasks.Task SaveLicenseAsync(System.IO.Stream file);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/ValidateLicense", ReplyAction="http://tempuri.org/Iauth/ValidateLicenseResponse")]
        bool ValidateLicense();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/ValidateLicense", ReplyAction="http://tempuri.org/Iauth/ValidateLicenseResponse")]
        System.Threading.Tasks.Task<bool> ValidateLicenseAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/GetAccessCompany", ReplyAction="http://tempuri.org/Iauth/GetAccessCompanyResponse")]
        int[] GetAccessCompany(string SecurityToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/GetAccessCompany", ReplyAction="http://tempuri.org/Iauth/GetAccessCompanyResponse")]
        System.Threading.Tasks.Task<int[]> GetAccessCompanyAsync(string SecurityToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/Lock", ReplyAction="http://tempuri.org/Iauth/LockResponse")]
        bool Lock(string SecurityToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/Lock", ReplyAction="http://tempuri.org/Iauth/LockResponse")]
        System.Threading.Tasks.Task<bool> LockAsync(string SecurityToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/Release", ReplyAction="http://tempuri.org/Iauth/ReleaseResponse")]
        bool Release(string SecurityToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Iauth/Release", ReplyAction="http://tempuri.org/Iauth/ReleaseResponse")]
        System.Threading.Tasks.Task<bool> ReleaseAsync(string SecurityToken);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IauthChannel : DuoSoftware.DuoSoftPhone.RefUserAuth.Iauth, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class IauthClient : System.ServiceModel.ClientBase<DuoSoftware.DuoSoftPhone.RefUserAuth.Iauth>, DuoSoftware.DuoSoftPhone.RefUserAuth.Iauth {
        
        public IauthClient() {
        }
        
        public IauthClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public IauthClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IauthClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IauthClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string getMD5Hash(string input) {
            return base.Channel.getMD5Hash(input);
        }
        
        public System.Threading.Tasks.Task<string> getMD5HashAsync(string input) {
            return base.Channel.getMD5HashAsync(input);
        }
        
        public void ClearLocks(string username, string password) {
            base.Channel.ClearLocks(username, password);
        }
        
        public System.Threading.Tasks.Task ClearLocksAsync(string username, string password) {
            return base.Channel.ClearLocksAsync(username, password);
        }
        
        public DuoSoftware.DuoSoftPhone.RefUserAuth.UserAuth login(string username, string password, int CompanyID, string Appliaction) {
            return base.Channel.login(username, password, CompanyID, Appliaction);
        }
        
        public System.Threading.Tasks.Task<DuoSoftware.DuoSoftPhone.RefUserAuth.UserAuth> loginAsync(string username, string password, int CompanyID, string Appliaction) {
            return base.Channel.loginAsync(username, password, CompanyID, Appliaction);
        }
        
        public DuoSoftware.DuoSoftPhone.RefUserAuth.UserAuth Processlogin(string SecurityToken, string username, string password) {
            return base.Channel.Processlogin(SecurityToken, username, password);
        }
        
        public System.Threading.Tasks.Task<DuoSoftware.DuoSoftPhone.RefUserAuth.UserAuth> ProcessloginAsync(string SecurityToken, string username, string password) {
            return base.Channel.ProcessloginAsync(SecurityToken, username, password);
        }
        
        public DuoSoftware.DuoSoftPhone.RefUserAuth.UserAuth Applogin(string AppliactionKey, int CompanyID) {
            return base.Channel.Applogin(AppliactionKey, CompanyID);
        }
        
        public System.Threading.Tasks.Task<DuoSoftware.DuoSoftPhone.RefUserAuth.UserAuth> ApploginAsync(string AppliactionKey, int CompanyID) {
            return base.Channel.ApploginAsync(AppliactionKey, CompanyID);
        }
        
        public DuoSoftware.DuoSoftPhone.RefUserAuth.UserAuth GetAccess(string SecurityToken, string AccessCode) {
            return base.Channel.GetAccess(SecurityToken, AccessCode);
        }
        
        public System.Threading.Tasks.Task<DuoSoftware.DuoSoftPhone.RefUserAuth.UserAuth> GetAccessAsync(string SecurityToken, string AccessCode) {
            return base.Channel.GetAccessAsync(SecurityToken, AccessCode);
        }
        
        public string SetAccessCompany(string SecurityToken, int Comapany) {
            return base.Channel.SetAccessCompany(SecurityToken, Comapany);
        }
        
        public System.Threading.Tasks.Task<string> SetAccessCompanyAsync(string SecurityToken, int Comapany) {
            return base.Channel.SetAccessCompanyAsync(SecurityToken, Comapany);
        }
        
        public string SetWriteAccess(string SecurityToken, decimal[] Records) {
            return base.Channel.SetWriteAccess(SecurityToken, Records);
        }
        
        public System.Threading.Tasks.Task<string> SetWriteAccessAsync(string SecurityToken, decimal[] Records) {
            return base.Channel.SetWriteAccessAsync(SecurityToken, Records);
        }
        
        public DuoSoftware.DuoSoftPhone.RefUserAuth.UserAuth ProcessloginForWrite(string SecurityToken, string username, string password, decimal[] Records) {
            return base.Channel.ProcessloginForWrite(SecurityToken, username, password, Records);
        }
        
        public System.Threading.Tasks.Task<DuoSoftware.DuoSoftPhone.RefUserAuth.UserAuth> ProcessloginForWriteAsync(string SecurityToken, string username, string password, decimal[] Records) {
            return base.Channel.ProcessloginForWriteAsync(SecurityToken, username, password, Records);
        }
        
        public DuoSoftware.DuoSoftPhone.RefUserAuth.ViewObjects[] GetViewObjectList(int CompanyID) {
            return base.Channel.GetViewObjectList(CompanyID);
        }
        
        public System.Threading.Tasks.Task<DuoSoftware.DuoSoftPhone.RefUserAuth.ViewObjects[]> GetViewObjectListAsync(int CompanyID) {
            return base.Channel.GetViewObjectListAsync(CompanyID);
        }
        
        public DuoSoftware.DuoSoftPhone.RefUserAuth.AccountContracts[] getAccountContracts(string SecurityToken) {
            return base.Channel.getAccountContracts(SecurityToken);
        }
        
        public System.Threading.Tasks.Task<DuoSoftware.DuoSoftPhone.RefUserAuth.AccountContracts[]> getAccountContractsAsync(string SecurityToken) {
            return base.Channel.getAccountContractsAsync(SecurityToken);
        }
        
        public void LogOut(string SecurityToken) {
            base.Channel.LogOut(SecurityToken);
        }
        
        public System.Threading.Tasks.Task LogOutAsync(string SecurityToken) {
            return base.Channel.LogOutAsync(SecurityToken);
        }
        
        public void SaveLicense(System.IO.Stream file) {
            base.Channel.SaveLicense(file);
        }
        
        public System.Threading.Tasks.Task SaveLicenseAsync(System.IO.Stream file) {
            return base.Channel.SaveLicenseAsync(file);
        }
        
        public bool ValidateLicense() {
            return base.Channel.ValidateLicense();
        }
        
        public System.Threading.Tasks.Task<bool> ValidateLicenseAsync() {
            return base.Channel.ValidateLicenseAsync();
        }
        
        public int[] GetAccessCompany(string SecurityToken) {
            return base.Channel.GetAccessCompany(SecurityToken);
        }
        
        public System.Threading.Tasks.Task<int[]> GetAccessCompanyAsync(string SecurityToken) {
            return base.Channel.GetAccessCompanyAsync(SecurityToken);
        }
        
        public bool Lock(string SecurityToken) {
            return base.Channel.Lock(SecurityToken);
        }
        
        public System.Threading.Tasks.Task<bool> LockAsync(string SecurityToken) {
            return base.Channel.LockAsync(SecurityToken);
        }
        
        public bool Release(string SecurityToken) {
            return base.Channel.Release(SecurityToken);
        }
        
        public System.Threading.Tasks.Task<bool> ReleaseAsync(string SecurityToken) {
            return base.Channel.ReleaseAsync(SecurityToken);
        }
    }
}
