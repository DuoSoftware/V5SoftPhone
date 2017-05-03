using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoSoftPhone.Controllers.Service;
using DuoSoftware.DuoSoftPhone.RefResourceProxy;
using DuoSoftware.DuoSoftPhone.RefUserAuth;
using DuoSoftware.DuoTools.DuoLogger;
using System;
using System.Collections.Generic;

namespace DuoSoftware.DuoSoftPhone.Controllers.AgentStatus
{
    /// <summary>
    ///
    /// </summary>
    public class Agent
    {
        #region Property

        
        /// <summary>
        ///
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        ///
        /// </summary>
        private AgentEvent _agentCurrentState;

        /// <summary>
        ///
        /// </summary>
        private AgentEvent _agentPvState;

        /// <summary>
        ///
        /// </summary>
        public DateTime LastActivity { get; private set; }

        /// <summary>
        ///
        /// </summary>
        private AgentMode _agentMode;

        /// <summary>
        ///
        /// </summary>
        private AgentMode _agentReqMode;

        /// <summary>
        ///
        /// </summary>
        public UserAuth Auth { get; private set; }

        /// <summary>
        ///
        /// </summary>
        private SoftPhoneResourceHandler ardsHandler;

        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, object> SipSetting { get; private set; }

        /// <summary>
        ///
        /// </summary>
        private IUiState UiState { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string IP { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public string AgentLogOnSessionId { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="sipSetting"></param>
        /// <param name="ip"></param>
        /// <param name="sessionId"></param>
        /// <param name="uiState"></param>
        public Agent(UserAuth auth, Dictionary<string, object> sipSetting, string ip, string sessionId, IUiState uiState)
        {
            try
            {
                Auth = auth;
                SipSetting = sipSetting;
                IP = ip;
                AgentLogOnSessionId = sessionId;
                UiState = uiState;
                AgentCurrentState = new AgentInitiate();
                ardsHandler = new SoftPhoneResourceHandler(auth.SecurityToken, auth.TenantID, auth.CompanyID);
                InitializeArdsEvent();
                CallHandler = new CallHandler(auth.SecurityToken, auth.TenantID, auth.CompanyID);
                LastActivity = DateTime.Now;
                
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "Agent", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
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
                    throw;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public AgentMode AgentMode
        {
            get { return _agentMode; }
            private set
            {
                try
                {
                    _agentMode = value;
                    UiState.OnResourceModeChanged(value);
                }
                catch (Exception exception)
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentMode", exception, Logger.LogLevel.Error);
                    throw;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public AgentEvent AgentCurrentState
        {
            get
            {
                return _agentCurrentState;
            }
            set
            {
                var logger = Logger.Instance;
                try
                {
                    _agentPvState = _agentCurrentState;
                    _agentCurrentState = value;
                    ChangeUI(value);
                    LastActivity = DateTime.Now;
                    logger.LogMessage(Logger.LogAppender.DuoLogger2,
                        string.Format("Agent Status Change [{0}] To [{1}]", _agentPvState, value), Logger.LogLevel.Info);
                }
                catch (Exception exception)
                {
                    logger.LogMessage(Logger.LogAppender.DuoLogger2, "AgentCurrentState", exception, Logger.LogLevel.Error);
                    throw;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string CallSessionId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int PortsipSessionId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public CallDirection CallDirection { get; set; }

        /// <summary>
        ///
        /// </summary>
        public CallHandler CallHandler { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool IsCallAnswer { get; set; }

        public bool TransferCall { get; set; }

        #endregion Property

        #region Methods

        #region Private

        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
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
                           ardsHandler.SendStatusChangeRequestIdel(Auth, CallSessionId, IsCallAnswer, TransferCall);
                       })
                       .Case<AgentBusy>(b =>
                       {
                           UiState.InAgentBusy();
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

        /// <summary>
        ///
        /// </summary>
        private void InitializeArdsEvent()
        {
            var logger = Logger.Instance;
            try
            {
                
                ardsHandler.OnStatusUpdatedMessage += s =>
                {
                    try
                    {
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoLogger2, "OnStatusUpdatedMessage", exception, Logger.LogLevel.Error);
                    }
                };

                #region Breake

                ardsHandler.OnSendResourceChangeBreakCompleted += s =>
                {
                    try
                    {
                        AgentCurrentState = new AgentBreak();
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoLogger2, "OnSendResourceChangeBreakCompleted", exception, Logger.LogLevel.Error);
                    }
                };

                ardsHandler.OnSendStatusChangeRequestBreakCompleted += s =>
                {
                    try
                    {
                        if (s.Command == WorkflowResultCode.ACDS301 || s.Command == WorkflowResultCode.ACDS4032)
                        {
                            AgentCurrentState = new AgentBreak();
                            ardsHandler.ResourceStatusChangeBreak(Auth, CallSessionId);
                        }
                        UiState.ShowNotifications(s);
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoLogger2, "OnSendStatusChangeRequestBreakCompleted", exception, Logger.LogLevel.Error);
                    }
                };

                ardsHandler.OnCancelBreakRequestCompleted += s =>
                {
                    try
                    {
                        UiState.CancelBreakRequest(s);
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoLogger2, "OnCancelBreakRequestCompleted", exception, Logger.LogLevel.Error);
                    }
                };

                #endregion Breake

                #region Reregistor

                ardsHandler.OnResourceRegistrationCompleted += s =>
                {
                    logger.LogMessage(Logger.LogAppender.DuoLogger2, "OnResourceRegistrationCompleted", Logger.LogLevel.Debug);
                };

                #endregion Reregistor

                #region StatusChangeRequestIdel

                ardsHandler.OnSendStatusChangeRequestIdelCompleted += s =>
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
                        logger.LogMessage(Logger.LogAppender.DuoLogger2, "OnSendStatusChangeRequestIdelCompleted", exception, Logger.LogLevel.Error);
                    }
                };

                #endregion StatusChangeRequestIdel

                #region Mode

                ardsHandler.OnSendModeChangeRequestOutboundCompleted += s =>
                {
                    try
                    {
                        if (s.Command == WorkflowResultCode.ACDS501)
                        {
                            ardsHandler.ResourceModeChange(Auth, CallSessionId);
                        }
                        else
                        {
                            UiState.OnSendModeChangeRequestOutbound(s);
                        }
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoLogger2, "OnSendModeChangeRequestOutboundCompleted", exception, Logger.LogLevel.Error);
                    }
                };

                ardsHandler.OnSendModeChangeRequestInboundCompleted += s =>
                {
                    try
                    {
                        if (s.Command == WorkflowResultCode.ACDS501)
                        {
                            ardsHandler.ResourceModeChange(Auth, CallSessionId);
                        }
                        else
                        {
                            UiState.OnSendModeChangeRequestInbound(s);
                        }
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoLogger2, "OnSendModeChangeRequestInboundCompleted", exception, Logger.LogLevel.Error);
                    }
                };

                ardsHandler.OnResourceModeChanged += r =>
                {
                    try
                    {
                        AgentMode = AgentReqMode;
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoLogger2, "OnResourceModeChanged", exception, Logger.LogLevel.Error);
                    }
                };

                #endregion Mode
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger2, "InitializeArdsEvent", exception, Logger.LogLevel.Error);
            }
        }

        #endregion Private

        #region public

        /// <summary>
        ///
        /// </summary>
        public void ResourceStatusChangeBusy()
        {
            try
            {
                ardsHandler.ResourceStatusChangeBusy(Auth, CallSessionId, IsCallAnswer);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "ResourceStatusChangeBusy", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void ResourceModeChange()
        {
            var logger = Logger.Instance;
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger2, "ResourceModeChange", Logger.LogLevel.Info);
                ardsHandler.ResourceModeChange(Auth, CallSessionId);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger2, "ResourceModeChange", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void CancelBreakRequest()
        {
            try
            {
                ardsHandler.CancelBreakRequest(Auth, CallSessionId);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "ResourceModeChange", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="breakReason"></param>
        public void SendStatusChangeRequestBreak(string breakReason)
        {
            try
            {
                ardsHandler.SendStatusChangeRequestBreak(Auth, CallSessionId, breakReason);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "SendStatusChangeRequestBreak", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void ResourceUnregistration()
        {
            try
            {
                ardsHandler.ResourceUnregistration(Auth);
                FrameworkHandler.LogOff(Auth.SecurityToken, string.Empty);
                /*var authClient = new RefUserAuth.IauthClient(); //move this part to ards to avoid session expire prob
                authClient.LogOut(Auth.SecurityToken);*/
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "SendStatusChangeRequestBreak", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        #endregion public

        #endregion Methods
    }
}