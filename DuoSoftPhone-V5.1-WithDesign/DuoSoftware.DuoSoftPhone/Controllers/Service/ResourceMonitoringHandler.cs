using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuoSoftware.DuoLogger;
using DuoSoftware.DuoSoftPhone.refResourceMonitoring;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    public struct OnlineIdleAgent
    {
        public string AgentCode { get; set; }
        public string Extention { get; set; }
    }

    public delegate void ResourceMonitoringEvent(List<OnlineIdleAgent> agentList);
    public class ResourceMonitoringHandler
    {
        public ResourceMonitoringHandler(string securityToken, string agentCode)
        {
            try
            {
                var resourceMap = new System.Timers.Timer
                {
                    Interval =
                        (int)
                            TimeSpan.FromSeconds(
                                Convert.ToDouble(
                                    System.Configuration.ConfigurationSettings.AppSettings[
                                        "AgentListUpdateTimerInterval"])).TotalMilliseconds
                };
                resourceMap.Elapsed += (s, e) => GetOnlineIdleAgentList(securityToken, agentCode);
                resourceMap.Enabled = true;
                resourceMap.Start();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger5, "ResourceMonitoringHandler", exception, Logger.LogLevel.Error);
            }
        }

        public event ResourceMonitoringEvent OnGetOnlineIdleAgentListCompleted;
        public void GetOnlineIdleAgentList(string securityToken, string agentCode)
        {
            try
            {

                var agentList1 = new List<OnlineIdleAgent>()
                {
                    new OnlineIdleAgent {AgentCode = "aaaa", Extention = "1111"},
                    new OnlineIdleAgent() {AgentCode = "bbbbb", Extention = "2222"}
                };

                if (OnGetOnlineIdleAgentListCompleted != null)
                    OnGetOnlineIdleAgentListCompleted(agentList1);
                return;
                var client = new refResourceMonitoring.ResourceMonitoringClient();
                client.GetOnlineIdleAgentListCompleted += (s, e) =>
                {
                    try
                    {
                        var agentList =
                                       e.Result.Where(a => !agentCode.Equals(a.AgentCode))
                                           .Select(a => new OnlineIdleAgent { AgentCode = a.AgentFullName, Extention = a.Extention }).ToList();
                        if (OnGetOnlineIdleAgentListCompleted != null)
                            OnGetOnlineIdleAgentListCompleted(agentList);
                        // var agentList = (from a in e.Result where !agentCode.Equals(a.AgentCode) select new OnlineIdleAgent {AgentCode = a.AgentCode, Extention = a.Extention}).ToList();
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger5, "GetOnlineIdleAgentListCompleted", exception, Logger.LogLevel.Error);
                    }
                };
                client.GetOnlineIdleAgentListAsync(securityToken);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger5, "GetOnlineIdleAgentList", exception, Logger.LogLevel.Error);
            }
        }


    }
}
