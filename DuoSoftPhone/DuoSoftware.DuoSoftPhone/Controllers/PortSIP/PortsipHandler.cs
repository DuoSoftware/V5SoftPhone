using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DuoSoftware.DuoLogger;
using DuoSoftware.DuoSoftPhone.Ui;

namespace DuoSoftware.DuoSoftPhone.Controllers.PortSIP
{
    public class PortsipHandler
    {
        #region Events

        public delegate void PortsipHandlerEvents(int sessionId ,string message,bool status);

        public event PortsipHandlerEvents OnMakeCall;
        
        #endregion

        #region Property

        private string _localIp;
        private string _hostName;
        const string LicenceKey = "qqqq"; //(string)this.InitParameters["licenceKey"];

        private int SessionId;
        private PortSIPCore portsip;

        #endregion
        
        public PortsipHandler(FormDialPad formDialPad)
        {
            portsip = new PortSIPCore(0, formDialPad);
        }


        private void InitAutioCodecs()
        {
            portsip.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_PCMA);
            portsip.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_PCMU);
            portsip.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_G729);


            portsip.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_DTMF); // For RTP event - DTMF (RFC2833)

            portsip.enableDTMFOfRFC2833(101);
        }
        
        public void Uninitialize()
        {
            try
            {
                portsip.shutdownCallbackHandlers();
                portsip.unInitialize();
                portsip.releaseCallbackHandlers();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Uninitialize", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        public bool InitializePhone(string userName, string displayName, string authName, string password, int localPort,int serverPort, string hostName)
        {

            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}", userName, authName, password, localPort), Logger.LogLevel.Info);
                _hostName = hostName;
                int errorCode = 0;
                portsip.createCallbackHandlers();
                var rt = portsip.initialize(TRANSPORT_TYPE.TRANSPORT_UDP, PORTSIP_LOG_LEVEL.PORTSIP_LOG_NONE, Application.StartupPath, 1, "PortSIP VoIP SDK 7.0", "", 0, out errorCode);
                if (rt == false)
                {
                    portsip.shutdownCallbackHandlers();
                    portsip.releaseCallbackHandlers();
                    //if (OnPortsipInitializeError != null)
                    //    OnPortsipInitializeError("Web Phone Initialization failed.");
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Web Phone Initialization failed.", Logger.LogLevel.Error);
                    return false;
                }

                portsip.setAudioCodecParameter(AUDIOCODEC_TYPE.AUDIOCODEC_AMRWB, "mode-set=0; octet-align=0; robust-sorting=0");

                var ipBuilder = new StringBuilder();
                var ipReturn = portsip.getLocalIP(0, ipBuilder, 16);
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}...... Step 1: Pass.", userName, authName, password, localPort), Logger.LogLevel.Info);
                if (ipReturn != 0)
                {
                    portsip.shutdownCallbackHandlers();
                    portsip.unInitialize();
                    portsip.releaseCallbackHandlers();
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "fail to getIp.", Logger.LogLevel.Error);
                    return false;
                }

                _localIp = ipBuilder.ToString();

                var outboundServer = "";
                var outboundServerPort = 0;
                var userDomain = "";
                var sipServerPort = serverPort;
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}...... Step 2: Pass.", userName, authName, password, localPort), Logger.LogLevel.Info);
                //    var licKey = "N0IyOTRGNjAzM0NCNkZCQTJDOTBEREI1NDlFNTk2N0RGRjA3NkQwNjM5MTJDQTQ2NzU3QTc5MUYyRjUzMjRERkMyNDdGRDgzNTgwN0FCMkU4NDU3MTA1Q0IwQTA4OEIyN0ZFNEJGNjIxM0I4NjU4ODIxNjVCOTFDQUYwQkY5RjBGNUJENjRBQTBCMTY0MkY3MUI5RkZEMTY3QzAwNDRFMUI2M0RBNzFFRThCOEQzNjU1ODIxNDYxMDFENjBBMTQ2Q0Y5Qzg1MDZGMUI4QTU3NTFDNTA0MzczMjNEQzcwNjI1NUY1RjY5MTYwRUE3NTg5MjdBMThFMjk2QkZGRERBQjc0Q0VGRTFGNjg0ODFDMDIwMDFCMDlERkUyOTU1MTAxQUM4REMwRTg2QUI4MDc4NDhGQ0RBRkI4MEU5Nzk0N0QyRjYzQUMwN0E4NDU4QjIyQjYxRTExRkM3QTAwRUQ3OTQwMThDRTRCQTMyQzQ4RTI.";
                var rt_userInfo = portsip.setUserInfo(userName, displayName, authName, password, _localIp, localPort, userDomain, hostName, sipServerPort, outboundServer, outboundServerPort);

                if (rt_userInfo != 0)
                {
                    portsip.shutdownCallbackHandlers();
                    portsip.unInitialize();
                    portsip.releaseCallbackHandlers();
                    //if (OnPortsipInitializeError != null)
                    //    OnPortsipInitializeError("SetUserInfo Failed");
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "SetUserInfo Failed.", Logger.LogLevel.Error);
                    return false;
                }

                portsip.setSrtpPolicy(SRTP_POLICY.SRTP_POLICY_NONE);

                portsip.setLicenseKey(LicenceKey);
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}...... Step 3: Pass.", userName, authName, password, localPort), Logger.LogLevel.Info);
                var rt_register = portsip.registerServer(90);


                if (rt_register != 0)
                {
                    portsip.shutdownCallbackHandlers();
                    portsip.unInitialize();
                    portsip.releaseCallbackHandlers();
                    //if (OnPortsipInitializeError != null)
                    //    OnPortsipInitializeError("Registration Failed");
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Registration Failed.", Logger.LogLevel.Error);
                    return false;
                }

                portsip.addSupportedMimeType("INFO", "text", "plain");
                InitAutioCodecs();

                portsip.setSpeakerVolume(26214);//40% volume
                portsip.setMicVolume(5243);//80%
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}...... Step 4: Pass.", userName, authName, password, localPort), Logger.LogLevel.Info);
                return true;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "InitializePhone", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        public  void ReInitializeSetting(string userName, string displayName, string authName, string password, int localPort, int serverPort, string hostName)
        {
            Uninitialize();
            InitializePhone(userName, displayName, authName, password, localPort, serverPort, hostName);
        }

        public bool DialCall(string sipNumber, int selectedSpeakerIndex, int selectedMicIndex)
        {

            try
            {
                var no = sipNumber;
                if (sipNumber != null)
                {
                    if (!sipNumber.ToLower().StartsWith("sip"))
                        sipNumber = "sip:" + sipNumber + "@" + _hostName;
                }

                portsip.setAudioDeviceId(selectedMicIndex, selectedSpeakerIndex);

                int errorCode;

                var sessionId = portsip.call(sipNumber, true, out errorCode);

                SessionId = sessionId;
                if (OnMakeCall != null)
                    OnMakeCall(sessionId,string.Format("Dialing To : {0}",no), true);
                return sessionId != -1;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "DialCall", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        public bool AnswerCall(int sessionId)
        {
            return portsip.answerCall(sessionId) == 0;
        }

        public bool RejectCall(int sessionId)
        {
            var result = portsip.rejectCall(sessionId, 486, "Busy here");
            return result == 0;
        }

        public bool CancelCall()
        {
            var result = portsip.terminateCall(SessionId);
            return result == 0;
        }

        public bool HangupCall()
        {
           return portsip.terminateCall(SessionId)==0;
        }

        public bool Disconnect()
        {
            portsip.shutdownCallbackHandlers();
            portsip.unInitialize();
            portsip.releaseCallbackHandlers();
            return true;
        }

        public bool HoldCall(int sessionId)
        {
            //portsip.hold(SessionId);
            var res = portsip.sendMessage(sessionId, "text", "plain", "hold");
            return res == 0;
        }

        public bool UnHoldCall(int sessionId)
        {
            //portsip.unHold(SessionId);
            var res = portsip.sendMessage(sessionId, "text", "plain", "unhold");
            return res == 0;
        }

        public bool TransferIvr(string toNumber)
        {
            var res = portsip.sendMessage(SessionId, "text", "plain", "ivrtransfer:" + toNumber);
            return res == 0;
        }

        public bool TransferCall(string toNumber, int sessionId)
        {

            var res = portsip.sendMessage(sessionId, "text", "plain", "transfer:" + toNumber);
            return res == 0;
        }

        public bool SwapCall(int sessionId)
        {
            var res = portsip.sendMessage(sessionId, "text", "plain", "swap");
            return res == 0;
        }

        public bool EtlCall(int sessionId)
        {
            var res = portsip.sendMessage(sessionId, "text", "plain", "etl");
            return res == 0;
        }

        public bool ConferenceCall()
        {
            var res = portsip.sendMessage(SessionId, "text", "plain", "conference");
            return res == 0;
        }

        public bool MuteMicrophone(bool val)
        {
            portsip.muteMicrophone(val);
            return true;
        }

        public bool MuteSpeaker(bool val)
        {
            portsip.muteSpeaker(val);
            return true;
        }
        
        public  void SendDtmf(int value)
        {
            char sendChar = 'x';

            if (value >= 0 && value <= 9)
                sendChar = (char)(48 + value);
            else if (value == 10)
                sendChar = '*';
            else if (value == 11)
                sendChar = '#';

            if (sendChar != 'x')
                portsip.sendDtmf(SessionId, (char)value);
        }

        

    }
}
