using DuoCallTesterLicenseKey;
using DuoSoftware.DuoSoftPhone.Controllers;
using DuoSoftware.DuoSoftPhone.Controllers.AgentStatus;
using DuoSoftware.DuoSoftPhone.Controllers.CallStatus;
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoSoftPhone.Controllers.Service;
using DuoSoftware.DuoSoftPhone.RefResourceProxy;
using DuoSoftware.DuoSoftPhone.RefUserAuth;
using DuoSoftware.DuoTools.DuoLogger;
using PortSIP;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using DuoSoftware.DuoSoftPhone.Properties;
using SpeechLib;
using TheCodeKing.Net.Messaging;
using WowserBrowser;
using AgentMode = DuoSoftware.DuoSoftPhone.Controllers.AgentStatus.AgentMode;
using Timer = System.Timers.Timer;
using System.Configuration;
using DuoSoftware.DuoSoftPhone.Controllers.Listners;

namespace DuoSoftware.DuoSoftPhone.UI
{
    public partial class FormDialPad : Form, SIPCallbackEvents, IUiState
    {
        #region private variables

        private Logger logger;
        
        private enum TransferType
        {
            Transfer = 0,
            Ivrtransfer = 1,
        }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InternetSetCookie(string UrlName, string CookieName, string CookieData);

        private bool waitToStartCallTIme;
        private readonly string _startupPath;
        private readonly string _favFile;
        private ResourceMonitoringHandler _resMonitoringHandler;
        private ResourceCheckerHandler _resourceCheckerHandler;
        private MainConfigHandler _resMainConfigHandler;
        private readonly string _passWord;
        private int _acwTime;
        private int _acwCotdown;
        private readonly Agent _agent;
        private Call _call;
        private DateTime _callStarTime;
        private Timer _callDurations;
        private DateTime _freezeStarTime;
        private Timer _freezeDurations;
        private Timer _acwCutdownTimer;
        private int _sipRegisterTryCount;
        private bool _isCallAnswerd;
        private Dictionary<Guid, CallLog> _callLogs;
        private string _selectedSpeaker = string.Empty;
        private string _selectedMic = string.Empty;
        private FrmIncomingCall _alert;
        private ComboBox _comboBoxMicrophones;
        private ComboBox _comboBoxSpeakers;
        private TextBox _textBoxNumber;
        private IXDListener _listner;
        private string _filePath = string.Empty;
        private readonly string _ringInfilePath = string.Empty;
        private bool _playRingtone = false;
        private bool isGreetingEnable = false;
        private bool _playRingInToneMenually = false;
        private bool _isNotAllowToReject = false;
        private bool _playingRingIntone = false;
        private bool _showCallAlert = false;
        private SoundPlayer _wavPlayer;
        private SoundPlayer _wavPlayerRingIn;

        private NameValueCollection appSetting;
        // Create the list to use as the custom source.
        private readonly AutoCompleteStringCollection _source = new AutoCompleteStringCollection();

        private CallHandler _callHandler;
        private PortSIPLib _phoneController;
        private int _reginTime;
        private string _n;
        
        private string CurrentNumber
        {
            get { return _n; }
            set
            {
                if (_n == null)
                    _n = string.Empty;
                _n = value;
                _textBoxNumber.Text = _n;
            }
        }

        #endregion private variables

        #region Private Methods

        private void StartCallTImmer()
        {
            _callStarTime = DateTime.Now;
            _callDurations.Enabled = true;
            _callDurations.Start();
        }

