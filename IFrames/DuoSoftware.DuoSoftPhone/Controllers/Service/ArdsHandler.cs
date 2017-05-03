using DuoSoftware.DuoSoftPhone.RefResourceProxy;
using DuoSoftware.DuoSoftPhone.RefUserAuth;
using DuoSoftware.DuoTools.DuoLogger;
using System;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    /// <summary>
    ///
    /// </summary>
    public class ArdsHandler : ISoftPhoneResourceHandler
    {
        readonly Logger _logger = Logger.Instance ;
        /// <summary>
        ///
        /// </summary>
        public event ArdsEvent OnResourceRegistrationCompleted;

        /// <summary>
        ///
        /// </summary>
        public event ArdsEvent OnSendModeChangeRequestInboundCompleted;

        /// <summary>
        ///
        /// </summary>
        public event ArdsEvent OnSendModeChangeRequestOutboundCompleted;

        /// <summary>
        ///
        /// </summary>
        public event ArdsEvent OnSendStatusChangeRequestBreakCompleted;

        /// <summary>
        ///
        /// </summary>
        public event ArdsEvent OnSendResourceChangeBreakCompleted;

        /// <summary>
        ///
        /// </summary>
        public event ArdsEvent OnCancelBreakRequestCompleted;

        /// <summary>
        ///
        /// </summary>
        public event ArdsEvent OnSendStatusChangeRequestIdelCompleted;

        /// <summary>
        ///
        /// </summary>
        public event ArdsEvent OnStatusUpdatedMessage;

        /// <summary>
        ///
        /// </summary>
        public event ArdsEvent OnResourceModeChanged;

        /// <summary>
        ///
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="ip"></param>
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
                        _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceRegistration-ResourceRegistrationCompleted. Time Take :{0}, id : {1}, Result : {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, result), Logger.LogLevel.Info);
                        if (OnStatusUpdatedMessage != null)
                            OnStatusUpdatedMessage(e.Result);
                        if (OnResourceRegistrationCompleted != null)
                            OnResourceRegistrationCompleted(result);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceRegistration[ResourceRegistrationCompleted], id : {0}", id), exception, Logger.LogLevel.Error);
                    }
                };
                var sqid = SequenceNumberGenerator.Instance.GetNextNo;
                if (auth != null)
                    client.ResourceRegistrationAsync(auth.SecurityToken, "DuoDialer-win", string.Empty, string.Empty,
                        CommunicationModes.WebService, sqid, string.Empty,
                        string.Format("{0}~{1}~{2}~{3}", auth.UserName, auth.UserName, auth.UserName, ip));
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceRegistration. Time Take :{0} , id : {1}, sqid: {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, sqid), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceRegistration[-------ResourceProxy-----------], id: {0}", id), exception, Logger.LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="auth"></param>
        public void ResourceUnregistration(UserAuth auth)
        {
            var id = Guid.NewGuid();
            try
            {
                var startTIme = DateTime.Now;
                var client = new ResourceProxyServicesClient();
                var sqid = SequenceNumberGenerator.Instance.GetNextNo;
                if (auth == null) return;
                var result = client.ResourceForceLogoff(auth.SecurityToken, null, null, null, CommunicationModes.WebService, sqid, null, null);
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceUnregistration-ResourceForceLogoff. Time Take :{0} , id : {1}, sqid : {2},{3}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, sqid, result), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceUnregistration[-------ResourceProxy-----------], id : {0}", id), exception, Logger.LogLevel.Error);
                throw;
            }
        }

        #region Mode

        /// <summary>
        ///
        /// </summary>
        /// <param name="auth"></param>
        public void SendModeChangeRequestInbound(UserAuth auth)
        {
            var id = Guid.NewGuid();
            try
            {
                var startTIme = DateTime.Now;
                var client = new ResourceProxyServicesClient();
                client.SendModeChangeRequestInboundCompleted += (s, e) =>
                {
                    try
                    {
                        _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("SendModeChangeRequestInbound-SendModeChangeRequestInboundCompleted. Time Take :{0} , id : {1}, {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, e.Result), Logger.LogLevel.Info);
                        if (OnStatusUpdatedMessage != null)
                            OnStatusUpdatedMessage(e.Result);
                        if (OnSendModeChangeRequestInboundCompleted != null)
                            OnSendModeChangeRequestInboundCompleted(e.Result);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceRegistration[SendModeChangeRequestInboundCompleted] , id : {0}", id), exception, Logger.LogLevel.Error);
                    }
                };
                var sqid = SequenceNumberGenerator.Instance.GetNextNo;
                if (auth != null)
                    client.SendModeChangeRequestInboundAsync(auth.SecurityToken, string.Empty, string.Empty, string.Empty, CommunicationModes.WebService, sqid, string.Empty, string.Empty);
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("SendModeChangeRequestInbound. Time Take :{0} , id : {1}, sqid : {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, sqid), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, "SendModeChangeRequestInbound [-------ResourceProxy-----------] ,  id: " + id, exception, Logger.LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="auth"></param>
        public void SendModeChangeRequestOutbound(UserAuth auth)
        {
            var id = Guid.NewGuid();
            try
            {
                var startTIme = DateTime.Now;
                var client = new ResourceProxyServicesClient();

                client.SendModeChangeRequestOutboundCompleted += (s, e) =>
                {
                    try
                    {
                        _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("SendModeChangeRequestOutboundCompleted-. Time Take :{0} ,Id : {1} , {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, e.Result), Logger.LogLevel.Info);
                        if (OnSendModeChangeRequestOutboundCompleted != null)
                            OnSendModeChangeRequestOutboundCompleted(e.Result);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("SendModeChangeRequestOutboundCompleted. id : {0}", id), exception, Logger.LogLevel.Error);
                    }
                };
                var sqid = SequenceNumberGenerator.Instance.GetNextNo;
                if (auth != null)
                    client.SendModeChangeRequestOutboundAsync(auth.SecurityToken, string.Empty, string.Empty, string.Empty, CommunicationModes.WebService, sqid, string.Empty, string.Empty);
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("SendModeChangeRequestOutbound. Time Take :{0} , id : {1} , sqid : {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, sqid), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("SendModeChangeRequestOutbound[-------ResourceProxy-----------] , id : {0}", id), exception, Logger.LogLevel.Error);
                throw;
                //return new ResourceProxyReplyDataResourceProxyReply { Command = WorkflowResultCode.Error };
            }
        }

        /*
        /// <summary>
        ///
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="callSessionId"></param>
        /// <returns></returns>
        public bool ResourceModeChangeNew(UserAuth auth, string callSessionId)
        {
            var id = Guid.NewGuid();
            try
            {
                var startTIme = DateTime.Now;
                var client = new ResourceProxyServicesClient();
                var sqid = SequenceNumberGenerator.Instance.GetNextNo;
                if (auth != null)
                {
                    var result = client.ResourceModeChange(auth.SecurityToken, null, null, null, CommunicationModes.WebService, sqid, null, null);
                    _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceModeChange. Time Take :{0}  SID : {1} , id : {2} , sqid :{4}  result : {3}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, callSessionId, id, result, sqid), Logger.LogLevel.Info);
                    return result.Command == WorkflowResultCode.ACDS601;
                }
            }
            catch (Exception exception)
            {
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceModeChange[-------ResourceProxy-----------] , id : {0}", id), exception, Logger.LogLevel.Error);
                throw;

            } return false;
        }*/

        /// <summary>
        ///
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="callSessionId"></param>
        /// <returns></returns>
        public bool ResourceModeChange(UserAuth auth, string callSessionId)
        {
            var id = Guid.NewGuid();
            try
            {
                var startTIme = DateTime.Now;
                var client = new ResourceProxyServicesClient();
                client.ResourceModeChangeCompleted += (s, e) =>
                {
                    try
                    {
                        _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceModeChange-ResourceModeChangeCompleted. Time Take :{0} , id : {1}, {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, e.Result), Logger.LogLevel.Info);
                        if (OnStatusUpdatedMessage != null)
                            OnStatusUpdatedMessage(e.Result);
                        if (OnResourceModeChanged != null)
                            OnResourceModeChanged(e.Result);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceRegistration[ResourceModeChangeCompleted], id : {0}", id), exception, Logger.LogLevel.Error);
                    }
                };
                var sqid = SequenceNumberGenerator.Instance.GetNextNo;
                if (auth != null)
                    client.ResourceModeChangeAsync(auth.SecurityToken, null, null, null, CommunicationModes.WebService, sqid, null, null);
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceModeChange. Time Take :{0} id : {1},  SID : {2}, sqid : {3}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, callSessionId, sqid), Logger.LogLevel.Info);
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceModeChange[-------ResourceProxy-----------], id: {0}", id), exception, Logger.LogLevel.Error);
                throw;
            }
        }

        #endregion Mode

        /// <summary>
        ///
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="callSessionId"></param>
        /// <param name="isCallAnswer"></param>
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
                        _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceStatusChangeBusy-ResourceStatusChangeBusy. Time Take :{0} , id : {1} , {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, e.Result), Logger.LogLevel.Info);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceRegistration[ResourceStatusChangeBusyCompleted], id : {0}", id), exception, Logger.LogLevel.Error);
                    }
                };
                var exDate = "{\"IsCallAnswer\":" + isCallAnswer + "}";
                var sqid = SequenceNumberGenerator.Instance.GetNextNo;
                if (auth != null)
                    client.ResourceStatusChangeBusyAsync(auth.SecurityToken, null, null, null, CommunicationModes.WebService, sqid, null, callSessionId, exDate);
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceStatusChangeBusy. Time Take :{0} , id : {1} , SID : {2}, sqid : {3}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, callSessionId, sqid), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, "ResourceStatusChangeBusy[-------ResourceProxy-----------]", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="callSessionId"></param>
        /// <param name="isCallAnswer"></param>
        public void ResourceStatusChangeAcw(UserAuth auth, string callSessionId, bool isCallAnswer)
        {
            var id = Guid.NewGuid();
            try
            {
                var startTIme = DateTime.Now;
                var client = new ResourceProxyServicesClient();
                client.ResourceStatusChangeACWCompleted += (s, e) =>
                {
                    try
                    {
                        _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceStatusChangeACWCompleted. Time Take :{0} , id : {1}, {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, e.Result), Logger.LogLevel.Info);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogMessage(Logger.LogAppender.DuoLogger1, "ResourceStatusChangeAcw[ResourceStatusChangeACWCompleted]", exception, Logger.LogLevel.Error);
                    }
                };

                var sqid = SequenceNumberGenerator.Instance.GetNextNo;
                var exDate = "{\"IsCallAnswer\":" + isCallAnswer + "}";
                if (auth != null)
                    client.ResourceStatusChangeACWAsync(auth.SecurityToken, null, null, null,
                        CommunicationModes.WebService, sqid, null, callSessionId, exDate);
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceStatusChangeAcw. Time Take :{0} , id : {1}, SID : {2}, sqid : {3}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, callSessionId, sqid), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceStatusChangeACW[-------ResourceProxy-----------], id : {0}", id), exception, Logger.LogLevel.Error);
                throw;
            }
        }

        #region Break

        /// <summary>
        ///
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="sessionId"></param>
        /// <param name="breakReason"></param>
        public void SendStatusChangeRequestBreak(UserAuth auth, string sessionId, string breakReason)
        {
            var id = Guid.NewGuid();
            try
            {
                var startTIme = DateTime.Now;
                var client = new ResourceProxyServicesClient();
                client.SendStatusChangeRequestBreakCompleted += (s, e) =>
                {
                    try
                    {
                        _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("SendStatusChangeRequestBreak-SendStatusChangeRequestBreakCompleted. Time Take :{0} ,id : {1}, {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, e.Result), Logger.LogLevel.Info);
                        if (OnStatusUpdatedMessage != null)
                            OnStatusUpdatedMessage(e.Result);
                        if (OnSendStatusChangeRequestBreakCompleted != null)
                            OnSendStatusChangeRequestBreakCompleted(e.Result);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceRegistration[SendStatusChangeRequestBreakCompleted], id : {0}", id), exception, Logger.LogLevel.Error);
                    }
                };
                var sqid = SequenceNumberGenerator.Instance.GetNextNo;
                if (auth != null)
                    client.SendStatusChangeRequestBreakAsync(auth.SecurityToken, string.Empty, string.Empty, string.Empty, CommunicationModes.WebService, sqid, auth.guUserId.ToString(), breakReason,
                        auth.CompanyID.ToString(), sessionId);
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceStatusChangeACWCompletedResourceStatusChangeACWCompleted. Time Take :{0}, id : {1}, SID : {2}, sqid : {3}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, sessionId, sqid), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("SendStatusChangeRequestBreak[-------ResourceProxy-----------], id: {0}", id), exception, Logger.LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="sessionId"></param>
        public void CancelBreakRequest(UserAuth auth, string sessionId)
        {
            var id = Guid.NewGuid();
            try
            {
                var startTIme = DateTime.Now;
                var client = new ResourceProxyServicesClient();
                client.CancelStatusChangeRequestCompleted += (s, e) =>
                {
                    try
                    {
                        _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("CancelBreakRequest-CancelStatusChangeRequestCompleted. Time Take :{0} ,id: {1}, {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, e.Result), Logger.LogLevel.Info);
                        if (OnStatusUpdatedMessage != null)
                            OnStatusUpdatedMessage(e.Result);
                        if (OnCancelBreakRequestCompleted != null)
                            OnCancelBreakRequestCompleted(e.Result);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceRegistration[CancelStatusChangeRequestCompleted], id : {0}", id), exception, Logger.LogLevel.Error);
                    }
                };
                var sqid = SequenceNumberGenerator.Instance.GetNextNo;
                if (auth != null)
                    client.CancelStatusChangeRequestAsync(auth.SecurityToken, string.Empty, string.Empty, string.Empty, CommunicationModes.WebService, sqid, auth.guUserId.ToString(), string.Empty);
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("CancelBreakRequest. Time Take :{0}, id : {1}, SID : {2}, sqid : {3}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, sessionId, sqid), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("CancelBreakRequest[-------ResourceProxy-----------], id : {0}", id), exception, Logger.LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="sessionId"></param>
        public void ResourceStatusChangeBreak(UserAuth auth, string sessionId)
        {
            var id = Guid.NewGuid();
            try
            {
                var startTIme = DateTime.Now;
                var client = new ResourceProxyServicesClient();
                client.ResourceStatusChangeBreakCompleted += (s, e) =>
                {
                    try
                    {
                        if (OnSendResourceChangeBreakCompleted != null)
                            OnSendResourceChangeBreakCompleted(e.Result);
                        _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceStatusChangeBreak-ResourceStatusChangeBreakCompleted. Time Take :{0} ,id : {1}, {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, e.Result), Logger.LogLevel.Info);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceRegistration[ResourceStatusChangeBreakCompleted], id : {0}", id), exception, Logger.LogLevel.Error);
                    }
                };

                var sqid = SequenceNumberGenerator.Instance.GetNextNo;
                if (auth != null)
                    client.ResourceStatusChangeBreakAsync(auth.SecurityToken, null, null, null,
                        CommunicationModes.WebService, sqid, null, sessionId, null);
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceStatusChangeBreak. Time Take :{0}, id : {1} , SID : {2}, sqid : {3}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, sessionId, sqid), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceStatusChangeBreak[-------ResourceProxy-----------], id: {0}", id), exception, Logger.LogLevel.Error);
                throw;
            }
        }

        #endregion Break

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="auth"></param>
        /// <param name="callSessionId"></param>
        /// <param name="isCallAnswer"></param>
        /// <param name="transferCall"></param>
        public void SendStatusChangeRequestIdel(UserAuth auth, string callSessionId, bool isCallAnswer, bool transferCall)
        {
            var id = Guid.NewGuid();
            try
            {
                var startTIme = DateTime.Now;
                var client = new ResourceProxyServicesClient();
                client.SendStatusChangeRequestIdelCompleted += (s, e) =>
                {
                    try
                    {
                        _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("SendStatusChangeRequestIdel-SendStatusChangeRequestIdelCompleted. Time Take :{0} , id : {1}, {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, e.Result), Logger.LogLevel.Info);
                        if (OnStatusUpdatedMessage != null)
                            OnStatusUpdatedMessage(e.Result);
                        if (OnSendStatusChangeRequestIdelCompleted != null)
                            OnSendStatusChangeRequestIdelCompleted(e.Result);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("ResourceRegistration[SendStatusChangeRequestIdelCompleted], id : {0}", id), exception, Logger.LogLevel.Error);
                    }
                };
                var sqid = SequenceNumberGenerator.Instance.GetNextNo;
                var exDate = "{\"IsCallAnswer\":"+isCallAnswer+",\"transferCall\":"+transferCall+"}";
                if (auth != null)
                    client.SendStatusChangeRequestIdelAsync(auth.SecurityToken, null, null, null, CommunicationModes.WebService, sqid, null, null, exDate, callSessionId);
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("SendStatusChangeRequestIdel. Time Take :{0}, id : {1}, SID : {2}, sqid : {3}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, id, callSessionId, sqid), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("SendStatusChangeRequestIdel[-------ResourceProxy-----------],id : {0}", id), exception, Logger.LogLevel.Error);
                throw;
            }
        }
    }
}