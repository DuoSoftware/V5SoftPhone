using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoTools.DuoLogger;
using Infragistics.Win.UltraWinExplorerBar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DuoSoftware.DuoSoftPhone.UI
{
    public partial class FrmCallLogs : Form
    {
        //Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup CallLogsGroup = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup();
        public delegate void NumberSelected(string phnNo);

        public event NumberSelected OnNumberSelect;

        private const int WmNclbuttondown = 0xA1;
        private const int HtCaption = 0x2;

        [DllImportAttribute("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ReleaseCapture();

        public FrmCallLogs(Dictionary<Guid, CallLog> callLogs)
        {
            InitializeComponent();

            if (callLogs == null) return;
            foreach (var callLog in callLogs)
            {
                switch (callLog.Value.Direction)
                {
                    case 0:
                        AddIncommingToCallLogs(callLog.Value.PhoneNo, callLog.Value.Time, callLog.Value.Durations);
                        break;

                    case 1:
                        AddOutgoingCallToCallLogs(callLog.Value.PhoneNo, callLog.Value.Time, callLog.Value.Durations);
                        break;

                    case 2:
                        AddMissCallToCallLogs(callLog.Value.PhoneNo, callLog.Value.Time, callLog.Value.Durations);
                        break;
                }
            }

            //AddIncommingToCallLogs("1234567890", DateTime.Now, 1236);
            //AddOutgoingCallToCallLogs("1234567890", DateTime.Now, 1236);
            //AddMissCallToCallLogs("1234567890", DateTime.Now, 1236);
        }

        private void AddIncommingToCallLogs(string no, DateTime time, double durations)
        {
            try
            {
                var appearance = new Infragistics.Win.Appearance
                {
                    Image = global::DuoSoftware.DuoSoftPhone.Properties.Resources.callLogincomming,
                    ForeColor = Color.Green,
                };

                var callLogsGroup = this.EBarCallInfo.Groups["CallLogs"];

                var ebItem = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem { Key = no, Text = string.Format("{0} - {1} - {2}", no, durations, time.ToString("d/M/yy HH:mm:ss")) };
                ebItem.Settings.AppearancesSmall.Appearance = appearance;
                //callLogsGroup.Items.Add(ebItem);
                callLogsGroup.Items.Insert(1, ebItem);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "AddIncommingToCallLogs", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        private void AddOutgoingCallToCallLogs(string no, DateTime time, double durations)
        {
            try
            {
                var appearance = new Infragistics.Win.Appearance
                {
                    Image = Properties.Resources.calloutgoing,
                    ForeColor = Color.DarkSlateBlue,
                };
                var callLogsGroup = this.EBarCallInfo.Groups["CallLogs"];

                var ebItem = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem { Key = no, Text = string.Format("{0} - {1} - {2}", no, durations, time.ToString("d/M/yy HH:mm:ss")) };
                ebItem.Settings.AppearancesSmall.Appearance = appearance;
                //callLogsGroup.Items.Add(ebItem);
                callLogsGroup.Items.Insert(1, ebItem);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "AddOutgoingCallToCallLogs", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        private void AddMissCallToCallLogs(string no, DateTime time, double durations)
        {
            try
            {
                var appearance = new Infragistics.Win.Appearance
                {
                    Image = Properties.Resources.MissCallCallLog,
                    ForeColor = Color.DarkRed,
                };
                var callLogsGroup = this.EBarCallInfo.Groups["CallLogs"];

                var ebItem = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem { Key = no, Text = string.Format("{0} - {1} - {2}", no, durations, time.ToString("d/M/yy HH:mm:ss")) };

                ebItem.Settings.AppearancesSmall.Appearance = appearance;
                //callLogsGroup.Items.Add(ebItem);
                callLogsGroup.Items.Insert(1, ebItem);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "AddMissCallToCallLogs", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        private void ExplorerBarAgent_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                var ebar = ((UltraExplorerBar)(sender));
                if (ebar.ActiveItem == null) return;
                if (ebar.ActiveItem.Key.Equals("0000000000"))
                    return;
                if (OnNumberSelect != null)
                    OnNumberSelect(ebar.ActiveItem.Key);
                //textBox1.Text = ebar.ActiveItem.Key;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ExplorerBarAgent_MouseDoubleClick", exception, Logger.LogLevel.Error);
            }
        }

        private void ExplorerBarAgent_ItemRemoving(object sender, Infragistics.Win.UltraWinExplorerBar.CancelableItemEventArgs e)
        {
            e.Cancel = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EBarCallInfo_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button != MouseButtons.Left) return;
                ReleaseCapture();
                SendMessage(Handle, WmNclbuttondown, HtCaption, 0);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "EBarCallInfo_MouseDown", exception, Logger.LogLevel.Error);
            }
        }

        private void EBarCallInfo_DragOver(object sender, DragEventArgs e)
        {
        }

        private void EBarCallInfo_ItemDoubleClick(object sender, ItemEventArgs e)
        {
        }
    }
}