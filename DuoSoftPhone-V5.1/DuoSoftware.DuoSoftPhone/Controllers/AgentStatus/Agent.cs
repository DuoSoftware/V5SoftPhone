using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DuoSoftware.DuoTools.DuoLogger;
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoSoftPhone.Controllers.Service;
using DuoSoftware.DuoSoftPhone.refResourceProxy;
using DuoSoftware.DuoSoftPhone.refUserAuth;

namespace DuoSoftware.DuoSoftPhone.Controllers.AgentStatus
{
    public class Agent
    {
        #region Property

        public bool SipStatus { get; set; }
        public string ErrorMsg { get; set; }
        public string StatusText { get; set; }
        public int StatusCode { get; set; }
        public DateTime LastActivity { get; private set; }
        private AgentEvent _agentCurrentState;
        private AgentEvent _agentPvState;

        private AgentMode _agentMode;
        private AgentMode _agentReqMode;

        public UserAuth Auth { get; private set; }
        private ResourceHandler ardsHandler;
        public Dictionary<string, object> SipSetting { get;private set; }
        private IUiState UiState { get; set; }
        public string IP { get; private set; }
        public string AgentLoginSessionId { get; set; }

        public Agent(UserAuth auth, Dictionary<string, object> sipSetting, string ip, string sessionId, IUiState uiState)
        {
            try
            {
                Auth = auth;
                SipSetting = sipSetting;
                IP = ip;
                AgentLoginSessionId = sessionId;
                UiState = uiState;
                AgentCurrentState = new AgentInitiate();
                ardsHandler = new ResourceHandler(auth.SecurityToken, auth.TenantID, auth.CompanyID);
                InitializeArdsEvent();
                CallHandler = new CallHandler(auth.SecurityToken, auth.TenantID, auth.CompanyID);
                LastActivity = DateTime.Now;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "Agent", exception, Logger.LogLevel.Error);
            }
        }

