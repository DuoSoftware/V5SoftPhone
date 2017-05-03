using DuoSoftware.DuoSoftPhone.Controllers.AgentStatus;
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoTools.DuoLogger;
using System;

namespace DuoSoftware.DuoSoftPhone.Controllers.CallStatus
{
    /// <summary>
    ///
    /// </summary>
    public class CallConnectedState : CallState
    {
        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnAnswer(Call call)
        {
            try
            {
                if (call != null) call.CallCurrentState = new CallConnectedState();
            }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallConnectedState", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="callAction"></param>
        public override void OnHold(Call call, CallAction callAction)
        {
            switch (callAction)
            {
                case Common.CallAction.Error:
                    break;

                case Common.CallAction.IncommingCallRequest:
                    break;

                case Common.CallAction.CallInProgress:
                    break;

                case Common.CallAction.Call:
                    break;

                case Common.CallAction.HoldRequested:
                case Common.CallAction.HoldInProgress:
                case Common.CallAction.Hold:
                    if (call != null) call.CallCurrentState = new CallHoldState(callAction);
                    break;

                case Common.CallAction.UnHoldRequested:
                    break;

                case Common.CallAction.UnHoldInProgress:
                    break;

                case Common.CallAction.UnHold:
                    break;

                case Common.CallAction.CallTransferRequested:
                    break;

                case Common.CallAction.CallTransferInProgress:
                    break;

                case Common.CallAction.CallTransferred:
                    break;

                case Common.CallAction.CallSwapRequested:
                    break;

                case Common.CallAction.CallSwapInProgress:
                    break;

                case Common.CallAction.CallSwap:
                    break;

                case Common.CallAction.ConferenceCallRequested:
                    break;

                case CallAction.ConferenceCallInProgress:
                    break;

                case CallAction.ConferenceCall:
                    break;

                case CallAction.EtlRequested:
                    break;

                case CallAction.EtlInProgress:
                    break;

                case CallAction.EtlCall:
                    break;

                default:
                    throw new ArgumentOutOfRangeException("callAction");
            }
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="callAction"></param>
        public override void OnUnHold(Call call, CallAction callAction)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallConnectedState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnNoAnswer(Call call)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallConnectedState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnReset(Call call)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallConnectedState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnDisconnected(Call call)
        {
            if (call != null) call.CallCurrentState = new CallDisconnectedState();
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="callAction"></param>
        public override void OnTransferReq(Call call, CallAction callAction)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallConnectedState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnTranferFail(Call call)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallConnectedState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnOperationFail(Call call)
        {
            if (call != null) call.CallCurrentState = new CallConnectedState();
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="callAction"></param>
        public override void OnSwapReq(Call call, CallAction callAction)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallConnectedState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnReject(Call call)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallConnectedState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="callAction"></param>
        public override void OnEndLinkLine(Call call, CallAction callAction)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallConnectedState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnMake(Call call)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallConnectedState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="callbackObject"></param>
        /// <param name="sessionId"></param>
        /// <param name="statusText"></param>
        /// <param name="statusCode"></param>
        public override void OnRinging(Call call, int callbackObject, int sessionId, string statusText, int statusCode)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallConnectedState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="callbackObject"></param>
        /// <param name="sessionId"></param>
        /// <param name="caller"></param>
        /// <param name="callee"></param>
        public override void OnIncoming(Call call, int callbackObject, int sessionId, string caller, string callee)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallConnectedState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="agent"></param>
        public override void OnTimeout(Call call, Agent agent)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallConnectedState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnEndSession(Call call)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallConnectedState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnConference(Call call)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallConnectedState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnConferenceFail(Call call)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallConnectedState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnSetStatus(Call call)
        {
            if (call != null) call.CallCurrentState = new CallHoldState(CallAction.Hold);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="val"></param>
        public override void OnSendDTMF(Call call, int val)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallConnectedState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="sessionId"></param>
        public override void OnSessinCreate(Call call, string sessionId)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallConnectedState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }
    }
}