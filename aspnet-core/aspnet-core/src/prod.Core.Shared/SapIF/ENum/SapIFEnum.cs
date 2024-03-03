namespace prod.SapIF.Enum
{
    public enum SapIFType
    {
        FundCommitment,
        OnlineBudgetCheck,
        WbsMaintenance,
        FundCommitmentDM
    }

    public enum SapIFMessageType
    {
        P, //Pending
        E, //Error
        W, //Warning
        S //Success
    }

    public enum SapIFBudgetingType
    {
        O, //Original Budget - Total Amount and do Release WBS Status (REL)
        //REV, //Revision Budget - Total Amount
        //E, //Early Budget (Gap Amount)
        S, //Supplement (Gap Amount)
        //R, //Return (Gap Amount)
        T //Transfer (Gap Amount) (2 WBS)																					
    }

    public enum SapIFBmsStatus
    {
        AALK, //Program locks WBS
        CLSD, // Program closed WBS
        X_AALK, // Program unlocked WBS
        X_CLSD // Program unclosed WBS
        //"" No update Status
    }
}
