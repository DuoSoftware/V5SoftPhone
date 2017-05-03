﻿using DuoSoftware.DuoTools.DuoLogger;
using System;

namespace DuoSoftware.DuoSoftPhone.Controllers.AgentStatus
{
    /// <summary>
    ///
    /// </summary>
    internal class AgentIdle : AgentEvent
    {
        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        public override void OnResetStatus(Agent agent)
        {
            //try { throw new NotImplementedException("Invalid Agent Status."); }
            //catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentIdle", exception, Logger.LogLevel.Error); }
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBusy", new NotImplementedException("Invalid Agent Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        public override void OnTimeOut(Agent agent)
        {
            //try { throw new NotImplementedException("Invalid Agent Status."); }
            //catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentIdle", exception, Logger.LogLevel.Error); }
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBusy", new NotImplementedException("Invalid Agent Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        /// <param name="caller"></param>
        /// <param name="sessionId"></param>
        public override void OnIncomingCall(Agent agent, string caller, int sessionId)
        {
            try
            {
                if (agent == null) return;
                agent.CallSessionId = caller.Split('@')[0].Replace("sip:", string.Empty); ;//var id = sessionId.Split('@');
                agent.PortsipSessionId = sessionId;
                agent.CallDirection = CallDirection.Incoming;
                agent.AgentCurrentState = new AgentBusy();
            }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentIdle", exception, Logger.LogLevel.Error); }
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        public override void OnMakeCall(Agent agent)
        {
            try
            {
                if (agent == null) return;
                agent.CallDirection = CallDirection.Outgoing;
                agent.AgentCurrentState = new AgentBusy();
            }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentIdle", exception, Logger.LogLevel.Error); }
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        /// <param name="callSessionId"></param>
        public override void OnAnswerCall(Agent agent, string callSessionId)
        {
            //try { throw new NotImplementedException("Invalid Agent Status."); }
            //catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentIdle", exception, Logger.LogLevel.Error); }
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBusy", new NotImplementedException("Invalid Agent Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        public override void OnFailMakeCall(Agent agent)
        {
            //try { throw new NotImplementedException("Invalid Agent Status."); }
            //catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentIdle", exception, Logger.LogLevel.Error); }
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBusy", new NotImplementedException("Invalid Agent Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        /// <param name="callSessionId"></param>
        public override void OnCallReject(Agent agent, string callSessionId)
        {
            //try { throw new NotImplementedException("Invalid Agent Status."); }
            //catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentIdle", exception, Logger.LogLevel.Error); }
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBusy", new NotImplementedException("Invalid Agent Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        /// <param name="isCallAnswerd"></param>
        public override void OnEndCall(Agent agent, bool isCallAnswerd)
        {
            //try { throw new NotImplementedException("Invalid Agent Status."); }
            //catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentIdle", exception, Logger.LogLevel.Error); }
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBusy", new NotImplementedException("Invalid Agent Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        /// <param name="callSessionId"></param>
        public override void OnEndAcw(Agent agent, string callSessionId)
        {
            //try { throw new NotImplementedException("Invalid Agent Status."); }
            //catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentIdle", exception, Logger.LogLevel.Error); }
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBusy", new NotImplementedException("Invalid Agent Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        public override void LogOn(Agent agent)
        {
            try {
                if (agent != null) agent.AgentCurrentState = new AgentIdle();
            }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentIdle", exception, Logger.LogLevel.Error); }
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        /// <param name="callSessionId"></param>
        public override void OnLoggedOn(Agent agent, string callSessionId)
        {
            //try { throw new NotImplementedException("Invalid Agent Status."); }
            //catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentIdle", exception, Logger.LogLevel.Error); }
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBusy", new NotImplementedException("Invalid Agent Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        public override void OnLogOff(Agent agent)
        {
            try
            {
                if (agent != null) agent.ResourceUnregistration();
            }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentIdle", exception, Logger.LogLevel.Error); }
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        /// <param name="statusText"></param>
        public override void OnOffline(Agent agent, string statusText)
        {
            try
            {
                if (agent != null) agent.AgentCurrentState = new AgentOffline();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentIdle", exception, Logger.LogLevel.Error);
            }
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        /// <param name="agentMode"></param>
        public override void OnRequestModeChange(Agent agent, AgentMode agentMode)
        {
            try {
                if (agent != null) agent.AgentReqMode = agentMode;
            }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentIdle", exception, Logger.LogLevel.Error); }
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        public override void OnChangeMode(Agent agent)
        {
            //try { throw new NotImplementedException("Invalid Agent Status."); }
            //catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentIdle", exception, Logger.LogLevel.Error); }
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBusy", new NotImplementedException("Invalid Agent Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        public override void OnBreak(Agent agent)
        {
            //try { throw new NotImplementedException("Invalid Agent Status."); }
            //catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentIdle", exception, Logger.LogLevel.Error); }
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBusy", new NotImplementedException("Invalid Agent Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        public override void OnEndBreak(Agent agent)
        {
            //try { throw new NotImplementedException("Invalid Agent Status."); }
            //catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentIdle", exception, Logger.LogLevel.Error); }
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBusy", new NotImplementedException("Invalid Agent Status."), Logger.LogLevel.Error);
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        /// <param name="breakReason"></param>
        public override void OnRequestBreak(Agent agent, string breakReason)
        {
            try {
                if (agent != null) agent.SendStatusChangeRequestBreak(breakReason);
            }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentIdle", exception, Logger.LogLevel.Error); }
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        public override void OnRequestBreakCancel(Agent agent)
        {
            try {
                if (agent != null) agent.CancelBreakRequest();
            }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentIdle", exception, Logger.LogLevel.Error); }
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="agent"></param>
        /// <param name="statusText"></param>
        /// <param name="statusCode"></param>
        /// <param name="msg"></param>
        public override void OnError(Agent agent, string statusText, int statusCode, string msg)
        {
            try
            {
                if (agent == null) return;
                agent.ErrorMsg = msg;
                agent.StatusCode = statusCode;
                agent.StatusText = statusText;
                agent.AgentCurrentState = new AgentOffline();
            }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentIdle", exception, Logger.LogLevel.Error); }
        }
    }
}