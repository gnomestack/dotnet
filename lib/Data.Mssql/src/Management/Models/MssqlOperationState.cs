namespace GnomeStack.Data.Mssql.Management.Models;

public enum MssqlOperationState
{
    Pending = 0,
    InProgress = 1,
    Completed = 2,
    Failed = 3,
    CancelInProgress = 4,
    Cancelled = 5,
}