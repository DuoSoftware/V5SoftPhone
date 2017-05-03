namespace DuoSoftware.DuoSoftPhone.Controllers
{
    public interface IUiState
    {
        void ShowCallLogs();
        void ShowSetting();
        void InIdleState();
        void InAcwState();
        //void InRiggingState();
        void InCallConnectedState();
        void InOfflineState();
        void InInitiateState();
        void InInitiateMsgState(bool autoAnswerchk, bool autoAnswerEnb, string userName);
        void Error(string statusText);
        void InBreakState();
        void InCallAgentClintConnectedState();
        void InCallAgentSupConnectedState(CallActions callAction);
        void InCallConferenceState();
        void InCallDisconnectedState();
        void InCallHoldState(CallActions callAction);
        void InCallIdleState();
        void InCallRingingState();
        void InCallTryingState();
        void InCallIncommingState();
    }

   
}
