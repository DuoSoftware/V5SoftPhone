using System;
using DuoSoftware.DuoSoftPhone.refResourceProxy;
using DuoSoftware.DuoSoftPhone.refUserAuth;
using DuoSoftware.DuoTools.DuoLogger;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    public class ResourceHandler : IResourceHandler
    {
        private readonly IResourceHandler handler;

        public event ArdsEvent OnCancelBreakRequestCompleted;
        public event ArdsEvent OnResourceRegistrationCompleted;
        public event ArdsEvent OnSendModeChangeRequestInboundCompleted;
        public event ArdsEvent OnSendModeChangeRequestOutboundCompleted;
        public event ArdsEvent OnSendStatusChangeRequestBreakCompleted;
        public event ArdsEvent OnSendResourceChangeBreakCompleted;
        public event ArdsEvent OnSendStatusChangeRequestIdelCompleted;
        public event ArdsEvent OnStatusUpdatedMessage;
        public event ArdsEvent OnResourceModeChanged;

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
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "GetEventInfo", exception, Logger.LogLevel.Error);
            }

            if (showMsg)
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
                            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault,
                                "Status Update Notification-Disable by Admin", Logger.LogLevel.Debug);
                        }
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Status Update Notification-Disable.",
                            exception, Logger.LogLevel.Error);
                    }
                };
            }

            handler.OnCancelBreakRequestCompleted += (s) =>
            {
                if (OnCancelBreakRequestCompleted != null)
                    OnCancelBreakRequestCompleted(s);
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

            handler.OnSendModeChangeRequestOutboundCompleted += (s) =>
            {
                if (OnSendModeChangeRequestOutboundCompleted != null)
                    OnSendModeChangeRequestOutboundCompleted(s);
            };


            handler.OnResourceModeChanged += (s) =>
            {
                if (OnResourceModeChanged != null)
                    OnResourceModeChanged(s);
            };

            handler.OnSendResourceChangeBreakCompleted += (s) =>
            {
                if (OnSendResourceChangeBreakCompleted != null)
                    OnSendResourceChangeBreakCompleted(s);
            };
        }

        public void CancelBreakRequest(UserAuth auth, string sessionId)
        {
            handler.CancelBreakRequest(auth, sessionId);
        }

        public bool ResourceModeChangeNew(UserAuth auth, string callSessionId)
        {
            return new ArdsHandler().ResourceModeChangeNew(auth, callSessionId);
        }

        public bool ResourceModeChange(UserAuth auth, string callSessionId)
        {
            return handler.ResourceModeChange(auth, callSessionId);
        }

        public void ResourceRegistration(UserAuth auth, string ip)
        {
            handler.ResourceRegistration(auth, ip);
        }

        public void ResourceStatusChangeAcw(UserAuth auth, string callSessionId, bool isCallAnswer)
        {
            handler.ResourceStatusChangeAcw(auth, callSessionId, isCallAnswer);
        }

        public void ResourceStatusChangeBreak(UserAuth auth, string sessionId)
        {
            handler.ResourceStatusChangeBreak(auth, sessionId);
        }

        public void ResourceStatusChangeBusy(UserAuth auth, string callSessionId, bool isCallAnswer)
        {
            handler.ResourceStatusChangeBusy(auth, callSessionId, isCallAnswer);
        }

        public void ResourceUnregistration(UserAuth auth)
        {
            handler.ResourceUnregistration(auth);
        }

        public void SendModeChangeRequestInbound(UserAuth auth)
        {
            handler.SendModeChangeRequestInbound(auth);
        }

        public void SendModeChangeRequestOutbound(UserAuth auth)
        {
            handler.SendModeChangeRequestOutbound(auth);
        }

        public void SendStatusChangeRequestBreak(UserAuth auth, string sessionId, string breakReason)
        {
            handler.SendStatusChangeRequestBreak(auth, sessionId, breakReason);
        }

        public void SendStatusChangeRequestIdel(UserAuth auth, string callSessionId, bool isCallAnswer)
        {
            handler.SendStatusChangeRequestIdel(auth, callSessionId, isCallAnswer);
        }
    }
}
