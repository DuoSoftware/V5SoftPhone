using System;

namespace DuoSoftware.DuoSoftPhone.Controllers.Common
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public enum CallFunction
    {
        MakeCall = 0,
        EndCall = 1,
        HoldCall = 2,
        TransferCall = 3,
    }

    /// <summary>
    ///
    /// </summary>
    public struct CallLog
    {
        public string PhoneNo;
        public double Durations;
        public DateTime Time;
        public int Direction;//0 -incoming, 1-outgoing , 2- miscall

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var c = (CallLog)obj;
            return ((PhoneNo == c.PhoneNo) && (Math.Abs(Durations - c.Durations) < 0.1) && (Time == c.Time) && (Direction == c.Direction));
        }
        public override int GetHashCode()
        {
            return 0;
        }
        public static bool operator ==(CallLog left, CallLog right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CallLog left, CallLog right)
        {
            return !(left == right);
        }
    }

    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public enum CallAction
    {
        Error = 0,
        IncommingCallRequest = 1,
        CallInProgress = 2,
        Call = 3,

        HoldRequested = 4,
        HoldInProgress = 5,
        Hold = 6,

        UnHoldRequested = 7,
        UnHoldInProgress = 8,
        UnHold = 9,

        CallTransferRequested = 10,
        CallTransferInProgress = 11,
        CallTransferred = 12,

        CallSwapRequested = 13,
        CallSwapInProgress = 14,
        CallSwap = 15,

        ConferenceCallRequested = 16,
        ConferenceCallInProgress = 17,
        ConferenceCall = 18,

        EtlRequested = 19,
        EtlInProgress = 20,
        EtlCall = 21,


    }
}