        private void PlayAgentGreeting(string language)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Start PlayAgentGreeting : {0},{1}", language,_call.CallSessionId), Logger.LogLevel.Info);
                if(_agent.AgentCurrentState.GetType() == typeof(AgentBusy))
                {
                    if (_call.CallCurrentState.GetType() == typeof(CallIncommingState) || _call.CallCurrentState.GetType() == typeof(CallRingingState) || _call.CallCurrentState.GetType() == typeof(CallTryingState)) 
                    {                                             
                        if (string.IsNullOrEmpty(language)) return;
                        if(_wavPlayer != null)
                           _wavPlayer.Stop();
                        var voice = new SpVoice();
                        voice.Speak(string.Format("There is a {0} Call", language));
                        language = string.Empty;
                        PlayRingTone();
                    }
                }
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Format("End PlayAgentGreeting. {0}|{1}|{2}", _call.CallCurrentState, _agent.AgentCurrentState, _call.CallSessionId), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "PlayAgentGreeting",exception, Logger.LogLevel.Error);
            }
        }

        private IEnumerable<Control> GetAll(Control control, Type type)
        {
            try
            {
                var controls = control.Controls.Cast<Control>();

                return controls.SelectMany(ctrl => GetAll(ctrl, type))
                    .Concat(controls)
                    .Where(c => c.GetType() == type);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "GetAll", exception, Logger.LogLevel.Error);
                return null;
            }
        }

        private void AddTabPage(string url)
        {
            try
            {
                var tab = new MyTab(new MyTabPara{MainTabControl = MainTabControl,PreviewPanel = PreviewPanel,BackToolStripButton = BackToolStripButton,ForwardToolStripButton = ForwardToolStripButton,StopToolStripButton = StopToolStripButton,PreviewSizeNumericUpDown = PreviewSizeNumericUpDown,AddressToolStripComboBox = AddressToolStripComboBox,MainToolTip = MainToolTip,UserName = _agent.Auth.UserName,PassWord = _passWord}).AddTab(url,phoner8ClickMenu);
                MainTabControl.SelectedTab = tab;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "AddTabPage", exception, Logger.LogLevel.Error);
            }
        }

        private void SelectDuoTab()
        {
            try
            {
                Invoke(((MethodInvoker)(() =>
                {
                    var tab1 =
                        MainTabControl.TabPages.Cast<TabPage>()
                            .First(n => n.Text.ToLower().Contains(_agent.CallHandler.SipServiceCall.BaseTabName));
                    MainTabControl.SelectedTab = tab1;
                })));
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "SelectDuoTab", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void HandleDeviceEvent()
        {
            try
            {
                if (_call.CallCurrentState.GetType() != typeof(CallIdleState))
                {
                    logger.LogMessage(Logger.LogAppender.DuoDeviceMonitorSmtp,
                        string.Format(
                            "The audio device on the computer [{0}] used by [{1}], was either removed /reconnected during an ongoing call, therefore the   agent console will be disabled  till remedial action is taken.\n Agent Session ID [{2}], Call Session ID : {3} .\n \n\n\n\nTHIS IS AN AUTO-GENERATED MESSAGE - PLEASE DO NOT REPLY TO THIS MESSAGE.\n\n\n\n\n",
                            _agent.IP, _agent.Auth.UserName, "***********", _call.CallSessionId), Logger.LogLevel.Error);
                    //The audio device on the computer [{0}] used by [{1}], was either removed or reconnected during an ongoing call, please take necessary steps to resolve the issue. %n%n%n THIS IS AN AUTO-GENERATED MESSAGE - PLEASE DO NOT REPLY TO THIS MESSAGE.
                    //The sound device on [{0}]  used by [{1}] was either forcefully  or unintentionally  disconnected/connected while on call, the console will be frozen till remedial action is taken
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDeviceMonitor, "HandleDeviceEvent", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void LoadIvr()
        {
            try
            {
                _resMainConfigHandler.OnGetIvrListCompleted += a =>
                {
                    try
                    {
                        GridIvrList.Invoke(new MethodInvoker(delegate
                        {
                            GridIvrList.DataSource = a;
                        }));
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoDefault, "OnGetIvrListCompleted", exception,
                            Logger.LogLevel.Error);
                    }
                };
                _resMainConfigHandler.GetIvRlist(_agent.Auth.SecurityToken);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "LoadIvr", exception, Logger.LogLevel.Error);
            }
        }

        private void Reregister()
        {
            try
            {
                //UninitializePhone();
                //StopRingInTone();
                //StopRingTone();
                //PhoneStatus.Image = Resources.offline;
                var account = new FrmPhoneConfig(_agent.Auth) { StartPosition = FormStartPosition.CenterParent };
                account.FormClosing += (o, t) =>
                {
                    try
                    {
                        txtStatus.Invoke(new MethodInvoker(delegate
                        {
                            txtStatus.ForeColor = Color.DarkGreen;
                            txtStatus.Text = Resources.FormDialPad_Reregister_Initializing;
                            mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Phone Reinitializing.", ToolTipIcon.Info);
                        }));

                        UninitializePhone();
                        InitializePhone(true);
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoDefault,
                            "accountSettingToolStripMenuItem_Click",
                            exception, Logger.LogLevel.Error);
                    }
                };

                account.ShowDialog(this);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
            }
        }

        private void OpenLogFils()
        {
            try
            {
                var filePaths =
                    Directory.GetFiles(string.Format(@"C:\Users\{0}\AppData\Local\DuoSoftware", Environment.UserName),
                        "*.duo"); //Directory.EnumerateFiles(workingDirectory, "*.duo")
                foreach (var file in filePaths)
                {
                    try
                    {
                        var startInfo = new ProcessStartInfo
                        {
                            FileName = "notepad.exe",
                            Arguments = file
                        };
                        Process.Start(startInfo);

                        //var notepadLog = new ProcessStartInfo("notepad.exe", file);//string.Format("{0}\\{1}.dat", workingDirectory, file)
                        //Process.Start(notepadLog);
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoDefault, "ViewErrorsLogFile - Find Controler",
                            exception, Logger.LogLevel.Error);
                    }
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "OpenLogFils", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void OpenLogFilsDirectory()
        {
            try
            {
                Process.Start("explorer.exe",
                    string.Format(@"C:\Users\{0}\AppData\Local\DuoSoftware", Environment.UserName));
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "OpenLogFilsDirectory", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void GetPhoneNo(string callSessionId)
        {
            try
            {
                _call.PhoneNo = string.Empty;
                var phnNo = EngagementHandler.GetPhoneNo(_agent.Auth.SecurityToken, callSessionId);
                if (_agent.AgentCurrentState.GetType() == typeof(AgentIdle))
                {
                    logger.LogMessage(Logger.LogAppender.DuoDefault, "Get info after call Disconnected",
                        Logger.LogLevel.Info);
                    return;
                }
                if (string.IsNullOrEmpty(phnNo))
                {
                    phnNo = _call.CallSessionId;
                }
                Invoke(((MethodInvoker)(() =>
                {
                    //txtStatus.Text =  "Incoming Call : " + phnNo;
                    _textBoxNumber.Text = phnNo;
                    _call.PhoneNo = phnNo;
                    if (!_source.Contains(phnNo))
                        _source.Add(phnNo);
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
                if (info != null)
                { 
                    var lang = info.FirstOrDefault(e => e.Key.Equals("language"));
                    var phone = info.FirstOrDefault(e => e.Key.Equals("phoneNumber"));

                    var ivr = info.FirstOrDefault(e => e.Key.Equals("ivrPath"));
                    var ivrInfo = string.Format("language : {0} , Phone No : {1} , IVR : {2} ", lang.Value, phone.Value, ivr.Value);
                    if(isGreetingEnable)
                        PlayAgentGreeting(lang.Value);
                    mynotifyicon.ShowBalloonTip(100, "Duo Soft Phone", ivrInfo, ToolTipIcon.Info);
                    logger.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Display Call info-Main : {0}:{1}> {2}", callSessionId, phnNo, ivrInfo), Logger.LogLevel.Info);
                }
                
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "GetPhoneNo", exception, Logger.LogLevel.Error);
            }
        }

        private void PlayRingTone()
        {
            try
            {
                if (_call.CallCurrentState.GetType() == typeof(CallIncommingState) || _call.CallCurrentState.GetType() == typeof(CallRingingState) || _call.CallCurrentState.GetType() == typeof(CallTryingState))
                {
                    logger.LogMessage(Logger.LogAppender.DuoDefault, "Start to play ring tone", Logger.LogLevel.Info);
                    if (_playRingtone)
                        _wavPlayer.PlayLooping();
                    logger.LogMessage(Logger.LogAppender.DuoDefault, "PlayRingTone > End", Logger.LogLevel.Info);
                }
                
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "PlayRingTone", exception,Logger.LogLevel.Error);
            }
        }

        private void StopRingTone()
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "StopRingTone>Start", Logger.LogLevel.Info);
                if (_wavPlayer == null) return;
                _wavPlayer.Stop();
                _wavPlayer.Dispose();
                logger.LogMessage(Logger.LogAppender.DuoDefault, "StopRingTone>End", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "StopRingTone", exception,Logger.LogLevel.Error);
            }
        }

        private void PlayRingInTone()
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "Start to play ringIn tone",
                    Logger.LogLevel.Info);
                if (!_playRingInToneMenually)
                {
                    logger.LogMessage(Logger.LogAppender.DuoDefault,
                        "Start to play ringIn tone- No Audio Files.", Logger.LogLevel.Error);
                    return;
                }
                if (_playingRingIntone)
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
                _playingRingIntone = true;
                logger.LogMessage(Logger.LogAppender.DuoDefault, "PlayRingInTone > End", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "PlayRingInTone", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void StopRingInTone()
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "StopRingInTone>Start", Logger.LogLevel.Info);
                _playingRingIntone = false;
                if (_wavPlayerRingIn == null) return;
                _wavPlayerRingIn.Stop();
                _wavPlayerRingIn.Dispose();
                logger.LogMessage(Logger.LogAppender.DuoDefault, "StopRingInTone>End", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "StopRingInTone", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void DisableAlert()
        {
            try
            {
                if ((_alert == null) || _alert.IsDisposed) return;
                if (InvokeRequired)
                {
                    _alert.Invoke(new MethodInvoker(delegate
                    {
                       if(!_alert.IsDisposed)
                           _alert.Close();
                    }));
                }
                else
                {
                    _alert.Close();   
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "DisableAlert", exception,
                    Logger.LogLevel.Error);
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
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Format("ReceveMeassge-> status : {0} , DialPadEventArgs: {1} , Agent State : {2} , Agent Mode : {3} , CallSessionId : {4}, waitToStartCallTIme : {5}", status, fullMessage, _agent.AgentCurrentState, _agent.AgentMode, _call.CallSessionId, waitToStartCallTIme), Logger.LogLevel.Info);
                if (string.IsNullOrEmpty(fullMessage)) return;

                txtStatus.Invoke(((MethodInvoker)(() => { txtStatus.ForeColor = Color.DimGray; txtStatus.Text = status; })));

                if (fullMessage.ToLower().Equals("remoteconnect"))
                {
                    if (waitToStartCallTIme)
                        StartCallTImmer();
                    return;
                }
                if (fullMessage.Contains(','))
                {
                    var splitData = fullMessage.Split(',');
                    var msgString = splitData.First().ToUpper();
                    switch (msgString)
                    {
                        case "SESSIONCREATED":
                            var sessionId = splitData[1];
                            _agent.CallSessionId = sessionId;
                            _agent.AgentCurrentState.OnMakeCall(_agent);
                            _call.CallSessionId = sessionId;
                            buttonReject.Invoke(new MethodInvoker(delegate { buttonReject.Enabled = true; }));
                            var jsonString = _agent.PortsipSessionId + "|" + _agent.CallSessionId + "|" + _call.PhoneNo;
                            InvokeBrowserScript("SIPEvents", new object[] { "OnCallOutgoing", _agent.CallSessionId, jsonString, _call.PhoneNo });
                            break;
                    }
                }
                else
                {
                    var msgString = fullMessage.ToUpper();
                    var reason = string.Empty;
                    if (fullMessage.Contains('-'))
                    {
                        var splitData = fullMessage.Split('-');
                        msgString = splitData.First().ToUpper();
                        reason = splitData[1];
                    }

                    if (msgString == "FAILED" || msgString == "FAIL")
                    {
                        txtStatus.Invoke(new MethodInvoker(delegate
                        {
                            txtStatus.ForeColor = Color.DarkRed;
                            txtStatus.Text = Environment.NewLine + (string.IsNullOrEmpty(reason) ? "Operation Fail." : reason);
                        }));

                        _call.CallCurrentState.OnOperationFail(_call);
                        mynotifyicon.ShowBalloonTip(2, "Duo Soft Phone", (string.IsNullOrEmpty(reason) ? "Operation Fail." : reason), ToolTipIcon.Error);
                    }
                    else if (msgString.ToUpper() == "ROUTEBACK" || msgString == "CALLFAIL")
                    {
                        _call.CallCurrentState.OnReset(_call);
                        txtStatus.Invoke(new MethodInvoker(delegate
                        {
                            txtStatus.ForeColor = Color.DarkGreen;
                            txtStatus.Text = Environment.NewLine + Resources.FormDialPad_ReceveMeassge_Call_Route_Back_;
                        }));
                        mynotifyicon.ShowBalloonTip(2, "Duo Soft Phone", Resources.FormDialPad_ReceveMeassge_Call_Route_Back_, ToolTipIcon.Info);
                    }
                    else if (msgString == "SUCCESS")
                    {
                        txtStatus.Invoke(new MethodInvoker(delegate
                        {
                            txtStatus.ForeColor = Color.DarkGreen;
                            txtStatus.Text = Environment.NewLine + Resources.FormDialPad_ReceveMeassge_Operation_Succeed_;
                        }));
                        _call.CallCurrentState.OnSetStatus( _call);
                    }
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "ReceveMeassge", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void StartDurations()
        {
            try
            {
                _acwCutdownTimer.Stop();
                _acwCutdownTimer.Enabled = false;

                _freezeDurations.Stop();
                _freezeDurations.Enabled = false;
                _freezeStarTime = DateTime.Now;
                _freezeDurations.Enabled = true;
                _freezeDurations.Start();
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "StartDurations", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void StoptDurations()
        {
            try
            {
                _freezeDurations.Stop();
                _freezeDurations.Enabled = false;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "StoptDurations", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void AddOutgoingCallToCallLogs(string no)
        {
            try
            {
                lock (_callLogs)
                {
                    _callLogs.Add(_call.currentCallLogId,
                        new CallLog { Direction = 1, Durations = 0, PhoneNo = no, Time = DateTime.Now });
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "AddOutgoingCallToCallLogs", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void AddIncommingToCallLogs(string no)
        {
            try
            {
                lock (_callLogs)
                {
                    if (_callLogs.ContainsKey(_call.currentCallLogId))
                    {
                        _callLogs.Remove(_call.currentCallLogId);
                    }
                    _callLogs.Add(_call.currentCallLogId,
                        new CallLog { Direction = 0, Durations = 0, PhoneNo = no, Time = DateTime.Now });
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "AddIncommingToCallLogs", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void AddCallDurations()
        {
            try
            {
                if (_callLogs == null) return;
                CallLog log;
                lock (_callLogs)
                {
                    log = _callLogs[_call.currentCallLogId];
                }
                log.Durations = Math.Round(DateTime.Now.Subtract(log.Time).TotalSeconds, 2);
                lock (_callLogs)
                {
                    _callLogs.Remove(_call.currentCallLogId);
                    _callLogs.Add(_call.currentCallLogId, log);
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "AddCallDurations", exception,
                    Logger.LogLevel.Error);
            }
        }

        private int _missCallCount;

        private void AddMiscallToCallLogs()
        {
            try
            {
                _missCallCount++;
                if (_callLogs != null)
                {
                    var log = _callLogs[_call.currentCallLogId];
                    log.Direction = 2;
                    lock (_callLogs)
                    {
                        _callLogs.Remove(_call.currentCallLogId);
                        _callLogs.Add(_call.currentCallLogId, log);
                    }
                }
                if (_missCallCount > 3)
                {
                    _agent.AgentCurrentState.OnOffline(_agent, "Exceed Maximum Miscall Count");
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "AddMiscallToCallLogs", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void InProgressState()
        {
            try
            {
                Invoke(new MethodInvoker(delegate
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
                logger.LogMessage(Logger.LogAppender.DuoDefault, "InProgressState", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void InitAdioWizItems()
        {
            _comboBoxMicrophones = new ComboBox();
            _comboBoxSpeakers = new ComboBox();
            //
            // ComboBoxMicrophones
            //
            _comboBoxMicrophones.DropDownStyle = ComboBoxStyle.DropDownList;
            _comboBoxMicrophones.FormattingEnabled = true;
            _comboBoxMicrophones.Location = new Point(90, 42);
            _comboBoxMicrophones.Name = "ComboBoxMicrophones";
            _comboBoxMicrophones.Size = new Size(308, 23);
            _comboBoxMicrophones.TabIndex = 49;

            //
            // ComboBoxSpeakers
            //
            _comboBoxSpeakers.DropDownStyle = ComboBoxStyle.DropDownList;
            _comboBoxSpeakers.FormattingEnabled = true;
            _comboBoxSpeakers.Location = new Point(90, 16);
            _comboBoxSpeakers.Name = "ComboBoxSpeakers";
            _comboBoxSpeakers.Size = new Size(308, 23);
            _comboBoxSpeakers.TabIndex = 48;

            _comboBoxMicrophones.SelectedIndexChanged += (s, e) =>
            {
                try
                {
                    _phoneController.setAudioDeviceId(_comboBoxMicrophones.SelectedIndex, _comboBoxSpeakers.SelectedIndex);
                    _selectedMic = _comboBoxMicrophones.SelectedItem.ToString();
                }
                catch (Exception exception)
                {
                    logger.LogMessage(Logger.LogAppender.DuoDefault, "ComboBoxMicrophones.SelectedIndexChanged",
                        exception, Logger.LogLevel.Error);
                }
            };

            _comboBoxSpeakers.SelectedIndexChanged += (s, e) =>
            {
                try
                {
                    _phoneController.setAudioDeviceId(_comboBoxMicrophones.SelectedIndex, _comboBoxSpeakers.SelectedIndex);
                    _selectedSpeaker = _comboBoxSpeakers.SelectedItem.ToString();
                }
                catch (Exception exception)
                {
                    logger.LogMessage(Logger.LogAppender.DuoDefault, "ComboBoxSpeakers.SelectedIndexChanged",
                        exception, Logger.LogLevel.Error);
                }
            };
        }

        private void DisableRingtone()
        {
            _playRingtone = false;
            mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Ring tone Off", ToolTipIcon.Info);
        }

        private void EnableRingtone()
        {
            try
            {
                _filePath = appSetting["RingToneFilePath"];
                var ringtone = appSetting["PlayRingtone"].ToLower();
                if (!File.Exists(_filePath))
                {
                    logger.LogMessage(Logger.LogAppender.DuoDefault,
                        string.Format("PlayRingTone> Cannot Find File {0}", _filePath), Logger.LogLevel.Error);
                    if (ringtone.Equals("1") || ringtone.Equals("true"))
                    {
                        _filePath = string.Format(@"{0}\{1}", _startupPath, "Ringtone.wav");
                        if (File.Exists(_filePath))
                        {
                            logger.LogMessage(Logger.LogAppender.DuoDefault,
                                "PlayRingTone> Play with default ringin tone", Logger.LogLevel.Info);
                            _playRingtone = true;
                            mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Ring tone On", ToolTipIcon.Info);
                            return;
                        }
                    }
                }

                _playRingtone = false;
                mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Ring tone Off-No Audio File", ToolTipIcon.Info);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "EnableRingtone", exception,
                    Logger.LogLevel.Error);
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
                logger.LogMessage(Logger.LogAppender.DuoLogger1,
                    string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}", userName, authName,
                        password, localPort), Logger.LogLevel.Info);

                logger.LogMessage(Logger.LogAppender.DuoLogger1,
                    string.Format(
                        "userName : {0}, authName : {1}, password : {2}, localPort : {3}.............................step 1 : pass.",
                        userName, authName, password, localPort), Logger.LogLevel.Info);
                int errorCode = 0;

                _phoneController = new PortSIPLib(0, 0, this);
                _phoneController.createCallbackHandlers();
                var rt = _phoneController.initialize(TransportType.TransportUdp,
                    PortsipLogLevel.PortsipLogNone,
                    _startupPath,
                    1,
                    "DuoSoftPhone",
                    false,
                    false);
                if (rt != 0)
                {
                    ResetCallback(userName, authName, password, localPort, errorCode);
                    return;
                }

                InitAdioWizItems();
                var isLoaded = LoadDevices();
                toolStripStatusDuoPhone.Invoke(((MethodInvoker)(() =>
                {
                    toolStripStatusDuoPhone.Appearance.BackColor = (isLoaded)? Color.Transparent : Color.DarkRed;
                })));

                if (!isLoaded)
                {
                    ResetCallback(userName, authName, password, localPort, errorCode);
                    logger.LogMessage(Logger.LogAppender.DuoDefault, "No audio device", Logger.LogLevel.Error);
                    mynotifyicon.ShowBalloonTip(5, "Duo Soft Phone", "No audio device to bind.", ToolTipIcon.Error);
                    
                    return;
                }
                _phoneController.setAudioDeviceId(0, 0);
                
                _phoneController.setAudioCodecParameter(AudiocodecType.AudiocodecAmrwb,
                    "mode-set=0; octet-align=0; robust-sorting=0");
                logger.LogMessage(Logger.LogAppender.DuoLogger1,
                    string.Format(
                        "userName : {0}, authName : {1}, password : {2}, localPort : {3}, sipServer : {4}, sipServerPort : {5}.............................step 2 : pass.",
                        userName, authName, password, localPort, sipServer, sipServerPort), Logger.LogLevel.Info);

                var outboundServer = string.Empty;
                var outboundServerPort = 0;
                var userDomain = string.Empty;

                var rtUserInfo = _phoneController.setUser(userName, displayName, authName, password, _agent.IP,
                    Convert.ToInt16(localPort), userDomain, sipServer, Convert.ToInt16(sipServerPort), string.Empty, 5060,
                    outboundServer, outboundServerPort);

                logger.LogMessage(Logger.LogAppender.DuoLogger1,
                    string.Format(
                        "userName : {0}, authName : {1}, password : {2}, localPort : {3}.............................step 4 : Pass.",
                        userName, authName, password, localPort), Logger.LogLevel.Info);
                if (rtUserInfo != 0)
                {
                    if (!isReInit)
                    {
                        _phoneController.unInitialize();
                        _phoneController.releaseCallbackHandlers();
                        logger.LogMessage(Logger.LogAppender.DuoLogger1,
                            string.Format(
                                "userName : {0}, authName : {1}, password : {2}, localPort : {3}.............................SetUserInfo Failed. errorCode : {4}",
                                userName, authName, password, localPort, errorCode), Logger.LogLevel.Info);
                        InitializeError("Fail to Set User Information's.", -997);
                        return;
                    }
                }

                _phoneController.setSrtpPolicy(SrtpPolicy.None);
                logger.LogMessage(Logger.LogAppender.DuoLogger1,
                    string.Format(
                        "userName : {0}, authName : {1}, password : {2}, localPort : {3}..............................step 5 : Pass.",
                        userName, authName, password, localPort), Logger.LogLevel.Info);
                string licenseKey = LicenseKeyHandler.GetLicenseKey("DuoS123");
                rt = _phoneController.setLicenseKey(licenseKey);

                if (rt == PortSipErrors.ECoreTrialVersionLicenseKey)
                {
                    MessageBox.Show(Resources.FormDialPad_InitializePhone_PortSIP_Errors_ECoreTrialVersionLicenseKey);
                    Text = Text + @" [built base on evaluation key]";
                    logger.LogMessage(Logger.LogAppender.DuoLogger1,
                        Resources.FormDialPad_InitializePhone_PortSIP_Errors_ECoreTrialVersionLicenseKey,
                        Logger.LogLevel.Info);
                }
                else if (rt == PortSipErrors.ECoreWrongLicenseKey)
                {
                    MessageBox.Show(
                        Resources.FormDialPad_InitializePhone_PortSIP_Errors_ECoreWrongLicenseKey);
                    Text = Text + @" [wrong license key was detected]";
                    logger.LogMessage(Logger.LogAppender.DuoLogger1,
                        Resources.FormDialPad_InitializePhone_PortSIP_Errors_ECoreWrongLicenseKey,
                        Logger.LogLevel.Info);
                }

                var rtRegister = _phoneController.registerServer(120, 3);

                logger.LogMessage(Logger.LogAppender.DuoLogger1,
                    string.Format(
                        "userName : {0}, authName : {1}, password : {2}, localPort : {3}..............................step 6 : Pass.",
                        userName, authName, password, localPort), Logger.LogLevel.Info);
                if (rtRegister != 0)
                {
                    _phoneController.unInitialize();
                    _phoneController.releaseCallbackHandlers();
                    logger.LogMessage(Logger.LogAppender.DuoLogger1,
                        string.Format(
                            "userName : {0}, authName : {1}, password : {2}, localPort : {3}..............................Registration Failed. errorCode : {4}",
                            userName, authName, password, localPort, errorCode), Logger.LogLevel.Info);
                    InitializeError("Fail to Register With SIP Server.", -996);
                    return;
                }

                _phoneController.addSupportedMimeType("INFO", "text", "plain");
                InitAutioCodecs();

                //phoneController.setSpeakerVolume(26214);//40% volume
                //phoneController.setMicVolume(52428);//80%
                logger.LogMessage(Logger.LogAppender.DuoLogger1,
                    string.Format(
                        "userName : {0}, authName : {1}, password : {2}, localPort : {3}..............................step 7 : Pass.",
                        userName, authName, password, localPort), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "InitializePhone", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void ResetCallback(string userName, string authName, string password, string localPort, int errorCode)
        {
            _phoneController.releaseCallbackHandlers();
            logger.LogMessage(Logger.LogAppender.DuoLogger1,
                string.Format(
                    "userName : {0}, authName : {1}, password : {2}, localPort : {3}.............................Phone Initialization failed. errorCode : {4}",
                    userName, authName, password, localPort, errorCode), Logger.LogLevel.Info);
            InitializeError("Phone Initialization failed.", -998);
            
        }

        private void UninitializePhone()
        {
            try
            {
                try
                {
                    _phoneController.hangUp(_agent.PortsipSessionId);
                    _phoneController.rejectCall(_agent.PortsipSessionId, 486);
                    _phoneController.unRegisterServer();
                    _phoneController.unInitialize();
                    _phoneController.releaseCallbackHandlers();
                }
                catch (Exception exception)
                {
                    logger.LogMessage(Logger.LogAppender.DuoDefault, "UninitializePhone", exception,
                        Logger.LogLevel.Error);
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "UninitializePhone", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void InitAutioCodecs()
        {
            _phoneController.addAudioCodec(AudiocodecType.AudiocodecG729);
            _phoneController.addAudioCodec(AudiocodecType.AudiocodecPcma);
            _phoneController.addAudioCodec(AudiocodecType.AudiocodecPcmu);

            _phoneController.addAudioCodec(AudiocodecType.AudiocodecDtmf); // For RTP event - DTMF (RFC2833)
        }

        private void FreezeAcwTime()
        {
            try
            {
                _acwCutdownTimer.Stop();
                _acwCutdownTimer.Enabled = false;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "StopACWTime", exception,
                    Logger.LogLevel.Error);
            }
        }

        private bool LoadDevices()
        {
            var num=0;
            try
            {
                _comboBoxSpeakers.Items.Clear();
                _comboBoxMicrophones.Items.Clear();
                num = _phoneController.getNumOfPlayoutDevices();
                for (var i = 0; i < num; ++i)
                {
                    var deviceName = new StringBuilder {Length = 256};

                    if (_phoneController.getPlayoutDeviceName(i, deviceName, 256) == 0)
                    {
                        _comboBoxSpeakers.Items.Add(deviceName.ToString());
                    }
                }

                if (_comboBoxSpeakers.Items.Count > 0)
                    _comboBoxSpeakers.SelectedIndex = _selectedSpeaker.Equals(string.Empty)
                        ? 0
                        : _comboBoxSpeakers.FindString(_selectedSpeaker);

                num = _phoneController.getNumOfRecordingDevices();
                for (int i = 0; i < num; ++i)
                {
                    var deviceName = new StringBuilder { Length = 256 };

                    if (_phoneController.getRecordingDeviceName(i, deviceName, 256) == 0)
                    {
                        _comboBoxMicrophones.Items.Add(deviceName.ToString());
                    }
                }

                if (_comboBoxMicrophones.Items.Count > 0)
                    _comboBoxMicrophones.SelectedIndex = _selectedMic.Equals(string.Empty)
                        ? 0
                        : _comboBoxMicrophones.FindString(_selectedMic);

                var spVol = _phoneController.getSpeakerVolume();
               var micVol = _phoneController.getMicVolume();
                
               logger.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Spvol : {0}, mic : {1}", spVol, micVol), Logger.LogLevel.Info);
               
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "loadDevices", exception,Logger.LogLevel.Error);
                
            }
            return (num != 0);
        }

        #endregion Private Methods

        #region keypad events

        private void googleSearch_Click(object sender, EventArgs e)
        {
            liveSearch.Checked = !googleSearch.Checked;
        }

        private void liveSearch_Click(object sender, EventArgs e)
        {
            googleSearch.Checked = !liveSearch.Checked;
        }

        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode != Keys.Enter) return;
                if (googleSearch.Checked)
                    _curBrowser.Navigate("http://google.com/search?q=" + searchTextBox.Text);
                else
                    _curBrowser.Navigate("http://search.live.com/results.aspx?q=" + searchTextBox.Text);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "searchTextBox_KeyDown", exception,Logger.LogLevel.Error);
            }
        }

        private void buttonAnswer_Click(object sender, EventArgs e)
        {
            MakeCall(_textBoxNumber.Text);
        }

        private void SendMsgToTappi(string no)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Send no : {0} To Tappi", no),
                    Logger.LogLevel.Info);
                UdpClient.Instance.SendToChannel(no);
                IXDBroadcast broadcast = XDBroadcast.CreateBroadcast(XDTransportMode.IOStream, false);
                broadcast.SendToChannel("CallerID", no);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "SendMsgToTappi", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void TransferCall(string transferNo, TransferType transferType)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault,string.Format("transferCall_Click-> Session Id : {0} , Status : {1}", _call.CallSessionId,_call.CallCurrentState),Logger.LogLevel.Info);
                if (_call.CallCurrentState.GetType() != typeof (CallHoldState)) return;
                if (string.IsNullOrEmpty(transferNo)) return;
                var res = _phoneController.sendInfo(_call.portSipSessionId, "text", "plain",string.Format("{0}:{1}", transferType.ToString().ToLower(), transferNo));
                if (res == 0)
                    _call.CallCurrentState.OnTransferReq(_call, CallAction.CallTransferRequested);
                txtStatus.Text = Environment.NewLine + (res != 0 ? "Transfer Failed." : "Transferring Call...");
                if (!_source.Contains(transferNo))
                    _source.Add(transferNo);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
            }
        }

        private void MakeCall(string no)
        {
            try
            {
                if (_agent.AgentCurrentState.GetType() == typeof(AgentIdle) && _agent.AgentMode==AgentMode.Outbound)
                {
                    if (!string.IsNullOrEmpty(no))
                    {
                        _agent.AgentCurrentState.OnMakeCall(_agent);
                        InAgentBusy();
                        _call = new Call(no, this) { portSipSessionId = _phoneController.call(no, true, false) };
                        if (_call.portSipSessionId < 0)
                        {
                            logger.LogMessage(Logger.LogAppender.DuoLogger4, "MakeCall-Fail",
                                Logger.LogLevel.Error);
                            mynotifyicon.ShowBalloonTip(5, "Duo Soft Phone", "Fail to Make Call", ToolTipIcon.Error);

                            _agent.AgentCurrentState.OnFailMakeCall(_agent);
                            _call.CallCurrentState = new CallIdleState();
                            return;
                        }
                        _agent.PortsipSessionId = _call.portSipSessionId;
                        _call.SetDialInfo(_call.portSipSessionId, Guid.NewGuid());
                        AddOutgoingCallToCallLogs(no);
                        SelectDuoTab();
                        new Thread(() => SendMsgToTappi(no)).Start();
                    }
                    else
                    {
                        mynotifyicon.ShowBalloonTip(5, "Duo Soft Phone", "Invalid Phone Number.", ToolTipIcon.Error);
                    }
                }
                else if (_call.CallCurrentState.GetType() == typeof(CallRingingState) ||
                         _call.CallCurrentState.GetType() == typeof(CallIncommingState))
                {
                    _phoneController.answerCall(_call.portSipSessionId, false);
                }
                else
                {
                    logger.LogMessage(Logger.LogAppender.DuoLogger4,
                        string.Format("MakeCall-Fail. AgentCurrentState: {0}, CallCurrentState: {1}",
                            _agent.AgentCurrentState, _call.CallCurrentState), Logger.LogLevel.Error);
                    mynotifyicon.ShowBalloonTip(5, "Duo Soft Phone", "Fail to Make Call", ToolTipIcon.Error);
                }
                Invoke(new MethodInvoker(delegate
                {
                    if (!_source.Contains(no))
                        _source.Add(no);
                    _source.Remove(string.Empty);
                }));
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "MakeCall", exception, Logger.LogLevel.Error);
            }
        }

        private void buttonReject_Click(object sender, EventArgs e)
        {
            EndCall();
            DisableAlert();
            _isCallAnswerd = false;
        }

        private void EndCall()
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault,
                    string.Format("End call. Agent Status : [{0}], Call Status : [{1}]", _agent.AgentCurrentState,
                        _call.CallCurrentState), Logger.LogLevel.Info);
                StopRingTone();
                var status = Resources.FormDialPad_onInviteClosed_Call_Ended;
                if (_call.CallCurrentState.GetType() == typeof(CallRingingState) ||
                    _call.CallCurrentState.GetType() == typeof(CallTryingState))
                {
                    if (_agent.CallDirection == CallDirection.Outgoing)
                    {
                        _phoneController.hangUp(_call.portSipSessionId);
                    }
                    else
                    {
                        _phoneController.rejectCall(_call.portSipSessionId, 486);
                        status = "Call Rejected";
                    }
                }
                else
                    _phoneController.hangUp(_call.portSipSessionId);

                logger.LogMessage(Logger.LogAppender.DuoDefault,
                    string.Format("End call. Agent Status : [{0}], Call Status : [{1}] , status : [{2}]",
                        _agent.AgentCurrentState, _call.CallCurrentState, status), Logger.LogLevel.Info);
                _call.CallCurrentState.OnDisconnected(_call);
                _agent.AgentCurrentState.OnEndCall(_agent, true);
                Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = Color.DarkGreen;
                    txtStatus.Text = string.IsNullOrEmpty(status)?"End Call" : status;
                })));

                AddCallDurations();
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "EndCall", exception, Logger.LogLevel.Error);
            }
        }

        private void buttonConference_Click(object sender, EventArgs e)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault,
                    string.Format("Conference_Click-> Session Id : {0} , Status : {1}", _agent.CallSessionId,
                        _call.CallCurrentState), Logger.LogLevel.Info);
                if (_call.CallCurrentState.GetType() != typeof (CallAgentClintConnectedState)) return;
                var res = _phoneController.sendInfo(_agent.PortsipSessionId, "text", "plain", "conference");
                if (res == 0)
                    _call.CallCurrentState.OnConference(_call);
                txtStatus.ForeColor = Color.DarkGreen;
                txtStatus.Text = (res != 0 ? "Conference Failed." : "Conference Call...");
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
                
            }
        }

        private void buttonEtl_Click(object sender, EventArgs e)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault,
                    string.Format("Etl_Click-> Session Id : {0} , Status : {1}", _agent.CallSessionId,
                        _call.CallCurrentState), Logger.LogLevel.Info);
                if (_call.CallCurrentState.GetType() == typeof(CallConferenceState) ||
                    _call.CallCurrentState.GetType() == typeof(CallAgentClintConnectedState) ||
                    _call.CallCurrentState.GetType() == typeof(CallAgentSupConnectedState))
                {
                    var res = _phoneController.sendInfo(_agent.PortsipSessionId, "text", "plain", "etl");
                    if (res == 0)
                        _call.CallCurrentState.OnEndLinkLine(_call, CallAction.EtlRequested);
                    txtStatus.ForeColor = Color.DarkGreen;
                    txtStatus.Text = (res != 0 ? "ETL Failed." : "ETL Call...");
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
            }
        }

        private void buttonswapCall_Click(object sender, EventArgs e)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault,
                    string.Format("swapCall_Click-> Session Id : {0} , Status : {1}", _call.CallSessionId,
                        _call.CallCurrentState), Logger.LogLevel.Info);
                if (_call.CallCurrentState.GetType() == typeof(CallAgentClintConnectedState) ||
                    _call.CallCurrentState.GetType() == typeof(CallAgentSupConnectedState))
                {
                    var res = _phoneController.sendInfo(_call.portSipSessionId, "text", "plain", "swap");
                    if (res == 0)
                        _call.CallCurrentState.OnSwapReq(_call, CallAction.CallSwapRequested);
                    txtStatus.Text = Environment.NewLine + (res != 0 ? "Swap Failed." : "Swap Call...");
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
            }
        }

        private void buttontransferCall_Click(object sender, EventArgs e)
        {
            TransferCall(_textBoxNumber.Text.Trim(), TransferType.Transfer);
        }

        private void buttontransferIvr_Click(object sender, EventArgs e)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault,
                    string.Format("transferIvr_Click-> Session Id : {0} , Status : {1}", _agent.CallSessionId,
                        _call.CallCurrentState), Logger.LogLevel.Info);
                TransferCall(_textBoxNumber.Text.Trim(), TransferType.Ivrtransfer);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_1_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "1";
                if (_call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDtmf(1);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_2_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "2";
                if (_call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDtmf(2);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_3_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "3";
                if (_call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDtmf(3);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_4_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "4";
                if (_call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDtmf(4);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_5_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "5";
                if (_call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDtmf(5);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_6_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "6";
                if (_call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDtmf(6);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_7_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "7";
                if (_call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDtmf(7);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_8_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "8";
                if (_call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDtmf(8);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_9_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "9";
                if (_call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDtmf(9);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_star_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "*";
                if (_call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDtmf(10);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_0_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "0";
                if (_call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDtmf(0);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
            }
        }

        private void button_key_hash_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentNumber += "#";
                if (_call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    SendDtmf(11);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
            }
        }

        private void SendDtmf(int digit)
        {
            try
            {
                if (_call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    _phoneController.sendDtmf(_agent.PortsipSessionId, DtmfMethod.DtmfRfc2833, Convert.ToInt16(digit),
                        160, true);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "SendDTMF", exception, Logger.LogLevel.Error);
            }
        }

        private void buttonHold_Click(object sender, EventArgs e)
        {
            logger.LogMessage(Logger.LogAppender.DuoDefault,
                string.Format("Hold_Click-> Session Id : {0} , Status : {1}", _call.CallSessionId, _call.CallCurrentState),
                Logger.LogLevel.Info);
            HoldUnholdCall();
        }

        private void HoldUnholdCall()
        {
            try
            {
                var status = "Hold Call";
                int res = -1;
                if (_call.CallCurrentState.GetType() == typeof(CallConnectedState))
                {
                    if (_agent.CallHandler.SipServiceCall.IsPBX)
                    {
                        res = _phoneController.hold(_call.portSipSessionId);
                        if (res == 0)
                        {
                            _call.CallCurrentState.OnHold(_call, CallAction.Hold);
                        }
                    }
                    else
                    {
                        _call.CallCurrentState.OnHold(_call, CallAction.HoldRequested);
                        res = _phoneController.sendInfo(_call.portSipSessionId, "text", "plain", "hold");
                        //, "hold".Length);
                    }
                }
                else if (_call.CallCurrentState.GetType() == typeof(CallHoldState))
                {
                    if (_agent.CallHandler.SipServiceCall.IsPBX)
                    {
                        res = _phoneController.unHold(_call.portSipSessionId);
                        if (res == 0)
                        {
                            _call.CallCurrentState.OnUnHold(_call, CallAction.UnHold);
                        }
                    }
                    else
                    {
                        _call.CallCurrentState.OnUnHold(_call, CallAction.UnHoldRequested);
                        res = _phoneController.sendInfo(_call.portSipSessionId, "text", "plain", "unhold");
                    }
                }

                txtStatus.Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = Color.DarkGreen;
                    txtStatus.Text = (string.Format("{0} {1}", status, (res != 0 ? "Failed" : string.Empty)));
                })));
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "HoldUnholdCall", exception,
                    Logger.LogLevel.Error);
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
                        var index = _textBoxNumber.SelectionStart - 1;
                        if (index >= 0)
                        {
                            CurrentNumber = CurrentNumber.Remove(index, 1);
                            _textBoxNumber.SelectionStart = index;
                            _textBoxNumber.Focus();
                        }
                        // CurrentNumber.Substring(0, CurrentNumber.Length - 1);
                    }
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
            }
        }

        #endregion keypad events

        #region FormEvents

        #region EDIT

        /*EDIT*/

        //cut
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_curBrowser.Document != null) _curBrowser.Document.ExecCommand("Cut", false, null);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "cutToolStripMenuItem_Click", exception, Logger.LogLevel.Error);
            }
        }

        //copy
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_curBrowser.Document != null) _curBrowser.Document.ExecCommand("Copy", false, null);
            }
            catch (Exception exception)
            {
              logger.LogMessage(Logger.LogAppender.DuoDefault, "copyToolStripMenuItem_Click", exception, Logger.LogLevel.Error);
            }
        }

        //paste
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_curBrowser.Document != null) _curBrowser.Document.ExecCommand("Paste", false, null);
            }
            catch (Exception exception)
            {
               logger.LogMessage(Logger.LogAppender.DuoDefault, "pasteToolStripMenuItem_Click", exception, Logger.LogLevel.Error);
            }
        }

        //select all
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_curBrowser.Document != null) _curBrowser.Document.ExecCommand("SelectAll", true, null);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "selectAllToolStripMenuItem_Click", exception, Logger.LogLevel.Error);
            }
        }

        #endregion EDIT

        private void GridIvrList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                panelIvr.Visible = false;
                if (GridIvrList.RowCount > 0)
                    _textBoxNumber.Text = GridIvrList["IvrExtension", e.RowIndex].Value.ToString();
                // GridAgentList.Rows[e.RowIndex].Cells["Extention"].Value.ToString();
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "GridAgentList_CellDoubleClick", exception,Logger.LogLevel.Error);
            }
        }

        private void btnLoadIvrList_Click(object sender, EventArgs e)
        {
            LoadIvr();
        }

        private void btnAgentListClose_Click(object sender, EventArgs e)
        {
            panelAgentList.Visible = false;
        }

        private void btnIvrListClose_Click(object sender, EventArgs e)
        {
            panelIvr.Visible = false;
        }

        private void btnLoadAgentList_Click(object sender, EventArgs e)
        {
            try
            {
                _resMonitoringHandler.GetOnlineIdleAgentList(_agent.Auth.SecurityToken, _agent.Auth.guUserId.ToString());
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "btnLoadAgentList_Click", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void ivrListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                panelIvr.Visible = !panelIvr.Visible;
                panelAgentList.Visible = false;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "ivrListToolStripMenuItem_Click", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void agentListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                panelAgentList.Visible = !panelAgentList.Visible;
                panelIvr.Visible = false;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "agentListToolStripMenuItem_Click", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void GridAgentList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                panelAgentList.Visible = false;
                if (GridAgentList.RowCount > 0)
                    _textBoxNumber.Text = GridAgentList["Extention", e.RowIndex].Value.ToString();
                // GridAgentList.Rows[e.RowIndex].Cells["Extention"].Value.ToString();
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "GridAgentList_CellDoubleClick", exception,
                    Logger.LogLevel.Error);
            }
        }

        public FormDialPad(UserAuth auth, string sessionId, Dictionary<string, object> sip, string ip, string passWord)
        {
            try
            {
                InitializeComponent();
                if (auth==null)
                    return;
                // Create and initialize the text box. //64, 64, 64
                _textBoxNumber = new TextBox
                {
                    BackColor = Color.Black,
                    // Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))),((int)(((byte)(64))))),
                    BorderStyle = BorderStyle.Fixed3D,
                    Font = new Font("Calibri", 15F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0))),
                    ForeColor = Color.White,
                    Location = new Point(2, 26),
                    Name = "textBoxNumber",
                    Size = new Size(192, 30), //194, 22
                    TabIndex = 1,
                    AutoCompleteCustomSource = _source,
                    AutoCompleteMode = AutoCompleteMode.Suggest,
                    AutoCompleteSource = AutoCompleteSource.CustomSource,
                    Visible = true,
                    TextAlign = HorizontalAlignment.Center,
                    ContextMenuStrip = phoner8ClickMenu,
                };

                PanelPhoneNoBox.ClientArea.Controls.Add(_textBoxNumber);
                _textBoxNumber.AutoCompleteCustomSource = _source;
                _textBoxNumber.AutoCompleteMode = AutoCompleteMode.Suggest;
                _textBoxNumber.AutoCompleteSource = AutoCompleteSource.CustomSource;

                _textBoxNumber.TextChanged += (s, e1) =>
                {
                    CurrentNumber = _textBoxNumber.Text;
                };
                _textBoxNumber.Focus();

                _textBoxNumber.KeyDown += (k, d) =>
                {
                    if (d.KeyCode != Keys.Enter) return;
                    if (_agent.AgentCurrentState.GetType() == typeof(AgentIdle))
                        MakeCall(_textBoxNumber.Text);
                };
                logger = Logger.Instance;
                _startupPath = Application.StartupPath;
                _favFile = _startupPath + "\\favs.xml";
                _passWord = passWord;
                appSetting = System.Configuration.ConfigurationSettings.AppSettings;
                _filePath = appSetting["RingToneFilePath"];
                var ringtone = appSetting["PlayRingtone"].ToLower();
                if (!File.Exists(_filePath))
                {
                    logger.LogMessage(Logger.LogAppender.DuoDefault,
                        string.Format("PlayRingTone> Cannot Find File {0}", _filePath), Logger.LogLevel.Error);
                    if (ringtone.Equals("1") || ringtone.Equals("true"))
                    {
                        _filePath = string.Format(@"{0}\{1}", _startupPath, "Ringtone.wav");
                        if (File.Exists(_filePath))
                            logger.LogMessage(Logger.LogAppender.DuoDefault,
                                "PlayRingTone> Play with default ring tone", Logger.LogLevel.Info);
                    }
                }
                _playRingtone = File.Exists(_filePath) && (ringtone.Equals("1") || ringtone.Equals("true"));

                _ringInfilePath = appSetting["RingInToneFilePath"];
                _playRingInToneMenually = File.Exists(_ringInfilePath);
                if (!_playRingInToneMenually)
                {
                    logger.LogMessage(Logger.LogAppender.DuoDefault,
                        string.Format("PlayRingINTone> Cannot Find File {0}", _ringInfilePath), Logger.LogLevel.Error);
                    _ringInfilePath = string.Format(@"{0}\{1}", _startupPath, "RingIntone.wav");
                    if (File.Exists(_ringInfilePath))
                    {
                        _playRingInToneMenually = true;
                        logger.LogMessage(Logger.LogAppender.DuoDefault,
                            "PlayRingINTone> Play with default ringIn tone", Logger.LogLevel.Info);
                    }
                }

                _agent = new Agent(auth, sip, ip, sessionId, this) { AgentReqMode = AgentMode.Outbound };
                InternetSetCookie(appSetting["DuoUrl"], "SecurityTokenCookie", auth.SecurityToken);

                isGreetingEnable = appSetting["Greeting"].Equals("1");
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "FormDialPad", exception,
                    Logger.LogLevel.Error);
                MessageBox.Show(Resources.FormDialPad_FormDialPad_Load_Critical_Error__Please_Contact_your_System_Administrator_, Resources.FormDialPad_FormDialPad_Load_Duo_Dialer,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void FormDialPad_Load(object sender, EventArgs e)
        {
            try
            {
                
                // DuoWebBrowser.Refresh();
               

                //
                onToolStripMenuItem.Checked = _playRingtone;
                offToolStripMenuItem.Checked = !_playRingtone;

                _wavPlayer = new SoundPlayer
                {
                    SoundLocation = _filePath,
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

                _listner = XDListener.CreateListener(XDTransportMode.IOStream, false);

                _listner.RegisterChannel("Command");

                _listner.MessageReceived += (o, e1) =>
                {
                    try
                    {
                        if (e1.DataGram.Channel != "Command") return;
                        var cli = e1.DataGram.Message;
                        txtStatus.Invoke(new MethodInvoker(delegate
                        {
                            logger.LogMessage(Logger.LogAppender.DuoDefault,
                                string.Format("Recive no : {0} frm Tappi", cli), Logger.LogLevel.Info);
                            txtStatus.Text = Resources.FormDialPad_FormDialPad_Load_Tappi_System_try_to_Dial_Call_;
                            mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", Resources.FormDialPad_FormDialPad_Load_Tappi_System_try_to_Dial_Call_,
                                ToolTipIcon.Info);
                            if (_agent.AgentCurrentState.GetType() == typeof(AgentIdle))
                            {
                                _textBoxNumber.Text = cli;
                                MakeCall(cli);
                            }
                            else
                            {
                                mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone",
                                    "Fail to Make calls. Agent Invalid State.", ToolTipIcon.Info);
                            }
                        }));
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoDefault, "listner.MessageReceived", exception,
                            Logger.LogLevel.Error);
                    }
                };

                _callHandler = new CallHandler(_agent.Auth.SecurityToken, _agent.Auth.TenantID, _agent.Auth.CompanyID);
                AutoAnswer.Checked = _callHandler.AutoAnswerByDefault();
                
                AutoAnswer.Enabled = (_callHandler.AutoAnswerByDefault() && _callHandler.AutoAnswerCanAgentEnable());
                _playRingInToneMenually = _callHandler.SipServiceCall.IsPlayRingtone;
                _isNotAllowToReject = _callHandler.SipServiceCall.IsAllowToReject;
                rejectCallToolStripMenuItem.Enabled = !_isNotAllowToReject;
                //new Thread(InitiateTimer).Start();
                OFFLINE.Visible = false;
                Text = string.Format("{0} : {1}", Text, _agent.Auth.UserName);

                txtStatus.ForeColor = Color.DarkMagenta;
                txtStatus.Text = Resources.FormDialPad_FormDialPad_Load_Initializing___;


                _showCallAlert = appSetting["ShowCallAlert"].Equals("1");
                _reginTime = Convert.ToInt16(appSetting["RingTime"]);

                #region Socket Server

                if (appSetting["SocketlistnerEnable"].ToLower().Equals("true") ||
                    appSetting["SocketlistnerEnable"].Equals("1"))
                {
                    var socketlistner = new AsynchronousSocketListener(_agent.IP,
                        Convert.ToInt16(appSetting["SocketlistnerPort"]), _agent.Auth.SecurityToken,
                        _agent.Auth.TenantID,
                        _agent.Auth.CompanyID);

                    socketlistner.OnRecive += (callFunction, no) =>
                    {
                        try
                        {
                            logger.LogMessage(Logger.LogAppender.DuoDefault,
                                string.Format("External Application send Commands. callFunction : {0}, Phone No : {1}",
                                    callFunction, no), Logger.LogLevel.Info);
                            switch (callFunction)
                            {
                                case CallFunction.MakeCall:
                                    MakeCall(no);
                                    break;

                                case CallFunction.EndCall:
                                    EndCall();
                                    break;

                                case CallFunction.HoldCall:
                                    HoldUnholdCall();
                                    break;

                                default:
                                    throw new ArgumentOutOfRangeException("callFunction");
                            }
                        }
                        catch (Exception exception)
                        {
                            logger.LogMessage(Logger.LogAppender.DuoDefault,
                                "FormDialPad_Load-OnSocketMessageRecive", exception, Logger.LogLevel.Error);
                        }
                    };
                }
                else
                {
                    logger.LogMessage(Logger.LogAppender.DuoDefault, "FormDialPad_Load-Socket-Disable",
                        Logger.LogLevel.Info);
                }

                #endregion Socket Server

                #region WebSocket Server

                if (appSetting["WebSocketlistnerEnable"].ToLower().Equals("true") ||
                    appSetting["WebSocketlistnerEnable"].Equals("1"))
                {
                    var webSocketlistner = new WebSocketServiceHost(Convert.ToInt16(appSetting["WebSocketlistnerPort"]), _agent.Auth.SecurityToken,
                        _agent.Auth.TenantID,
                        _agent.Auth.CompanyID);

                    webSocketlistner.OnRecive += (callFunction, no) =>
                    {
                        try
                        {
                            logger.LogMessage(Logger.LogAppender.DuoDefault,
                                string.Format(
                                    "[webSocketlistner]External Application send Commands. callFunction : {0}, Phone No : {1}",
                                    callFunction, no), Logger.LogLevel.Info);
                            switch (callFunction)
                            {
                                case CallFunction.MakeCall:
                                    MakeCall(no);
                                    break;

                                case CallFunction.EndCall:
                                    EndCall();
                                    break;

                                case CallFunction.HoldCall:
                                    HoldUnholdCall();
                                    break;

                                case CallFunction.TransferCall:
                                    {
                                        if (!string.IsNullOrEmpty(no))
                                        {
                                            if (_call.CallCurrentState.GetType() == typeof(CallHoldState))
                                            {
                                                HoldUnholdCall();
                                            }
                                            _textBoxNumber.Invoke(new MethodInvoker(delegate { _textBoxNumber.Text = no; }));
                                            TransferCall(no, TransferType.Transfer);
                                        }
                                    }
                                    break;

                                default:
                                    throw new ArgumentOutOfRangeException("callFunction");
                            }
                        }
                        catch (Exception exception)
                        {
                            logger.LogMessage(Logger.LogAppender.DuoDefault,
                                "[webSocketlistner]FormDialPad_Load-OnSocketMessageRecive", exception,
                                Logger.LogLevel.Error);
                        }
                    };
                }
                else
                {
                    logger.LogMessage(Logger.LogAppender.DuoDefault,
                        "FormDialPad_Load-[webSocketlistner]-Disable", Logger.LogLevel.Info);
                }

                #endregion WebSocket Server

                _callLogs = new Dictionary<Guid, CallLog>();

                #region ACW Timer

                _acwCutdownTimer = new Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);

                _acwTime =
                    (int)
                        TimeSpan.FromSeconds(
                            Convert.ToDouble(ProfileManagementHandler.GetAcwTime(_agent.Auth.SecurityToken)))
                            .TotalSeconds;
                _acwCotdown = _acwTime;

                _acwCutdownTimer.Elapsed += (s, e1) =>
                {
                    try
                    {
                        if (_acwCotdown <= 0)
                        {
                            Invoke(new MethodInvoker(() =>
                            {
                                txtStatus.Text = string.Empty;
                                btnFreez.Enabled = false;
                            }));
                            if (_acwCotdown < 0)
                            {
                                _acwCutdownTimer.Stop();
                                _acwCutdownTimer.Enabled = false;
                                logger.LogMessage(Logger.LogAppender.DuoLogger1,
                                    string.Format("End ACW time with default time.{0}", _agent.CallSessionId),
                                    Logger.LogLevel.Info);
                                _call.CallCurrentState.OnEndSession(_call);
                                _agent.AgentCurrentState.OnEndAcw(_agent, _agent.CallSessionId);
                                return;
                            }
                            _acwCotdown--;
                            return;
                        }
                        txtStatus.Invoke(
                            new MethodInvoker(() => { txtStatus.Text = string.Format("ACW : {0}", _acwCotdown); }));
                        InvokeBrowserScript("SIPEvents",
                            new object[] { "onACW", "***?***", string.Format("ACW : {0}", _acwCotdown), _acwCotdown.ToString() });
                        _acwCotdown--;
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoDefault, "acwCutdownTimer.Elapsed", exception,
                            Logger.LogLevel.Error);
                        logger.LogMessage(Logger.LogAppender.DuoLogger1, "acwCutdownTimer.Elapsed", exception,
                            Logger.LogLevel.Error);
                    }
                };

                #endregion ACW Timer

                _freezeDurations = new Timer(TimeSpan.FromSeconds(1).TotalSeconds);
                _freezeDurations.Elapsed += (s, e1) =>
                {
                    var ts = e1.SignalTime.Subtract(_freezeStarTime);
                    var elapsedTime = ts.Hours > 0
                        ? String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds)
                        : String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);

                    Invoke(new MethodInvoker(delegate
                    {
                        txtStatus.Text = elapsedTime;
                    }));
                };

                _callDurations = new Timer(TimeSpan.FromSeconds(1).TotalSeconds);
                _callDurations.Elapsed += (s, e1) =>
                {
                    var ts = e1.SignalTime.Subtract(_callStarTime);
                    var elapsedTime = ts.Hours > 0
                        ? String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds)
                        : String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);

                    Invoke(new MethodInvoker(delegate
                    {
                        txtStatus.Text = elapsedTime;
                    }));
                };

                //PanelbtnLog.Location = new Point(4, 286);
                //PanelPhoneFunc.Location = new Point(4, 228);
                //PanelDialPad.Location = new Point(4, 3);
                //gbBreakMode.Location = new Point(2, 62);
                ////OFFLINE.Location = new Point(2, 1);
                //PanelPhoneNoBox.Location = new Point(4, 2);

                InitializePhone(false);

                #region resourceCheckerHandler

                _resourceCheckerHandler = new ResourceCheckerHandler(_agent);
                _resourceCheckerHandler.OnResourceStatusMisMatch += () =>
                {
                    try
                    {
                        _agent.AgentCurrentState.OnError(_agent, "Status Mismatch", -9999,
                            "Unable to Communicate With Proxy Servers or Lost some of Data Packets. Please Contact Your System Administrator.");
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoDefault,
                            "resourceMonitoringHandler.OnResourceStatusMisMatch", exception, Logger.LogLevel.Error);
                    }
                };

                #endregion resourceCheckerHandler

                #region AgentList

                _resMonitoringHandler = new ResourceMonitoringHandler(_agent.Auth.SecurityToken,
                    _agent.Auth.guUserId.ToString());
                _resMonitoringHandler.OnGetOnlineIdleAgentListCompleted += a =>
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
                        logger.LogMessage(Logger.LogAppender.DuoDefault,
                            "resHandler.OnGetOnlineIdleAgentListCompleted", exception, Logger.LogLevel.Error);
                    }
                };

                #endregion AgentList

                #region IvrList

                _resMainConfigHandler = new MainConfigHandler();
                LoadIvr();

                #endregion IvrList

                #region DeviceMonitor

                try
                {
                    var device = new DuoDeviceMonitor.DeviceMonitor();
                    device.OnSoundDeviceAddEvent += d =>
                    {
                        try
                        {
                            logger.LogMessage(Logger.LogAppender.DuoDeviceMonitor,
                                string.Format(
                                    "OnSoundDeviceAddEvent - The audio device on the computer was reconnected. AgentCurrentState : {0}, CallCurrentState : {1}",
                                    _agent.AgentCurrentState, _call.CallCurrentState),
                                Logger.LogLevel.Error);
                            //logger.LogMessage(Logger.LogAppender.DuoDefault, string.Format("The audio device on the computer [{0}] used by [{1}], was either removed /reconnected during an ongoing call, therefore the   agent console will be disabled  till remedial action is taken. Agent Session ID [{2}], Call Session ID : {3},  Agent Status : {4}, Call Status : {5}", agent.AgentProfile.IpAddress, agent.AgentProfile.UserName, agent.AgentSessionId, call.CallSessionId, agent.AgentCurrentState.GetType().Name, call.CurrentState.GetType().Name), Logger.LogLevel.Error);
                            HandleDeviceEvent();
                            toolStripStatusDuoPhone.Invoke(((MethodInvoker)(() =>
                            {
                                toolStripStatusDuoPhone.Appearance.ForeColor = Color.DarkOrange;
                            })));
                            mynotifyicon.ShowBalloonTip(100, "Duo Soft Phone", "The audio device on the computer was reconnected.", ToolTipIcon.Warning);
                        }
                        catch (Exception exception)
                        {
                            logger.LogMessage(Logger.LogAppender.DuoDeviceMonitor, "OnSoundDeviceAddEvent",
                                exception,
                                Logger.LogLevel.Error);
                        }
                    };
                    device.OnSoundDeviceRemoveEvent += d =>
                    {
                        try
                        {
                            logger.LogMessage(Logger.LogAppender.DuoDeviceMonitor,
                                string.Format(
                                    "OnSoundDeviceRemoveEvent - The audio device on the computer was removed . AgentCurrentState : {0}, CallCurrentState : {1}",
                                    _agent.AgentCurrentState, _call.CallCurrentState),
                                Logger.LogLevel.Error);
                            HandleDeviceEvent();
                            toolStripStatusDuoPhone.Invoke(((MethodInvoker) (() =>
                            {
                                toolStripStatusDuoPhone.Appearance.ForeColor = Color.Crimson;
                            })));
                            mynotifyicon.ShowBalloonTip(100, "Duo Soft Phone", "The audio device on the computer was removed.", ToolTipIcon.Warning);
                        }
                        catch (Exception exception)
                        {
                            logger.LogMessage(Logger.LogAppender.DuoDeviceMonitor, "OnSoundDeviceRemoveEvent",
                                exception, Logger.LogLevel.Error);
                        }
                    };
                    device.EnableSoundDeviceArrivedWatcher();
                }
                catch (Exception exception)
                {
                    logger.LogMessage(Logger.LogAppender.DuoDefault, "FormDialPad_Load-DeviceMonitor",
                        exception, Logger.LogLevel.Error);
                }

                #endregion DeviceMonitor

                #region Browser

                AddressToolStripComboBox.Text = appSetting["DuoUrl"].Trim();
                DuoWebBrowser.Url = new Uri(appSetting["DuoUrl"].Trim());

                DuoWebBrowser.CanGoBackChanged += (s, er) =>
                {
                    try
                    {
                        BackToolStripButton.Enabled = _curBrowser.CanGoBack;
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception,
                            Logger.LogLevel.Error);
                    }
                };
                DuoWebBrowser.CanGoForwardChanged += (s, er) =>
                {
                    try
                    {
                        ForwardToolStripButton.Enabled = _curBrowser.CanGoForward;
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception,
                            Logger.LogLevel.Error);
                    }
                };

                DuoWebBrowser.DocumentCompleted += (s, er) =>
                {
                    try
                    {
                        statusStripBottom.Visible = false;
                        if (DuoWebBrowser.IsBusy) return;
                        using (var pic = (BrowserPreview) DuoWebBrowser.Tag)
                        {
                            using (var bitmap = new Bitmap(DuoWebBrowser.DisplayRectangle.Width, DuoWebBrowser.DisplayRectangle.Height))
                            {
                                using (var gfxdst = Graphics.FromImage(bitmap))
                                {
                                    gfxdst.CopyFromScreen(DuoWebBrowser.PointToScreen(new Point(DuoWebBrowser.DisplayRectangle.X,DuoWebBrowser.DisplayRectangle.Y)), new Point(0, 0), DuoWebBrowser.DisplayRectangle.Size);
                                    pic.Image = bitmap.GetThumbnailImage(Convert.ToInt32(PreviewSizeNumericUpDown.Value),Convert.ToInt32(PreviewSizeNumericUpDown.Value),GetThumbCallback, IntPtr.Zero);
                                    pic.SizeMode = PictureBoxSizeMode.AutoSize;
                                    pic.BorderStyle = BorderStyle.Fixed3D;
                                    gfxdst.Dispose();
                                }
                                //gfxsrc.Dispose()
                                bitmap.Dispose();
                            }
                        }
                        DuoWebBrowser.ContextMenuStrip = phoner8ClickMenu;
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception,Logger.LogLevel.Error);
                    }
                };
                DuoWebBrowser.DocumentTitleChanged += (s, er) =>
                {
                    try
                    {
                        if (DuoWebBrowser.DocumentTitle.Length > 0)
                        {
                            MainTabControl.SelectedTab.Text = DuoWebBrowser.DocumentTitle;
                            //maintabcontrol.SelectedTab.ImageIndex =
                        }
                        else
                        {
                            MainTabControl.SelectedTab.Text = AddressToolStripComboBox.Text.Length < 1? string.Format("Page {0}", MainTabControl.TabCount + 1): AddressToolStripComboBox.Text;
                        }
                        var pic = (BrowserPreview)DuoWebBrowser.Tag;
                        MainToolTip.SetToolTip(pic, MainTabControl.SelectedTab.Text);
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception,Logger.LogLevel.Error);
                    }
                };
                DuoWebBrowser.EncryptionLevelChanged += (s, er) =>
                {
                    try
                    {
                        switch (DuoWebBrowser.EncryptionLevel)
                        {
                            case WebBrowserEncryptionLevel.Bit128:
                            case WebBrowserEncryptionLevel.Bit40:
                            case WebBrowserEncryptionLevel.Bit56:
                            case WebBrowserEncryptionLevel.Fortezza:
                                SecureToolStripStatusLabel.Visible = true;
                                break;
                            default:
                                SecureToolStripStatusLabel.Visible = false;
                                break;
                        }
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception,Logger.LogLevel.Error);
                    }
                };
                DuoWebBrowser.ProgressChanged += (s, er) =>
                {
                    try
                    {
                        if (MainProgressBar.Value <= er.MaximumProgress)
                        {
                            MainProgressBar.Maximum = (int)er.MaximumProgress;
                            if (er.CurrentProgress >= MainProgressBar.Minimum)
                            {
                                MainProgressBar.Value = (int)er.CurrentProgress;
                            }
                            else
                            {
                                MainProgressBar.Value = 0;
                            }
                        }
                        else
                        {
                            MainProgressBar.Value = 0;
                            MainProgressBar.Maximum = (int)er.MaximumProgress;
                        }
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception,Logger.LogLevel.Error);
                    }
                };
                DuoWebBrowser.StatusTextChanged += (s, er) =>
                {
                    try
                    {
                        MainToolStripStatusLabel.Text = DuoWebBrowser.StatusText ?? "Done";
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception,Logger.LogLevel.Error);
                    }
                };
                DuoWebBrowser.Navigating += (s, er) =>
                {
                    try
                    {
                        statusStripBottom.Visible = true;
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception,Logger.LogLevel.Error);
                    }
                };
                DuoWebBrowser.Navigated += (s, er) =>
                {
                    try
                    {
                        if (DuoWebBrowser.Url != null)
                        {
                            AddressToolStripComboBox.Text = DuoWebBrowser.Url.ToString();
                        }
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception,Logger.LogLevel.Error);
                    }
                };

                try
                {
                    if (ShowPreviewToolStripButton.Checked)
                    {
                        if (frmLayoutsplitContainer.Panel1Collapsed)
                        {
                            frmLayoutsplitContainer.Panel1Collapsed = false;
                        }
                    }
                    else
                    {
                        if (!ShowFavoritesToolStripButton.Checked && !ShowPreviewToolStripButton.Checked)
                        {
                            frmLayoutsplitContainer.Panel1Collapsed = true;
                        }
                        else
                        {
                            frmLayoutsplitContainer.Panel1Collapsed = false;
                        }
                    }

                    AddressToolStripComboBox.Text = appSetting["DuoUrl"].Trim();
                    DuoWebBrowser.Navigate(appSetting["DuoUrl"].Trim());
                    AddressToolStripComboBox.Items.Add(appSetting["DuoUrl"].Trim());

                    DuoWebBrowser.Tag = Page1BrowserPreview;
                    Page1BrowserPreview.Tag = MainTabControl.SelectedTab;
                    Page1BrowserPreview.Selected = true;
                    
                    if (File.Exists(_favFile))
                    {
                        var xdoc = new XmlDocument();
                        xdoc.Load(_favFile);
                        var root = xdoc["Favorites"];
                        if (root != null)
                            foreach (XmlElement elm in root)
                            {
                                switch (elm.Name)
                                {
                                    case "Single":
                                    {
                                        var tn = new TreeNode
                                        {
                                            Name = elm.Attributes["name"].Value,
                                            Text = elm.Attributes["name"].Value,
                                            Tag = elm.Attributes["url"].Value
                                        };
                                        FavoritesTreeView.Nodes.Add(tn);
                                        break;
                                    }
                                    case "Group":
                                    {
                                        var mtn = new TreeNode { Name = elm.Attributes["name"].Value };
                                        mtn.Text = mtn.Name;
                                        mtn.Tag = "#GROUP#";
                                        foreach (var tn in from XmlElement ent in elm
                                            select new TreeNode
                                            {
                                                Name = ent.Attributes["name"].Value,
                                                Text = ent.Attributes["name"].Value,
                                                Tag = ent.Attributes["url"].Value
                                            })
                                        {
                                            mtn.Nodes.Add(tn);
                                        }
                                        FavoritesTreeView.Nodes.Add(mtn);
                                        break;
                                    }
                                }
                            }

                        FavoritesTreeView.CollapseAll();
                    }

                    //load url
                    var urlList = CommonDataHandler.GetUrls(_agent.Auth.SecurityToken, _agent.Auth.TenantID,_agent.Auth.CompanyID);
                    if (urlList.Any())
                    {
                        foreach (var url in urlList)
                        {
                            AddTabPage(url);
                        }
                    }
                    else
                    {
                        MainTabControl.TabPages.Remove(tabDefault);
                    }
                }
                catch (Exception exception)
                {
                    logger.LogMessage(Logger.LogAppender.DuoDefault, "MyTab", exception,
                        Logger.LogLevel.Error);
                }

                #endregion Browser

                #region callHandler
                try
                {
                    _callHandler.AnswerCall(string.Empty, string.Empty);
                    _callHandler.ConferenceCall(string.Empty, string.Empty);
                    _callHandler.DialCall(string.Empty);
                    _callHandler.EtlCall(string.Empty, string.Empty);
                    _callHandler.HoldCall(string.Empty, string.Empty);
                    _callHandler.EndCall(string.Empty, string.Empty);
                    _callHandler.SwapCallCall(string.Empty, string.Empty);
                    _callHandler.TransferCall(string.Empty, string.Empty);
                    _callHandler.UnHoldCall(string.Empty, string.Empty);
                    
                }
                catch (Exception exception)
                {
                    logger.LogMessage(Logger.LogAppender.DuoDefault, "callHandler", exception,
                    Logger.LogLevel.Error);
                }
                #endregion

                var section = (NameValueCollection)ConfigurationManager.GetSection("DuoBaseInfo");
                var tableName = section["waitToCallServerMsg"];
               waitToStartCallTIme = !string.IsNullOrEmpty(tableName) && (tableName.ToLower().Equals("true") || tableName.ToLower().Equals("1"));
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "FormDialPad_Load", exception,
                    Logger.LogLevel.Error);
                MessageBox.Show(Resources.FormDialPad_FormDialPad_Load_Critical_Error__Please_Contact_your_System_Administrator_, Resources.FormDialPad_FormDialPad_Load_Duo_Dialer,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        private void InvokeBrowserScript(string scriptName, object[] args)
        {
            try
            {
                new Thread(() => DuoWebBrowser.Invoke(new MethodInvoker(() =>
                {
                    try
                    {
                        if (DuoWebBrowser.Document != null)
                        {
                            logger.LogMessage(Logger.LogAppender.DuoDefault,string.Format("InvokeBrowserScript. scriptName: {0}, args: {1}", scriptName,string.Join("|", args)), Logger.LogLevel.Info);
                            DuoWebBrowser.Document.InvokeScript(scriptName, args);
                        }
                        else
                        {
                            logger.LogMessage(Logger.LogAppender.DuoDefault,string.Format("InvokeBrowserScript. DuoWebBrowser.Document != null. scriptName: {0}, args: {1}",scriptName, string.Join("|", args)), Logger.LogLevel.Error);
                        }
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoDefault, "InvokeBrowserScript-", exception,Logger.LogLevel.Error);
                    }
                }))).Start();
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "InvokeBrowserScript", exception,Logger.LogLevel.Error);
            }
        }

        private void FormDialPad_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                WindowState = FormWindowState.Minimized;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
            }
        }

        private void FormDialPad_Resize(object sender, EventArgs e)
        {
            try
            {
                setPhonePanelLocation();
                if (FormWindowState.Minimized == WindowState)
                {
                    // Hide();
                    mynotifyicon.Visible = true;
                    mynotifyicon.ShowBalloonTip(3000);
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
            }
        }

        private void FormDialPad_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                var result = MessageBox.Show(Resources.FormDialPad_FormDialPad_FormClosing_Are_you_sure_you_want_to_exit_the_application_, Resources.FormDialPad_FormDialPad_FormClosing_Confirmation,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    UninitializePhone();
                    _agent.AgentCurrentState.OnLogOff(_agent);

                    #region Browser

                    var xdoc = new XmlDocument();
                    var root = xdoc.CreateElement("Favorites");
                    xdoc.AppendChild(root);
                    foreach (TreeNode tn in FavoritesTreeView.Nodes)
                    {
                        if (tn.Tag.ToString() == "#GROUP#")
                        {
                            var ge = xdoc.CreateElement("Group");
                            var att = xdoc.CreateAttribute("name");

                            att.Value = tn.Text;
                            ge.Attributes.Append(att);

                            foreach (TreeNode cn in tn.Nodes)
                            {
                                var ee = xdoc.CreateElement("Entry");
                                var atr = xdoc.CreateAttribute("name");
                                atr.Value = cn.Text;
                                ee.Attributes.Append(atr);
                                atr = xdoc.CreateAttribute("url");
                                atr.Value = cn.Tag.ToString();
                                ee.Attributes.Append(atr);
                                ge.AppendChild(ee);
                            }

                            root.AppendChild(ge);
                        }
                        else
                        {
                            var ee = xdoc.CreateElement("Entry");
                            var atr = xdoc.CreateAttribute("name");
                            atr.Value = tn.Text;
                            ee.Attributes.Append(atr);
                            atr = xdoc.CreateAttribute("url");
                            atr.Value = tn.Tag.ToString();
                            ee.Attributes.Append(atr);
                            root.AppendChild(ee);
                        }
                    }
                    xdoc.Save(_favFile);

                    #endregion Browser
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
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
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Empty, exception, Logger.LogLevel.Error);
            }
        }

        private void accountSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reregister();
        }

        private void muteMicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MuteMicrophone(!picMic.Visible);
        }

        private void muteSpeakerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MuteSpeaker(!picSpek.Visible);
        }

        private void volumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                LoadDevices();

                var frmAudio = new AudioWiz(new SoundSetting { ComboBoxMicrophones = _comboBoxMicrophones, ComboBoxSpeakers = _comboBoxSpeakers, MicVolume = _phoneController.getMicVolume(), SpeakerVolume = _phoneController.getSpeakerVolume(), MicIndex = _comboBoxMicrophones.FindString(_selectedMic), SepkIndex = _comboBoxSpeakers.FindString(_selectedSpeaker) , MicMute = picMic.Visible, SpkMute=picSpek.Visible});

                frmAudio.Closing += (s, e1) =>
                {
                    try
                    {
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoDefault, "frmAudio.OnAudioPlayTest", exception,
                            Logger.LogLevel.Error);
                    }
                };

                frmAudio.OnAudioPlayTest += val =>
                {
                    try
                    {
                        _phoneController.audioPlayLoopbackTest(val);
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoDefault, "frmAudio.OnAudioPlayTest", exception,
                            Logger.LogLevel.Error);
                    }
                };

                frmAudio.OnMicChanged += val =>
                {
                    try
                    {
                        _phoneController.setMicVolume(val);
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoDefault, "frmAudio.OnMicVolumeChanged",
                            exception, Logger.LogLevel.Error);
                    }
                };

                frmAudio.OnSpeakerChanged += val =>
                {
                    try
                    {
                        _phoneController.setSpeakerVolume(val);
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoDefault, "frmAudio.OnSpeakerVolumeChanged",
                            exception, Logger.LogLevel.Error);
                    }
                };

                frmAudio.OnMic += MuteMicrophone;

                frmAudio.OnSpeaker += MuteSpeaker;

                frmAudio.ShowDialog(this);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "volumeToolStripMenuItem_Click", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void MuteSpeaker(bool val)
        {
            try
            {
                _phoneController.muteSpeaker(val);
                picSpek.Visible = val;
                if (!val)
                    _phoneController.setSpeakerVolume(_phoneController.getMicVolume());
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "frmAudio.OnSpeakerMute", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void MuteMicrophone(bool val)
        {
            try
            {
                _phoneController.muteMicrophone(val);
                picMic.Visible = val;

                if (!val)
                    _phoneController.setMicVolume(_phoneController.getMicVolume());
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "frmAudio.OnMicMute", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void officialBreakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _agent.AgentCurrentState.OnRequestBreak(_agent, "Official Break");
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "officialBreakToolStripMenuItem_Click",
                    exception, Logger.LogLevel.Error);
            }
        }

        private void mealBreakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _agent.AgentCurrentState.OnRequestBreak(_agent, "Meal Break");
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "mealBreakToolStripMenuItem_Click", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void aUXBreakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _agent.AgentCurrentState.OnRequestBreak(_agent, "AUX Break");
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "aUXBreakToolStripMenuItem_Click", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void CancelRequestmenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _agent.AgentCurrentState.OnRequestBreakCancel(_agent);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "CancelRequestmenuItem_Click", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void EndBreakmenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                StoptDurations();
                Invoke(((MethodInvoker)(() =>
                {
                    gbBreakMode.SendToBack();
                    gbBreakMode.Visible = false;
                    breakRequestToolStripMenuItem.Enabled = true;
                    cancelRequestToolStripMenuItem.Enabled = false;
                    endBreakToolStripMenuItem.Enabled = false;
                    _textBoxNumber.BackColor = Color.Black;
                    txtStatus.BackColor = Color.Black;
                    endBreakToolStripMenuItem.Enabled = false;
                    officialBreakToolStripMenuItem.Enabled = true;
                    mealBreakToolStripMenuItem.Enabled = true;
                    aUXBreakToolStripMenuItem.Enabled = true;
                    txtStatus.Text = string.Empty;
                })));
                _agent.AgentCurrentState.OnEndBreak(_agent);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "EndBreakmenuItem_Click", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void btnFreez_Click(object sender, EventArgs e)
        {
            try
            {
                string msg;
                if (btnFreez.Text.Equals(Resources.FormDialPad_btnFreez_Click_Freeze))
                {
                    FreezeAcwTime();
                    txtStatus.Text = Resources.FormDialPad_btnFreez_Click_Freeze;
                    btnFreez.Text = Resources.FormDialPad_btnFreez_Click_End_Freeze;
                    msg = Resources.FormDialPad_btnFreez_Click_Freeze;
                    StartDurations();
                    InvokeBrowserScript("DialerState", new object[] { _agent.CallSessionId, msg });
                }
                else
                {
                    StoptDurations();
                    txtStatus.Text = string.Empty;
                    btnFreez.Text = Resources.FormDialPad_btnFreez_Click_Freeze;
                    btnFreez.Enabled = false;
                    msg = Resources.FormDialPad_btnFreez_Click_End_Freeze;
                    logger.LogMessage(Logger.LogAppender.DuoLogger1,
                        string.Format("End ACW time After Freeze. {0}", _agent.CallSessionId), Logger.LogLevel.Info);
                    _agent.AgentCurrentState.OnEndAcw(_agent, _agent.CallSessionId);
                    InvokeBrowserScript("DialerState", new object[] { _agent.CallSessionId, msg });
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "btnFreez_Click", exception,
                    Logger.LogLevel.Error);
                logger.LogMessage(Logger.LogAppender.DuoLogger1, "btnFreez_Click", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void wavPlayer_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "wavPlayer_LoadCompleted",
                    Logger.LogLevel.Info);
                //if (playingRingIntone)
                //    ((SoundPlayer) sender).PlayLooping();
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "wavPlayer_LoadCompleted", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void setPhonePanelLocation()
        {
            try
            {
                /*
                PanelPhone.Location = new Point(Width - 250, Height - 410);//208, 333
                panelAgentList.Location = new Point(Width - 460, Height - 400);
                panelIvr.Location = new Point(Width - 460, Height - 420);
                 */
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "setPhonePanelLocation", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void toolStripStatusDuoPhone_Click(object sender, EventArgs e)
        {
            setPhonePanelLocation();
            PanelPhone.Visible = !PanelPhone.Visible;
            //ultraPopupControlPhone.Show();
        }

        private void toolStripStatusTools_Click(object sender, EventArgs e)
        {
            ultraPopupControlTools.Show();
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
            onToolStripMenuItem.Checked = false;
            offToolStripMenuItem.Checked = !onToolStripMenuItem.Checked;
        }

        private void btnBreakMode_Click(object sender, EventArgs e)
        {
            EndBreakmenuItem_Click(sender, e);
        }

        private void menuItemAnswerCall_Click(object sender, EventArgs e)
        {
            MakeCall(_textBoxNumber.Text);
        }

        private void menuItemRejectCall_Click(object sender, EventArgs e)
        {
            EndCall();
        }

        private void menuItemHoldCall_Click(object sender, EventArgs e)
        {
            HoldUnholdCall();
        }

        private void btnReregister_Click(object sender, EventArgs e)
        {
            Reregister();
        }

        private bool _isCallLogOpen;
        private FrmCallLogs _logs;

        private void btnCallLogs_Click(object sender, EventArgs e)
        {
            try
            {
                if (_isCallLogOpen)
                    return;
                _logs = new FrmCallLogs(_callLogs);
                _logs.OnNumberSelect += no =>
                {
                    _textBoxNumber.Text = no;
                };
                _logs.FormClosed += (w, k) =>
                {
                    _isCallLogOpen = false;
                };
                _isCallLogOpen = true;
                _logs.Show(this);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "btnCallLogs_Click", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void AutoAnswer_Click(object sender, EventArgs e)
        {
            AutoAnswer.Checked = !AutoAnswer.Checked;
        }

        private void AutoAnswer_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                AutoAnswer.BackColor = AutoAnswer.Checked ? Color.Maroon : Color.Black;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "AutoAnswer_CheckedChanged", exception,Logger.LogLevel.Error);
            }
        }

        private void onToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                onToolStripMenuItem.BackColor = onToolStripMenuItem.Checked ? Color.DarkGreen : Color.Black;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "onToolStripMenuItem_CheckedChanged",
                    exception, Logger.LogLevel.Error);
            }
        }

        private void offToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                offToolStripMenuItem.BackColor = offToolStripMenuItem.Checked ? Color.DarkRed : Color.Black;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "offToolStripMenuItem_CheckedChanged",
                    exception, Logger.LogLevel.Error);
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
                logger.LogMessage(Logger.LogAppender.DuoDefault, "OFFLINE_Click", exception,
                    Logger.LogLevel.Error);
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
                logger.LogMessage(Logger.LogAppender.DuoDefault, "openLogfiles_Click", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void lblAgentMode_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.ToolTipTitle = "Agent Mode";
            toolTip1.Show(_agent.AgentMode.ToString(), lblAgentMode, 1500);
        }

        private void outboundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _agent.AgentCurrentState.OnRequestModeChange(_agent, AgentMode.Outbound);
                outboundToolStripMenuItem.Enabled = false;
                inboundToolStripMenuItem.Enabled = false;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "outboundToolStripMenuItem_Click", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void inboundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _agent.AgentCurrentState.OnRequestModeChange(_agent, AgentMode.Inbound);
                outboundToolStripMenuItem.Enabled = false;
                inboundToolStripMenuItem.Enabled = false;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "outboundToolStripMenuItem_Click", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void openLogDic_Click(object sender, EventArgs e)
        {
            try
            {
                OpenLogFilsDirectory();
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "openLogDic_Click", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void InitializeError(string statusText, int statusCode)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4,
                    string.Format("phoneController_OnInitializeError : {0} , SipRegisterTryCount : {1}", statusText,
                        _sipRegisterTryCount), Logger.LogLevel.Error);
                
                _sipRegisterTryCount++;

                _agent.AgentCurrentState.OnError(_agent, statusText, statusCode,
                    "Unable to Communicate With Servers. Please Contact Your System Administrator.");

                //Invoke(((MethodInvoker)(() =>
                //{
                //    btnReregister.Visible = true;
                //    txtStatus.ForeColor = Color.Red;
                //    txtStatus.Text = "Error on Initializing" + statusText;
                //    PhoneStatus.Image = Resources.offline;
                //})));

                //if (SipRegisterTryCount >= 3)
                //{
                //    Invoke(((MethodInvoker)(() =>
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
                //    Invoke(new MethodInvoker(delegate
                //    {
                //        txtStatus.ForeColor = Color.DarkGreen;
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
                logger.LogMessage(Logger.LogAppender.DuoDefault, "phoneController_OnInitializeError", exception,
                    Logger.LogLevel.Error);
            }
        }

        #endregion FormEvents

        #region SIPCallbackEvents

        public int OnRegisterSuccess(int callbackIndex, int callbackObject, string statusText, int statusCode)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "OnRegisterSuccess", Logger.LogLevel.Info);
                

                _sipRegisterTryCount = 0;
                Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = Color.DarkGreen;
                    txtStatus.Text = Resources.FormDialPad_onRegisterSuccess_Phone_Initialized_with_the_IP + statusText;
                    PhoneStatus.Image = Resources.online;
                    PanelPhone.Enabled = false;
                    phoner8ClickMenu.Enabled = true;
                    btnReregister.Visible = false;
                })));
                mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Phone Initialized.", ToolTipIcon.Info);
                _call = new Call(string.Empty, this);
                _agent.AgentCurrentState.LogOn(_agent);
                InvokeBrowserScript("SIPEvents",new object[] { "OnRegisterSuccess", "***?***", statusText, statusCode.ToString() });
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "OnRegisterSuccess", exception,
                    Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onRegisterFailure(int callbackIndex, int callbackObject, string statusText, int statusCode)
        {
            logger.LogMessage(Logger.LogAppender.DuoLogger4, "onRegisterFailure", Logger.LogLevel.Info);
            InitializeError(statusText, statusCode);
            InvokeBrowserScript("SIPEvents", new object[] { "onRegisterFailure", "***?***", statusText, statusCode.ToString() });
            return 0;
        }

        public int onInviteRinging(int callbackIndex, int callbackObject, int sessionId, string statusText,
            int statusCode)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteRinging", Logger.LogLevel.Info);
                PlayRingInTone();
                _call.CallCurrentState.OnRinging(_call, callbackObject, sessionId, statusText,
                    statusCode);
                Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = Color.DarkGreen;
                    txtStatus.Text = statusText;
                })));
                InvokeBrowserScript("SIPEvents", new object[] { "onInviteRinging", _agent.CallSessionId, "***?***", "***?***" });
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteRinging", exception,
                    Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteIncoming(int callbackIndex, int callbackObject, int sessionId, string callerDisplayName,string caller,string calleeDisplayName, string callee, string audioCodecNames, string videoCodecNames, bool existsAudio,bool existsVideo)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4,string.Format("onInviteIncoming. caller : {0} Agent State : {1}, Call State : {2}", caller,_agent.AgentCurrentState, _call.CallCurrentState), Logger.LogLevel.Info);
                if (_agent.AgentCurrentState.GetType() != typeof(AgentIdle))
                {
                    logger.LogMessage(Logger.LogAppender.DuoLogger4,string.Format("Call receive in Invalid Agent State.. caller : {0}, Agent State : {1}", caller,_agent.AgentCurrentState), Logger.LogLevel.Error);
                    _phoneController.rejectCall(sessionId, 486);
                    return -1;
                }
                if (_call.CallCurrentState.GetType() != typeof(CallIdleState))
                    _call.CallCurrentState = new CallIdleState();

                _agent.AgentCurrentState.OnIncomingCall(_agent, caller, sessionId);
                _agent.PortsipSessionId = sessionId;

                _call.CallSessionId = caller.Split('@')[0].Replace("sip:", string.Empty);
                _agent.CallSessionId = _call.CallSessionId;
                _call.SetDialInfo(sessionId, Guid.NewGuid());
                _call.CallCurrentState.OnIncoming(_call, callbackObject, sessionId,caller, callee);
                _call.currentCallLogId = Guid.NewGuid();
                Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = Color.DarkMagenta;
                    txtStatus.Text = Resources.FormDialPad_onInviteIncoming_Incoming_Call__ + callee;

                    new Thread(() => GetPhoneNo(_agent.CallSessionId)).Start();
                    if (_playRingtone)
                        PlayRingTone();
                    
                    _alert = new FrmIncomingCall(_isNotAllowToReject, _reginTime);
                    if (!_showCallAlert) return;
                    _alert.Closed += (s, e) =>
                    {
                        if (AutoAnswer.Checked)
                            return;
                        if (_isCallAnswerd)
                            return;
                        StopRingTone();
                        if (_alert.IsCallAnswered)
                            MakeCall(string.Empty);
                        else
                            EndCall();
                    };
                    _alert.Show(this);
                })));

                var jsonString = sessionId + "|" + _agent.CallSessionId + "|" + callerDisplayName;

                InvokeBrowserScript("SIPEvents",new object[] { "OnCallIncoming", _agent.CallSessionId, jsonString, calleeDisplayName });
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteIncoming", exception,Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteTrying(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteTrying", Logger.LogLevel.Info);
                PlayRingInTone();
                _call.CallCurrentState.OnMake(_call);
                InvokeBrowserScript("SIPEvents", new object[] { "onInviteTrying", _agent.CallSessionId, "***?***", "***?***" });
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteTrying", exception,
                    Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteSessionProgress(int callbackIndex, int callbackObject, int sessionId, string audioCodecNames,
            string videoCodecNames, bool existsEarlyMedia, bool existsAudio, bool existsVideo)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteSessionProgress",
                    Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onInviteAnswered(int callbackIndex, int callbackObject, int sessionId, string callerDisplayName,
            string caller,
            string calleeDisplayName, string callee, string audioCodecNames, string videoCodecNames, bool existsAudio,
            bool existsVideo)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteAnswered", Logger.LogLevel.Info);
                StopRingTone();
                StopRingInTone();

                _call.CallCurrentState.OnAnswer(_call);
                Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = Color.DarkGoldenrod;
                    txtStatus.Text = Resources.FormDialPad_onInviteAnswered_Call_Answered;
                })));
                InvokeBrowserScript("SIPEvents", new object[] { "onInviteAnswered", _agent.CallSessionId, "***?***", "***?***" });
                _missCallCount = 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteAnswered", exception,
                    Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteFailure(int callbackIndex, int callbackObject, int sessionId, string reason, int code)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteFailure", Logger.LogLevel.Info);
                DisableAlert();
                StopRingInTone();
                StopRingTone();
                _call.CallCurrentState.OnReject(_call);
                var sid = _agent.CallSessionId;
                _agent.AgentCurrentState.OnFailMakeCall(_agent);
                Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = Color.DarkGreen;
                    txtStatus.Text = Resources.FormDialPad_onInviteFailure_Call_Rejected_from_Other_End + reason;
                })));
                _isCallAnswerd = false;
                InvokeBrowserScript("SIPEvents", new object[] { "onInviteFailure", sid, "***?***", "***?***" });
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteFailure", exception,
                    Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteUpdated(int callbackIndex, int callbackObject, int sessionId, string audioCodecNames,
            string videoCodecNames,
            bool existsAudio, bool existsVideo)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteUpdated", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onInviteConnected(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteConnected", Logger.LogLevel.Info);
                _isCallAnswerd = true;
                DisableAlert();
                StopRingTone();
                StopRingInTone();

                switch (_agent.AgentMode)
                {
                    case AgentMode.Outbound:
                        if (!waitToStartCallTIme)
                        {
                            StartCallTImmer();
                        }
                        break;
                    default:
                        StartCallTImmer();
                        break;
                }
                _call.CallCurrentState.OnAnswer(_call);
                Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = Color.DarkGoldenrod;
                    txtStatus.Text = Resources.FormDialPad_onInviteConnected_Call_Established_;
                })));
                InvokeBrowserScript("SIPEvents", new object[] { "onInviteConnected", _agent.CallSessionId, "***?***", "***?***" });
                _missCallCount = 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteAnswered", exception,
                    Logger.LogLevel.Error);
            }
            return 0;
        }
        
        public int onInviteBeginingForward(int callbackIndex, int callbackObject, string forwardTo)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteBeginingForward",Logger.LogLevel.Info);
                Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = Color.DarkGoldenrod;
                    txtStatus.Text = Resources.FormDialPad_onInviteBeginingForward_Call_Beginning_Forward_;
                })));
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteBeginingForward", exception,Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteClosed(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteClosed", Logger.LogLevel.Info);
                DisableAlert();
                StopRingInTone();
                StopRingTone();
                _call.CallCurrentState.OnDisconnected(_call);
                _agent.AgentCurrentState.OnEndCall(_agent, _isCallAnswerd);
                Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = Color.DarkGreen;
                    txtStatus.Text = Resources.FormDialPad_onInviteClosed_Call_Ended;
                })));

                if (!_isCallAnswerd)
                {
                    AddMiscallToCallLogs();
                }
                AddCallDurations();
                _isCallAnswerd = false;
                InvokeBrowserScript("SIPEvents", new object[] { "onInviteClosed", _agent.CallSessionId, "***?***", "***?***" });
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteClosed", exception,
                    Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onRemoteHold(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onRemoteHold", Logger.LogLevel.Info);
                Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = Color.DarkGreen;
                    txtStatus.Text = Resources.FormDialPad_onRemoteHold_Remote_Hold;
                })));
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onRemoteUnHold(int callbackIndex, int callbackObject, int sessionId, string audioCodecNames,
            string videoCodecNames,
            bool existsAudio, bool existsVideo)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onRemoteUnHold", Logger.LogLevel.Info);
                Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.ForeColor = Color.DarkGreen;
                    txtStatus.Text = Resources.FormDialPad_onRemoteUnHold_Remote_UnHold;
                })));
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onReceivedRefer(int callbackIndex, int callbackObject, int sessionId, int referId, string to,
            string @from,
            string referSipMessage)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onReceivedRefer", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onReferAccepted(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onReferAccepted", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onReferRejected(int callbackIndex, int callbackObject, int sessionId, string reason, int code)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onReferRejected", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onTransferTrying(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onTransferTrying", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onTransferRinging(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onTransferRinging", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onACTVTransferSuccess(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onACTVTransferSuccess", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onACTVTransferFailure(int callbackIndex, int callbackObject, int sessionId, string reason, int code)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onACTVTransferFailure", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onReceivedSignaling(int callbackIndex, int callbackObject, int sessionId, StringBuilder signaling)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onReceivedSignaling", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onSendingSignaling(int callbackIndex, int callbackObject, int sessionId, StringBuilder signaling)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onSendingSignaling", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onWaitingVoiceMessage(int callbackIndex, int callbackObject, string messageAccount,
            int urgentNewMessageCount,
            int urgentOldMessageCount, int newMessageCount, int oldMessageCount)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onWaitingVoiceMessage", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onWaitingFaxMessage(int callbackIndex, int callbackObject, string messageAccount,
            int urgentNewMessageCount,
            int urgentOldMessageCount, int newMessageCount, int oldMessageCount)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onWaitingFaxMessage", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onRecvDtmfTone(int callbackIndex, int callbackObject, int sessionId, int tone)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onRecvDtmfTone", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onRecvOptions(int callbackIndex, int callbackObject, StringBuilder optionsMessage)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onRecvOptions", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onRecvInfo(int callbackIndex, int callbackObject, StringBuilder infoMessage)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onRecvInfo", Logger.LogLevel.Info);
                if (infoMessage != null)
                {
                    ReceveMeassge("Receive Information", infoMessage.ToString());
                    InvokeBrowserScript("SIPEvents",new object[] { "onRecvInfo", _agent.CallSessionId, infoMessage.ToString(), "***?***" });
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onPresenceRecvSubscribe(int callbackIndex, int callbackObject, int subscribeId,
            string fromDisplayName, string @from,
            string subject)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onPresenceRecvSubscribe",
                    Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onPresenceOnline(int callbackIndex, int callbackObject, string fromDisplayName, string @from,
            string stateText)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onPresenceOnline", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onPresenceOffline(int callbackIndex, int callbackObject, string fromDisplayName, string @from)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onPresenceOffline", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onRecvMessage(int callbackIndex, int callbackObject, int sessionId, string mimeType,
            string subMimeType,
            byte[] messageData, int messageDataLength)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onRecvMessage", Logger.LogLevel.Info);
                if (mimeType == "text" && subMimeType == "plain")
                {
                    var mesageText = GetString(messageData);
                    ReceveMeassge("Receive Information", mesageText);
                    InvokeBrowserScript("SIPEvents",new object[] { "onRecvMessage", _agent.CallSessionId, mesageText, "***?***" });
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onRecvOutOfDialogMessage(int callbackIndex, int callbackObject, string fromDisplayName, string @from,string toDisplayName, string to, string mimeType, string subMimeType, byte[] messageData,int messageDataLength)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onRecvOutOfDialogMessage",Logger.LogLevel.Info);
                if (mimeType == "text" && subMimeType == "plain")
                {
                    var mesageText = GetString(messageData);
                    ReceveMeassge("Receive Information", mesageText);
                    InvokeBrowserScript("SIPEvents",new object[] { "onRecvOutOfDialogMessage", _agent.CallSessionId, mesageText, "***?***" });
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onSendMessageSuccess(int callbackIndex, int callbackObject, int sessionId, int messageId)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onSendMessageSuccess", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onSendMessageFailure(int callbackIndex, int callbackObject, int sessionId, int messageId,
            string reason, int code)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onSendMessageFailure", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onSendOutOfDialogMessageSuccess(int callbackIndex, int callbackObject, int messageId,
            string fromDisplayName,
            string @from, string toDisplayName, string to)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onSendOutOfDialogMessageSuccess",
                    Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onSendOutOfDialogMessageFailure(int callbackIndex, int callbackObject, int messageId,
            string fromDisplayName,
            string @from, string toDisplayName, string to, string reason, int code)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onSendOutOfDialogMessageFailure",
                    Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onPlayAudioFileFinished(int callbackIndex, int callbackObject, int sessionId, string fileName)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onPlayAudioFileFinished",
                    Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onPlayVideoFileFinished(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onPlayVideoFileFinished",
                    Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onReceivedRtpPacket(IntPtr callbackObject, int sessionId, bool isAudio, byte[] rtpPacket,int packetSize)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onReceivedRtpPacket", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onSendingRtpPacket(IntPtr callbackObject, int sessionId, bool isAudio, byte[] rtpPacket,int packetSize)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onSendingRtpPacket", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onAudioRawCallback(IntPtr callbackObject, int sessionId, int callbackType, byte[] data,
            int dataLength,
            int samplingFreqHz)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onAudioRawCallback", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onVideoRawCallback(IntPtr callbackObject, int sessionId, int callbackType, int width, int height,
            byte[] data,
            int dataLength)
        {
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "onVideoRawCallback", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception,
                    Logger.LogLevel.Error);
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
                        Invoke(new MethodInvoker(delegate
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
                logger.LogMessage(Logger.LogAppender.DuoDefault, "ShowNotifications", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void ShowCallLogs()
        {
            logger.LogMessage(Logger.LogAppender.DuoDefault, "ShowCallLogs", Logger.LogLevel.Debug);
        }

        public void ShowSetting()
        {
            logger.LogMessage(Logger.LogAppender.DuoDefault, "ShowSetting", Logger.LogLevel.Debug);
        }

        public void InAgentIdleState(AgentEvent agentPvState)
        {
            try
            {
                Invoke(((MethodInvoker)(() =>
                {
                    OFFLINE.Visible = false;
                    gbBreakMode.SendToBack();
                    gbBreakMode.Visible = false;
                    breakRequestToolStripMenuItem.Enabled = true;
                    cancelRequestToolStripMenuItem.Enabled = false;
                    endBreakToolStripMenuItem.Enabled = false;
                    _textBoxNumber.BackColor = Color.Black;
                    txtStatus.BackColor = Color.Black;

                    txtStatus.ForeColor = Color.DarkGreen;
                    txtStatus.Text = Resources.FormDialPad_InAgentIdleState_IDLE;
                    _textBoxNumber.Text = string.Empty;
                    CurrentNumber = string.Empty;
                    buttonAnswer.Enabled = true;
                    buttonReject.Enabled = false;
                    PanelPhone.Enabled = true;
                    ActiveControl = _textBoxNumber;
                    btnFreez.Visible = false;
                    _textBoxNumber.Focus();
                    if (agentPvState.GetType() == typeof(AgentInitiate) ||
                        agentPvState.GetType() == typeof(AgentOffline))
                    {
                        PhoneStatus.Image = Resources.online;
                        new Thread(() => _resourceCheckerHandler.StartStatusMap()).Start();
                    }

                    setPhonePanelLocation();
                    PanelPhone.Visible = true;
                })));

                _agent.CallSessionId = string.Empty;
                _agent.PortsipSessionId = -1;
                _agent.IsCallAnswer = false;
                _call.CallSessionId = string.Empty;
                _call.portSipSessionId = -1;
                _call.PhoneNo = string.Empty;

                _callDurations.Stop();
                _callDurations.Enabled = false;
                _acwCutdownTimer.Stop();
                _acwCutdownTimer.Enabled = false;
                _freezeDurations.Stop();
                _freezeDurations.Enabled = false;
                _agent.TransferCall = false;
                logger.LogMessage(Logger.LogAppender.DuoDefault, "CallSessionId set to Empty.",
                    Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "InIdleState", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void InAgentAcwState()
        {
            try
            {
                _callDurations.Stop();
                _callDurations.Enabled = false;
                _acwCotdown = _acwTime;
                _acwCutdownTimer.Enabled = true;
                _acwCutdownTimer.Start();
                Invoke(new MethodInvoker(() =>
                {
                    btnFreez.Enabled = true;
                    btnFreez.Visible = true;
                    buttonAnswer.Enabled = false;
                    buttonReject.Enabled = false;
                    txtStatus.Text = string.Format("ACW : {0}", _acwCotdown);
                }));

                InCallIdleState();
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "InAcwState", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallConnectedState()
        {
            try
            {
                Invoke(((MethodInvoker)(() =>
                {
                    buttonHold.Text = Resources.FormDialPad_InCallDisconnectedState_Hold;
                    buttonHold.Enabled = true;

                    buttonAnswer.Enabled = false;
                    buttonReject.Enabled = true;
                    buttontransferIvr.Enabled = false;
                    buttontransferCall.Enabled = false;
                    buttonEtl.Enabled = false;
                    buttonswapCall.Enabled = false;
                    buttonConference.Enabled = false;

                    answerCallToolStripMenuItem.Enabled = false;
                    rejectCallToolStripMenuItem.Enabled = !_isNotAllowToReject;
                    holdCallToolStripMenuItem.Enabled = false;
                })));
                _agent.IsCallAnswer = true;
                _agent.TransferCall = false;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "InCallConnectedState", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void InOfflineState(string statusText, string msg, int statusCode)
        {
            try
            {
                Invoke(((MethodInvoker)(() =>
                {
                    phoner8ClickMenu.Enabled = false;
                    PhoneStatus.Image = Resources.offline;
                    txtStatus.ForeColor = Color.Red;
                    btnReregister.Visible = true;
                    txtStatus.Text = statusText;
                    mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", txtStatus.Text, ToolTipIcon.Error);
                    OFFLINE.Text = msg;
                    OFFLINE.Visible = true;
                    setPhonePanelLocation();
                    PanelPhone.Visible = true;
                    if (statusCode == -9999)
                        UninitializePhone();
                })));
                
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "InOfflineState", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void InInitiateState()
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(((MethodInvoker) (SetInitialState)));
                }
                else
                {
                    SetInitialState();
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "InInitiateState", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void SetInitialState()
        {
            buttonHold.Enabled = false;
            buttonHold.Text = Resources.FormDialPad_InCallDisconnectedState_Hold;
            txtStatus.ForeColor = Color.DarkGreen;
            txtStatus.Text = string.Empty;

            buttonAnswer.Enabled = true;
            buttonReject.Enabled = false;
            buttontransferIvr.Enabled = false;
            buttontransferCall.Enabled = false;
            buttonEtl.Enabled = false;
            buttonswapCall.Enabled = false;
            buttonConference.Enabled = false;
            answerCallToolStripMenuItem.Enabled = false;
            rejectCallToolStripMenuItem.Enabled = false;
            holdCallToolStripMenuItem.Enabled = false;
            ActiveControl = _textBoxNumber;
            _textBoxNumber.Focus();
        }

        public void InInitiateMsgState(bool autoAnswerchk, bool autoAnswerEnb, string userName)
        {
            logger.LogMessage(Logger.LogAppender.DuoDefault, "InInitiateMsgState", Logger.LogLevel.Debug);
        }

        public void Error(string statusText)
        {
            
            logger.LogMessage(Logger.LogAppender.DuoDefault, "Error" + statusText, Logger.LogLevel.Error);
        }

        public void InBreakState()
        {
            try
            {
                Invoke(new MethodInvoker(delegate
                {
                    gbBreakMode.BringToFront();
                    txtStatus.Text = Resources.FormDialPad_InBreakState_Break_Mode;
                    _textBoxNumber.BackColor = Color.DarkRed;
                    txtStatus.BackColor = Color.DarkRed;
                    gbBreakMode.Visible = true;
                    StartDurations();
                    breakRequestToolStripMenuItem.Enabled = false;
                    cancelRequestToolStripMenuItem.Enabled = false;
                    endBreakToolStripMenuItem.Enabled = true;

                    answerCallToolStripMenuItem.Enabled = false;
                    rejectCallToolStripMenuItem.Enabled = false;
                    holdCallToolStripMenuItem.Enabled = false;
                })); 
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "InBreakState", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void InCallAgentClintConnectedState()
        {
            try
            {
                Invoke(((MethodInvoker)(() =>
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
                _agent.TransferCall = true;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "InCallAgentClintConnectedState", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void InCallAgentSupConnectedState()
        {
            try
            {
                Invoke(((MethodInvoker)(() =>
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
                _agent.TransferCall = true;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "InCallAgentSupConnectedState", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void InCallConferenceState()
        {
            try
            {
                Invoke(((MethodInvoker)(() =>
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
                    buttonEtl.Enabled = false;
                    buttonswapCall.Enabled = false;
                    answerCallToolStripMenuItem.Enabled = false;
                    rejectCallToolStripMenuItem.Enabled = false;
                    holdCallToolStripMenuItem.Enabled = false;
                })));
                _agent.TransferCall = true;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "InCallConferenceState", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void InCallDisconnectedState()
        {
            try
            {
                Invoke(((MethodInvoker)(() =>
                {
                    buttonHold.Enabled = false;
                    buttonHold.Text = Resources.FormDialPad_InCallDisconnectedState_Hold;

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
                logger.LogMessage(Logger.LogAppender.DuoDefault, "InCallConferenceState", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void InCallHoldState()
        {
            try
            {
                Invoke(((MethodInvoker)(() =>
                {
                    buttonHold.Text = Resources.FormDialPad_InCallHoldState_Unhold;

                    buttontransferCall.Enabled = true;
                    buttontransferIvr.Enabled = true;
                    buttonHold.Enabled = true;
                    buttonEtl.Enabled = false;

                    answerCallToolStripMenuItem.Enabled = false;
                    rejectCallToolStripMenuItem.Enabled = false;
                    holdCallToolStripMenuItem.Enabled = true;
                })));
                
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "InCallHoldState", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void InCallIdleState()
        {
            try
            {
                Invoke(((MethodInvoker)(() =>
                {
                    buttonAnswer.Enabled = false;
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
                logger.LogMessage(Logger.LogAppender.DuoDefault, "InCallIdleState", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void InAgentBusy()
        {
            try
            {
                Invoke(((MethodInvoker)(() =>
                {
                    txtStatus.Text = string.Empty;
                    //var val = callDirection == CallDirection.Incoming;
                    buttonAnswer.Enabled = false;
                    buttonReject.Enabled = false;
                    buttontransferIvr.Enabled = false;
                    buttontransferCall.Enabled = false;
                    buttonEtl.Enabled = false;
                    buttonswapCall.Enabled = false;
                    buttonConference.Enabled = false;
                    if (_isCallLogOpen)
                        _logs.Close();
                }))); 
                
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "InAgentBusy", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void CancelBreakRequest(ResourceProxyReplyDataResourceProxyReply result)
        {
            try
            {
                if (result.Command == WorkflowResultCode.ACDS701)
                {
                    Invoke(((MethodInvoker)(() =>
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
                logger.LogMessage(Logger.LogAppender.DuoDefault, "InAgentBusy", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void InCallRingingState()
        {
            try
            {
                Invoke(((MethodInvoker)(() =>
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
                    rejectCallToolStripMenuItem.Enabled = !_isNotAllowToReject;
                    holdCallToolStripMenuItem.Enabled = false;
                })));
                
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "InCallRingingState", exception,
                    Logger.LogLevel.Error);
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
                Invoke(((MethodInvoker)(() =>
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
                    rejectCallToolStripMenuItem.Enabled = !_isNotAllowToReject;
                    holdCallToolStripMenuItem.Enabled = false;
                })));
                SelectDuoTab(); 
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "InCallIncommingState", exception,
                    Logger.LogLevel.Error);
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
                            Invoke(new MethodInvoker(() =>
                            {
                                outboundToolStripMenuItem.Enabled = true;
                                inboundToolStripMenuItem.Enabled = false;
                                lblAgentMode.Appearance.Image = Resources.AgentInboundMode;
                                lblAgentMode.Text = string.Empty;
                                toolTip1.ToolTipTitle = "Inbound";
                                mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Agent Mode Change to Inbound.",
                                    ToolTipIcon.Info);
                            }));
                        }
                        break;

                    case AgentMode.Outbound:
                        {
                            Invoke(new MethodInvoker(() =>
                            {
                                outboundToolStripMenuItem.Enabled = false;
                                inboundToolStripMenuItem.Enabled = true;
                                lblAgentMode.Appearance.Image = Resources.AgentOutboundMode;
                                lblAgentMode.Text = string.Empty;
                                toolTip1.ToolTipTitle = "Outbound";
                                mynotifyicon.ShowBalloonTip(300, "Duo Soft Phone", "Agent Mode Change to Outbound.",
                                    ToolTipIcon.Info);
                            }));
                        }

                        break;

                    default:
                        throw new ArgumentOutOfRangeException("mode");
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "OnResourceModeChanged", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void OnSendModeChangeRequestOutbound(ResourceProxyReplyDataResourceProxyReply result)
        {
            try
            {
                if (result.Command == WorkflowResultCode.Error)
                {
                    Invoke(new MethodInvoker(() =>
                    {
                        outboundToolStripMenuItem.Enabled = true;
                        inboundToolStripMenuItem.Enabled = true;
                    }));
                    logger.LogMessage(Logger.LogAppender.DuoDefault,
                        string.Format("Error Occur in Mode Change Request. {0}", result), Logger.LogLevel.Error);
                }
                if (result.Command == WorkflowResultCode.ACDS502)
                {
                    Invoke(new MethodInvoker(() =>
                    {
                        lblAgentMode.Appearance.Image = Resources.AgentOutboundModeQ;
                        mynotifyicon.ShowBalloonTip(5, "Duo Soft Phone","Agent Successfully Registered for Mode Change.", ToolTipIcon.Info);
                    }));
                }
                if (result.Command == WorkflowResultCode.ACDE502)
                {
                    Invoke(new MethodInvoker(() =>
                    {
                        outboundToolStripMenuItem.Enabled = false;
                        inboundToolStripMenuItem.Enabled = true;
                    }));
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "OnSendModeChangeRequestOutbound", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void OnSendModeChangeRequestInbound(ResourceProxyReplyDataResourceProxyReply result)
        {
            try
            {
                switch (result.Command)
                {
                    case WorkflowResultCode.ACDS502:
                        {
                            Invoke(new MethodInvoker(() =>
                            {
                                lblAgentMode.Appearance.Image = Resources.AgentInboundModeQ;
                                mynotifyicon.ShowBalloonTip(5, "Duo Soft Phone",
                                    "Agent Successfully Registered for Mode Change.", ToolTipIcon.Info);
                            }));
                        }
                        break;

                    case WorkflowResultCode.ACDE502:
                        {
                            Invoke(new MethodInvoker(() =>
                            {
                                outboundToolStripMenuItem.Enabled = true;
                                inboundToolStripMenuItem.Enabled = false;
                            }));
                        }
                        break;

                    case WorkflowResultCode.Error:
                        {
                            Invoke(new MethodInvoker(() =>
                            {
                                outboundToolStripMenuItem.Enabled = true;
                                inboundToolStripMenuItem.Enabled = true;
                            }));
                            logger.LogMessage(Logger.LogAppender.DuoDefault,
                                string.Format("Error Occur in Mode Change Request. {0}", result), Logger.LogLevel.Error);
                        }
                        break;
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "OnSendModeChangeRequestOutbound", exception,
                    Logger.LogLevel.Error);
            }
        }

        #endregion UI State

        #region Browser

        private WebBrowser _curBrowser;
        

        private void BackToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                _curBrowser.GoBack();
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void ForwardToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                _curBrowser.GoForward();
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void StopToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                _curBrowser.Stop();
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void RefreshToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                _curBrowser.Refresh();
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void AddressToolStripComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        if (e.Shift)
                        {
                            GoNewTabToolStripMenuItem.PerformClick();
                        }
                        else
                        {
                            GoToolStripDropDownButton.PerformButtonClick();
                        }
                        break;
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void AddTabToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                AddTabPage(string.Empty);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _curBrowser = (WebBrowser)GetAll(MainTabControl.SelectedTab, typeof(WebBrowser)).First();
                adrBar.Visible = (!_curBrowser.DocumentTitle.ToLower().Contains(_agent.CallHandler.SipServiceCall.BaseTabName));

                if (MainTabControl.SelectedIndex == MainTabControl.TabPages.Count - 1)
                {
                    AddTabPage(string.Empty);
                    return;
                }

                if (MainTabControl.TabCount > 1)
                {
                    RemoveTabToolStripButton.Enabled = true;
                    CloseAllToolStripButton.Enabled = true;
                }
                else
                {
                    RemoveTabToolStripButton.Enabled = false;
                    CloseAllToolStripButton.Enabled = false;
                }

               
                AddressToolStripComboBox.Text = _curBrowser.Url != null ? _curBrowser.Url.ToString() : "about:blank";
                SelectPic((BrowserPreview)_curBrowser.Tag);

                switch (_curBrowser.EncryptionLevel)
                {
                    case WebBrowserEncryptionLevel.Bit128:
                    case WebBrowserEncryptionLevel.Bit40:
                    case WebBrowserEncryptionLevel.Bit56:
                    case WebBrowserEncryptionLevel.Fortezza:
                        SecureToolStripStatusLabel.Visible = true;
                        break;
                    default:
                        SecureToolStripStatusLabel.Visible = false;
                        break;
                }
                if (_curBrowser.DocumentTitle.Length > 0)
                {
                    MainTabControl.SelectedTab.Text = _curBrowser.DocumentTitle;
                    MainTabControl.SelectedTab.ImageIndex = 0;
                }
                else
                {
                    MainTabControl.SelectedTab.Text = AddressToolStripComboBox.Text.Length < 1 ? string.Format("Page {0}", MainTabControl.TabCount + 1) : AddressToolStripComboBox.Text;
                }
                BackToolStripButton.Enabled = _curBrowser.CanGoBack;
                ForwardToolStripButton.Enabled = _curBrowser.CanGoForward;

                
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void GoNewTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AddTabToolStripButton.PerformClick();
                GoToolStripDropDownButton.PerformButtonClick();
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void RemoveTabToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                if ((_curBrowser.DocumentTitle.ToLower().Contains(_agent.CallHandler.SipServiceCall.BaseTabName)) ||
                    (_curBrowser.DocumentTitle.Equals("*")))
                    return;

                var pic = (BrowserPreview)_curBrowser.Tag;
                pic.Click -= Page1BrowserPreview_Click;
                PreviewPanel.Controls.Remove(pic);
                MainTabControl.TabPages.Remove(MainTabControl.SelectedTab);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void GoToolStripDropDownButton_ButtonClick(object sender, EventArgs e)
        {
            try
            {
                var authHdr = "Authorization: Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(_agent.Auth.UserName + ":" + _passWord)) + "\r\n";
                _curBrowser.Navigate(AddressToolStripComboBox.Text, null, null, authHdr);
                if (!AddressToolStripComboBox.Items.Contains(AddressToolStripComboBox.Text))
                {
                    AddressToolStripComboBox.Items.Add(AddressToolStripComboBox.Text);
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private bool GetThumbCallback()
        {
            return false;
        }

        private void Page1BrowserPreview_Click(object sender, EventArgs e)
        {
            try
            {
                MainTabControl.SelectedTab = (TabPage)((BrowserPreview)sender).Tag;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void SelectPic(BrowserPreview pic)
        {
            try
            {
                foreach (var ctrl in PreviewPanel.Controls.Cast<Control>().Where(ctrl => ctrl.GetType() == typeof(BrowserPreview)))
                {
                    ((BrowserPreview)ctrl).Selected = ctrl == pic;
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void AddFavoriteToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                var fa = new AddFavorite(this)
                {
                    SingleTitleTextBox = {Text = MainTabControl.SelectedTab.Text},
                    SingleURLTextBox = {Text = AddressToolStripComboBox.Text}
                };
                foreach (var tn in FavoritesTreeView.Nodes.Cast<TreeNode>().Where(tn => tn.Tag.ToString() == "#GROUP#"))
                {
                    fa.GroupFavoriteComboBox.Items.Add(tn.Text);
                }
                if (fa.ShowDialog() != DialogResult.OK) return;
                var name = string.Empty;
                var url = string.Empty;
                var parentNodes = FavoritesTreeView.Nodes;
                switch (fa.AddSingleRadioButton.Checked)
                {
                    case true:
                        name = fa.SingleTitleTextBox.Text;
                        url = fa.SingleURLTextBox.Text;
                        parentNodes = FavoritesTreeView.Nodes;
                        break;
                    case false:
                        name = fa.GroupTitleTextBox.Text;
                        url = fa.GroupURLTextBox.Text;
                        parentNodes = FavoritesTreeView.Nodes[fa.GroupFavoriteComboBox.SelectedItem.ToString()].Nodes;
                        break;
                }
                var nodes = new TreeNode
                {
                    Name = name,
                    Text = name,
                    Tag = url
                };
                parentNodes.Add(nodes);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void PreviewFillButton_Click(object sender, EventArgs e)
        {
            try
            {
                frmLayoutsplitContainer.Panel2Collapsed = !frmLayoutsplitContainer.Panel2Collapsed;
                PreviewRefreshButton.Enabled = !frmLayoutsplitContainer.Panel2Collapsed;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void ShowPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                panelFavorites.Visible = false;
                panelPreview.Visible = true;
                frmLayoutsplitContainer.Panel1Collapsed = !frmLayoutsplitContainer.Panel1Collapsed;
                frmLayoutsplitContainer.Panel2Collapsed = false;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void ShowFavoritesToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                panelFavorites.Visible = true;
                panelPreview.Visible = false;
                frmLayoutsplitContainer.Panel1Collapsed = !frmLayoutsplitContainer.Panel1Collapsed;
                frmLayoutsplitContainer.Panel2Collapsed = false;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void PreviewRefreshButton_Click(object sender, EventArgs e)
        {
            try
            {
                TabPage curtab = MainTabControl.SelectedTab;

                foreach (var tab in MainTabControl.TabPages.Cast<TabPage>().Where(tab => !tab.Text.Equals("*")))
                {
                    MainTabControl.SelectedTab = tab;
                    Application.DoEvents();

                    var br = _curBrowser;
                    var pic = (BrowserPreview)br.Tag;
                    var bitmap = new Bitmap(br.DisplayRectangle.Width, br.DisplayRectangle.Height);
                    var gfxdst = Graphics.FromImage(bitmap);
                    gfxdst.CopyFromScreen(br.PointToScreen(new Point(br.DisplayRectangle.X, br.DisplayRectangle.Y)),new Point(0, 0), br.DisplayRectangle.Size);
                    pic.Image = bitmap.GetThumbnailImage(Convert.ToInt32(PreviewSizeNumericUpDown.Value),Convert.ToInt32(PreviewSizeNumericUpDown.Value),GetThumbCallback, IntPtr.Zero);
                    pic.SizeMode = PictureBoxSizeMode.AutoSize;
                    pic.BorderStyle = BorderStyle.Fixed3D;
                    gfxdst.Dispose();
                    bitmap.Dispose();
                }
                MainTabControl.SelectedTab = curtab;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void ScrollToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MainTabControl.Multiline = !ScrollToolStripMenuItem.Checked;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void TopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (TopToolStripMenuItem.Checked)
                {
                    BottomToolStripMenuItem.Checked = false;
                    LeftToolStripMenuItem.Checked = false;
                    RightToolStripMenuItem.Checked = false;
                    MainTabControl.Alignment = TabAlignment.Top;
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void BottomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (BottomToolStripMenuItem.Checked)
                {
                    TopToolStripMenuItem.Checked = false;
                    LeftToolStripMenuItem.Checked = false;
                    RightToolStripMenuItem.Checked = false;
                    MainTabControl.Alignment = TabAlignment.Bottom;
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void LeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (LeftToolStripMenuItem.Checked)
                {
                    BottomToolStripMenuItem.Checked = false;
                    TopToolStripMenuItem.Checked = false;
                    RightToolStripMenuItem.Checked = false;
                    MainTabControl.Alignment = TabAlignment.Left;
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void RightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!RightToolStripMenuItem.Checked) return;
                BottomToolStripMenuItem.Checked = false;
                LeftToolStripMenuItem.Checked = false;
                TopToolStripMenuItem.Checked = false;
                MainTabControl.Alignment = TabAlignment.Right;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _curBrowser.ShowSaveAsDialog();
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }
        
        private void PrintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _curBrowser.Print();
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }
        private void PrintOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _curBrowser.ShowPrintDialog();
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void PageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _curBrowser.ShowPageSetupDialog();
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void PrintPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _curBrowser.ShowPrintPreviewDialog();
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void PropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _curBrowser.ShowPropertiesDialog();
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        private void CloseAllToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                TabPage selectedPage = null;
                for (var i = MainTabControl.TabCount - 1; i >= 0; i--)
                {
                    if (MainTabControl.TabPages[i].Text.ToLower().Contains(_agent.CallHandler.SipServiceCall.BaseTabName))
                    {
                        selectedPage = MainTabControl.TabPages[i];
                        continue;
                    }
                    if (MainTabControl.TabPages[i].Text.Equals("*"))
                    {
                        continue;
                    }

                    var br = (WebBrowser)GetAll(MainTabControl.SelectedTab, typeof(WebBrowser)).First();
                    var pic = (BrowserPreview)br.Tag;
                    pic.Click -= Page1BrowserPreview_Click;
                    PreviewPanel.Controls.Remove(pic);
                    MainTabControl.TabPages.RemoveAt(i);
                }
                if (selectedPage != null)
                    MainTabControl.SelectedTab = selectedPage;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "CloseAllToolStripButton_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void FavoritesTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                if (e.Node.Tag.ToString() != "#GROUP#")
                {
                    AddressToolStripComboBox.Text = e.Node.Tag.ToString();
                    GoNewTabToolStripMenuItem.PerformClick();
                }
                else
                {
                    try
                    {
                        foreach (TreeNode tn in e.Node.Nodes)
                        {
                            AddressToolStripComboBox.Text = tn.Tag.ToString();
                            if (tn == e.Node.FirstNode)
                            {
                                GoToolStripDropDownButton.PerformButtonClick();
                            }
                            else
                            {
                                GoNewTabToolStripMenuItem.PerformClick();
                            }

                            //While CurBrowser.IsBusy
                            //    Application.DoEvents()
                            //End While
                        }
                    }
                    catch (Exception exception)
                    {
                        logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception,
                            Logger.LogLevel.Error);
                    }
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "WebBrowser", exception, Logger.LogLevel.Error);
            }
        }

        #endregion Browser

       /* private void OnIvrTestClick(object sender, EventArgs e)
        {
            var res = _phoneController.sendInfo(_call.portSipSessionId, "text", "plain", string.Format("{0}:{1}", TransferType.Ivrtransfer.ToString().ToLower(), _textBoxNumber.Text.Trim()));
        }
        */
        







    }
}