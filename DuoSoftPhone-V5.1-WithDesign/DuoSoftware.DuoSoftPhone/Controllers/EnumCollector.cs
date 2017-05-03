using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoSoftware.DuoSoftPhone.Controllers
{
    public enum CallActions
    {
        Error = 0,
        Incomming_Call_Request = 1,
        Call_InProgress = 2,
        Call = 3,

        Hold_Requested = 4,
        Hold_InProgress = 5,
        Hold = 6,

        UnHold_Requested = 7,
        UnHold_InProgress = 8,
        UnHold = 9,

        Call_Transfer_Requested = 10,
        Call_Transfer_InProgress = 11,
        Call_Transferred = 12,

        Call_Swap_Requested = 13,
        Call_Swap_InProgress = 14,
        Call_Swap = 15,

        Conference_Call_Requested = 16,
        Conference_Call_InProgress = 17,
        Conference_Call = 18,

        ETL_Requested = 19,
        ETL_InProgress = 20,
        ETL_Call = 21,
    }

    public enum CallFunctions
    {
        MakeCall = 0,
        EndCall = 1,
        HoldCall = 2,
        TransferCall = 3,
    }

    public enum DialerState
    {
        Initiate,
        NotOnCall,
        CallRouted,
        CallIncoming,
        CallOutgoing,
        OnHold,
        AgentSupervisorTalking,
        AgentClientTalking,
        Conference,
        etlCall,
        swapCall,
        transferCall,
        transferIvr,
        Error,
        ACW,
    }
}
