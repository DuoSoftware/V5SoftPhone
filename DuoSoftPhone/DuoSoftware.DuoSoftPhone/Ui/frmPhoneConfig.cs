using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DuoSoftware.DuoAuth.RefAuth.Iauth;
using DuoSoftware.DuoSoftPhone.Controllers;
using DuoSoftware.DuoSoftPhone.Controllers.Service;

namespace DuoSoftware.DuoSoftPhone.Ui
{
    public partial class frmPhoneConfig : Form
    {
        public ProfileInfo SipProfile;
        private UserAuth Auth;
        public frmPhoneConfig(UserAuth auth)
        {
            Auth = auth;
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPhoneConfig_Load(object sender, EventArgs e)
        {
            SipProfile = ProfileManagementHandler.GetSipProfile(Auth.SecurityToken, Auth.guUserId) ;//?? AbstractPhoneController.InitDictionary;// ?? XMLHandler.ReadXML();
            txtuserName.Text = SipProfile.userName;
            txtPassword.Text = SipProfile.password;
            txtDisolayName.Text = SipProfile.displayName;
            txtauthName.Text = SipProfile.authName;
            txtlocalPort.Text = SipProfile.localPort.ToString();
            txtserverPort.Text = SipProfile.serverPort.ToString();
            txtHostName.Text = SipProfile.hostName;

        }
    }
}
