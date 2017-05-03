using System;
using DuoSoftware.DuoLogger;
using DuoSoftware.DuoSoftPhone.Controllers.Service;

namespace DuoSoftware.DuoSoftPhone.Controllers.AgentStatus
{
    public class AgentOffline : AgentState
    {
        public override void OnTimeOut(ref Agent agent)
        {
            try
            {
                try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
                
            }
        }

        public override void OnLogOn(ref Agent agent)
        {
            agent.ResoursHandler.ResourceRegistration(agent.Auth, agent.IP);
        }

        public override void OnMakeCall(ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnAnswerCall(ref Agent agent, string callSessionId)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
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
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
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
            try { throw new NotImplementedException("Invalid Agent Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnRequestAgentBreakCancel(ref Agent agent, string callSessionId)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }
    }
}