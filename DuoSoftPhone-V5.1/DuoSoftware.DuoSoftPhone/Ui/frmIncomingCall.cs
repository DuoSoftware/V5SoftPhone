using System;
using System.Windows.Forms;

namespace DuoSoftware.DuoSoftPhone.Ui
{
    public partial class frmIncomingCall : Form
    {
        private int Interval = 25;

        public bool IsCallAnswered;


        public frmIncomingCall(bool isNotAllowToReject, int reginTime = 60)
        {
            InitializeComponent();
            Interval = reginTime;

            buttonReject.Enabled = !isNotAllowToReject;
            rejectCallToolStripMenuItem.Enabled = !isNotAllowToReject;
        }

        private void frmIncomingCall_Load(object sender, EventArgs e)
        {
            this.ActiveControl = buttonAnswer;
            buttonAnswer.Focus();
            //this.ContextMenu = phoner8ClickMenu;
           
            timer1.Interval =(int) new TimeSpan(0, 0, Interval).TotalMilliseconds;
            timer1.Tick += (s, t) =>
            {
                timer1.Stop();
                timer1.Enabled = false;
                IsCallAnswered = false;
                this.Close();
            };
            timer1.Enabled = true;
            timer1.Start();

        }


        private void buttonAnswer_Click(object sender, EventArgs e)
        {
            IsCallAnswered = true; this.Close();
        }

        private void buttonReject_Click(object sender, EventArgs e)
        {
            IsCallAnswered = false; this.Close();
        }
        
    }
}
