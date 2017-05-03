namespace DuoSoftware.DuoSoftPhone.Controllers.PortSIP
{
    internal class Session
    {
        // Fields
        private bool mHoldState = false;
        private bool mRecvCallState = false;
        private int mSessionId = 0;
        private bool mSessionState = false;

        // Methods
        public bool getHoldState()
        {
            return this.mHoldState;
        }

        public bool getRecvCallState()
        {
            return this.mRecvCallState;
        }

        public int getSessionId()
        {
            return this.mSessionId;
        }

        public bool getSessionState()
        {
            return this.mSessionState;
        }

        public void reset()
        {
            this.mSessionId = 0;
            this.mHoldState = false;
            this.mSessionState = false;
            this.mRecvCallState = false;
        }

        public void setHoldState(bool state)
        {
            this.mHoldState = state;
        }

        public void setRecvCallState(bool state)
        {
            this.mRecvCallState = state;
        }

        public void setSessionId(int sessionId)
        {
            this.mSessionId = sessionId;
        }

        public void setSessionState(bool state)
        {
            this.mSessionState = state;
        }
    }

}
