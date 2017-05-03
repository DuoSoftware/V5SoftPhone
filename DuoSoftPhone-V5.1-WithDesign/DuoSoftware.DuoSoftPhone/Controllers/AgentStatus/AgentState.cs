namespace DuoSoftware.DuoSoftPhone.Controllers.AgentStatus
{
    public enum CallDirection
    {
        Incoming = 0,
        Outgoing = 1,
    }

    public enum AgentMode
    {
        Offline = 0,
        Inbound = 1,
        Outbound = 2,
    }

    public abstract class AgentState
    {
        public abstract void OnTimeOut(ref Agent agent);
        public abstract void OnLogOn(ref Agent agent);
        public abstract void OnMakeCall(ref Agent agent);
        public abstract void OnAnswerCall(ref Agent agent, string callSessionId);
        public abstract void OnEndCall(ref Agent agent, string callSessionId);
        public abstract void OnEndCallLog(ref Agent agent, string callSessionId);
        public abstract void OnChangeAgentStatus(ref Agent agent);
        public abstract void OnLoggedOn(ref Agent agent, string callSessionId);
        public abstract void OnLogOff(ref Agent agent);
        public abstract void OnFailMakeCall(ref Agent agent);
        public abstract void OnCallReject(ref Agent agent, string callSessionId);
        public abstract void OnOffline(ref Agent agent, string statusText);
        public abstract void OnAgentChangeMode(ref Agent agent);
        public abstract void OnResetStatus(ref Agent agent);
        public abstract void OnRequestAgentModeChange(ref Agent agent, AgentMode agentMode);
        public abstract void OnBreak(ref Agent agent);
        public abstract void OnEndBreak(ref Agent agent);
        public abstract void OnRequestAgentBreak(ref Agent agent, string callSessionId, string breakReason);
        public abstract void OnRequestAgentBreakCancel(ref Agent agent, string callSessionId);
    }
}