using System;
using DuoSoftware.DuoLogger;
using DuoSoftware.DuoSoftPhone.refResourceChecker;


namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    public class ResourceCheckerHandler
    {
        public static bool CheckAgentStatus(string securityToken,DialerState state)
        {
            try
            {
                var uiState = UiStatus.Initializing;
                switch (state)
                {
                    case DialerState.Initiate:
                        uiState = UiStatus.Initializing;
                        break;
                    case DialerState.NotOnCall:
                        uiState = UiStatus.Idle;
                        break;
                    case DialerState.CallRouted:
                    case DialerState.CallIncoming:
                    case DialerState.CallOutgoing:
                    case DialerState.OnHold:
                    case DialerState.AgentSupervisorTalking:
                    case DialerState.AgentClientTalking:
                    case DialerState.Conference:
                    case DialerState.etlCall:
                    case DialerState.swapCall:
                    case DialerState.transferCall:
                    case DialerState.transferIvr:
                    case DialerState.ACW:
                        uiState = UiStatus.Busy;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("state");
                }
                var startTIme = DateTime.Now;
                var client = new ResourceStatusCheckerClient("BasicHttpBinding_IResourceStatusChecker");
                var reply = client.CheckCurrentState(securityToken, uiState, UiModes.Inbound);
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger1,string.Format("ResourceStatusChecker. Time Take :{0}. Softphone Status : {1} Map>uiState {2}. Reply : {3}",DateTime.Now.Subtract(startTIme).TotalMilliseconds, state, uiState, reply), Logger.LogLevel.Info);
                return (reply.IsMatching == false) && reply.CurrentMode == UiModes.Offline && reply.CurrentState == UiStatus.Initializing;

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "CheckAgentStatus", exception,Logger.LogLevel.Error);
                return true;
            }
        }
    }
}
