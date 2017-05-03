using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using Alchemy;
using Alchemy.Classes;
using DuoSoftware.DuoTools.DuoLogger;
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoSoftPhone.Controllers.Service;
using DuoSoftware.DuoSoftPhone.Ui;


namespace DuoSoftware.DuoSoftPhone.Controllers
{

    public class WebSocketServiceHost
    {
        public delegate void SocketMessageRecive(CallFunctions callFunction, string message);

        public event SocketMessageRecive OnSocketMessageRecive;

        private WebSocketServer server;
        private string DuoKey;

        public WebSocketServiceHost(string ipAddress, int port, string securityToken, int tenantId, int companyId)
        {
            try
            {
                DuoKey = CommonDataHandler.GetEventInfo(securityToken, tenantId, companyId).SocketMsgKey;
                server = new WebSocketServer(port, IPAddress.Any)
                {
                    OnConnected = OnConnected,
                    OnDisconnect = OnDisconnected,
                    OnSend = OnSend,
                    OnConnect = OnConnect,
                    OnReceive = OnRecieve,
                    TimeOut = new TimeSpan(0, 5, 0)
                };

                server.Start();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "WebSocketServiceHost", exception, Logger.LogLevel.Error);
            }

        }

        //public void Start()
        //{
        //    //server.Start();

        //}

        //public void Stop()
        //{
        //    server.Stop();
        //}

        private void OnSend(UserContext aContext)
        {

        }

        private void OnRecieve(UserContext aContext)
        {
            
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnRecieve : " + aContext.ClientAddress, Logger.LogLevel.Info);
                if (aContext.DataFrame != null)
                {
                    string message = aContext.DataFrame.ToString();
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, string.Format("message : {0} form : {1}",message,aContext.ClientAddress), Logger.LogLevel.Info);

                    var callInfo = message.Split('|');
                    if (callInfo.Length != 4)
                        throw new FieldAccessException("Invalid Call Information's.");
                    if (!DuoKey.Equals(callInfo[0]))
                        throw new ExpiredSecurityTokenException("Invalid SecurityToken or Expired.");

                    var callFunction = (CallFunctions)Enum.Parse(typeof(CallFunctions), callInfo[1]);

                    if (OnSocketMessageRecive != null)
                        OnSocketMessageRecive(callFunction, callInfo[2]);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnRecieve", exception, Logger.LogLevel.Error);
            }
        }


        private void OnConnect(UserContext aContext)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnConnect : "+aContext.ClientAddress, Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnConnect", exception, Logger.LogLevel.Error);
            }
        }

        private void OnConnected(UserContext aContext)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnConnected : " + aContext.ClientAddress, Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnConnected", exception, Logger.LogLevel.Error);
            }
        }

        private void OnDisconnected(UserContext aContext)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnDisconnected : " + aContext.ClientAddress, Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnDisconnected", exception, Logger.LogLevel.Error);
            }
        }
    }
}

