using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DuoSoftware.DuoAuth.RefAuth.Iauth;
using DuoSoftware.DuoLogger;
using DuoSoftware.DuoSoftPhone.Controllers.PortSIP;
using DuoSoftware.DuoSoftPhone.Controllers.Service;
using DuoSoftware.DuoSoftPhone.refResourceProxy;
using DuoSoftware.DuoSoftPhone.Ui;

namespace DuoSoftware.DuoSoftPhone.Controllers.AgentStatus
{
    public class Agent
    {
        public delegate void AgentRequestHandler(AgentState agentState, bool valur=false);

        public event AgentRequestHandler OnAgentBreakRequestGranted;
        public event AgentRequestHandler OnAgentBreakRequestQueued;
        public event AgentRequestHandler OnAgentBreakRequestCancel;
        public event AgentRequestHandler OnAgentBreakEnd;

        public delegate void AgentStatusUpdate();

        public event AgentStatusUpdate OnAgentStatusUpdating;
        public event AgentStatusUpdate OnAgentStatusUpdated;

        private AgentState _agentCurrentState;
        private AgentState _agentPreviouState;

        public Agent()
        {
            AgentCurrentMode = new AgentMode();
            AgentPreviousMode = new AgentMode();
            _agentCurrentState = new AgentInitiate();
        }
        public AgentMode AgentCurrentMode { get; set; }
        public AgentMode AgentPreviousMode { get; set; }

        public ProfileInfo SipProfile { get; set; }

        public AgentState AgentCurrentState
        {
            get { return _agentCurrentState; }
            set
            {
                _agentPreviouState = _agentCurrentState;
               _agentCurrentState = value;
                UpdateAgentStatus(value);
            }
        }

        public bool IsAgentBreakeRequest { get; set; }

        public bool IsAgentRequestModeChange { get; set; }

        public bool IsAgentBreakGranted { get; set; }
        public String AgentSessionId { get; set; }
        public UserAuth UserAuth { get; set; }
        public string CallSessionId { get; set; }

        private void UpdateAgentStatus(AgentState status)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger5,string.Format("UpdateAgentStatus. {0} To {1}", _agentPreviouState, _agentCurrentState),Logger.LogLevel.Info);

                new SwitchOnType<AgentState>(status)
                    .Case<AgentAcw>(ac =>
                    {
                        OnAgentStatusUpdating();
                        ArdsHandler.ResourceStatusChangeACW(UserAuth, CallSessionId);
                        OnAgentStatusUpdated();

                    })
                    .Case<AgentBreak>(asc =>
                    {
                        OnAgentStatusUpdating();
                        ArdsHandler.ResourceStatusChangeBreak(UserAuth, CallSessionId);
                        OnAgentStatusUpdated();
                    })
                    .Case<AgentBusy>(conf =>
                    {
                        OnAgentStatusUpdating();
                        ArdsHandler.ResourceStatusChangeBusy(UserAuth, CallSessionId);
                        OnAgentStatusUpdated();
                    })
                    .Case<AgentIdle>(cont =>
                    {
                        OnAgentStatusUpdating();
                        var reply = ArdsHandler.SendStatusChangeRequestIdel(UserAuth, CallSessionId);
                        BreakRequsetHandle(reply.Command);
                        OnAgentStatusUpdated();
                    });
                //.Case<AgentInitiate>(disc => SetDisconnectDialPad())
                //.Case<AgentOffline>(spotTrade => SetHoldDialPad())
                //.Default<AgentState>(t => DisableRingtone());

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "UpdateAgentStatus", exception,Logger.LogLevel.Error);
                OnAgentStatusUpdated();
            }
        }

        public void BreakRequsetHandle(WorkflowResultCode result)
        {

            try
            {
                if (result == WorkflowResultCode.ACDS301 || result == WorkflowResultCode.ACDS4032)
                {
                    IsAgentBreakGranted = true;
                    if (AgentCurrentState.GetType() == typeof (AgentIdle))
                        if (OnAgentBreakRequestGranted != null)
                            OnAgentBreakRequestGranted(AgentCurrentState);
                    return;
                }

                IsAgentBreakeRequest = (result == WorkflowResultCode.ACDS302 || result == WorkflowResultCode.ACDS301);
                if (IsAgentBreakeRequest && OnAgentBreakRequestQueued != null)
                {
                    OnAgentBreakRequestQueued(AgentCurrentState);
                }

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "BreakRequsetHandle", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void CancelAgentBreakRequest()
        {
            try
            {
                IsAgentBreakGranted = false;
                IsAgentBreakeRequest = !ArdsHandler.CancelBreakRequest(UserAuth);
                if (OnAgentBreakRequestCancel != null)
                {
                    OnAgentBreakRequestCancel(AgentCurrentState, !IsAgentBreakeRequest);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "BreakRequsetHandle", exception, Logger.LogLevel.Error);
            }
        }

        public void EndAgentBreak()
        {
            try
            {
                var reply = ArdsHandler.SendStatusChangeRequestIdel(UserAuth,string.Empty);
                if (OnAgentBreakEnd != null)
                    OnAgentBreakEnd(AgentCurrentState, reply.Command == WorkflowResultCode.ACDS403);
                IsAgentBreakGranted = false;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "EndAgentBreak", exception, Logger.LogLevel.Error);
            }
        }
    }
}
