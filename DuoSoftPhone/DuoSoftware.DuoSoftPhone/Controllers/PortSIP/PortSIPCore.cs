using System;
using System.Runtime.InteropServices;
using System.Text;

//////////////////////////////////////////////////////////////////////////
//
//  !!!IMPORTANT!!! DON'T EDIT BELOW SOURCE CODE  
//
//////////////////////////////////////////////////////////////////////////


namespace DuoSoftware.DuoSoftPhone.Controllers.PortSIP
{
    internal class PortSIPCore
    {
        // Fields
        private static PortSIP_NativeMethods.audioRawCallback _arc = new PortSIP_NativeMethods.audioRawCallback(PortSIPCore.onAudioRawCallback);
        private static PortSIP_NativeMethods.arrivedSignaling _asg = new PortSIP_NativeMethods.arrivedSignaling(PortSIPCore.onArrivedSignaling);
        private static PortSIP_NativeMethods.ACTVTransferFailure _atf = new PortSIP_NativeMethods.ACTVTransferFailure(PortSIPCore.onACTVTransferFailure);
        private static PortSIP_NativeMethods.ACTVTransferSuccess _ats = new PortSIP_NativeMethods.ACTVTransferSuccess(PortSIPCore.onACTVTransferSuccess);
        private static IntPtr _callbackHandle = IntPtr.Zero;
        private int _callbackObject;
        private static PortSIP_NativeMethods.inviteAnswered _iva = new PortSIP_NativeMethods.inviteAnswered(PortSIPCore.onInviteAnswered);
        private static PortSIP_NativeMethods.inviteBeginingForward _ivbf = new PortSIP_NativeMethods.inviteBeginingForward(PortSIPCore.onInviteBeginingForward);
        private static PortSIP_NativeMethods.inviteClosed _ivc = new PortSIP_NativeMethods.inviteClosed(PortSIPCore.onInviteClosed);
        private static PortSIP_NativeMethods.inviteUACConnected _ivcc = new PortSIP_NativeMethods.inviteUACConnected(PortSIPCore.onInviteUACConnected);
        private static PortSIP_NativeMethods.inviteFailure _ivf = new PortSIP_NativeMethods.inviteFailure(PortSIPCore.onInviteFailure);
        private static PortSIP_NativeMethods.inviteIncoming _ivi = new PortSIP_NativeMethods.inviteIncoming(PortSIPCore.onInviteIncoming);
        private static PortSIP_NativeMethods.inviteRinging _ivr = new PortSIP_NativeMethods.inviteRinging(PortSIPCore.onInviteRinging);
        private static PortSIP_NativeMethods.inviteUASConnected _ivsc = new PortSIP_NativeMethods.inviteUASConnected(PortSIPCore.onInviteUASConnected);
        private static PortSIP_NativeMethods.inviteTrying _ivt = new PortSIP_NativeMethods.inviteTrying(PortSIPCore.onInviteTrying);
        private static PortSIP_NativeMethods.inviteUpdated _ivu = new PortSIP_NativeMethods.inviteUpdated(PortSIPCore.onInviteUpdated);
        private static PortSIP_NativeMethods.playAviFileToRemoteFinished _paf = new PortSIP_NativeMethods.playAviFileToRemoteFinished(PortSIPCore.onPlayAviFileFinished);
        private static PortSIP_NativeMethods.presenceOffline _pof = new PortSIP_NativeMethods.presenceOffline(PortSIPCore.onPresenceOffline);
        private static PortSIP_NativeMethods.presenceOnline _pon = new PortSIP_NativeMethods.presenceOnline(PortSIPCore.onPresenceOnline);
        private static PortSIP_NativeMethods.presenceRecvSubscribe _prs = new PortSIP_NativeMethods.presenceRecvSubscribe(PortSIPCore.onPresenceRecvSubscribe);
        private static PortSIP_NativeMethods.PASVTransferFailure _ptf = new PortSIP_NativeMethods.PASVTransferFailure(PortSIPCore.onPASVTransferFailure);
        private static PortSIP_NativeMethods.PASVTransferSuccess _pts = new PortSIP_NativeMethods.PASVTransferSuccess(PortSIPCore.onPASVTransferSuccess);
        private static PortSIP_NativeMethods.playWaveFileToRemoteFinished _pwf = new PortSIP_NativeMethods.playWaveFileToRemoteFinished(PortSIPCore.onPlayWaveFileFinished);
        private static PortSIP_NativeMethods.recvBinaryMessage _rbm = new PortSIP_NativeMethods.recvBinaryMessage(PortSIPCore.onRecvBinaryMessage);
        private static PortSIP_NativeMethods.recvBinaryPagerMessage _rbpm = new PortSIP_NativeMethods.recvBinaryPagerMessage(PortSIPCore.onRecvBinaryPagerMessage);
        private static PortSIP_NativeMethods.recvDtmfTone _rdt = new PortSIP_NativeMethods.recvDtmfTone(PortSIPCore.onRecvDtmfTone);
        private static PortSIP_NativeMethods.registerFailure _rgf = new PortSIP_NativeMethods.registerFailure(PortSIPCore.onRegisterFailure);
        private static PortSIP_NativeMethods.registerSuccess _rgs = new PortSIP_NativeMethods.registerSuccess(PortSIPCore.onRegisterSuccess);
        private static PortSIP_NativeMethods.recvInfo _ri = new PortSIP_NativeMethods.recvInfo(PortSIPCore.onRecvInfo);
        private static PortSIP_NativeMethods.recvMessage _rms = new PortSIP_NativeMethods.recvMessage(PortSIPCore.onRecvMessage);
        private static PortSIP_NativeMethods.recvOptions _ro = new PortSIP_NativeMethods.recvOptions(PortSIPCore.onRecvOptions);
        private static PortSIP_NativeMethods.recvPagerMessage _rpm = new PortSIP_NativeMethods.recvPagerMessage(PortSIPCore.onRecvPagerMessage);
        private static PortSIP_NativeMethods.remoteHold _rth = new PortSIP_NativeMethods.remoteHold(PortSIPCore.onRemoteHold);
        private static PortSIP_NativeMethods.remoteUnHold _rtu = new PortSIP_NativeMethods.remoteUnHold(PortSIPCore.onRemoteUnHold);
        private static SIPCallbackEvents _SIPCallbackEvents;
        private static IntPtr _SIPCoreHandle = IntPtr.Zero;
        private static PortSIP_NativeMethods.sendPagerMessageFailure _spmf = new PortSIP_NativeMethods.sendPagerMessageFailure(PortSIPCore.onSendPagerMessageFailure);
        private static PortSIP_NativeMethods.sendPagerMessageSuccess _spms = new PortSIP_NativeMethods.sendPagerMessageSuccess(PortSIPCore.onSendPagerMessageSuccess);
        private static PortSIP_NativeMethods.transferRinging _tsr = new PortSIP_NativeMethods.transferRinging(PortSIPCore.onTransferRinging);
        private static PortSIP_NativeMethods.transferTrying _tst = new PortSIP_NativeMethods.transferTrying(PortSIPCore.onTransferTrying);
        private static PortSIP_NativeMethods.videoRawCallback _vrc = new PortSIP_NativeMethods.videoRawCallback(PortSIPCore.onVideoRawCallback);
        private static PortSIP_NativeMethods.waitingFaxMessage _wfm = new PortSIP_NativeMethods.waitingFaxMessage(PortSIPCore.onWaitingFaxMessage);
        private static PortSIP_NativeMethods.waitingVoiceMessage _wvm = new PortSIP_NativeMethods.waitingVoiceMessage(PortSIPCore.onWaitingVoiceMessage);

