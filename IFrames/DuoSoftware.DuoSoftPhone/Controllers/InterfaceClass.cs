using DuoSoftware.DuoSoftPhone.Controllers.AgentStatus;
using DuoSoftware.DuoSoftPhone.RefResourceProxy;

namespace DuoSoftware.DuoSoftPhone.Controllers
{
    /// <summary>
    /// IUiState
    /// </summary>
    public interface IUiState
    {
        /// <summary>
        /// ShowNotifications
        /// </summary>
        /// <param name="result"></param>
        void ShowNotifications(ResourceProxyReplyDataResourceProxyReply result);

        /// <summary>
        /// InAgentIdleState
        /// </summary>
        /// <param name="agentPvState"></param>
        void InAgentIdleState(AgentEvent agentPvState);

        /// <summary>
        /// InAgentAcwState
        /// </summary>
        void InAgentAcwState();

        /// <summary>
        /// InCallConnectedState
        /// </summary>
        void InCallConnectedState();

        /// <summary>
        /// InOfflineState
        /// </summary>
        /// <param name="statusText"></param>
        /// <param name="msg"></param>
        /// <param name="statusCode"></param>
        void InOfflineState(string statusText, string msg, int statusCode);

        /// <summary>
        /// InInitiateState
        /// </summary>
        void InInitiateState();

        /// <summary>
        /// InBreakState
        /// </summary>
        void InBreakState();

        /// <summary>
        /// InCallAgentClintConnectedState
        /// </summary>
        void InCallAgentClintConnectedState();

        /// <summary>
        /// InCallAgentSupConnectedState
        /// </summary>
        void InCallAgentSupConnectedState();

        /// <summary>
        /// InCallConferenceState
        /// </summary>
        void InCallConferenceState();

        /// <summary>
        /// InCallDisconnectedState
        /// </summary>
        void InCallDisconnectedState();

        /// <summary>
        /// InCallHoldState
        /// </summary>
        void InCallHoldState();

        /// <summary>
        /// InCallIdleState
        /// </summary>
        void InCallIdleState();

        /// <summary>
        /// InCallTryingState
        /// </summary>
        void InCallTryingState();

        /// <summary>
        /// InCallIncommingState
        /// </summary>
        void InCallIncommingState();

        /// <summary>
        /// OnResourceModeChanged
        /// </summary>
        /// <param name="mode"></param>
        void OnResourceModeChanged(AgentMode mode);

        /// <summary>
        /// OnSendModeChangeRequestOutbound
        /// </summary>
        /// <param name="result"></param>
        void OnSendModeChangeRequestOutbound(ResourceProxyReplyDataResourceProxyReply result);

        /// <summary>
        /// InAgentBusy
        /// </summary>
        void InAgentBusy();

        /// <summary>
        /// CancelBreakRequest
        /// </summary>
        /// <param name="result"></param>
        void CancelBreakRequest(ResourceProxyReplyDataResourceProxyReply result);

        /// <summary>
        /// OnSendModeChangeRequestInbound
        /// </summary>
        /// <param name="result"></param>
        void OnSendModeChangeRequestInbound(ResourceProxyReplyDataResourceProxyReply result);
    }
}