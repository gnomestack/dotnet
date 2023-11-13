namespace GnomeStack.Data.Mssql.Management.Models;

public class MssqlOperationRecord
{
    public MssqlOperationState State { get; set; }

    public string MajorResourceId { get; set; } = string.Empty;

    public string Operation { get; set; } = string.Empty;

    public double PercentComplete { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? LastModifyTime { get; set; }

    public long? ErrorCode { get; set; }

    public string? ErrorMessage { get; set; }
}