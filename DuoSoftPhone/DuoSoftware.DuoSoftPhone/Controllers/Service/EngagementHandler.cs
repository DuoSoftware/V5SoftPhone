using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuoSoftware.DuoLogger;

namespace DuoSoftware.DuoSoftPhone.Controllers
{
    public class EngagementHandler
    {
        public static string GetPhoneNo(string securitytoken, string sessionId)
        {
            try
            {
                var id = sessionId.Split('@');
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, string.Format("GetPhoneNo sessionId : {0}", sessionId), Logger.LogLevel.Info);
                var client = new refEngagement.DuoEngagementServiceClient("basicEngagementEndpoint");
                var engagements = client.ViewEngagementOriginateBySessionID(securitytoken, id[0]);

                if (engagements != null)
                {
                    string[] parts = System.Text.RegularExpressions.Regex.Split(engagements.OtherData, "-");
                    Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, string.Format("GetPhoneNo - {0} . sessionId : {1}", engagements.OtherData, sessionId), Logger.LogLevel.Info);
                    return parts[0];
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Login fail", exception, Logger.LogLevel.Error);
            }
            return string.Empty;
        }
    }
}
