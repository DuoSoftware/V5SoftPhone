using System;
using DuoSoftware.CommonTools.Security;
using DuoSoftware.DuoLogger;
using DuoSoftware.DuoSoftPhone.refResourceProxy;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    public class ArdsHandler : IResourceHandler 
    {
        

        public event ArdsEvent OnResourceRegistrationCompleted;
        public event ArdsEvent OnSendModeChangeRequestInboundCompleted;
        public event ArdsEvent OnSendStatusChangeRequestBreakCompleted;
        public event ArdsEvent OnCancelStatusChangeRequestCompleted;
        public event ArdsEvent OnSendStatusChangeRequestIdelCompleted;
        public event ArdsEvent OnStatusUpdatedMessage;

        public   void SendModeChangeRequestInbound(UserAuth auth)
        {
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                client.SendModeChangeRequestInboundCompleted += (s, e) =>
                {
                    try
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("SendModeChangeRequestInbound-SendModeChangeRequestInboundCompleted. Time Take :{0} , {1}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, e.Result), Logger.LogLevel.Info);
                        if (OnStatusUpdatedMessage != null)
                            OnStatusUpdatedMessage(e.Result);
                        if (OnSendModeChangeRequestInboundCompleted != null)
                            OnSendModeChangeRequestInboundCompleted(e.Result);
                        
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "ResourceRegistration[SendModeChangeRequestInboundCompleted]", exception, Logger.LogLevel.Error);
                    }
                };
                client.SendModeChangeRequestInboundAsync(auth.SecurityToken, "", "", "",CommunicationModes.WebService, "", "");
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("SendModeChangeRequestInbound. Time Take :{0} ", DateTime.Now.Subtract(startTIme).TotalMilliseconds), Logger.LogLevel.Info);
                
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "SendModeChangeRequestInbound [-------ResourceProxy-----------] ", exception, Logger.LogLevel.Error);
                
            }
        }

        public   ResourceProxyReplyDataResourceProxyReply SendModeChangeRequestOutbound(UserAuth auth)
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
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "SendModeChangeRequestOutbound[-------ResourceProxy-----------]", exception, Logger.LogLevel.Error);
                return new ResourceProxyReplyDataResourceProxyReply { Command = WorkflowResultCode.Error };
            }
        }

        public   bool ResourceModeChange(UserAuth auth, string callSessionId)
        {
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                client.ResourceModeChangeCompleted += (s, e) =>
                {
                    try
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("ResourceModeChange-ResourceModeChangeCompleted. Time Take :{0} , {1}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, e.Result), Logger.LogLevel.Info);
                        if (OnStatusUpdatedMessage != null)
                            OnStatusUpdatedMessage(e.Result);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "ResourceRegistration[ResourceModeChangeCompleted]", exception, Logger.LogLevel.Error);
                    }
                };
                client.ResourceModeChangeAsync(auth.SecurityToken, null, null, null, CommunicationModes.WebService, null, null);
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("ResourceModeChange. Time Take :{0} ", DateTime.Now.Subtract(startTIme).TotalMilliseconds), Logger.LogLevel.Info); 
                return true;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "ResourceModeChange[-------ResourceProxy-----------]", exception, Logger.LogLevel.Error);
                return false;
            }
        }

        public   void ResourceRegistration(UserAuth auth, string ip)
         {
             try
             {
                 var startTIme = DateTime.Now;
                 var client = new ResourceProxyServicesClient();
                 client.ResourceRegistrationCompleted += (s, e) =>
                 {
                     try
                     {
                         var result = (e.Error == null) ? e.Result : new ResourceProxyReplyDataResourceProxyReply { Command = WorkflowResultCode.Error, ExtraData = e.Error.Message };
                         Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("ResourceRegistration-ResourceRegistrationCompleted. Time Take :{0} : Result : {1}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, result), Logger.LogLevel.Info);
                         if (OnStatusUpdatedMessage != null)
                             OnStatusUpdatedMessage(e.Result);
                         if (OnResourceRegistrationCompleted != null)
                             OnResourceRegistrationCompleted(result);
                     }
                     catch (Exception exception)
                     {
                         Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "ResourceRegistration[ResourceRegistrationCompleted]", exception, Logger.LogLevel.Error);
                     }
                 };
                 client.ResourceRegistrationAsync(auth.SecurityToken, string.Empty, string.Empty, string.Empty,
                    CommunicationModes.WebService, string.Empty,
                    string.Format("{0}~{1}~{2}~{3}", auth.UserName, auth.UserName, auth.UserName, ip));
                 Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("ResourceRegistration. Time Take :{0} ", DateTime.Now.Subtract(startTIme).TotalMilliseconds), Logger.LogLevel.Info); 
                 
             }
             catch (Exception exception)
             {
                 Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "ResourceRegistration[-------ResourceProxy-----------]", exception, Logger.LogLevel.Error);
                 
             }
        }
        
        public   void ResourceUnregistration(UserAuth auth)
         {
             try
             {
                 var startTIme = DateTime.Now;
                 var client = new refResourceProxy.ResourceProxyServicesClient();
                 var result = client.ResourceForceLogoff(auth.SecurityToken, null, null, null, CommunicationModes.WebService, null, null);
                 Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("ResourceUnregistration-ResourceForceLogoff. Time Take :{0} , {1}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, result), Logger.LogLevel.Info); 
             }
             catch (Exception exception)
             {
                 Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "ResourceUnregistration[-------ResourceProxy-----------]", exception, Logger.LogLevel.Error);
             }
        }

        public   void ResourceStatusChangeBusy(UserAuth auth, string callSessionId)
        {
            try
            {
                var startTIme = DateTime.Now;
                var client = new ResourceProxyServicesClient();
                client.ResourceStatusChangeBusyCompleted += (s, e) =>
                {
                    try
                    {
                        if (OnStatusUpdatedMessage != null)
                            OnStatusUpdatedMessage(e.Result);
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("ResourceStatusChangeBusy-ResourceStatusChangeBusy. Time Take :{0} , {1}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, e.Result), Logger.LogLevel.Info);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "ResourceRegistration[ResourceStatusChangeBusyCompleted]", exception, Logger.LogLevel.Error);
                    }
                };
                client.ResourceStatusChangeBusyAsync(auth.SecurityToken, null, null, null,
                                                                 CommunicationModes.WebService,SequenceNumberGenerator.Instance.GetNextNo(), null, callSessionId, null);
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("ResourceStatusChangeBusy. Time Take :{0} ", DateTime.Now.Subtract(startTIme).TotalMilliseconds), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "ResourceStatusChangeBusy[-------ResourceProxy-----------]", exception, Logger.LogLevel.Error);
            }
        }

        public   void ResourceStatusChangeAcw(UserAuth auth, string callSessionId)
        {
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                client.ResourceStatusChangeACWCompleted += (s, e) =>
                {
                    try
                    {
                        if (OnStatusUpdatedMessage != null)
                            OnStatusUpdatedMessage(e.Result);
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("ResourceStatusChangeACW. Time Take :{0} , {1}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, e.Result), Logger.LogLevel.Info);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "ResourceRegistration[ResourceStatusChangeACWCompleted]", exception, Logger.LogLevel.Error);
                    }
                };
                client.ResourceStatusChangeACWAsync(auth.SecurityToken, null, null, null,
                                                                 CommunicationModes.WebService, SequenceNumberGenerator.Instance.GetNextNo(), null, callSessionId, null);
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("ResourceStatusChangeACW. Time Take :{0} ", DateTime.Now.Subtract(startTIme).TotalMilliseconds), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "ResourceStatusChangeACW[-------ResourceProxy-----------]", exception, Logger.LogLevel.Error);
            }
        }

        public   void ResourceStatusChangeBreak(UserAuth auth, string sessionId)
        {
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                client.ResourceStatusChangeBreakCompleted += (s, e) =>
                {
                    try
                    {
                        if (OnStatusUpdatedMessage != null)
                            OnStatusUpdatedMessage(e.Result);
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("ResourceStatusChangeBreak-ResourceStatusChangeBreakCompleted. Time Take :{0} , {1}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, e.Result), Logger.LogLevel.Info);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "ResourceRegistration[ResourceStatusChangeBreakCompleted]", exception, Logger.LogLevel.Error);
                    }
                };
                client.ResourceStatusChangeBreakAsync(auth.SecurityToken, null, null, null,
                                                                 CommunicationModes.WebService, SequenceNumberGenerator.Instance.GetNextNo(), null, sessionId, null);
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("ResourceStatusChangeBreak. Time Take :{0} ", DateTime.Now.Subtract(startTIme).TotalMilliseconds), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "ResourceStatusChangeBreak[-------ResourceProxy-----------]", exception, Logger.LogLevel.Error);
            }
        }

        public   void SendStatusChangeRequestBreak(UserAuth auth, string sessionId, string breakReason)
        {
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                client.SendStatusChangeRequestBreakCompleted += (s, e) =>
                {
                    try
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("SendStatusChangeRequestBreak-SendStatusChangeRequestBreakCompleted. Time Take :{0} , {1}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, e.Result), Logger.LogLevel.Info);
                        if (OnStatusUpdatedMessage != null)
                            OnStatusUpdatedMessage(e.Result); 
                        if (OnSendStatusChangeRequestBreakCompleted != null)
                            OnSendStatusChangeRequestBreakCompleted(e.Result);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "ResourceRegistration[SendStatusChangeRequestBreakCompleted]", exception, Logger.LogLevel.Error);
                    }
                };
                client.SendStatusChangeRequestBreakAsync(auth.SecurityToken, string.Empty, string.Empty,string.Empty, CommunicationModes.WebService,SequenceNumberGenerator.Instance.GetNextNo(), auth.guUserId.ToString(),breakReason,
                    auth.CompanyID.ToString(), sessionId);
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("SendStatusChangeRequestBreak. Time Take :{0}", DateTime.Now.Subtract(startTIme).TotalMilliseconds), Logger.LogLevel.Info);
                
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "SendStatusChangeRequestBreak[-------ResourceProxy-----------]", exception, Logger.LogLevel.Error);
                
            }
        }

        public   void CancelBreakRequest(UserAuth auth, string sessionId)
        {
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                client.CancelStatusChangeRequestCompleted += (s, e) =>
                {
                    try
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("CancelBreakRequest-CancelStatusChangeRequestCompleted. Time Take :{0} , {1}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, e.Result), Logger.LogLevel.Info);
                        if (OnStatusUpdatedMessage != null)
                            OnStatusUpdatedMessage(e.Result); 
                        if (OnCancelStatusChangeRequestCompleted != null)
                            OnCancelStatusChangeRequestCompleted(e.Result);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "ResourceRegistration[CancelStatusChangeRequestCompleted]", exception, Logger.LogLevel.Error);
                    }
                };
                client.CancelStatusChangeRequestAsync(auth.SecurityToken, string.Empty, string.Empty, string.Empty, CommunicationModes.WebService, auth.guUserId.ToString(),string.Empty);
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("CancelBreakRequest. Time Take :{0} ", DateTime.Now.Subtract(startTIme).TotalMilliseconds), Logger.LogLevel.Info);
                
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "CancelBreakRequest[-------ResourceProxy-----------]", exception, Logger.LogLevel.Error);
                
            }
        }

        public   void SendStatusChangeRequestIdel(UserAuth auth, string callSessionId)
        {
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                client.SendStatusChangeRequestIdelCompleted += (s, e) =>
                {
                    try
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("SendStatusChangeRequestIdel-SendStatusChangeRequestIdelCompleted. Time Take :{0} , {1}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, e.Result), Logger.LogLevel.Info);
                        if (OnStatusUpdatedMessage != null)
                            OnStatusUpdatedMessage(e.Result);
                        if (OnSendStatusChangeRequestIdelCompleted != null)
                            OnSendStatusChangeRequestIdelCompleted(e.Result);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "ResourceRegistration[SendStatusChangeRequestIdelCompleted]", exception, Logger.LogLevel.Error);
                    }
                };
                client.SendStatusChangeRequestIdelAsync(auth.SecurityToken, null, null, null, CommunicationModes.WebService,SequenceNumberGenerator.Instance.GetNextNo(), null, null, null, callSessionId);
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("SendStatusChangeRequestIdel. Time Take :{0} ", DateTime.Now.Subtract(startTIme).TotalMilliseconds), Logger.LogLevel.Info);
                
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "SendStatusChangeRequestIdel[-------ResourceProxy-----------]", exception, Logger.LogLevel.Error);
                
            }
        }
    }
}
