using System;
using System.Windows.Forms;

namespace DuoSoftware.DuoSoftPhone.UI
{
    public partial class FrmIncomingCall : Form
    {
        private readonly int _interval;

        public bool IsCallAnswered;

        public FrmIncomingCall(bool isNotAllowToReject, int reginTime = 60)
        {
            InitializeComponent();
            _interval = reginTime;

            buttonReject.Enabled = !isNotAllowToReject;
            rejectCallToolStripMenuItem.Enabled = !isNotAllowToReject;
        }

        private void frmIncomingCall_Load(object sender, EventArgs e)
        {
            ActiveControl = buttonAnswer;
            buttonAnswer.Focus();
            //this.ContextMenu = phoner8ClickMenu;

            timer1.Interval = (int)new TimeSpan(0, 0, _interval).TotalMilliseconds;
            timer1.Tick += (s, t) =>
            {
                timer1.Stop();
                timer1.Enabled = false;
                IsCallAnswered = false;
                Close();
            };
            timer1.Enabled = true;
            timer1.Start();
        }

        private void buttonAnswer_Click(object sender, EventArgs e)
        {
            IsCallAnswered = true; Close();
        }

        private void buttonReject_Click(object sender, EventArgs e)
        {
            IsCallAnswered = false; Close();
        }
    }
}