using DuoSoftware.DuoSoftPhone.Controllers.Service;
using DuoSoftware.DuoSoftPhone.RefUserAuth;
using DuoSoftware.DuoTools.DuoLogger;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DuoSoftware.DuoSoftPhone.UI
{
    public partial class FrmPhoneConfig : Form
    {
        private Dictionary<string, object> initDictionary = new Dictionary<string, object>();
        private UserAuth Auth;

        public FrmPhoneConfig(UserAuth auth)
        {
            Auth = auth;
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //XMLHandler.WriteXML(txtlocalPort.Text,txtHostName.Text,txtserverPort.Text,txtuserName.Text,txtPassword.Text,txtDisolayName.Text,txtauthName.Text);

            //initDictionary["userName"] = txtuserName.Text;
            //initDictionary["password"] = txtPassword.Text;
            //initDictionary["displayName"] = txtDisolayName.Text;
            //initDictionary["authName"] = txtauthName.Text;
            //initDictionary["localPort"] = txtlocalPort.Text;
            //initDictionary["serverPort"] = txtserverPort.Text;
            //initDictionary["hostName"] = txtHostName.Text;

            this.Close();
        }

        private void frmPhoneConfig_Load(object sender, EventArgs e)
        {
            try
            {
                initDictionary = ProfileManagementHandler.GetSipProfile(Auth.SecurityToken, Auth.guUserId);// ?? XMLHandler.ReadXML();
                txtuserName.Text = (string)initDictionary["userName"];
                txtPassword.Text = (string)initDictionary["password"];
                txtDisolayName.Text = (string)initDictionary["displayName"];
                txtauthName.Text = (string)initDictionary["authName"];
                txtlocalPort.Text = initDictionary["localPort"].ToString();
                txtserverPort.Text = initDictionary["serverPort"].ToString();
                txtHostName.Text = (string)initDictionary["hostName"];
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "frmPhoneConfig_Load", exception, Logger.LogLevel.Error);
            }
        }
    }
}