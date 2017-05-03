using System;
using DuoSoftware.DuoLogger;
using DuoSoftware.DuoSoftPhone.Controllers.AgentStatus;
using DuoSoftware.DuoSoftPhone.Controllers.PortSIP;
using DuoSoftware.DuoSoftPhone.Ui;

namespace DuoSoftware.DuoSoftPhone.Controllers.CallStatus
{
    public class CallRingingState : CallState
    {
        public override void OnAnswer(ref Call call, PortsipHandler sipHandler)
        {
            call.CurrentState = new CallConnectedState();
            sipHandler.AnswerCall(call.PortSipSessionId);
            
        }

        public override void OnHold(ref Call call, PortsipHandler sipHandler)
        {
            try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnUnHold(ref Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnNoAnswer(ref Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnReset(ref Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnDisconnected(ref Call call, PortsipHandler sipHandler)
        {
            sipHandler.RejectCall(call.PortSipSessionId);
            call.CurrentState = new CallIdleState();
            
        }

        public override void OnTransferReq(ref Call call, string transferNo, PortsipHandler sipHandler)
        {
            try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnTranferFail(ref Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnSwapReq(ref Call call, PortsipHandler sipHandler)
        {
            try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnCallReject(ref Call call, PortsipHandler sipHandler)
        {
            sipHandler.RejectCall(call.PortSipSessionId);
            call.CurrentState = new CallIdleState();
            
        }


        public override void OnEndLinkLine(ref Call call, PortsipHandler sipHandler)
        {
            try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnMakeCall(ref Call call, PortsipHandler sipHandler, string number)
        {
            try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnRinging(ref Call call, int sessionId, string caller, string callerDisplayName, string callee, string calleeDisplayName)
        {
            call.CurrentState = new CallRingingState();
        }

        public override void OnTimeout(ref Call call, ref Agent agent)
        {
            call.CurrentState = new CallIdleState();
            agent.AgentCurrentState = new AgentIdle();
        }

        public override void OnEndCallSession(ref Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnCallConference(ref Call call, PortsipHandler sipHandler)
        {
            try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnCallConferenceFail(ref Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnResetStatus(ref Call call)
        {
            call.CurrentState = new CallIdleState();
            
        }

        public override void OnSendDTMF(ref Call call, PortsipHandler sipHandler, int val)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }
    }
}