﻿using System;
using DuoSoftware.DuoLogger;
using DuoSoftware.DuoSoftPhone.Controllers.AgentStatus;
using PortSIP;

namespace DuoSoftware.DuoSoftPhone.Controllers.CallStatus
{
    public class CallRingingState : CallState
    {
        public override void OnAnswer(ref Call call)
        {
            call.CallCurrentState = new CallConnectedState();
        }

        public override void OnHold(ref Call call, CallActions callAction)
        {
            try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnUnHold(ref Call call, CallActions callAction)
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

        public override void OnDisconnected(ref Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
            
        }

        public override void OnTransferReq(ref Call call, CallActions callAction)
        {
            try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnTranferFail(ref Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnOperationFail(ref Call call)
        {
             try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnSwapReq(ref Call call, CallActions callAction)
        {
            try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnCallReject(ref Call call)
        {
            call.CallCurrentState = new CallDisconnectedState();
        }


        public override void OnEndLinkLine(ref Call call, PortSIPLib sipHandler)
        {
            try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnMakeCall(ref Call call, PortSIPLib sipHandler, string number)
        {
            try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnRinging(ref Call call, int callbackIndex, int callbackObject, int sessionId, string statusText, int statusCode)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnIncoming(ref Call call, int callbackIndex, int callbackObject, int sessionId, string callerDisplayName,
            string caller, string calleeDisplayName, string callee, string audioCodecNames, string videoCodecNames,
            bool existsAudio, bool existsVideo)
        {
            try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnTimeout(ref Call call, ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnEndCallSession(ref Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnCallConference(ref Call call, PortSIPLib sipHandler)
        {
            try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnCallConferenceFail(ref Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnSetStatus(ref Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnSendDTMF(ref Call call, PortSIPLib sipHandler, int val)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnSessinCreate(ref Call call, string sessionId)
        {
             try { throw new NotImplementedException("Invalid Call Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }
    }
}