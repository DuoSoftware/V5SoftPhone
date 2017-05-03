#region About

/*
Object				:
Purpose             :
Developed By		: Rajinda waruna
Developed On		: Oct 29, 2010
Modified By		    :
Last Updated 		:
Notes				:
*/

#endregion About

using DuoSoftware.DuoTools.DuoLogger;
using DuoSoftware.DuoSoftPhone.Controllers.Common;

#region References

using System;

#endregion References

#region Content

namespace DuoSoftware.DuoSoftPhone.Controllers.CallStatus
{
    /// <summary>
    ///
    /// </summary>
    public class Call
    {
        #region Local Variables

        /// <summary>
        ///
        /// </summary>
        private CallState _callCurrentState;

        /// <summary>
        ///
        /// </summary>
        private CallState _callPvState;

        /// <summary>
        ///
        /// </summary>
        public Guid currentCallLogId;

        #endregion Local Variables

        #region Properties

        /// <summary>
        ///
        /// </summary>
        private IUiState UiState { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int portSipSessionId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public CallState CallCurrentState
        {
            get
            {
                return _callCurrentState;
            }
            set
            {
                _callPvState = _callCurrentState;
                _callCurrentState = value;
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, string.Format("Call Current States [{0}] To [{1}]", _callPvState, value), Logger.LogLevel.Info);
                ChangeUI(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string PhoneNo { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string CallSessionId { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        ///
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="uiState"></param>
        public Call(string phoneNo, IUiState uiState)
        {
            UiState = uiState;
            PhoneNo = phoneNo;
            CallCurrentState = new CallIdleState();
        }

        #endregion Constructors

        #region Interface Implementations

        #endregion Interface Implementations

        #region Events

        #region Phone events

        #endregion Phone events

        #endregion Events

        #region Methods

        #region Private Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        private void ChangeUI(CallState state)
        {
            try
            {
                new ComMethods.SwitchOnType<CallState>(state)
                    .Case<CallAgentClintConnectedState>(initiate => UiState.InCallAgentClintConnectedState())
                    .Case<CallAgentSupConnectedState>(i => UiState.InCallAgentSupConnectedState())
                    .Case<CallConferenceState>(b => UiState.InCallConferenceState())
                     .Case<CallConnectedState>(b => UiState.InCallConnectedState())
                     .Case<CallIncommingState>(b => UiState.InCallIncommingState())
                      .Case<CallDisconnectedState>(b => UiState.InCallDisconnectedState())
                        .Case<CallHoldState>(b => UiState.InCallHoldState())
                         .Case<CallIdleState>(b => UiState.InCallIdleState())
                    //.Case<CallRingingState>(b => UiState.InCallRingingState())
                         .Case<CallTryingState>(b => UiState.InCallTryingState())
                    .Default<CallIdleState>(t => UiState.InCallIdleState());
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "Call ChangeUI", exception, Logger.LogLevel.Error);
            }
        }

        #endregion Private Methods

        #region protected Methods

        #endregion protected Methods

        #region Internal

        #endregion Internal

        #region Public Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="portsipSessionId"></param>
        /// <param name="newGuid"></param>
        public void SetDialInfo(int portsipSessionId, Guid newGuid)
        {
            currentCallLogId = newGuid;
            portSipSessionId = portsipSessionId;
        }

        #endregion Public Methods

        #endregion Methods
    }
}

#endregion Content