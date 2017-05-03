using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DuoSoftware.DuoLogger;
using DuoSoftware.DuoSoftPhone.Controllers;
using DuoSoftware.DuoSoftPhone.Controllers.AgentStatus;
using DuoSoftware.DuoSoftPhone.Controllers.Service;
using DuoSoftware.DuoSoftPhone.refResourceProxy;
using VIBlend.Utilities;

namespace DuoSoftware.DuoSoftPhone.Ui
{
    public partial class Login : Form
    {
        private static Mutex mutex;
        public Login()
        {
            InitializeComponent();
        }

        private void button_login_Click(object sender, EventArgs e)
        {
            UserLogin();
        }

        private string GetLocalIPAddress()
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

        private Agent _agent;
        private void UserLogin()
        {
            try
            {
                
                var settingObject = System.Configuration.ConfigurationSettings.AppSettings;
                var company = settingObject["Company"];
                var auth = DuoAuth.DuoAuthorizationService.Login(txtUserName.Text.Trim(), txtPassword.Text.Trim(),
                    int.Parse(company), "DuoSoftPhone");

                if(auth==null)
                    throw new Exception("Fail to Login to Auth.");
                
                var result = ArdsHandler.ResourceRegistration(auth, GetLocalIPAddress());

                if (result.Command == WorkflowResultCode.ACDS101)
                {
                    result = ArdsHandler.SendStatusChangeRequestIdel(auth, result.SessionID);
                    if (result.Command == WorkflowResultCode.ACDS403 || result.Command == WorkflowResultCode.ACDS405)
                    {
                        result = ArdsHandler.SendModeChangeRequestInbound(auth);
                        if (result.Command == WorkflowResultCode.ACDS601 || result.Command == WorkflowResultCode.ACDS501)
                        {
                            var sip = ProfileManagementHandler.GetSipProfile(auth.SecurityToken, auth.guUserId);
                            if (string.IsNullOrEmpty(sip.userName))
                            {
                                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Fail to get SIP Profile info",Logger.LogLevel.Error);
                                throw new Exception("Fail to get SIP Profile info.");
                            }

                            // set agent status 
                            _agent = new Agent
                            {
                                AgentCurrentState = new AgentInitiate(),
                                AgentCurrentMode =AgentMode.Inbound,
                                SipProfile = sip,
                                AgentSessionId = SessionId.UniqueId(txtUserName.Text.Trim()),
                                UserAuth = auth,
                            };
                            var frm = new FormDialPad(auth,_agent.AgentSessionId , sip,_agent);
                            _agent.AgentCurrentState.OnLogOn(ref _agent, string.Empty);
                            Hide();
                            frm.ShowDialog(this);
                            this.Close();
                            Environment.Exit(0);
                            return;
                        }
                        else
                        {
                            Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Fail to set Agent Mode",
                                Logger.LogLevel.Error);
                            MessageBox.Show("Fail to set Agent Mode", "Duo Dialer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Fail to set Agent Status",
                            Logger.LogLevel.Error);
                        MessageBox.Show("Fail to set Agent Status", "Duo Dialer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Fail to ResourceRegistration",
                        Logger.LogLevel.Error);
                    MessageBox.Show("Resource Registration Fail", "Duo Dialer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                ArdsHandler.ResourceUnregistration(auth);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Login fail", exception, Logger.LogLevel.Error);
                MessageBox.Show("Login Fail", "Duo Dialer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            txtPassword.Text = string.Empty;
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            UserLogin();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            FillStyleGradientEx highlightGradient = new FillStyleGradientEx(Color.LightGreen, Color.GreenYellow, Color.Green, Color.DarkGreen, 90f, 0.2f, 0.3f);
            FillStyleGradientEx defaultGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.3f, 0.5f);
            FillStyleGradientEx pressedGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.4f, 0.5f);
            FillStyleGradientEx disabledGradient = new FillStyleGradientEx(Color.Silver, Color.Silver, Color.Silver, Color.Silver, 90f, 0.4f, 0.5f);
            ControlTheme theme = ControlTheme.GetDefaultTheme(VIBLEND_THEME.STEEL);
            theme.StyleHighlight.FillStyle = highlightGradient;
            theme.StyleDisabled.FillStyle = disabledGradient;
            theme.StylePressed.FillStyle = pressedGradient;
            theme.StyleNormal.FillStyle = defaultGradient;
            this.button_login.StyleKey = "answerStyle";
            this.button_login.Theme = theme;
            this.button_login.UseThemeTextColor = false;
            this.button_login.HighlightTextColor = Color.White;
            this.button_login.ForeColor = Color.White;
            this.button_login.PressedTextColor = Color.White;
            ActiveControl = txtUserName;
            txtUserName.Select();

            bool ok;

            //The name used when creating the mutex can be any string you want
            mutex = new Mutex(true, "DuoSoftPhone", out ok);

            if (!ok)
            {
                MessageBox.Show("Application Already Running", "Duo Soft Phone", MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification, false);
                Environment.Exit(100);
            }
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        
    }
}