        public AgentMode AgentReqMode
        {
            private get { return _agentReqMode; }
            set
            {
                try
                {
                    _agentReqMode = value;
                    switch (value)
                    {
                        case AgentMode.Offline:
                            break;
                        case AgentMode.Inbound:
                            ardsHandler.SendModeChangeRequestInbound(Auth);
                            break;
                        case AgentMode.Outbound:
                            ardsHandler.SendModeChangeRequestOutbound(Auth);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("value");
                    }
                    
                }
                catch (Exception exception)
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentReqMode", exception, Logger.LogLevel.Error);
                }
            }
        }

        public AgentMode AgentMode
        {
        get { return _agentMode; }
            set
            {
                try
                {
                    _agentMode = value;
                    UiState.OnResourceModeChanged(value);
                }
                catch (Exception exception)
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentMode", exception,Logger.LogLevel.Error);
                }
            }
        }

        public AgentEvent AgentCurrentState
        {
            get
            {
                return _agentCurrentState;
            }
            set
            {
                try
                {
                    _agentPvState = _agentCurrentState;
                    _agentCurrentState = value;
                    ChangeUI(value);
                    LastActivity = DateTime.Now;
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, string.Format("Agent Status Change [{0}] To [{1}]", _agentPvState, value), Logger.LogLevel.Info);
                }
                catch (Exception exception)
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentCurrentState", exception, Logger.LogLevel.Error);
                }
            }
        }

        public string CallSessionId { get; set; }
        public int PortsipSessionId { get; set; }
        public CallDirection CallDirection { get; set; }
        public CallHandler CallHandler { get; set; }
        public bool IsCallAnswer { get; set; }

        #endregion

        #region Methods

        #region Private

        private void ChangeUI(AgentEvent state)
        {
            try
            {
                new ComMethods.SwitchOnType<AgentEvent>(state)
                       .Case<AgentInitiate>(initiate =>
                       {
                           UiState.InInitiateState();
                       })
                       .Case<AgentIdle>(i =>
                       {
                           ardsHandler.SendStatusChangeRequestIdel(Auth, CallSessionId, IsCallAnswer);
                       })
                       .Case<AgentBusy>(b =>
                       {
                           UiState.InAgentBusy(CallDirection);
                           if (CallDirection == CallDirection.Incoming)
                               ardsHandler.ResourceStatusChangeBusy(Auth, CallSessionId, IsCallAnswer);
                       })
                        .Case<AgentAcw>(b =>
                        {
                            UiState.InAgentAcwState();
                            ardsHandler.ResourceStatusChangeAcw(Auth, CallSessionId, IsCallAnswer);
                        })
                         .Case<AgentBreak>(b =>
                         {
                             UiState.InBreakState();
                         })
                           .Case<AgentOffline>(b => UiState.InOfflineState(StatusText, ErrorMsg, StatusCode))
                       .Default<AgentEvent>(t => UiState.InOfflineState(StatusText, ErrorMsg, StatusCode));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "Agent ChangeUI", exception, Logger.LogLevel.Error);
            }
        }

        private void InitializeArdsEvent()
        {
            try
            {
                ardsHandler.OnStatusUpdatedMessage += (s) =>
                {
                    try
                    {
                       
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnStatusUpdatedMessage", exception, Logger.LogLevel.Error);
                    }
                };

                

                #region Breake

                ardsHandler.OnSendResourceChangeBreakCompleted += (s) =>
                {
                    try
                    {
                        AgentCurrentState = new AgentBreak();
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnSendResourceChangeBreakCompleted", exception, Logger.LogLevel.Error);
                    }
                };


                ardsHandler.OnSendStatusChangeRequestBreakCompleted += (s) =>
                {
                    try
                    {
                        if (s.Command == WorkflowResultCode.ACDS301 || s.Command == WorkflowResultCode.ACDS4032)
                        {
                            AgentCurrentState = new AgentBreak();
                            ardsHandler.ResourceStatusChangeBreak(Auth,CallSessionId);
                        }
                        UiState.ShowNotifications(s);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnSendStatusChangeRequestBreakCompleted", exception, Logger.LogLevel.Error);
                    }
                };

                ardsHandler.OnCancelBreakRequestCompleted += (s) =>
                {
                    try
                    {
                        UiState.CancelBreakRequest(s);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnCancelBreakRequestCompleted", exception, Logger.LogLevel.Error);
                    }
                };
                
                #endregion Breake

                #region Reregistor

                ardsHandler.OnResourceRegistrationCompleted += (s) =>
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnResourceRegistrationCompleted",  Logger.LogLevel.Debug);
                };

                #endregion Reregistor

               

                #region StatusChangeRequestIdel

                ardsHandler.OnSendStatusChangeRequestIdelCompleted += (s) =>
                {
                    try
                    {
                        switch (s.Command)
                        {
                            case WorkflowResultCode.ACDS4033:
                                {
                                    ardsHandler.ResourceModeChange(Auth, CallSessionId);
                                }
                                break;
                            case WorkflowResultCode.ACDS403:
                                {
                                    UiState.InAgentIdleState(_agentPvState);
                                }
                                break;

                            case WorkflowResultCode.ACDS301:
                            case WorkflowResultCode.ACDS4032:
                                {
                                    AgentCurrentState = new AgentBreak();
                                    ardsHandler.ResourceStatusChangeBreak(Auth, CallSessionId);
                                } 
                                break;
                        }
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnSendStatusChangeRequestIdelCompleted", exception, Logger.LogLevel.Error);
                    }
                };

                #endregion StatusChangeRequestIdel

                #region Mode

                ardsHandler.OnSendModeChangeRequestOutboundCompleted += (s) =>
                {
                    if (s.Command == WorkflowResultCode.ACDS501)
                    {
                        ardsHandler.ResourceModeChange(Auth, CallSessionId);
                    }
                    else
                    {
                        UiState.OnSendModeChangeRequestOutbound(s);
                    }
                    
                };

                ardsHandler.OnSendModeChangeRequestInboundCompleted += (s) =>
                {
                    if (s.Command == WorkflowResultCode.ACDS501)
                    {
                        ardsHandler.ResourceModeChange(Auth, CallSessionId);
                    }
                    else
                    {
                        UiState.OnSendModeChangeRequestInbound(s);
                    }

                };

                ardsHandler.OnResourceModeChanged += (r) =>
                {
                    try
                    {
                        AgentMode = AgentReqMode;
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnResourceModeChanged", exception,Logger.LogLevel.Error);
                    }
                }; 
                #endregion

                
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "InitializeArdsEvent", exception,Logger.LogLevel.Error);
            }
        }

        #endregion

        #region public

        public void ResourceStatusChangeBusy()
        {
            try
            {
                ardsHandler.ResourceStatusChangeBusy(Auth, CallSessionId, IsCallAnswer);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "ResourceStatusChangeBusy", exception, Logger.LogLevel.Error);
            }
        }

        public void ResourceModeChange()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentInitiate-OnLogin", Logger.LogLevel.Info);
                ardsHandler.ResourceModeChange(Auth, CallSessionId);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "ResourceModeChange", exception, Logger.LogLevel.Error);
               
            }
        }

        public void CancelBreakRequest()
        {
            try
            {
                ardsHandler.CancelBreakRequest(Auth, CallSessionId);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "ResourceModeChange", exception, Logger.LogLevel.Error);

            }
        }

        public void SendStatusChangeRequestBreak(string breakReason)
        {
            try
            {
                ardsHandler.SendStatusChangeRequestBreak(Auth, CallSessionId,breakReason);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "SendStatusChangeRequestBreak", exception, Logger.LogLevel.Error);
            }
        }

        public void ResourceUnregistration()
        {

            try
            {
                ardsHandler.ResourceUnregistration(Auth);
                var authClient = new refUserAuth.IauthClient();
                authClient.LogOut(Auth.SecurityToken);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "SendStatusChangeRequestBreak", exception, Logger.LogLevel.Error);
            }
        }

        #endregion

        #endregion

       
    }
}
