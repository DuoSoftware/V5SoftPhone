using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuoSoftware.DuoTools.DuoLogger;
using System.Net;
using DuoSoftware.DuoSoftPhone.refEngagement;

namespace DuoSoftware.DuoSoftPhone.Controllers
{
    public class EngagementHandler
    {
        private string print<TKey, TValue>(Dictionary<TKey, TValue> source, string keyValueSeparator = "=", string sequenceSeparator = "|")
        {
            try
            {
                return source.Aggregate(string.Empty, (acc, pair) => string.Format("{0}{1}{2}{3}{4}", acc, pair.Key, keyValueSeparator, pair.Value, sequenceSeparator));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ToString", exception, Logger.LogLevel.Error);
                return "";
            }
        }

        public static string GetPhoneNo(string securitytoken, string sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("GetPhoneNo sessionId : {0}",  sessionId), Logger.LogLevel.Info);
                var startTIme = DateTime.Now;
                var client = new refEngagement.DuoEngagementServiceClient("basicEngagementEndpoint");
                var engagements = client.ViewEngagementOriginateBySessionID(securitytoken, sessionId);

                if (engagements != null)
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("GetPhoneNo - {0} . sessionId : {1} . Time Take : {2} . Engagement : {3}", engagements.OtherData, sessionId, DateTime.Now.Subtract(startTIme).TotalMilliseconds, engagements), Logger.LogLevel.Info);
                    string[] parts = System.Text.RegularExpressions.Regex.Split(engagements.OtherData, "-");
                    
                    return parts[0];
                }
                else
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("GetPhoneNo - {0} . sessionId : {1} . Time Take : {2}", "Receive Null Data", sessionId, DateTime.Now.Subtract(startTIme).TotalMilliseconds), Logger.LogLevel.Info);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "GetPhoneNo", exception, Logger.LogLevel.Error);
            }
            return string.Empty;
        }
        
        public static string ViewCurrentEngagementDisplayInfo(string securitytoken, string sessionId)
        {
            try
            {
                //var id = sessionId.Split('@');
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("ViewCurrentEngagementDisplayInfo sessionId : {0}", sessionId), Logger.LogLevel.Info);
                var client = new refEngagement.DuoEngagementServiceClient("basicEngagementEndpoint");
                var engagements = client.ViewCurrentEngagementDisplayInfo(securitytoken, sessionId, "callserver", "ivr", "language");
                
                if (engagements != null)
                {
                    var lang = engagements.FirstOrDefault(e => e.Key.Equals("language"));
                    var phone = engagements.FirstOrDefault(e => e.Key.Equals("phoneNumber"));
                    var ivr = engagements.FirstOrDefault(e => e.Key.Equals("ivrPath"));
                    return string.Format("language : {0} , Phone No : {1} , IVR : {2} ",  lang.Value, phone.Value,ivr.Value );
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ViewCurrentEngagementDisplayInfo", exception, Logger.LogLevel.Error);
            }
            return string.Empty;
        }
    }

}
