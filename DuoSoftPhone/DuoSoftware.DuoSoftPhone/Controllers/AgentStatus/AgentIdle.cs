using System;
using DuoSoftware.DuoLogger;
using DuoSoftware.DuoSoftPhone.Controllers.Service;
using DuoSoftware.DuoSoftPhone.refResourceProxy;
using DuoSoftware.DuoSoftPhone.Ui;

namespace DuoSoftware.DuoSoftPhone.Controllers.AgentStatus
{
    internal class AgentIdle : AgentState
    {
        public override void OnTimeOut(ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnLogOn(ref Agent agent, string callSessionId)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnMakeCall(ref Agent agent)
        {
            agent.AgentCurrentState = new AgentBusy();
            //ArdsHandler.ResourceStatusChangeBusy(dialPad.Auth, dialPad.CallSessionId); // call server not accept dis msg. kusal ask me to remove this update.
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
            agent.CancelAgentBreakRequest();
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

        public override void OnOffline(ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnAgentChangeMode(ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnResetStatus(ref Agent agent)
        {
            agent.AgentCurrentState = new AgentIdle();
            
        }

        public override void OnRequestAgentModeChange(ref Agent agent, AgentMode agentMode)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnRequestAgentBreak(ref Agent agent, AgentMode agentMode, string callSessionId)
        {
            var result = ArdsHandler.SendStatusChangeRequestBreak(agent.UserAuth, callSessionId);
            agent.BreakRequsetHandle(result);
        }
    }
}