using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DuoSoftware.DuoSoftPhone.RefEngagement;
using DuoSoftware.DuoTools.DuoLogger;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    public static class EngagementHandler
    {
        /*
        private string print<TKey, TValue>(Dictionary<TKey, TValue> source, string keyValueSeparator = "=", string sequenceSeparator = "|")
        {
            try
            {
                return source.Aggregate(string.Empty, (acc, pair) => string.Format("{0}{1}{2}{3}{4}", acc, pair.Key, keyValueSeparator, pair.Value, sequenceSeparator));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ToString", exception, Logger.LogLevel.Error);
                return string.Empty;
            }
        }
        */

        /// <summary>
        /// get incoming phone no
        /// </summary>
        /// <param name="securitytoken"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static string GetPhoneNo(string securitytoken, string sessionId)
        {
            var logger = Logger.Instance;
            try
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Format("GetPhoneNo sessionId : {0}", sessionId), Logger.LogLevel.Info);
                var startTIme = DateTime.Now;
                DuoEngagements engagements;
                using (var client = new DuoEngagementServiceClient("basicEngagementEndpoint"))
                {
                    engagements = client.ViewEngagementOriginateBySessionID(securitytoken, sessionId);
                }

                if (engagements != null)
                {
                    logger.LogMessage(Logger.LogAppender.DuoDefault, string.Format("GetPhoneNo - [{0}] . sessionId : {1} . Time Take : {2} . Engagement : {3}", engagements.OtherData, sessionId, DateTime.Now.Subtract(startTIme).TotalMilliseconds, engagements), Logger.LogLevel.Info);
                    string[] parts = Regex.Split(engagements.OtherData, "-");
                    return parts[0];
                }
                else
                {
                    logger.LogMessage(Logger.LogAppender.DuoDefault, string.Format("GetPhoneNo - {0} . sessionId : {1} . Time Take : {2}", "Receive Null Data", sessionId, DateTime.Now.Subtract(startTIme).TotalMilliseconds), Logger.LogLevel.Info);
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "GetPhoneNo", exception, Logger.LogLevel.Error);
            }
            return string.Empty;
        }

        /// <summary>
        /// view incoming call details. ivr path,no, etc..
        /// </summary>
        /// <param name="securitytoken"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ViewCurrentEngagementDisplayInfo(string securitytoken, string sessionId)
        {
            var logger = Logger.Instance;
            try
            {
                //var id = sessionId.Split('@');
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Format("ViewCurrentEngagementDisplayInfo sessionId : {0}", sessionId), Logger.LogLevel.Info);
                Dictionary<string, string> engagements;
                using (var client = new DuoEngagementServiceClient("basicEngagementEndpoint"))
                {
                    engagements = client.ViewCurrentEngagementDisplayInfo(securitytoken, sessionId, "callserver", "ivr", "language");
                }

                return engagements;
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "ViewCurrentEngagementDisplayInfo", exception, Logger.LogLevel.Error);
                throw;
            }
        }
    }
}