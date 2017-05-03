﻿using System.Runtime.InteropServices;
using DuoCallTesterLicenseKey;

using DuoSoftware.DuoSoftPhone.Controllers;
using DuoSoftware.DuoSoftPhone.Controllers.Service;
using DuoSoftware.DuoSoftPhone.refResourceProxy;
using DuoSoftware.DuoSoftPhone.refUserAuth;
using DuoSoftware.DuoTools.DuoLogger;
using PortSIP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using DuoSoftware.DuoSoftPhone.Controllers.AgentStatus;
using DuoSoftware.DuoSoftPhone.Controllers.CallStatus;
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TheCodeKing.Net.Messaging;
using AgentMode = DuoSoftware.DuoSoftPhone.Controllers.AgentStatus.AgentMode;
using Timer = System.Timers.Timer;


namespace DuoSoftware.DuoSoftPhone.Ui
{
    public partial class FormDialPad : Form, SIPCallbackEvents, IUiState
    {

        #region private variables

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetSetCookie(string UrlName, string CookieName, string CookieData);

        private ResourceMonitoringHandler resMonitoringHandler;
        private ResourceCheckerHandler resourceCheckerHandler;

        private int acwTime;
        private int acwCotdown;

        private Agent _agent;
        private Call call;

        private DateTime callStarTime;
        private System.Timers.Timer CallDurations;
        private DateTime freezeStarTime;
        private System.Timers.Timer FreezeDurations;
        private int SipRegisterTryCount = 0;
        private Timer acwCutdownTimer;
        private bool isCallAnswerd;
        private Dictionary<Guid, CallLog> callLogs;

        private string selectedSpeaker = string.Empty;
        private string selectedMic = string.Empty;
        private frmIncomingCall alert;
        private ComboBox ComboBoxMicrophones;
        private ComboBox ComboBoxSpeakers;
        private bool _SIPLogined = false;
        private bool isSipRegistrationOk = false;
        private bool isARDSRegistrationOk = true;
        private TextBox textBoxNumber;



        private IXDListener listner;
        private string filePath = string.Empty;
        private string _ringInfilePath = string.Empty;
        private bool playRingtone = false;
        private bool playRingInToneMenually = false;
        private bool IsNotAllowToReject = false;
        private bool DND;
        private bool playingRingIntone = false;
        private bool ShowCallAlert = false;
        private SoundPlayer _wavPlayer;
        private SoundPlayer _wavPlayerRingIn;

        // Create the list to use as the custom source.
        private AutoCompleteStringCollection source = new AutoCompleteStringCollection();

        private CallHandler callHandler;
        private PortSIPLib phoneController;




        private int X;
        private int Y;
        private int reginTime;


        private string _n;
        private string CurrentNumber
        {
            get
            {
                return _n;
            }
            set
            {
                if (_n == null)
                    _n = "";
                this._n = value;
                textBoxNumber.Text = this._n;
            }
        }


        #endregion private variables

        #region Private Methods

