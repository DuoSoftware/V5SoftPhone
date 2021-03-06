﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DuoSoftware.DuoSoftPhone.RefResourceChecker {
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
        private DuoSoftware.DuoSoftPhone.RefResourceChecker.UiModes CurrentModeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private DuoSoftware.DuoSoftPhone.RefResourceChecker.UiStatus CurrentStateField;
        
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
        public DuoSoftware.DuoSoftPhone.RefResourceChecker.UiModes CurrentMode {
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
        public DuoSoftware.DuoSoftPhone.RefResourceChecker.UiStatus CurrentState {
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="RefResourceChecker.IResourceStatusChecker")]
    public interface IResourceStatusChecker {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IResourceStatusChecker/CheckCurrentState", ReplyAction="http://tempuri.org/IResourceStatusChecker/CheckCurrentStateResponse")]
        DuoSoftware.DuoSoftPhone.RefResourceChecker.StatusReplyInfo CheckCurrentState(string securityToken, DuoSoftware.DuoSoftPhone.RefResourceChecker.UiStatus currentStatus, DuoSoftware.DuoSoftPhone.RefResourceChecker.UiModes currentMode, int sequenceNumber);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IResourceStatusChecker/CheckCurrentState", ReplyAction="http://tempuri.org/IResourceStatusChecker/CheckCurrentStateResponse")]
        System.IAsyncResult BeginCheckCurrentState(string securityToken, DuoSoftware.DuoSoftPhone.RefResourceChecker.UiStatus currentStatus, DuoSoftware.DuoSoftPhone.RefResourceChecker.UiModes currentMode, int sequenceNumber, System.AsyncCallback callback, object asyncState);
        
        DuoSoftware.DuoSoftPhone.RefResourceChecker.StatusReplyInfo EndCheckCurrentState(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IResourceStatusCheckerChannel : DuoSoftware.DuoSoftPhone.RefResourceChecker.IResourceStatusChecker, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CheckCurrentStateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public CheckCurrentStateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public DuoSoftware.DuoSoftPhone.RefResourceChecker.StatusReplyInfo Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((DuoSoftware.DuoSoftPhone.RefResourceChecker.StatusReplyInfo)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ResourceStatusCheckerClient : System.ServiceModel.ClientBase<DuoSoftware.DuoSoftPhone.RefResourceChecker.IResourceStatusChecker>, DuoSoftware.DuoSoftPhone.RefResourceChecker.IResourceStatusChecker {
        
        private BeginOperationDelegate onBeginCheckCurrentStateDelegate;
        
        private EndOperationDelegate onEndCheckCurrentStateDelegate;
        
        private System.Threading.SendOrPostCallback onCheckCurrentStateCompletedDelegate;
        
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
        
        public event System.EventHandler<CheckCurrentStateCompletedEventArgs> CheckCurrentStateCompleted;
        
        public DuoSoftware.DuoSoftPhone.RefResourceChecker.StatusReplyInfo CheckCurrentState(string securityToken, DuoSoftware.DuoSoftPhone.RefResourceChecker.UiStatus currentStatus, DuoSoftware.DuoSoftPhone.RefResourceChecker.UiModes currentMode, int sequenceNumber) {
            return base.Channel.CheckCurrentState(securityToken, currentStatus, currentMode, sequenceNumber);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginCheckCurrentState(string securityToken, DuoSoftware.DuoSoftPhone.RefResourceChecker.UiStatus currentStatus, DuoSoftware.DuoSoftPhone.RefResourceChecker.UiModes currentMode, int sequenceNumber, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginCheckCurrentState(securityToken, currentStatus, currentMode, sequenceNumber, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public DuoSoftware.DuoSoftPhone.RefResourceChecker.StatusReplyInfo EndCheckCurrentState(System.IAsyncResult result) {
            return base.Channel.EndCheckCurrentState(result);
        }
        
        private System.IAsyncResult OnBeginCheckCurrentState(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string securityToken = ((string)(inValues[0]));
            DuoSoftware.DuoSoftPhone.RefResourceChecker.UiStatus currentStatus = ((DuoSoftware.DuoSoftPhone.RefResourceChecker.UiStatus)(inValues[1]));
            DuoSoftware.DuoSoftPhone.RefResourceChecker.UiModes currentMode = ((DuoSoftware.DuoSoftPhone.RefResourceChecker.UiModes)(inValues[2]));
            int sequenceNumber = ((int)(inValues[3]));
            return this.BeginCheckCurrentState(securityToken, currentStatus, currentMode, sequenceNumber, callback, asyncState);
        }
        
        private object[] OnEndCheckCurrentState(System.IAsyncResult result) {
            DuoSoftware.DuoSoftPhone.RefResourceChecker.StatusReplyInfo retVal = this.EndCheckCurrentState(result);
            return new object[] {
                    retVal};
        }
        
        private void OnCheckCurrentStateCompleted(object state) {
            if ((this.CheckCurrentStateCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CheckCurrentStateCompleted(this, new CheckCurrentStateCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CheckCurrentStateAsync(string securityToken, DuoSoftware.DuoSoftPhone.RefResourceChecker.UiStatus currentStatus, DuoSoftware.DuoSoftPhone.RefResourceChecker.UiModes currentMode, int sequenceNumber) {
            this.CheckCurrentStateAsync(securityToken, currentStatus, currentMode, sequenceNumber, null);
        }
        
        public void CheckCurrentStateAsync(string securityToken, DuoSoftware.DuoSoftPhone.RefResourceChecker.UiStatus currentStatus, DuoSoftware.DuoSoftPhone.RefResourceChecker.UiModes currentMode, int sequenceNumber, object userState) {
            if ((this.onBeginCheckCurrentStateDelegate == null)) {
                this.onBeginCheckCurrentStateDelegate = new BeginOperationDelegate(this.OnBeginCheckCurrentState);
            }
            if ((this.onEndCheckCurrentStateDelegate == null)) {
                this.onEndCheckCurrentStateDelegate = new EndOperationDelegate(this.OnEndCheckCurrentState);
            }
            if ((this.onCheckCurrentStateCompletedDelegate == null)) {
                this.onCheckCurrentStateCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCheckCurrentStateCompleted);
            }
            base.InvokeAsync(this.onBeginCheckCurrentStateDelegate, new object[] {
                        securityToken,
                        currentStatus,
                        currentMode,
                        sequenceNumber}, this.onEndCheckCurrentStateDelegate, this.onCheckCurrentStateCompletedDelegate, userState);
        }
    }
}
