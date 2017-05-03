using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DuoSoftware.DuoAuth.RefAuth.Iauth;
using DuoSoftware.DuoLogger;
using DuoSoftware.DuoSoftPhone.Controllers;
using DuoSoftware.DuoSoftPhone.Controllers.AgentStatus;
using DuoSoftware.DuoSoftPhone.Controllers.CallStatus;
using DuoSoftware.DuoSoftPhone.Controllers.PortSIP;
using DuoSoftware.DuoSoftPhone.Controllers.Service;
using DuoSoftware.DuoSoftPhone.refResourceProxy;
using TheCodeKing.Net.Messaging;
using VIBlend.Utilities;

namespace DuoSoftware.DuoSoftPhone.Ui
{
    public partial class FormDialPad : Form, SIPCallbackEvents
    {

        #region private variables

        private bool IsStatusUpdating = false;
        string _tempStatus = string.Empty;
        private int retryCount = 0;
        IXDListener listner;
        private bool DND;
        private PortsipHandler _portsipHandler;
        private string filePath = string.Empty;
        private string _ringInfilePath = string.Empty;
        private bool playRingtone = false;
        private bool playingRingIntone = false;
        private SoundPlayer _wavPlayer;
        private SoundPlayer _wavPlayerRingIn;
        // Create the list to use as the custom source.  
        AutoCompleteStringCollection source = new AutoCompleteStringCollection();
        
        private string _n;
        private int X;
        private int Y;
        
        private ProfileInfo _sipProfile;
        private bool _isPhoneInitialize;
        #endregion

        #region Property

        public UserAuth Auth;
        
        

        private Agent _agent;
        private Call _call;

        #endregion
        
        #region keypad events

