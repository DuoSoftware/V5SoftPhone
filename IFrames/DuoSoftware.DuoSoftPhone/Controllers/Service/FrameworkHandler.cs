using DuoSoftware.DuoTools.DuoLogger;
using System;
using System.Collections.Generic;
using DuoSoftware.DuoSoftPhone.RefFramework;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    public static class FrameworkHandler
    {
        public static void LogOff(string securityToken, string sessionId)
        {
            try
            {
                var frameworkClient = new UiAccessServiceClient();
                //Disconnect client
                var dicData = new Dictionary<string, object> { { "SecurityToken", securityToken }, { "SessionID", sessionId } };
                frameworkClient.InvokeCommandBySecurityToken(securityToken, "LOGOFF", dicData);

                var dicData2 = new Dictionary<string, object>();
                frameworkClient.InvokeCommandBySecurityToken(securityToken, "CLOSE-CONTAINER", dicData2);

                /*
                 * var frameworkClient = new FrameworkServerClient(new InstanceContext(new FrameworkHandlerCallBack()));
                 //Disconnect client
                 var dicData = new Dictionary<string, object> { { "SecurityToken", securityToken }, { "SessionID", sessionId } };
                 frameworkClient.SendCommand("LOGOFF", dicData);

                 var dicData2 = new Dictionary<string, object>();
                 frameworkClient.SendCommand("CLOSE-CONTAINER", dicData2);
                 * */
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "FrameworkHandler-LogOff", exception, Logger.LogLevel.Error);
                throw;
            }
        }
    }

    /*
    public class FrameworkHandlerCallBack:refFramework.IFrameworkServerCallback
    {
        public void RecieveCommand(string command, Dictionary<string, object> commandData)
        {
           Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "FrameworkHandler-RecieveCommand", Logger.LogLevel.Info   );
        }

        public void ConnectCallback()
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "FrameworkHandler-ConnectCallback", Logger.LogLevel.Info);
        }

        public void ClientPing()
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "FrameworkHandler-ClientPing", Logger.LogLevel.Info);
        }
    }
     * */
}