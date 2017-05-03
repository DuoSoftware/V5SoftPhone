//////////////////////////////////////////////////////////////////////////
//
// IMPORTANT: DON'T EDIT THIS FILE
//
//////////////////////////////////////////////////////////////////////////


using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DuoSoftware.DuoSoftPhone.Controllers.PortSIP
{
    public unsafe class PortSIP_NativeMethods
    {
        // Methods
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_addAudioCodec(IntPtr SIPCoreHandle, int codecType);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_addExtensionHeader(IntPtr SIPCoreHandle, string headerName, string headerValue);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_addSupportedMimeType(IntPtr SIPCoreHandle, string methodName, string mimeType, string subMimeType);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_addVideoCodec(IntPtr SIPCoreHandle, int codecType);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_answerCall(IntPtr SIPCoreHandle, int sessionId);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_attendedRefer(IntPtr SIPCoreHandle, int sessionId, int replaceSessionId, string referTo);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_audioPlayLoopbackTest(IntPtr SIPCoreHandle, [MarshalAs(UnmanagedType.I1)] bool enable);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_call(IntPtr SIPCoreHandle, string callTo, [MarshalAs(UnmanagedType.I1)] bool sendSdp, out int errorCode);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_clearAddExtensionHeaders(IntPtr SIPCoreHandle);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_clearAudioCodec(IntPtr SIPCoreHandle);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_clearModifyHeaders(IntPtr SIPCoreHandle);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_clearVideoCodec(IntPtr SIPCoreHandle);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_createConference(IntPtr SIPCoreHandle, IntPtr conferenceVideoWindow, int videoResolution, [MarshalAs(UnmanagedType.I1)] bool viewLocalVideoImage);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_createConferenceEx(IntPtr SIPCoreHandle, IntPtr conferenceVideoWindow, int videoResolution, [MarshalAs(UnmanagedType.I1)] bool viewLocalVideoImage, [MarshalAs(UnmanagedType.LPArray)] int[] sessionIds, int sessionIdNums);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_destroyConference(IntPtr SIPCoreHandle);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_detectMwi(IntPtr SIPCoreHandle);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_disableCallForwarding(IntPtr SIPCoreHandle);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_disableSessionTimer(IntPtr SIPCoreHandle);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_enableAEC(IntPtr SIPCoreHandle, [MarshalAs(UnmanagedType.I1)] bool state);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_enableAGC(IntPtr SIPCoreHandle, [MarshalAs(UnmanagedType.I1)] bool state);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_enableAudioStreamCallback(IntPtr SIPCoreHandle, int sessionId, [MarshalAs(UnmanagedType.I1)] bool enable, int callbackType, IntPtr reserve, audioRawCallback callbackHandler);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_enableCallForwarding(IntPtr SIPCoreHandle, [MarshalAs(UnmanagedType.I1)] bool forBusyOnly, string forwardingTo);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_enableCheckMwi(IntPtr SIPCoreHandle, [MarshalAs(UnmanagedType.I1)] bool state);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_enableCNG(IntPtr SIPCoreHandle, [MarshalAs(UnmanagedType.I1)] bool state);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_enableDTMFOfInfo(IntPtr SIPCoreHandle);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_enableDTMFOfRFC2833(IntPtr SIPCoreHandle, int RTPPayloadOf2833);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_enableSendPcmStreamToRemote(IntPtr SIPCoreHandle, int sessionId, [MarshalAs(UnmanagedType.I1)] bool state, int streamSamplesPerSec);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_enableSessionTimer(IntPtr SIPCoreHandle, int timerSeconds);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_enableStackLog(IntPtr SIPCoreHandle, string logFilePath);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_enableVAD(IntPtr SIPCoreHandle, [MarshalAs(UnmanagedType.I1)] bool state);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_enableVideoStreamCallback(IntPtr SIPCoreHandle, int sessionId, int callbackType, IntPtr reserve, videoRawCallback callbackHandler);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_forwardCall(IntPtr SIPCoreHandle, int sessionId, string forwardTo);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_getAudioRtcpStatistics(IntPtr SIPCoreHandle, int sessionId, out int bytesSent, out int packetsSent, out int bytesReceived, out int packetsReceived);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_getAudioRtpStatistics(IntPtr SIPCoreHandle, int sessionId, out int averageJitterMs, out int maxJitterMs, out int discardedPackets);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_getDynamicVolumeLevel(IntPtr SIPCoreHandle, out int speakerVolume, out int microphoneVolume);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_getExtensionHeaderValue(IntPtr SIPCoreHandle, string sipMessage, string headerName, StringBuilder headerValue, int headerValueLength);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_getLocalIP(IntPtr SIPCoreHandle, int index, StringBuilder ip, int ipLength);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_getLocalIP6(IntPtr SIPCoreHandle, int index, StringBuilder ip, int ipLength);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_getMicVolume(IntPtr SIPCoreHandle);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_getNumOfPlayoutDevices(IntPtr SIPCoreHandle);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_getNumOfRecordingDevices(IntPtr SIPCoreHandle);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_getNumOfVideoCaptureDevices(IntPtr SIPCoreHandle);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_getPlayoutDeviceName(IntPtr SIPCoreHandle, int deviceIndex, StringBuilder nameUTF8, int nameUTF8Length);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_getRecordingDeviceName(IntPtr SIPCoreHandle, int deviceIndex, StringBuilder nameUTF8, int nameUTF8Length);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_getSpeakerVolume(IntPtr SIPCoreHandle);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern bool PortSIP_getSystemInputMute(IntPtr SIPCoreHandle);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern bool PortSIP_getSystemOutputMute(IntPtr SIPCoreHandle);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_getVersion(IntPtr SIPCoreHandle, out int majorVersion, out int minorVersion);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_getVideoCaptureDeviceName(IntPtr SIPCoreHandle, int deviceIndex, StringBuilder uniqueIdUTF8, int uniqueIdUTF8Length, StringBuilder deviceNameUTF8, int deviceNameUTF8Length);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_getVideoRtpStatistics(IntPtr SIPCoreHandle, int sessionId, out int bytesSent, out int packetsSent, out int bytesReceived, out int packetsReceived);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_hold(IntPtr SIPCoreHandle, int sessionId);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern IntPtr PortSIP_initialize(IntPtr callbackEvent, int transportType, int appLogLevel, string logFilePath, int maximumLines, string agent, string STUNSever, int STUNServerPort, out int errorCode);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern bool PortSIP_isAudioCodecEmpty(IntPtr SIPCoreHandle);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern bool PortSIP_isVideoCodecEmpty(IntPtr SIPCoreHandle);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_joinToConference(IntPtr SIPCoreHandle, int sessionId);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_modifyHeaderValue(IntPtr SIPCoreHandle, string headerName, string headerValue);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_muteMicrophone(IntPtr SIPCoreHandle, [MarshalAs(UnmanagedType.I1)] bool mute);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_muteSpeaker(IntPtr SIPCoreHandle, [MarshalAs(UnmanagedType.I1)] bool mute);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_playLocalWaveFile(IntPtr SIPCoreHandle, string waveFile, [MarshalAs(UnmanagedType.I1)] bool loop);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_presenceAcceptSubscribe(IntPtr SIPCoreHandle, int subscribeId);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_presenceOffline(IntPtr SIPCoreHandle, int subscribeId);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_presenceOnline(IntPtr SIPCoreHandle, int subscribeId, string stateText);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_presenceRejectSubscribe(IntPtr SIPCoreHandle, int subscribeId);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_presenceSubscribeContact(IntPtr SIPCoreHandle, string contact, string subject);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_refer(IntPtr SIPCoreHandle, int sessionId, string referTo);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_registerServer(IntPtr SIPCoreHandle, int expires);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_rejectCall(IntPtr SIPCoreHandle, int sessionId, int code, string reason);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_removeFromConference(IntPtr SIPCoreHandle, int sessionId);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_sendDTMF(IntPtr SIPCoreHandle, int sessionId, char code);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_sendInfo(IntPtr SIPCoreHandle, int sessionId, string mimeType, string subMimeType, string infoContents);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_sendMessage(IntPtr SIPCoreHandle, int sessionId, string mimeType, string subMimeType, string message);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_sendMessageEx(IntPtr SIPCoreHandle, int sessionId, string mimeType, string subMimeType, [MarshalAs(UnmanagedType.LPArray)] byte[] message, int messageLength);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_sendOptions(IntPtr SIPCoreHandle, string to, string sdp);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_sendOutOfDialogMessage(IntPtr SIPCoreHandle, string to, string mimeType, string subMimeType, string message);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_sendOutOfDialogMessageEx(IntPtr SIPCoreHandle, string to, string mimeType, string subMimeType, [MarshalAs(UnmanagedType.LPArray)] byte[] message, int messageLength);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_sendPagerMessage(IntPtr SIPCoreHandle, string to, string message);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_sendPcmStreamToRemote(IntPtr SIPCoreHandle, int sessionId, [MarshalAs(UnmanagedType.LPArray)] byte[] data, int dataLength);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_setAudioCodecParameter(IntPtr SIPCoreHandle, int codecType, string parameter);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_setAudioDeviceId(IntPtr SIPCoreHandle, int recordingDeviceId, int playoutDeviceId);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_setAudioQos(IntPtr SIPCoreHandle, [MarshalAs(UnmanagedType.I1)] bool state, int DSCPValue, int priority);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_setAudioSamples(IntPtr SIPCoreHandle, int samples);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_setDTMFSamples(IntPtr SIPCoreHandle, int samples);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_setKeepAliveTime(IntPtr SIPCoreHandle, int keepAliveTime);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_setLicenseKey(IntPtr SIPCoreHandle, string key);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_setLocalVideoWindow(IntPtr SIPCoreHandle, IntPtr localVideoWindow);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_setMicVolume(IntPtr SIPCoreHandle, int volume);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_setPlayAviFileToRemote(IntPtr SIPCoreHandle, int sessionId, string aviFile, [MarshalAs(UnmanagedType.I1)] bool enableState, [MarshalAs(UnmanagedType.I1)] bool loop, [MarshalAs(UnmanagedType.I1)] bool playAudio, IntPtr reserve, playAviFileToRemoteFinished callbackHandler);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_setPlayWaveFileToRemote(IntPtr SIPCoreHandle, int sessionId, string waveFile, [MarshalAs(UnmanagedType.I1)] bool enableState, int playMode, int fileSamplesPerSec, IntPtr reserve, playWaveFileToRemoteFinished callbackHandler);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_setRemoteVideoWindow(IntPtr SIPCoreHandle, int sessionId, IntPtr remoteVideoWindow);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_setRtpKeepAlive(IntPtr SIPCoreHandle, [MarshalAs(UnmanagedType.I1)] bool state, int keepAlivePayloadType, int deltaTransmitTimeMS);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_setRtpPortRange(IntPtr SIPCoreHandle, int minimumRtpAudioPort, int maximumRtpAudioPort, int minimumRtpVideoPort, int maximumRtpVideoPort);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_setSpeakerVolume(IntPtr SIPCoreHandle, int volume);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_setSrtpPolicy(IntPtr SIPCoreHandle, int srtPolicy);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_setSystemInputMute(IntPtr SIPCoreHandle, [MarshalAs(UnmanagedType.I1)] bool enable);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_setSystemOutputMute(IntPtr SIPCoreHandle, [MarshalAs(UnmanagedType.I1)] bool enable);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_setUserInfo(IntPtr SIPCoreLib, string userName, string displayName, string authName, string password, string localIp, int localSipPort, string userDomain, string SIPServer, int SIPServerPort, string outboundServer, int outboundServerPort);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_setVideoBitrate(IntPtr SIPCoreHandle, int bitrateKbps);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_setVideoCodecParameter(IntPtr SIPCoreHandle, int codecType, string parameter);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_setVideoDeviceId(IntPtr SIPCoreHandle, int deviceId);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_setVideoFrameRate(IntPtr SIPCoreHandle, int frameRate);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_setVideoQos(IntPtr SIPCoreHandle, [MarshalAs(UnmanagedType.I1)] bool state, int DSCPValue);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_setVideoResolution(IntPtr SIPCoreHandle, int resolution);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_showVideoCaptureSettingsDialogBox(IntPtr SIPCoreHandle, string uniqueIdUTF8, int uniqueIdUTF8Length, string dialogTitle, IntPtr parentWindow, int x, int y);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_startAudioRecording(IntPtr SIPCoreHandle, string filePath, string fileName, [MarshalAs(UnmanagedType.I1)] bool appendTimestamp, int fileFormat);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_startVideoRecording(IntPtr SIPCoreHandle, string filePath, string fileName, [MarshalAs(UnmanagedType.I1)] bool appendTimestamp, int codecType);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_startVideoSending(IntPtr SIPCoreHandle, int sessionId, [MarshalAs(UnmanagedType.I1)] bool state);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_stopAudioRecording(IntPtr SIPCoreHandle);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_stopVideoRecording(IntPtr SIPCoreHandle);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_terminateCall(IntPtr SIPCoreHandle, int sessionId);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_unHold(IntPtr SIPCoreHandle, int sessionId);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_unInitialize(IntPtr SIPCoreHandle);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void PortSIP_unRegisterServer(IntPtr SIPCoreHandle);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_updateInvite(IntPtr SIPCoreHandle, int sessionId);
        [DllImport("PortSIPCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int PortSIP_viewLocalVideo(IntPtr SIPCoreHandle, [MarshalAs(UnmanagedType.I1)] bool state);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern IntPtr SIPEV_createSIPCallbackHandle();
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_releaseSIPCallbackHandle(IntPtr callbackHandle);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setACTVTransferFailureHandler(IntPtr callbackHandle, ACTVTransferFailure eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setACTVTransferSuccessHandler(IntPtr callbackHandle, ACTVTransferSuccess eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setArrivedSignalingHandler(IntPtr callbackHandle, arrivedSignaling eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setInviteAnsweredHandler(IntPtr callbackHandle, inviteAnswered eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setInviteBeginingForwardHandler(IntPtr callbackHandle, inviteBeginingForward eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setInviteClosedHandler(IntPtr callbackHandle, inviteClosed eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setInviteFailureHandler(IntPtr callbackHandle, inviteFailure eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setInviteIncomingHandler(IntPtr callbackHandle, inviteIncoming eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setInviteRingingHandler(IntPtr callbackHandle, inviteRinging eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setInviteTryingHandler(IntPtr callbackHandle, inviteTrying eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setInviteUACConnectedHandler(IntPtr callbackHandle, inviteUACConnected eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setInviteUASConnectedHandler(IntPtr callbackHandle, inviteUASConnected eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setInviteUpdatedHandler(IntPtr callbackHandle, inviteUpdated eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setPASVTransferFailureHandler(IntPtr callbackHandle, PASVTransferFailure eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setPASVTransferSuccessHandler(IntPtr callbackHandle, PASVTransferSuccess eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setPresenceOfflineHandler(IntPtr callbackHandle, presenceOffline eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setPresenceOnlineHandler(IntPtr callbackHandle, presenceOnline eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setPresenceRecvSubscribeHandler(IntPtr callbackHandle, presenceRecvSubscribe eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setRecvBinaryMessageHandler(IntPtr callbackHandle, recvBinaryMessage eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setRecvBinaryPagerMessageHandler(IntPtr callbackHandle, recvBinaryPagerMessage eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setRecvDtmfToneHandler(IntPtr callbackHandle, recvDtmfTone eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setRecvInfoHandler(IntPtr callbackHandle, recvInfo eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setRecvMessageHandler(IntPtr callbackHandle, recvMessage eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setRecvOptionsHandler(IntPtr callbackHandle, recvOptions eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setRecvPagerMessageHandler(IntPtr callbackHandle, recvPagerMessage eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setRegisterFailureHandler(IntPtr callbackHandle, registerFailure eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setRegisterSuccessHandler(IntPtr callbackHandle, registerSuccess eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setRemoteHoldHandler(IntPtr callbackHandle, remoteHold eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setRemoteUnHoldHandler(IntPtr callbackHandle, remoteUnHold eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setSendPagerMessageFailureHandler(IntPtr callbackHandle, sendPagerMessageFailure eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setSendPagerMessageSuccessHandler(IntPtr callbackHandle, sendPagerMessageSuccess eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setTransferRingingHandler(IntPtr callbackHandle, transferRinging eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setTransferTryingHandler(IntPtr callbackHandle, transferTrying eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setWaitingFaxMessageHandler(IntPtr callbackHandle, waitingFaxMessage eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_setWaitingVoiceMessageHandler(IntPtr callbackHandle, waitingVoiceMessage eventHandler, int callbackObject);
        [DllImport("SIPEventEX.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void SIPEV_shutdownSIPCallbackHandle(IntPtr callbackHandle);

        // Nested Types
        public delegate int ACTVTransferFailure(int callbackObject, int sessionId, int statusCode, string statusText);

        public delegate int ACTVTransferSuccess(int callbackObject, int sessionId);

        public delegate int arrivedSignaling(int callbackObject, int sessionId, StringBuilder signaling);

        public delegate int audioRawCallback(IntPtr callbackObject, int sessionId, int callbackType, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] byte[] data, int dataLength, int samplingFreqHz);

        public delegate int inviteAnswered(int callbackObject, int sessionId, [MarshalAs(UnmanagedType.I1)] bool hasVideo, int statusCode, string statusText, string audioCodecName, string videoCodecName);

        public delegate int inviteBeginingForward(int callbackObject, string forwardingTo);

        public delegate int inviteClosed(int callbackObject, int sessionId);

        public delegate int inviteFailure(int callbackObject, int sessionId, int statusCode, string statusText);

        public delegate int inviteIncoming(int callbackObject, int sessionId, string caller, string callerDisplayName, string callee, string calleeDisplayName, string audioCodecName, string videoCodecName, [MarshalAs(UnmanagedType.I1)] bool hasVideo);

        public delegate int inviteRinging(int callbackObject, int sessionId, [MarshalAs(UnmanagedType.I1)] bool hasEarlyMedia, [MarshalAs(UnmanagedType.I1)] bool hasVideo, string audioCodecName, string videoCodecName);

        public delegate int inviteTrying(int callbackObject, int sessionId, string caller, string callee);

        public delegate int inviteUACConnected(int callbackObject, int sessionId, int statusCode, string statusText);

        public delegate int inviteUASConnected(int callbackObject, int sessionId, int statusCode, string statusText);

        public delegate int inviteUpdated(int callbackObject, int sessionId, [MarshalAs(UnmanagedType.I1)] bool hasVideo, string audioCodecName, string videoCodecName);

        public delegate int PASVTransferFailure(int callbackObject, int sessionId, int statusCode, string statusText);

        public delegate int PASVTransferSuccess(int callbackObject, int sessionId, [MarshalAs(UnmanagedType.I1)] bool hasVideo);

        public delegate int playAviFileToRemoteFinished(IntPtr callbackObject, int sessionId);

        public delegate int playWaveFileToRemoteFinished(IntPtr callbackObject, int sessionId, string fileName);

        public delegate int presenceOffline(int callbackObject, string from, string fromDisplayName);

        public delegate int presenceOnline(int callbackObject, string from, string fromDisplayName, string stateText);

        public delegate int presenceRecvSubscribe(int callbackObject, int subscribeId, string from, string fromDisplayName, string subject);

        public delegate int recvBinaryMessage(int callbackObject, int sessionId, StringBuilder message, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] byte[] messageBody, int length);

        public delegate int recvBinaryPagerMessage(int callbackObject, StringBuilder from, StringBuilder fromDisplayName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] byte[] messageBody, int length);

        public delegate int recvDtmfTone(int callbackObject, int sessionId, int tone);

        public delegate int recvInfo(int callbackObject, int sessionId, StringBuilder infoMessage);

        public delegate int recvMessage(int callbackObject, int sessionId, StringBuilder message);

        public delegate int recvOptions(int callbackObject, StringBuilder optionsMessage);

        public delegate int recvPagerMessage(int callbackObject, string from, string fromDisplayName, StringBuilder message);

        public delegate int registerFailure(int callbackObject, int statusCode, string statusText);

        public delegate int registerSuccess(int callbackObject, int statusCode, string statusText);

        public delegate int remoteHold(int callbackObject, int sessionId);

        public delegate int remoteUnHold(int callbackObject, int sessionId);

        public delegate int sendPagerMessageFailure(int callbackObject, string caller, string callerDisplayName, string callee, string calleeDisplayName, int statusCode, string statusText);

        public delegate int sendPagerMessageSuccess(int callbackObject, string caller, string callerDisplayName, string callee, string calleeDisplayName);

        public delegate int transferRinging(int callbackObject, int sessionId, [MarshalAs(UnmanagedType.I1)] bool hasVideo);

        public delegate int transferTrying(int callbackObject, int sessionId, string referTo);

        public delegate int videoRawCallback(IntPtr callbackObject, int sessionId, int callbackType, int width, int height, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 6)] byte[] data, int dataLength);

        public delegate int waitingFaxMessage(int callbackObject, string messageAccount, int urgentNewMessageCount, int urgentOldMessageCount, int newMessageCount, int oldMessageCount);

        public delegate int waitingVoiceMessage(int callbackObject, string messageAccount, int urgentNewMessageCount, int urgentOldMessageCount, int newMessageCount, int oldMessageCount);
    }


}
