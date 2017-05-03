using System;
using System.Text;

namespace DuoSoftware.DuoSoftPhone.Controllers.PortSIP
{
    internal interface SIPCallbackEvents
    {
        // Methods
        int onACTVTransferFailure(int callbackObject, int sessionId, int statusCode, string statusText);
        int onACTVTransferSuccess(int callbackObject, int sessionId);
        int onArrivedSignaling(int callbackObject, int sessionId, StringBuilder signaling);
        int onAudioRawCallback(IntPtr callbackObject, int sessionId, int callbackType, byte[] data, int dataLength, int samplingFreqHz);
        int onInviteAnswered(int callbackObject, int sessionId, bool hasVideo, int statusCode, string statusText, string audioCodecName, string videoCodecName);
        int onInviteBeginingForward(int callbackObject, string forwardingTo);
        int onInviteClosed(int callbackObject, int sessionId);
        int onInviteFailure(int callbackObject, int sessionId, int statusCode, string statusText);
        int onInviteIncoming(int callbackObject, int sessionId, string caller, string callerDisplayName, string callee, string calleeDisplayName, string audioCodecName, string videoCodecName, bool hasVideo);
        int onInviteRinging(int callbackObject, int sessionId, bool hasEarlyMedia, bool hasVideo, string audioCodecName, string videoCodecName);
        int onInviteTrying(int callbackObject, int sessionId, string caller, string callee);
        int onInviteUACConnected(int callbackObject, int sessionId, int statusCode, string statusText);
        int onInviteUASConnected(int callbackObject, int sessionId, int statusCode, string statusText);
        int onInviteUpdated(int callbackObject, int sessionId, bool hasVideo, string audioCodecName, string videoCodecName);
        int onPASVTransferFailure(int callbackObject, int sessionId, int statusCode, string statusText);
        int onPASVTransferSuccess(int callbackObject, int sessionId, bool hasVideo);
        int onPlayAviFileFinished(IntPtr callbackObject, int sessionId);
        int onPlayWaveFileFinished(IntPtr callbackObject, int sessionId, string fileName);
        int onPresenceOffline(int callbackObject, string from, string fromDisplayName);
        int onPresenceOnline(int callbackObject, string from, string fromDisplayName, string stateText);
        int onPresenceRecvSubscribe(int callbackObject, int subscribeId, string from, string fromDisplayName, string subject);
        int onRecvBinaryMessage(int callbackObject, int sessionId, StringBuilder message, byte[] messageBody, int length);
        int onRecvBinaryPagerMessage(int callbackObject, StringBuilder from, StringBuilder fromDisplayName, byte[] messageBody, int length);
        int onRecvDtmfTone(int callbackObject, int sessionId, int tone);
        int onRecvInfo(int callbackObject, int sessionId, StringBuilder infoMessage);
        int onRecvMessage(int callbackObject, int sessionId, StringBuilder message);
        int onRecvOptions(int callbackObject, StringBuilder optionsMessage);
        int onRecvPagerMessage(int callbackObject, string from, string fromDisplayName, StringBuilder message);
        int onRegisterFailure(int callbackObject, int statusCode, string statusText);
        int onRegisterSuccess(int callbackObject, int statusCode, string statusText);
        int onRemoteHold(int callbackObject, int sessionId);
        int onRemoteUnHold(int callbackObject, int sessionId);
        int onSendPagerMessageFailure(int callbackObject, string caller, string callerDisplayName, string callee, string calleeDisplayName, int statusCode, string statusText);
        int onSendPagerMessageSuccess(int callbackObject, string caller, string callerDisplayName, string callee, string calleeDisplayName);
        int onTransferRinging(int callbackObject, int sessionId, bool hasVideo);
        int onTransferTrying(int callbackObject, int sessionId, string referTo);
        int onVideoRawCallback(IntPtr callbackObject, int sessionId, int callbackType, int width, int height, byte[] data, int dataLength);
        int onWaitingFaxMessage(int callbackObject, string messageAccount, int urgentNewMessageCount, int urgentOldMessageCount, int newMessageCount, int oldMessageCount);
        int onWaitingVoiceMessage(int callbackObject, string messageAccount, int urgentNewMessageCount, int urgentOldMessageCount, int newMessageCount, int oldMessageCount);
    }


}
