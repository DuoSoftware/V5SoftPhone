using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using DuoCallTesterLicenseKey;
using DuoSoftware.CommonTools.Security;
using DuoSoftware.DuoLogger;
using DuoSoftware.DuoSoftPhone.Controllers;
using DuoSoftware.DuoSoftPhone.Controllers.AgentStatus;
using DuoSoftware.DuoSoftPhone.Controllers.CallStatus;
using DuoSoftware.DuoSoftPhone.Controllers.Service;
using DuoSoftware.DuoSoftPhone.Model;
using DuoSoftware.DuoSoftPhone.refResourceProxy;
using Infragistics.Win;
using Infragistics.Win.Misc;
using PortSIP;
using TheCodeKing.Net.Messaging;
using AutoCompleteMode = System.Windows.Forms.AutoCompleteMode;
using System.Data;
using System.Diagnostics;
using Timer = System.Timers.Timer;

namespace DuoSoftware.DuoSoftPhone.View
{
    public partial class FormDialPad : Form, SIPCallbackEvents, IUiState
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );

        #region Property
        private ResourceMonitoringHandler resMonitoringHandler;
        private bool IsBreakRequest;
        private bool isCallAnswerd;
        private IXDListener listner;
        private SoundPlayer _wavPlayer;
        private SoundPlayer _wavPlayerRingIn;

        private bool playingRingIntone;
        private bool playRingtone;
        private DateTime callStarTime;
        private DateTime freezeStarTime;
        private System.Timers.Timer CallDurations;
        private System.Timers.Timer FreezeDurations;
        DataTable CallLog = new DataTable("Table");
        private TextBox textBoxNumber;
        private AutoCompleteStringCollection source = new AutoCompleteStringCollection();
        

        private static Mutex mutex;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private Agent agent;
        private Call call;

        private string callSessionId;

        private int acwTime;
        private int acwCotdown;
        private System.Timers.Timer acwCutdownTimer;
        private PortSIPLib phoneController;
        private bool _SIPLogined;
        private XMLHandler audioHandler;


        #endregion


        public FormDialPad()
        {
            InitializeComponent();

            //Needed to make the custom shaped Form
            this.FormBorderStyle = FormBorderStyle.None;
            this.Width = this.BackgroundImage.Width;
            this.Height = this.BackgroundImage.Height;

            //Slow version
            //this.Region = BitmapToRegion.getRegion((Bitmap)this.BackgroundImage, Color.FromArgb(0, 255, 0), 100);
            //this.Region = BitmapToRegion.getRegion((Bitmap)this.BackgroundImage, Color.Blue, 100);
            //Fast version
            this.Region = BitmapToRegion.getRegionFast((Bitmap)this.BackgroundImage, Color.Gold, 100);


            audioHandler = new XMLHandler();
            
        }

        private void FormDialPad_Load(object sender, EventArgs e)
        {


            try
            {
                bool ok;

                //The name used when creating the mutex can be any string you want
                mutex = new Mutex(true, "DuoSoftPhone", out ok);

                if (!ok)
                {
                    MessageBox.Show("Application Already Running", "Duo Soft Phone", MessageBoxButtons.OK, MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification, false);
                    Environment.Exit(100);
                }

                //Create three columns that will hold sample data.
                DataColumn Directions = new DataColumn("Directions", typeof(Bitmap));
                DataColumn Duration = new DataColumn("Duration", typeof(double));
                DataColumn Id = new DataColumn("Id", typeof(Guid));
                DataColumn Time = new DataColumn("Time", typeof(string));
                DataColumn Number = new DataColumn("Number", typeof(string));
                DataColumn CallStartTime = new DataColumn("CallStartTime", typeof(DateTime));

                //Add the three columns to the table.
                CallLog.Columns.AddRange(new DataColumn[] { Directions, Duration, Id, Time, Number });


                ActiveControl = txtUserName;
                txtUserName.Select();




                // Create and initialize the text box. //64, 64, 64
                textBoxNumber = new TextBox
                {
                    BackColor = Color.DarkGray,// System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64))))),
                    BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                    Font =
                        new System.Drawing.Font("Calibri", 16F, System.Drawing.FontStyle.Bold,
                            System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    //ForeColor = System.Drawing.Color.White,
                    ForeColor = System.Drawing.Color.White,
                    Location = new System.Drawing.Point(5, 4),//47
                    Name = "textBoxNumber",
                    Size = new System.Drawing.Size(220, 33),//21 230
                    TabIndex = 78,
                    AutoCompleteCustomSource = source,
                    AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                    AutoCompleteSource = AutoCompleteSource.CustomSource,
                    Visible = true,
                    TextAlign = HorizontalAlignment.Center,
                };

                textBoxNumber.KeyDown += (d, k) =>
                {
                    try
                    {
                        if (k.KeyCode != Keys.Enter) return;
                        MakeCall(textBoxNumber.Text);
                    }
                    catch (Exception exception)
                    {
                       Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "textBoxNumber.KeyDown", exception,
                    Logger.LogLevel.Error);
                    }
                };

                GBInfo.Controls.Add(textBoxNumber);

                textBoxNumber.AutoCompleteCustomSource = source;
                textBoxNumber.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                textBoxNumber.AutoCompleteSource = AutoCompleteSource.CustomSource;
                
                #region load music file

                var settingObject = System.Configuration.ConfigurationSettings.AppSettings;
                var filePath = settingObject["RingToneFilePath"];
                var ringtone = settingObject["PlayRingtone"].ToLower();
                if (!File.Exists(filePath))
                {
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, string.Format("PlayRingTone> Cannot Find File {0}", filePath), Logger.LogLevel.Error);
                    if (ringtone.Equals("1") || ringtone.Equals("true"))
                    {
                        filePath = string.Format(@"{0}\{1}", Application.StartupPath, "Ringtone.wav");
                        if (File.Exists(filePath))
                            Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "PlayRingTone> Play with default ring tone", Logger.LogLevel.Info);
                    }
                }
                playRingtone = File.Exists(filePath) && (ringtone.Equals("1") || ringtone.Equals("true"));
                var ringInfilePath = settingObject["RingInToneFilePath"];
                if (!File.Exists(ringInfilePath))
                {
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, string.Format("PlayRingINTone> Cannot Find File {0}", ringInfilePath), Logger.LogLevel.Error);
                    ringInfilePath = string.Format(@"{0}\{1}", Application.StartupPath, "RingIntone.wav");
                    if (File.Exists(ringInfilePath))
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "PlayRingINTone> Play with default ringIn tone", Logger.LogLevel.Info);
                }

                _wavPlayer = new SoundPlayer
                {
                    SoundLocation = filePath,
                    // @"C:\Users\Public\Music\Sample Music\ALBSlide.wav"
                };
                _wavPlayer.LoadCompleted += (d, f) => Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "wavPlayer_LoadCompleted", Logger.LogLevel.Info);
                _wavPlayer.LoadAsync();

                _wavPlayerRingIn = new SoundPlayer
                {
                    SoundLocation = ringInfilePath,
                    // @"C:\Users\Public\Music\Sample Music\ALBSlide.wav"
                };
                _wavPlayerRingIn.LoadCompleted += (d, f) => Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "wavPlayer_LoadCompleted", Logger.LogLevel.Info);
                _wavPlayerRingIn.LoadAsync();

                #endregion

                #region Tappi

                listner = XDListener.CreateListener(XDTransportMode.IOStream, false);


                listner.RegisterChannel("Command");

                listner.MessageReceived += (o, e1) =>
                {
                    try
                    {
                        if (e1.DataGram.Channel == "Command")
                        {
                            string cli = e1.DataGram.Message;
                            txtStatus.Invoke(new MethodInvoker(delegate
                            {
                                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault,
                                    string.Format("Recive no : {0} frm Tappi", cli), Logger.LogLevel.Info);
                                txtStatus.Text = Environment.NewLine + "Tappi System try to Dial Call.";
                                //mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Tappi System try to Dial Call.",ToolTipIcon.Info);
                                if (agent.AgentCurrentState.GetType() == typeof(AgentIdle) && call.CallCurrentState.GetType() == typeof(CallIdleState))
                                {
                                    textBoxNumber.Text = cli;
                                    MakeCall(cli);
                                }
                                else
                                {
                                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "listner.MessageReceived Fail to Make calls. Agent Invalid State.", Logger.LogLevel.Error);
                                }
                            }));
                        }
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "listner.MessageReceived", exception, Logger.LogLevel.Error);
                    }
                };

                #endregion

                #region Set Ui Locations

                TilePhoneLog.Location = new Point(17, 51);
                TileInCall.Location = new Point(17, 51);
                TilePhoneMain.Location = new Point(17, 51);
                TileDialling.Location = new Point(17, 51);
                TileIncommingCall.Location = new Point(17, 51);
                TilePhoneOffline.Location = new Point(17, 51);
                TilePhoneSetting.Location = new Point(17, 51);
                TilePhoneCallLogs.Location = new Point(17, 51);
                TilePhoneAcw.Location = new Point(17, 51);

                TilePhoneLog.Visible = true;
                TileInCall.Visible = false;
                TilePhoneMain.Visible = false;
                TileDialling.Visible = false;
                TileIncommingCall.Visible = false;
                TilePhoneOffline.Visible = false;
                TilePhoneSetting.Visible = false;
                TilePhoneCallLogs.Visible = false;
                TilePhoneAcw.Visible = false;

                var imgPath = string.Format(@"{0}\{1}", Application.StartupPath, "Log250X50pix.png");
                if (File.Exists(imgPath))
                {
                    LogoDisplay.ImageLocation = imgPath;
                    LogoDisplay.AutoSize = false;
                    LogoDisplay.Padding = new Padding(2, 2, 2, 2);

                    LogoDisplay1.ImageLocation = imgPath;
                    LogoDisplay1.AutoSize = false;
                    LogoDisplay1.Padding = new Padding(2, 2, 2, 2);

                    LogoDisplay2.ImageLocation = imgPath;
                    LogoDisplay2.AutoSize = false;
                    LogoDisplay2.Padding = new Padding(2, 2, 2, 2);

                    LogoDisplay3.ImageLocation = imgPath;
                    LogoDisplay3.AutoSize = false;
                    LogoDisplay3.Padding = new Padding(2, 2, 2, 2);

                    LogoDisplay4.ImageLocation = imgPath;
                    LogoDisplay4.AutoSize = false;
                    LogoDisplay4.Padding = new Padding(2, 2, 2, 2);

                    LogoDisplay5.ImageLocation = imgPath;
                    LogoDisplay5.AutoSize = false;
                    LogoDisplay5.Padding = new Padding(2, 2, 2, 2);

                    LogoDisplay6.ImageLocation = imgPath;
                    LogoDisplay6.AutoSize = false;
                    LogoDisplay6.Padding = new Padding(2, 2, 2, 2);

                    LogoDisplay7.ImageLocation = imgPath;
                    LogoDisplay7.AutoSize = false;
                    LogoDisplay7.Padding = new Padding(2, 2, 2, 2);
                }

                #endregion
                CallDurations = new System.Timers.Timer(TimeSpan.FromSeconds(1).TotalSeconds);
                CallDurations.Elapsed += (s, e1) =>
                {
                    var ts = e1.SignalTime.Subtract(callStarTime);
                    var elapsedTime = ts.Hours > 0 ? String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds) : String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);

                    this.Invoke(new MethodInvoker(delegate
                    {
                        if (ts.Hours > 0)
                            txtDurations.Location = new Point(63, 12);
                        txtDurations.Text = elapsedTime;
                    }));

                };

                FreezeDurations = new System.Timers.Timer(TimeSpan.FromSeconds(1).TotalSeconds);
                FreezeDurations.Elapsed += (s, e1) =>
                {
                    var ts = e1.SignalTime.Subtract(freezeStarTime);
                    var elapsedTime = ts.Hours > 0 ? String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds) : String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);

                    this.Invoke(new MethodInvoker(delegate
                    {
                        //if (ts.Hours > 0)
                        //    txtDurations.Location = new Point(63, 12);
                        LabelCountdown.Text = elapsedTime;
                    }));

                };


            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "FormDialPad_Load", exception,
                    Logger.LogLevel.Error);
                MessageBox.Show("Critical Error. Please Contact your System Administrator.", "Duo Dialer",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        #region Form Event

        private void btnAgentListClose_Click(object sender, EventArgs e)
        {
            PanelAgentList.Visible = false;
        }

        private void btnLoadAgentList_Click(object sender, System.EventArgs e)
        {
            try
            {
                resMonitoringHandler.GetOnlineIdleAgentList(agent.Auth.SecurityToken, agent.Auth.guUserId.ToString());
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "btnLoadAgentList_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void agentListToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                PanelAgentList.Visible = !PanelAgentList.Visible;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "agentListToolStripMenuItem_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void GridAgentList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                PanelAgentList.Visible = false;
                if (GridAgentList.RowCount > 0)
                    textBoxNumber.Text = GridAgentList["Extention", e.RowIndex].Value.ToString(); // GridAgentList.Rows[e.RowIndex].Cells["Extention"].Value.ToString();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "GridAgentList_CellDoubleClick", exception, Logger.LogLevel.Error);
            }
        }

        private void btnConference_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, string.Format("Conference_Click-> Session Id : {0} , Status : {1}",call.CallSessionId, call.CallCurrentState), Logger.LogLevel.Info);
            //    if (call.CallCurrentState.GetType() == typeof(CallAgentClintConnectedState))
            //    {
            //        var res = phoneController.sendInfo(call.portSipSessionId, "text", "plain", "conference");
            //        if (res == 0)
            //            call.CallActions = CallActions.Conference_Call_Requested;
            //        txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
            //        txtStatus.Text = Environment.NewLine + (res != 0 ? "Conference Failed." : "Conference Call...");
            //    }
            //}
            //catch (Exception exception)
            //{
            //    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            //}
        }

        private void btnEtl_Click(object sender, EventArgs e)
        {

        }

        private void btnHold_Click(object sender, EventArgs e)
        {
            Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, string.Format("Hold_Click-> Session Id : {0} , Status : {1}", call.CallSessionId, call.CallCurrentState), Logger.LogLevel.Info);
            HoldUnholdCall();
        }

        private void openLogFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenLogFils();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "openLogfiles_Click", exception,
                                    Logger.LogLevel.Error);
            }
        }

        private void openLogFileDicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenLogFilsDirectory();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "openLogDic_Click", exception,
                    Logger.LogLevel.Error);
            }
        }
        
        private void AutoAnswer_Click(object sender, EventArgs e)
        {
            AutoAnswer.Checked = !AutoAnswer.Checked;
        }

        private void accountSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reregister();
        }

        private void volumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                loadDevices();
                ShowSetting();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "volumeToolStripMenuItem_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void breakRequestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IsBreakRequest = true;
                agent.AgentCurrentState.OnRequestAgentBreak(ref agent,call.CallSessionId, "Oficial Break");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "BreakToolStripMenuItem_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void cancelRequestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                agent.AgentCurrentState.OnRequestAgentBreakCancel(ref agent, call.CallSessionId);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "CancelRequestmenuItem_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void endBreakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                agent.AgentCurrentState.OnEndBreak(ref agent);
                endBreakToolStripMenuItem.Enabled = false;
                //officialBreakToolStripMenuItem.Enabled = true;
                //mealBreakToolStripMenuItem.Enabled = true;
                //aUXBreakToolStripMenuItem.Enabled = true;
                txtStatus.Text = "";
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "EndBreakmenuItem_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void onToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnableRingtone();
            onToolStripMenuItem.Checked = true;
            offToolStripMenuItem.Checked = !onToolStripMenuItem.Checked;
        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisableRingtone();
            onToolStripMenuItem.Checked = false;
            offToolStripMenuItem.Checked = !onToolStripMenuItem.Checked;
        }

        private void answerCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MakeCall(textBoxNumber.Text);
        }

        private void rejectCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EndCall();
        }

        private void holdCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HoldUnholdCall();
        }

        private void btnOffline_Click(object sender, EventArgs e)
        {
            try
            {
                agent.AgentCurrentState.OnLogOn(ref agent);
                InitializePhone();
                btnOffline.Text = btnOffline.Text + "\n [Exceed Max try count.]";
                btnOffline.Enabled = false;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "OFFLINE_Click", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void btnOffline_MouseLeave(object sender, EventArgs e)
        {
            btnOffline.BorderStyleInner = UIElementBorderStyle.None;
            btnOffline.BorderStyleOuter = UIElementBorderStyle.None;
        }
        
        private void btnpadDial_Click(object sender, EventArgs e)
        {
            TransferCall();
        }

        private void btnTransferCall_Click(object sender, EventArgs e)
        {
            txtNumberpad.Text = "";
            btnDialpad.Enabled = true;
            GBCallIn.SendToBack();
            //GBCallIn.Visible = false;

            DialPadFrmPanel.Visible = true;
            DialPadFrmPanel.BringToFront();

        }

       

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        
        private void btnSwapCall_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, string.Format("swapCall_Click-> Session Id : {0} , Status : {1}", call.CallSessionId, call.CallCurrentState), Logger.LogLevel.Info);
                if (call.CallCurrentState.GetType()==typeof(CallAgentClintConnectedState)|| call.CallCurrentState.GetType()==typeof(CallAgentSupConnectedState))
                {
                    var res = phoneController.sendInfo(call.portSipSessionId, "text", "plain", "swap");
                    if (res == 0)
                        call.CallCurrentState.OnSwapReq(ref call, CallActions.Call_Swap_Requested);
                    txtStatus.Text = Environment.NewLine + (res != 0 ? "Swap Failed." : "Swap Call...");
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void btnEndCall_Click(object sender, EventArgs e)
        {
            EndCall();
        }

        private void btnCallLog_Click(object sender, EventArgs e)
        {
            ShowCallLogs();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            if (agent.AgentCurrentState.GetType() == typeof (AgentIdle))
                InIdleState();
        }

        private void TrackBarSpeaker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                phoneController.setSpeakerVolume(TrackBarSpeaker.Value);
                audioHandler.WriteXML(ComboBoxMicrophones.SelectedIndex, TrackBarMicrophone.Value, ComboBoxSpeakers.SelectedIndex, ComboBoxSpeakers.SelectedItem.ToString(), TrackBarSpeaker.Value, ComboBoxMicrophones.SelectedItem.ToString());
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "TrackBarSpeaker_ValueChanged", exception, Logger.LogLevel.Error);
            }
        }

        private void TrackBarMicrophone_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                phoneController.setMicVolume(TrackBarMicrophone.Value);
                audioHandler.WriteXML(ComboBoxMicrophones.SelectedIndex, TrackBarMicrophone.Value, ComboBoxSpeakers.SelectedIndex, ComboBoxSpeakers.SelectedItem.ToString(), TrackBarSpeaker.Value, ComboBoxMicrophones.SelectedItem.ToString());
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "TrackBarMicrophone_ValueChanged", exception, Logger.LogLevel.Error);
            }
        }

        private void CheckBoxMute_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                phoneController.muteMicrophone(CheckBoxMute.Checked);
                picMic.Visible = CheckBoxMute.Checked;

                if (!chkboxMuteSpeaker.Checked)
                    phoneController.setMicVolume(phoneController.getMicVolume());
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "AudioWiz MuteMicrophone - " + CheckBoxMute.Checked, Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "CheckBoxMute_CheckedChanged", exception, Logger.LogLevel.Error);
            }
        }

        private void chkboxMuteSpeaker_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                phoneController.muteSpeaker(chkboxMuteSpeaker.Checked);
                picSpek.Visible = chkboxMuteSpeaker.Checked;
                if (!chkboxMuteSpeaker.Checked)
                    phoneController.setSpeakerVolume(phoneController.getMicVolume());

                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "AudioWiz MuteSpeaker - " + chkboxMuteSpeaker.Checked, Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "chkboxMuteSpeaker_CheckedChanged", exception, Logger.LogLevel.Error);
            }
        }
        private void btnTestAudio_Click(object sender, EventArgs e)
        {

            try
            {
                if (btnTestAudio.Text == "Test Audio")
                {
                    btnTestAudio.Text = "Stop Test";
                    phoneController.audioPlayLoopbackTest(true);
                }
                else
                {
                    btnTestAudio.Text = "Test Audio";
                    phoneController.audioPlayLoopbackTest(false);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "ButtonTestAudio_Click", exception, Logger.LogLevel.Error);
            }
        }

        #region Key Pad

        private void btnKey_MouseEnter(object sender, EventArgs e)
        {
            var obj = (UltraLabel)sender;
            obj.BorderStyleInner = UIElementBorderStyle.Rounded1;
            obj.BorderStyleOuter = UIElementBorderStyle.Rounded4Thick;
        }

        private void PanelACW_MouseEnter(object sender, EventArgs e)
        {
            btnFreez.BorderStyleInner = UIElementBorderStyle.None;
            btnFreez.BorderStyleOuter = UIElementBorderStyle.None;
        }

        private void DialPad_MouseEnter(object sender, EventArgs e)
        {
            foreach (var control in DialPadPanel.Controls)
            {
                if (control.GetType() == typeof(UltraLabel))
                {
                    var obj = (UltraLabel)control;
                    obj.BorderStyleInner = UIElementBorderStyle.None;
                    obj.BorderStyleOuter = UIElementBorderStyle.None;
                }

            }
        }

        private void GBIncommng_MouseEnter(object sender, EventArgs e)
        {
            foreach (var control in GBIncommng.Controls)
            {
                if (control.GetType() == typeof(UltraLabel))
                {
                    var obj = (UltraLabel)control;
                    obj.BorderStyleInner = UIElementBorderStyle.None;
                    obj.BorderStyleOuter = UIElementBorderStyle.None;
                }

            }
        }

        private void GBLogfrm_MouseEnter(object sender, EventArgs e)
        {
            foreach (var control in GBLogfrm.Controls)
            {
                if (control.GetType() == typeof(UltraLabel))
                {
                    var obj = (UltraLabel)control;
                    obj.BorderStyleInner = UIElementBorderStyle.None;
                    obj.BorderStyleOuter = UIElementBorderStyle.None;
                }

            }
        }

        private void GBInCallDialpad_MouseEnter(object sender, EventArgs e)
        {
            foreach (var control in GBInCallDialpad.Controls)
            {
                if (control.GetType() == typeof(UltraLabel))
                {
                    var obj = (UltraLabel)control;
                    obj.BorderStyleInner = UIElementBorderStyle.None;
                    obj.BorderStyleOuter = UIElementBorderStyle.None;
                }

            }
        }

        private void GBDialling_MouseEnter(object sender, EventArgs e)
        {
            foreach (var control in GBDialling.Controls)
            {
                if (control.GetType() == typeof(UltraLabel))
                {
                    var obj = (UltraLabel)control;
                    obj.BorderStyleInner = UIElementBorderStyle.None;
                    obj.BorderStyleOuter = UIElementBorderStyle.None;
                }

            }
        }

        private void GBCallIn_MouseEnter(object sender, EventArgs e)
        {
            foreach (var control in GBCallIn.Controls)
            {
                if (control.GetType() == typeof(UltraLabel))
                {
                    var obj = (UltraLabel)control;
                    obj.BorderStyleInner = UIElementBorderStyle.None;
                    obj.BorderStyleOuter = UIElementBorderStyle.None;
                }

            }
        }

        private void btnDial_Click(object sender, EventArgs e)
        {
           MakeCall(textBoxNumber.Text);
            
        }

        private void btnKey1_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text += "1";
                SendDTMF(1);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void btnKey2_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text += "2";
                
                    SendDTMF(2);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void btnKey3_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text += "3";
                
                    SendDTMF(3);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void btnKey4_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text += "4";
                
                    SendDTMF(4);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void btnKey5_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text += "5";
                
                    SendDTMF(5);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void btnKey6_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text += "6";
                
                    SendDTMF(6);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void btnKey7_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text += "7";
                
                    SendDTMF(7);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }

        }

        private void btnKey8_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text += "8";
                
                    SendDTMF(8);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void btnKey9_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text += "9";
                
                    SendDTMF(9);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void btnKeyStar_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text += "*";
                
                    SendDTMF(10);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void btnKey0_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text += "0";
                
                    SendDTMF(0);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void btnKeyHash_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text += "#";
                
                    SendDTMF(11);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxNumber.Text != null)
                {
                    if (textBoxNumber.Text.Length != 0)
                    {
                        var index = textBoxNumber.SelectionStart - 1;
                        if (index >= 0)
                        {
                            textBoxNumber.Text = textBoxNumber.Text.Remove(index, 1);
                            textBoxNumber.SelectionStart = index;
                            textBoxNumber.Focus();
                        }
                        // textBoxNumber.Text.Substring(0, textBoxNumber.Text.Length - 1);
                    }

                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void btnDialpad_Click(object sender, EventArgs e)
        {

        }

        private void btnDialPadClose_Click(object sender, EventArgs e)
        {

        }

        private void btnFreez_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnFreez.Text.Equals("Freeze"))
                {
                    LabelCountdown.Location = new Point(-11, 104);
                    LabelCountdown.Appearance.FontData.SizeInPoints = 65;
                    freezeStarTime = DateTime.Now;
                    FreezeACWTime();
                    txtStatus.Text = "Freeze";
                    btnFreez.Text = "End Freeze";
                    FreezeDurations.Enabled = true;
                    FreezeDurations.Start();
                }
                else
                {
                    LabelCountdown.Text = "";
                    LabelCountdown.Location = new Point(69, 104);
                    LabelCountdown.Appearance.FontData.SizeInPoints = 80;
                    txtStatus.Text = "";
                    btnFreez.Text = "Freeze";
                    FreezeDurations.Stop();
                    FreezeDurations.Enabled = false;
                    textBoxNumber.Text = string.Empty;
                    agent.AgentCurrentState.OnEndCallLog(ref agent, call.CallSessionId);
                }

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "btnFreez_Click", exception, Logger.LogLevel.Error);
            }
        }

        #endregion

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            Login();
        }

        

        private void FormDialPad_MouseDown(object sender, MouseEventArgs e)
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login();
        }

        
        #endregion

        #region UI State

        public void ShowCallLogs()
        {
            try
            {

                this.Invoke(new MethodInvoker(delegate
                {
                    TilePhoneMain.Visible = false;
                    TileIncommingCall.Visible = false;
                    TilePhoneOffline.Visible = false;
                    TileDialling.Visible = false;
                    TileInCall.Visible = false;
                    TilePhoneSetting.Visible = false;
                    TilePhoneLog.Visible = false;
                    TilePhoneCallLogs.Visible = true;
                    TilePhoneAcw.Visible = false;
                }));
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "ShowCallLogs", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "ShowCallLogs", exception, Logger.LogLevel.Error);
            }
        }

        public void ShowSetting()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    TilePhoneCallLogs.Visible = false;
                    TileIncommingCall.Visible = false;
                    TilePhoneOffline.Visible = false;
                    TileDialling.Visible = false;
                    TileInCall.Visible = false;
                    TilePhoneLog.Visible = false;
                    TilePhoneMain.Visible = false;
                    TilePhoneSetting.Visible = true;
                    TilePhoneAcw.Visible = false;
                }));
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "ShowSetting", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "ShowSetting", exception, Logger.LogLevel.Error);
            }
        }

        public void InIdleState()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    TilePhoneLog.Visible = false;
                    TileIncommingCall.Visible = false;
                    TilePhoneOffline.Visible = false;
                    TileDialling.Visible = false;
                    TileInCall.Visible = false;
                    TilePhoneSetting.Visible = false;
                    TilePhoneCallLogs.Visible = false;
                    TilePhoneMain.Visible = true;
                    TilePhoneAcw.Visible = false;

                    txtStatus.Text = string.Empty;
                    textBoxNumber.Text = string.Empty;

                    agent.CallSessionId = string.Empty;
                    call.CallSessionId = string.Empty;
                    call.portSipSessionId = -1;
                    call.PhoneNo = string.Empty;

                    ActiveControl = textBoxNumber;
                    textBoxNumber.Select();

                }));
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "UI Changed to Idle State", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "InIdleState", exception, Logger.LogLevel.Error);
            }
        }

        public void InOnlineState(string statusText)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    TilePhoneLog.Visible = false;
                    TileIncommingCall.Visible = false;
                    TilePhoneOffline.Visible = false;
                    TileDialling.Visible = false;
                    TileInCall.Visible = false;
                    TilePhoneSetting.Visible = false;
                    TilePhoneCallLogs.Visible = false;
                    TilePhoneMain.Visible = true;
                    TilePhoneAcw.Visible = false;

                    txtStatus.ForeColor = Color.DarkGreen;
                    txtStatus.Text = Environment.NewLine + "Phone Initialized with the IP" + statusText;
                    
                    PhoneStatus.Image = Properties.Resources.online;
                    txtStatus.Text = Environment.NewLine + statusText;
                    //buttonAnswer.Enabled = true;
                    //btnReregister.Visible = false;
                    textBoxNumber.BackColor = Color.White;
                    
                }));
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "UI Changed to Online State", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "InOnlineState", exception, Logger.LogLevel.Error);
            }
        }

        public void InAcwState()
        {
            try
            {
                StartACWTime();
                this.Invoke(new MethodInvoker(delegate
                {
                    TilePhoneMain.Visible = false;
                    TilePhoneLog.Visible = false;
                    TileIncommingCall.Visible = false;
                    TilePhoneOffline.Visible = false;
                    TileInCall.Visible = false;
                    TilePhoneSetting.Visible = false;
                    TilePhoneCallLogs.Visible = false;
                    TileDialling.Visible = false;
                    TilePhoneAcw.Visible = true;
                }));
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "UI Changed to ACW State", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "InACWState", exception, Logger.LogLevel.Error);
            }
        }

       

        public void InCallConnectedState()
        {
            try
            {
                StopRingTone();
                StopRingInTone();
               
                this.Invoke(new MethodInvoker(delegate
                {
                    TilePhoneMain.Visible = false;
                    TilePhoneLog.Visible = false;
                    TileIncommingCall.Visible = false;
                    TilePhoneOffline.Visible = false;
                    TileDialling.Visible = false;
                    TilePhoneSetting.Visible = false;
                    TilePhoneCallLogs.Visible = false;
                    TileInCall.Visible = true;
                    TilePhoneAcw.Visible = false;


                    btnHold.Text = "Hold";
                    btnHold.Enabled = true;

                    btnAnswer.Enabled = false;
                    btnReject.Enabled = true;
                    //buttontransferIvr.Enabled = false;
                    btnTransferCall.Enabled = false;
                    btnEtl.Enabled = false;
                    btnSwapCall.Enabled = false;
                    btnConference.Enabled = false;
                    btnDialpad.Enabled = true;

                    txtStatus.ForeColor = System.Drawing.Color.DarkGoldenrod;
                    txtStatus.Text = Environment.NewLine + "Call Established";
                }));
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "UI Changed to Call Connected State", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "InCallConnectedState", exception, Logger.LogLevel.Error);
            }
        }

        public void InOfflineState()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    TilePhoneMain.Visible = false;
                    TilePhoneLog.Visible = false;
                    TileIncommingCall.Visible = false;
                    TilePhoneOffline.Visible = true;
                    TileDialling.Visible = false;
                    TileInCall.Visible = false;
                    TilePhoneSetting.Visible = false;
                    TilePhoneCallLogs.Visible = false;
                    TilePhoneAcw.Visible = false;

                    btnAnswer.Enabled = false;
                    btnReject.Enabled = false;
                    //buttontransferIvr.Enabled = false;
                    btnTransferCall.Enabled = false;
                    btnEtl.Enabled = false;
                    btnSwapCall.Enabled = false;
                    btnConference.Enabled = false;
                    txtStatus.ForeColor = Color.Red;
                    txtStatus.Text = "OFFLINE";
                    UninitializePhone();
                }));
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "UI Changed to Offline State", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "InOfflineState", exception, Logger.LogLevel.Error);
            }
        }

        public void InInitiateState()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    TilePhoneMain.Visible = false;
                    TilePhoneLog.Visible = true;
                    TileIncommingCall.Visible = false;
                    TilePhoneOffline.Visible = false;
                    TileDialling.Visible = false;
                    TileInCall.Visible = false;
                    TilePhoneSetting.Visible = false;
                    TilePhoneCallLogs.Visible = false;
                    TilePhoneAcw.Visible = false;


                    //btnHold.Enabled = false;
                    //btnHold.Text = "Hold";
                    txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    txtStatus.Text = "";

                    //buttonAnswer.Enabled = true;
                    //buttonReject.Enabled = false;
                    ////buttontransferIvr.Enabled = false;
                    //btnTransferCall.Enabled = false;
                    //btnEtl.Enabled = false;
                    //btnSwapCall.Enabled = false;
                    //btnConference.Enabled = false;
                    this.ActiveControl = textBoxNumber;
                    textBoxNumber.Focus();

                }));
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "UI Changed to Offline State", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "InOfflineState", exception, Logger.LogLevel.Error);
            }
        }

        public void InInitiateMsgState(bool autoAnswerchk, bool autoAnswerEnb, string userName)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {

                    AutoAnswer.Checked = autoAnswerchk;
                    AutoAnswer.Enabled = autoAnswerEnb;


                    this.Text = string.Format("{0} : {1}", this.Text, userName);

                    txtStatus.ForeColor = Color.DarkMagenta;
                    txtStatus.Text = Environment.NewLine + "Initializing...";
                    
                }));
                InitializePhone();
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "InInitiateMsgState", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "InOfflineState", exception, Logger.LogLevel.Error);
            }
        }

        public void Error(string statusText)
        {
            this.Invoke(((MethodInvoker)(() =>
            {
                txtStatus.ForeColor = Color.Red;
                txtStatus.Text = Environment.NewLine + "Error on Initializing" + statusText;
                TilePhoneOffline.Visible = true;
            })));
        }

        public void InBreakState()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    TilePhoneMain.Visible = false;
                    TilePhoneLog.Visible = false;
                    TileIncommingCall.Visible = false;
                    TilePhoneOffline.Visible = false;
                    TileDialling.Visible = false;
                    TileInCall.Visible = false;
                    TilePhoneSetting.Visible = false;
                    TilePhoneCallLogs.Visible = false;
                    TilePhoneAcw.Visible = false;

                }));
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "UI Changed to Break State", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "InBreakState", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallAgentClintConnectedState()
        {
            try
            {
                this.Invoke(((MethodInvoker)(() =>
                   {
                       TilePhoneMain.Visible = false;
                       TilePhoneLog.Visible = false;
                       TileIncommingCall.Visible = false;
                       TilePhoneOffline.Visible = false;
                       TileDialling.Visible = false;
                       TilePhoneSetting.Visible = false;
                       TilePhoneCallLogs.Visible = false;
                       TileInCall.Visible = true;
                       TilePhoneAcw.Visible = false;


                       btnHold.Text = "Hold";
                       btnHold.Enabled = true;

                       btnAnswer.Enabled = false;
                       btnReject.Enabled = true;
                       //buttontransferIvr.Enabled = false;
                       btnTransferCall.Enabled = false;
                       btnEtl.Enabled = false;
                       btnSwapCall.Enabled = false;
                       btnConference.Enabled = false;
                       btnDialpad.Enabled = true;

                       btnSwapCall.Enabled = true;
                       btnEtl.Enabled = true;
                       btnConference.Enabled = true;
                       btnHold.Enabled = false;
                       btnTransferCall.Enabled = false;
                       btnDialpad.Enabled = false;
                   })));
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "UI Changed to Call CallAgentClintConnectedState", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "InCallAgentClintConnectedState", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallAgentSupConnectedState(CallActions callAction)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    TilePhoneMain.Visible = false;
                    TilePhoneLog.Visible = false;
                    TileIncommingCall.Visible = false;
                    TilePhoneOffline.Visible = false;
                    TileDialling.Visible = false;
                    TilePhoneSetting.Visible = false;
                    TilePhoneCallLogs.Visible = false;
                    TileInCall.Visible = true;
                    TilePhoneAcw.Visible = false;
                    
                    btnSwapCall.Enabled = callAction == CallActions.Call_Transferred;
                    btnEtl.Enabled = false;
                    btnConference.Enabled = false;
                    btnTransferCall.Enabled = false;
                    //buttontransferIvr.Enabled = false;
                    btnHold.Enabled = false;
                    btnDialpad.Enabled = false;
                }));
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "UI Changed to Call AgentSupConnectedState", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "InCallAgentSupConnectedState", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallConferenceState()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    TilePhoneMain.Visible = false;
                    TilePhoneLog.Visible = false;
                    TileIncommingCall.Visible = false;
                    TilePhoneOffline.Visible = false;
                    TileDialling.Visible = false;
                    TilePhoneSetting.Visible = false;
                    TilePhoneCallLogs.Visible = false;
                    TileInCall.Visible = true;
                    TilePhoneAcw.Visible = false;


                    btnHold.Text = "Hold";
                    btnHold.Enabled = true;

                    btnAnswer.Enabled = false;
                    btnReject.Enabled = true;
                    //buttontransferIvr.Enabled = false;
                    btnTransferCall.Enabled = false;
                    btnEtl.Enabled = false;
                    btnSwapCall.Enabled = false;
                    btnConference.Enabled = false;
                    btnDialpad.Enabled = true;

                    btnEtl.Enabled = false;
                    btnSwapCall.Enabled = false;
                    btnConference.Enabled = false;
                    btnHold.Enabled = false;
                    btnDialpad.Enabled = false;
                }));
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "UI Changed to CallConferenceState", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "InCallConferenceState", exception, Logger.LogLevel.Error);
            }
            
        }

        public void InCallDisconnectedState()
        {
            try
            {
               
                StopRingInTone();
                StopRingTone();

                this.Invoke(new MethodInvoker(delegate
                {
                    TilePhoneLog.Visible = false;
                    TileIncommingCall.Visible = false;
                    TilePhoneOffline.Visible = false;
                    TileDialling.Visible = false;
                    TileInCall.Visible = false;
                    TilePhoneSetting.Visible = false;
                    TilePhoneCallLogs.Visible = false;
                    TilePhoneMain.Visible = true;
                    TilePhoneAcw.Visible = false;

                    btnHold.Enabled = false;
                    btnHold.Text = "Hold";
                    txtStatus.ForeColor = Color.DarkGreen;
                    txtStatus.Text = "";
                    
                    btnAnswer.Enabled = true;
                    btnReject.Enabled = false;
                    //buttontransferIvr.Enabled = false;
                    btnTransferCall.Enabled = false;
                    btnEtl.Enabled = false;
                    btnSwapCall.Enabled = false;
                    btnConference.Enabled = false;

                    btnDialpad.Enabled = true;
                    txtDurations.Text = "00:00";
                    //this.ActiveControl = textBoxNumber;
                    //textBoxNumber.Focus();

                    call.CallSessionId = string.Empty;
                    CallDurations.Stop();

                    txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    txtStatus.Text = Environment.NewLine + "Call Ended";

                    if (!isCallAnswerd)
                    {
                        AddMiscallToCallLogs();
                    }
                    AddCallDurations();
                }));
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "UI Changed to InCallDisconnectedState", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "InCallDisconnectedState", exception, Logger.LogLevel.Error);
            }

            
        }

        public void InCallHoldState(CallActions callAction)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    TilePhoneMain.Visible = false;
                    TilePhoneLog.Visible = false;
                    TileIncommingCall.Visible = false;
                    TilePhoneOffline.Visible = false;
                    TileDialling.Visible = false;
                    TilePhoneSetting.Visible = false;
                    TilePhoneCallLogs.Visible = false;
                    TileInCall.Visible = true;
                    TilePhoneAcw.Visible = false;

                    switch (callAction)
                    {
                        case CallActions.UnHold_Requested:
                        case CallActions.UnHold_InProgress:
                        case CallActions.UnHold:
                            btnHold.Text = "Hold";
                            btnHold.Enabled = true;
                            btnAnswer.Enabled = false;
                            btnReject.Enabled = false;
                            btnTransferCall.Enabled = false;
                            btnEtl.Enabled = false;
                            btnSwapCall.Enabled = false;
                            btnConference.Enabled = false;
                            btnDialpad.Enabled = true;
                            break;
                        case CallActions.Hold_Requested:
                        case CallActions.Hold_InProgress:
                        case CallActions.Hold:
                            btnHold.Text = "Unhold";
                            btnTransferCall.Enabled = true;
                            //buttontransferIvr.Enabled = false;
                            btnHold.Enabled = true;
                            btnEtl.Enabled = false;
                            btnDialpad.Enabled = false;
                            break;
                            
                    }
                }));
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault,string.Format("UI Changed to Call Un/Hold State. callAction : {0}",callAction), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "InCallHoldState", exception, Logger.LogLevel.Error);
            }
           
        }

        public void InCallIdleState()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {

                    TilePhoneLog.Visible = false;
                    TileIncommingCall.Visible = false;
                    TilePhoneOffline.Visible = false;
                    TileDialling.Visible = false;
                    TileInCall.Visible = false;
                    TilePhoneSetting.Visible = false;
                    TilePhoneCallLogs.Visible = false;
                    TilePhoneMain.Visible = true;
                    TilePhoneAcw.Visible = false;

                    btnDial.Enabled = true;
                    btnHold.Enabled = false;
                    btnHold.Text = "Hold";
                    txtStatus.ForeColor = Color.DarkGreen;
                    txtStatus.Text = "";
                    textBoxNumber.Text = string.Empty;
                    btnAnswer.Enabled = true;
                    btnReject.Enabled = false;
                    //buttontransferIvr.Enabled = false;
                    btnTransferCall.Enabled = false;
                    btnEtl.Enabled = false;
                    btnSwapCall.Enabled = false;
                    btnConference.Enabled = false;

                    btnDialpad.Enabled = true;
                    txtDurations.Text = "00:00";

                    //this.ActiveControl = textBoxNumber;
                    //textBoxNumber.Focus();

                }));
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "UI Changed to Idle State", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "InCallIdleState", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallRingingState()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    TilePhoneMain.Visible = false;
                    TilePhoneLog.Visible = false;
                    TileIncommingCall.Visible = false;
                    TilePhoneOffline.Visible = false;
                    TileInCall.Visible = false;
                    TilePhoneSetting.Visible = false;
                    TilePhoneCallLogs.Visible = false;
                    TilePhoneAcw.Visible = false;
                    btnAnswer.Enabled = false;
                    btnReject.Enabled = true;
                    TileDialling.Visible = true;
                    TilePhoneMain.SendToBack();
                    TileDialling.BringToFront();
                    txtStatus.Text = "Dialing...";
                }));
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "UI Changed to Rigging State", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "InRiggingState", exception, Logger.LogLevel.Error);
            }
        }

        public void InRiggingState()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    TilePhoneMain.Visible = false;
                    TilePhoneLog.Visible = false;
                    TileIncommingCall.Visible = false;
                    TilePhoneOffline.Visible = false;
                    TileInCall.Visible = false;
                    TilePhoneSetting.Visible = false;
                    TilePhoneCallLogs.Visible = false;
                    TilePhoneAcw.Visible = false;
                    TileDialling.Visible = true;

                }));
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "UI Changed to Rigging State", Logger.LogLevel.Info);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "InRiggingState", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallTryingState()
        {
           InCallRingingState();
        }

        public void InCallIncommingState()
        {
            try
            {
                if (playRingtone)
                    PlayRingTone();
                new Thread(() => GetPhoneNo(call.CallSessionId)).Start();

                this.Invoke(new MethodInvoker(delegate
                {
                    TilePhoneMain.Visible = false;
                    TilePhoneLog.Visible = false;
                    TileIncommingCall.Visible = false;
                    TilePhoneOffline.Visible = false;
                    TileInCall.Visible = false;
                    TilePhoneSetting.Visible = false;
                    TilePhoneCallLogs.Visible = false;
                    TileDialling.Visible = true;
                    TilePhoneAcw.Visible = false;

                    btnAnswer.Enabled = true;
                    btnReject.Enabled = true;

                    txtStatus.ForeColor = Color.DarkMagenta;
                    txtStatus.Text = Environment.NewLine + "Call Incoming......";

                }));
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "UI Changed to CallIncommingState", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "InCallIncommingState", exception, Logger.LogLevel.Error);
            }
        }

        #endregion

        #region SIPCallbackEvents

        public int onRegisterSuccess(int callbackIndex, int callbackObject, string statusText, int statusCode)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "phoneController_OnInitialized", Logger.LogLevel.Info);
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onRegisterSuccess", Logger.LogLevel.Info);
                _SIPLogined = true;
                call = new Call(string.Empty, this);
                agent.AgentCurrentState.OnLoggedOn(ref agent,callSessionId);
                
                this.Invoke(new MethodInvoker(() =>
                {
                    ActiveControl = textBoxNumber;
                    textBoxNumber.Select();
                    PhoneStatus.Image = Properties.Resources.online;
                    //btnReregister.Visible = false;
                }));

                #region AgentList

                resMonitoringHandler = new ResourceMonitoringHandler(agent.Auth.SecurityToken, agent.Auth.guUserId.ToString());
                resMonitoringHandler.OnGetOnlineIdleAgentListCompleted += (a) =>
                {
                    try
                    {
                        GridAgentList.Invoke(new MethodInvoker(delegate
                        {
                            GridAgentList.DataSource = a;
                        }));
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "resHandler.OnGetOnlineIdleAgentListCompleted", exception, Logger.LogLevel.Error);
                    }
                };


                #endregion
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "onRegisterSuccess", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onRegisterFailure(int callbackIndex, int callbackObject, string statusText, int statusCode)
        {
            Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onRegisterFailure", Logger.LogLevel.Info);
            InitializeError(statusText);
            return 0;
        }

        public int onInviteIncoming(int callbackIndex, int callbackObject, int sessionId, string callerDisplayName, string caller,
            string calleeDisplayName, string callee, string audioCodecNames, string videoCodecNames, bool existsAudio,
            bool existsVideo)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onInviteIncoming", Logger.LogLevel.Info);
                call.CallSessionId = caller.Split('@')[0].Replace("sip:", "");
                call.SetDialInfo(sessionId, Guid.NewGuid());

                agent.AgentCurrentState.OnAnswerCall(ref agent, call.CallSessionId);
                call.CallCurrentState.OnIncoming(ref call,callbackIndex,callbackObject,sessionId,calleeDisplayName,caller,calleeDisplayName,callee,audioCodecNames,videoCodecNames,existsAudio,existsVideo);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "onInviteIncoming", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteTrying(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onInviteTrying", Logger.LogLevel.Info);
                PlayRingInTone();
                call.CallCurrentState.OnRinging(ref call, callbackIndex, callbackObject, sessionId,string.Empty,0);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "onInviteTrying", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteSessionProgress(int callbackIndex, int callbackObject, int sessionId, string audioCodecNames,
            string videoCodecNames, bool existsEarlyMedia, bool existsAudio, bool existsVideo)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onInviteSessionProgress", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onInviteRinging(int callbackIndex, int callbackObject, int sessionId, string statusText, int statusCode)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onInviteRinging", Logger.LogLevel.Info);
                PlayRingInTone();
                call.CallCurrentState.OnRinging(ref call,callbackIndex,callbackObject,sessionId,statusText,statusCode);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "onInviteRinging", exception, Logger.LogLevel.Error);
            }
            return 0;

        }

        public int onInviteAnswered(int callbackIndex, int callbackObject, int sessionId, string callerDisplayName, string caller,
            string calleeDisplayName, string callee, string audioCodecNames, string videoCodecNames, bool existsAudio,
            bool existsVideo)
        {

            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onInviteAnswered", Logger.LogLevel.Info);
                
                call.CallCurrentState.OnAnswer(ref call);
               
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "onInviteAnswered", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteFailure(int callbackIndex, int callbackObject, int sessionId, string reason, int code)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onInviteFailure", Logger.LogLevel.Info);

                this.Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    txtStatus.Text = Environment.NewLine + "Call Rejected from Other End" + Environment.NewLine + reason;
                }))); 
                call.CallCurrentState.OnCallReject(ref call);
                agent.AgentCurrentState.OnCallReject(ref agent,call.CallSessionId);
                
                isCallAnswerd = false;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "onInviteFailure", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteUpdated(int callbackIndex, int callbackObject, int sessionId, string audioCodecNames, string videoCodecNames,
            bool existsAudio, bool existsVideo)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onInviteUpdated", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onInviteConnected(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onInviteConnected", Logger.LogLevel.Info);
                isCallAnswerd = true;
                
                call.CallCurrentState.OnAnswer(ref call);
                
                callStarTime = DateTime.Now;
                CallDurations.Start();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "onInviteAnswered", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteBeginingForward(int callbackIndex, int callbackObject, string forwardTo)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onInviteBeginingForward", Logger.LogLevel.Info);
                this.Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = System.Drawing.Color.DarkGoldenrod;
                    txtStatus.Text = Environment.NewLine + "Call Begining Forward.";
                })));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "onInviteBeginingForward", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteClosed(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onInviteClosed", Logger.LogLevel.Info);
                //DisableAlert();
                call.CallCurrentState.OnDisconnected(ref call);
                agent.AgentCurrentState.OnEndCall(ref agent,call.CallSessionId);
                isCallAnswerd = false;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "onInviteClosed", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onRemoteHold(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onRemoteHold", Logger.LogLevel.Info);
                this.Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    txtStatus.Text = Environment.NewLine + "Remote Hold";
                })));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
            } return 0;
        }

        public int onRemoteUnHold(int callbackIndex, int callbackObject, int sessionId, string audioCodecNames, string videoCodecNames,
            bool existsAudio, bool existsVideo)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onRemoteUnHold", Logger.LogLevel.Info);
                this.Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    txtStatus.Text = Environment.NewLine + "Remote UnHold";
                })));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
            } return 0;
        }

        public int onReceivedRefer(int callbackIndex, int callbackObject, int sessionId, int referId, string to, string @from,
            string referSipMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onReceivedRefer", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onReferAccepted(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onReferAccepted", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onReferRejected(int callbackIndex, int callbackObject, int sessionId, string reason, int code)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onReferRejected", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onTransferTrying(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onTransferTrying", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onTransferRinging(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onTransferRinging", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onACTVTransferSuccess(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onACTVTransferSuccess", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onACTVTransferFailure(int callbackIndex, int callbackObject, int sessionId, string reason, int code)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onACTVTransferFailure", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onReceivedSignaling(int callbackIndex, int callbackObject, int sessionId, StringBuilder signaling)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onReceivedSignaling", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onSendingSignaling(int callbackIndex, int callbackObject, int sessionId, StringBuilder signaling)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onSendingSignaling", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onWaitingVoiceMessage(int callbackIndex, int callbackObject, string messageAccount, int urgentNewMessageCount,
            int urgentOldMessageCount, int newMessageCount, int oldMessageCount)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onWaitingVoiceMessage", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onWaitingFaxMessage(int callbackIndex, int callbackObject, string messageAccount, int urgentNewMessageCount,
            int urgentOldMessageCount, int newMessageCount, int oldMessageCount)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onWaitingFaxMessage", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onRecvDtmfTone(int callbackIndex, int callbackObject, int sessionId, int tone)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onRecvDtmfTone", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onRecvOptions(int callbackIndex, int callbackObject, StringBuilder optionsMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onRecvOptions", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onRecvInfo(int callbackIndex, int callbackObject, StringBuilder infoMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onRecvInfo", Logger.LogLevel.Info);
                ReceveMeassge("Receive Information", infoMessage.ToString());
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onPresenceRecvSubscribe(int callbackIndex, int callbackObject, int subscribeId, string fromDisplayName, string @from,
            string subject)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onPresenceRecvSubscribe", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onPresenceOnline(int callbackIndex, int callbackObject, string fromDisplayName, string @from, string stateText)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onPresenceOnline", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onPresenceOffline(int callbackIndex, int callbackObject, string fromDisplayName, string @from)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onPresenceOffline", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onRecvMessage(int callbackIndex, int callbackObject, int sessionId, string mimeType, string subMimeType,
            byte[] messageData, int messageDataLength)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onRecvMessage", Logger.LogLevel.Info);
                if (mimeType == "text" && subMimeType == "plain")
                {
                    string mesageText = GetString(messageData);
                    ReceveMeassge("Receive Information", mesageText);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
            } return 0;
        }

        public int onRecvOutOfDialogMessage(int callbackIndex, int callbackObject, string fromDisplayName, string @from,
            string toDisplayName, string to, string mimeType, string subMimeType, byte[] messageData, int messageDataLength)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onRecvOutOfDialogMessage", Logger.LogLevel.Info);
                if (mimeType == "text" && subMimeType == "plain")
                {
                    string mesageText = GetString(messageData);
                    ReceveMeassge("Receive Information", mesageText);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
            } return 0;
        }

        public int onSendMessageSuccess(int callbackIndex, int callbackObject, int sessionId, int messageId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onSendMessageSuccess", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onSendMessageFailure(int callbackIndex, int callbackObject, int sessionId, int messageId, string reason, int code)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onSendMessageFailure", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onSendOutOfDialogMessageSuccess(int callbackIndex, int callbackObject, int messageId, string fromDisplayName,
            string @from, string toDisplayName, string to)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onSendOutOfDialogMessageSuccess", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onSendOutOfDialogMessageFailure(int callbackIndex, int callbackObject, int messageId, string fromDisplayName,
            string @from, string toDisplayName, string to, string reason, int code)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onSendOutOfDialogMessageFailure", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onPlayAudioFileFinished(int callbackIndex, int callbackObject, int sessionId, string fileName)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onPlayAudioFileFinished", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onPlayVideoFileFinished(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onPlayVideoFileFinished", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onReceivedRtpPacket(IntPtr callbackObject, int sessionId, bool isAudio, byte[] RTPPacket, int packetSize)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onReceivedRtpPacket", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onSendingRtpPacket(IntPtr callbackObject, int sessionId, bool isAudio, byte[] RTPPacket, int packetSize)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onSendingRtpPacket", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onAudioRawCallback(IntPtr callbackObject, int sessionId, int callbackType, byte[] data, int dataLength,
            int samplingFreqHz)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onAudioRawCallback", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onVideoRawCallback(IntPtr callbackObject, int sessionId, int callbackType, int width, int height, byte[] data,
            int dataLength)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger4, "onVideoRawCallback", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        #endregion SIPCallbackEvents

        #region Private

        private void Reregister()
        {
            try
            {
                //ProgressBar.Show();
                //ProgressBar.Start();
                UninitializePhone();
                StopRingInTone();
                StopRingTone();
                PhoneStatus.Image = Properties.Resources.offline;
                InIdleState();
                this.Invoke(new MethodInvoker(delegate
                {
                    txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    txtStatus.Text = Environment.NewLine + "Initializing";
                    // mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Phone Reinitializing.", ToolTipIcon.Info);
                }));
                agent.AgentCurrentState.OnLogOn(ref agent);
                InitializePhone();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Reregister", exception, Logger.LogLevel.Error);
            }
        }

        private bool DisableRingtone()
        {
            playRingtone = false;
            //mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Ring tone Off", ToolTipIcon.Info);
            return true;
        }

        private bool EnableRingtone()
        {
            try
            {
                var settingObject = System.Configuration.ConfigurationSettings.AppSettings;
                var filePath = settingObject["RingToneFilePath"];
                var ringtone = settingObject["PlayRingtone"].ToLower();
                if (!File.Exists(filePath))
                {
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, string.Format("PlayRingTone> Cannot Find File {0}", filePath), Logger.LogLevel.Error);
                    if (ringtone.Equals("1") || ringtone.Equals("true"))
                    {
                        filePath = string.Format(@"{0}\{1}", Application.StartupPath, "Ringtone.wav");
                        if (File.Exists(filePath))
                        {
                            Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "PlayRingTone> Play with default ringin tone", Logger.LogLevel.Info);
                            playRingtone = true;
                            //mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Ring tone On", ToolTipIcon.Info);
                            return true;
                        }
                    }
                }

                playRingtone = false;
                // mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Ring tone Off-No Audio File", ToolTipIcon.Info);
                return false;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "EnableRingtone", exception, Logger.LogLevel.Error);
                return false;
            }
        }

        private void OpenLogFils()
        {
            try
            {
                string[] filePaths = Directory.GetFiles(string.Format(@"C:\Users\{0}\AppData\Local\DuoSoftware", Environment.UserName), "*.duo");//Directory.EnumerateFiles(workingDirectory, "*.duo")
                foreach (var file in filePaths)
                {
                    try
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.FileName = "notepad.exe";
                        startInfo.Arguments = file;
                        Process.Start(startInfo);

                        //var notepadLog = new ProcessStartInfo("notepad.exe", file);//string.Format("{0}\\{1}.dat", workingDirectory, file)
                        //Process.Start(notepadLog);
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "ViewErrorsLogFile - Find Controler", exception, Logger.LogLevel.Error);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "OpenLogFils", exception, Logger.LogLevel.Error);
            }
        }

        private void OpenLogFilsDirectory()
        {
            try
            {
                Process.Start("explorer.exe", string.Format(@"C:\Users\{0}\AppData\Local\DuoSoftware", Environment.UserName));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "OpenLogFilsDirectory", exception, Logger.LogLevel.Error);
            }
        }

        private void SendDTMF(int digit)
        {
            try
            {
                if (call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    phoneController.sendDtmf(call.portSipSessionId, DTMF_METHOD.DTMF_RFC2833, Convert.ToInt16(digit), 160, true);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "SendDTMF", exception, Logger.LogLevel.Error);
            }
        }

        private void HoldUnholdCall()
        {
            try
            {
                var status = "Hold Call";
                int res = -1;
                if (call.CallCurrentState.GetType() == typeof(CallConnectedState))
                {
                    if (agent.CallHandler._sipServiceCall.IsPBX)
                    {
                        res = phoneController.hold(call.portSipSessionId);
                        if (res == 0)
                        {
                            call.CallCurrentState.OnHold(ref call, CallActions.Hold);
                        }
                    }
                    else
                    {
                        call.CallCurrentState.OnHold(ref call, CallActions.Hold_Requested);
                        res = phoneController.sendInfo(call.portSipSessionId, "text", "plain", "hold");//, "hold".Length);
                    }

                }
                else if (call.CallCurrentState.GetType() == typeof(CallHoldState))
                {
                    if (agent.CallHandler._sipServiceCall.IsPBX)
                    {
                        res = phoneController.unHold(call.portSipSessionId);
                        if (res == 0)
                        {
                            call.CallCurrentState.OnUnHold(ref call, CallActions.UnHold);
                        }
                    }
                    else
                    {
                        call.CallCurrentState.OnUnHold(ref call, CallActions.UnHold_Requested);
                        res = phoneController.sendInfo(call.portSipSessionId, "text", "plain", "unhold");
                        
                            
                    }
                }

                //this.txtStatus.Invoke(((MethodInvoker)(() =>
                //{
                //    txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                //    txtStatus.Text = Environment.NewLine + (string.Format("{0} {1}", status, (res != 0 ? "Failed" : "")));
                //})));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "HoldUnholdCall", exception, Logger.LogLevel.Error);
            }
        }

        private void ReceveMeassge(string status, string fullMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, string.Format("ReceveMeassge-> status : {0} , DialPadEventArgs: {1} : ", status, fullMessage), Logger.LogLevel.Info);
                
                if (!String.IsNullOrEmpty(fullMessage))
                {
                    if (fullMessage.Contains(','))
                    {
                        string[] splitData = fullMessage.Split(',');
                        var msgString = splitData.First().ToUpper();

                        switch (msgString)
                        {
                            case "SESSIONCREATED":
                                var sessionId = splitData[1];
                                call.CallSessionId = sessionId;
                                agent.SessionCreated(sessionId);
                                break;
                        }
                    }
                    else
                    {
                        var msgString = fullMessage.ToUpper();
                        if (msgString == "FAILED" || msgString == "FAIL")
                        {
                            txtStatus.Invoke(new MethodInvoker(delegate
                            {
                                txtStatus.ForeColor = Color.DarkRed;
                                txtStatus.Text = Environment.NewLine + "Operation Fail.";
                            }));

                            call.CallCurrentState.OnOperationFail(ref call);
                            
                        }
                        else if (msgString.ToUpper() == "ROUTEBACK")
                        {
                            call.CallCurrentState.OnReset(ref call);
                            txtStatus.Invoke(new MethodInvoker(delegate
                            {
                                txtStatus.ForeColor = Color.DarkGreen;
                                txtStatus.Text = Environment.NewLine + "Call Route Back.";
                            }));
                        }
                        else if (msgString == "SUCCESS")
                        {
                            txtStatus.Invoke(new MethodInvoker(delegate
                            {
                                txtStatus.ForeColor = Color.DarkGreen;
                                txtStatus.Text = Environment.NewLine + "Operation Succeed.";
                            }));
                            call.CallCurrentState.OnSetStatus(ref call);
                        }


                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "ReceveMeassge", exception, Logger.LogLevel.Error);
            }
        }


        private string GetString(byte[] bytes)
        {
            return System.Text.Encoding.Default.GetString(bytes);
        }

        private void GetPhoneNo(string callSessionId)
        {
            try
            {
                call.PhoneNo = string.Empty;
                var phnNo = EngagementHandler.GetPhoneNo(agent.Auth.SecurityToken, callSessionId);
                if (call.CallCurrentState.GetType()==typeof(CallDisconnectedState)|| call.CallCurrentState.GetType()==typeof(CallIdleState))
                {
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Get info after call Disconnected", Logger.LogLevel.Info);
                    return;
                }

                this.Invoke(((MethodInvoker)(() =>
                {
                    //txtStatus.Text = Environment.NewLine + "Incoming Call : " + phnNo;
                    textBoxNumber.Text = phnNo;
                    call.PhoneNo = phnNo;
                    if (!source.Contains(phnNo))
                        source.Add(phnNo);
                    if (!string.IsNullOrEmpty(phnNo))
                    {
                        AddIncommingToCallLogs(phnNo);
                    }
                })));
                new Thread(() => SendMsgToTappi(phnNo)).Start();
                //if (AutoAnswer.Checked)
                //{
                //    MakeCall(phnNo);
                //}
                var info = EngagementHandler.ViewCurrentEngagementDisplayInfo(agent.Auth.SecurityToken, callSessionId);
                mynotifyicon.ShowBalloonTip(500, "Duo Soft Phone", info, ToolTipIcon.Info);

                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, string.Format("Display Call info-Main : {0}:{1}> {2}", callSessionId, phnNo, info), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "GetPhoneNo", exception, Logger.LogLevel.Error);
            }
        }

        private void SendMsgToTappi(string no)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, string.Format("Send no : {0} To Tappi", no), Logger.LogLevel.Info);
                IXDBroadcast broadcast = XDBroadcast.CreateBroadcast(XDTransportMode.IOStream, false);
                broadcast.SendToChannel("CallerID", no);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "SendMsgToTappi", exception, Logger.LogLevel.Error);
            }
        }

        private void PlayRingTone()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Start to play ring tone", Logger.LogLevel.Info);
                if (playRingtone)
                    _wavPlayer.PlayLooping();
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "PlayRingTone > End", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "PlayRingTone", exception, Logger.LogLevel.Error);
            }
        }

        private void StopRingTone()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "StopRingTone>Start", Logger.LogLevel.Info);
                if (_wavPlayer == null) return;
                _wavPlayer.Stop();
                _wavPlayer.Dispose();
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "StopRingTone>End", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "StopRingTone", exception, Logger.LogLevel.Error);
            }
        }

        private void PlayRingInTone()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Start to play ringIn tone", Logger.LogLevel.Info);
                if (playingRingIntone)
                    return;
                if (_wavPlayerRingIn == null) return;
                _wavPlayerRingIn.PlayLooping();
                playingRingIntone = true;
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "PlayRingInTone > End", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "PlayRingInTone", exception, Logger.LogLevel.Error);
            }
        }

        private void StopRingInTone()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "StopRingInTone>Start", Logger.LogLevel.Info);
                playingRingIntone = false;
                if (_wavPlayerRingIn == null) return;
                _wavPlayerRingIn.Stop();
                _wavPlayerRingIn.Dispose();
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "StopRingInTone>End", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "StopRingInTone", exception, Logger.LogLevel.Error);
            }
        }

        private void AddOutgoingCallToCallLogs(string no)
        {
            try
            {
                lock (CallLog)
                {
                    var row = CallLog.NewRow();
                    row["Directions"] = (Bitmap)Properties.Resources.calloutgoing;
                    row["Duration"] = 0;
                    row["Id"] = call.currentCallLogId;
                    row["Time"] = DateTime.Now.ToString("MM : dd HH:mm:ss");
                    row["CallStartTime"] = DateTime.Now;
                    row["Number"] = no;

                    CallLog.Rows.Add(row);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "AddOutgoingCallToCallLogs", exception, Logger.LogLevel.Error);
            }
        }

        private void AddIncommingToCallLogs(string no)
        {
            try
            {
                lock (CallLog)
                {

                    var log = CallLog.Select().ToList();
                    if (log.Exists(r => r["Id"].Equals(call.currentCallLogId)))
                    {
                        CallLog.Rows.Remove(log.Find(r => r["Id"].Equals(call.currentCallLogId)));
                    }
                    var row = CallLog.NewRow();
                    row["Directions"] = (Bitmap)Properties.Resources.IncommingCall;
                    row["Duration"] = 0;
                    row["Id"] = call.currentCallLogId;
                    row["Time"] = DateTime.Now.ToString("MM : dd HH:mm:ss");
                    row["CallStartTime"] = DateTime.Now;
                    row["Number"] = no;

                    CallLog.Rows.Add(row);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "AddIncommingToCallLogs", exception, Logger.LogLevel.Error);
            }
        }

        private void AddCallDurations()
        {
            try
            {
                var log = CallLog.Select().ToList().Find(r => r["Id"].Equals(call.currentCallLogId));
                log["Duration"] = Math.Round(DateTime.Now.Subtract(Convert.ToDateTime(log["CallStartTime"])).TotalSeconds, 2);//
                lock (CallLog)
                {
                    CallLog.Rows.Remove(log);
                    CallLog.Rows.Add(log);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "AddCallDurations", exception, Logger.LogLevel.Error);
            }
        }

        private void AddMiscallToCallLogs()
        {
            try
            {
                var log = CallLog.Select().ToList().Find(r => r["Id"].Equals(call.currentCallLogId));
                log["Directions"] = (Bitmap)Properties.Resources.MissCallCallLog;
                lock (CallLog)
                {
                    CallLog.Rows.Remove(log);
                    CallLog.Rows.Add(log);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "AddMiscallToCallLogs", exception, Logger.LogLevel.Error);
            }
        }

        private void StartACWTime()
        {
            try
            {
                acwCotdown = acwTime;
                acwCutdownTimer.Enabled = true;
                acwCutdownTimer.Start();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "StartACWTime", exception, Logger.LogLevel.Error);
            }
        }

        private void FreezeACWTime()
        {
            try
            {
                acwCutdownTimer.Stop();
                acwCutdownTimer.Enabled = false;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "StopACWTime", exception, Logger.LogLevel.Error);
            }
        }

        private void TransferCall()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault,
                    string.Format("transferCall_Click-> Session Id : {0} , Status : {1}", call.CallSessionId,call.CallCurrentState),
                    Logger.LogLevel.Info);
                if (call.CallCurrentState.GetType() == typeof(CallHoldState))
                {
                    if (!String.IsNullOrEmpty(txtNumberpad.Text))
                    {
                        var res = phoneController.sendInfo(call.portSipSessionId, "text", "plain", "transfer:" + txtNumberpad.Text);
                        if (res == 0)
                            call.CallCurrentState.OnTransferReq(ref call, CallActions.Call_Transfer_Requested);
                        txtStatus.Text = Environment.NewLine + (res != 0 ? "Transfer Failed." : "Transferring Call...");
                        if (!source.Contains(txtNumberpad.Text))
                            source.Add(txtNumberpad.Text);

                        DialPadFrmPanel.Visible = false;
                        DialPadFrmPanel.SendToBack();
                        //GBCallIn.Visible = true;
                        GBCallIn.BringToFront();
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "TransferCall", exception, Logger.LogLevel.Error);
            }
        }

        private void EndCall()
        {
            try
            {
                StopRingTone();
                var status = "Call Ended";
                if (call.CallCurrentState.GetType() == typeof(CallRingingState))
                {
                    if (agent.CallDirection == CallDirection.Outgoing)
                    {
                        phoneController.hangUp(call.portSipSessionId);
                    }
                    else
                    {
                        phoneController.rejectCall(call.portSipSessionId, 486);
                    }
                    status = "Call Rejected";
                }
                else
                    phoneController.hangUp(call.portSipSessionId);

                agent.AgentCurrentState.OnEndCall(ref agent,call.CallSessionId);
                this.Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    txtStatus.Text = Environment.NewLine + status;
                })));

                AddCallDurations();

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void MakeCall(string no)
        {
            try
            {
                if (agent.AgentCurrentState.GetType() == typeof (AgentIdle))
                {
                    if (!String.IsNullOrEmpty(no))
                    {
                        btnDial.Invoke(new MethodInvoker(() =>
                        {
                            txtNumberpad.Text = textBoxNumber.Text;
                            txtDiallingNo.Text = textBoxNumber.Text; 
                            btnDial.Enabled = false;
                        }));

                        new Thread(() => InCallTryingState()).Start();
                        agent.AgentCurrentState.OnMakeCall(ref agent);
                        call = new Call(no,this) {portSipSessionId = phoneController.call(no, true, false)};
                        if (call.portSipSessionId < 0)
                        {
                            btnDial.Invoke(new MethodInvoker(() => { btnDial.Enabled = true; }));
                            mynotifyicon.ShowBalloonTip(5, "Duo Soft Phone", "Fail to Make Call", ToolTipIcon.Error);
                            agent.AgentCurrentState.OnFailMakeCall(ref agent);
                            call.CallCurrentState = new CallIdleState();
                            return;
                        }
                        call.SetDialInfo(call.portSipSessionId, Guid.NewGuid());
                        AddOutgoingCallToCallLogs(no);
                        
                    }
                    else
                    {
                        mynotifyicon.ShowBalloonTip(5, "Duo Soft Phone", "Invalid Phone Number.", ToolTipIcon.Error);
                    }
                }
                else if (call.GetType() == typeof(CallRingingState))
                {
                    phoneController.answerCall(call.portSipSessionId, false);
                }

                if (!source.Contains(no))
                    source.Add(no);
                source.Remove("");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "MakeCall", exception, Logger.LogLevel.Error);
            }
        }

        private void Login()
        {
            try
            {
                btnLogin.Enabled = false;
                agent = new Agent(txtUserName.Text, txtPassword.Text, this);
                agent.AgentCurrentState.OnLogOn(ref agent);

                btnLogin.Enabled = true;

                #region ACW Timer

                acwCutdownTimer = new Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);

                acwTime = (int)TimeSpan.FromSeconds(Convert.ToDouble(ProfileManagementHandler.GetACWTime(agent.Auth.SecurityToken))).TotalSeconds;
                acwCotdown = acwTime;
                LabelCountdown.Text = acwTime.ToString();
                acwCutdownTimer.Elapsed += (s, e1) =>
                {
                    if (acwCotdown < 0)
                    {
                        acwCutdownTimer.Stop();
                        acwCutdownTimer.Enabled = false;
                        agent.AgentCurrentState.OnEndCallLog(ref agent, call.CallSessionId);
                        LabelCountdown.Invoke(new MethodInvoker(() =>
                        {
                            LabelCountdown.Text = acwTime.ToString();
                            textBoxNumber.Text = string.Empty;
                        }));// st
                        return;
                    }
                    LabelCountdown.Invoke(new MethodInvoker(() =>
                    {
                        LabelCountdown.Text = acwCotdown.ToString();
                    }));// string.Format("ACW : {0}", acwCotdown); }));
                    acwCotdown--;
                };

                #endregion
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Login", exception, Logger.LogLevel.Error);
            }
        }


        private void InitAdioWizItems()
        {


            this.ComboBoxMicrophones.SelectionChanged += (s, e) =>
            {
                try
                {
                    phoneController.setAudioDeviceId(ComboBoxMicrophones.SelectedIndex, ComboBoxSpeakers.SelectedIndex);
                    audioHandler.WriteXML(ComboBoxMicrophones.SelectedIndex, phoneController.getMicVolume(), ComboBoxSpeakers.SelectedIndex, ComboBoxSpeakers.SelectedItem.ToString(), phoneController.getSpeakerVolume(), ComboBoxMicrophones.SelectedItem.ToString());
                }
                catch (Exception exception)
                {
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "ComboBoxMicrophones.SelectedIndexChanged", exception, Logger.LogLevel.Error);
                }
            };


            this.ComboBoxSpeakers.SelectionChanged += (s, e) =>
            {
                try
                {
                    phoneController.setAudioDeviceId(ComboBoxMicrophones.SelectedIndex, ComboBoxSpeakers.SelectedIndex);
                    audioHandler.WriteXML(ComboBoxMicrophones.SelectedIndex, phoneController.getMicVolume(), ComboBoxSpeakers.SelectedIndex, ComboBoxSpeakers.SelectedItem.ToString(), phoneController.getSpeakerVolume(), ComboBoxMicrophones.SelectedItem.ToString());
                }
                catch (Exception exception)
                {
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "ComboBoxSpeakers.SelectedIndexChanged", exception, Logger.LogLevel.Error);
                }
            };


        }

        private void loadDevices()
        {
            try
            {
                string selectedSpeaker = string.Empty;
                string selectedMic = string.Empty;
                int MicVolume = 0;
                int SpkVolume = 0;
                var setting = audioHandler.ReadXML();
                if (setting != null)
                {
                    try
                    {
                        selectedSpeaker = setting["SpkDeviceName"].ToString();
                        selectedMic = setting["MicDeviceName"].ToString();
                        MicVolume = Convert.ToInt16(setting["MicVolume"]);
                        SpkVolume = Convert.ToInt16(setting["SpkVolume"]);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "loadDevices=Convert.ToInt16", exception, Logger.LogLevel.Error);
                    }
                }

                ComboBoxSpeakers.Items.Clear();
                ComboBoxMicrophones.Items.Clear();
                int num = phoneController.getNumOfPlayoutDevices();
                for (int i = 0; i < num; ++i)
                {
                    StringBuilder deviceName = new StringBuilder();
                    deviceName.Length = 256;

                    if (phoneController.getPlayoutDeviceName(i, deviceName, 256) == 0)
                    {
                        ComboBoxSpeakers.Items.Add(deviceName.ToString());
                    }
                }

                ComboBoxSpeakers.SelectedIndex = selectedSpeaker.Equals(string.Empty) ? 0 : ComboBoxSpeakers.FindString(selectedSpeaker);

                num = phoneController.getNumOfRecordingDevices();
                for (int i = 0; i < num; ++i)
                {
                    StringBuilder deviceName = new StringBuilder();
                    deviceName.Length = 256;

                    if (phoneController.getRecordingDeviceName(i, deviceName, 256) == 0)
                    {
                        ComboBoxMicrophones.Items.Add(deviceName.ToString());
                    }
                }

                ComboBoxMicrophones.SelectedIndex = selectedMic.Equals(string.Empty) ? 0 : ComboBoxMicrophones.FindString(selectedMic);


                TrackBarMicrophone.Value = MicVolume > 0 ? MicVolume : phoneController.getMicVolume();
                TrackBarSpeaker.Value = SpkVolume > 0 ? SpkVolume : phoneController.getSpeakerVolume();

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "loadDevices", exception, Logger.LogLevel.Error);
            }
        }
        
        private void InitializeError(string statusText)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "phoneController_OnInitializeError", Logger.LogLevel.Error);
                _SIPLogined = false;
                agent.AgentCurrentState.OnOffline(ref agent,statusText);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "phoneController_OnInitializeError", exception,
                    Logger.LogLevel.Error);
            }
        }


        private void InitializePhone()
        {
            try
            {
                string userName = agent.SipSetting["userName"].ToString();
                string password = agent.SipSetting["password"].ToString();
                string displayName = agent.SipSetting["displayName"].ToString();
                string authName = agent.SipSetting["authName"].ToString();
                string localPort = agent.SipSetting["localPort"].ToString();
                string sipServerPort = agent.SipSetting["serverPort"].ToString();
                string sipServer = agent.SipSetting["hostName"].ToString();
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}", userName, authName, password, localPort), Logger.LogLevel.Info);

                txtuserNameSIP.Text = userName;
                txtPasswordSIP.Text = password;
                txtDisplayName.Text = displayName;
                txtauthName.Text = authName;
                txtlocalPort.Text = localPort;
                txtserverPort.Text = sipServerPort;
                txtHostName.Text = sipServer;

                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}.............................step 1 : pass.", userName, authName, password, localPort), Logger.LogLevel.Info);
                int errorCode = 0;

                phoneController = new PortSIPLib(0, 0, this);
                UninitializePhone();
                phoneController.createCallbackHandlers();
                var rt = phoneController.initialize(TRANSPORT_TYPE.TRANSPORT_UDP,
                                 PORTSIP_LOG_LEVEL.PORTSIP_LOG_NONE,
                                 Application.StartupPath,
                                 1,
                                 "DuoSoftPhone",
                                 false,
                                 false);
                if (rt != 0)
                {
                    phoneController.releaseCallbackHandlers();
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}.............................Phone Initialization failed. errorCode : {4}", userName, authName, password, localPort, errorCode), Logger.LogLevel.Info);
                    InitializeError("Phone Initialization failed.");
                    return;
                }

                InitAdioWizItems();
                loadDevices();
                phoneController.setAudioDeviceId(0, 0);
                phoneController.setAudioCodecParameter(AUDIOCODEC_TYPE.AUDIOCODEC_AMRWB, "mode-set=0; octet-align=0; robust-sorting=0");
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}.............................step 2 : pass.", userName, authName, password, localPort), Logger.LogLevel.Info);

                var outboundServer = "";
                var outboundServerPort = 0;
                var userDomain = "";

                var rt_userInfo = phoneController.setUser(userName, displayName, authName, password, agent.IP, Convert.ToInt16(localPort), userDomain, sipServer, Convert.ToInt16(sipServerPort), "", 5060, outboundServer, outboundServerPort);

                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}.............................step 4 : Pass.", userName, authName, password, localPort), Logger.LogLevel.Info);
                if (rt_userInfo != 0)
                {
                    phoneController.unInitialize();
                    phoneController.releaseCallbackHandlers();
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}.............................SetUserInfo Failed. errorCode : {4}", userName, authName, password, localPort, errorCode), Logger.LogLevel.Info);
                    InitializeError("Fail to Set User Information's.");
                    return;
                }

                phoneController.setSrtpPolicy(SRTP_POLICY.SRTP_POLICY_NONE);
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}..............................step 5 : Pass.", userName, authName, password, localPort), Logger.LogLevel.Info);
                string licenseKey = LicenseKeyHandler.GetLicenseKey("DuoS123");
                rt = phoneController.setLicenseKey(licenseKey);

                if (rt == PortSIP_Errors.ECoreTrialVersionLicenseKey)
                {
                    MessageBox.Show("This sample was built base on evaluation key, which allows only three minutes conversation. The conversation will be cut off automatically after three minutes, then you can't hearing anything. Feel free contact us at: waruna@duosoftware.com to purchase the official version.");
                    this.Text = this.Text + " [built base on evaluation key]";
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "This sample was built base on evaluation key, which allows only three minutes conversation. The conversation will be cut off automatically after three minutes, then you can't hearing anything. Feel free contact us at: waruna@duosoftware.com to purchase the official version.", Logger.LogLevel.Info);
                }
                else if (rt == PortSIP_Errors.ECoreWrongLicenseKey)
                {
                    MessageBox.Show("The wrong license key was detected, please check with waruna@duosoftware.com or support@duosoftware.com");
                    this.Text = this.Text + " [wrong license key was detected]";
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "The wrong license key was detected, please check with waruna@duosoftware.com or support@duosoftware.com", Logger.LogLevel.Info);
                }

                var rt_register = phoneController.registerServer(120, 0);

                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}..............................step 6 : Pass.", userName, authName, password, localPort), Logger.LogLevel.Info);
                if (rt_register != 0)
                {
                    phoneController.unInitialize();
                    phoneController.releaseCallbackHandlers();
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}..............................Registration Failed. errorCode : {4}", userName, authName, password, localPort, errorCode), Logger.LogLevel.Info);
                    InitializeError("Fail to Register With SIP Server.");
                    return;
                }

                phoneController.addSupportedMimeType("INFO", "text", "plain");
                initAutioCodecs();

                phoneController.setSpeakerVolume(26214);//40% volume
                phoneController.setMicVolume(52428);//80%
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}..............................step 7 : Pass.", userName, authName, password, localPort), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "InitializePhone", exception, Logger.LogLevel.Error);
            }
        }

        private void UninitializePhone()
        {
            try
            {
                try
                {
                    phoneController.hangUp(call.portSipSessionId);
                    phoneController.rejectCall(call.portSipSessionId, 486);
                    if (_SIPLogined)
                    {
                        phoneController.unRegisterServer();
                        _SIPLogined = false;
                    }

                    phoneController.unInitialize();
                    phoneController.releaseCallbackHandlers();
                }
                catch (Exception exception)
                {
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "UninitializePhone", exception, Logger.LogLevel.Error);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "UninitializePhone", exception, Logger.LogLevel.Error);
            }
        }

        private void initAutioCodecs()
        {
            phoneController.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_PCMA);
            phoneController.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_PCMU);
            phoneController.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_G729);

            phoneController.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_DTMF); // For RTP event - DTMF (RFC2833)
        }


        #endregion

        
        private void DuoPhoneTilePanel_TileStateChanged(object sender, TileStateChangedEventArgs e)
        {

        }

        private void b(object sender, EventArgs e)
        {

        }

       
    }
}
