using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using DuoSoftware.DuoTools.DuoLogger;
using DuoSoftware.DuoSoftPhone.Controllers.Service;
using DuoSoftware.DuoSoftPhone.refResourceProxy;

namespace DuoSoftware.DuoSoftPhone.Ui
{
    public partial class Login : Form
    {
        
        private static Mutex mutex;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

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
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "GetLocalIPAddress", exception, Logger.LogLevel.Error);
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
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "IsValidIP", exception, Logger.LogLevel.Error);
                //Console.WriteLine(exception.Message);
                return false;
            }
        }

        private void UserLogin()
        {
            try
            {

                button_login.Enabled = false;
                //ProgressBar.Start();
                //ProgressBar.Show();
                var settingObject = System.Configuration.ConfigurationSettings.AppSettings;
                var company = settingObject["Company"];


                var auth = new refUserAuth.IauthClient().login(txtUserName.Text.Trim(), txtPassword.Text.Trim(),
                    int.Parse(company), "DuoSoftPhone");



                var sip = ProfileManagementHandler.GetSipProfile(auth.SecurityToken, auth.guUserId);
                if (sip == null)
                {
                    button_login.Enabled = true;
                    //ProgressBar.Stop();
                    //ProgressBar.Hide();
                    txtPassword.Text = string.Empty;
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Fail to Get SIP Profile", Logger.LogLevel.Error);
                    MessageBox.Show("Fail to Get SIP Profile", "Duo Dialer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var id = Guid.NewGuid().ToString();
                var callBack = new ResourceHandler(auth.SecurityToken, auth.TenantID, auth.CompanyID);
                var ip = GetLocalIPAddress();

                callBack.OnResourceRegistrationCompleted += (r) =>
                {
                    #region Resource Registration

                    this.Invoke(new MethodInvoker(delegate
                    {
                        button_login.Enabled = true;
                        //ProgressBar.Stop();
                        //ProgressBar.Hide();
                        txtPassword.Text = string.Empty;


                        switch (r.Command)
                        {
                            case WorkflowResultCode.ACDS101: //- Agent sucessfully registered (ACDS101)
                                Hide();
                                new FormDialPad(auth, id, sip, ip).ShowDialog(this);
                                this.Close();
                                Environment.Exit(0);
                                break;

                            case WorkflowResultCode.ACDE101: //- Agent already registered with different IP (ACDE101)
                                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Agent already registered with different IP-ARDS Code : ACDE101", Logger.LogLevel.Info);
                                MessageBox.Show("Agent already registered with different IP", "Duo Dialer", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                break;

                            default:
                                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Login Fail-- ARDS not allow to Login. ARDS Code : " + r.Command, Logger.LogLevel.Info);
                                MessageBox.Show("Login Fail", "Duo Dialer", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                break;
                        }
                    }));

                    #endregion
                };

                callBack.ResourceRegistration(auth, ip);
            }
            catch (Exception exception)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    button_login.Enabled = true;
                    //ProgressBar.Stop();
                    //ProgressBar.Hide();
                    txtPassword.Text = string.Empty;
                }));
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Login fail", exception, Logger.LogLevel.Error);
                MessageBox.Show("Login Fail", "Duo Dialer", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }


        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            UserLogin();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            

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
            button_login.Enabled = true;
            //ProgressBar.Hide();
        }

        private void Login_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            }
            catch (Exception exception)
            {

            }
        }

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        
    }
}
