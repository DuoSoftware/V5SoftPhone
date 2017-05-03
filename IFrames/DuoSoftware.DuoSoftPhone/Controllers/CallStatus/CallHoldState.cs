using DuoSoftware.DuoSoftPhone.Controllers.AgentStatus;
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoTools.DuoLogger;
using System;

namespace DuoSoftware.DuoSoftPhone.Controllers.CallStatus
{
    /// <summary>
    ///
    /// </summary>
    public class CallHoldState : CallState
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="callAction"></param>
        public CallHoldState(CallAction callAction)
        {
            CallAction = callAction;
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnAnswer(Call call)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallHoldState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="callAction"></param>
        public override void OnHold(Call call, CallAction callAction)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallHoldState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="callAction"></param>
        public override void OnUnHold(Call call, CallAction callAction)
        {
            CallAction = callAction;
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnNoAnswer(Call call)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallHoldState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnReset(Call call)
        {
            if (call != null) call.CallCurrentState = new CallConnectedState();
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnDisconnected(Call call)
        {
            try {
                if (call != null) call.CallCurrentState = new CallDisconnectedState();
            }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallHoldState", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="callAction"></param>
        public override void OnTransferReq(Call call, CallAction callAction)
        {
            CallAction = callAction;
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnTranferFail(Call call)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallHoldState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnOperationFail(Call call)
        {
            try
            {
                if (call == null) return;
                switch (call.CallCurrentState.CallAction)
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
                        break;

                    case Common.CallAction.UnHoldRequested:
                    case Common.CallAction.UnHoldInProgress:
                    case Common.CallAction.UnHold:
                    case Common.CallAction.CallTransferRequested:
                    case Common.CallAction.CallTransferInProgress:
                    case Common.CallAction.CallTransferred:
                        call.CallCurrentState = new CallHoldState(Common.CallAction.Hold);
                        break;

                    case Common.CallAction.CallSwapRequested:
                        break;

                    case Common.CallAction.CallSwapInProgress:
                        break;

                    case Common.CallAction.CallSwap:
                        break;

                    case Common.CallAction.ConferenceCallRequested:
                    case Common.CallAction.ConferenceCallInProgress:
                    case Common.CallAction.ConferenceCall:
                        call.CallCurrentState = new CallAgentClintConnectedState();
                        break;

                    case Common.CallAction.EtlRequested:
                    case Common.CallAction.EtlInProgress:
                    case Common.CallAction.EtlCall:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "OnOperationFail", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="callAction"></param>
        public override void OnSwapReq(Call call, CallAction callAction)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallHoldState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnReject(Call call)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallHoldState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="callAction"></param>
        public override void OnEndLinkLine(Call call, CallAction callAction)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallHoldState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnMake(Call call)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallHoldState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
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
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallHoldState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
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
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallHoldState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="agent"></param>
        public override void OnTimeout(Call call, Agent agent)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallHoldState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnEndSession(Call call)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallHoldState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnConference(Call call)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallHoldState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnConferenceFail(Call call)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallHoldState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public override void OnSetStatus(Call call)
        {
            try
            {
                switch (CallAction)
                {
                    case CallAction.Hold:
                    case CallAction.HoldInProgress:
                    case CallAction.HoldRequested:
                        if (call != null) call.CallCurrentState = new CallHoldState(CallAction);
                        break;

                    case CallAction.UnHoldRequested:
                    case CallAction.UnHoldInProgress:
                    case CallAction.UnHold:
                        if (call != null) call.CallCurrentState = new CallConnectedState();
                        break;

                    case CallAction.CallTransferRequested:
                    case CallAction.CallTransferInProgress:
                    case CallAction.CallTransferred:
                        if (call != null) call.CallCurrentState = new CallAgentSupConnectedState();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "OnSetStatus", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="val"></param>
        public override void OnSendDTMF(Call call, int val)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallHoldState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="sessionId"></param>
        public override void OnSessinCreate(Call call, string sessionId)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "CallHoldState", new NotImplementedException("Invalid Call Status."), Logger.LogLevel.Error);
        }
    }
}