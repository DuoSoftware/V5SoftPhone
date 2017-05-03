using DuoSoftware.DuoTools.DuoLogger;
using System;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    public class CallHandler
    {
        public CommonDataHandler.SipServiceCall SipServiceCall;
        private readonly Logger _logger;
        
        /// <summary>
        ///
        /// </summary>
        /// <param name="securityToken"></param>
        /// <param name="tenantId"></param>
        /// <param name="companyId"></param>
        public CallHandler(string securityToken, int tenantId, int companyId)
        {
            SipServiceCall = CommonDataHandler.GetEventInfo(securityToken, tenantId, companyId);
            _logger = Logger.Instance;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sipNumber"></param>
        public void DialCall(string sipNumber)
        {
            try
            {
                if (!SipServiceCall.Dial) return;
                var startTIme = DateTime.Now;
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, String.Format("Not implement in CcDispatch-HoldCall:{0}", sipNumber), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                _logger.LogMessage(Logger.LogAppender.DuoDefault, "HoldCall", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool AutoAnswerByDefault()
        {
            return SipServiceCall.AutoAnswerByDefault;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool AutoAnswerCanAgentEnable()
        {
            return SipServiceCall.AutoAnswerCanAgentEnable;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="securityToken"></param>
        public void EndCall(string sessionId, string securityToken)
        {
            try
            {
                if (!SipServiceCall.Reject) return;
                var startTIme = DateTime.Now;
                var client = new RefCcDispatch.CallMonitorClient("CCCallMonitorEp");
                client.DisconnectCallCompleted += (s, e) =>
                {
                    try
                    {
                        _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("Framework-DisconnectCallCompleted. Time Take :{0} ,. sessionId : {1} , Reply : {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, sessionId, e.Result), Logger.LogLevel.Info);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogMessage(Logger.LogAppender.DuoDefault, "DisconnectCallCompleted", e.Error, Logger.LogLevel.Error);
                        _logger.LogMessage(Logger.LogAppender.DuoDefault, "DisconnectCallCompleted", exception, Logger.LogLevel.Error);
                    }
                };
                client.DisconnectCallAsync(sessionId, securityToken);
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("Framework-endcall. Time Take :{0} ,. sessionId : {1} , Reply : {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, sessionId, "reply"), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                _logger.LogMessage(Logger.LogAppender.DuoDefault, "EndCall", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="securityToken"></param>
        public void HoldCall(string sessionId, string securityToken)
        {
            try
            {
                if (!SipServiceCall.Hold) return;
                var startTIme = DateTime.Now;
                _logger.LogMessage(Logger.LogAppender.DuoLogger1,string.Format("Not implement in CcDispatch-HoldCall.{0}:{1}",sessionId,securityToken), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                _logger.LogMessage(Logger.LogAppender.DuoDefault, "HoldCall", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="securityToken"></param>
        public void UnHoldCall(string sessionId, string securityToken)
        {
            try
            {
                if (!SipServiceCall.UnHold) return;
                var startTIme = DateTime.Now;
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("Not implement in CcDispatch-UnHoldCall.{0}:{1}",sessionId,securityToken), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                _logger.LogMessage(Logger.LogAppender.DuoDefault, "UnHoldCall", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="securityToken"></param>
        public void AnswerCall(string sessionId, string securityToken)
        {
            try
            {
                if (!SipServiceCall.Answer) return;
                var startTIme = DateTime.Now;
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("Not implement in CcDispatch-AnswerCall.{0}:{1}", sessionId, securityToken), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                _logger.LogMessage(Logger.LogAppender.DuoDefault, "AnswerCall", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="securityToken"></param>
        public void TransferCall(string sessionId, string securityToken)
        {
            try
            {
                if (!SipServiceCall.Transfer) return;
                var startTIme = DateTime.Now;
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("Not implement in CcDispatch-TransferCall.{0}:{1}", sessionId, securityToken), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                _logger.LogMessage(Logger.LogAppender.DuoDefault, "TransferCall", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="securityToken"></param>
        public void ConferenceCall(string sessionId, string securityToken)
        {
            try
            {
                if (!SipServiceCall.Conference) return;
                var startTIme = DateTime.Now;
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("Not implement in CcDispatch-ConferenceCall.{0}:{1}", sessionId, securityToken), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                _logger.LogMessage(Logger.LogAppender.DuoDefault, "ConferenceCall", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="securityToken"></param>
        public void EtlCall(string sessionId, string securityToken)
        {
            try
            {
                if (!SipServiceCall.ETL) return;
                var startTIme = DateTime.Now;
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("Not implement in CcDispatch-ConferenceCall-ETLCall.{0}:{1}", sessionId, securityToken), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                _logger.LogMessage(Logger.LogAppender.DuoDefault, "ETLCall", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="securityToken"></param>
        public void SwapCallCall(string sessionId, string securityToken)
        {
            try
            {
                if (!SipServiceCall.SwapCall) return;
                _logger.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("Not implement in CcDispatch-ConferenceCall-SwapCallCall.{0}:{1}", sessionId, securityToken), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                _logger.LogMessage(Logger.LogAppender.DuoDefault, "SwapCallCall", exception, Logger.LogLevel.Error);
                throw;
            }
        }
    }
}