using System;
using System.Collections.Generic;
using DuoSoftware.DuoLogger;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    public struct ProfileInfo
    {
        public string userName;
        public string password;
        public string displayName;
        public string authName;
        public int localPort;
        public int serverPort;
        public string hostName;
    }

    public class ProfileManagementHandler
    {
        public static ProfileInfo GetSipProfile(string securityToken, decimal guUserId)
        {

            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault,string.Format("GetSipProfile-{0}",guUserId), Logger.LogLevel.Info);
                var client = new refProfileManagement.ProfileManagementServiceClient();
                var sipSetting = client.GetAppSettings("SIP Settings", guUserId, securityToken);
                var proInfo = new ProfileInfo
                {
                    userName = sipSetting["SIPUserName"].ToString(),
                    password = sipSetting["SIPPassword"].ToString(),
                    displayName = sipSetting["SIPDisplayName"].ToString(),
                    authName = sipSetting["SIPAuthName"].ToString(),
                    localPort = Convert.ToInt32(sipSetting["SIPLocalPort"].ToString()),
                    serverPort = Convert.ToInt32(sipSetting["SIPDefaultPort"].ToString()),
                    hostName = sipSetting["DialHostName"].ToString(),
                };

                return proInfo;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "GetSipProfile", exception, Logger.LogLevel.Error);
                return new ProfileInfo();
            }
        }
    }
}
