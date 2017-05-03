using DuoSoftware.DuoTools.DuoLogger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    public static class CommonDataHandler
    {
        
        
        public struct SipServiceCall
        {
            public bool AutoAnswerByDefault;
            public bool AutoAnswerCanAgentEnable;
            public bool Dial;
            public bool Hold;
            public bool UnHold;
            public bool Answer;
            public bool Reject;
            public bool Transfer;
            public bool Conference;
            public bool ETL;
            public bool SwapCall;
            public bool ShowMsg;
            public bool IsPBX;
            public bool IsPlayRingtone;
            public bool IsAllowToReject;
            public string SocketMsgKey;
            public string BaseTabName;

            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }
                var c = (SipServiceCall)obj;
                return ((AutoAnswerByDefault == c.AutoAnswerByDefault) && (AutoAnswerCanAgentEnable == c.AutoAnswerCanAgentEnable) && (Dial == c.Dial)&& (Hold == c.Hold) && (UnHold == c.UnHold) && (Answer == c.Answer) && (Reject == c.Reject) && (Transfer == c.Transfer)&& (Conference == c.Conference) && (ETL == c.ETL) && (SwapCall == c.SwapCall) && (ShowMsg == c.ShowMsg) && (IsPBX == c.IsPBX)&& (IsPlayRingtone == c.IsPlayRingtone) && (IsAllowToReject == c.IsAllowToReject) && (SocketMsgKey == c.SocketMsgKey) && (BaseTabName == c.BaseTabName));
            }
            public override int GetHashCode()
            {
                return 0;
            }
            public static bool operator ==(SipServiceCall left, SipServiceCall right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(SipServiceCall left, SipServiceCall right)
            {
                return !(left == right);
            }

            public override string ToString()
            {
                return
                    string.Format(
                        "Dial : {0}, Hold : {1},UnHold : {2}, Answer : {3}, Reject : {4},Transfer : {5}, Conference : {6},ETL : {7}, SwapCall : {8}, AutoAnswerByDefault : {9}, AutoAnswerCanAgentEnable : {10}, ShowMsg : {11} ,  SocketMsgKey : {12} , IsPBX : {13}, IsPlayRingtone : {14}, IsAllowToReject : {15}, BaseTabName : {16}",
                        Dial, Hold, UnHold, Answer, Reject, Transfer, Conference, ETL, SwapCall, AutoAnswerByDefault, AutoAnswerCanAgentEnable, ShowMsg, SocketMsgKey.Length, IsPBX, IsPlayRingtone, IsAllowToReject, BaseTabName);
            }
        }

        public static SipServiceCall GetEventInfo(string securityToken, int tenantId, int companyId)
        {
            var logger = Logger.Instance;
            var serviceCall = new SipServiceCall { AutoAnswerByDefault = false, AutoAnswerCanAgentEnable = false, Answer = false, Conference = false, ETL = false, Dial = false, Hold = false, Reject = false, SwapCall = false, Transfer = false, UnHold = false, BaseTabName = "duo" };
            try
            {
                var section = (NameValueCollection)ConfigurationManager.GetSection("DuoBaseInfo");
                var tableName = section["ZipEventTableName"];
                var clinet = new RefCommonData.CommonDataClient("BasicHttpBinding_ICommonData");
                clinet.Open();
                var data = clinet.GetData(tableName, securityToken);

                var eventInfo = data.Where(d => d["TenantID"].Equals(tenantId.ToString()) && d["CompanyID"].Equals(companyId.ToString()));

                serviceCall = new SipServiceCall
                {
                    AutoAnswerByDefault = eventInfo.Any(e => e["AutoAnswerByDefault"].ToString().ToLower().Equals("yes") || e["AutoAnswerByDefault"].ToString().ToLower().Equals("1")),
                    AutoAnswerCanAgentEnable = eventInfo.Any(e => e["AutoAnswerCanAgentEnable"].ToString().ToLower().Equals("yes") || e["AutoAnswerCanAgentEnable"].ToString().ToLower().Equals("1")),
                    Dial = eventInfo.Any(e => e["Dial"].ToString().ToLower().Equals("yes") || e["Dial"].ToString().ToLower().Equals("1")),
                    Answer = eventInfo.Any(e => e["Answer"].ToString().ToLower().Equals("yes") || e["Answer"].ToString().ToLower().Equals("1")),
                    Hold = eventInfo.Any(e => e["Hold"].ToString().ToLower().Equals("yes") || e["Hold"].ToString().ToLower().Equals("1")),
                    UnHold = eventInfo.Any(e => e["Hold"].ToString().ToLower().Equals("yes") || e["Hold"].ToString().ToLower().Equals("1")),
                    Reject = eventInfo.Any(e => e["Reject"].ToString().ToLower().Equals("yes") || e["Reject"].ToString().ToLower().Equals("1")),
                    Transfer = eventInfo.Any(e => e["transferCall"].ToString().ToLower().Equals("yes") || e["transferCall"].ToString().ToLower().Equals("1")),
                    Conference = eventInfo.Any(e => e["Conference"].ToString().ToLower().Equals("yes") || e["Conference"].ToString().ToLower().Equals("1")),
                    ETL = eventInfo.Any(e => e["ETL"].ToString().ToLower().Equals("yes") || e["ETL"].ToString().ToLower().Equals("1")),
                    SwapCall = eventInfo.Any(e => e["SwapCall"].ToString().ToLower().Equals("yes") || e["SwapCall"].ToString().ToLower().Equals("1")),
                    ShowMsg = eventInfo.Any(e => e["ShowMsg"].ToString().ToLower().Equals("yes") || e["ShowMsg"].ToString().ToLower().Equals("1")),
                    IsPBX = eventInfo.Any(e => e["value1"].ToString().ToLower().Equals("pbx") || e["value1"].ToString().ToLower().Equals("1")),
                };
                object data1 = string.Empty;
                var temp = eventInfo.FirstOrDefault(x => x.TryGetValue("SocketMsgKey", out data1));
                if (data1 != null) serviceCall.SocketMsgKey = data1.ToString();

                serviceCall.IsPlayRingtone = !
                    eventInfo.Any(
                        e =>
                            e["value2"].ToString().ToLower().Equals("yes") ||
                            e["value2"].ToString().ToLower().Equals("1"));

                serviceCall.IsAllowToReject = eventInfo.Any(
                        e =>
                            e["value3"].ToString().ToLower().Equals("yes") ||
                            e["value3"].ToString().ToLower().Equals("1"));
                object tabName = "duo";
                eventInfo.FirstOrDefault(x => x.TryGetValue("BaseTabName", out tabName));
                serviceCall.BaseTabName = (tabName != null) ? tabName.ToString() : "duo";
                logger.LogMessage(Logger.LogAppender.DuoDefault, string.Format("SipServiceCall : {0}", serviceCall), Logger.LogLevel.Info);
            }
            catch (IndexOutOfRangeException exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoDefault, "GetEventInfo", exception, Logger.LogLevel.Error);
            } return serviceCall;
        }

        public static Collection<string> GetUrls(string securityToken, int tenantId, int companyId)
        {
            var urls = new Collection<string>();
            try
            {
                var section = (NameValueCollection)ConfigurationManager.GetSection("DuoBaseInfo");
                var tableName = section["CrmUrlTableName"];
                var clinet = new RefCommonData.CommonDataClient("BasicHttpBinding_ICommonData");
                clinet.Open();
                var data = clinet.GetData(tableName, securityToken);

                var eventInfo = data.Where(d => d["TenantID"].Equals(tenantId.ToString()) && d["CompanyID"].Equals(companyId.ToString()));

                eventInfo.AsParallel().ForAll(u =>
                {
                    urls.Add(u["Url"].ToString());
                });
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "GetUrls", exception, Logger.LogLevel.Error);
                throw;
            }
            return urls;
        }
    }
}