        private void HandleDeviceEvent()
        {
            try
            {
                if (call.CallCurrentState.GetType() != typeof(CallIdleState))
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoSmtp, string.Format("The audio device on the computer [{0}] used by [{1}], was either removed /reconnected during an ongoing call, therefore the   agent console will be disabled  till remedial action is taken.\n Agent Session ID [{2}], Call Session ID : {3} .\n \n\n\n\nTHIS IS AN AUTO-GENERATED MESSAGE - PLEASE DO NOT REPLY TO THIS MESSAGE.\n\n\n\n\n", _agent.IP, _agent.Auth.UserName, "***********", call.CallSessionId), Logger.LogLevel.Error);
                    //The audio device on the computer [{0}] used by [{1}], was either removed or reconnected during an ongoing call, please take necessary steps to resolve the issue. %n%n%n THIS IS AN AUTO-GENERATED MESSAGE - PLEASE DO NOT REPLY TO THIS MESSAGE.
                    //The sound device on [{0}]  used by [{1}] was either forcefully  or unintentionally  disconnected/connected while on call, the console will be frozen till remedial action is taken

                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "HandleDeviceEvent", exception, Logger.LogLevel.Error);
            }
        }


        private void Reregister()
        {
            try
            {
                //UninitializePhone();
                //StopRingInTone();
                //StopRingTone();
                //PhoneStatus.Image = Properties.Resources.offline;
                var account = new frmPhoneConfig(_agent.Auth) { StartPosition = FormStartPosition.CenterParent };
                account.FormClosing += (o, t) =>
                {
                    try
                    {
                        txtStatus.Invoke(new MethodInvoker(delegate
                        {
                            txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                            txtStatus.Text = "Initializing";
                            mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Phone Reinitializing.", ToolTipIcon.Info);
                        }));

                        UninitializePhone();
                        InitializePhone(true);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "accountSettingToolStripMenuItem_Click",
                            exception, Logger.LogLevel.Error);
                    }
                };

                account.ShowDialog(this);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
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
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ViewErrorsLogFile - Find Controler", exception, Logger.LogLevel.Error);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "OpenLogFils", exception, Logger.LogLevel.Error);
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
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "OpenLogFilsDirectory", exception, Logger.LogLevel.Error);
            }
        }

        private void GetPhoneNo(string callSessionId)
        {
            try
            {
                call.PhoneNo = string.Empty;
                var phnNo = EngagementHandler.GetPhoneNo(_agent.Auth.SecurityToken, callSessionId);
                if (_agent.AgentCurrentState.GetType() == typeof(AgentIdle))
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Get info after call Disconnected", Logger.LogLevel.Info);
                    return;
                }

                this.Invoke(((MethodInvoker)(() =>
                {
                    //txtStatus.Text =  "Incoming Call : " + phnNo;
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
                if (AutoAnswer.Checked)
                {
                    MakeCall(phnNo);
                }
                var info = EngagementHandler.ViewCurrentEngagementDisplayInfo(_agent.Auth.SecurityToken, callSessionId);
                mynotifyicon.ShowBalloonTip(100, "Duo Soft Phone", info, ToolTipIcon.Info);

                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Display Call info-Main : {0}:{1}> {2}", callSessionId, phnNo, info), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "GetPhoneNo", exception, Logger.LogLevel.Error);
            }
        }

        private void PlayRingTone()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Start to play ring tone", Logger.LogLevel.Info);
                if (playRingtone)
                    _wavPlayer.PlayLooping();
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "PlayRingTone > End", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "PlayRingTone", exception, Logger.LogLevel.Error);
            }
        }

        private void StopRingTone()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "StopRingTone>Start", Logger.LogLevel.Info);
                if (_wavPlayer == null) return;
                _wavPlayer.Stop();
                _wavPlayer.Dispose();
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "StopRingTone>End", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "StopRingTone", exception, Logger.LogLevel.Error);
            }
        }

        private void PlayRingInTone()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Start to play ringIn tone", Logger.LogLevel.Info);
                if (!playRingInToneMenually)
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Start to play ringIn tone- No Audio Files.", Logger.LogLevel.Error);
                    return;
                }
                if (playingRingIntone)
                    return;
                if (_wavPlayerRingIn == null) return;
                _wavPlayerRingIn.PlayLooping();

                //_wavPlayer = new SoundPlayer
                //    {
                //        SoundLocation = _ringInfilePath,
                //        // @"C:\Users\Public\Music\Sample Music\ALBSlide.wav"
                //    };
                //    _wavPlayer.LoadCompleted += wavPlayer_LoadCompleted;
                //    _wavPlayer.LoadAsync();
                playingRingIntone = true;
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "PlayRingInTone > End", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "PlayRingInTone", exception, Logger.LogLevel.Error);
            }
        }

        private void StopRingInTone()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "StopRingInTone>Start", Logger.LogLevel.Info);
                playingRingIntone = false;
                if (_wavPlayerRingIn == null) return;
                _wavPlayerRingIn.Stop();
                _wavPlayerRingIn.Dispose();
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "StopRingInTone>End", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "StopRingInTone", exception, Logger.LogLevel.Error);
            }
        }

        private void DisableAlert()
        {
            try
            {

                if (alert != null && !alert.IsDisposed)
                    alert.Invoke(new MethodInvoker(delegate { if (alert != null) alert.Close(); }));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "DisableAlert", exception, Logger.LogLevel.Error);
            }
        }

        private string GetString(byte[] bytes)
        {
            return System.Text.Encoding.Default.GetString(bytes);
        }

        private void ReceveMeassge(string status, string fullMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("ReceveMeassge-> status : {0} , DialPadEventArgs: {1} , Agent State : {2} , Agent Mode : {3} , CallSessionId : {4}", status, fullMessage, _agent.AgentCurrentState, _agent.AgentMode, call.CallSessionId), Logger.LogLevel.Info);
                this.txtStatus.Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = System.Drawing.Color.DimGray;
                    txtStatus.Text = status;
                })));

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
                                _agent.CallSessionId = sessionId;
                                _agent.AgentCurrentState.OnMakeCall(ref _agent);
                                call.CallSessionId = sessionId;
                                buttonReject.Invoke(new MethodInvoker(delegate { buttonReject.Enabled = true; }));
                                var jsonString = _agent.PortsipSessionId + "|" + _agent.CallSessionId + "|" + call.PhoneNo;

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
                            mynotifyicon.ShowBalloonTip(2, "Duo Soft Phone", "Operation Fail.", ToolTipIcon.Error);
                        }
                        else if (msgString.ToUpper() == "ROUTEBACK" || msgString == "CALLFAIL") 
                        {
                            call.CallCurrentState.OnReset(ref call);
                            txtStatus.Invoke(new MethodInvoker(delegate
                            {
                                txtStatus.ForeColor = Color.DarkGreen;
                                txtStatus.Text = Environment.NewLine + "Call Route Back.";
                            }));
                            mynotifyicon.ShowBalloonTip(2, "Duo Soft Phone", "Call Route Back.", ToolTipIcon.Info);
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
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ReceveMeassge", exception, Logger.LogLevel.Error);
            }
        }

        void StartDurations()
        {
            try
            {
                acwCutdownTimer.Stop();
                acwCutdownTimer.Enabled = false;

                FreezeDurations.Stop();
                FreezeDurations.Enabled = false;
                freezeStarTime = DateTime.Now;
                FreezeDurations.Enabled = true;
                FreezeDurations.Start();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "StartDurations", exception, Logger.LogLevel.Error);
            }
        }

        void StoptDurations()
        {
            try
            {
                FreezeDurations.Stop();
                FreezeDurations.Enabled = false;

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "StoptDurations", exception, Logger.LogLevel.Error);
            }
        }

        private void AddOutgoingCallToCallLogs(string no)
        {
            try
            {
                lock (callLogs)
                {
                    callLogs.Add(call.currentCallLogId, new CallLog { Direction = 1, Durations = 0, PhoneNo = no, time = DateTime.Now });
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "AddOutgoingCallToCallLogs", exception, Logger.LogLevel.Error);
            }
        }

        private void AddIncommingToCallLogs(string no)
        {
            try
            {
                lock (callLogs)
                {
                    if (callLogs.ContainsKey(call.currentCallLogId))
                    {
                        callLogs.Remove(call.currentCallLogId);
                    }
                    callLogs.Add(call.currentCallLogId, new CallLog { Direction = 0, Durations = 0, PhoneNo = no, time = DateTime.Now });
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "AddIncommingToCallLogs", exception, Logger.LogLevel.Error);
            }
        }

        private void AddCallDurations()
        {
            try
            {
                var log = callLogs[call.currentCallLogId];
                log.Durations = Math.Round(DateTime.Now.Subtract(log.time).TotalSeconds, 2);
                lock (callLogs)
                {
                    callLogs.Remove(call.currentCallLogId);
                    callLogs.Add(call.currentCallLogId, log);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "AddCallDurations", exception, Logger.LogLevel.Error);
            }
        }

        private int _missCallCount = 0;
        private void AddMiscallToCallLogs()
        {
            try
            {
                var log = callLogs[call.currentCallLogId];
                log.Direction = 2;
                lock (callLogs)
                {
                    callLogs.Remove(call.currentCallLogId);
                    callLogs.Add(call.currentCallLogId, log);
                }
                if (_missCallCount > 3)
                {
                    _agent.AgentCurrentState.OnOffline(ref _agent, "Exceed Maximum Miscall Count");
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "AddMiscallToCallLogs", exception, Logger.LogLevel.Error);
            }
        }

        private void InProgressState()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    buttonHold.Enabled = false;
                    buttontransferCall.Enabled = false;
                    buttonConference.Enabled = false;
                    buttontransferIvr.Enabled = false;
                    buttonEtl.Enabled = false;
                    buttonswapCall.Enabled = false;
                }));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InProgressState", exception, Logger.LogLevel.Error);
            }
        }

        private void InitAdioWizItems()
        {
            this.ComboBoxMicrophones = new ComboBox();
            this.ComboBoxSpeakers = new ComboBox();
            //
            // ComboBoxMicrophones
            //
            this.ComboBoxMicrophones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxMicrophones.FormattingEnabled = true;
            this.ComboBoxMicrophones.Location = new System.Drawing.Point(90, 42);
            this.ComboBoxMicrophones.Name = "ComboBoxMicrophones";
            this.ComboBoxMicrophones.Size = new System.Drawing.Size(308, 23);
            this.ComboBoxMicrophones.TabIndex = 49;


            //
            // ComboBoxSpeakers
            //
            this.ComboBoxSpeakers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxSpeakers.FormattingEnabled = true;
            this.ComboBoxSpeakers.Location = new System.Drawing.Point(90, 16);
            this.ComboBoxSpeakers.Name = "ComboBoxSpeakers";
            this.ComboBoxSpeakers.Size = new System.Drawing.Size(308, 23);
            this.ComboBoxSpeakers.TabIndex = 48;

            this.ComboBoxMicrophones.SelectedIndexChanged += (s, e) =>
            {
                try
                {
                    phoneController.setAudioDeviceId(ComboBoxMicrophones.SelectedIndex, ComboBoxSpeakers.SelectedIndex);
                    selectedMic = ComboBoxMicrophones.SelectedItem.ToString();

                }
                catch (Exception exception)
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ComboBoxMicrophones.SelectedIndexChanged", exception, Logger.LogLevel.Error);
                }
            };


            this.ComboBoxSpeakers.SelectedIndexChanged += (s, e) =>
            {
                try
                {
                    phoneController.setAudioDeviceId(ComboBoxMicrophones.SelectedIndex, ComboBoxSpeakers.SelectedIndex);
                    selectedSpeaker = ComboBoxSpeakers.SelectedItem.ToString();
                }
                catch (Exception exception)
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ComboBoxSpeakers.SelectedIndexChanged", exception, Logger.LogLevel.Error);
                }
            };


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
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("PlayRingTone> Cannot Find File {0}", filePath), Logger.LogLevel.Error);
                    if (ringtone.Equals("1") || ringtone.Equals("true"))
                    {
                        filePath = string.Format(@"{0}\{1}", Application.StartupPath, "Ringtone.wav");
                        if (File.Exists(filePath))
                        {
                            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "PlayRingTone> Play with default ringin tone", Logger.LogLevel.Info);
                            playRingtone = true;
                            mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Ring tone On", ToolTipIcon.Info);
                            return true;
                        }
                    }
                }

                playRingtone = false;
                mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Ring tone Off-No Audio File", ToolTipIcon.Info);
                return false;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "EnableRingtone", exception, Logger.LogLevel.Error);
                return false;
            }
        }

        private void InitializePhone(bool isReInit)
        {
            try
            {
                string userName = _agent.SipSetting["userName"].ToString();
                string password = _agent.SipSetting["password"].ToString();
                string displayName = _agent.SipSetting["displayName"].ToString();
                string authName = _agent.SipSetting["authName"].ToString();
                string localPort = _agent.SipSetting["localPort"].ToString();
                string sipServerPort = _agent.SipSetting["serverPort"].ToString();
                string sipServer = _agent.SipSetting["hostName"].ToString();
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}", userName, authName, password, localPort), Logger.LogLevel.Info);

                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}.............................step 1 : pass.", userName, authName, password, localPort), Logger.LogLevel.Info);
                int errorCode = 0;

                phoneController = new PortSIPLib(0, 0, this);
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
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}.............................Phone Initialization failed. errorCode : {4}", userName, authName, password, localPort, errorCode), Logger.LogLevel.Info);
                    InitializeError("Phone Initialization failed.", 408);
                    return;
                }

                InitAdioWizItems();
                loadDevices();
                phoneController.setAudioDeviceId(0, 0);
                phoneController.setAudioCodecParameter(AUDIOCODEC_TYPE.AUDIOCODEC_AMRWB, "mode-set=0; octet-align=0; robust-sorting=0");
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}.............................step 2 : pass.", userName, authName, password, localPort), Logger.LogLevel.Info);

                var outboundServer = "";
                var outboundServerPort = 0;
                var userDomain = "";

                var rt_userInfo = phoneController.setUser(userName, displayName, authName, password, _agent.IP, Convert.ToInt16(localPort), userDomain, sipServer, Convert.ToInt16(sipServerPort), "", 5060, outboundServer, outboundServerPort);

                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}.............................step 4 : Pass.", userName, authName, password, localPort), Logger.LogLevel.Info);
                if (rt_userInfo != 0)
                {
                    if (!isReInit)
                    {
                        phoneController.unInitialize();
                        phoneController.releaseCallbackHandlers();
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1,
                            string.Format(
                                "userName : {0}, authName : {1}, password : {2}, localPort : {3}.............................SetUserInfo Failed. errorCode : {4}",
                                userName, authName, password, localPort, errorCode), Logger.LogLevel.Info);
                        InitializeError("Fail to Set User Information's.", rt_userInfo);
                        return;
                    }
                }

                phoneController.setSrtpPolicy(SRTP_POLICY.SRTP_POLICY_NONE);
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}..............................step 5 : Pass.", userName, authName, password, localPort), Logger.LogLevel.Info);
                string licenseKey = LicenseKeyHandler.GetLicenseKey("DuoS123");
                rt = phoneController.setLicenseKey(licenseKey);

                if (rt == PortSIP_Errors.ECoreTrialVersionLicenseKey)
                {
                    MessageBox.Show("This sample was built base on evaluation key, which allows only three minutes conversation. The conversation will be cut off automatically after three minutes, then you can't hearing anything. Feel free contact us at: waruna@duosoftware.com to purchase the official version.");
                    this.Text = this.Text + " [built base on evaluation key]";
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "This sample was built base on evaluation key, which allows only three minutes conversation. The conversation will be cut off automatically after three minutes, then you can't hearing anything. Feel free contact us at: waruna@duosoftware.com to purchase the official version.", Logger.LogLevel.Info);
                }
                else if (rt == PortSIP_Errors.ECoreWrongLicenseKey)
                {
                    MessageBox.Show("The wrong license key was detected, please check with waruna@duosoftware.com or support@duosoftware.com");
                    this.Text = this.Text + " [wrong license key was detected]";
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "The wrong license key was detected, please check with waruna@duosoftware.com or support@duosoftware.com", Logger.LogLevel.Info);
                }

                var rt_register = phoneController.registerServer(120, 3);

                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}..............................step 6 : Pass.", userName, authName, password, localPort), Logger.LogLevel.Info);
                if (rt_register != 0)
                {
                    phoneController.unInitialize();
                    phoneController.releaseCallbackHandlers();
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}..............................Registration Failed. errorCode : {4}", userName, authName, password, localPort, errorCode), Logger.LogLevel.Info);
                    InitializeError("Fail to Register With SIP Server.", rt_register);
                    return;
                }

                phoneController.addSupportedMimeType("INFO", "text", "plain");
                initAutioCodecs();

                //phoneController.setSpeakerVolume(26214);//40% volume
                //phoneController.setMicVolume(52428);//80%
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}..............................step 7 : Pass.", userName, authName, password, localPort), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InitializePhone", exception, Logger.LogLevel.Error);
            }
        }

        private void UninitializePhone()
        {
            try
            {
                try
                {
                    phoneController.hangUp(_agent.PortsipSessionId);
                    phoneController.rejectCall(_agent.PortsipSessionId, 486);
                    phoneController.unRegisterServer();
                    phoneController.unInitialize();
                    phoneController.releaseCallbackHandlers();
                }
                catch (Exception exception)
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "UninitializePhone", exception, Logger.LogLevel.Error);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "UninitializePhone", exception, Logger.LogLevel.Error);
            }
        }

        private void initAutioCodecs()
        {
            phoneController.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_PCMA);
            phoneController.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_PCMU);
            phoneController.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_G729);

            phoneController.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_DTMF); // For RTP event - DTMF (RFC2833)
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
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "StopACWTime", exception, Logger.LogLevel.Error);
            }
        }

        private void loadDevices()
        {
            try
            {
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

                if (ComboBoxSpeakers.Items.Count > 0)
                    ComboBoxSpeakers.SelectedIndex = selectedSpeaker.Equals(string.Empty) ? 0 : ComboBoxSpeakers.FindString(selectedSpeaker);

                num = phoneController.getNumOfRecordingDevices();
                for (int i = 0; i < num; ++i)
                {
                    var deviceName = new StringBuilder { Length = 256 };

                    if (phoneController.getRecordingDeviceName(i, deviceName, 256) == 0)
                    {
                        ComboBoxMicrophones.Items.Add(deviceName.ToString());
                    }
                }

                if (ComboBoxMicrophones.Items.Count > 0)
                    ComboBoxMicrophones.SelectedIndex = selectedMic.Equals(string.Empty) ? 0 : ComboBoxMicrophones.FindString(selectedMic);

                int volume = phoneController.getSpeakerVolume();

                volume = phoneController.getMicVolume();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "loadDevices", exception, Logger.LogLevel.Error);
            }
        }

        #endregion

        #region keypad events

        private void buttonAnswer_Click(object sender, EventArgs e)
        {
            MakeCall(textBoxNumber.Text);
        }

        private void SendMsgToTappi(string no)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Send no : {0} To Tappi", no), Logger.LogLevel.Info);
                IXDBroadcast broadcast = XDBroadcast.CreateBroadcast(XDTransportMode.IOStream, false);
                broadcast.SendToChannel("CallerID", no);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "SendMsgToTappi", exception, Logger.LogLevel.Error);
            }
        }

        private void TransferCall()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault,
                   string.Format("transferCall_Click-> Session Id : {0} , Status : {1}", call.CallSessionId, call.CallCurrentState),
                   Logger.LogLevel.Info);
                if (call.CallCurrentState.GetType() == typeof(CallHoldState))
                {
                    if (!String.IsNullOrEmpty(textBoxNumber.Text))
                    {
                        var res = phoneController.sendInfo(call.portSipSessionId, "text", "plain", "transfer:" + textBoxNumber.Text);
                        if (res == 0)
                            call.CallCurrentState.OnTransferReq(ref call, CallActions.Call_Transfer_Requested);
                        txtStatus.Text = Environment.NewLine + (res != 0 ? "Transfer Failed." : "Transferring Call...");
                        if (!source.Contains(textBoxNumber.Text))
                            source.Add(textBoxNumber.Text);


                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void MakeCall(string no)
        {
            try
            {

                if (_agent.AgentCurrentState.GetType() == typeof(AgentIdle))
                {
                    if (!String.IsNullOrEmpty(no))
                    {

                        InAgentBusy(CallDirection.Outgoing);
                        _agent.AgentCurrentState.OnMakeCall(ref _agent);
                        call = new Call(no, this) { portSipSessionId = phoneController.call(no, true, false) };
                        if (call.portSipSessionId < 0)
                        {
                            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "MakeCall-Fail", Logger.LogLevel.Error);
                            mynotifyicon.ShowBalloonTip(5, "Duo Soft Phone", "Fail to Make Call", ToolTipIcon.Error);
                            _agent.AgentCurrentState.OnFailMakeCall(ref _agent);
                            call.CallCurrentState.OnTimeout(ref call);
                            return;
                        }
                        _agent.PortsipSessionId = call.portSipSessionId;
                        call.SetDialInfo(call.portSipSessionId, Guid.NewGuid());
                        AddOutgoingCallToCallLogs(no);

                    }
                    else
                    {
                        mynotifyicon.ShowBalloonTip(5, "Duo Soft Phone", "Invalid Phone Number.", ToolTipIcon.Error);
                    }
                }
                else if (call.CallCurrentState.GetType() == typeof(CallRingingState) || call.CallCurrentState.GetType() == typeof(CallIncommingState))
                {
                    phoneController.answerCall(call.portSipSessionId, false);
                }
                else
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, string.Format("MakeCall-Fail. AgentCurrentState: {0}, CallCurrentState: {1}", _agent.AgentCurrentState, call.CallCurrentState), Logger.LogLevel.Error);
                    mynotifyicon.ShowBalloonTip(5, "Duo Soft Phone", "Fail to Make Call", ToolTipIcon.Error);
                }

                this.Invoke(new MethodInvoker(delegate
                {
                    if (!source.Contains(no))
                        source.Add(no);
                    source.Remove("");
                }));


            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "MakeCall", exception, Logger.LogLevel.Error);
            }
        }



        private void buttonReject_Click(object sender, EventArgs e)
        {
            EndCall();
            DisableAlert();
            isCallAnswerd = false;
        }

        private void EndCall()
        {
            try
            {
                StopRingTone();
                var status = "Call Ended";
                if (call.CallCurrentState.GetType() == typeof(CallRingingState) || call.CallCurrentState.GetType() == typeof(CallTryingState))
                {
                    if (_agent.CallDirection == CallDirection.Outgoing)
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

                call.CallCurrentState.OnDisconnected(ref call);
                _agent.AgentCurrentState.OnEndCall(ref _agent,true);
                this.Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    txtStatus.Text = Environment.NewLine + status;
                })));

                AddCallDurations();

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void buttonConference_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Conference_Click-> Session Id : {0} , Status : {1}", _agent.CallSessionId, call.CallCurrentState), Logger.LogLevel.Info);
                if (call.CallCurrentState.GetType() == typeof(CallAgentClintConnectedState))
                {
                    var res = phoneController.sendInfo(_agent.PortsipSessionId, "text", "plain", "conference");
                    if (res == 0)
                        call.CallCurrentState.OnCallConference(ref call);
                    txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    txtStatus.Text = (res != 0 ? "Conference Failed." : "Conference Call...");
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void buttonEtl_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Etl_Click-> Session Id : {0} , Status : {1}", _agent.CallSessionId, call.CallCurrentState), Logger.LogLevel.Info);
                if (call.CallCurrentState.GetType() == typeof(CallConferenceState) || call.CallCurrentState.GetType() == typeof(CallAgentClintConnectedState) || call.CallCurrentState.GetType() == typeof(CallAgentSupConnectedState))
                {
                    var res = phoneController.sendInfo(_agent.PortsipSessionId, "text", "plain", "etl");
                    if (res == 0)
                        call.CallCurrentState.OnEndLinkLine(ref call, CallActions.ETL_Requested);
                    txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    txtStatus.Text = (res != 0 ? "ETL Failed." : "ETL Call...");
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void buttonswapCall_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("swapCall_Click-> Session Id : {0} , Status : {1}", call.CallSessionId, call.CallCurrentState), Logger.LogLevel.Info);
                if (call.CallCurrentState.GetType() == typeof(CallAgentClintConnectedState) || call.CallCurrentState.GetType() == typeof(CallAgentSupConnectedState))
                {
                    var res = phoneController.sendInfo(call.portSipSessionId, "text", "plain", "swap");
                    if (res == 0)
                        call.CallCurrentState.OnSwapReq(ref call, CallActions.Call_Swap_Requested);
                    txtStatus.Text = Environment.NewLine + (res != 0 ? "Swap Failed." : "Swap Call...");
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void buttontransferCall_Click(object sender, EventArgs e)
        {
            TransferCall();
        }

        private void buttontransferIvr_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("transferIvr_Click-> Session Id : {0} , Status : {1}", _agent.CallSessionId, call.CallCurrentState), Logger.LogLevel.Info);
                //if (this.State != DialerState.NotOnCall)
                //{
                //    if (this.State == DialerState.OnHold)
                //    {
                //        if (!String.IsNullOrEmpty(textBoxNumber.Text))
                //        {
                //            txtStatus.Text =  "Transferring Call...";
                //            this.State = DialerState.transferCall;
                //            var res = phoneController.sendInfo(SessionId, "text", "plain", "ivrtransfer:" + textBoxNumber.Text);
                //            txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                //            txtStatus.Text =  (res != 0 ? "IVR Transfer Failed." : "Transferring Call...");
                //        }
                //    }
                //}
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_1_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "1";
                if (call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDTMF(1);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_2_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "2";
                if (call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDTMF(2);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_3_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "3";
                if (call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDTMF(3);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_4_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "4";
                if (call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDTMF(4);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_5_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "5";
                if (call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDTMF(5);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_6_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "6";
                if (call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDTMF(6);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_7_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "7";
                if (call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDTMF(7);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_8_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "8";
                if (call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDTMF(8);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_9_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "9";
                if (call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDTMF(9);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_star_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "*";
                if (call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDTMF(10);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_0_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "0";
                if (call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDTMF(0);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_hash_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "#";
                if (call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDTMF(11);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void SendDTMF(int digit)
        {
            try
            {
                if (call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    phoneController.sendDtmf(_agent.PortsipSessionId, DTMF_METHOD.DTMF_RFC2833, Convert.ToInt16(digit),
                        160, true);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "SendDTMF", exception, Logger.LogLevel.Error);
            }
        }

        private void buttonHold_Click(object sender, EventArgs e)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Hold_Click-> Session Id : {0} , Status : {1}", call.CallSessionId, call.CallCurrentState), Logger.LogLevel.Info);
            HoldUnholdCall();
        }

        private void HoldUnholdCall()
        {
            try
            {
                var status = "Hold Call";
                int res = -1;
                if (call.CallCurrentState.GetType() == typeof(CallConnectedState))
                {
                    if (_agent.CallHandler._sipServiceCall.IsPBX)
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
                    if (_agent.CallHandler._sipServiceCall.IsPBX)
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

                this.txtStatus.Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    txtStatus.Text = (string.Format("{0} {1}", status, (res != 0 ? "Failed" : "")));
                })));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "HoldUnholdCall", exception, Logger.LogLevel.Error);
            }
        }

        private void buttonBackspace_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentNumber != null)
                {
                    if (CurrentNumber.Length != 0)
                    {
                        var index = textBoxNumber.SelectionStart - 1;
                        if (index >= 0)
                        {
                            CurrentNumber = CurrentNumber.Remove(index, 1);
                            textBoxNumber.SelectionStart = index;
                            textBoxNumber.Focus();
                        }
                        // CurrentNumber.Substring(0, CurrentNumber.Length - 1);
                    }

                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        #endregion keypad events

        #region FormEvents

        private void btnAgentListClose_Click(object sender, System.EventArgs e)
        {
            panelAgentList.Visible = false;
        }

        private void btnLoadAgentList_Click(object sender, System.EventArgs e)
        {
            try
            {
                resMonitoringHandler.GetOnlineIdleAgentList(_agent.Auth.SecurityToken, _agent.Auth.guUserId.ToString());
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "btnLoadAgentList_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void agentListToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                panelAgentList.Visible = !panelAgentList.Visible;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "agentListToolStripMenuItem_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void GridAgentList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                textBoxNumber.Text = GridAgentList.Rows[e.RowIndex].Cells["Extention"].Value.ToString();
                panelAgentList.Visible = false;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "GridAgentList_CellDoubleClick", exception, Logger.LogLevel.Error);
            }
        }

        private void dNDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DND = !DND;
            //phoneController.SetDnd(DND);
            //if (DND)
            //{
            //    textBoxNumber.BackColor = Color.DarkRed;
            //    txtStatus.BackColor = Color.DarkRed;
            //    textBoxNumber.Text = "DND Enable";
            //    txtStatus.Text = Environment.NewLine + "DND Enable";
            //    PhoneStatusMsg.Text = "DND";
            //}
            //else
            //{
            //    textBoxNumber.BackColor = Color.Black;
            //    txtStatus.BackColor = Color.Black;
            //    textBoxNumber.Text = "";
            //    txtStatus.Text = Environment.NewLine + "";
            //    PhoneStatusMsg.Text = "";
            //}
        }

        private void BreakRequestmenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //IsBreakRequest = true;
                //ardsHandler.SendStatusChangeRequestBreak(Auth, CallSessionId);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "BreakToolStripMenuItem_Click", exception, Logger.LogLevel.Error);
            }
        }



        public FormDialPad(UserAuth auth, string sessionId, Dictionary<string, object> sip, string ip)
        {
            try
            {

                InitializeComponent();

                buttonHold.Enabled = false;
                buttonHold.Text = "Hold";
                txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                txtStatus.Text = "";

                buttonAnswer.Enabled = false;
                buttonReject.Enabled = false;
                buttontransferIvr.Enabled = false;
                buttontransferCall.Enabled = false;
                buttonEtl.Enabled = false;
                buttonswapCall.Enabled = false;
                buttonConference.Enabled = false;
                   

                var settingObject = System.Configuration.ConfigurationSettings.AppSettings;
                filePath = settingObject["RingToneFilePath"];
                var ringtone = settingObject["PlayRingtone"].ToLower();
                if (!File.Exists(filePath))
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("PlayRingTone> Cannot Find File {0}", filePath), Logger.LogLevel.Error);
                    if (ringtone.Equals("1") || ringtone.Equals("true"))
                    {
                        filePath = string.Format(@"{0}\{1}", Application.StartupPath, "Ringtone.wav");
                        if (File.Exists(filePath))
                            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "PlayRingTone> Play with default ring tone", Logger.LogLevel.Info);
                    }
                }
                playRingtone = File.Exists(filePath) && (ringtone.Equals("1") || ringtone.Equals("true"));
                playRingInToneMenually = File.Exists(_ringInfilePath);
                if (!playRingInToneMenually)
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("PlayRingINTone> Cannot Find File {0}", _ringInfilePath), Logger.LogLevel.Error);
                    _ringInfilePath = string.Format(@"{0}\{1}", Application.StartupPath, "RingIntone.wav");
                    if (File.Exists(_ringInfilePath))
                    {
                        playRingInToneMenually = true;
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "PlayRingINTone> Play with default ringIn tone", Logger.LogLevel.Info);
                    }
                }
                
                _agent = new Agent(auth, sip, ip, sessionId, this) { AgentReqMode = AgentMode.Outbound };

                var appSetting = System.Configuration.ConfigurationSettings.AppSettings;
                InternetSetCookie(appSetting["DuoUrl"], "SecurityTokenCookie", auth.SecurityToken);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "FormDialPad", exception, Logger.LogLevel.Error);
                MessageBox.Show("Critical Error. Please Contact your System Administrator.", "Duo Dialer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        private void FormDialPad_Load(object sender, EventArgs e)
        {
            try
            {

                var appSetting = System.Configuration.ConfigurationSettings.AppSettings;

                // Create and initialize the text box. //64, 64, 64
                textBoxNumber = new TextBox
                {
                    BackColor = Color.Black, // System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))),((int)(((byte)(64))))),
                    BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D,
                    Font =
                        new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold,
                            System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    ForeColor = System.Drawing.Color.White,
                    //Location = new System.Drawing.Point(2, 26),
                    Name = "textBoxNumber",
                    //Size = new System.Drawing.Size(186, 30),
                    TabIndex = 1,
                    AutoCompleteCustomSource = source,
                    AutoCompleteMode = AutoCompleteMode.Suggest,
                    AutoCompleteSource = AutoCompleteSource.CustomSource,
                    Visible = true,
                    TextAlign = HorizontalAlignment.Center,
                    Dock = DockStyle.Fill,
                };

                panelNo.Controls.Add(textBoxNumber);
                textBoxNumber.AutoCompleteCustomSource = source;
                textBoxNumber.AutoCompleteMode = AutoCompleteMode.Suggest;
                textBoxNumber.AutoCompleteSource = AutoCompleteSource.CustomSource;

                textBoxNumber.TextChanged += (s, e1) =>
                {
                    CurrentNumber = textBoxNumber.Text;
                };
                textBoxNumber.Focus();

                textBoxNumber.KeyDown += (k, d) =>
                {
                    if (d.KeyCode != Keys.Enter) return;
                    if (_agent.AgentCurrentState.GetType() == typeof (AgentIdle))
                        MakeCall(textBoxNumber.Text);
                };
                //              
                onToolStripMenuItem.Checked = playRingtone;
                offToolStripMenuItem.Checked = !playRingtone;


                _wavPlayer = new SoundPlayer
                {
                    SoundLocation = filePath,
                    // @"C:\Users\Public\Music\Sample Music\ALBSlide.wav"
                };
                _wavPlayer.LoadCompleted += wavPlayer_LoadCompleted;
                _wavPlayer.LoadAsync();

                _wavPlayerRingIn = new SoundPlayer
                {
                    SoundLocation = _ringInfilePath,
                    // @"C:\Users\Public\Music\Sample Music\ALBSlide.wav"
                };
                _wavPlayerRingIn.LoadCompleted += wavPlayer_LoadCompleted;
                _wavPlayerRingIn.LoadAsync();


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
                                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault,
                                    string.Format("Recive no : {0} frm Tappi", cli), Logger.LogLevel.Info);
                                txtStatus.Text = "Tappi System try to Dial Call.";
                                mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Tappi System try to Dial Call.",
                                    ToolTipIcon.Info);
                                if (this._agent.AgentCurrentState.GetType() == typeof(AgentIdle))
                                {
                                    textBoxNumber.Text = cli;
                                    MakeCall(cli);
                                }
                                else
                                {
                                    mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone",
                                        "Fail to Make calls. Agent Invalid State.", ToolTipIcon.Info);
                                }
                            }));
                        }
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "listner.MessageReceived", exception, Logger.LogLevel.Error);
                    }
                };

                callHandler = new CallHandler(_agent.Auth.SecurityToken, _agent.Auth.TenantID, _agent.Auth.CompanyID);
                //System.Configuration.ConfigurationSettings.AppSettings["AutoAnswer"]
                AutoAnswer.Checked = callHandler.AutoAnswerByDefault();// System.Configuration.ConfigurationSettings.AppSettings["AutoAnswer"].Equals("1");
                AutoAnswer.Enabled = (callHandler.AutoAnswerByDefault() && callHandler.AutoAnswerCanAgentEnable());
                playRingInToneMenually = callHandler._sipServiceCall.IsPlayRingtone;
                IsNotAllowToReject = callHandler._sipServiceCall.IsAllowToReject;
                rejectCallToolStripMenuItem.Enabled = !IsNotAllowToReject;
                //new Thread(InitiateTimer).Start();
                OFFLINE.Visible = false;
                this.Text = string.Format("{0} : {1}", this.Text, _agent.Auth.UserName);

                txtStatus.ForeColor = Color.DarkMagenta;
                txtStatus.Text = "Initializing...";


                var settingObject = System.Configuration.ConfigurationSettings.AppSettings;
                ShowCallAlert = settingObject["ShowCallAlert"].Equals("1");
                reginTime = Convert.ToInt16(settingObject["RingTime"]);

                #region Socket Server

                if (settingObject["SocketlistnerEnable"].ToLower().Equals("true") || settingObject["SocketlistnerEnable"].Equals("1"))
                {
                    var Socketlistner = new AsynchronousSocketListener(_agent.IP,
                        Convert.ToInt16(settingObject["SocketlistnerPort"]), _agent.Auth.SecurityToken, _agent.Auth.TenantID,
                        _agent.Auth.CompanyID);

                    Socketlistner.OnSocketMessageRecive += (callFunction, no) =>
                    {
                        try
                        {
                            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("External Application send Commands. callFunction : {0}, Phone No : {1}", callFunction, no), Logger.LogLevel.Info);
                            switch (callFunction)
                            {
                                case CallFunctions.MakeCall:
                                    MakeCall(no);
                                    break;

                                case CallFunctions.EndCall:
                                    EndCall();
                                    break;

                                case CallFunctions.HoldCall:
                                    HoldUnholdCall();
                                    break;

                                default:
                                    throw new ArgumentOutOfRangeException("callFunction");
                            }
                        }
                        catch (Exception exception)
                        {
                            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault,
                                "FormDialPad_Load-OnSocketMessageRecive", exception, Logger.LogLevel.Error);
                        }
                    };
                }
                else
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "FormDialPad_Load-Socket-Disable", Logger.LogLevel.Info);
                }

                #endregion Socket Server

                #region WebSocket Server

                if (settingObject["WebSocketlistnerEnable"].ToLower().Equals("true") || settingObject["WebSocketlistnerEnable"].Equals("1"))
                {
                    var webSocketlistner = new WebSocketServiceHost(_agent.IP, Convert.ToInt16(settingObject["WebSocketlistnerPort"]), _agent.Auth.SecurityToken, _agent.Auth.TenantID,
                        _agent.Auth.CompanyID);

                    webSocketlistner.OnSocketMessageRecive += (callFunction, no) =>
                    {
                        try
                        {
                            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("[webSocketlistner]External Application send Commands. callFunction : {0}, Phone No : {1}",
                                    callFunction, no), Logger.LogLevel.Info);
                            switch (callFunction)
                            {
                                case CallFunctions.MakeCall:
                                    MakeCall(no);
                                    break;

                                case CallFunctions.EndCall:
                                    EndCall();
                                    break;

                                case CallFunctions.HoldCall:
                                    HoldUnholdCall();
                                    break;

                                case CallFunctions.TransferCall:
                                    {
                                        if (!string.IsNullOrEmpty(no))
                                        {
                                            if (call.CallCurrentState.GetType() == typeof(CallHoldState))
                                            {
                                                HoldUnholdCall();
                                            }
                                            textBoxNumber.Invoke(new MethodInvoker(delegate { textBoxNumber.Text = no; }));
                                            TransferCall();
                                        }
                                    }
                                    break;

                                default:
                                    throw new ArgumentOutOfRangeException("callFunction");
                            }
                        }
                        catch (Exception exception)
                        {
                            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault,
                                "[webSocketlistner]FormDialPad_Load-OnSocketMessageRecive", exception, Logger.LogLevel.Error);
                        }
                    };
                }
                else
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "FormDialPad_Load-[webSocketlistner]-Disable", Logger.LogLevel.Info);
                }

                #endregion WebSocket Server

                callLogs = new Dictionary<Guid, CallLog>();

                #region ACW Timer

                acwCutdownTimer = new Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);

                acwTime = (int)TimeSpan.FromSeconds(Convert.ToDouble(ProfileManagementHandler.GetACWTime(_agent.Auth.SecurityToken))).TotalSeconds;
                acwCotdown = acwTime;

                acwCutdownTimer.Elapsed += (s, e1) =>
                {
                    try
                    {
                        if (acwCotdown <= 0)
                        {
                            this.Invoke(new MethodInvoker(() => {  txtStatus.Text = string.Empty;
                                                                    btnFreez.Enabled = false;
                            }));
                            if (acwCotdown < -1)
                            {
                                acwCutdownTimer.Stop();
                                acwCutdownTimer.Enabled = false;
                                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("End ACW time with default time.{0}", _agent.CallSessionId), Logger.LogLevel.Info);
                                call.CallCurrentState.OnEndCallSession(ref call);
                                _agent.AgentCurrentState.OnEndACW(ref _agent, _agent.CallSessionId);
                                return;
                            }
                            acwCotdown--;
                            return;
                        }
                        txtStatus.Invoke(new MethodInvoker(() => { txtStatus.Text = string.Format("ACW : {0}", acwCotdown); }));

                        acwCotdown--;
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "acwCutdownTimer.Elapsed", exception, Logger.LogLevel.Error);
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "acwCutdownTimer.Elapsed", exception, Logger.LogLevel.Error);
                    }
                };

                #endregion

                FreezeDurations = new System.Timers.Timer(TimeSpan.FromSeconds(1).TotalSeconds);
                FreezeDurations.Elapsed += (s, e1) =>
                {
                    var ts = e1.SignalTime.Subtract(freezeStarTime);
                    var elapsedTime = ts.Hours > 0 ? String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds) : String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);

                    this.Invoke(new MethodInvoker(delegate
                    {
                        txtStatus.Text = elapsedTime;
                    }));

                };

                CallDurations = new System.Timers.Timer(TimeSpan.FromSeconds(1).TotalSeconds);
                CallDurations.Elapsed += (s, e1) =>
                {
                    var ts = e1.SignalTime.Subtract(callStarTime);
                    var elapsedTime = ts.Hours > 0 ? String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds) : String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);

                    this.Invoke(new MethodInvoker(delegate
                    {
                        txtStatus.Text = elapsedTime;
                    }));

                };


                //PanelbtnLog.Location = new Point(4, 286);
                //PanelPhoneFunc.Location = new Point(4, 228);
                //PanelDialPad.Location = new Point(4, 3);
                //gbBreakMode.Location = new Point(2, 62);
                OFFLINE.Dock = DockStyle.Bottom; //.Location = new Point(2, 1);
                //PanelPhoneNoBox.Location = new Point(2, 2);
                //PanelBtnNo.Location = new Point(2, 55);

                InitializePhone(false);

                #region resourceCheckerHandler

                resourceCheckerHandler = new ResourceCheckerHandler(ref _agent, ref call);
                resourceCheckerHandler.OnResourceStatusMisMatch += () =>
                {
                    try
                    {
                        _agent.AgentCurrentState.OnError(ref _agent, "Status Mismatch", -9999, "Unable to Communicate With Proxy Servers or Lost some of Data Packets. Please Contact Your System Administrator.");
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "resourceMonitoringHandler.OnResourceStatusMisMatch", exception, Logger.LogLevel.Error);
                    }
                };

                #endregion

                var imgPath = string.Format(@"{0}\{1}", Application.StartupPath, "Log207X55pix.png");
                if (File.Exists(imgPath))
                {
                    LogoDisplay.ImageLocation = imgPath;
                    LogoDisplay.AutoSize = false;
                    
                }

                #region AgentList

                resMonitoringHandler = new ResourceMonitoringHandler(_agent.Auth.SecurityToken, _agent.Auth.guUserId.ToString());
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
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "resHandler.OnGetOnlineIdleAgentListCompleted", exception, Logger.LogLevel.Error);
                    }
                };

                #endregion

                #region DeviceMonitor

                try
                {

                    var device = new DuoDeviceMonitor.DeviceMonitor();
                    device.OnSoundDeviceAddEvent += (d) =>
                    {
                        try
                        {
                            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault,
                               string.Format("OnSoundDeviceAddEvent - The audio device on the computer was reconnected. AgentCurrentState : {0}, CallCurrentState : {1}", _agent.AgentCurrentState, call.CallCurrentState),
                                Logger.LogLevel.Error);
                            //Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("The audio device on the computer [{0}] used by [{1}], was either removed /reconnected during an ongoing call, therefore the   agent console will be disabled  till remedial action is taken. Agent Session ID [{2}], Call Session ID : {3},  Agent Status : {4}, Call Status : {5}", agent.AgentProfile.IpAddress, agent.AgentProfile.UserName, agent.AgentSessionId, call.CallSessionId, agent.AgentCurrentState.GetType().Name, call.CurrentState.GetType().Name), Logger.LogLevel.Error);
                            HandleDeviceEvent();
                        }
                        catch (Exception exception)
                        {
                            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "OnSoundDeviceAddEvent", exception,
                                Logger.LogLevel.Error);
                        }
                    };
                    device.OnSoundDeviceRemoveEvent += (d) =>
                    {
                        try
                        {
                            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault,
                                string.Format("OnSoundDeviceRemoveEvent - The audio device on the computer was removed . AgentCurrentState : {0}, CallCurrentState : {1}", _agent.AgentCurrentState, call.CallCurrentState),
                                Logger.LogLevel.Error);
                            HandleDeviceEvent();
                        }
                        catch (Exception exception)
                        {
                            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "OnSoundDeviceRemoveEvent", exception, Logger.LogLevel.Error);
                        }
                    };
                    device.EnableSoundDeviceArrivedWatcher();

                }
                catch (Exception exception)
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "FormDialPad_Load-DeviceMonitor", exception, Logger.LogLevel.Error);
                }

                #endregion

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "FormDialPad_Load", exception,
                    Logger.LogLevel.Error);
                MessageBox.Show("Critical Error. Please Contact your System Administrator.", "Duo Dialer",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void FormDialPad_Resize(object sender, EventArgs e)
        {
            try
            {
                setPhonePanelLocation();
                if (FormWindowState.Minimized == this.WindowState)
                {
                    // this.Hide();
                    mynotifyicon.Visible = true;
                    mynotifyicon.ShowBalloonTip(3000);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void FormDialPad_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are you sure you want to exit the application?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    UninitializePhone();
                    _agent.AgentCurrentState.OnLogOff(ref _agent);
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }


        private void mynotifyicon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                Show();
                WindowState = FormWindowState.Normal;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
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
                var frmAudio = new AudioWiz(ComboBoxMicrophones, ComboBoxSpeakers, phoneController.getMicVolume(), phoneController.getSpeakerVolume(), ComboBoxMicrophones.FindString(selectedMic), ComboBoxSpeakers.FindString(selectedSpeaker));

                frmAudio.Closing += (s, e1) =>
                {
                    try
                    {
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "frmAudio.OnAudioPlayTest", exception, Logger.LogLevel.Error);
                    }
                };

                frmAudio.OnAudioPlayTest += (val) =>
                {
                    try
                    {
                        phoneController.audioPlayLoopbackTest(val);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "frmAudio.OnAudioPlayTest", exception, Logger.LogLevel.Error);
                    }
                };

                frmAudio.OnMicVolumeChanged += (val) =>
                {
                    try
                    {
                        phoneController.setMicVolume(val);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "frmAudio.OnMicVolumeChanged", exception, Logger.LogLevel.Error);
                    }
                };

                frmAudio.OnSpeakerVolumeChanged += (val) =>
                {
                    try
                    {
                        phoneController.setSpeakerVolume(val);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "frmAudio.OnSpeakerVolumeChanged", exception, Logger.LogLevel.Error);
                    }
                };

                frmAudio.OnMicMute += (val) =>
                {
                    try
                    {
                        phoneController.muteMicrophone(val);
                        picMic.Visible = val;

                        if (!val)
                            phoneController.setMicVolume(phoneController.getMicVolume());
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "frmAudio.OnMicMute", exception, Logger.LogLevel.Error);
                    }
                };

                frmAudio.OnSpeakerMute += (val) =>
                {
                    try
                    {
                        phoneController.muteSpeaker(val);
                        picSpek.Visible = val;
                        if (!val)
                            phoneController.setSpeakerVolume(phoneController.getMicVolume());
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "frmAudio.OnSpeakerMute", exception, Logger.LogLevel.Error);
                    }
                };

                frmAudio.ShowDialog(this);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "volumeToolStripMenuItem_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void officialBreakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _agent.AgentCurrentState.OnRequestAgentBreak(ref _agent, "Official Break");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "officialBreakToolStripMenuItem_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void mealBreakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _agent.AgentCurrentState.OnRequestAgentBreak(ref _agent, "Meal Break");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "mealBreakToolStripMenuItem_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void aUXBreakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _agent.AgentCurrentState.OnRequestAgentBreak(ref _agent, "AUX Break");

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "aUXBreakToolStripMenuItem_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void CancelRequestmenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _agent.AgentCurrentState.OnRequestAgentBreakCancel(ref _agent);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "CancelRequestmenuItem_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void EndBreakmenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                StoptDurations();
                this.Invoke(((MethodInvoker)(() =>
                {
                    gbBreakMode.SendToBack();
                    gbBreakMode.Visible = false;
                    breakRequestToolStripMenuItem.Enabled = true;
                    cancelRequestToolStripMenuItem.Enabled = false;
                    endBreakToolStripMenuItem.Enabled = false;
                    textBoxNumber.BackColor = Color.Black;
                    txtStatus.BackColor = Color.Black;
                    endBreakToolStripMenuItem.Enabled = false;
                    officialBreakToolStripMenuItem.Enabled = true;
                    mealBreakToolStripMenuItem.Enabled = true;
                    aUXBreakToolStripMenuItem.Enabled = true;
                    txtStatus.Text = "";
                })));
                _agent.AgentCurrentState.OnEndBreak(ref _agent);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "EndBreakmenuItem_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void outboundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _agent.AgentCurrentState.OnRequestAgentModeChange(ref _agent, AgentMode.Outbound);
                outboundToolStripMenuItem.Enabled = false;
                inboundToolStripMenuItem.Enabled = false;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "outboundToolStripMenuItem_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void inboundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _agent.AgentCurrentState.OnRequestAgentModeChange(ref _agent, AgentMode.Inbound);
                outboundToolStripMenuItem.Enabled = false;
                inboundToolStripMenuItem.Enabled = false;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "outboundToolStripMenuItem_Click", exception,
                    Logger.LogLevel.Error);
            }
        }
        private void btnFreez_Click(object sender, EventArgs e)
        {
            try
            {
                var msg = string.Empty;
                if (btnFreez.Text.Equals("Freeze"))
                {

                    FreezeACWTime();
                    txtStatus.Text = "Freeze";
                    btnFreez.Text = "End Freeze";
                    msg = "Freeze";
                    StartDurations();

                }
                else
                {
                    StoptDurations();
                    txtStatus.Text = "";
                    btnFreez.Text = "Freeze";
                    btnFreez.Enabled = false;
                    msg = "EndFreeze";
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("End ACW time After Freeze. {0}", _agent.CallSessionId), Logger.LogLevel.Info);
                    _agent.AgentCurrentState.OnEndACW(ref _agent, _agent.CallSessionId);

                }

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "btnFreez_Click", exception, Logger.LogLevel.Error);
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "btnFreez_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void wavPlayer_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "wavPlayer_LoadCompleted", Logger.LogLevel.Info);
                //if (playingRingIntone)
                //    ((SoundPlayer) sender).PlayLooping();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "wavPlayer_LoadCompleted", exception, Logger.LogLevel.Error);
            }
        }

        private void setPhonePanelLocation()
        {
            // PanelPhone.Location = new Point(this.Width - 249, this.Height - 418);
        }

        private void toolStripStatusDuoPhone_Click(object sender, EventArgs e)
        {
            setPhonePanelLocation();
            //PanelPhone.Visible = !PanelPhone.Visible;
            //ultraPopupControlPhone.Show();
        }

        private void toolStripStatusTools_Click(object sender, EventArgs e)
        {
            // ultraPopupControlTools.Show();
        }

        private void RingtoneOnMenuItem_Click(object sender, EventArgs e)
        {
            EnableRingtone();
            onToolStripMenuItem.Checked = true;
            offToolStripMenuItem.Checked = !onToolStripMenuItem.Checked;
        }

        private void RingtoneOffmenuItem_Click(object sender, EventArgs e)
        {
            DisableRingtone();
            offToolStripMenuItem.Checked = true;
            onToolStripMenuItem.Checked = !offToolStripMenuItem.Checked;
        }

        private void btnBreakMode_Click(object sender, EventArgs e)
        {
            EndBreakmenuItem_Click(sender, e);
        }

        private void menuItemAnswerCall_Click(object sender, EventArgs e)
        {
            MakeCall(textBoxNumber.Text);
        }

        private void menuItemRejectCall_Click(object sender, EventArgs e)
        {
            EndCall();
        }

        private void menuItemHoldCall_Click(object sender, EventArgs e)
        {
            HoldUnholdCall();
        }

        private void duoLink_Click(object sender, EventArgs e)
        {
            try
            {
                duoLink.LinkVisited = true;
                System.Diagnostics.Process.Start("http://www.duosoftware.com");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "duoLink_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void btnReregister_Click(object sender, EventArgs e)
        {
            Reregister();
        }

        private bool IsCallLogOpen = false;
        private frmCallLogs logs;
        private void btnCallLogs_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsCallLogOpen)
                    return;
                logs = new frmCallLogs(callLogs);
                logs.OnNumberSelect += (no) =>
                {
                    textBoxNumber.Text = no;
                };
                logs.FormClosed += (w, k) =>
                {
                    IsCallLogOpen = false;
                };
                IsCallLogOpen = true;
                logs.Show(this);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "btnCallLogs_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void AutoAnswer_Click(object sender, EventArgs e)
        {
            AutoAnswer.Checked = !AutoAnswer.Checked;

        }

        private void AutoAnswer_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                AutoAnswer.BackColor = AutoAnswer.Checked ? Color.Maroon : Color.Black;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "AutoAnswer_CheckedChanged", exception, Logger.LogLevel.Error);
            }
        }

        private void onToolStripMenuItem_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                onToolStripMenuItem.BackColor = onToolStripMenuItem.Checked ? Color.DarkGreen : Color.Black;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "onToolStripMenuItem_CheckedChanged", exception, Logger.LogLevel.Error);
            }
        }

        private void offToolStripMenuItem_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                offToolStripMenuItem.BackColor = offToolStripMenuItem.Checked ? Color.DarkRed : Color.Black;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "offToolStripMenuItem_CheckedChanged", exception, Logger.LogLevel.Error);
            }
        }

        private void OFFLINE_Click(object sender, EventArgs e)
        {
            try
            {
                UninitializePhone();
                InitializePhone(true);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "OFFLINE_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void openLogfiles_Click(object sender, EventArgs e)
        {
            try
            {
                OpenLogFils();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "openLogfiles_Click", exception,
                                    Logger.LogLevel.Error);
            }
        }

        private void lblAgentMode_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.ToolTipTitle = "Agent Mode";
            // toolTip1.Show(_agent.AgentMode.ToString(), lblAgentMode, 1500);
        }

        private void openLogDic_Click(object sender, EventArgs e)
        {
            try
            {
                OpenLogFilsDirectory();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "openLogDic_Click", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void InitializeError(string statusText, int statusCode)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, string.Format("phoneController_OnInitializeError : {0} , SipRegisterTryCount : {1}", statusText, SipRegisterTryCount), Logger.LogLevel.Error);
                _SIPLogined = false;
                isSipRegistrationOk = false;
                SipRegisterTryCount++;

                _agent.AgentCurrentState.OnError(ref _agent, statusText, statusCode, "Unable to Communicate With Servers. Please Contact Your System Administrator.");

                //this.Invoke(((MethodInvoker)(() =>
                //{
                //    btnReregister.Visible = true;
                //    txtStatus.ForeColor = Color.Red;
                //    txtStatus.Text = "Error on Initializing" + statusText;
                //    PhoneStatus.Image = Properties.Resources.offline;
                //})));

                //if (SipRegisterTryCount >= 3)
                //{
                //    this.Invoke(((MethodInvoker)(() =>
                //    {
                //        txtStatus.Text =
                //                       "Error on Initializing. exceed maximum retry count. please contact your system administrator.";
                //        mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", txtStatus.Text, ToolTipIcon.Error);
                //        OFFLINE.Visible = true;

                //    })));
                //    _agent.AgentCurrentState.OnError(ref _agent);
                //}
                //else
                //{
                //    this.Invoke(new MethodInvoker(delegate
                //    {
                //        txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                //        txtStatus.Text = "Initializing";
                //        mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Phone Reinitializing.", ToolTipIcon.Error);
                //        txtStatus.Text = statusText;
                //    }));

                //    UninitializePhone();
                //    InitializePhone(true);
                //}

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "phoneController_OnInitializeError", exception,
                    Logger.LogLevel.Error);
            }
        }

        #endregion

        #region SIPCallbackEvents



        public int onRegisterSuccess(int callbackIndex, int callbackObject, string statusText, int statusCode)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRegisterSuccess", Logger.LogLevel.Info);
                _SIPLogined = true;
                _agent.SipStatus = false;
                SipRegisterTryCount = 0;
                this.Invoke(((MethodInvoker)(() =>
                {
                    btnReregister.Visible = false;
                    txtStatus.ForeColor = Color.DarkGreen;
                    txtStatus.Text = "Phone Initialized with the IP" + statusText;
                    PhoneStatus.Image = Properties.Resources.online;
                    phoner8ClickMenu.Enabled = true;
                    btnReregister.Visible = false;
                }))); mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Phone Initialized.", ToolTipIcon.Info);
                call = new Call(string.Empty, this);
                _agent.AgentCurrentState.OnLogin(ref _agent);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRegisterSuccess", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onRegisterFailure(int callbackIndex, int callbackObject, string statusText, int statusCode)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, string.Format("onRegisterFailure. statusText: {0}, statusCode: {1}", statusText, statusCode), Logger.LogLevel.Info);
            _agent.SipStatus = false;
            _SIPLogined = false;
            InitializeError(statusText, statusCode);

            return 0;
        }

        public int onInviteRinging(int callbackIndex, int callbackObject, int sessionId, string statusText, int statusCode)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteRinging", Logger.LogLevel.Info);
                PlayRingInTone();
                call.CallCurrentState.OnRinging(ref call, callbackIndex, callbackObject, sessionId, statusText, statusCode);
                this.Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    txtStatus.Text = statusText;
                })));

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteRinging", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteIncoming(int callbackIndex, int callbackObject, int sessionId, string callerDisplayName, string caller,
            string calleeDisplayName, string callee, string audioCodecNames, string videoCodecNames, bool existsAudio,
            bool existsVideo)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, string.Format("onInviteIncoming. caller : {0} Agent State : {1}, Call State : {2}", caller, _agent.AgentCurrentState, call.CallCurrentState), Logger.LogLevel.Info);

                if (_agent.AgentCurrentState.GetType() != typeof(AgentIdle))
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, string.Format("Call receive in Invalid Agent State.. caller : {0}, Agent State : {1}", caller, _agent.AgentCurrentState), Logger.LogLevel.Error);
                    phoneController.rejectCall(sessionId, 486);
                    return -1;
                }
                if (call.CallCurrentState.GetType() != typeof(CallIdleState))
                    call.CallCurrentState = new CallIdleState();

                _agent.AgentCurrentState.OnIncomingCall(ref _agent, caller, sessionId);
                _agent.PortsipSessionId = sessionId;


                call.CallSessionId = caller.Split('@')[0].Replace("sip:", "");
                _agent.CallSessionId = call.CallSessionId;
                call.SetDialInfo(sessionId, Guid.NewGuid());
                call.CallCurrentState.OnIncoming(ref call, callbackIndex, callbackObject, sessionId, calleeDisplayName, caller, calleeDisplayName, callee, audioCodecNames, videoCodecNames, existsAudio, existsVideo);
                call.currentCallLogId = Guid.NewGuid();

                new Thread(() => GetPhoneNo(_agent.CallSessionId)).Start();

                //this.Invoke(((MethodInvoker)(() =>
                //{
                //    txtStatus.ForeColor = Color.DarkMagenta;
                //    txtStatus.Text = "Call Incoming : " + callee;

                //    if (playRingtone)
                //        PlayRingTone();
                //    txtStatus.Text = "Call Incoming......";
                //    alert = new frmIncomingCall(reginTime);
                //    if (ShowCallAlert)
                //    {
                //        alert.Closed += (s, e) =>
                //        {
                //            if (AutoAnswer.Checked)
                //                return;
                //            if (isCallAnswerd)
                //                return;
                //            StopRingTone();
                //            if (alert.IsCallAnswered)
                //                MakeCall("");
                //            else
                //                EndCall();
                //        };
                //        alert.Show(this);
                //    }
                //})));

                var jsonString = sessionId + "|" + _agent.CallSessionId + "|" + callerDisplayName;


            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteIncoming", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteTrying(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteTrying", Logger.LogLevel.Info);
                PlayRingInTone();
                call.CallCurrentState.OnMakeCall(ref call);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteTrying", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteSessionProgress(int callbackIndex, int callbackObject, int sessionId, string audioCodecNames,
            string videoCodecNames, bool existsEarlyMedia, bool existsAudio, bool existsVideo)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteSessionProgress", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }
        public int onInviteAnswered(int callbackIndex, int callbackObject, int sessionId, string callerDisplayName, string caller,
            string calleeDisplayName, string callee, string audioCodecNames, string videoCodecNames, bool existsAudio,
            bool existsVideo)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteAnswered", Logger.LogLevel.Info);
                StopRingTone();
                StopRingInTone();

                call.CallCurrentState.OnAnswer(ref call);
                this.Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = System.Drawing.Color.DarkGoldenrod;
                    txtStatus.Text = "Call Answered";
                })));
                _missCallCount = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteAnswered", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteFailure(int callbackIndex, int callbackObject, int sessionId, string reason, int code)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteFailure", Logger.LogLevel.Info);
                DisableAlert();
                StopRingInTone();
                StopRingTone();
                call.CallCurrentState.OnCallReject(ref call);
                var sid = _agent.CallSessionId;
                _agent.AgentCurrentState.OnFailMakeCall(ref _agent);
                this.Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    txtStatus.Text = "Call Rejected from Other End" + reason;
                })));
                isCallAnswerd = false;

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteFailure", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteUpdated(int callbackIndex, int callbackObject, int sessionId, string audioCodecNames, string videoCodecNames,
            bool existsAudio, bool existsVideo)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteUpdated", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onInviteConnected(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteConnected", Logger.LogLevel.Info);
                isCallAnswerd = true;
                DisableAlert();
                StopRingTone();
                StopRingInTone();
                callStarTime = DateTime.Now;
                CallDurations.Enabled = true;
                CallDurations.Start();
                call.CallCurrentState.OnAnswer(ref call);
                this.Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = System.Drawing.Color.DarkGoldenrod;
                    txtStatus.Text = "Call Established.";
                })));
                _missCallCount = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteAnswered", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteBeginingForward(int callbackIndex, int callbackObject, string forwardTo)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteBeginingForward", Logger.LogLevel.Info);
                this.Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = System.Drawing.Color.DarkGoldenrod;
                    txtStatus.Text = "Call Begining Forward.";
                })));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteBeginingForward", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteClosed(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteClosed", Logger.LogLevel.Info);
                DisableAlert();
                StopRingInTone();
                StopRingTone();
                call.CallCurrentState.OnDisconnected(ref call);
                _agent.AgentCurrentState.OnEndCall(ref _agent,isCallAnswerd);
                this.Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    txtStatus.Text = "Call Ended";
                })));

                if (!isCallAnswerd)
                {
                    AddMiscallToCallLogs();
                }
                AddCallDurations();
                isCallAnswerd = false;

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteClosed", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onRemoteHold(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRemoteHold", Logger.LogLevel.Info);
                this.Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    txtStatus.Text = "Remote Hold";
                })));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
            } return 0;
        }

        public int onRemoteUnHold(int callbackIndex, int callbackObject, int sessionId, string audioCodecNames, string videoCodecNames,
            bool existsAudio, bool existsVideo)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRemoteUnHold", Logger.LogLevel.Info);
                this.Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    txtStatus.Text = "Remote UnHold";
                })));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
            } return 0;
        }

        public int onReceivedRefer(int callbackIndex, int callbackObject, int sessionId, int referId, string to, string @from,
            string referSipMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onReceivedRefer", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onReferAccepted(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onReferAccepted", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onReferRejected(int callbackIndex, int callbackObject, int sessionId, string reason, int code)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onReferRejected", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onTransferTrying(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onTransferTrying", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onTransferRinging(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onTransferRinging", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onACTVTransferSuccess(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onACTVTransferSuccess", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onACTVTransferFailure(int callbackIndex, int callbackObject, int sessionId, string reason, int code)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onACTVTransferFailure", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onReceivedSignaling(int callbackIndex, int callbackObject, int sessionId, StringBuilder signaling)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onReceivedSignaling", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onSendingSignaling(int callbackIndex, int callbackObject, int sessionId, StringBuilder signaling)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSendingSignaling", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onWaitingVoiceMessage(int callbackIndex, int callbackObject, string messageAccount, int urgentNewMessageCount,
            int urgentOldMessageCount, int newMessageCount, int oldMessageCount)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onWaitingVoiceMessage", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onWaitingFaxMessage(int callbackIndex, int callbackObject, string messageAccount, int urgentNewMessageCount,
            int urgentOldMessageCount, int newMessageCount, int oldMessageCount)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onWaitingFaxMessage", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onRecvDtmfTone(int callbackIndex, int callbackObject, int sessionId, int tone)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRecvDtmfTone", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onRecvOptions(int callbackIndex, int callbackObject, StringBuilder optionsMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRecvOptions", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onRecvInfo(int callbackIndex, int callbackObject, StringBuilder infoMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRecvInfo", Logger.LogLevel.Info);
                ReceveMeassge("Receive Information", infoMessage.ToString());

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onPresenceRecvSubscribe(int callbackIndex, int callbackObject, int subscribeId, string fromDisplayName, string @from,
            string subject)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onPresenceRecvSubscribe", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onPresenceOnline(int callbackIndex, int callbackObject, string fromDisplayName, string @from, string stateText)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onPresenceOnline", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onPresenceOffline(int callbackIndex, int callbackObject, string fromDisplayName, string @from)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onPresenceOffline", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onRecvMessage(int callbackIndex, int callbackObject, int sessionId, string mimeType, string subMimeType,
            byte[] messageData, int messageDataLength)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRecvMessage", Logger.LogLevel.Info);
                if (mimeType == "text" && subMimeType == "plain")
                {
                    string mesageText = GetString(messageData);
                    ReceveMeassge("Receive Information", mesageText);

                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
            } return 0;
        }

        public int onRecvOutOfDialogMessage(int callbackIndex, int callbackObject, string fromDisplayName, string @from,
            string toDisplayName, string to, string mimeType, string subMimeType, byte[] messageData, int messageDataLength)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRecvOutOfDialogMessage", Logger.LogLevel.Info);
                if (mimeType == "text" && subMimeType == "plain")
                {
                    string mesageText = GetString(messageData);
                    ReceveMeassge("Receive Information", mesageText);

                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
            } return 0;
        }

        public int onSendMessageSuccess(int callbackIndex, int callbackObject, int sessionId, int messageId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSendMessageSuccess", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onSendMessageFailure(int callbackIndex, int callbackObject, int sessionId, int messageId, string reason, int code)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSendMessageFailure", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onSendOutOfDialogMessageSuccess(int callbackIndex, int callbackObject, int messageId, string fromDisplayName,
            string @from, string toDisplayName, string to)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSendOutOfDialogMessageSuccess", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onSendOutOfDialogMessageFailure(int callbackIndex, int callbackObject, int messageId, string fromDisplayName,
            string @from, string toDisplayName, string to, string reason, int code)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSendOutOfDialogMessageFailure", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onPlayAudioFileFinished(int callbackIndex, int callbackObject, int sessionId, string fileName)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onPlayAudioFileFinished", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onPlayVideoFileFinished(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onPlayVideoFileFinished", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onReceivedRtpPacket(IntPtr callbackObject, int sessionId, bool isAudio, byte[] RTPPacket, int packetSize)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onReceivedRtpPacket", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onSendingRtpPacket(IntPtr callbackObject, int sessionId, bool isAudio, byte[] RTPPacket, int packetSize)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSendingRtpPacket", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onAudioRawCallback(IntPtr callbackObject, int sessionId, int callbackType, byte[] data, int dataLength,
            int samplingFreqHz)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onAudioRawCallback", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onVideoRawCallback(IntPtr callbackObject, int sessionId, int callbackType, int width, int height, byte[] data,
            int dataLength)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onVideoRawCallback", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        #endregion SIPCallbackEvents

        #region UI State

        public void ShowNotifications(ResourceProxyReplyDataResourceProxyReply result)
        {
            try
            {
                string msg;
                switch (result.Command)
                {
                    case WorkflowResultCode.ACDS301:
                    case WorkflowResultCode.ACDS4032:
                        msg = "Successfully Granted BREAK";
                        break;
                    case WorkflowResultCode.ACDS302: //- Agent registered for BREAK request "ACDS302"
                        msg = "Registered for BREAK request";
                        this.Invoke(new MethodInvoker(delegate
                        {
                            cancelRequestToolStripMenuItem.Enabled = true;
                            officialBreakToolStripMenuItem.Enabled = false;
                            mealBreakToolStripMenuItem.Enabled = false;
                            aUXBreakToolStripMenuItem.Enabled = false;
                        }));
                        break;

                    case WorkflowResultCode.ACDE301: //- Agent does not exist / not correctly registered "ACDE301"
                        msg = "Agent does not exist / not correctly registered";
                        break;

                    case WorkflowResultCode.ACDE302: //- Agent cannot go to BREAK while on Initalizing State "ACDE302"
                        msg = "Agent cannot go to BREAK while on Initializing State.";
                        break;

                    case WorkflowResultCode.ACDE303: //- Agent cannot go to BREAK while on OFFLINE requested "ACDE303"
                        msg = "Agent cannot go to BREAK while on OFFLINE requested.";
                        break;

                    default:
                        msg = "Receiving Agent Break Info.";
                        break;
                }
                mynotifyicon.ShowBalloonTip(5, "Duo Soft Phone", msg, ToolTipIcon.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ShowNotifications", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void ShowCallLogs()
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ShowCallLogs", Logger.LogLevel.Debug);
        }

        public void ShowSetting()
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ShowSetting", Logger.LogLevel.Debug);
        }

        public void InAgentIdleState(AgentEvent agentPvState)
        {
            try
            {
                this.Invoke(((MethodInvoker)(() =>
                {
                    OFFLINE.Visible = false;
                    gbBreakMode.SendToBack();
                    gbBreakMode.Visible = false;
                    breakRequestToolStripMenuItem.Enabled = true;
                    cancelRequestToolStripMenuItem.Enabled = false;
                    endBreakToolStripMenuItem.Enabled = false;
                    textBoxNumber.BackColor = Color.Black;
                    txtStatus.BackColor = Color.Black;

                    txtStatus.ForeColor = Color.DarkGreen;
                    txtStatus.Text = "IDLE";
                    textBoxNumber.Text = string.Empty;
                    CurrentNumber = string.Empty;
                    buttonAnswer.Enabled = true;
                    buttonReject.Enabled = false;
                    //PanelPhone.Enabled = true;
                    gbFreeze.Visible = false;
                    btnFreez.Visible = false;
                   
                    this.ActiveControl = textBoxNumber;
                    textBoxNumber.Focus();
                    if (agentPvState.GetType() == typeof(AgentInitiate) ||
                        agentPvState.GetType() == typeof(AgentOffline))
                    {
                        PhoneStatus.Image = Properties.Resources.online;
                        new Thread(() => resourceCheckerHandler.StartStatusMap()).Start();
                    }

                    setPhonePanelLocation();
                    //PanelPhone.Visible = true;
                })));

                _agent.CallSessionId = string.Empty;
                _agent.PortsipSessionId = -1;
                _agent.IsCallAnswer = false;
                call.CallSessionId = string.Empty;
                call.portSipSessionId = -1;
                call.PhoneNo = string.Empty;

                CallDurations.Stop();
                CallDurations.Enabled = false;
                acwCutdownTimer.Stop();
                acwCutdownTimer.Enabled = false;
                FreezeDurations.Stop();
                FreezeDurations.Enabled = false;

                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "CallSessionId set to Empty.", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InIdleState", exception, Logger.LogLevel.Error);
            }
        }

        public void InAgentAcwState()
        {
            try
            {
                CallDurations.Stop();
                CallDurations.Enabled = false;
                acwCotdown = acwTime;
                acwCutdownTimer.Enabled = true;
                acwCutdownTimer.Start();
                btnFreez.Invoke(new MethodInvoker(() =>
                {
                    gbFreeze.Visible = true; btnFreez.Visible = true;
                    btnFreez.Enabled = true;
                    buttonAnswer.Enabled = false;
                    buttonReject.Enabled = false;
                    txtStatus.Text = string.Format("ACW : {0}", acwCotdown);
                }));
                InCallIdleState();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InAcwState", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallConnectedState()
        {
            try
            {
                this.Invoke(((MethodInvoker)(() =>
                {
                    buttonHold.Text = "Hold";
                    buttonHold.Enabled = true;

                    buttonAnswer.Enabled = false;
                    buttonReject.Enabled = true;
                    buttontransferIvr.Enabled = false;
                    buttontransferCall.Enabled = false;
                    buttonEtl.Enabled = false;
                    buttonswapCall.Enabled = false;
                    buttonConference.Enabled = false;
                })));
                _agent.IsCallAnswer = true;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InCallConnectedState", exception, Logger.LogLevel.Error);
            }
        }

        public void InOfflineState(string statusText, string msg, int statusCode)
        {

            try
            {
                this.Invoke(((MethodInvoker)(() =>
                {
                    phoner8ClickMenu.Enabled = false;
                    PhoneStatus.Image = Properties.Resources.offline;
                    txtStatus.ForeColor = Color.Red;
                    btnReregister.Visible = true;
                    txtStatus.Text = statusText;
                    mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", txtStatus.Text, ToolTipIcon.Error);
                    OFFLINE.Text = msg;
                    OFFLINE.Visible = true;
                    if (statusCode == -9999)
                        UninitializePhone();
                })));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InOfflineState", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void InInitiateState()
        {
            try
            {
                this.Invoke(((MethodInvoker)(() =>
                {
                    buttonHold.Enabled = false;
                    buttonHold.Text = "Hold";
                    txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    txtStatus.Text = "";

                    buttonAnswer.Enabled = false;
                    buttonReject.Enabled = false;
                    buttontransferIvr.Enabled = false;
                    buttontransferCall.Enabled = false;
                    buttonEtl.Enabled = false;
                    buttonswapCall.Enabled = false;
                    buttonConference.Enabled = false;
                   
                })));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InInitiateState", exception, Logger.LogLevel.Error);
            }
        }

        public void InInitiateMsgState(bool autoAnswerchk, bool autoAnswerEnb, string userName)
        {
            try
            {
                this.Invoke(((MethodInvoker)(() =>
                {
                    buttonHold.Enabled = false;
                    buttonHold.Text = "Hold";
                    txtStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    txtStatus.Text = "";

                    buttonAnswer.Enabled = false;
                    buttonReject.Enabled = false;
                    buttontransferIvr.Enabled = false;
                    buttontransferCall.Enabled = false;
                    buttonEtl.Enabled = false;
                    buttonswapCall.Enabled = false;
                    buttonConference.Enabled = false;
                   
                })));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InInitiateMsgState", exception, Logger.LogLevel.Error);
            }
        }

        public void Error(string statusText)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Error" + statusText, Logger.LogLevel.Error);
        }

        public void InBreakState()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    gbBreakMode.BringToFront();
                    txtStatus.Text = "Break Mode";
                    textBoxNumber.BackColor = Color.DarkRed;
                    txtStatus.BackColor = Color.DarkRed;
                    gbBreakMode.Visible = true;
                    StartDurations();
                    breakRequestToolStripMenuItem.Enabled = false;
                    cancelRequestToolStripMenuItem.Enabled = false;
                    endBreakToolStripMenuItem.Enabled = true;
                }));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InBreakState", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallAgentClintConnectedState()
        {
            try
            {
                this.Invoke(((MethodInvoker)(() =>
                {
                    buttonswapCall.Enabled = true;
                    buttonEtl.Enabled = true;
                    buttonConference.Enabled = true;
                    buttonHold.Enabled = false;
                    buttontransferCall.Enabled = false;
                    buttontransferIvr.Enabled = false;

                    answerCallToolStripMenuItem.Enabled = false;
                    rejectCallToolStripMenuItem.Enabled = false;
                    holdCallToolStripMenuItem.Enabled = false;
                })));

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InCallAgentClintConnectedState", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallAgentSupConnectedState(CallActions callAction)
        {
            try
            {
                this.Invoke(((MethodInvoker)(() =>
                {
                    buttonswapCall.Enabled = true;
                    buttonEtl.Enabled = false;
                    buttonConference.Enabled = false;
                    buttontransferCall.Enabled = false;
                    buttontransferIvr.Enabled = false;
                    buttonHold.Enabled = false;

                    answerCallToolStripMenuItem.Enabled = false;
                    rejectCallToolStripMenuItem.Enabled = false;
                    holdCallToolStripMenuItem.Enabled = false;
                })));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InCallAgentSupConnectedState", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallConferenceState()
        {
            try
            {
                this.Invoke(((MethodInvoker)(() =>
                {
                    buttonEtl.Enabled = false;
                    buttonswapCall.Enabled = false;
                    buttonConference.Enabled = false;
                    buttonHold.Enabled = false;
                    buttonAnswer.Enabled = false;
                    buttonReject.Enabled = true;
                    buttonHold.Enabled = false;
                    buttontransferCall.Enabled = false;
                    buttonConference.Enabled = false;
                    buttontransferIvr.Enabled = false;
                    buttonEtl.Enabled = true;
                    buttonswapCall.Enabled = false;

                    answerCallToolStripMenuItem.Enabled = false;
                    rejectCallToolStripMenuItem.Enabled = false;
                    holdCallToolStripMenuItem.Enabled = false;
                })));

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InCallConferenceState", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallDisconnectedState()
        {
            try
            {
                this.Invoke(((MethodInvoker)(() =>
                {
                    buttonHold.Enabled = false;
                    buttonHold.Text = "Hold";

                    buttonAnswer.Enabled = false;
                    buttonReject.Enabled = false;
                    buttontransferIvr.Enabled = false;
                    buttontransferCall.Enabled = false;
                    buttonEtl.Enabled = false;
                    buttonswapCall.Enabled = false;
                    buttonConference.Enabled = false;

                    answerCallToolStripMenuItem.Enabled = false;
                    rejectCallToolStripMenuItem.Enabled = false;
                    holdCallToolStripMenuItem.Enabled = false;
                })));

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InCallConferenceState", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallHoldState(CallActions callAction)
        {
            try
            {
                this.Invoke(((MethodInvoker)(() =>
                {
                    buttonHold.Text = "Unhold";

                    buttontransferCall.Enabled = true;
                    buttontransferIvr.Enabled = false;
                    buttonHold.Enabled = true;
                    buttonEtl.Enabled = false;

                    answerCallToolStripMenuItem.Enabled = false;
                    rejectCallToolStripMenuItem.Enabled = false;
                    holdCallToolStripMenuItem.Enabled = true;
                })));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InCallHoldState", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallIdleState()
        {
            try
            {
                this.Invoke(((MethodInvoker)(() =>
                {
                    buttonAnswer.Enabled = true;
                    buttonReject.Enabled = false;
                    buttonHold.Enabled = false;
                    buttontransferCall.Enabled = false;
                    buttonConference.Enabled = false;
                    buttontransferIvr.Enabled = false;
                    buttonEtl.Enabled = false;
                    buttonswapCall.Enabled = false;

                    answerCallToolStripMenuItem.Enabled = false;
                    rejectCallToolStripMenuItem.Enabled = false;
                    holdCallToolStripMenuItem.Enabled = false;
                })));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InCallIdleState", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void InAgentBusy(CallDirection callDirection)
        {
            try
            {
                this.Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.Text = string.Empty;
                    var val = callDirection == CallDirection.Incoming;
                    buttonAnswer.Enabled = val;
                    buttonReject.Enabled = val;
                    buttontransferIvr.Enabled = false;
                    buttontransferCall.Enabled = false;
                    buttonEtl.Enabled = false;
                    buttonswapCall.Enabled = false;
                    buttonConference.Enabled = false;

                    answerCallToolStripMenuItem.Enabled = false;
                    rejectCallToolStripMenuItem.Enabled = !IsNotAllowToReject;
                    holdCallToolStripMenuItem.Enabled = false;

                    if (IsCallLogOpen)
                        logs.Close();
                })));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InAgentBusy", exception, Logger.LogLevel.Error);
            }
        }

        public void CancelBreakRequest(ResourceProxyReplyDataResourceProxyReply s)
        {
            try
            {
                if (s.Command == WorkflowResultCode.ACDS701)
                {
                    this.Invoke(((MethodInvoker)(() =>
                    {
                        breakRequestToolStripMenuItem.Enabled = true;
                        cancelRequestToolStripMenuItem.Enabled = false;
                        endBreakToolStripMenuItem.Enabled = false;

                        officialBreakToolStripMenuItem.Enabled = true;
                        mealBreakToolStripMenuItem.Enabled = true;
                        aUXBreakToolStripMenuItem.Enabled = true;
                    })));
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InAgentBusy", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallRingingState()
        {
            try
            {
                this.Invoke(((MethodInvoker)(() =>
                {
                    buttonAnswer.Enabled = false;
                    buttonReject.Enabled = true;
                    buttonHold.Enabled = false;
                    buttontransferCall.Enabled = false;
                    buttonConference.Enabled = false;
                    buttontransferIvr.Enabled = false;
                    buttonEtl.Enabled = false;
                    buttonswapCall.Enabled = false;

                    answerCallToolStripMenuItem.Enabled = false;
                    rejectCallToolStripMenuItem.Enabled = !IsNotAllowToReject;
                    holdCallToolStripMenuItem.Enabled = false;


                })));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InCallRingingState", exception, Logger.LogLevel.Error);
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
                this.Invoke(((MethodInvoker)(() =>
                {
                    buttonAnswer.Enabled = true;
                    buttonReject.Enabled = true;
                    buttonHold.Enabled = false;
                    buttontransferCall.Enabled = false;
                    buttonConference.Enabled = false;
                    buttontransferIvr.Enabled = false;
                    buttonEtl.Enabled = false;
                    buttonswapCall.Enabled = false;
                    answerCallToolStripMenuItem.Enabled = true;
                    rejectCallToolStripMenuItem.Enabled = !IsNotAllowToReject;
                    holdCallToolStripMenuItem.Enabled = false;
                })));

                this.Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = Color.DarkMagenta;
                    txtStatus.Text = "Incoming Call";

                    if (playRingtone)
                        PlayRingTone();
                    alert = new frmIncomingCall(IsNotAllowToReject,reginTime);
                    if (ShowCallAlert)
                    {
                        alert.Closed += (s, e) =>
                        {
                            if (AutoAnswer.Checked)
                                return;
                            if (isCallAnswerd)
                                return;
                            StopRingTone();
                            if (alert.IsCallAnswered)
                                MakeCall("");
                            else
                                EndCall();
                        };
                        alert.Show(this);
                    }
                })));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InCallIncommingState", exception, Logger.LogLevel.Error);
            }
        }

        public void OnResourceModeChanged(AgentMode mode)
        {
            try
            {
                switch (mode)
                {
                    case AgentMode.Offline:
                        break;
                    case AgentMode.Inbound:
                        {
                            this.Invoke(new MethodInvoker(() =>
                            {
                                outboundToolStripMenuItem.Enabled = true;
                                inboundToolStripMenuItem.Enabled = false;
                                lblAgentMode.Image = Properties.Resources.AgentInboundMode;
                                lblAgentMode.Text = "";
                                toolTip1.ToolTipTitle = "Inbound";
                                mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Agent Mode Change to Inbound.", ToolTipIcon.Info);
                            }));
                        }
                        break;
                    case AgentMode.Outbound:
                        {
                            this.Invoke(new MethodInvoker(() =>
                            {
                                outboundToolStripMenuItem.Enabled = false;
                                inboundToolStripMenuItem.Enabled = true;
                                lblAgentMode.Image = Properties.Resources.AgentOutboundMode;
                                lblAgentMode.Text = "";
                                toolTip1.ToolTipTitle = "Outbound";
                                mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Agent Mode Change to Outbound.", ToolTipIcon.Info);
                            }));
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException("mode");
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "OnResourceModeChanged", exception, Logger.LogLevel.Error);
            }
        }

        public void OnSendModeChangeRequestOutbound(ResourceProxyReplyDataResourceProxyReply s)
        {
            try
            {
                if (s.Command == WorkflowResultCode.Error)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        outboundToolStripMenuItem.Enabled = true;
                        inboundToolStripMenuItem.Enabled = true;

                    }));
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Error Occur in Mode Change Request. {0}", s), Logger.LogLevel.Error);

                }
                if (s.Command == WorkflowResultCode.ACDS502)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        lblAgentMode.Image = Properties.Resources.AgentOutboundModeQ ;
                        mynotifyicon.ShowBalloonTip(5, "Duo Soft Phone", "Agent Successfully Registered for Mode Change.", ToolTipIcon.Info);
                    }));
                }
                if (s.Command == WorkflowResultCode.ACDE502)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        outboundToolStripMenuItem.Enabled = false;
                        inboundToolStripMenuItem.Enabled = true;
                    }));
                }

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "OnSendModeChangeRequestOutbound", exception, Logger.LogLevel.Error);
            }
        }

        public void OnSendModeChangeRequestInbound(ResourceProxyReplyDataResourceProxyReply s)
        {
            try
            {
                switch (s.Command)
                {
                    case WorkflowResultCode.ACDS502:
                        {
                            this.Invoke(new MethodInvoker(() =>
                            {
                                lblAgentMode.Image = Properties.Resources.AgentInboundModeQ;
                                mynotifyicon.ShowBalloonTip(5, "Duo Soft Phone", "Agent Successfully Registered for Mode Change.", ToolTipIcon.Info);
                            }));
                        }
                        break;
                    case WorkflowResultCode.ACDE502:
                        {
                            this.Invoke(new MethodInvoker(() =>
                            {
                                outboundToolStripMenuItem.Enabled = true;
                                inboundToolStripMenuItem.Enabled = false;
                            }));
                        }
                        break;
                    case WorkflowResultCode.Error:
                        {
                            this.Invoke(new MethodInvoker(() =>
                            {
                                outboundToolStripMenuItem.Enabled = true;
                                inboundToolStripMenuItem.Enabled = true;

                            }));
                            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Error Occur in Mode Change Request. {0}", s), Logger.LogLevel.Error);
                        }
                        break;
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "OnSendModeChangeRequestOutbound", exception, Logger.LogLevel.Error);
            }
        }


        #endregion

        

        

    }
}