//////////////////////////////////////////////////////////////////////////
//
// IMPORTANT: DON'T EDIT THIS FILE
//
//////////////////////////////////////////////////////////////////////////

using System;

namespace PortSIP
{
    /// Audio codec type
    [Serializable]
    public enum AudiocodecType : int
    {
        AudiocodecNone = -1,
        AudiocodecG729 = 18,	    ///< G729 8KHZ 8kbit/s
        AudiocodecPcma = 8,	    ///< PCMA/G711 A-law 8KHZ 64kbit/s
        AudiocodecPcmu = 0,	    ///< PCMU/G711 μ-law 8KHZ 64kbit/s
        AudiocodecGsm = 3,	        ///< GSM 8KHZ 13kbit/s
        AudiocodecG722 = 9,	    ///< G722 16KHZ 64kbit/s
        AudiocodecIlbc = 97,	    ///< iLBC 8KHZ 30ms-13kbit/s 20 ms-15kbit/s
        AudiocodecAmr = 98,	    ///< Adaptive Multi-Rate (AMR) 8KHZ (4.75,5.15,5.90,6.70,7.40,7.95,10.20,12.20)kbit/s
        AudiocodecAmrwb = 99,	    ///< Adaptive Multi-Rate Wideband (AMR-WB)16KHZ (6.60,8.85,12.65,14.25,15.85,18.25,19.85,23.05,23.85)kbit/s
        AudiocodecSpeex = 100,	    ///< SPEEX 8KHZ (2-24)kbit/s
        AudiocodecSpeexwb = 102,	///< SPEEX 16KHZ (4-42)kbit/s
        AudiocodecIsacwb = 103,	///< internet Speech Audio Codec(iSAC) 16KHZ (32-54)kbit/s
        AudiocodecIsacswb = 104,	///< internet Speech Audio Codec(iSAC) 16KHZ (32-160)kbit/s
        AudiocodecG7221 = 121,	    ///< G722.1 16KHZ (16,24,32)kbit/s
        AudiocodecOpus = 105,	    ///< OPUS 48KHZ 32kbit/s
        AudiocodecDtmf = 101	    ///< DTMF RFC 2833
    }

    /// Video codec type
    [Serializable]
    public enum VideocodecType : int
    {
        VideoCodeNone = -1,	        ///< Not use Video codec
        VideoCodecI420 = 113,	        ///< I420/YUV420 Raw Video format, just use with startRecord
        VideoCodecH263 = 34,	        ///< H263 video codec
        VideoCodecH2631998 = 115,	///< H263+/H263 1998 video codec
        VideoCodecH264 = 125,	        ///< H264 video codec
        VideoCodecVp8 = 120	        ///< VP8 video code
    }

    /// Video Resolution
    [Serializable]
    public enum VideoResolution : int
    {
        VideoNone = 0,
        VideoQcif = 1,		///<	176X144		- for H.263, H.263-1998, H.264, VP8
        VideoCif = 2,		///<	352X288		- for H.263, H.263-1998, H.264, VP8
        VideoVga = 3,		///<	640X480		- for H.264, VP8
        VideoSvga = 4,		///<	800X600		- for H.264, VP8
        VideoXvga = 5,		///<	1024X768	- for H.264, VP8
        Video720P = 6,		///<	1280X720	- for H.264, VP8
        VideoQvga = 7		///<	320X240		- for H.264, VP8
    }

    /// The audio record file format
    [Serializable]
    public enum AudioRecordingFileformat : int
    {
        FileformatWave = 1,	///<	The record audio file is WAVE format.
        FileformatAmr,			///<	The record audio file is AMR format - all voice data is compressed by AMR codec.
    }

    ///The audio/Video record mode
    [Serializable]
    public enum RecordMode : int
    {
        RecordNone = 0,		///<	Not Record.
        RecordRecv = 1,		///<	Only record the received data.
        RecordSend,			///<	Only record send data.
        RecordBoth				///<	The record audio file is WAVE format.
    }

    [Serializable]
    public enum CallbackSessionId : int
    {
        PortsipLocalMixId = -1,
        PortsipRemoteMixId = -2,
    }

    ///The audio stream callback mode
    [Serializable]
    public enum AudiostreamCallbackMode : int
    {
        AudiostreamNone = 0,
        AudiostreamLocalMix,				///<	Callback the audio stream from microphone for all channels.
        AudiostreamLocalPerChannel,		///<  Callback the audio stream from microphone for one channel base on the session ID
        AudiostreamRemoteMix,				///<	Callback the received audio stream that mixed including all channels.
        AudiostreamRemotePerChannel,		///<  Callback the received audio stream for one channel base on the session ID.
    }

    ///The video stream callback mode
    [Serializable]
    public enum VideostreamCallbackMode : int
    {
        VideostreamNone = 0,	///< Disable video stream callback
        VideostreamLocal,		///< Local video stream callback
        VideostreamRemote,		///< Remote video stream callback
        VideostreamBoth,		///< Both of local and remote video stream callback
    }

    /// Log level
    [Serializable]
    public enum PortsipLogLevel : int
    {
        PortsipLogNone = -1,
        PortsipLogError = 1,
        PortsipLogWarning = 2,
        PortsipLogOnfo = 3,
        PortsipLogDebug = 4
    }

    /// SRTP Policy
    [Serializable]
    public enum SrtpPolicy : int
    {
        None = 0,	///< No use SRTP, The SDK can receive the encrypted call(SRTP) and unencrypted call both, but can't place outgoing encrypted call.
        Force,		///< All calls must use SRTP, The SDK just allows receive encrypted Call and place outgoing encrypted call only.
        Prefer		///< Top priority to use SRTP, The SDK allows receive encrypted and decrypted call, and allows place outgoing encrypted call and unencrypted call.
    }

    /// Transport for SIP signaling.
    [Serializable]
    public enum TransportType : int
    {
        TransportUdp = 0,	///< UDP Transport
        TransportTls,		///< Tls Transport
        TransportTcp,		///< TCP Transport
        TransportPers		///< PERS is the DuoCallTestTool private transport for anti the SIP blocking, it must using with the PERS Server http://www.DuoCallTestTool.com/pers.html.
    }

    ///The session refresh by UAC or UAS
    [Serializable]
    public enum SessionRefreshMode : int
    {
        SessionRefereshUac = 0,	///< The session refresh by UAC
        SessionRefereshUas		///< The session refresh by UAS
    }

    ///send DTMF tone with two methods
    [Serializable]
    public enum DtmfMethod
    {
        DtmfRfc2833 = 0,	///<	send DTMF tone with RFC 2833, recommend.
        DtmfInfo = 1	    ///<	send DTMF tone with SIP INFO.
    }
}