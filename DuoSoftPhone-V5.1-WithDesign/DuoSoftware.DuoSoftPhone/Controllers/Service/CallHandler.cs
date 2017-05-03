using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuoSoftware.DuoLogger;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    public class CallHandler
    {
        public CommonDataHandler.SipServiceCall _sipServiceCall; 
        
        public CallHandler(string securityToken, int tenantId, int companyId)
        {
            _sipServiceCall = CommonDataHandler.GetEventInfo(securityToken,tenantId,companyId);
        }

        public void DialCall(string sipNumber)
        {
            try
            {
                if (!_sipServiceCall.Dial) return;
                var startTIme = DateTime.Now;
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "Not implement in CcDispatch-HoldCall", Logger.LogLevel.Info);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "HoldCall", exception, Logger.LogLevel.Error);
            }
        }

        public  bool AutoAnswerByDefault()
        {
            return _sipServiceCall.AutoAnswerByDefault;
        }

        public  bool AutoAnswerCanAgentEnable()
        {
            return _sipServiceCall.AutoAnswerCanAgentEnable;
        }

        public  void EndCall(string sessionId, string securityToken)
        {
            try
            {
                if (!_sipServiceCall.Reject) return;
                var startTIme = DateTime.Now;
                var client = new refCcDispatch.CallMonitorClient("CCCallMonitorEp");
                client.DisconnectCallCompleted += (s, e) =>
                {
                    try
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, string.Format("Framework-DisconnectCallCompleted. Time Take :{0} ,. sessionId : {1} , Reply : {2}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, sessionId, e.Result), Logger.LogLevel.Info);
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "DisconnectCallCompleted", e.Error, Logger.LogLevel.Error);
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "DisconnectCallCompleted", exception, Logger.LogLevel.Error);
                    }
                };
                client.DisconnectCallAsync(sessionId, securityToken);
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1,string.Format("Framework-endcall. Time Take :{0} ,. sessionId : {1} , Reply : {2}",DateTime.Now.Subtract(startTIme).TotalMilliseconds, sessionId, "reply"), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "EndCall", exception, Logger.LogLevel.Error);
            }
        }

        public  void HoldCall(string sessionId, string securityToken)
        {
            try
            {
                if (!_sipServiceCall.Hold) return;
                var startTIme = DateTime.Now;
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "Not implement in CcDispatch-HoldCall", Logger.LogLevel.Info);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "HoldCall", exception, Logger.LogLevel.Error);
            }
        }

        public  void UnHoldCall(string sessionId, string securityToken)
        {
            try
            {
                if (!_sipServiceCall.UnHold) return;
                var startTIme = DateTime.Now;
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "Not implement in CcDispatch-UnHoldCall", Logger.LogLevel.Info);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "UnHoldCall", exception, Logger.LogLevel.Error);
            }
        }
        public  void AnswerCall(string sessionId, string securityToken)
        {
            try
            {
                if (!_sipServiceCall.Answer) return;
                var startTIme = DateTime.Now;
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "Not implement in CcDispatch-AnswerCall", Logger.LogLevel.Info);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "AnswerCall", exception, Logger.LogLevel.Error);
            }
        }
        public  void TransferCall(string sessionId, string securityToken)
        {
            try
            {
                if (!_sipServiceCall.Transfer) return;
                var startTIme = DateTime.Now;
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "Not implement in CcDispatch-TransferCall", Logger.LogLevel.Info);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "TransferCall", exception, Logger.LogLevel.Error);
            }
        }
        public  void ConferenceCall(string sessionId, string securityToken)
        {
            try
            {
                if (!_sipServiceCall.Conference) return;
                var startTIme = DateTime.Now;
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "Not implement in CcDispatch-ConferenceCall", Logger.LogLevel.Info);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "ConferenceCall", exception, Logger.LogLevel.Error);
            }
        }
        public  void ETLCall(string sessionId, string securityToken)
        {
            try
            {
                if (!_sipServiceCall.ETL) return;
                var startTIme = DateTime.Now;
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "Not implement in CcDispatch-ConferenceCall-ETLCall", Logger.LogLevel.Info);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "ETLCall", exception, Logger.LogLevel.Error);
            }
        }

        public  void SwapCallCall(string sessionId, string securityToken)
        {
            try
            {
                if (!_sipServiceCall.SwapCall) return;
                var startTIme = DateTime.Now;
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1, "Not implement in CcDispatch-ConferenceCall-SwapCallCall", Logger.LogLevel.Info);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "SwapCallCall", exception, Logger.LogLevel.Error);
            }
        }
    }
}
