using DuoSoftware.DuoSoftPhone.Controllers.AgentStatus;
using DuoSoftware.DuoSoftPhone.Controllers.PortSIP;
using DuoSoftware.DuoSoftPhone.Ui;

namespace DuoSoftware.DuoSoftPhone.Controllers.CallStatus
{
    public abstract class CallState
    {
        public abstract void OnAnswer(ref Call call,PortsipHandler sipHandler);
        public abstract void OnHold(ref Call call,PortsipHandler sipHandler);
        public abstract void OnUnHold(ref Call call);
        public abstract void OnNoAnswer(ref Call call);
        public abstract void OnReset(ref Call call);
        public abstract void OnDisconnected(ref Call call, PortsipHandler sipHandler);
        public abstract void OnTransferReq(ref Call call,string transferNo,PortsipHandler sipHandler);
        public abstract void OnTranferFail(ref Call call);
        public abstract void OnSwapReq(ref Call call, PortsipHandler sipHandler);
        public abstract void OnCallReject(ref Call call, PortsipHandler sipHandler);
        public abstract void OnEndLinkLine(ref Call call,PortsipHandler sipHandler);
        public abstract void OnMakeCall(ref Call call,PortsipHandler sipHandler,string number);
        public abstract void OnRinging(ref Call call,int sessionId,string caller,string callerDisplayName,string callee,string calleeDisplayName);
        public abstract void OnTimeout(ref Call call,ref Agent agent);
        public abstract void OnEndCallSession(ref Call call);
        public abstract void OnCallConference(ref Call call, PortsipHandler sipHandler);
        public abstract void OnCallConferenceFail(ref Call call);
        public abstract void OnResetStatus(ref Call call);
        public abstract void OnSendDTMF(ref Call call, PortsipHandler sipHandler,int val);
    }
}