        // Methods
        public PortSIPCore(int callbackObject, SIPCallbackEvents calbackevents)
        {
            this._callbackObject = callbackObject;
            _SIPCallbackEvents = calbackevents;
        }

        public void addAudioCodec(AUDIOCODEC_TYPE codecType)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_addAudioCodec(_SIPCoreHandle, (int)codecType);
            }
        }

        public int addExtensionHeader(string headerName, string headerValue)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_addExtensionHeader(_SIPCoreHandle, headerName, headerValue);
        }

        public int addSupportedMimeType(string methodName, string mimeType, string subMimeType)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_addSupportedMimeType(_SIPCoreHandle, methodName, mimeType, subMimeType);
        }

        public void addVideoCodec(VIDEOCODEC_TYPE codecType)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_addVideoCodec(_SIPCoreHandle, (int)codecType);
            }
        }

        public int answerCall(int sessionId)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_answerCall(_SIPCoreHandle, sessionId);
        }

        public int attendedRefer(int sessionId, int replaceSessionId, string referTo)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_attendedRefer(_SIPCoreHandle, sessionId, replaceSessionId, referTo);
        }

        public void audioPlayLoopbackTest(bool enable)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_audioPlayLoopbackTest(_SIPCoreHandle, enable);
            }
        }

        public int call(string callTo, bool sendSdp, out int errorCode)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                errorCode = PortSIP_Errors.ECoreSDKObjectNull;
                return PortSIP_Errors.INVALID_SESSION_ID;
            }
            return PortSIP_NativeMethods.PortSIP_call(_SIPCoreHandle, callTo, sendSdp, out errorCode);
        }

        public void clearAddExtensionHeaders()
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_clearAddExtensionHeaders(_SIPCoreHandle);
            }
        }

        public void clearAudioCodec()
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_clearAudioCodec(_SIPCoreHandle);
            }
        }

        public void clearModifyHeaders()
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_clearModifyHeaders(_SIPCoreHandle);
            }
        }

        public void clearVideoCodec()
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_clearVideoCodec(_SIPCoreHandle);
            }
        }

        public bool createCallbackHandlers()
        {
            if (_callbackHandle != IntPtr.Zero)
            {
                return false;
            }
            if (!this.createSIPCallbackHandle())
            {
                return false;
            }
            this.setCallbackHandlers();
            return true;
        }

        public int createConference(IntPtr conferenceVideoWindow, VIDEO_RESOLUTION videoResolution, bool viewLocalVideoImage)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_createConference(_SIPCoreHandle, conferenceVideoWindow, (int)videoResolution, viewLocalVideoImage);
        }

        public int createConferenceEx(IntPtr conferenceVideoWindow, VIDEO_RESOLUTION videoResolution, bool viewLocalVideoImage, int[] sessionIds, int sessionIdNums)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_createConferenceEx(_SIPCoreHandle, conferenceVideoWindow, (int)videoResolution, viewLocalVideoImage, sessionIds, sessionIdNums);
        }

        private bool createSIPCallbackHandle()
        {
            _callbackHandle = PortSIP_NativeMethods.SIPEV_createSIPCallbackHandle();
            if (_callbackHandle == IntPtr.Zero)
            {
                return false;
            }
            return true;
        }

        public void destroyConference()
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_destroyConference(_SIPCoreHandle);
            }
        }

        public void detectMwi()
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_detectMwi(_SIPCoreHandle);
            }
        }

        public void disableCallForwarding()
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_disableCallForwarding(_SIPCoreHandle);
            }
        }

        public void disableSessionTimer()
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_disableSessionTimer(_SIPCoreHandle);
            }
        }

        public void enableAEC(bool state)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_enableAEC(_SIPCoreHandle, state);
            }
        }

        public void enableAGC(bool state)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_enableAGC(_SIPCoreHandle, state);
            }
        }

        public int enableAudioStreamCallback(int sessionId, bool enable, AUDIOSTREAM_CALLBACK_MODE callbackType)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_enableAudioStreamCallback(_SIPCoreHandle, sessionId, enable, (int)callbackType, _SIPCoreHandle, _arc);
        }

        public int enableCallForwarding(bool forBusyOnly, string forwardingTo)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_enableCallForwarding(_SIPCoreHandle, forBusyOnly, forwardingTo);
        }

        public void enableCheckMwi(bool state)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_enableCheckMwi(_SIPCoreHandle, state);
            }
        }

        public void enableCNG(bool state)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_enableCNG(_SIPCoreHandle, state);
            }
        }

        public void enableDTMFOfInfo()
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_enableDTMFOfInfo(_SIPCoreHandle);
            }
        }

        public void enableDTMFOfRFC2833(int RTPPayloadOf2833)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_enableDTMFOfRFC2833(_SIPCoreHandle, RTPPayloadOf2833);
            }
        }

        public int enableSendPcmStreamToRemote(int sessionId, bool state, int streamSamplesPerSec)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_enableSendPcmStreamToRemote(_SIPCoreHandle, sessionId, state, streamSamplesPerSec);
        }

        public int enableSessionTimer(int timerSeconds)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_enableSessionTimer(_SIPCoreHandle, timerSeconds);
        }

        public void enableStackLog(string logFilePath)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_enableStackLog(_SIPCoreHandle, logFilePath);
            }
        }

        public void enableVAD(bool state)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_enableVAD(_SIPCoreHandle, state);
            }
        }

        public int enableVideoStreamCallback(int sessionId, VIDEOSTREAM_CALLBACK_MODE callbackType)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_enableVideoStreamCallback(_SIPCoreHandle, sessionId, (int)callbackType, _SIPCoreHandle, _vrc);
        }

        public int forwardCall(int sessionId, string forwardTo)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_forwardCall(_SIPCoreHandle, sessionId, forwardTo);
        }

        public int getAudioRtpStatistics(int sessionId, out int averageJitterMs, out int maxJitterMs, out int discardedPackets)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                averageJitterMs = 0;
                maxJitterMs = 0;
                discardedPackets = 0;
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_getAudioRtpStatistics(_SIPCoreHandle, sessionId, out averageJitterMs, out maxJitterMs, out discardedPackets);
        }

        public void getDynamicVolumeLevel(out int speakerVolume, out int microphoneVolume)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                speakerVolume = 0;
                microphoneVolume = 0;
            }
            else
            {
                PortSIP_NativeMethods.PortSIP_getDynamicVolumeLevel(_SIPCoreHandle, out speakerVolume, out microphoneVolume);
            }
        }

        public int getExtensionHeaderValue(string sipMessage, string headerName, StringBuilder headerValue, int headerValueLength)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_getExtensionHeaderValue(_SIPCoreHandle, sipMessage, headerName, headerValue, headerValueLength);
        }

        public int getLocalIP(int index, StringBuilder ip, int ipLength)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_getLocalIP(_SIPCoreHandle, index, ip, ipLength);
        }

        public int getLocalIP6(int index, StringBuilder ip, int ipLength)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_getLocalIP6(_SIPCoreHandle, index, ip, ipLength);
        }

        public int getMicVolume()
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_getMicVolume(_SIPCoreHandle);
        }

        public int getNumOfPlayoutDevices()
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_getNumOfPlayoutDevices(_SIPCoreHandle);
        }

        public int getNumOfRecordingDevices()
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_getNumOfRecordingDevices(_SIPCoreHandle);
        }

        public int getNumOfVideoCaptureDevices()
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_getNumOfVideoCaptureDevices(_SIPCoreHandle);
        }

        public int getPlayoutDeviceName(int deviceIndex, StringBuilder nameUTF8, int nameUTF8Length)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_getPlayoutDeviceName(_SIPCoreHandle, deviceIndex, nameUTF8, nameUTF8Length);
        }

        public int getRecordingDeviceName(int deviceIndex, StringBuilder nameUTF8, int nameUTF8Length)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_getRecordingDeviceName(_SIPCoreHandle, deviceIndex, nameUTF8, nameUTF8Length);
        }

        public int getSpeakerVolume()
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_getSpeakerVolume(_SIPCoreHandle);
        }

        public bool getSystemInputMute()
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return false;
            }
            return PortSIP_NativeMethods.PortSIP_getSystemInputMute(_SIPCoreHandle);
        }

        public bool getSystemOutputMute()
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return false;
            }
            return PortSIP_NativeMethods.PortSIP_getSystemOutputMute(_SIPCoreHandle);
        }

        public void getVersion(out int majorVersion, out int minorVersion)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                majorVersion = 0;
                minorVersion = 0;
            }
            else
            {
                PortSIP_NativeMethods.PortSIP_getVersion(_SIPCoreHandle, out majorVersion, out minorVersion);
            }
        }

        public int getVideoCaptureDeviceName(int deviceIndex, StringBuilder uniqueIdUTF8, int uniqueIdUTF8Length, StringBuilder deviceNameUTF8, int deviceNameUTF8Length)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_getVideoCaptureDeviceName(_SIPCoreHandle, deviceIndex, uniqueIdUTF8, uniqueIdUTF8Length, deviceNameUTF8, deviceNameUTF8Length);
        }

        public int getVideoRtpStatistics(int sessionId, out int bytesSent, out int packetsSent, out int bytesReceived, out int packetsReceived)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                bytesSent = 0;
                packetsSent = 0;
                bytesReceived = 0;
                packetsReceived = 0;
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_getVideoRtpStatistics(_SIPCoreHandle, sessionId, out bytesSent, out packetsSent, out bytesReceived, out packetsReceived);
        }

        public int hold(int sessionId)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_hold(_SIPCoreHandle, sessionId);
        }

        public bool initialize(TRANSPORT_TYPE transportType, PORTSIP_LOG_LEVEL appLogLevel, string logFilePath, int maximumLines, string agent, string STUNSever, int STUNServerPort, out int errorCode)
        {
            if ((_callbackHandle == IntPtr.Zero) || (_SIPCoreHandle != IntPtr.Zero))
            {
                errorCode = PortSIP_Errors.ECoreSDKObjectNull;
                return false;
            }
            _SIPCoreHandle = PortSIP_NativeMethods.PortSIP_initialize(_callbackHandle, (int)transportType, (int)appLogLevel, logFilePath, maximumLines, agent, STUNSever, STUNServerPort, out errorCode);
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return false;
            }
            return true;
        }

        public bool isAudioCodecEmpty()
        {
            return ((_SIPCoreHandle == IntPtr.Zero) || PortSIP_NativeMethods.PortSIP_isAudioCodecEmpty(_SIPCoreHandle));
        }

        public bool isVideoCodecEmpty()
        {
            return ((_SIPCoreHandle == IntPtr.Zero) || PortSIP_NativeMethods.PortSIP_isVideoCodecEmpty(_SIPCoreHandle));
        }

        public int joinToConference(int sessionId)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_joinToConference(_SIPCoreHandle, sessionId);
        }

        public int modifyHeaderValue(string headerName, string headerValue)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_modifyHeaderValue(_SIPCoreHandle, headerName, headerValue);
        }

        public void muteMicrophone(bool mute)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_muteMicrophone(_SIPCoreHandle, mute);
            }
        }

        public void muteSpeaker(bool mute)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_muteSpeaker(_SIPCoreHandle, mute);
            }
        }

        private static int onACTVTransferFailure(int callbackObject, int sessionId, int statusCode, string statusText)
        {
            _SIPCallbackEvents.onACTVTransferFailure(callbackObject, sessionId, statusCode, statusText);
            return 0;
        }

        private static int onACTVTransferSuccess(int callbackObject, int sessionId)
        {
            _SIPCallbackEvents.onACTVTransferSuccess(callbackObject, sessionId);
            return 0;
        }

        private static int onArrivedSignaling(int callbackObject, int sessionId, StringBuilder signaling)
        {
            _SIPCallbackEvents.onArrivedSignaling(callbackObject, sessionId, signaling);
            return 0;
        }

        private static int onAudioRawCallback(IntPtr callbackObject, int sessionId, int callbackType, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] byte[] data, int dataLength, int samplingFreqHz)
        {
            _SIPCallbackEvents.onAudioRawCallback(callbackObject, sessionId, callbackType, data, dataLength, samplingFreqHz);
            return 0;
        }

        private static int onInviteAnswered(int callbackObject, int sessionId, bool hasVideo, int statusCode, string statusText, string audioCodecName, string videoCodecName)
        {
            _SIPCallbackEvents.onInviteAnswered(callbackObject, sessionId, hasVideo, statusCode, statusText, audioCodecName, videoCodecName);
            return 0;
        }

        private static int onInviteBeginingForward(int callbackObject, string forwardingTo)
        {
            _SIPCallbackEvents.onInviteBeginingForward(callbackObject, forwardingTo);
            return 0;
        }

        private static int onInviteClosed(int callbackObject, int sessionId)
        {
            _SIPCallbackEvents.onInviteClosed(callbackObject, sessionId);
            return 0;
        }

        private static int onInviteFailure(int callbackObject, int sessionId, int statusCode, string statusText)
        {
            _SIPCallbackEvents.onInviteFailure(callbackObject, sessionId, statusCode, statusText);
            return 0;
        }

        private static int onInviteIncoming(int callbackObject, int sessionId, string caller, string callerDisplayName, string callee, string calleeDisplayName, string audioCodecName, string videoCodecName, bool hasVideo)
        {
            _SIPCallbackEvents.onInviteIncoming(callbackObject, sessionId, caller, callerDisplayName, callee, calleeDisplayName, audioCodecName, videoCodecName, hasVideo);
            return 0;
        }

        private static int onInviteRinging(int callbackObject, int sessionId, bool hasEarlyMedia, bool hasVideo, string audioCodecName, string videoCodecName)
        {
            _SIPCallbackEvents.onInviteRinging(callbackObject, sessionId, hasEarlyMedia, hasVideo, audioCodecName, videoCodecName);
            return 0;
        }

        private static int onInviteTrying(int callbackObject, int sessionId, string caller, string callee)
        {
            _SIPCallbackEvents.onInviteTrying(callbackObject, sessionId, caller, callee);
            return 0;
        }

        private static int onInviteUACConnected(int callbackObject, int sessionId, int statusCode, string statusText)
        {
            _SIPCallbackEvents.onInviteUACConnected(callbackObject, sessionId, statusCode, statusText);
            return 0;
        }

        private static int onInviteUASConnected(int callbackObject, int sessionId, int statusCode, string statusText)
        {
            _SIPCallbackEvents.onInviteUASConnected(callbackObject, sessionId, statusCode, statusText);
            return 0;
        }

        private static int onInviteUpdated(int callbackObject, int sessionId, bool hasVideo, string audioCodecName, string videoCodecName)
        {
            _SIPCallbackEvents.onInviteUpdated(callbackObject, sessionId, hasVideo, audioCodecName, videoCodecName);
            return 0;
        }

        private static int onPASVTransferFailure(int callbackObject, int sessionId, int statusCode, string statusText)
        {
            _SIPCallbackEvents.onPASVTransferFailure(callbackObject, sessionId, statusCode, statusText);
            return 0;
        }

        private static int onPASVTransferSuccess(int callbackObject, int sessionId, bool hasVideo)
        {
            _SIPCallbackEvents.onPASVTransferSuccess(callbackObject, sessionId, hasVideo);
            return 0;
        }

        private static int onPlayAviFileFinished(IntPtr callbackObject, int sessionId)
        {
            _SIPCallbackEvents.onPlayAviFileFinished(callbackObject, sessionId);
            return 0;
        }

        private static int onPlayWaveFileFinished(IntPtr callbackObject, int sessionId, string fileName)
        {
            _SIPCallbackEvents.onPlayWaveFileFinished(callbackObject, sessionId, fileName);
            return 0;
        }

        private static int onPresenceOffline(int callbackObject, string from, string fromDisplayName)
        {
            _SIPCallbackEvents.onPresenceOffline(callbackObject, from, fromDisplayName);
            return 0;
        }

        private static int onPresenceOnline(int callbackObject, string from, string fromDisplayName, string stateText)
        {
            _SIPCallbackEvents.onPresenceOnline(callbackObject, from, fromDisplayName, stateText);
            return 0;
        }

        private static int onPresenceRecvSubscribe(int callbackObject, int subscribeId, string from, string fromDisplayName, string subject)
        {
            _SIPCallbackEvents.onPresenceRecvSubscribe(callbackObject, subscribeId, from, fromDisplayName, subject);
            return 0;
        }

        private static int onRecvBinaryMessage(int callbackObject, int sessionId, StringBuilder message, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] byte[] messageBody, int length)
        {
            _SIPCallbackEvents.onRecvBinaryMessage(callbackObject, sessionId, message, messageBody, length);
            return 0;
        }

        private static int onRecvBinaryPagerMessage(int callbackObject, StringBuilder from, StringBuilder fromDisplayName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] byte[] messageBody, int length)
        {
            _SIPCallbackEvents.onRecvBinaryPagerMessage(callbackObject, from, fromDisplayName, messageBody, length);
            return 0;
        }

        private static int onRecvDtmfTone(int callbackObject, int sessionId, int tone)
        {
            _SIPCallbackEvents.onRecvDtmfTone(callbackObject, sessionId, tone);
            return 0;
        }

        private static int onRecvInfo(int callbackObject, int sessionId, StringBuilder infoMessage)
        {
            _SIPCallbackEvents.onRecvInfo(callbackObject, sessionId, infoMessage);
            return 0;
        }

        private static int onRecvMessage(int callbackObject, int sessionId, StringBuilder message)
        {
            _SIPCallbackEvents.onRecvMessage(callbackObject, sessionId, message);
            return 0;
        }

        private static int onRecvOptions(int callbackObject, StringBuilder optionsMessage)
        {
            _SIPCallbackEvents.onRecvOptions(callbackObject, optionsMessage);
            return 0;
        }

        private static int onRecvPagerMessage(int callbackObject, string from, string fromDisplayName, StringBuilder message)
        {
            _SIPCallbackEvents.onRecvPagerMessage(callbackObject, from, fromDisplayName, message);
            return 0;
        }

        private static int onRegisterFailure(int callbackObject, int statusCode, string statusText)
        {
            _SIPCallbackEvents.onRegisterFailure(callbackObject, statusCode, statusText);
            return 0;
        }

        private static int onRegisterSuccess(int callbackObject, int statusCode, string statusText)
        {
            _SIPCallbackEvents.onRegisterSuccess(callbackObject, statusCode, statusText);
            return 0;
        }

        private static int onRemoteHold(int callbackObject, int sessionId)
        {
            _SIPCallbackEvents.onRemoteHold(callbackObject, sessionId);
            return 0;
        }

        private static int onRemoteUnHold(int callbackObject, int sessionId)
        {
            _SIPCallbackEvents.onRemoteUnHold(callbackObject, sessionId);
            return 0;
        }

        private static int onSendPagerMessageFailure(int callbackObject, string caller, string callerDisplayName, string callee, string calleeDisplayName, int statusCode, string statusText)
        {
            _SIPCallbackEvents.onSendPagerMessageFailure(callbackObject, caller, callerDisplayName, callee, calleeDisplayName, statusCode, statusText);
            return 0;
        }

        private static int onSendPagerMessageSuccess(int callbackObject, string caller, string callerDisplayName, string callee, string calleeDisplayName)
        {
            _SIPCallbackEvents.onSendPagerMessageSuccess(callbackObject, caller, callerDisplayName, callee, calleeDisplayName);
            return 0;
        }

        private static int onTransferRinging(int callbackObject, int sessionId, bool hasVideo)
        {
            _SIPCallbackEvents.onTransferRinging(callbackObject, sessionId, hasVideo);
            return 0;
        }

        private static int onTransferTrying(int callbackObject, int sessionId, string referTo)
        {
            _SIPCallbackEvents.onTransferTrying(callbackObject, sessionId, referTo);
            return 0;
        }

        private static int onVideoRawCallback(IntPtr callbackObject, int sessionId, int callbackType, int width, int height, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 6)] byte[] data, int dataLength)
        {
            _SIPCallbackEvents.onVideoRawCallback(callbackObject, sessionId, callbackType, width, height, data, dataLength);
            return 0;
        }

        private static int onWaitingFaxMessage(int callbackObject, string messageAccount, int urgentNewMessageCount, int urgentOldMessageCount, int newMessageCount, int oldMessageCount)
        {
            _SIPCallbackEvents.onWaitingFaxMessage(callbackObject, messageAccount, urgentNewMessageCount, urgentOldMessageCount, newMessageCount, oldMessageCount);
            return 0;
        }

        private static int onWaitingVoiceMessage(int callbackObject, string messageAccount, int urgentNewMessageCount, int urgentOldMessageCount, int newMessageCount, int oldMessageCount)
        {
            _SIPCallbackEvents.onWaitingVoiceMessage(callbackObject, messageAccount, urgentNewMessageCount, urgentOldMessageCount, newMessageCount, oldMessageCount);
            return 0;
        }

        public int playLocalWaveFile(string waveFile, bool loop)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_playLocalWaveFile(_SIPCoreHandle, waveFile, loop);
        }

        public int PortSIP_getAudioRtcpStatistics(int sessionId, out int bytesSent, out int packetsSent, out int bytesReceived, out int packetsReceived)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                bytesSent = 0;
                packetsSent = 0;
                bytesReceived = 0;
                packetsReceived = 0;
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_getAudioRtcpStatistics(_SIPCoreHandle, sessionId, out bytesSent, out packetsSent, out bytesReceived, out packetsReceived);
        }

        public int presenceAcceptSubscribe(int subscribeId)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_presenceAcceptSubscribe(_SIPCoreHandle, subscribeId);
        }

        public int presenceOffline(int subscribeId)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_presenceOffline(_SIPCoreHandle, subscribeId);
        }

        public int presenceOnline(int subscribeId, string stateText)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_presenceOnline(_SIPCoreHandle, subscribeId, stateText);
        }

        public int presenceRejectSubscribe(int subscribeId)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_presenceRejectSubscribe(_SIPCoreHandle, subscribeId);
        }

        public int presenceSubscribeContact(string contact, string subject)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_presenceSubscribeContact(_SIPCoreHandle, contact, subject);
        }

        public int refer(int sessionId, string referTo)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_refer(_SIPCoreHandle, sessionId, referTo);
        }

        public int registerServer(int expires)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_registerServer(_SIPCoreHandle, expires);
        }

        public int rejectCall(int sessionId, int code, string reason)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_rejectCall(_SIPCoreHandle, sessionId, code, reason);
        }

        public void releaseCallbackHandlers()
        {
            if (_callbackHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.SIPEV_releaseSIPCallbackHandle(_callbackHandle);
                _callbackHandle = IntPtr.Zero;
            }
        }

        public int removeFromConference(int sessionId)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_removeFromConference(_SIPCoreHandle, sessionId);
        }

        public int sendDtmf(int sessionId, char code)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_sendDTMF(_SIPCoreHandle, sessionId, code);
        }

        public int sendInfo(int sessionId, string mimeType, string subMimeType, string infoContents)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_sendInfo(_SIPCoreHandle, sessionId, mimeType, subMimeType, infoContents);
        }

        public int sendMessage(int sessionId, string mimeType, string subMimeType, string message)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_sendMessage(_SIPCoreHandle, sessionId, mimeType, subMimeType, message);
        }

        public int sendMessageEx(int sessionId, string mimeType, string subMimeType, byte[] message, int messageLength)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_sendMessageEx(_SIPCoreHandle, sessionId, mimeType, subMimeType, message, messageLength);
        }

        public int sendOptions(string to, string sdp)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_sendOptions(_SIPCoreHandle, to, sdp);
        }

        public int sendOutOfDialogMessage(string to, string mimeType, string subMimeType, string message)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_sendOutOfDialogMessage(_SIPCoreHandle, to, mimeType, subMimeType, message);
        }

        public int sendOutOfDialogMessageEx(string to, string mimeType, string subMimeType, byte[] message, int messageLength)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_sendOutOfDialogMessageEx(_SIPCoreHandle, to, mimeType, subMimeType, message, messageLength);
        }

        public int sendPagerMessage(string to, string message)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_sendPagerMessage(_SIPCoreHandle, to, message);
        }

        public int sendPcmStreamToRemote(int sessionId, byte[] data, int dataLength, int dataSamplesPerSec)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_sendPcmStreamToRemote(_SIPCoreHandle, sessionId, data, dataLength);
        }

        public int setAudioCodecParameter(AUDIOCODEC_TYPE codecType, string parameter)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_setAudioCodecParameter(_SIPCoreHandle, (int)codecType, parameter);
        }

        public void setAudioDeviceId(int recordingDeviceId, int playoutDeviceId)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_setAudioDeviceId(_SIPCoreHandle, recordingDeviceId, playoutDeviceId);
            }
        }

        public int setAudioQos(bool state, int DSCPValue, int priority)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_setAudioQos(_SIPCoreHandle, state, DSCPValue, priority);
        }

        public int setAudioSamples(int samples)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_setAudioSamples(_SIPCoreHandle, samples);
        }

        private void setCallbackHandlers()
        {
            PortSIP_NativeMethods.SIPEV_setRegisterSuccessHandler(_callbackHandle, _rgs, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setRegisterFailureHandler(_callbackHandle, _rgf, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setInviteIncomingHandler(_callbackHandle, _ivi, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setInviteTryingHandler(_callbackHandle, _ivt, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setInviteRingingHandler(_callbackHandle, _ivr, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setInviteAnsweredHandler(_callbackHandle, _iva, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setInviteFailureHandler(_callbackHandle, _ivf, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setInviteClosedHandler(_callbackHandle, _ivc, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setInviteUpdatedHandler(_callbackHandle, _ivu, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setInviteUASConnectedHandler(_callbackHandle, _ivsc, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setInviteUACConnectedHandler(_callbackHandle, _ivcc, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setInviteBeginingForwardHandler(_callbackHandle, _ivbf, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setRemoteHoldHandler(_callbackHandle, _rth, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setRemoteUnHoldHandler(_callbackHandle, _rtu, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setTransferTryingHandler(_callbackHandle, _tst, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setTransferRingingHandler(_callbackHandle, _tsr, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setPASVTransferSuccessHandler(_callbackHandle, _pts, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setACTVTransferSuccessHandler(_callbackHandle, _ats, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setPASVTransferFailureHandler(_callbackHandle, _ptf, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setACTVTransferFailureHandler(_callbackHandle, _atf, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setRecvPagerMessageHandler(_callbackHandle, _rpm, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setSendPagerMessageSuccessHandler(_callbackHandle, _spms, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setSendPagerMessageFailureHandler(_callbackHandle, _spmf, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setArrivedSignalingHandler(_callbackHandle, _asg, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setWaitingVoiceMessageHandler(_callbackHandle, _wvm, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setWaitingFaxMessageHandler(_callbackHandle, _wfm, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setRecvDtmfToneHandler(_callbackHandle, _rdt, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setPresenceRecvSubscribeHandler(_callbackHandle, _prs, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setPresenceOnlineHandler(_callbackHandle, _pon, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setPresenceOfflineHandler(_callbackHandle, _pof, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setRecvOptionsHandler(_callbackHandle, _ro, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setRecvInfoHandler(_callbackHandle, _ri, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setRecvMessageHandler(_callbackHandle, _rms, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setRecvBinaryMessageHandler(_callbackHandle, _rbm, this._callbackObject);
            PortSIP_NativeMethods.SIPEV_setRecvBinaryPagerMessageHandler(_callbackHandle, _rbpm, this._callbackObject);
        }

        public void setDtmfSamples(int samples)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_setDTMFSamples(_SIPCoreHandle, samples);
            }
        }

        public void setKeepAliveTime(int keepAliveTime)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_setKeepAliveTime(_SIPCoreHandle, keepAliveTime);
            }
        }

        public void setLicenseKey(string key)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_setLicenseKey(_SIPCoreHandle, key);
            }
        }

        public void setLocalVideoWindow(IntPtr localVideoWindow)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_setLocalVideoWindow(_SIPCoreHandle, localVideoWindow);
            }
        }

        public int setMicVolume(int volume)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_setMicVolume(_SIPCoreHandle, volume);
        }

        public int setPlayAviFileToRemote(int sessionId, string aviFile, bool enableState, bool loop, bool playAudio, bool eventReport)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            if (eventReport)
            {
                return PortSIP_NativeMethods.PortSIP_setPlayAviFileToRemote(_SIPCoreHandle, sessionId, aviFile, enableState, loop, playAudio, _SIPCoreHandle, _paf);
            }
            return PortSIP_NativeMethods.PortSIP_setPlayAviFileToRemote(_SIPCoreHandle, sessionId, aviFile, enableState, loop, playAudio, _SIPCoreHandle, null);
        }

        public void setPlayWaveFileToRemote(int sessionId, string waveFile, bool enableState, int playMode, int fileSamplesPerSec, bool eventReport)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                if (eventReport)
                {
                    PortSIP_NativeMethods.PortSIP_setPlayWaveFileToRemote(_SIPCoreHandle, sessionId, waveFile, enableState, playMode, fileSamplesPerSec, _SIPCoreHandle, _pwf);
                }
                else
                {
                    PortSIP_NativeMethods.PortSIP_setPlayWaveFileToRemote(_SIPCoreHandle, sessionId, waveFile, enableState, playMode, fileSamplesPerSec, _SIPCoreHandle, null);
                }
            }
        }

        public int setRemoteVideoWindow(int sessionId, IntPtr remoteVideoWindow)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_setRemoteVideoWindow(_SIPCoreHandle, sessionId, remoteVideoWindow);
        }

        public void setRtpKeepAlive(bool state, int keepAlivePayloadType, int deltaTransmitTimeMS)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_setRtpKeepAlive(_SIPCoreHandle, state, keepAlivePayloadType, deltaTransmitTimeMS);
            }
        }

        public int setRtpPortRange(int minimumRtpAudioPort, int maximumRtpAudioPort, int minimumRtpVideoPort, int maximumRtpVideoPort)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_setRtpPortRange(_SIPCoreHandle, minimumRtpAudioPort, maximumRtpAudioPort, minimumRtpVideoPort, maximumRtpVideoPort);
        }

        public int setSpeakerVolume(int volume)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_setSpeakerVolume(_SIPCoreHandle, volume);
        }

        public void setSrtpPolicy(SRTP_POLICY srtPolicy)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_setSrtpPolicy(_SIPCoreHandle, (int)srtPolicy);
            }
        }

        public int setSystemInputMute(bool enable)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_setSystemInputMute(_SIPCoreHandle, enable);
        }

        public int setSystemOutputMute(bool enable)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_setSystemOutputMute(_SIPCoreHandle, enable);
        }

        public int setUserInfo(string userName, string displayName, string authName, string password, string localIp, int localSipPort, string userDomain, string SIPServer, int SIPServerPort, string outboundServer, int outboundServerPort)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_setUserInfo(_SIPCoreHandle, userName, displayName, authName, password, localIp, localSipPort, userDomain, SIPServer, SIPServerPort, outboundServer, outboundServerPort);
        }

        public void setVideoBitrate(int bitrateKbps)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_setVideoBitrate(_SIPCoreHandle, bitrateKbps);
            }
        }

        public int setVideoCodecParameter(VIDEOCODEC_TYPE codecType, string parameter)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_setVideoCodecParameter(_SIPCoreHandle, (int)codecType, parameter);
        }

        public void setVideoDeviceId(int deviceId)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_setVideoDeviceId(_SIPCoreHandle, deviceId);
            }
        }

        public void setVideoFrameRate(int frameRate)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_setVideoFrameRate(_SIPCoreHandle, frameRate);
            }
        }

        public int setVideoQos(bool state, int DSCPValue)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_setVideoQos(_SIPCoreHandle, state, DSCPValue);
        }

        public void setVideoResolution(VIDEO_RESOLUTION resolution)
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_setVideoResolution(_SIPCoreHandle, (int)resolution);
            }
        }

        public int showVideoCaptureSettingsDialogBox(string uniqueIdUTF8, int uniqueIdUTF8Length, string dialogTitle, IntPtr parentWindow, int x, int y)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_showVideoCaptureSettingsDialogBox(_SIPCoreHandle, uniqueIdUTF8, uniqueIdUTF8Length, dialogTitle, parentWindow, x, y);
        }

        public void shutdownCallbackHandlers()
        {
            if (_callbackHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.SIPEV_shutdownSIPCallbackHandle(_callbackHandle);
            }
        }

        public int startAudioRecording(string filePath, string fileName, bool appendTimestamp, AUDIO_RECORDING_FILEFORMAT fileFormat)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_startAudioRecording(_SIPCoreHandle, filePath, fileName, appendTimestamp, (int)fileFormat);
        }

        public int startVideoRecording(string filePath, string fileName, bool appendTimestamp, VIDEOCODEC_TYPE codecType)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_startVideoRecording(_SIPCoreHandle, filePath, fileName, appendTimestamp, (int)codecType);
        }

        public int startVideoSending(int sessionId, bool state)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_startVideoSending(_SIPCoreHandle, sessionId, state);
        }

        public int stopAudioRecording()
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_stopAudioRecording(_SIPCoreHandle);
        }

        public int stopVideoRecording()
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_stopVideoRecording(_SIPCoreHandle);
        }

        public int terminateCall(int sessionId)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_terminateCall(_SIPCoreHandle, sessionId);
        }

        public int unHold(int sessionId)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_unHold(_SIPCoreHandle, sessionId);
        }

        public void unInitialize()
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_unInitialize(_SIPCoreHandle);
                _SIPCoreHandle = IntPtr.Zero;
            }
        }

        public void unRegisterServer()
        {
            if (_SIPCoreHandle != IntPtr.Zero)
            {
                PortSIP_NativeMethods.PortSIP_unRegisterServer(_SIPCoreHandle);
            }
        }

        public int updateInvite(int sessionId)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_updateInvite(_SIPCoreHandle, sessionId);
        }

        public int viewLocalVideo(bool state)
        {
            if (_SIPCoreHandle == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }
            return PortSIP_NativeMethods.PortSIP_viewLocalVideo(_SIPCoreHandle, state);
        }
    }
}