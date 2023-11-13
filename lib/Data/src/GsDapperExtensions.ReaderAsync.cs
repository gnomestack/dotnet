using System.Data;
using System.Data.Common;

using Dapper;

using GnomeStack.Functional;

using Polly;

using ReaderResult = GnomeStack.Functional.Result<System.Data.IDataReader, System.Exception>;

namespace GnomeStack.Data;

public static partial class GsDapperExtensions
{
    public static Task<IDataReader> ReaderAsync(
        this IDbConnection connection,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        return connection.ExecuteReaderAsync(sql, param, transaction, commandTimeout, commandType);
    }

    public static Task<IDataReader> ReaderAsync(
        this IDbConnection connection,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        var query = builder.Build();
        if (query.IsError)
            query.ThrowIfError();

        var (sql, param) = query.Unwrap();
        return connection.ExecuteReaderAsync(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType);
    }

    public static Task<IDataReader> ReaderAsync(
        this IDbConnection connection,
        CommandDefinition command)
    {
        return connection.ExecuteReaderAsync(command);
    }

    public static Task<ReaderResult> ReaderAsResultAsync(
        this IDbConnection connection,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        CancellationToken cancellationToken = default)
    {
        return Result.TryAsync(() => connection.ExecuteReaderAsync(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType));
    }

    public static Task<ReaderResult> ReaderAsResultAsync(
        this IDbConnection connection,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        CancellationToken cancellationToken = default)
    {
        var query = builder.Build();
        if (query.IsError)
            return Result.Error<IDataReader>(query.UnwrapError());

        var (sql, param) = query.Unwrap();
        return Result.TryAsync(() => connection.ExecuteReaderAsync(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType));
    }

    public static Task<ReaderResult> ReaderAsResultAsync(
        this IDbConnection connection,
        CommandDefinition command)
    {
        return Result.TryAsync(()
            => connection.ExecuteReaderAsync(command));
    }

    public static async Task<IDataReader> RetryReaderAsync(
        this IDbConnection cnn,
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
            async (_) => await cnn.ExecuteReaderAsync(
                sql,
                param,
                transaction,
                commandTimeout,
                commandType),
            cancellationToken);
    }

    public static async Task<IDataReader> RetryReaderAsync(
        this IDbConnection cnn,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.ReaderAsync(
                builder,
                transaction,
                commandTimeout,
                commandType),
            cancellationToken);
    }

    public static async Task<IDataReader> RetryReaderAsync(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.ExecuteReaderAsync(command),
            cancellationToken);
    }

    public static async Task<ReaderResult> RetryReaderAsResultAsync(
        this IDbConnection cnn,
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
            var dr = await retryPolicy.ExecuteAsync(
                async (_) => await cnn.ReaderAsync(
                    builder,
                    transaction,
                    commandTimeout,
                    commandType),
                cancellationToken);
            return ReaderResult.Ok(dr);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static async Task<ReaderResult> RetryReaderAsResultAsync(
        this IDbConnection cnn,
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
            var dr = await retryPolicy.ExecuteAsync(
                async (_) => await cnn.ExecuteReaderAsync(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType),
                cancellationToken);
            return ReaderResult.Ok(dr);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static async Task<ReaderResult> RetryReaderAsResultAsync(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        try
        {
            var dr = await retryPolicy.ExecuteAsync(
                async (_) => await cnn.ExecuteReaderAsync(command),
                cancellationToken);

            return ReaderResult.Ok(dr);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}