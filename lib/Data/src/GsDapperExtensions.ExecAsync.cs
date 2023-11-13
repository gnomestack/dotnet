using System.Data;
using System.Data.Common;

using Dapper;

using GnomeStack.Functional;

using Polly;

using ExecuteResult = GnomeStack.Functional.Result<int, System.Exception>;

namespace GnomeStack.Data;

public static partial class GsDapperExtensions
{
    public static Task<int> ExecAsync(
        this DbConnection connection,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        return connection.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
    }

    public static Task<int> ExecAsync(
        this DbConnection connection,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        var query = builder.Build();
        if (query.IsError)
            query.ThrowIfError();

        var (sql, param) = query.Unwrap();
        return connection.ExecuteAsync(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType);
    }

    public static Task<int> ExecAsync(
        this DbConnection connection,
        CommandDefinition command)
    {
        return connection.ExecuteAsync(command);
    }

    public static async Task<ExecuteResult> ExecAsResultAsync(
        this IDbConnection connection,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        var r = await Result.TryAsync(async () => await connection.ExecuteAsync(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType));
        return r;
    }

    public static async Task<ExecuteResult> ExecAsResultAsync(
        this IDbConnection connection,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        var query = builder.Build();
        if (query.IsError)
            return query.UnwrapError();

        var (sql, param) = query.Unwrap();
        return await Result.TryAsync(async () => await connection.ExecuteAsync(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType));
    }

    public static async Task<ExecuteResult> ExecAsResultAsync(
        this IDbConnection connection,
        CommandDefinition command)
    {
        var r = await Result.TryAsync(async () => await connection.ExecuteAsync(
            command));
        return r;
    }

    public static async Task<ExecuteResult> RetryExecAsync(
        this DbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.ExecuteAsync(
                sql,
                param,
                transaction,
                commandTimeout,
                commandType),
            cancellationToken);
    }

    public static async Task<ExecuteResult> RetryExecAsync(
        this DbConnection cnn,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.ExecAsync(
                builder,
                transaction,
                commandTimeout,
                commandType),
            cancellationToken);
    }

    public static async Task<ExecuteResult> RetryExecAsync(
        this DbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.ExecAsync(command),
            cancellationToken);
    }

    public static async Task<ExecuteResult> RetryExecAsResultAsync(
        this DbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        try
        {
            return await retryPolicy.ExecuteAsync(
                async (_) => await cnn.ExecuteAsync(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType),
                cancellationToken);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static async Task<ExecuteResult> RetryExecAsResultAsync(
        this DbConnection cnn,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        try
        {
            return await retryPolicy.ExecuteAsync(
                async (_) => await cnn.ExecAsync(
                    builder,
                    transaction,
                    commandTimeout,
                    commandType),
                cancellationToken);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static async Task<ExecuteResult> RetryExecAsResultAsync(
        this DbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        try
        {
            return await retryPolicy.ExecuteAsync(
                async (_) => await cnn.ExecuteAsync(
                    command),
                cancellationToken);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}