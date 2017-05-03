using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DuoSoftware.CommonTools.Security;
using DuoSoftware.DuoLogger;
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoSoftPhone.Controllers.Service;
using DuoSoftware.DuoSoftPhone.refResourceProxy;

namespace DuoSoftware.DuoSoftPhone.Controllers.AgentStatus
{
    public class Agent
    {
        #region Property

        private AgentState _agentCurrentState;
        private AgentState _agentPvState;
        private bool ShowCallAlert;

        public AgentState AgentCurrentState
        {
            get
            {
                return _agentCurrentState;
            }
            set
            {
                _agentPvState = _agentCurrentState;
                ChangeUI(value);
                _agentCurrentState = value;
            }
        }

        public string CallSessionId { get; set; }
        public CallDirection CallDirection { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Company { get; set; }
        public UserAuth Auth { get; set; }
        public ResourceHandler ResoursHandler { get; set; }
        public CallHandler CallHandler { get; set; }
        public Dictionary<string, object> SipSetting { get; set; }
        private IUiState UiState { get; set; }
        public string IP { get; set; }
        #endregion

        #region Methods

        #region Private

        private void ChangeUI(AgentState state)
        {
            try
            {
                new ComMethods.SwitchOnType<AgentState>(state)
                       .Case<AgentInitiate>(initiate =>
                       {
                           UiState.InInitiateState();
                           
                       })
                       .Case<AgentIdle>(i =>
                       {
                           UiState.InIdleState();
                           ResoursHandler.SendStatusChangeRequestIdel(Auth,CallSessionId);
                       })
                       .Case<AgentBusy>(b =>
                       {
                          UiState.InCallRingingState();// (CallDirection);
                           if (CallDirection == CallDirection.Incoming)
                               ResoursHandler.ResourceStatusChangeBusy(Auth, CallSessionId);
                       })
                        .Case<AgentAcw>(b =>
                        {
                            UiState.InAcwState();
                            ResoursHandler.ResourceStatusChangeAcw(Auth, CallSessionId);
                        })
                         .Case<AgentBreak>(b =>
                         {
                             UiState.InBreakState();
                             ResoursHandler.ResourceStatusChangeBreak(Auth,CallSessionId);
                         })
                           .Case<AgentOffline>(b => UiState.InOfflineState())
                       .Default<AgentState>(t => UiState.InOfflineState());
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Agent ChangeUI", exception, Logger.LogLevel.Error);
            }
        }

        private string GetLocalIpAddress()
        {
            try
            {
                string myHost = Dns.GetHostName();

                string myIp = (from ipAddress in Dns.GetHostEntry(myHost).AddressList
                               where ipAddress.IsIPv6LinkLocal == false
                               where ipAddress.IsIPv6Multicast == false
                               where ipAddress.IsIPv6SiteLocal == false
                               where ipAddress.IsIPv6Teredo == false
                               select ipAddress).Select(ipAddress => ipAddress.ToString()).FirstOrDefault();

                if (!IsValidIP(myIp))
                {
                    IPAddress[] myIp1 = Dns.GetHostAddresses(myHost);
                    foreach (IPAddress ipAddress in
                        myIp1.Where(ipAddress => ipAddress.AddressFamily == AddressFamily.InterNetwork))
                    {
                        myIp = ipAddress.ToString();
                        break;
                    }
                }

                return myIp;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "GetLocalIPAddress", exception, Logger.LogLevel.Error);
                return string.Empty;
            }
        }

        private bool IsValidIP(params object[] list)
        {
            try
            {
                var addr = list[0].ToString();
                //create our match pattern
                //            const string pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.
                //    ([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";

                const string pattern = "\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}";
                //create our Regular Expression object
                var check = new Regex(pattern);
                //boolean variable to hold the status
                bool valid = false;
                //check to make sure an ip address was provided
                valid = addr != "" && check.IsMatch(addr, 0);
                //return the results
                return valid;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "IsValidIP", exception, Logger.LogLevel.Error);
                //Console.WriteLine(exception.Message);
                return false;
            }
        }


