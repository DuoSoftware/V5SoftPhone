using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuoSoftware.DuoLogger;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    public class CommonDataHandler
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
            public string SocketMsgKey;


            public override string ToString()
            {
                return
                    string.Format(
                        "Dial : {0}, Hold : {1},UnHold : {2}, Answer : {3}, Reject : {4},Transfer : {5}, Conference : {6},ETL : {7}, SwapCall : {8}, AutoAnswerByDefault : {9}, AutoAnswerCanAgentEnable : {10}, ShowMsg : {11} ,  SocketMsgKey : {12} , IsPBX : {13}",
                        Dial, Hold, UnHold, Answer, Reject, Transfer, Conference, ETL, SwapCall, AutoAnswerByDefault, AutoAnswerCanAgentEnable, ShowMsg, SocketMsgKey.Length, IsPBX);
            }
        }

        public static SipServiceCall GetEventInfo(string securityToken, int TenantID, int CompanyID)
        {
            var serviceCall = new SipServiceCall { AutoAnswerByDefault = false,AutoAnswerCanAgentEnable =false, Answer = false, Conference = false, ETL = false, Dial = false, Hold = false, Reject = false, SwapCall = false, Transfer = false, UnHold = false };
            try
            {
                var section = (NameValueCollection)ConfigurationManager.GetSection("DuoBaseInfo");
                var tableName = section["ZipEventTableName"];
                var clinet = new refCommonData.CommonDataClient("BasicHttpBinding_ICommonData");
                clinet.Open();
                var data = clinet.GetData(tableName, securityToken);

                var eventInfo = data.Where(d => d["TenantID"].Equals(TenantID.ToString()) && d["CompanyID"].Equals(CompanyID.ToString()));
                
                serviceCall = new SipServiceCall
                {
                    AutoAnswerByDefault = eventInfo.Any(e => e["AutoAnswerByDefault"].ToString().ToLower().Equals("yes")),
                    AutoAnswerCanAgentEnable = eventInfo.Any(e => e["AutoAnswerCanAgentEnable"].ToString().ToLower().Equals("yes")),
                    Dial = eventInfo.Any(e => e["Dial"].ToString().ToLower().Equals("yes")),
                    Answer = eventInfo.Any(e => e["Answer"].ToString().ToLower().Equals("yes")),
                    Hold = eventInfo.Any(e => e["Hold"].ToString().ToLower().Equals("yes")),
                    UnHold = eventInfo.Any(e => e["Hold"].ToString().ToLower().Equals("yes")),
                    Reject = eventInfo.Any(e => e["Reject"].ToString().ToLower().Equals("yes")),
                    Transfer = eventInfo.Any(e => e["transferCall"].ToString().ToLower().Equals("yes")),
                    Conference = eventInfo.Any(e => e["Conference"].ToString().ToLower().Equals("yes")),
                    ETL = eventInfo.Any(e => e["ETL"].ToString().ToLower().Equals("yes")),
                    SwapCall = eventInfo.Any(e => e["SwapCall"].ToString().ToLower().Equals("yes")),
                    ShowMsg = eventInfo.Any(e => e["ShowMsg"].ToString().ToLower().Equals("yes")),
                    IsPBX = eventInfo.Any(e => e["value1"].ToString().ToLower().Equals("pbx")),
                };
                try
                {
                    object data1="";
                    var temp = eventInfo.FirstOrDefault(x => x.TryGetValue("SocketMsgKey", out data1));
                    serviceCall.SocketMsgKey = data1.ToString();
                }
                catch (Exception exception)
                {
                   Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "GetEventInfo-Fail to get SocketMsgKey", exception, Logger.LogLevel.Error);
                    serviceCall.SocketMsgKey = string.Empty;
                }
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, string.Format("SipServiceCall : {0}",serviceCall), Logger.LogLevel.Info);
                
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "GetEventInfo", exception, Logger.LogLevel.Error);
                
            }return serviceCall;
        }

       
    }
}
