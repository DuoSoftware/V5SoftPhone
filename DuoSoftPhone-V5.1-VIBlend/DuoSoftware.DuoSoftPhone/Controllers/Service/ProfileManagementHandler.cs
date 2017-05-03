using System;
using System.Collections.Generic;
using DuoSoftware.DuoTools.DuoLogger;


namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    public class ProfileManagementHandler
    {
        public static Dictionary<string, object> GetSipProfile(string securityToken, decimal guUserId)
        {

            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("GetSipProfile-{0}", guUserId), Logger.LogLevel.Info);
                var client = new refProfileManagement.ProfileManagementServiceClient();
                var sipSetting = client.GetAppSettings("SIP Settings", guUserId, securityToken);

                string userName = sipSetting["SIPUserName"].ToString();
                string password = sipSetting["SIPPassword"].ToString();
                string displayName = sipSetting["SIPDisplayName"].ToString();
                string authName = sipSetting["SIPAuthName"].ToString();
                string localPort = sipSetting["SIPLocalPort"].ToString();
                string serverPort = sipSetting["SIPDefaultPort"].ToString();
                string hostName = sipSetting["DialHostName"].ToString();
                string licenceKey = "N0I5NUNCNDBFODY1NzJCNDU5OUFCQTVEMTM0QjI1RUNAOEI4RUUyQzVCRUYzQjBGNDdEOThEODE3NEUzNENBNzVAMTAzOUZDRjE0MDVDQTVFQjNDRjFBQ0M3QUYzQUZBRThARDM0QjBEQkUyRkFGNTNFNjVBODkyNTlCODA3NTk1MzA.";//"N0I5M0Q5QTQ0QkVCQzg1NEFENzhCMUNCNTlENzg4N0REQjVDNzFDMENBREM3NjUyMjQ5NUUyNTQyNUU1MzBFMEUxQUJGNjE1Q0JBQUUyQzU4QzQ0RkJEQThGMkU5ODM5M0U4NTQ5NDgzOTZFOTE0NUUzRTRGNzg2QTE3REY0RDcxQjUxQjRBRkVFNUE1MUIzOTk0NkE0OTM2MjE5NkM1MkFCMDY0NzQwOUQ0MzE3REUxQTc5RjY1Q0Q4MzZDNDY1MkRGNTZBOEVEQUExMkZFODg4RURENUUzNERCODg3REYyNDIyNjg1NTAwRDU1MDY0QTE3RDdGMTlGQ0ZENjNFQkQ2QkMxRTU3RDE3MUQxQUFGODZDQjUzODZGQTMxMUYzNjFDM0ZBNTYwQzRCQzBCMzkwMTUyNDZCRDIwMzc3REY2QjcyNEJFMUNFOTc0M0I1OTk5OTU5Mjk2Njk3MUE4MjUwQkFBODhFRjk2MDQ2OTQ3RjkxNkU0NDNBNEVGMTg4";// "6TfjdAS2nKvnVdM35kw7QQ==@pxae/X5M9ELMxkBvLGG8dw==@ldyQ6njXXVKTGgVpAiAOAQ==@Zc+H5mB4Mx7df/kWby4Uyw==";// "N0I5M0Q5QTQ0QkVCQzg1NDY3NDhDRDhFRTJFMkU2Q0Q0MzMwNzgxOEQ2NTI4NDM4NzAyMjNFOUJGNUE3NEY4MTdCNTVDNjRBMTgwMjg1QzMyRjAxRjUzMEYxQzM5MDlGOTRFRTk0RDI3RjdEOEM4RjhBNTcwNkU2Mjk3ODY2OThGRkFDQjIxRTNEM0NFQjE3OUE4RDI0OUY4RUY2OUEyRDg0NDA3MkFCQjVFMTBCRUZBQTBCMUM0NjExMzBFOTc0RkRGNUFFRTA3MjdDMzdFMEYxRkYzRjc4RTA4Nzk5NUQ4OUM5OTQ1QzREMjI1RTc4N0Y2NDAzMDVGNDkzQUNBQzE1RDkyMDZCMUQ3REZCNjY4MTAwQTM2OEMyMkJDRDZFN0MwOTEwMzEwNEVGQzM4NUJBMDYwMjJCRjhEN0IzOTg3MjI1NDg1RkQ3MUVDNzhCRTQyMkRCRTFEREYwMTk0QTQyNjJDRjc5QTIwMzE1RjQ5NUE2QTYxODkyRjRGNkRB";
                //try
                //{
                //    var key = sipSetting["SIPDefaultHost"].ToString();
                //    if (!string.IsNullOrEmpty(key)&& key.Length >= 50)
                //        licenceKey = key;
                //}
                //catch (Exception exception)
                //{
                //    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "licenceKey", exception,Logger.LogLevel.Error);
                //}

                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Sip Info.... userName : {0}, password : {1}, displayName : {2}, authName : {3}, localPort : {4}, serverPort : {5}, hostName : {6}", userName, "password", displayName, authName, localPort, serverPort, hostName), Logger.LogLevel.Debug);

                var initDictionary = new Dictionary<string, object>
                {
                    {"userName", userName},
                    {"password", password},
                    {"displayName", displayName},
                    {"authName", authName},
                    {"localPort", Int32.Parse(localPort)},
                    {"licenceKey",licenceKey},
                    {"serverPort", Int32.Parse(serverPort)},
                    {"hostName", hostName}
                };

                //XMLHandler.WriteXML(localPort,hostName,serverPort,userName,password,displayName,authName);
                return initDictionary;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "GetSipProfile[---ProfileManagementService---]", exception, Logger.LogLevel.Error);
                return null;
            }
        }

        public static int GetACWTime(string securityToken)
        {
            try
            {
                var profileClient = new refProfileManagement.ProfileManagementServiceClient();


                var Result = profileClient.GetApplicationSettings(14123110382978506, securityToken);

                if (Result != null)
                {
                    if (Result.Count != 0)
                    {
                        if (Result["ACWTime"] != null)
                        {
                            int refreshTime = 0;
                            if (int.TryParse(Result["ACWTime"].ToString(), out refreshTime))
                            {
                                return int.Parse(Result["ACWTime"].ToString());
                            }
                        }
                    }
                }

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "GetACWTime", exception, Logger.LogLevel.Error);

            }
            return 10;
        }
    }
}
