//////////////////////////////////////////////////////////////////////////
//
// IMPORTANT: DON'T EDIT THIS FILE
//
//////////////////////////////////////////////////////////////////////////


namespace DuoSoftware.DuoSoftPhone.Controllers.PortSIP
{
    public enum AUDIOCODEC_TYPE : int
    {
        AUDIOCODEC_G723 = 4,	//8KHZ
        AUDIOCODEC_G729 = 18,	//8KHZ
        AUDIOCODEC_PCMA = 8,	//8KHZ
        AUDIOCODEC_PCMU = 0,	//8KHZ
        AUDIOCODEC_GSM = 3,	//8KHZ
        AUDIOCODEC_G722 = 9,	//16KHZ
        AUDIOCODEC_ILBC = 97,	//8KHZ
        AUDIOCODEC_AMR = 98,	//8KHZ
        AUDIOCODEC_AMRWB = 99,	//16KHZ
        AUDIOCODEC_SPEEX = 100,	//8KHZ
        AUDIOCODEC_SPEEXWB = 102,	//16KHZ
        AUDIOCODEC_ISACWB = 103,	//16KHZ
        AUDIOCODEC_ISACSWB = 104,	//32KHZ
        AUDIOCODEC_G7221 = 121,	//16KHZ
        AUDIOCODEC_DTMF = 101
    }

    public enum VIDEOCODEC_TYPE : int
    {
        VIDEOCODEC_i420 = 113,
        VIDEOCODEC_H263 = 34,
        VIDEOCODEC_H263_1998 = 115,
        VIDEOCODEC_H264 = 125,
	    VIDEOCODEC_VP8	= 120
    }



    public enum VIDEO_RESOLUTION : int
    {
        VIDEO_QCIF = 1,		//	176X144		- for H263, H263-1998, H264
        VIDEO_CIF = 2,		//	352X288		- for H263, H263-1998, H264
        VIDEO_VGA = 3,		//	640X480		- for H264 only
        VIDEO_SVGA = 4,		//	800X600		- for H264 only
        VIDEO_XVGA = 5,		//	1024X768	- for H264 only
        VIDEO_720P = 6,		//	1280X720	- for H264 only
        VIDEO_QVGA = 7		//	320X240 	- for H264 only
    }



    public enum AUDIO_RECORDING_FILEFORMAT : int
    {
        FILEFORMAT_WAVE = 1,
        FILEFORMAT_OGG,
        FILEFORMAT_MP3
    }




    public enum CALLBACK_SESSION_ID : int
    {
        PORTSIP_LOCAL_MIX_ID = -1,	
        PORTSIP_REMOTE_MIX_ID = -2,	
    }


    public enum AUDIOSTREAM_CALLBACK_MODE : int
    {
        AUDIOSTREAM_NONE = 0,             //  Disable callback
        AUDIOSTREAM_LOCAL_MIX,		    	//	Callback the audio stream from microphone for all channels.
        AUDIOSTREAM_LOCAL_PER_CHANNEL,		//  Callback the audio stream from microphone for one channel base on the session ID
        AUDIOSTREAM_REMOTE_MIX,				//	Callback the received audio stream that mixed including all channels.
        AUDIOSTREAM_REMOTE_PER_CHANNEL,		//  Callback the received audio stream for one channel base on the session ID.
    }



    public enum VIDEOSTREAM_CALLBACK_MODE : int
    {
        VIDEOSTREAM_NONE = 0,	// Disable video stream callback
        VIDEOSTREAM_LOCAL,		// Local video stream callback
        VIDEOSTREAM_REMOTE,		// Remote video stream callback
        VIDEOSTREAM_BOTH,		// Both of local and remote video stream callback
    }



    public enum PRESENCE_MODE : int
    {
        PRESENCE_P2P,
        PRESENCE_AGENT
    }



    // Log level
    public enum PORTSIP_LOG_LEVEL : int
    {
        PORTSIP_LOG_NONE = -1,
        PORTSIP_LOG_INFO = 0,
        PORTSIP_LOG_WARNING = 1,
        PORTSIP_LOG_ERROR = 2
    }



    // SRTP Policy
    public enum SRTP_POLICY : int
    {
        SRTP_POLICY_NONE = 0,	// No use SRTP
        SRTP_POLICY_FORCE,		// All calls must use SRTP
        SRTP_POLICY_PREFER		// Top priority to use SRTP
    }



    // Transport
    public enum TRANSPORT_TYPE : int
    {
        TRANSPORT_UDP = 0,
        TRANSPORT_TLS,
        TRANSPORT_TCP
    }



    // Audio jitter buffer level
    public enum JITTER_BUFFER_LEVEL : int
    {
        JITTER_SIZE_0 = 0,
        JITTER_SIZE_5,
        JITTER_SIZE_10,	//default jitter buffer size
        JITTER_SIZE_20
    }


    // Audio record mode
    public enum RECORD_MODE : int
    {
        RECORD_RECV = 1,
        RECORD_SEND,
        RECORD_BOTH
    }

    public enum PROVISIONAL_MODE : int
    {
        UAC_PRACK_NONE = 0,
        UAC_PRACK_SUPPORTED,
        UAC_PRACK_REQUIRED,
        UAS_PRACK_NONE,
        UAS_PRACK_SUPPORTED,
        UAS_PRACK_REQUIRED
    }

   public enum SESSION_REFRESH_MODE : int
    {
        SESSION_REFERESH_UAC = 0,
        SESSION_REFERESH_UAS
    }




    //////////////////////////////////////////////////////////////////////////
    //
    // Extend types, don't use it
    //
    //////////////////////////////////////////////////////////////////////////



public enum AMR_RATE_CHANGE_MODE
{
	AMR_RATE_CHANGE_PRIORITY		= 0,
	AMR_RATE_CHANGE_FIXED			= 1,
	AMR_RATE_CHANGE_PEERREQUESTED	= 2,
	AMR_RATE_CHANGE_STRESS			= 3
}

    //////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////

}
