using DuoSoftware.DuoSoftPhone.RefResourceProxy;
using DuoSoftware.DuoSoftPhone.RefUserAuth;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    public delegate void ArdsEvent(ResourceProxyReplyDataResourceProxyReply result);

    internal interface ISoftPhoneResourceHandler
    {
        event ArdsEvent OnCancelBreakRequestCompleted;

        event ArdsEvent OnResourceRegistrationCompleted;

        event ArdsEvent OnSendModeChangeRequestInboundCompleted;

        event ArdsEvent OnSendModeChangeRequestOutboundCompleted;

        event ArdsEvent OnSendStatusChangeRequestBreakCompleted;

        event ArdsEvent OnSendResourceChangeBreakCompleted;

        event ArdsEvent OnSendStatusChangeRequestIdelCompleted;

        event ArdsEvent OnStatusUpdatedMessage;

        event ArdsEvent OnResourceModeChanged;

        void CancelBreakRequest(UserAuth auth, string sessionId);

        bool ResourceModeChange(UserAuth auth, string callSessionId);

        void ResourceRegistration(UserAuth auth, string ip);

        void ResourceStatusChangeAcw(UserAuth auth, string callSessionId, bool isCallAnswer);

        void ResourceStatusChangeBreak(UserAuth auth, string sessionId);

        void ResourceStatusChangeBusy(UserAuth auth, string callSessionId, bool isCallAnswer);

        void ResourceUnregistration(UserAuth auth);

        void SendModeChangeRequestInbound(UserAuth auth);

        void SendModeChangeRequestOutbound(UserAuth auth);

        void SendStatusChangeRequestBreak(UserAuth auth, string sessionId, string breakReason);

        void SendStatusChangeRequestIdel(UserAuth auth, string callSessionId, bool isCallAnswer,bool transferCall);
    }
}