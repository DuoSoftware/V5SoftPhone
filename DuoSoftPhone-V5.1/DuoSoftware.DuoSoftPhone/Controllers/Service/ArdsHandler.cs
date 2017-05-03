using System;
using DuoSoftware.DuoTools.DuoLogger;
using DuoSoftware.DuoSoftPhone.refResourceProxy;
using DuoSoftware.DuoSoftPhone.refUserAuth;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    public class ArdsHandler : IResourceHandler
    {


        public event ArdsEvent OnResourceRegistrationCompleted;
        public event ArdsEvent OnSendModeChangeRequestInboundCompleted;
        public event ArdsEvent OnSendModeChangeRequestOutboundCompleted;
        public event ArdsEvent OnSendStatusChangeRequestBreakCompleted;
        public event ArdsEvent OnSendResourceChangeBreakCompleted;
        public event ArdsEvent OnCancelBreakRequestCompleted;

        public event ArdsEvent OnSendStatusChangeRequestIdelCompleted;
        public event ArdsEvent OnStatusUpdatedMessage;
        public event ArdsEvent OnResourceModeChanged;

        public void ResourceRegistration(UserAuth auth, string ip)
        {
            var id = Guid.NewGuid();
            try
            {
                var startTIme = DateTime.Now;
                var client = new ResourceProxyServicesClient();
                client.ResourceRegistrationCompleted += (s, e) =>
                {
                    try
                    {
                        var result = (e.Error == null) ? e.Result : new ResourceProxyReplyDataResourceProxyReply { Command = WorkflowResultCode.Error, ExtraData = e.Error.Message };
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceRegistration-ResourceRegistrationCompleted. Time Take :{0}, id : {1}, Result : {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, result), Logger.LogLevel.Info);
                        if (OnStatusUpdatedMessage != null)
                            OnStatusUpdatedMessage(e.Result);
                        if (OnResourceRegistrationCompleted != null)
                            OnResourceRegistrationCompleted(result);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceRegistration[ResourceRegistrationCompleted], id : {0}", id), exception, Logger.LogLevel.Error);
                    }
                };
                var sqid = SequenceNumberGenerator.Instance.GetNextNo();
                client.ResourceRegistrationAsync(auth.SecurityToken, "DuoDialer-win", string.Empty, string.Empty,
                   CommunicationModes.WebService, sqid, string.Empty,
                   string.Format("{0}~{1}~{2}~{3}", auth.UserName, auth.UserName, auth.UserName, ip));
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceRegistration. Time Take :{0} , id : {1}, sqid: {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, sqid), Logger.LogLevel.Info);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceRegistration[-------ResourceProxy-----------], id: {0}", id), exception, Logger.LogLevel.Error);

            }
        }

        public void ResourceUnregistration(UserAuth auth)
        {
            var id = Guid.NewGuid();
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                var sqid = SequenceNumberGenerator.Instance.GetNextNo();
                var result = client.ResourceForceLogoff(auth.SecurityToken, null, null, null, CommunicationModes.WebService, sqid, null, null);
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceUnregistration-ResourceForceLogoff. Time Take :{0} , id : {1}, sqid : {2},{3}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, sqid, result), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceUnregistration[-------ResourceProxy-----------], id : {0}", id), exception, Logger.LogLevel.Error);
            }
        }

        #region Mode

        public void SendModeChangeRequestInbound(UserAuth auth)
        {
            var id = Guid.NewGuid();
            try
            {

                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                client.SendModeChangeRequestInboundCompleted += (s, e) =>
                {
                    try
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("SendModeChangeRequestInbound-SendModeChangeRequestInboundCompleted. Time Take :{0} , id : {1}, {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, e.Result), Logger.LogLevel.Info);
                        if (OnStatusUpdatedMessage != null)
                            OnStatusUpdatedMessage(e.Result);
                        if (OnSendModeChangeRequestInboundCompleted != null)
                            OnSendModeChangeRequestInboundCompleted(e.Result);

                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceRegistration[SendModeChangeRequestInboundCompleted] , id : {0}", id), exception, Logger.LogLevel.Error);
                    }
                };
                var sqid = SequenceNumberGenerator.Instance.GetNextNo();
                client.SendModeChangeRequestInboundAsync(auth.SecurityToken, "", "", "", CommunicationModes.WebService, sqid, "", "");
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("SendModeChangeRequestInbound. Time Take :{0} , id : {1}, sqid : {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, sqid), Logger.LogLevel.Info);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "SendModeChangeRequestInbound [-------ResourceProxy-----------] ,  id: " + id, exception, Logger.LogLevel.Error);

            }
        }

        public void SendModeChangeRequestOutbound(UserAuth auth)
        {
            var id = Guid.NewGuid();
            try
            {

                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();

                client.SendModeChangeRequestOutboundCompleted += (s, e) =>
                {
                    try
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("SendModeChangeRequestOutboundCompleted-. Time Take :{0} ,Id : {1} , {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, e.Result), Logger.LogLevel.Info);
                        if (OnSendModeChangeRequestOutboundCompleted != null)
                            OnSendModeChangeRequestOutboundCompleted(e.Result);

                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("SendModeChangeRequestOutboundCompleted. id : {0}", id), exception, Logger.LogLevel.Error);
                    }
                };
                var sqid = SequenceNumberGenerator.Instance.GetNextNo();
                client.SendModeChangeRequestOutboundAsync(auth.SecurityToken, "", "", "", CommunicationModes.WebService, sqid, "", "");
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("SendModeChangeRequestOutbound. Time Take :{0} , id : {1} , sqid : {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, sqid), Logger.LogLevel.Info);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("SendModeChangeRequestOutbound[-------ResourceProxy-----------] , id : {0}", id), exception, Logger.LogLevel.Error);
                //return new ResourceProxyReplyDataResourceProxyReply { Command = WorkflowResultCode.Error };
            }
        }

        public bool ResourceModeChangeNew(UserAuth auth, string callSessionId)
        {
            var id = Guid.NewGuid();
            try
            {

                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                var sqid = SequenceNumberGenerator.Instance.GetNextNo();
                var result = client.ResourceModeChange(auth.SecurityToken, null, null, null, CommunicationModes.WebService, sqid, null, null);
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceModeChange. Time Take :{0}  SID : {1} , id : {2} , sqid :{4}  result : {3}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, callSessionId, id, result, sqid), Logger.LogLevel.Info);
                return result.Command == WorkflowResultCode.ACDS601;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceModeChange[-------ResourceProxy-----------] , id : {0}", id), exception, Logger.LogLevel.Error);
                return false;
            }
        }

        public bool ResourceModeChange(UserAuth auth, string callSessionId)
        {
            var id = Guid.NewGuid();
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                client.ResourceModeChangeCompleted += (s, e) =>
                {
                    try
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceModeChange-ResourceModeChangeCompleted. Time Take :{0} , id : {1}, {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, e.Result), Logger.LogLevel.Info);
                        if (OnStatusUpdatedMessage != null)
                            OnStatusUpdatedMessage(e.Result);
                        if (OnResourceModeChanged != null)
                            OnResourceModeChanged(e.Result);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceRegistration[ResourceModeChangeCompleted], id : {0}", id), exception, Logger.LogLevel.Error);
                    }
                };
                var sqid = SequenceNumberGenerator.Instance.GetNextNo();
                client.ResourceModeChangeAsync(auth.SecurityToken, null, null, null, CommunicationModes.WebService, sqid, null, null);
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceModeChange. Time Take :{0} id : {1},  SID : {2}, sqid : {3}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, callSessionId, sqid), Logger.LogLevel.Info);
                return true;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceModeChange[-------ResourceProxy-----------], id: {0}", id), exception, Logger.LogLevel.Error);
                return false;
            }
        }

        #endregion

        public void ResourceStatusChangeBusy(UserAuth auth, string callSessionId, bool isCallAnswer)
        {
            var id = Guid.NewGuid();
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
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceStatusChangeBusy-ResourceStatusChangeBusy. Time Take :{0} , id : {1} , {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, e.Result), Logger.LogLevel.Info);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceRegistration[ResourceStatusChangeBusyCompleted], id : {0}", id), exception, Logger.LogLevel.Error);
                    }
                };

                var sqid = SequenceNumberGenerator.Instance.GetNextNo();
                var exDate = "{\"IsCallAnswer\":" + isCallAnswer + "}";
                client.ResourceStatusChangeBusyAsync(auth.SecurityToken, null, null, null, CommunicationModes.WebService, sqid, null, callSessionId, exDate);
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceStatusChangeBusy. Time Take :{0} , id : {1} , SID : {2}, sqid : {3}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, callSessionId, sqid), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "ResourceStatusChangeBusy[-------ResourceProxy-----------]", exception, Logger.LogLevel.Error);
            }
        }

        public void ResourceStatusChangeAcw(UserAuth auth, string callSessionId, bool isCallAnswer)
        {
            var id = Guid.NewGuid();
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                client.ResourceStatusChangeACWCompleted += (s, e) =>
                {
                    try
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceStatusChangeACWCompleted. Time Take :{0} , id : {1}, {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, e.Result), Logger.LogLevel.Info);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "ResourceStatusChangeAcw[ResourceStatusChangeACWCompleted]", exception, Logger.LogLevel.Error);
                    }
                };

                var sqid = SequenceNumberGenerator.Instance.GetNextNo();
                var exDate = "{\"IsCallAnswer\":" + isCallAnswer + "}";
                client.ResourceStatusChangeACWAsync(auth.SecurityToken, null, null, null,
                                                                 CommunicationModes.WebService, sqid, null, callSessionId, exDate);
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceStatusChangeAcw. Time Take :{0} , id : {1}, SID : {2}, sqid : {3}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, callSessionId, sqid), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceStatusChangeACW[-------ResourceProxy-----------], id : {0}", id), exception, Logger.LogLevel.Error);
            }
        }

        #region Break

        public void SendStatusChangeRequestBreak(UserAuth auth, string sessionId, string breakReason)
        {
            var id = Guid.NewGuid();
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                client.SendStatusChangeRequestBreakCompleted += (s, e) =>
                {
                    try
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("SendStatusChangeRequestBreak-SendStatusChangeRequestBreakCompleted. Time Take :{0} ,id : {1}, {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, e.Result), Logger.LogLevel.Info);
                        if (OnStatusUpdatedMessage != null)
                            OnStatusUpdatedMessage(e.Result);
                        if (OnSendStatusChangeRequestBreakCompleted != null)
                            OnSendStatusChangeRequestBreakCompleted(e.Result);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceRegistration[SendStatusChangeRequestBreakCompleted], id : {0}", id), exception, Logger.LogLevel.Error);
                    }
                };
                var sqid = SequenceNumberGenerator.Instance.GetNextNo();
                client.SendStatusChangeRequestBreakAsync(auth.SecurityToken, string.Empty, string.Empty, string.Empty, CommunicationModes.WebService, sqid, auth.guUserId.ToString(), breakReason,
                    auth.CompanyID.ToString(), sessionId);
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceStatusChangeACWCompletedResourceStatusChangeACWCompleted. Time Take :{0}, id : {1}, SID : {2}, sqid : {3}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, sessionId, sqid), Logger.LogLevel.Info);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("SendStatusChangeRequestBreak[-------ResourceProxy-----------], id: {0}", id), exception, Logger.LogLevel.Error);

            }
        }

        public void CancelBreakRequest(UserAuth auth, string sessionId)
        {
            var id = Guid.NewGuid();
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                client.CancelStatusChangeRequestCompleted += (s, e) =>
                {
                    try
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("CancelBreakRequest-CancelStatusChangeRequestCompleted. Time Take :{0} ,id: {1}, {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, e.Result), Logger.LogLevel.Info);
                        if (OnStatusUpdatedMessage != null)
                            OnStatusUpdatedMessage(e.Result);
                        if (OnCancelBreakRequestCompleted != null)
                            OnCancelBreakRequestCompleted(e.Result);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceRegistration[CancelStatusChangeRequestCompleted], id : {0}", id), exception, Logger.LogLevel.Error);
                    }
                };
                var sqid = SequenceNumberGenerator.Instance.GetNextNo();
                client.CancelStatusChangeRequestAsync(auth.SecurityToken, string.Empty, string.Empty, string.Empty, CommunicationModes.WebService, sqid, auth.guUserId.ToString(), string.Empty);
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("CancelBreakRequest. Time Take :{0}, id : {1}, SID : {2}, sqid : {3}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, sessionId, sqid), Logger.LogLevel.Info);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("CancelBreakRequest[-------ResourceProxy-----------], id : {0}", id), exception, Logger.LogLevel.Error);

            }
        }

        public void ResourceStatusChangeBreak(UserAuth auth, string sessionId)
        {
            var id = Guid.NewGuid();
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                client.ResourceStatusChangeBreakCompleted += (s, e) =>
                {
                    try
                    {
                        if (OnSendResourceChangeBreakCompleted != null)
                            OnSendResourceChangeBreakCompleted(e.Result);
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceStatusChangeBreak-ResourceStatusChangeBreakCompleted. Time Take :{0} ,id : {1}, {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, e.Result), Logger.LogLevel.Info);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceRegistration[ResourceStatusChangeBreakCompleted], id : {0}", id), exception, Logger.LogLevel.Error);
                    }
                };

                var sqid = SequenceNumberGenerator.Instance.GetNextNo();
                client.ResourceStatusChangeBreakAsync(auth.SecurityToken, null, null, null,
                                                                 CommunicationModes.WebService, sqid, null, sessionId, null);
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceStatusChangeBreak. Time Take :{0}, id : {1} , SID : {2}, sqid : {3}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, sessionId, sqid), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceStatusChangeBreak[-------ResourceProxy-----------], id: {0}", id), exception, Logger.LogLevel.Error);
            }
        }

        #endregion

        public void SendStatusChangeRequestIdel(UserAuth auth, string callSessionId, bool isCallAnswer)
        {
            var id = Guid.NewGuid();
            try
            {
                var startTIme = DateTime.Now;
                var client = new refResourceProxy.ResourceProxyServicesClient();
                client.SendStatusChangeRequestIdelCompleted += (s, e) =>
                {
                    try
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("SendStatusChangeRequestIdel-SendStatusChangeRequestIdelCompleted. Time Take :{0} , id : {1}, {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, e.Result), Logger.LogLevel.Info);
                        if (OnStatusUpdatedMessage != null)
                            OnStatusUpdatedMessage(e.Result);
                        if (OnSendStatusChangeRequestIdelCompleted != null)
                            OnSendStatusChangeRequestIdelCompleted(e.Result);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceRegistration[SendStatusChangeRequestIdelCompleted], id : {0}", id), exception, Logger.LogLevel.Error);
                    }
                };
                var sqid = SequenceNumberGenerator.Instance.GetNextNo();
                var exDate = "{\"IsCallAnswer\":" + isCallAnswer + "}";
                client.SendStatusChangeRequestIdelAsync(auth.SecurityToken, null, null, null, CommunicationModes.WebService, sqid, null, null, exDate, callSessionId);
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("SendStatusChangeRequestIdel. Time Take :{0}, id : {1}, SID : {2}, sqid : {3}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, callSessionId, sqid), Logger.LogLevel.Info);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("SendStatusChangeRequestIdel[-------ResourceProxy-----------],id : {0}", id), exception, Logger.LogLevel.Error);
            }
        }
    }
}
