using System;
using DuoSoftware.DuoAuth.RefAuth.Iauth;
using DuoSoftware.DuoLogger;
using DuoSoftware.DuoSoftPhone.refResourceProxy;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    public class ArdsHandler
    {
        public static ResourceProxyReplyDataResourceProxyReply SendModeChangeRequestInbound(UserAuth auth)
        {
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                var result = client.SendModeChangeRequestInbound(auth.SecurityToken, "", "", "", CommunicationModes.WebService, "", "");
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("SendModeChangeRequestInbound. Time Take :{0} , {1}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, result), Logger.LogLevel.Info);
                return result;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "SendModeChangeRequestInbound", exception, Logger.LogLevel.Error);
                return new ResourceProxyReplyDataResourceProxyReply { Command = WorkflowResultCode.Error };
            }
        }

        public static ResourceProxyReplyDataResourceProxyReply SendModeChangeRequestOutbound(UserAuth auth)
        {
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                var result = client.SendModeChangeRequestOutbound(auth.SecurityToken, "", "", "", CommunicationModes.WebService, "", "");
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("SendModeChangeRequestOutbound. Time Take :{0} , {1}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, result), Logger.LogLevel.Info);
                return result;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "SendModeChangeRequestOutbound", exception, Logger.LogLevel.Error);
                return new ResourceProxyReplyDataResourceProxyReply { Command = WorkflowResultCode.Error };
            }
        }

        public static bool ResourceModeChange(UserAuth auth)
        {
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                var result = client.ResourceModeChange(auth.SecurityToken, null, null, null, CommunicationModes.WebService, null, null);
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("ResourceModeChange. Time Take :{0} , {1}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, result), Logger.LogLevel.Info);
                return true;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "ResourceModeChange", exception, Logger.LogLevel.Error);
                return false;
            }
        }

        public static ResourceProxyReplyDataResourceProxyReply ResourceRegistration(UserAuth auth, string ip)
        {
            try
            {
                var startTIme = DateTime.Now;
                var client = new ResourceProxyServicesClient();
                var result = client.ResourceRegistration(auth.SecurityToken, string.Empty, string.Empty, string.Empty,
                   CommunicationModes.WebService, string.Empty,
                   string.Format("{0}~{1}~{2}~{3}", auth.UserName, auth.UserName, auth.UserName, ip));
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("ResourceRegistration. Time Take :{0} , {1}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, result), Logger.LogLevel.Info);
                return result;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "ResourceRegistration", exception, Logger.LogLevel.Error);
                return new ResourceProxyReplyDataResourceProxyReply { Command = WorkflowResultCode.Error };
            }
        }

        public static void ResourceUnregistration(UserAuth auth)
        {
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                var result = client.ResourceUnregistration(auth.SecurityToken, null, null, null, CommunicationModes.WebService, null, null);
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("ResourceUnregistration. Time Take :{0} , {1}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, result), Logger.LogLevel.Info);
                var t = result.Command;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "ResourceUnregistration", exception, Logger.LogLevel.Error);
            }
        }

        public static void ResourceStatusChangeBusy(UserAuth auth, string callSessionId)
        {
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                var result = client.ResourceStatusChangeBusy(auth.SecurityToken, null, null, null,
                                                                 CommunicationModes.WebService, null, callSessionId, null);
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("ResourceStatusChangeBusy. Time Take :{0} , {1}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, result), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "ResourceStatusChangeBusy", exception, Logger.LogLevel.Error);
            }
        }

        public static void ResourceStatusChangeACW(UserAuth auth, string callSessionId)
        {
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                var result = client.ResourceStatusChangeACW(auth.SecurityToken, null, null, null,
                                                                 CommunicationModes.WebService, null, callSessionId, null);
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("ResourceStatusChangeACW. Time Take :{0} , {1}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, result), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "ResourceStatusChangeACW", exception, Logger.LogLevel.Error);
            }
        }

        public static void ResourceStatusChangeBreak(UserAuth auth, string sessionId)
        {
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                var result = client.ResourceStatusChangeBreak(auth.SecurityToken, null, null, null,
                                                                 CommunicationModes.WebService, null, sessionId, null);
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("ResourceStatusChangeBreak. Time Take :{0} , {1}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, result), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "ResourceStatusChangeBreak", exception, Logger.LogLevel.Error);
            }
        }

        public static WorkflowResultCode SendStatusChangeRequestBreak(UserAuth auth, string sessionId)
        {
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                var result = client.SendStatusChangeRequestBreak(auth.SecurityToken, string.Empty, string.Empty, string.Empty, CommunicationModes.WebService, auth.guUserId.ToString(), "Official Break",
                    auth.CompanyID.ToString(), sessionId);
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("SendStatusChangeRequestBreak. Time Take :{0} , {1}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, result), Logger.LogLevel.Info);
                return result.Command;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "SendStatusChangeRequestBreak", exception, Logger.LogLevel.Error);
                return WorkflowResultCode.Error;
            }
        }

        public static bool CancelBreakRequest(UserAuth auth)
        {
            try
            {
                try
                {
                    var startTIme = DateTime.Now;
                    var client = new refResourceProxy.ResourceProxyServicesClient();
                    var result = client.CancelStatusChangeRequest(auth.SecurityToken, string.Empty, string.Empty, string.Empty, CommunicationModes.WebService, auth.guUserId.ToString(), string.Empty);
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("CancelBreakRequest. Time Take :{0} , {1}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, result), Logger.LogLevel.Info);
                    return result.Command == WorkflowResultCode.ACDS701;
                }
                catch (Exception exception)
                {
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "CancelBreakRequest[-------ResourceProxy-----------]", exception, Logger.LogLevel.Error);
                    return false;
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "SendStatusChangeRequestBreak", exception, Logger.LogLevel.Error);
                return false;
            }
        }

        public static ResourceProxyReplyDataResourceProxyReply SendStatusChangeRequestIdel(UserAuth auth, string callSessionId)
        {
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                var result = client.SendStatusChangeRequestIdel(auth.SecurityToken, null, null, null, CommunicationModes.WebService, null, null, null, callSessionId);
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("SendStatusChangeRequestIdel. Time Take :{0} , {1}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, result), Logger.LogLevel.Info);
                return result;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "SendStatusChangeRequestIdel", exception, Logger.LogLevel.Error);
                return new ResourceProxyReplyDataResourceProxyReply { Command = WorkflowResultCode.Error };
            }
        }
    }
}
