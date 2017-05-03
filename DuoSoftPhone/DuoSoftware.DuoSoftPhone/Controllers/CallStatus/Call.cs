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

#endregion

using DuoSoftware.DuoAuth.RefAuth.Iauth;
using DuoSoftware.DuoSoftPhone.Controllers.Service;
using DuoSoftware.DuoSoftPhone.Ui;

#region References

using System;

#endregion

#region Content

namespace DuoSoftware.DuoSoftPhone.Controllers.CallStatus
{
    public class Call
    {
        #region Local Variables

        private CallState _currentState;

        #endregion

        #region Properties

        private FormDialPad _dialPad { set; get; }

        public UserAuth UserAuth { get; set; }

        public String CallSessionId { get; set; }

        public int PortSipSessionId { get; set; }

        public CallState CurrentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                _currentState = value;
                _dialPad.SetDialPadUi(value);
            }
        }

        public ProfileInfo SipProfile { get; set; }
        public string AgentSessionId { get; set; }

        #endregion

        #region Constructors

        public Call(FormDialPad dialPad)
        {
            CallSessionId = string.Empty;
            AgentSessionId = string.Empty;
            PortSipSessionId = -1;
            _dialPad = dialPad;
            UserAuth = dialPad.Auth;
        }

        #endregion

        #region Interface Implementations

        #endregion

        #region Events

        #region Phone events

        #endregion

        #endregion

        #region Methods

        #region Private Methods

        #endregion

        #region protected Methods

        #endregion

        #region Internal

        #endregion

        #region Public Methods

        #endregion

        #endregion

        
    }
}

#endregion