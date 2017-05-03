using DuoSoftware.DuoTools.DuoLogger;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    public struct OnlineIdleAgent
    {
        public string AgentCode { private get; set; }

        public string Extention { private get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var c = (OnlineIdleAgent)obj;
            return ((AgentCode == c.AgentCode) && (Extention == c.Extention));
        }
        public override int GetHashCode()
        {
            return 0;
        }
        public static bool operator ==(OnlineIdleAgent left, OnlineIdleAgent right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(OnlineIdleAgent left, OnlineIdleAgent right)
        {
            return !left.Equals(right);
        }
    }

    public delegate void ResourceMonitoringEvent(List<OnlineIdleAgent> agentList);

    public class ResourceMonitoringHandler
    {
        private readonly System.Timers.Timer _resourceMap;

        public ResourceMonitoringHandler(string securityToken, string agentCode)
        {
            var logger = Logger.Instance;
            try
            {
                _resourceMap = new System.Timers.Timer
                    {
                        Interval =(int)
                                TimeSpan.FromSeconds(Convert.ToDouble(System.Configuration.ConfigurationSettings.AppSettings["AgentListUpdateTimerInterval"])).TotalSeconds
                    };
                _resourceMap.Elapsed += (s, e) => GetOnlineIdleAgentList(securityToken, agentCode);
                _resourceMap.Enabled = true;
                _resourceMap.Start();
                
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger5, "ResourceMonitoringHandler-", exception, Logger.LogLevel.Error);
                _resourceMap.Stop();
                _resourceMap.Enabled = false;
            }
        }

        public event ResourceMonitoringEvent OnGetOnlineIdleAgentListCompleted;

        public void GetOnlineIdleAgentList(string securityToken, string agentCode)
        {
            using (var client = new RefResourceMonitoring.ResourceMonitoringClient())
            {
                client.GetOnlineIdleAgentListCompleted += (s, e) =>
                {
                    try
                    {
                        var agentList =
                                           e.Result.Where(a => !agentCode.Equals(a.AgentCode))
                                               .Select(
                                                   a => new OnlineIdleAgent { AgentCode = a.AgentFullName, Extention = a.Extention })
                                               .ToList();
                        if (OnGetOnlineIdleAgentListCompleted != null)
                            OnGetOnlineIdleAgentListCompleted(agentList);
                    }
                    catch (Exception exception)
                    {
                        _resourceMap.Stop();
                        _resourceMap.Enabled = false;
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger5, "GetOnlineIdleAgentList", exception, Logger.LogLevel.Error);
                    }
                };
                client.GetOnlineIdleAgentListAsync(securityToken);
            }
        }
    }
}