        private void buttonAnswer_Click(object sender, EventArgs e)
        {
            if (IsStatusUpdating)
            {
                MessageBox.Show("Internal Operations Pressing. Please wait.", "Duo DialPad", MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }
           MakeCall(textBoxNumber.Text.Trim());
        }

        private void buttonReject_Click(object sender, EventArgs e)
        {
            try
            {
                buttonReject.Enabled = false;
                StopRingTone();
                StopRingInTone();
                _call.CurrentState.OnCallReject(ref _call, _portsipHandler);
                _agent.AgentCurrentState.OnCallReject(ref _agent, _call.CallSessionId);
                buttonAnswer.Enabled = true;
                buttonReject.Enabled = true;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void buttonConference_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, string.Format("Conference_Click-> Session Id : {0} , Status : {1}", _call.CallSessionId, _call.CurrentState), Logger.LogLevel.Info);
                _call.CurrentState.OnCallConference(ref _call,_portsipHandler);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "buttonConference_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void buttonEtl_Click(object sender, EventArgs e)
        {
            try
            {
                _call.CurrentState.OnEndLinkLine(ref _call, _portsipHandler);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "buttonEtl_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void buttonswapCall_Click(object sender, EventArgs e)
        {
            try
            {
                _call.CurrentState.OnSwapReq(ref _call, _portsipHandler);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "buttonswapCall_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void buttontransferCall_Click(object sender, EventArgs e)
        {
            try
            {
                _call.CurrentState.OnTransferReq(ref _call, textBoxNumber.Text, _portsipHandler);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "buttontransferCall_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void buttontransferIvr_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_1_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text = textBoxNumber.Text + "1";
                _call.CurrentState.OnSendDTMF(ref _call, _portsipHandler, 1);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "button_key_1_Click", exception, Logger.LogLevel.Error);
            }

        }

        private void button_key_2_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text = textBoxNumber.Text + "2";
                _call.CurrentState.OnSendDTMF(ref _call, _portsipHandler, 2);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "button_key_2_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_3_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text = textBoxNumber.Text + "3";
                _call.CurrentState.OnSendDTMF(ref _call, _portsipHandler, 3);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "button_key_3_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_4_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text = textBoxNumber.Text + "4";
                _call.CurrentState.OnSendDTMF(ref _call, _portsipHandler, 4);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "button_key_4_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_5_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text = textBoxNumber.Text + "5";
                _call.CurrentState.OnSendDTMF(ref _call, _portsipHandler, 5);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "button_key_5_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_6_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text = textBoxNumber.Text + "6";
                _call.CurrentState.OnSendDTMF(ref _call, _portsipHandler, 6);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "button_key_6_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_7_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text = textBoxNumber.Text + "7";
                _call.CurrentState.OnSendDTMF(ref _call, _portsipHandler, 7);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "button_key_7_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_8_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text = textBoxNumber.Text + "8";
                _call.CurrentState.OnSendDTMF(ref _call, _portsipHandler, 8);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "button_key_8_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_9_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text = textBoxNumber.Text + "9";
                _call.CurrentState.OnSendDTMF(ref _call, _portsipHandler, 9);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "button_key_9_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_star_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text = textBoxNumber.Text + "*";
                _call.CurrentState.OnSendDTMF(ref _call, _portsipHandler, 10);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "button_key_star_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_0_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text = textBoxNumber.Text + "0";
                _call.CurrentState.OnSendDTMF(ref _call, _portsipHandler, 0);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "button_key_0_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_hash_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxNumber.Text = textBoxNumber.Text + "#";
                _call.CurrentState.OnSendDTMF(ref _call, _portsipHandler, 11);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "button_key_hash_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void buttonHold_Click(object sender, EventArgs e)
        {
            try
            {
                _call.CurrentState.OnHold(ref _call, _portsipHandler);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void buttonBackspace_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void dialNumber(bool sendToTapi, string number)
        {
            try
            {
                //phoneController.dialNumber(number, 0, 0, false);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        #endregion

        #region FormEvents

        public FormDialPad(UserAuth auth, string sessionId, ProfileInfo sip, Agent agent)
        {
            try
            {
                _agent = agent;
                _sipProfile = sip;
                Auth = auth;

                _call = new Call(this)
                {
                    CurrentState = new CallIdleState(),
                    SipProfile = _agent.SipProfile,
                    AgentSessionId = _agent.AgentSessionId,
                    CallSessionId = sessionId,
                };

                _agent.CallSessionId = sessionId;
                //this.phoneController = Controllers.AbstractPhoneController.Get();
                //this.tapiController = Controllers.TapiController.Get();
                InitializeComponent();

                ProgressBar.Show();
                ProgressBar.Start();

                var settingObject = System.Configuration.ConfigurationSettings.AppSettings;
                filePath = settingObject["RingToneFilePath"];
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

                _portsipHandler = new PortsipHandler(this);
                _portsipHandler.OnMakeCall += (id, msg, status) =>
                {
                    _call.PortSipSessionId = id;
                    txtStatus.Invoke(new MethodInvoker(delegate { txtStatus.Text = msg; }));
                };
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
                MessageBox.Show("Critical Error. Please Contact your System Administrator.", "Duo Dialer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }
        
        private void FormDialPad_Load(object sender, EventArgs e)
        {
            try
            {
                txtStatus.ForeColor = System.Drawing.Color.DarkMagenta;
                txtStatus.Text = "Initializing...";
                this.ContextMenu = phoner8ClickMenu;
                applyStyle();

                textBoxNumber.AutoCompleteCustomSource = source;
                textBoxNumber.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                textBoxNumber.AutoCompleteSource = AutoCompleteSource.CustomSource;

                ProgressBar.Start();
                ProgressBar.Show();
                PhoneStatus.Image = Properties.Resources.offline;

                new Thread(PhoneInitialize).Start();

                listner = XDListener.CreateListener(XDTransportMode.IOStream, false);


                listner.RegisterChannel("Command");

                listner.MessageReceived += (o, e1) =>
                {

                    if (e1.DataGram.Channel == "Command")
                    {
                        string cli = e1.DataGram.Message;
                        txtStatus.Invoke(new MethodInvoker(delegate
                        {
                            Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, string.Format("Recive no : {0} frm Tappi", cli), Logger.LogLevel.Info);
                            txtStatus.Text = "Tappi System try to Dial Call.";
                            mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Tappi System try to Dial Call.", ToolTipIcon.Info);
                            if (_agent.AgentCurrentState.GetType()==typeof(AgentIdle)&&_call.CurrentState.GetType()==typeof(CallIdleState))
                            {
                                textBoxNumber.Text = cli;
                                MakeCall(cli);
                            }
                            else
                            {
                                txtStatus.Text = "";
                                mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Fail to Make calls. Agent Invalid State.", ToolTipIcon.Info);
                            }

                        }));

                    }

                };

                #region Agent Break request


                _agent.OnAgentBreakEnd += (status, val) =>
                {
                    try
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            if (val)
                            {
                                gbBreakMode.Visible = false;
                                txtStatus.Text = "";
                                textBoxNumber.Text = "";
                                textBoxNumber.BackColor = Color.Black;
                                txtStatus.BackColor = Color.Black;

                                BreakRequestmenuItem.Enabled = true;
                                EndBreakmenuItem.Enabled = false;
                                CancelRequestmenuItem.Enabled = false;

                                BreakRequestmenuItem.Checked = false;
                                CancelRequestmenuItem.Checked = false;
                                EndBreakmenuItem.Checked = false;
                            }
                            else
                            {
                                txtStatus.Text = "Fail to End Break";
                                ShowNotification(txtStatus.Text, ToolTipIcon.Error);
                            }
                        }));

                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "OnAgentBreakEnd", exception, Logger.LogLevel.Error);
                    }
                };

                _agent.OnAgentBreakRequestCancel += (status, val) =>
                {
                    try
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            if (val)
                            {
                                txtStatus.Text = "Break Request Cancel.";

                                BreakRequestmenuItem.Enabled = true;
                                EndBreakmenuItem.Enabled = false;
                                CancelRequestmenuItem.Enabled = false;

                                BreakRequestmenuItem.Checked = false;
                                CancelRequestmenuItem.Checked = false;
                                EndBreakmenuItem.Checked = false;
                            }
                            else
                            {
                                txtStatus.Text = "Fail to Cancel Break Request.";
                            }

                            ShowNotification(txtStatus.Text, ToolTipIcon.Info);
                        }));

                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "OnAgentBreakEnd", exception, Logger.LogLevel.Error);
                    }
                };

                _agent.OnAgentBreakRequestQueued += (status, val) =>
                {
                    try
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            txtStatus.Text = "Break Request Queued.";

                            BreakRequestmenuItem.Enabled = false;
                            EndBreakmenuItem.Enabled = false;
                            CancelRequestmenuItem.Enabled = true;

                            BreakRequestmenuItem.Checked = true;
                            CancelRequestmenuItem.Checked = false;
                            EndBreakmenuItem.Checked = false;
                            ShowNotification(txtStatus.Text, ToolTipIcon.Info);
                        }));

                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "OnAgentBreakRequestQueued", exception, Logger.LogLevel.Error);
                    }
                };

                _agent.OnAgentBreakRequestGranted += (status, val) =>
                {
                    try
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            txtStatus.Text = "Break Mode.";

                            BreakRequestmenuItem.Enabled = false;
                            EndBreakmenuItem.Enabled = true;
                            CancelRequestmenuItem.Enabled = false;

                            BreakRequestmenuItem.Checked = false;
                            CancelRequestmenuItem.Checked = false;
                            EndBreakmenuItem.Checked = false;
                            ShowNotification(txtStatus.Text, ToolTipIcon.Info);
                        }));

                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "OnAgentBreakRequestQueued", exception, Logger.LogLevel.Error);
                    }
                }; 
                #endregion

                #region Agent Status Update

                
                _agent.OnAgentStatusUpdating += () =>
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        IsStatusUpdating = true;
                        _tempStatus = txtStatus.Text;
                        txtStatus.ForeColor = Color.Magenta;
                        txtStatus.Text = "Status Updating.....";
                        mynotifyicon.ShowBalloonTip(10, "Duo Soft Phone", txtStatus.Text, ToolTipIcon.Info);
                    })); 
                    
                };

                _agent.OnAgentStatusUpdated += () => this.Invoke(new MethodInvoker(delegate
                {
                    txtStatus.ForeColor = Color.DarkGreen;
                    txtStatus.Text = _tempStatus;
                    mynotifyicon.ShowBalloonTip(5, "Duo Soft Phone", "Status Updated.", ToolTipIcon.Info);
                    IsStatusUpdating = false;
                }));

                #endregion

                #region Ringtone

                var settingObject = System.Configuration.ConfigurationSettings.AppSettings;
                filePath = settingObject["RingToneFilePath"];
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
                _ringInfilePath = settingObject["RingInToneFilePath"];
                if (!File.Exists(_ringInfilePath))
                {
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, string.Format("PlayRingINTone> Cannot Find File {0}", _ringInfilePath), Logger.LogLevel.Error);
                    _ringInfilePath = string.Format(@"{0}\{1}", Application.StartupPath, "RingIntone.wav");
                    if (File.Exists(_ringInfilePath))
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "PlayRingINTone> Play with default ringIn tone", Logger.LogLevel.Info);
                }

                _wavPlayer = new SoundPlayer
                {
                    SoundLocation = filePath,
                };
                _wavPlayer.LoadCompleted += wavPlayer_LoadCompleted;
                _wavPlayer.LoadAsync();

                _wavPlayerRingIn = new SoundPlayer
                {
                    SoundLocation = _ringInfilePath,
                };
                _wavPlayerRingIn.LoadCompleted += wavPlayer_LoadCompleted;
                _wavPlayerRingIn.LoadAsync();

                #endregion
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "FormDialPad_Load", exception, Logger.LogLevel.Error);
                MessageBox.Show("Critical Error. Please Contact your System Administrator.", "Duo Dialer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        private void FormDialPad_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Minimized;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void FormDialPad_Resize(object sender, EventArgs e)
        {
            try
            {
                if (FormWindowState.Minimized == this.WindowState)
                {
                    // this.Hide();
                    mynotifyicon.Visible = true;
                    mynotifyicon.ShowBalloonTip(3000);

                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void FormDialPad_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                ArdsHandler.ResourceUnregistration(Auth);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void btnBreakMode_Click(object sender, EventArgs e)
        {
            EndBreakmenuItem_Click(sender, e);
        }

        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
        //    try
        //    {
        //        if (this.ClientRectangle.Width > 0 && this.ClientRectangle.Height > 0)
        //        {
        //            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle,
        //                                                                       Color.Black,
        //                                                                       Color.Black,
        //                                                                       90F))
        //            {
        //                e.Graphics.FillRectangle(brush, this.ClientRectangle);
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
        //    }
        //}

        private void mynotifyicon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                Show();
                WindowState = FormWindowState.Normal;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }
    
        private void btnMore_Click(object sender, EventArgs e)
        {
            //PlayRingTone();
        }

        private void accountSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ProgressBar.Show();
                ProgressBar.Start();
                _portsipHandler.Uninitialize();
                PhoneStatus.Image = Properties.Resources.offline;
                var account = new frmPhoneConfig(Auth) { StartPosition = FormStartPosition.CenterParent };
                account.FormClosing += (o, t) =>
                {
                    try
                    {
                        txtStatus.Invoke(new MethodInvoker(delegate
                        {
                            txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                            txtStatus.Text = "Initializing";
                            mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Phone Reinitializing.", ToolTipIcon.Info);
                            _sipProfile = account.SipProfile;
                        }));
                        _portsipHandler.ReInitializeSetting(_sipProfile.userName, _sipProfile.displayName, _sipProfile.authName, _sipProfile.password, _sipProfile.localPort, _sipProfile.serverPort, _sipProfile.hostName);
                        _agent.AgentCurrentState.OnResetStatus(ref _agent);
                        _call.CurrentState.OnResetStatus(ref _call);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "accountSettingToolStripMenuItem_Click", exception, Logger.LogLevel.Error);
                    }
                };
                account.ShowDialog(this);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void volumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //TrackBarMicrophone.Value = ConvertSystemVolumToPortsip(phoneController.getVolumeMic());
                //TrackBarSpeaker.Value = ConvertSystemVolumToPortsip(phoneController.getVolumeSpeaker());
                pnlVolume.Location = new Point(1, Y);
                pnlVolume.Visible = true;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "volumeToolStripMenuItem_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void btnVolumeClose_Click(object sender, EventArgs e)
        {
            try
            {
                pnlVolume.Visible = false;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "btnVolumeClose_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void FormDialPad_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                pnlVolume.Visible = false;
                X = e.X;
                Y = e.Y;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "FormDialPad_MouseUp", exception, Logger.LogLevel.Error);
            }
        }

        private void TrackBarSpeaker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                ////phoneController.setVolumeSpeaker(TrackBarSpeaker.Value);
                volume.SetToolTip(TrackBarSpeaker, string.Format("{0}%", DisplayValue(TrackBarSpeaker.Value)));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "TrackBarSpeaker_ValueChanged", exception, Logger.LogLevel.Error);
            }
        }

        private int DisplayValue(int val)
        {
            return (int)(decimal.Divide(val, 65535) * 100);
        }
        
        private void TrackBarMicrophone_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                // //phoneController.setVolumeMic(TrackBarMicrophone.Value);
                volume.SetToolTip(TrackBarMicrophone, string.Format("{0}%", DisplayValue(TrackBarMicrophone.Value)));
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
                //phoneController.MuteMicrophone(CheckBoxMute.Checked);
                picMic.Visible = CheckBoxMute.Checked;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "CheckBoxMute_CheckedChanged", exception, Logger.LogLevel.Error);
            }
        }

        private void chkSpeakerMute_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //phoneController.MuteSpeaker(chkSpeakerMute.Checked);
                picSpek.Visible = chkSpeakerMute.Checked;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "chkSpeakerMute_CheckedChanged", exception, Logger.LogLevel.Error);
            }
        }

        private void dNDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DND = !DND;
            //phoneController.SetDnd(DND);
            if (DND)
            {
                textBoxNumber.BackColor = Color.DarkRed;
                txtStatus.BackColor = Color.DarkRed;
                textBoxNumber.Text = "DND Enabled";
                txtStatus.Text = "DND Enabled";
                PhoneStatusMsg.Text = "DND";
            }
            else
            {
                textBoxNumber.BackColor = Color.Black;
                txtStatus.BackColor = Color.Black;
                textBoxNumber.Text = "";
                txtStatus.Text = "";
                PhoneStatusMsg.Text = "";
            }
        }

        private void BreakRequestmenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _agent.AgentCurrentState.OnRequestAgentBreak(ref _agent,AgentMode.Inbound, _call.CallSessionId);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "BreakToolStripMenuItem_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void CancelRequestmenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _agent.AgentCurrentState.OnEndBreak(ref _agent);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "CancelRequestmenuItem_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void EndBreakmenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _agent.AgentCurrentState.OnEndBreak(ref _agent);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "EndBreakmenuItem_Click", exception, Logger.LogLevel.Error);
            }
        }
        
        private void wavPlayer_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "wavPlayer_LoadCompleted", Logger.LogLevel.Error);
                //if (playingRingIntone)
                //    ((SoundPlayer) sender).PlayLooping();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "wavPlayer_LoadCompleted", exception, Logger.LogLevel.Error);
            }
        }

        private void RingtoneOnMenuItem_Click(object sender, EventArgs e)
        {
            EnableRingtone();
        }

        private void RingtoneOffmenuItem_Click(object sender, EventArgs e)
        {
            DisableRingtone();
        }

        private void TrackBarSpeaker_MouseEnter(object sender, EventArgs e)
        {
            volume.SetToolTip(TrackBarSpeaker, string.Format("{0}%", DisplayValue(TrackBarSpeaker.Value)));
        }

        private void TrackBarMicrophone_MouseEnter(object sender, EventArgs e)
        {
            volume.SetToolTip(TrackBarMicrophone, string.Format("{0}%", DisplayValue(TrackBarMicrophone.Value)));
        }

        #endregion

        #region SIPCallbackEvents

        private void PhoneInitialize()
        {
            try
            {
                _isPhoneInitialize = _portsipHandler.InitializePhone(_sipProfile.userName, _sipProfile.displayName, _sipProfile.authName, _sipProfile.password, _sipProfile.localPort, _sipProfile.serverPort, _sipProfile.hostName);
                this.Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = _isPhoneInitialize ? Color.DarkGreen : Color.DarkRed;
                    txtStatus.Text = _isPhoneInitialize ? "Phone Initialized" : "Fail to Initialize Phone";
                    mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", txtStatus.Text, ToolTipIcon.Info);
                    //PhoneStatus.Image = _isPhoneInitialize ? Properties.Resources.online : Properties.Resources.offline;
                })));

              
            }
            catch (Exception exception)
            {
                MessageBox.Show("Fail to Initialize Phone", "Duo Soft Phone", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public int onACTVTransferFailure(int callbackObject, int sessionId, int statusCode, string statusText)
        {
            return 0;
        }

        public int onACTVTransferSuccess(int callbackObject, int sessionId)
        {
            return 0;
        }

        public int onArrivedSignaling(int callbackObject, int sessionId, StringBuilder signaling)
        {
            return 0;
        }

        public int onAudioRawCallback(IntPtr callbackObject, int sessionId, int callbackType, byte[] data, int dataLength,
            int samplingFreqHz)
        {
            return 0;
        }

        public int onInviteAnswered(int callbackObject, int sessionId, bool hasVideo, int statusCode, string statusText,
            string audioCodecName, string videoCodecName)
        {
            ShowNotification(statusText,ToolTipIcon.Info);
            StopRingInTone();
            _agent.AgentCurrentState.OnAnswerCall(ref _agent, _call.CallSessionId);
            _call.CurrentState.OnAnswer(ref _call, _portsipHandler);
            return 0;
        }

        public int onInviteBeginingForward(int callbackObject, string forwardingTo)
        {
            return 0;
        }

        public int onInviteClosed(int callbackObject, int sessionId)
        {
            ShowNotification("Disconnect", ToolTipIcon.Info);
            StopRingInTone();
            _call.CurrentState.OnDisconnected(ref _call, _portsipHandler);
            _agent.AgentCurrentState.OnEndCall(ref _agent, _call.CallSessionId);
            _agent.AgentCurrentState.OnEndCallLog(ref _agent, _call.CallSessionId);
            ShowNotification("Disconnect.", ToolTipIcon.Info);
            return 0;
        }

        public int onInviteFailure(int callbackObject, int sessionId, int statusCode, string statusText)
        {
            ShowNotification(statusText, ToolTipIcon.Info);
            StopRingInTone();
            _call.CurrentState.OnTimeout(ref _call, ref _agent);
            ShowNotification(statusText, ToolTipIcon.Info);
            return 0;
        }

        public int onInviteIncoming(int callbackObject, int sessionId, string caller, string callerDisplayName, string callee,
            string calleeDisplayName, string audioCodecName, string videoCodecName, bool hasVideo)
        {

            try
            {
                _call.PortSipSessionId = sessionId;
                if (DND)
                {
                    string reason = "Busy here";
                    _portsipHandler.RejectCall(sessionId);
                    return -1;
                }

                this.Invoke(((MethodInvoker)(() =>
                {
                    if (playRingtone)
                        PlayRingTone();
                    //SessionId + "|" + caller + "|" + callerDisplayName;
                    txtStatus.ForeColor = System.Drawing.Color.DarkMagenta;
                    txtStatus.Text = "Call Incoming : " + callerDisplayName;
                    _call.CallSessionId = caller.Split('@')[0];// splits[1].Split('@')[0];
                    _agent.CallSessionId = caller.Split('@')[0];
                    new Thread(() => CallEngagementService(Auth.SecurityToken, _call.CallSessionId)).Start();
                })));
                _call.CurrentState.OnRinging(ref _call, sessionId, caller, callerDisplayName, callee, calleeDisplayName);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        private void CallEngagementService(string securityToken, string callSessionId)
        {
            try
            {
                var phnNo = EngagementHandler.GetPhoneNo(securityToken, callSessionId);
                txtStatus.Invoke(new MethodInvoker(delegate
                {
                    txtStatus.Text = "Incoming Call : " + phnNo;
                    textBoxNumber.Text = phnNo;
                }));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "CallEngagementService", exception, Logger.LogLevel.Error);
            }
        }

        public int onInviteRinging(int callbackObject, int sessionId, bool hasEarlyMedia, bool hasVideo, string audioCodecName,
            string videoCodecName)
        {
            PlayRingInTone();
            
            _call.CurrentState.OnRinging(ref _call, sessionId, "", "", "", "");
            ShowNotification("Invite Ringing", ToolTipIcon.Info);
            return 0;
        }

        public int onInviteTrying(int callbackObject, int sessionId, string caller, string callee)
        {
            PlayRingInTone();

            _call.CurrentState.OnRinging(ref _call, 0, "", "", "", ""); ShowNotification("Invite Trying", ToolTipIcon.Info);
            return 0;
        }

        public int onInviteUACConnected(int callbackObject, int sessionId, int statusCode, string statusText)
        {
            return 0;
        }

        public int onInviteUASConnected(int callbackObject, int sessionId, int statusCode, string statusText)
        {
            return 0;
        }

        public int onInviteUpdated(int callbackObject, int sessionId, bool hasVideo, string audioCodecName, string videoCodecName)
        {
            return 0;
        }

        public int onPASVTransferFailure(int callbackObject, int sessionId, int statusCode, string statusText)
        {
            return 0;
        }

        public int onPASVTransferSuccess(int callbackObject, int sessionId, bool hasVideo)
        {
            return 0;
        }

        public int onPlayAviFileFinished(IntPtr callbackObject, int sessionId)
        {
            return 0;
        }

        public int onPlayWaveFileFinished(IntPtr callbackObject, int sessionId, string fileName)
        {
            return 0;
        }

        public int onPresenceOffline(int callbackObject, string @from, string fromDisplayName)
        {
            return 0;
        }

        public int onPresenceOnline(int callbackObject, string @from, string fromDisplayName, string stateText)
        {
            return 0;
        }

        public int onPresenceRecvSubscribe(int callbackObject, int subscribeId, string @from, string fromDisplayName, string subject)
        {
            return 0;
        }

        public int onRecvBinaryMessage(int callbackObject, int sessionId, StringBuilder message, byte[] messageBody, int length)
        {
            return 0;
        }

        public int onRecvBinaryPagerMessage(int callbackObject, StringBuilder @from, StringBuilder fromDisplayName, byte[] messageBody,
            int length)
        {
            return 0;
        }

        public int onRecvDtmfTone(int callbackObject, int sessionId, int tone)
        {
            return 0;
        }

        public int onRecvInfo(int callbackObject, int sessionId, StringBuilder infoMessage)
        {
            ShowNotification("Message Receive.",ToolTipIcon.Info);
            RecieveMessage(callbackObject, "", "", infoMessage);
            return 0;
        }

        public int onRecvMessage(int callbackObject, int sessionId, StringBuilder message)
        {
            ShowNotification("Message Receive.", ToolTipIcon.Info);
            RecieveMessage(callbackObject, "", "", message);
            return 0;
        }

        public int onRecvOptions(int callbackObject, StringBuilder optionsMessage)
        {
            return 0;
        }

        public int onRecvPagerMessage(int callbackObject, string @from, string fromDisplayName, StringBuilder message)
        {
            ShowNotification("Message Receive.", ToolTipIcon.Info);
            RecieveMessage(callbackObject, @from, fromDisplayName, message);
            return 0;
        }


        public int onRegisterFailure(int callbackObject, int statusCode, string statusText)
        {
            try
            {
                ShowNotification(statusText, ToolTipIcon.Error);
                retryCount++;
                this.Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = Color.DarkGreen;
                    txtStatus.Text = "Fail to Register with SIP Server. Retrying... Count : "+retryCount;
                    mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", txtStatus.Text, ToolTipIcon.Error);
                    ProgressBar.Stop();
                    ProgressBar.Hide();
                    PhoneStatus.Image = Properties.Resources.offline;
                })));
                
                if (retryCount > 3)
                {
                    _portsipHandler.Uninitialize();
                    txtStatus.Text = "Error on Initializing. exceed maximum retry count. please contact your system administrator.";
                    mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", txtStatus.Text, ToolTipIcon.Error);
                    Thread.Sleep(1000);
                    Environment.Exit(0);
                }
                _portsipHandler.ReInitializeSetting(_sipProfile.userName, _sipProfile.displayName, _sipProfile.authName, _sipProfile.password, _sipProfile.localPort, _sipProfile.serverPort, _sipProfile.hostName);
                
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "onRegisterFailure", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onRegisterSuccess(int callbackObject, int statusCode, string statusText)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    ProgressBar.Stop();
                    ProgressBar.Hide();
                    ShowNotification(statusText, ToolTipIcon.Info);

                    _tempStatus = txtStatus.Text;
                    txtStatus.ForeColor = Color.Magenta;
                    txtStatus.Text = "Status Updating.....";
                    mynotifyicon.ShowBalloonTip(20, "Duo Soft Phone", txtStatus.Text, ToolTipIcon.Info);
                    _agent.AgentCurrentState.OnLoggedOn(ref _agent, _call.CallSessionId);
                    txtStatus.ForeColor = Color.DarkGreen;
                    txtStatus.Text = "Phone Initialized.";
                    mynotifyicon.ShowBalloonTip(5, "Duo Soft Phone", "Phone Initialized.", ToolTipIcon.Info);
                    PhoneStatus.Image = Properties.Resources.online;
                }));
               
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "onRegisterSuccess", exception,
                    Logger.LogLevel.Error);
            }
            return 0;
        }


        public int onRemoteHold(int callbackObject, int sessionId)
        {
            return 0;
        }

        public int onRemoteUnHold(int callbackObject, int sessionId)
        {
            return 0;
        }

        public int onSendPagerMessageFailure(int callbackObject, string caller, string callerDisplayName, string callee,
            string calleeDisplayName, int statusCode, string statusText)
        {
            return 0;
        }

        public int onSendPagerMessageSuccess(int callbackObject, string caller, string callerDisplayName, string callee,
            string calleeDisplayName)
        {
            return 0;
        }

        public int onTransferRinging(int callbackObject, int sessionId, bool hasVideo)
        {
            return 0;
        }

        public int onTransferTrying(int callbackObject, int sessionId, string referTo)
        {
            return 0;
        }

        public int onVideoRawCallback(IntPtr callbackObject, int sessionId, int callbackType, int width, int height, byte[] data,
            int dataLength)
        {
            return 0;
        }

        public int onWaitingFaxMessage(int callbackObject, string messageAccount, int urgentNewMessageCount, int urgentOldMessageCount,
            int newMessageCount, int oldMessageCount)
        {
            return 0;
        }

        public int onWaitingVoiceMessage(int callbackObject, string messageAccount, int urgentNewMessageCount,
            int urgentOldMessageCount, int newMessageCount, int oldMessageCount)
        {
            return 0;
        }

        #endregion

        #region Set UI State

        public void SetDialPadUi(CallState value)
        {
            try
            {
                this.Invoke(
                    new MethodInvoker(() =>
                    {
                        DialPadDisable();
                        new SwitchOnType<CallState>(value)
                            //.Case<CallAgentClintConnectedState>(ac => SetAgentClinetConnectDialPad()) // enable only operation succeed.
                            //.Case<CallAgentSupConnectedState>(asc => SetAgentSupConnectDialPad())
                            .Case<CallConferenceState>(conf => SetConferenceDailPad())
                            .Case<CallConnectedState>(cont => SetConnectDialPad())
                            .Case<CallDisconnectedState>(disc => SetDisconnectDialPad())
                            .Case<CallHoldState>(spotTrade => SetHoldDialPad())
                            .Case<CallRingingState>(spotTrade => SetRingingDialPad())
                            .Case<CallTryingState>(spotTrade => SetTryingDialPad())
                            .Case<CallIdleState>(spotTrade => SetIdleDialPad());
                        //.Default<CallState>(t => DisableRingtone());
                    }));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "SetUI", exception, Logger.LogLevel.Error);
            }
        }

        private void SetIdleDialPad()
        {
            
            try
            {
                buttonHold.Text = "Hold";
                txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                txtStatus.Text = "";
                buttonAnswer.Enabled = true; buttonEtl.Enabled = false;
                
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "SetIdleDialPad", exception, Logger.LogLevel.Error);
            }
        }

        private void SetTryingDialPad()
        {
            try
            {
                buttonReject.Enabled = true; buttonEtl.Enabled = false;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "SetRingingDialPad", exception, Logger.LogLevel.Error);
            }
        }

        private void SetRingingDialPad()
        {
            try
            {
                buttonAnswer.Enabled = true;
                buttonReject.Enabled = true;
                buttonEtl.Enabled = false;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "SetRingingDialPad", exception, Logger.LogLevel.Error);
            }
        }

        private void SetHoldDialPad()
        {
            try
            {
                buttonHold.Enabled = false;
                buttonReject.Enabled = true;
                buttonEtl.Enabled = false;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "SetHoldDialPad", exception, Logger.LogLevel.Error);
            }
        }

        private void SetDisconnectDialPad()
        {
            try
            {
                buttonAnswer.Enabled = true;
                buttonEtl.Enabled = false;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "SetDisconnectDialPad", exception, Logger.LogLevel.Error);
            }
        }

        private void SetConnectDialPad()
        {
            try
            {
                buttonReject.Enabled = true;
                buttonHold.Enabled = true;
                buttonEtl.Enabled = false;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "SetConnectDialPad", exception, Logger.LogLevel.Error);
            }
        }

        private void SetConferenceDailPad()
        {
            try
            {
                buttonReject.Enabled = true;
                buttonEtl.Enabled = true;
                txtStatus.Text = "Conference.";
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "SetConferenceDailPad", exception, Logger.LogLevel.Error);
            }
        }

        private void SetAgentSupConnectDialPad()
        {
            try
            {
                buttonReject.Enabled = true;
                buttonEtl.Enabled = true;
                buttonswapCall.Enabled = true;
                txtStatus.Text = "Agent Supervisor Connected.";
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "SetAgentSupConnectDialPad", exception, Logger.LogLevel.Error);
            }
        }

        private void SetAgentClinetConnectDialPad()
        {
            try
            {
                buttonReject.Enabled = true;
                buttonConference.Enabled = true;
                buttonEtl.Enabled = true;
                buttonswapCall.Enabled = true;
                txtStatus.Text = "Agent Clint Connected.";
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "setAgentClinetConnectDialPad", exception, Logger.LogLevel.Error);
            }
        }

        private void DialPadDisable()
        {
            try
            {
                buttonAnswer.Enabled = false;
                buttonReject.Enabled = false;
                buttonHold.Enabled = false;
                buttontransferCall.Enabled = false;
                buttonConference.Enabled = false;
                buttontransferIvr.Enabled = false;
                buttonEtl.Enabled = false;
                buttonswapCall.Enabled = false;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "DialPadDisable", exception, Logger.LogLevel.Error);
            }
        }

        

        #endregion

        #region Methods

        private void SendMsgToTappi(string no)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, string.Format("Sendn no : {0} To Tappi", no), Logger.LogLevel.Info);
                IXDBroadcast broadcast = XDBroadcast.CreateBroadcast(XDTransportMode.IOStream, false);
                broadcast.SendToChannel("CallerID", no);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "SendMsgToTappi", exception, Logger.LogLevel.Error);
            }
        }

        private void MakeCall(string no)
        {
            try
            {
                source.Add(no);
                if (_agent.AgentCurrentState.GetType() == typeof(AgentIdle))
                {
                    if (_call.CurrentState.GetType() == typeof(CallIdleState))
                    {
                        buttonAnswer.Invoke(new MethodInvoker(delegate
                        {
                            buttonAnswer.Enabled = false;
                            txtStatus.Text = string.Empty;
                        }));
                        //CallSessionId = SessionId.UniqueId(_sipProfile.userName);
                        //new Thread(() => SendMsgToTappi(textBoxNumber.Text.Trim())).Start();
                        _agent.AgentCurrentState.OnMakeCall(ref _agent);
                        _call.CurrentState.OnMakeCall(ref _call, _portsipHandler,no);
                    }
                }
                else if (_call.CurrentState.GetType() == typeof (CallRingingState))   
                {
                    new Thread(() => SendMsgToTappi(textBoxNumber.Text.Trim())).Start();
                    _agent.AgentCurrentState.OnAnswerCall(ref _agent, _call.CallSessionId);
                    _call.CurrentState.OnAnswer(ref _call, _portsipHandler);
                }
                else
                {
                    ShowNotification("Invalid Operation. Please Reset Phone.",ToolTipIcon.Error);
                }

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "MakeCall", exception, Logger.LogLevel.Error);
            }
        }

        private bool DisableRingtone()
        {
            playRingtone = false;
            mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Ring tone Off", ToolTipIcon.Info);
            return true;
        }

        private bool EnableRingtone()
        {
            try
            {
                var settingObject = System.Configuration.ConfigurationSettings.AppSettings;
                filePath = settingObject["RingToneFilePath"];
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
                            mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Ring tone On", ToolTipIcon.Info);
                            return true;
                        }
                    }
                }

                playRingtone = true;
                mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Ring tone On", ToolTipIcon.Info);
                return true;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "EnableRingtone", exception, Logger.LogLevel.Error);
                return false;
            }
        }
        
        private void applyStyle()
        {

            //using vblend styles downloaded from http://www.viblend.com/products/net/windows-forms/controls/free-winforms-controls.aspx

            FillStyleGradientEx highlightGradient = new FillStyleGradientEx(Color.LightGreen, Color.GreenYellow, Color.Green, Color.DarkGreen, 90f, 0.2f, 0.3f);
            FillStyleGradientEx defaultGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.3f, 0.5f);
            FillStyleGradientEx pressedGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.4f, 0.5f);
            FillStyleGradientEx disabledGradient = new FillStyleGradientEx(Color.Silver, Color.Silver, Color.Silver, Color.Silver, 90f, 0.4f, 0.5f);
            ControlTheme theme = ControlTheme.GetDefaultTheme(VIBLEND_THEME.STEEL);
            theme.StyleHighlight.FillStyle = highlightGradient;
            theme.StyleDisabled.FillStyle = disabledGradient;
            theme.StylePressed.FillStyle = pressedGradient;
            theme.StyleNormal.FillStyle = defaultGradient;
            this.buttonAnswer.StyleKey = "answerStyle";
            this.buttonAnswer.Theme = theme;
            this.buttonAnswer.UseThemeTextColor = false;
            this.buttonAnswer.HighlightTextColor = Color.White;
            this.buttonAnswer.ForeColor = Color.White;
            this.buttonAnswer.PressedTextColor = Color.White;

            gbVolume.Theme = theme;

            ProgressBar.UseThemeBackground = true;
            ProgressBar.Theme = theme;
            ProgressBar.Hide();

            FillStyleGradientEx rejecthighlightGradient = new FillStyleGradientEx(Color.OrangeRed, Color.OrangeRed, Color.DarkRed, Color.DarkRed, 90f, 0.2f, 0.3f);
            FillStyleGradientEx rejectdefaultGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.3f, 0.5f);
            FillStyleGradientEx rejectpressedGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.4f, 0.5f);
            FillStyleGradientEx rejectdisabledGradient = new FillStyleGradientEx(Color.Silver, Color.Silver, Color.Silver, Color.Silver, 90f, 0.4f, 0.5f);
            ControlTheme rejecttheme = ControlTheme.GetDefaultTheme(VIBLEND_THEME.STEEL);
            rejecttheme.StyleHighlight.FillStyle = rejecthighlightGradient;
            rejecttheme.StyleDisabled.FillStyle = rejectdisabledGradient;
            rejecttheme.StylePressed.FillStyle = rejectpressedGradient;
            rejecttheme.StyleNormal.FillStyle = rejectdefaultGradient;
            this.buttonReject.StyleKey = "rejectStyle";
            this.buttonReject.Theme = rejecttheme;
            this.buttonReject.UseThemeTextColor = false;
            this.buttonReject.HighlightTextColor = Color.White;
            this.buttonReject.ForeColor = Color.White;
            this.buttonReject.PressedTextColor = Color.White;
            FillStyleGradientEx numhighlightGradient = new FillStyleGradientEx(Color.Blue, Color.Blue, Color.Blue, Color.Blue, 90f, 0.2f, 0.3f);
            FillStyleGradientEx numdefaultGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.3f, 0.5f);
            FillStyleGradientEx numpressedGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.4f, 0.5f);
            FillStyleGradientEx numdisabledGradient = new FillStyleGradientEx(Color.Silver, Color.Silver, Color.Silver, Color.Silver, 90f, 0.4f, 0.5f);
            ControlTheme numtheme = ControlTheme.GetDefaultTheme(VIBLEND_THEME.STEEL);
            numtheme.StyleHighlight.FillStyle = numhighlightGradient;
            numtheme.StyleDisabled.FillStyle = numdisabledGradient;
            numtheme.StylePressed.FillStyle = numpressedGradient;
            numtheme.StyleNormal.FillStyle = numdefaultGradient;

            this.button_key_1.StyleKey = "numstyle";
            this.button_key_1.Theme = numtheme;
            this.button_key_1.UseThemeTextColor = false;
            this.button_key_1.HighlightTextColor = Color.White;
            this.button_key_1.ForeColor = Color.White;
            this.button_key_1.PressedTextColor = Color.White;
            this.button_key_2.StyleKey = "numstyle";
            this.button_key_2.Theme = numtheme;
            this.button_key_2.UseThemeTextColor = false;
            this.button_key_2.HighlightTextColor = Color.White;
            this.button_key_2.ForeColor = Color.White;
            this.button_key_2.PressedTextColor = Color.White;
            this.button_key_3.StyleKey = "numstyle";
            this.button_key_3.Theme = numtheme;
            this.button_key_3.UseThemeTextColor = false;
            this.button_key_3.HighlightTextColor = Color.White;
            this.button_key_3.ForeColor = Color.White;
            this.button_key_3.PressedTextColor = Color.White;
            this.button_key_4.StyleKey = "numstyle";
            this.button_key_4.Theme = numtheme;
            this.button_key_4.UseThemeTextColor = false;
            this.button_key_4.HighlightTextColor = Color.White;
            this.button_key_4.ForeColor = Color.White;
            this.button_key_4.PressedTextColor = Color.White;
            this.button_key_5.StyleKey = "numstyle";
            this.button_key_5.Theme = numtheme;
            this.button_key_5.UseThemeTextColor = false;
            this.button_key_5.HighlightTextColor = Color.White;
            this.button_key_5.ForeColor = Color.White;
            this.button_key_5.PressedTextColor = Color.White;
            this.button_key_6.StyleKey = "numstyle";
            this.button_key_6.Theme = numtheme;
            this.button_key_6.UseThemeTextColor = false;
            this.button_key_6.HighlightTextColor = Color.White;
            this.button_key_6.ForeColor = Color.White;
            this.button_key_6.PressedTextColor = Color.White;
            this.button_key_7.StyleKey = "numstyle";
            this.button_key_7.Theme = numtheme;
            this.button_key_7.UseThemeTextColor = false;
            this.button_key_7.HighlightTextColor = Color.White;
            this.button_key_7.ForeColor = Color.White;
            this.button_key_7.PressedTextColor = Color.White;
            this.button_key_8.StyleKey = "numstyle";
            this.button_key_8.Theme = numtheme;
            this.button_key_8.UseThemeTextColor = false;
            this.button_key_8.HighlightTextColor = Color.White;
            this.button_key_8.ForeColor = Color.White;
            this.button_key_8.PressedTextColor = Color.White;
            this.button_key_9.StyleKey = "numstyle";
            this.button_key_9.Theme = numtheme;
            this.button_key_9.UseThemeTextColor = false;
            this.button_key_9.HighlightTextColor = Color.White;
            this.button_key_9.ForeColor = Color.White;
            this.button_key_9.PressedTextColor = Color.White;
            this.button_key_0.StyleKey = "numstyle";
            this.button_key_0.Theme = numtheme;
            this.button_key_0.UseThemeTextColor = false;
            this.button_key_0.HighlightTextColor = Color.White;
            this.button_key_0.ForeColor = Color.White;
            this.button_key_0.PressedTextColor = Color.White;

            this.btnBreakMode.StyleKey = "numstyle";
            this.btnBreakMode.Theme = numtheme;
            this.btnBreakMode.UseThemeTextColor = false;
            this.btnBreakMode.HighlightTextColor = Color.White;
            this.btnBreakMode.ForeColor = Color.White;
            this.btnBreakMode.PressedTextColor = Color.White;

            FillStyleGradientEx holdhighlightGradient = new FillStyleGradientEx(Color.OrangeRed, Color.OrangeRed, Color.DarkRed, Color.DarkRed, 90f, 0.2f, 0.3f);
            FillStyleGradientEx holddefaultGradient = new FillStyleGradientEx(Color.Gray, Color.LightGray, Color.Black, Color.Black, 90f, 0.3f, 0.5f);
            FillStyleGradientEx holdpressedGradient = new FillStyleGradientEx(Color.Gray, Color.LightGray, Color.Black, Color.Black, 90f, 0.4f, 0.5f);
            FillStyleGradientEx holddisabledGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.4f, 0.5f);
            ControlTheme holdtheme = ControlTheme.GetDefaultTheme(VIBLEND_THEME.STEEL);
            holdtheme.StyleHighlight.FillStyle = holdhighlightGradient;
            holdtheme.StyleDisabled.FillStyle = holddisabledGradient;
            holdtheme.StylePressed.FillStyle = holdpressedGradient;
            holdtheme.StyleNormal.FillStyle = holddefaultGradient;
            this.buttonHold.StyleKey = "holdStyle";
            this.buttonHold.Theme = holdtheme;
            this.buttonHold.UseThemeTextColor = false;
            this.buttonHold.HighlightTextColor = Color.White;
            this.buttonHold.ForeColor = Color.White;
            this.buttonHold.PressedTextColor = Color.White;


            FillStyleGradientEx starhighlightGradient = new FillStyleGradientEx(Color.Red, Color.Red, Color.DarkRed, Color.DarkRed, 90f, 0.2f, 0.3f);
            FillStyleGradientEx stardefaultGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.3f, 0.5f);
            FillStyleGradientEx starpressedGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.4f, 0.5f);
            FillStyleGradientEx stardisabledGradient = new FillStyleGradientEx(Color.Silver, Color.Silver, Color.Silver, Color.Silver, 90f, 0.4f, 0.5f);
            ControlTheme startheme = ControlTheme.GetDefaultTheme(VIBLEND_THEME.STEEL);
            startheme.StyleHighlight.FillStyle = starhighlightGradient;
            startheme.StyleDisabled.FillStyle = stardisabledGradient;
            startheme.StylePressed.FillStyle = starpressedGradient;
            startheme.StyleNormal.FillStyle = stardefaultGradient;
            this.button_key_star.StyleKey = "starStyle";
            this.button_key_star.Theme = startheme;
            this.button_key_star.UseThemeTextColor = false;
            this.button_key_star.HighlightTextColor = Color.White;
            this.button_key_star.ForeColor = Color.White;
            this.button_key_star.PressedTextColor = Color.White;


            FillStyleGradientEx hashhighlightGradient = new FillStyleGradientEx(Color.Red, Color.Red, Color.DarkRed, Color.DarkRed, 90f, 0.2f, 0.3f);
            FillStyleGradientEx hashdefaultGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.3f, 0.5f);
            FillStyleGradientEx hashpressedGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.4f, 0.5f);
            FillStyleGradientEx hashdisabledGradient = new FillStyleGradientEx(Color.Silver, Color.Silver, Color.Silver, Color.Silver, 90f, 0.4f, 0.5f);
            ControlTheme hashtheme = ControlTheme.GetDefaultTheme(VIBLEND_THEME.STEEL);
            hashtheme.StyleHighlight.FillStyle = hashhighlightGradient;
            hashtheme.StyleDisabled.FillStyle = hashdisabledGradient;
            hashtheme.StylePressed.FillStyle = hashpressedGradient;
            hashtheme.StyleNormal.FillStyle = hashdefaultGradient;
            this.button_key_hash.StyleKey = "hashthemeStyle";
            this.button_key_hash.Theme = hashtheme;
            this.button_key_hash.UseThemeTextColor = false;
            this.button_key_hash.HighlightTextColor = Color.White;
            this.button_key_hash.ForeColor = Color.White;
            this.button_key_hash.PressedTextColor = Color.White;

            FillStyleGradientEx backhighlightGradient = new FillStyleGradientEx(Color.Red, Color.Red, Color.DarkRed, Color.DarkRed, 90f, 0.2f, 0.3f);
            FillStyleGradientEx backdefaultGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.3f, 0.5f);
            FillStyleGradientEx habackpressedGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.4f, 0.5f);
            FillStyleGradientEx backdisabledGradient = new FillStyleGradientEx(Color.Silver, Color.Silver, Color.Silver, Color.Silver, 90f, 0.4f, 0.5f);
            ControlTheme backtheme = ControlTheme.GetDefaultTheme(VIBLEND_THEME.STEEL);
            hashtheme.StyleHighlight.FillStyle = hashhighlightGradient;
            hashtheme.StyleDisabled.FillStyle = hashdisabledGradient;
            hashtheme.StylePressed.FillStyle = hashpressedGradient;
            hashtheme.StyleNormal.FillStyle = hashdefaultGradient;
            this.buttonBackspace.StyleKey = "backthemeStyle";
            this.buttonBackspace.Theme = hashtheme;
            this.buttonBackspace.UseThemeTextColor = false;
            this.buttonBackspace.HighlightTextColor = Color.White;
            this.buttonBackspace.ForeColor = Color.White;
            this.buttonBackspace.PressedTextColor = Color.White;



            FillStyleGradientEx morehighlightGradient = new FillStyleGradientEx(Color.Red, Color.Red, Color.DarkRed, Color.DarkRed, 90f, 0.2f, 0.3f);
            FillStyleGradientEx morehighlightGradientdefaultGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.3f, 0.5f);
            FillStyleGradientEx morehighlightGradientpressedGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.4f, 0.5f);
            FillStyleGradientEx morehighlightGradientisabledGradient = new FillStyleGradientEx(Color.Silver, Color.Silver, Color.Silver, Color.Silver, 90f, 0.4f, 0.5f);
            ControlTheme moretheme = ControlTheme.GetDefaultTheme(VIBLEND_THEME.STEEL);
            moretheme.StyleHighlight.FillStyle = hashhighlightGradient;
            moretheme.StyleDisabled.FillStyle = hashdisabledGradient;
            moretheme.StylePressed.FillStyle = hashpressedGradient;
            moretheme.StyleNormal.FillStyle = hashdefaultGradient;
            this.btnMore.StyleKey = "morethemethemeStyle";
            this.btnMore.Theme = moretheme;
            this.btnMore.UseThemeTextColor = false;
            this.btnMore.HighlightTextColor = Color.White;
            this.btnMore.ForeColor = Color.White;
            this.btnMore.PressedTextColor = Color.White;



        }

        private int ConvertSystemVolumToPortsip(int val)
        {
            return (int)(decimal.Divide(val, 255) * 65535);
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
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "StopRingTone>Start", Logger.LogLevel.Error);
                if (_wavPlayer == null) return;
                _wavPlayer.Stop();
                _wavPlayer.Dispose();
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "StopRingTone>End", Logger.LogLevel.Error);

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
                if (!File.Exists(_ringInfilePath))
                {
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Start to play ringIn tone- No Audio Files.", Logger.LogLevel.Error);
                    return;
                }
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
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "StopRingInTone>Start", Logger.LogLevel.Error);
                playingRingIntone = false;
                if (_wavPlayerRingIn == null) return;
                _wavPlayerRingIn.Stop();
                _wavPlayerRingIn.Dispose();
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "StopRingInTone>End", Logger.LogLevel.Error);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "StopRingInTone", exception, Logger.LogLevel.Error);
            }
        }
        
        private void RecieveMessage(int callbackObject, string @from, string fromDisplayName, StringBuilder message)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, string.Format("OnRecieveMessage-> status : {0} ", message), Logger.LogLevel.Info);
                
                var fullMessage = Convert.ToString(message);

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
                                _call.CallSessionId = sessionId;
                                _agent.CallSessionId = sessionId;
                                txtStatus.Invoke(new MethodInvoker(delegate
                                {
                                    _tempStatus = txtStatus.Text;
                                    txtStatus.ForeColor = Color.Magenta;
                                    txtStatus.Text = "Status Updating.....";
                                    mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", txtStatus.Text, ToolTipIcon.Info);
                                })); 
                                ArdsHandler.ResourceStatusChangeBusy(Auth, sessionId);
                                txtStatus.Invoke(new MethodInvoker(delegate
                                {
                                    txtStatus.ForeColor = Color.DarkGreen;
                                    txtStatus.Text = _tempStatus;
                                }));
                                break;

                        }
                    }
                    else
                    {
                        var msgString = message.ToString().ToUpper();

                        this.Invoke(new MethodInvoker(delegate
                        {
                            txtStatus.ForeColor = Color.DarkGreen;
                            txtStatus.Text = msgString.Equals("SUCCESS") ? "Operation Succeed." : (msgString.Equals("ROUTEBACK") ? "Route back" : "Operation Fail.");

                            new SwitchOnType<CallState>(_call.CurrentState)
                                .Case<CallAgentClintConnectedState>(ac =>
                                {
                                    if (msgString.Equals("SUCCESS"))
                                    {
                                        SetAgentClinetConnectDialPad();
                                    }
                                    else if (msgString == "FAILED" || msgString == "FAIL")
                                    {
                                        buttonHold.Text = "Hold";
                                        ac.OnTranferFail(ref _call);
                                    }
                                    else if (msgString.Equals("ROUTEBACK"))
                                    {
                                        
                                    }
                                    
                                })
                                .Case<CallAgentSupConnectedState>(asc =>
                                {
                                    if (msgString.Equals("SUCCESS"))
                                    {
                                        SetAgentSupConnectDialPad();
                                    }
                                    
                                    else if (msgString == "FAILED" || msgString == "FAIL")
                                    {
                                        buttonHold.Text = "Hold";
                                       //asc.OnTranferFail(ref _call);
                                    }
                                    else if (msgString.Equals("ROUTEBACK"))
                                    {
                                        asc.OnTranferFail(ref _call);
                                    }
                                })
                                .Case<CallConferenceState>(conf =>
                                {
                                    if (msgString.Equals("SUCCESS"))
                                    {
                                        txtStatus.Text = "Conference.";
                                    }
                                    else if (msgString == "FAILED" || msgString == "FAIL")
                                    {
                                        _call.CurrentState.OnCallConferenceFail(ref _call);
                                    }
                                    else if (msgString.Equals("ROUTEBACK"))
                                    {
                                        conf.OnEndCallSession(ref _call);
                                    }
                                })
                                .Case<CallConnectedState>(cont =>
                                {
                                    if (msgString.Equals("SUCCESS"))
                                    {
                                        buttonHold.Text = "Hold";
                                    }
                                    
                                    else if (msgString == "FAILED" || msgString == "FAIL")
                                    {
                                        buttonHold.Text = "Hold";
                                    }
                                    else if (msgString.Equals("ROUTEBACK"))
                                    {

                                    }
                                })
                                //.Case<CallDisconnectedState>(disc => SetDisconnectDialPad())
                                .Case<CallHoldState>(hold =>
                                {
                                    buttonHold.Enabled = true;
                                    if (msgString.Equals("SUCCESS"))
                                    {
                                        buttontransferCall.Enabled = true;
                                        buttonHold.Text = "Unhold";
                                    }
                                    
                                    else if (msgString == "FAILED" || msgString == "FAIL")
                                    {
                                        buttonHold.Text = "Hold";
                                        hold.OnUnHold(ref _call);
                                    }
                                    else if (msgString.Equals("ROUTEBACK"))
                                    {

                                    }
                                });
                            // .Case<CallRingingState>(spotTrade => SetRingingDialPad())
                            //.Case<CallTryingState>(spotTrade => SetTryingDialPad())
                            //.Case<CallIdleState>(spotTrade => SetIdleDialPad());
                            //.Default<CallState>(t => DisableRingtone());
                        }));

                       
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "RecieveMessage", exception, Logger.LogLevel.Error);
            }
        }
        
        public void ShowNotification(string msg, ToolTipIcon icon)
        {
            try
            {
                txtStatus.Invoke(new MethodInvoker(delegate
                {
                    txtStatus.ForeColor = Color.DarkGreen;
                    txtStatus.Text = msg;
                }));
                mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", msg, icon);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "ShowNotification", exception, Logger.LogLevel.Error);
            }
        }


        #endregion



    }

    public class SwitchOnType<T1>
    {
        bool _break = false;
        private T1 _object;

        public SwitchOnType(T1 obj)
        {
            _object = obj;
        }

        public SwitchOnType<T1> Case<T2>(Action<T2> action) where T2 : class
        {
            if (_break == false)
            {
                T2 t = _object as T2;
                if (t != null)
                {
                    action(t);
                    _break = true;
                }
            }
            return this as SwitchOnType<T1>;
        }
        public void Default<T2>(Action<T2> action) where T2 : class
        {
            if (_break == false)
            {
                T2 t = _object as T2;
                if (t != null)
                    action(t);
            }
        }
    }
}
