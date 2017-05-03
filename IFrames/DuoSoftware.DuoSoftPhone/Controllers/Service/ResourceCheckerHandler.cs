using DuoSoftware.DuoSoftPhone.Controllers.AgentStatus;
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoTools.DuoLogger;
using System;
using DuoSoftware.DuoSoftPhone.RefResourceChecker;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    public delegate void ResourceCheckerEvent();

    public class ResourceCheckerHandler
    {
        public event ResourceCheckerEvent OnResourceStatusMisMatch;

        public ResourceCheckerHandler(Agent agent)
        {
            try
            {
                if (agent == null)
                    return;
                _resourceMap = new System.Timers.Timer
                {
                    Interval =
                        (int)
                            TimeSpan.FromSeconds(
                                Convert.ToDouble(
                                    System.Configuration.ConfigurationSettings.AppSettings[
                                        "AgentStatusTestTimerInterval"])).TotalMilliseconds
                };

                
                
                var errCount = 0;
                _resourceMap.Elapsed += (s, e) =>
                {
                    var isNeedToMap = false;
                    try
                    {
                        new ComMethods.SwitchOnType<AgentEvent>(agent.AgentCurrentState)
                            .Case<AgentInitiate>(initiate =>
                            {
                                if (DateTime.Now.Subtract(agent.LastActivity).TotalMinutes >= 2)
                                {
                                    isNeedToMap = true;
                                }
                            })
                            .Case<AgentIdle>(i =>
                            {
                                if (DateTime.Now.Subtract(agent.LastActivity).TotalMinutes >= 1)
                                {
                                    isNeedToMap = true;
                                }
                            })
                            .Case<AgentBusy>(b =>
                            {
                                if (DateTime.Now.Subtract(agent.LastActivity).TotalMinutes >= 5)
                                {
                                    isNeedToMap = true;
                                }
                            })
                            .Case<AgentAcw>(b =>
                            {
                                if (DateTime.Now.Subtract(agent.LastActivity).TotalMinutes >= 5)
                                {
                                    isNeedToMap = true;
                                }
                            })
                            .Case<AgentBreak>(b =>
                            {
                                isNeedToMap = true;
                            })
                            .Case<AgentOffline>(b => { isNeedToMap = true; })
                            .Default<AgentEvent>(t => { isNeedToMap = true; });
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger5, "Agent ChangeUI", exception, Logger.LogLevel.Error);
                    }

                    if (!isNeedToMap) return;
                    if (!CheckAgentStatus(agent)) return;
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger5,
                        "Status Mismatch. errCount : " + errCount, Logger.LogLevel.Error);
                    errCount++;

                    if (errCount <= 3) return;
                    _resourceMap.Enabled = false;
                    _resourceMap.Stop();
                    if (OnResourceStatusMisMatch != null)
                        OnResourceStatusMisMatch();
                };
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger5, "ResourceMonitoringHandler", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        private readonly System.Timers.Timer _resourceMap;

        public void StartStatusMap()
        {
            try
            {
                if (_resourceMap == null) return;
                _resourceMap.Enabled = true;
                _resourceMap.Start();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger5, "StartStatusMap", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        private static bool CheckAgentStatus(Agent agent)
        {
            try
            {
                var uiMode = UiModes.Offline;

                switch (agent.AgentMode)
                {
                    case AgentMode.Offline:
                        break;

                    case AgentMode.Inbound:
                        uiMode = UiModes.Inbound;
                        break;

                    case AgentMode.Outbound:
                        uiMode = UiModes.Outbound;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var uiState = UiStatus.Initializing;

                new ComMethods.SwitchOnType<AgentEvent>(agent.AgentCurrentState)
                    .Case<AgentInitiate>(initiate =>
                    {
                        uiState = UiStatus.Initializing;
                    })
                    .Case<AgentIdle>(i =>
                    {
                        uiState = UiStatus.Idle;
                    })
                    .Case<AgentBusy>(b =>
                    {
                        uiState = UiStatus.Busy;
                    })
                    .Case<AgentAcw>(b =>
                    {
                        uiState = UiStatus.Acw;
                    })
                    .Case<AgentBreak>(b =>
                    {
                        uiState = UiStatus.Break;
                    });

                var startTIme = DateTime.Now;
                var client = new ResourceStatusCheckerClient("BasicHttpBinding_IResourceStatusChecker");
                var sqid = SequenceNumberGenerator.Instance.GetNextNo;
                var reply = client.CheckCurrentState(agent.Auth.SecurityToken, uiState, uiMode, sqid);
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger5, string.Format("ResourceStatusChecker. Time Take :{0}. Softphone Status : {1} Map>uiState {2}. Reply : {3}, sqid : {4}", DateTime.Now.Subtract(startTIme).TotalMilliseconds, agent.AgentCurrentState, uiState, reply, sqid), Logger.LogLevel.Info);
                return (reply.IsMatching == false) && reply.CurrentMode == UiModes.Offline && reply.CurrentState == UiStatus.Initializing;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger5, "CheckAgentStatus", exception, Logger.LogLevel.Error);
                return true;
            }
        }
    }
}