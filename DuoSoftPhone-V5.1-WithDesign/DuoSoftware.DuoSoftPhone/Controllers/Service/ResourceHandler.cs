using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuoSoftware.CommonTools.Security;
using DuoSoftware.DuoLogger;
using DuoSoftware.DuoSoftPhone.refResourceProxy;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    public class ResourceHandler : IResourceHandler
    {
        private readonly IResourceHandler handler;

        public event ArdsEvent OnCancelStatusChangeRequestCompleted;
        public event ArdsEvent OnResourceRegistrationCompleted;
        public event ArdsEvent OnSendModeChangeRequestInboundCompleted;
        public event ArdsEvent OnSendStatusChangeRequestBreakCompleted;
        public event ArdsEvent OnSendStatusChangeRequestIdelCompleted;
        public event ArdsEvent OnStatusUpdatedMessage;

        public ResourceHandler(string securityToken, int tenantId, int companyId)
        {
            handler = new ArdsHandler();
            var showMsg = false;
            
            try
            {
                var setting = CommonDataHandler.GetEventInfo(securityToken, tenantId, companyId);
                showMsg = setting.ShowMsg;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "GetEventInfo", exception, Logger.LogLevel.Error);
            }

            if(showMsg)
            {
                handler.OnStatusUpdatedMessage += (s) =>
                {
                    try
                    {
                        if (OnStatusUpdatedMessage != null)
                        {
                            var msg = string.Empty;
                            switch (s.Command)
                            {
                                case WorkflowResultCode.ACDS401: //- Agent status updated to Break "ACDS401"
                                    msg = "Agent status updated to Break";
                                    break;
                                case WorkflowResultCode.ACDS402: //- Agent status updated to Busy "ACDS402"
                                    msg = "Agent status updated to Busy";
                                    break;
                                case WorkflowResultCode.ACDS403: //- Agent status updated to Idle "ACDS403"
                                    msg = "Agent status updated to Idle";
                                    break;

                                case WorkflowResultCode.ACDS4031: //- Logoff Granted on Idle "ACDS4031"
                                    msg = "Logoff Granted on Idle";
                                    break;
                                case WorkflowResultCode.ACDS4032: //	- Break Granted on Idle "ACDS4032"
                                    msg = "Break Granted on Idle";
                                    break;
                                case WorkflowResultCode.ACDS4033: //	- Mode Change Granted on Idle "ACDS4033"
                                    msg = "Mode Change Granted on Idle";
                                    break;
                                case WorkflowResultCode.ACDS404: //- Agent status updated to ACW "ACDS404"
                                    msg = "Agent status updated to ACW";
                                    break;
                                case WorkflowResultCode.ACDE401:
                                    //- Agent does not exist / not correctly registered "ACDE401"
                                    msg = "Agent does not exist / not correctly registered";
                                    break;
                                case WorkflowResultCode.ACDE402: //- Agent status updated to Break Failed "ACDE402"
                                    msg = "Agent status updated to Break Failed";
                                    break;
                                case WorkflowResultCode.ACDE403: //- Agent status updated to Busy Failed "ACDE403"
                                    msg = "Agent status updated to Busy Failed";
                                    break;
                                case WorkflowResultCode.ACDE404: //- Agent status updated to ACW Failed "ACDE404"
                                    msg = "Agent status updated to ACW Failed";
                                    break;
                                default:
                                    msg = "Agent status updated.";
                                    break;
                            }
                            s.ExtraData = msg;
                            OnStatusUpdatedMessage(s);

                        }
                        else
                        {
                            Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault,
                                "Status Update Notification-Disable by Admin", Logger.LogLevel.Debug);
                        }
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Status Update Notification-Disable.",
                            exception, Logger.LogLevel.Error);
                    }
                };
            }

            handler.OnCancelStatusChangeRequestCompleted += (s) =>
            {
                if (OnCancelStatusChangeRequestCompleted != null)
                    OnCancelStatusChangeRequestCompleted(s);
            };

            handler.OnResourceRegistrationCompleted += (s) =>
            {
                if (OnResourceRegistrationCompleted != null)
                    OnResourceRegistrationCompleted(s);
            };

            handler.OnSendModeChangeRequestInboundCompleted += (s) =>
            {
                if (OnSendModeChangeRequestInboundCompleted != null)
                    OnSendModeChangeRequestInboundCompleted(s);
            };

            handler.OnSendStatusChangeRequestBreakCompleted += (s) =>
            {
                if (OnSendStatusChangeRequestBreakCompleted != null)
                    OnSendStatusChangeRequestBreakCompleted(s);
            };

            handler.OnSendStatusChangeRequestIdelCompleted += (s) =>
            {
                if (OnSendStatusChangeRequestIdelCompleted != null)
                    OnSendStatusChangeRequestIdelCompleted(s);
            };
        }

        public void CancelBreakRequest(UserAuth auth, string sessionId)
        {
          handler.CancelBreakRequest(auth,sessionId);
        }

        public bool ResourceModeChange(UserAuth auth, string callSessionId)
        {
            return handler.ResourceModeChange(auth, callSessionId);
        }

        public void ResourceRegistration(UserAuth auth, string ip)
        {
            handler.ResourceRegistration(auth,ip);
        }

        public void ResourceStatusChangeAcw(UserAuth auth, string callSessionId)
        {
            handler.ResourceStatusChangeAcw(auth,callSessionId);
        }

        public void ResourceStatusChangeBreak(UserAuth auth, string sessionId)
        {
            handler.ResourceStatusChangeBreak(auth,sessionId);
        }

        public void ResourceStatusChangeBusy(UserAuth auth, string callSessionId)
        {
            handler.ResourceStatusChangeBusy(auth,callSessionId);
        }

        public void ResourceUnregistration(UserAuth auth)
        {
            handler.ResourceUnregistration(auth);
        }

        public void SendModeChangeRequestInbound(UserAuth auth)
        {
            handler.SendModeChangeRequestInbound(auth);
        }

        public ResourceProxyReplyDataResourceProxyReply SendModeChangeRequestOutbound(UserAuth auth)
        {
           return handler.SendModeChangeRequestOutbound(auth);
        }

        public void SendStatusChangeRequestBreak(UserAuth auth, string sessionId, string breakReason)
        {
            handler.SendStatusChangeRequestBreak(auth,sessionId, breakReason);
        }

        public void SendStatusChangeRequestIdel(UserAuth auth, string callSessionId)
        {
            handler.SendStatusChangeRequestIdel(auth,callSessionId);
        }
    }
}