        #endregion

        #region public

        public void SessionCreated(string sessionId)
        {
            try
            {
                CallSessionId = sessionId;
                ResoursHandler.ResourceStatusChangeBusy(Auth, sessionId);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "SessionCreated", exception, Logger.LogLevel.Error);
            }
        }
        public Agent(string userName, string password, IUiState uiState)
        {
            UiState = uiState;
            UserName = userName;
            Password = password;
            var settingObject = System.Configuration.ConfigurationSettings.AppSettings;
            Company = int.Parse(settingObject["Company"]);
            ShowCallAlert = settingObject["ShowCallAlert"].Equals("1");
            AgentCurrentState = new AgentInitiate();
            IP = GetLocalIpAddress();
        }

        

        public void SetAuth(UserAuth auth)
        {
            try
            {

                Auth = auth;

                ResoursHandler = new ResourceHandler(Auth.SecurityToken, Auth.TenantID, Auth.CompanyID);

                SipSetting = ProfileManagementHandler.GetSipProfile(auth.SecurityToken, auth.guUserId);
                if (SipSetting == null)
                {
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Fail to Get SIP Profile", Logger.LogLevel.Error);
                    MessageBox.Show("Fail to Get SIP Profile", "Duo Dialer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ResoursHandler.OnResourceRegistrationCompleted += (r) =>
                {
                    try
                    {
                        switch (r.Command)
                        {
                            case WorkflowResultCode.ACDS101: //- Agent sucessfully registered (ACDS101)
                                ResoursHandler.SendModeChangeRequestInbound(Auth);
                                break;

                            case WorkflowResultCode.ACDE101: //- Agent already registered with different IP (ACDE101)
                                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Agent already registered with different IP-ARDS Code : ACDE101", Logger.LogLevel.Info);
                                MessageBox.Show("Agent already registered with different IP", "Duo Dialer", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                break;

                            default:
                                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Login Fail-- ARDS not allow to Login. ARDS Code : " + r.Command, Logger.LogLevel.Info);
                                MessageBox.Show("Login Fail", "Duo Dialer", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                break;
                        }
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "OnResourceRegistrationCompleted", exception, Logger.LogLevel.Error);
                    }
                };

                ResoursHandler.OnSendModeChangeRequestInboundCompleted += (s) =>
                {
                    try
                    {
                        CallHandler = new CallHandler(auth.SecurityToken, auth.TenantID, auth.CompanyID);
                        UiState.InInitiateMsgState(CallHandler.AutoAnswerByDefault(), (CallHandler.AutoAnswerByDefault() && CallHandler.AutoAnswerCanAgentEnable()), auth.UserName);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "OnSendModeChangeRequestInboundCompleted", exception, Logger.LogLevel.Error);
                    }
                };

                ResoursHandler.OnStatusUpdatedMessage += (s) =>
                {
                    try
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault,string.Format("OnStatusUpdatedMessage : {0}",s), Logger.LogLevel.Info);
                        //if (!string.IsNullOrEmpty(s.ExtraData))
                        //    mynotifyicon.ShowBalloonTip(5, "Duo Soft Phone", s.ExtraData, ToolTipIcon.Info);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "OnStatusUpdatedMessage", exception, Logger.LogLevel.Error);
                    }
                };

                ResoursHandler.OnSendStatusChangeRequestIdelCompleted += (s) =>
                {
                    try
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, string.Format("OnSendStatusChangeRequestIdelCompleted : {0}", s), Logger.LogLevel.Info);
                        
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "OnSendStatusChangeRequestIdelCompleted", exception, Logger.LogLevel.Error);
                    }
                };
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "SetAuth", exception, Logger.LogLevel.Error);
            }

        }



        #endregion
        #endregion
       
    }
}
