using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DuoSoftware.DuoLogger;
using DuoSoftware.DuoSoftPhone.Controllers.Service;
using DuoSoftware.DuoSoftPhone.refResourceProxy;

namespace DuoSoftware.DuoSoftPhone.Controllers.AgentStatus
{
    internal class AgentInitiate : AgentState
    {
        
        private void UserLogin(Agent agent)
        {
            try
            {   var sip = ProfileManagementHandler.GetSipProfile(agent.Auth.SecurityToken, agent.Auth.guUserId);
                if (sip == null)
                {
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Fail to Get SIP Profile", Logger.LogLevel.Error);
                    MessageBox.Show("Fail to Get SIP Profile", "Duo Dialer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                agent.ResoursHandler.ResourceRegistration(agent.Auth, agent.IP);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Login fail", exception, Logger.LogLevel.Error);
                MessageBox.Show("Login Fail", "Duo Dialer", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        public override void OnTimeOut(ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnLogOn(ref Agent agent)
        {
            var auth = DuoAuth.DuoAuthorizationService.Login(agent.UserName, agent.Password,agent.Company, "DuoSoftPhone");
            agent.SetAuth(auth);
            UserLogin(agent);
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
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnChangeAgentStatus(ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnLoggedOn(ref Agent agent, string callSessionId)
        {
            try
            {
                agent.ResoursHandler.ResourceModeChange(agent.Auth, callSessionId);
                agent.AgentCurrentState = new AgentIdle();

            }
            catch (Exception exception)
            {
                { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "OnLoggedOn", exception, Logger.LogLevel.Error); }
            }
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
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnRequestAgentBreakCancel(ref Agent agent, string callSessionId)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); } catch (Exception exception) { Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error); }
        }
    }
}