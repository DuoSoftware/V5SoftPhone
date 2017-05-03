using DuoSoftware.DuoSoftPhone.Controllers.AgentStatus;
using DuoSoftware.DuoSoftPhone.Controllers.Common;

namespace DuoSoftware.DuoSoftPhone.Controllers.CallStatus
{
    /// <summary>
    ///
    /// </summary>
    public abstract class CallState
    {
        /// <summary>
        ///
        /// </summary>
        public CallAction CallAction { get; set; }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public abstract void OnAnswer(Call call);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="callAction"></param>
        public abstract void OnHold(Call call, CallAction callAction);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="callAction"></param>
        public abstract void OnUnHold(Call call, CallAction callAction);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public abstract void OnNoAnswer(Call call);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public abstract void OnReset(Call call);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public abstract void OnDisconnected(Call call);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="callAction"></param>
        public abstract void OnTransferReq(Call call, CallAction callAction);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public abstract void OnTranferFail(Call call);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public abstract void OnOperationFail( Call call);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="callAction"></param>
        public abstract void OnSwapReq( Call call, CallAction callAction);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public abstract void OnReject(Call call);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="callAction"></param>
        public abstract void OnEndLinkLine( Call call, CallAction callAction);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public abstract void OnMake(Call call);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="callbackObject"></param>
        /// <param name="sessionId"></param>
        /// <param name="statusText"></param>
        /// <param name="statusCode"></param>
        public abstract void OnRinging( Call call, int callbackObject, int sessionId, string statusText, int statusCode);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="callbackObject"></param>
        /// <param name="sessionId"></param>
        /// <param name="caller"></param>
        /// <param name="callee"></param>
        public abstract void OnIncoming( Call call, int callbackObject, int sessionId, string caller, string callee);

        public abstract void OnTimeout( Call call, Agent agent);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public abstract void OnEndSession(Call call);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public abstract void OnConference(Call call);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public abstract void OnConferenceFail(Call call);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        public abstract void OnSetStatus(Call call);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="val"></param>
        public abstract void OnSendDTMF(Call call, int val);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="call"></param>
        /// <param name="sessionId"></param>
        public abstract void OnSessinCreate( Call call, string sessionId);
    }
}