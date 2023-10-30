using System.Data.Common;

using Dapper;

using GnomeStack.Data.Management;
using GnomeStack.Data.SqlServer.Management;
using GnomeStack.Data.SqlServer.Management.Models;
using GnomeStack.Functional;

using Microsoft.Data.SqlClient;

namespace GnomeStack.Data.SqlServer;

public class ServerExecutor
{
    public ServerExecutor(DbConnection connection)
    {
        this.Connection = connection;
    }

    protected DbConnection Connection { get; set; }

    public async Task<Result<int, Exception>> CreateMssqlDatabaseAsync(
        CreateDatabase createDatabase,
        CancellationToken cancellationToken = default)
    {
        var query = createDatabase.Build();
        if (query.IsError)
            return query.UnwrapError();

        return await Result.TryAsync(async () => await this.Connection.ExecuteAsync(query.Unwrap()));
    }

    public async Task<Result<CreateMssqlLogin, Exception>> CreateMssqlLoginAsync(
        CreateMssqlLogin createLogin,
        CancellationToken cancellationToken = default)
    {
        var query = createLogin.Build();
        if (query.IsError)
            return query.UnwrapError();

        return await Result.TryAsync(async () =>
        {
            await this.Connection.ExecuteAsync(query.Unwrap()).NoCap();
            return createLogin;
        });
    }

    public async Task<Result<bool, Exception>> DatabaseExistsAsync(
        SelectDbExists selectDbExists,
        CancellationToken cancellationToken = default)
    {
        var query = selectDbExists.Build();
        if (query.IsError)
            return query.UnwrapError();

        return await Result.TryAsync(async () =>
        {
            var r = await this.Connection.ExecuteScalarAsync<int>(query.Unwrap());
            return r == 1;
        });
    }

    public async Task<Result<int, Exception>> DropDatabaseAsync(
        DropMssqlDatabase dropDatabase,
        CancellationToken cancellationToken = default)
    {
        var query = dropDatabase.Build();
        if (query.IsError)
            return query.UnwrapError();

        return await Result.TryAsync<int, Exception>(async () =>
        {
            var subQuery = new DropMssqlConnections { DatabaseName = dropDatabase.DatabaseName };
            var subQueryResult = subQuery.Build();
            if (subQueryResult.IsError)
                subQueryResult.ThrowIfError();

            await this.Connection.ExecuteAsync(subQueryResult.Unwrap()).NoCap();
            return await this.Connection.ExecuteAsync(query.Unwrap()).NoCap();
        });
    }

    public async Task<Result<int, Exception>> ShrinkDatabaseAsync(
        ShrinkDatabase shrinkDatabase,
        CancellationToken cancellationToken = default)
    {
        var query = shrinkDatabase.Build();
        if (query.IsError)
            return query.UnwrapError();

        cancellationToken.ThrowIfCancellationRequested();

        return await Result.TryAsync(async () => await this.Connection.ExecuteAsync(query.Unwrap()));
    }

    public async Task<Result<IReadOnlyList<MssqlOperationRecord>, Exception>> GetOperationRecordAsync(
        SelectOperationStatus operationStatus,
        CancellationToken cancellationToken = default)
    {
        var query = operationStatus.Build();
        if (query.IsError)
            return query.UnwrapError();

        return await Result.TryAsync<IReadOnlyList<MssqlOperationRecord>>(async () =>
        {
            var list = new List<MssqlOperationRecord>();
            using var dr = await this.Connection.ExecuteReaderAsync(operationStatus);
            while (await dr.ReadAsync(cancellationToken).NoCap())
            {
                var r = new MssqlOperationRecord
                {
                    State = (MssqlOperationState)dr.GetInt32(0),
                    MajorResourceId = dr.GetString(1),
                    Operation = dr.GetString(2),
                    PercentComplete = dr.GetDouble(3),
                    StartTime = dr.GetDateTime(4),
                    LastModifyTime = dr.GetNullableDateTime(5),
                    ErrorCode = dr.GetNullableInt64(6),
                    ErrorMessage = dr.GetNullableString(7),
                };
                list.Add(r);
            }

            return list;
        });
    }
}