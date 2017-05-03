using Alchemy;
using Alchemy.Classes;
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoSoftPhone.Controllers.Service;
using DuoSoftware.DuoTools.DuoLogger;
using System;
using System.Net;
using System.ServiceModel.Security;


namespace DuoSoftware.DuoSoftPhone.Controllers
{
    //delegate 
    public delegate void SocketMessage(CallFunction callFunction, string message);
    /// <summary>
    /// Socket Listener 
    /// </summary>
    public class WebSocketServiceHost
    {
        

        //event
        public event SocketMessage OnRecive;

        // key
        private readonly string _duoKey;

        /// <summary>
        /// -- WebSocketServiceHost
        /// </summary>
        /// <param name="port"></param>
        /// <param name="securityToken"></param>
        /// <param name="tenantId"></param>
        /// <param name="companyId"></param>
        public WebSocketServiceHost(int port, string securityToken, int tenantId, int companyId)
        {
            try
            {
                _duoKey = CommonDataHandler.GetEventInfo(securityToken, tenantId, companyId).SocketMsgKey;
                using (var server = new WebSocketServer(port, IPAddress.Any)
                {
                    OnConnected = OnConnected,
                    OnDisconnect = OnDisconnected,
                    OnSend = OnSend,
                    OnConnect = OnConnect,
                    OnReceive = OnRecieve,
                    TimeOut = new TimeSpan(0, 5, 0)
                })
                {
                    server.Start();
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "WebSocketServiceHost", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        /// <summary>
        /// On Send
        /// </summary>
        /// <param name="aContext"></param>
        private static void OnSend(UserContext aContext)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnSend : " + aContext.ClientAddress, Logger.LogLevel.Info);
        }

        /// <summary>
        /// On Receive 
        /// </summary>
        /// <param name="aContext"></param>
        private void OnRecieve(UserContext aContext)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnRecieve : " + aContext.ClientAddress, Logger.LogLevel.Info);
                if (aContext.DataFrame == null) return;
                var message = aContext.DataFrame.ToString();
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, string.Format("message : {0} form : {1}", message, aContext.ClientAddress), Logger.LogLevel.Info);

                var callInfo = message.Split('|');
                if (callInfo.Length != 4)
                    throw new FieldAccessException("Invalid Call Information's.");
                if (!_duoKey.Equals(callInfo[0]))
                    throw new ExpiredSecurityTokenException("Invalid SecurityToken or Expired.");

                var callFunction = (CallFunction)Enum.Parse(typeof(CallFunction), callInfo[1]);

                if (OnRecive != null)
                    OnRecive(callFunction, callInfo[2]);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnRecieve", exception, Logger.LogLevel.Error);
            }
        }

        /// <summary>
        /// On Connect
        /// </summary>
        /// <param name="aContext"></param>
        private static void OnConnect(UserContext aContext)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnConnect : " + aContext.ClientAddress, Logger.LogLevel.Info);
            //try
            //{
            //    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnConnect : " + aContext.ClientAddress, Logger.LogLevel.Info);
            //}
            //catch (Exception exception)
            //{
            //    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnConnect", exception, Logger.LogLevel.Error);
            //}
        }

        /// <summary>
        /// On Connected 
        /// </summary>
        /// <param name="aContext"></param>
        private static void OnConnected(UserContext aContext)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnConnected : " + aContext.ClientAddress, Logger.LogLevel.Info);
            //try
            //{
            //    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnConnected : " + aContext.ClientAddress, Logger.LogLevel.Info);
            //}
            //catch (Exception exception)
            //{
            //    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnConnected", exception, Logger.LogLevel.Error);
            //}
        }

        /// <summary>
        /// on disconnect
        /// </summary>
        /// <param name="aContext"></param>
        private static void OnDisconnected(UserContext aContext)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnDisconnected : " + aContext.ClientAddress, Logger.LogLevel.Info);
            //try
            //{

            //}
            //catch (Exception exception)
            //{
            //    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnDisconnected", exception, Logger.LogLevel.Error);
            //}
        }
    }
}