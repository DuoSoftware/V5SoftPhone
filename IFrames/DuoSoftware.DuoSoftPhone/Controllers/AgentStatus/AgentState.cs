using System;

namespace DuoSoftware.DuoSoftPhone.Controllers.AgentStatus
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public enum CallDirection
    {
        Incoming = 0,
        Outgoing = 1,
    }

    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public enum AgentMode
    {
        Offline = 0,
        Inbound = 1,
        Outbound = 2,
    }

    /// <summary>
    ///
    /// </summary>
    
    public abstract class AgentEvent
    {
        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        public abstract void OnResetStatus(Agent agent);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        public abstract void OnTimeOut(Agent agent);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        /// <param name="caller"></param>
        /// <param name="sessionId"></param>
        public abstract void OnIncomingCall(Agent agent, string caller, int sessionId);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        public abstract void OnMakeCall(Agent agent);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        /// <param name="callSessionId"></param>
        public abstract void OnAnswerCall(Agent agent, string callSessionId);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        public abstract void OnFailMakeCall( Agent agent);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        /// <param name="callSessionId"></param>
        public abstract void OnCallReject( Agent agent, string callSessionId);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        /// <param name="isCallAnswerd"></param>
        public abstract void OnEndCall( Agent agent, bool isCallAnswerd);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        /// <param name="callSessionId"></param>
        public abstract void OnEndAcw( Agent agent, string callSessionId);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        public abstract void LogOn(Agent agent);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        /// <param name="callSessionId"></param>
        public abstract void OnLoggedOn( Agent agent, string callSessionId);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        public abstract void OnLogOff( Agent agent);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        /// <param name="statusText"></param>
        public abstract void OnOffline( Agent agent, string statusText);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        /// <param name="agentMode"></param>
        public abstract void OnRequestModeChange(Agent agent, AgentMode agentMode);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        public abstract void OnChangeMode(Agent agent);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        public abstract void OnBreak( Agent agent);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        public abstract void OnEndBreak( Agent agent);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        /// <param name="breakReason"></param>
        public abstract void OnRequestBreak(Agent agent, string breakReason);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        public abstract void OnRequestBreakCancel(Agent agent);

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        /// <param name="statusText"></param>
        /// <param name="statusCode"></param>
        /// <param name="msg"></param>
        public abstract void OnError( Agent agent, string statusText, int statusCode, string msg);
    }
}