﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DuoSoftware.DuoSoftPhone.refResourceChecker {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="UiStatus", Namespace="http://schemas.datacontract.org/2004/07/DuoSoftware.DC.ResourceMonitoringService." +
        "Enums")]
    public enum UiStatus : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Initializing = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Idle = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Busy = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Acw = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Break = 4,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="UiModes", Namespace="http://schemas.datacontract.org/2004/07/DuoSoftware.DC.ResourceMonitoringService." +
        "Enums")]
    public enum UiModes : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Inbound = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Outbound = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Online = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Offline = 3,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StatusReply.Info", Namespace="http://schemas.datacontract.org/2004/07/DuoSoftware.DC.ResourceMonitoringService." +
        "Classes")]
    [System.SerializableAttribute()]
    public partial struct StatusReplyInfo : System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private DuoSoftware.DuoSoftPhone.refResourceChecker.UiModes CurrentModeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private DuoSoftware.DuoSoftPhone.refResourceChecker.UiStatus CurrentStateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsMatchingField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public DuoSoftware.DuoSoftPhone.refResourceChecker.UiModes CurrentMode {
            get {
                return this.CurrentModeField;
            }
            set {
                if ((this.CurrentModeField.Equals(value) != true)) {
                    this.CurrentModeField = value;
                    this.RaisePropertyChanged("CurrentMode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public DuoSoftware.DuoSoftPhone.refResourceChecker.UiStatus CurrentState {
            get {
                return this.CurrentStateField;
            }
            set {
                if ((this.CurrentStateField.Equals(value) != true)) {
                    this.CurrentStateField = value;
                    this.RaisePropertyChanged("CurrentState");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsMatching {
            get {
                return this.IsMatchingField;
            }
            set {
                if ((this.IsMatchingField.Equals(value) != true)) {
                    this.IsMatchingField = value;
                    this.RaisePropertyChanged("IsMatching");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="refResourceChecker.IResourceStatusChecker")]
    public interface IResourceStatusChecker {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IResourceStatusChecker/CheckCurrentState", ReplyAction="http://tempuri.org/IResourceStatusChecker/CheckCurrentStateResponse")]
        DuoSoftware.DuoSoftPhone.refResourceChecker.StatusReplyInfo CheckCurrentState(string securityToken, DuoSoftware.DuoSoftPhone.refResourceChecker.UiStatus currentStatus, DuoSoftware.DuoSoftPhone.refResourceChecker.UiModes currentMode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IResourceStatusChecker/CheckCurrentState", ReplyAction="http://tempuri.org/IResourceStatusChecker/CheckCurrentStateResponse")]
        System.Threading.Tasks.Task<DuoSoftware.DuoSoftPhone.refResourceChecker.StatusReplyInfo> CheckCurrentStateAsync(string securityToken, DuoSoftware.DuoSoftPhone.refResourceChecker.UiStatus currentStatus, DuoSoftware.DuoSoftPhone.refResourceChecker.UiModes currentMode);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IResourceStatusCheckerChannel : DuoSoftware.DuoSoftPhone.refResourceChecker.IResourceStatusChecker, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ResourceStatusCheckerClient : System.ServiceModel.ClientBase<DuoSoftware.DuoSoftPhone.refResourceChecker.IResourceStatusChecker>, DuoSoftware.DuoSoftPhone.refResourceChecker.IResourceStatusChecker {
        
        public ResourceStatusCheckerClient() {
        }
        
        public ResourceStatusCheckerClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ResourceStatusCheckerClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ResourceStatusCheckerClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ResourceStatusCheckerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public DuoSoftware.DuoSoftPhone.refResourceChecker.StatusReplyInfo CheckCurrentState(string securityToken, DuoSoftware.DuoSoftPhone.refResourceChecker.UiStatus currentStatus, DuoSoftware.DuoSoftPhone.refResourceChecker.UiModes currentMode) {
            return base.Channel.CheckCurrentState(securityToken, currentStatus, currentMode);
        }
        
        public System.Threading.Tasks.Task<DuoSoftware.DuoSoftPhone.refResourceChecker.StatusReplyInfo> CheckCurrentStateAsync(string securityToken, DuoSoftware.DuoSoftPhone.refResourceChecker.UiStatus currentStatus, DuoSoftware.DuoSoftPhone.refResourceChecker.UiModes currentMode) {
            return base.Channel.CheckCurrentStateAsync(securityToken, currentStatus, currentMode);
        }
    }
}