using System;
using DuoSoftware.CommonTools.Security;
using DuoSoftware.DuoSoftPhone.refResourceProxy;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
     public delegate void ArdsEvent(ResourceProxyReplyDataResourceProxyReply result);
    interface IResourceHandler
    {
        event ArdsEvent OnCancelStatusChangeRequestCompleted;
        event ArdsEvent OnResourceRegistrationCompleted;
        event ArdsEvent OnSendModeChangeRequestInboundCompleted;
        event ArdsEvent OnSendStatusChangeRequestBreakCompleted;
        event ArdsEvent OnSendStatusChangeRequestIdelCompleted;
        event ArdsEvent OnStatusUpdatedMessage;
        void CancelBreakRequest(CommonTools.Security.UserAuth auth, string sessionId); 
        bool ResourceModeChange(CommonTools.Security.UserAuth auth, string callSessionId);
        void ResourceRegistration(CommonTools.Security.UserAuth auth, string ip);
        void ResourceStatusChangeAcw(CommonTools.Security.UserAuth auth, string callSessionId);
        void ResourceStatusChangeBreak(CommonTools.Security.UserAuth auth, string sessionId);
        void ResourceStatusChangeBusy(CommonTools.Security.UserAuth auth, string callSessionId);
        void ResourceUnregistration(CommonTools.Security.UserAuth auth);
        void SendModeChangeRequestInbound(CommonTools.Security.UserAuth auth);
        refResourceProxy.ResourceProxyReplyDataResourceProxyReply SendModeChangeRequestOutbound(CommonTools.Security.UserAuth auth);
        void SendStatusChangeRequestBreak(CommonTools.Security.UserAuth auth, string sessionId, string breakReason);
        void SendStatusChangeRequestIdel(CommonTools.Security.UserAuth auth, string callSessionId);
    }
}
