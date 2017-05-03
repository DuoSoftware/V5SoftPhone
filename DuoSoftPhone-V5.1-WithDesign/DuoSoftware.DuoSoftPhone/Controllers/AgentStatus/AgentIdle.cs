﻿using System;
using DuoSoftware.DuoLogger;
using DuoSoftware.DuoSoftPhone.Controllers.Service;

namespace DuoSoftware.DuoSoftPhone.Controllers.AgentStatus
{
    internal class AgentIdle : AgentState
    {
        public override void OnTimeOut(ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnLogOn(ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnMakeCall(ref Agent agent)
        {
            agent.AgentCurrentState = new AgentBusy();
        }

        public override void OnAnswerCall(ref Agent agent, string callSessionId)
        {
            agent.AgentCurrentState = new AgentBusy();
        }

        public override void OnEndCall(ref Agent agent, string callSessionId)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnEndCallLog(ref Agent agent, string callSessionId)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnBreak(ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnEndBreak(ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnChangeAgentStatus(ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnLoggedOn(ref Agent agent, string callSessionId)
        {
            Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Due to Reconfiguration",Logger.LogLevel.Info);
        }

        public override void OnLogOff(ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnFailMakeCall(ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnCallReject(ref Agent agent, string callSessionId)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnOffline(ref Agent agent, string statusText)
        {
            agent.AgentCurrentState = new AgentOffline();
            
        }

        public override void OnAgentChangeMode(ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnResetStatus(ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnRequestAgentModeChange(ref Agent agent, AgentMode agentMode)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnRequestAgentBreak(ref Agent agent, string callSessionId, string breakReason)
        {
            agent.ResoursHandler.SendStatusChangeRequestBreak(agent.Auth,callSessionId, breakReason);
        }

        public override void OnRequestAgentBreakCancel(ref Agent agent, string callSessionId)
        {
            agent.ResoursHandler.CancelBreakRequest(agent.Auth, callSessionId);
        }
    }
}