using DuoSoftware.DuoTools.DuoLogger;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.ServiceModel;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    public static class ProfileManagementHandler
    {
        public static Dictionary<string, object> GetSipProfile(string securityToken, decimal guUserId)
        {
            var logger = Logger.Instance;
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Format("GetSipProfile-{0}", guUserId), Logger.LogLevel.Info);
                var client = new RefProfileManagement.ProfileManagementServiceClient();
                var sipSetting = client.GetAppSettings("SIP Settings", guUserId, securityToken);

                var userName = sipSetting["SIPUserName"].ToString();
                var password = sipSetting["SIPPassword"].ToString();
                var displayName = sipSetting["SIPDisplayName"].ToString();
                var authName = sipSetting["SIPAuthName"].ToString();
                var localPort = sipSetting["SIPLocalPort"].ToString();
                var serverPort = sipSetting["SIPDefaultPort"].ToString();
                var hostName = sipSetting["DialHostName"].ToString();
                const string licenceKey = "N0I5NUNCNDBFODY1NzJCNDU5OUFCQTVEMTM0QjI1RUNAOEI4RUUyQzVCRUYzQjBGNDdEOThEODE3NEUzNENBNzVAMTAzOUZDRjE0MDVDQTVFQjNDRjFBQ0M3QUYzQUZBRThARDM0QjBEQkUyRkFGNTNFNjVBODkyNTlCODA3NTk1MzA."; //"N0I5M0Q5QTQ0QkVCQzg1NEFENzhCMUNCNTlENzg4N0REQjVDNzFDMENBREM3NjUyMjQ5NUUyNTQyNUU1MzBFMEUxQUJGNjE1Q0JBQUUyQzU4QzQ0RkJEQThGMkU5ODM5M0U4NTQ5NDgzOTZFOTE0NUUzRTRGNzg2QTE3REY0RDcxQjUxQjRBRkVFNUE1MUIzOTk0NkE0OTM2MjE5NkM1MkFCMDY0NzQwOUQ0MzE3REUxQTc5RjY1Q0Q4MzZDNDY1MkRGNTZBOEVEQUExMkZFODg4RURENUUzNERCODg3REYyNDIyNjg1NTAwRDU1MDY0QTE3RDdGMTlGQ0ZENjNFQkQ2QkMxRTU3RDE3MUQxQUFGODZDQjUzODZGQTMxMUYzNjFDM0ZBNTYwQzRCQzBCMzkwMTUyNDZCRDIwMzc3REY2QjcyNEJFMUNFOTc0M0I1OTk5OTU5Mjk2Njk3MUE4MjUwQkFBODhFRjk2MDQ2OTQ3RjkxNkU0NDNBNEVGMTg4";// "6TfjdAS2nKvnVdM35kw7QQ==@pxae/X5M9ELMxkBvLGG8dw==@ldyQ6njXXVKTGgVpAiAOAQ==@Zc+H5mB4Mx7df/kWby4Uyw==";// "N0I5M0Q5QTQ0QkVCQzg1NDY3NDhDRDhFRTJFMkU2Q0Q0MzMwNzgxOEQ2NTI4NDM4NzAyMjNFOUJGNUE3NEY4MTdCNTVDNjRBMTgwMjg1QzMyRjAxRjUzMEYxQzM5MDlGOTRFRTk0RDI3RjdEOEM4RjhBNTcwNkU2Mjk3ODY2OThGRkFDQjIxRTNEM0NFQjE3OUE4RDI0OUY4RUY2OUEyRDg0NDA3MkFCQjVFMTBCRUZBQTBCMUM0NjExMzBFOTc0RkRGNUFFRTA3MjdDMzdFMEYxRkYzRjc4RTA4Nzk5NUQ4OUM5OTQ1QzREMjI1RTc4N0Y2NDAzMDVGNDkzQUNBQzE1RDkyMDZCMUQ3REZCNjY4MTAwQTM2OEMyMkJDRDZFN0MwOTEwMzEwNEVGQzM4NUJBMDYwMjJCRjhEN0IzOTg3MjI1NDg1RkQ3MUVDNzhCRTQyMkRCRTFEREYwMTk0QTQyNjJDRjc5QTIwMzE1RjQ5NUE2QTYxODkyRjRGNkRB";
                //try
                //{
                //    var key = sipSetting["SIPDefaultHost"].ToString();
                //    if (!string.IsNullOrEmpty(key)&& key.Length >= 50)
                //        licenceKey = key;
                //}
                //catch (Exception exception)
                //{
                //    logger.LogMessage(Logger.LogAppender.DuoDefault, "licenceKey", exception,Logger.LogLevel.Error);
                //}

                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Sip Info.... userName : {0}, password : {1}, displayName : {2}, authName : {3}, localPort : {4}, serverPort : {5}, hostName : {6}", userName, "password", displayName, authName, localPort, serverPort, hostName), Logger.LogLevel.Debug);
                int lPort,sPort;
                int.TryParse(localPort,out lPort);
                int.TryParse(serverPort, out sPort);
                var initDictionary = new Dictionary<string, object>
                {
                    {"userName", userName},
                    {"password", password},
                    {"displayName", displayName},
                    {"authName", authName},
                    {"localPort", lPort},
                    {"licenceKey",licenceKey},
                    {"serverPort", sPort},
                    {"hostName", hostName}
                };

                //XMLHandler.WriteXML(localPort,hostName,serverPort,userName,password,displayName,authName);
                return initDictionary;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "GetSipProfile[---ProfileManagementService---]", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        public static int GetAcwTime(string securityToken)
        {
            var logger = Logger.Instance;
            try
            {
                var profileClient = new RefProfileManagement.ProfileManagementServiceClient();

                var result = profileClient.GetApplicationSettings(14123110382978506, securityToken);

                if (result != null)
                {
                    if (result.Count != 0)
                    {
                        if (result["ACWTime"] != null)
                        {
                            int refreshTime;
                            if (int.TryParse(result["ACWTime"].ToString(), out refreshTime))
                            {
                                return refreshTime;
                            }
                        }
                    }
                }
            }
            catch (EndpointNotFoundException exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "GetACWTime", exception, Logger.LogLevel.Error);
            }
            catch (InvalidCastException exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "GetACWTime", exception, Logger.LogLevel.Error);
            }

            return 12;
        }
    }
}