using System.Data;

using Dapper;

using GnomeStack.Functional;

using Polly;

namespace GnomeStack.Data;

public static partial class GsDapperExtensions
{
    public static Task<IEnumerable<T>> QueryAsync<T>(
        this IDbConnection cnn,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        var query = builder.Build();
        if (query.IsError)
            query.ThrowIfError();

        var (sql, param) = query.Unwrap();
        return cnn.QueryAsync<T>(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType);
    }

    public static async Task<Result<IEnumerable<T>, Exception>> QueryAsResultAsync<T>(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        try
        {
            var results = await cnn.QueryAsync<T>(
                sql,
                param,
                transaction,
                commandTimeout,
                commandType);

            return Result.Ok<IEnumerable<T>, Exception>(results);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static async Task<Result<IEnumerable<T>, Exception>> QueryAsResultAsync<T>(
        this IDbConnection cnn,
        string sql,
        Type[] types,
        Func<object[], T> map,
        object? param = null,
        IDbTransaction? transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        try
        {
            var results = await cnn.QueryAsync<T>(
                sql,
                types,
                map,
                param,
                transaction,
                buffered,
                splitOn,
                commandTimeout,
                commandType);

            return Result.Ok<IEnumerable<T>, Exception>(results);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static async Task<Result<IEnumerable<T>, Exception>> QueryAsResultAsync<T>(
        this IDbConnection cnn,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        try
        {
            var query = builder.Build();
            if (query.IsError)
                return query.UnwrapError();

            var (sql, param) = query.Unwrap();
            var results = await cnn.QueryAsync<T>(
                sql,
                param,
                transaction,
                commandTimeout,
                commandType);

            return Result.Ok<IEnumerable<T>, Exception>(results);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static Task<Result<IEnumerable<T>, Exception>> QueryAsResultAsync<T>(
        this IDbConnection cnn,
        CommandDefinition command)
    {
        var results = cnn.Query<T>(command);

        return Result.Ok<IEnumerable<T>, Exception>(results);
    }

    public static async Task<IEnumerable<T>> RetryQueryAsync<T>(
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
            async (_) => await cnn.QueryAsync<T>(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType),
            cancellationToken);
    }

    public static async Task<IEnumerable<T>> RetryQueryAsync<T>(
        this IDbConnection cnn,
        string sql,
        Type[] types,
        Func<object[], T> map,
        object? param = null,
        IDbTransaction? transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QueryAsync<T>(
                sql,
                types,
                map,
                param,
                transaction,
                buffered,
                splitOn,
                commandTimeout,
                commandType),
            cancellationToken);
    }

    public static async Task<IEnumerable<T>> RetryQueryAsync<T>(
        this IDbConnection cnn,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        var query = builder.Build();
        if (query.IsError)
            query.ThrowIfError();

        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QueryAsync<T>(
                builder,
                transaction,
                commandTimeout,
                commandType),
            cancellationToken);
    }

    public static async Task<IEnumerable<T>> RetryQueryAsync<T>(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QueryAsync<T>(command),
            cancellationToken);
    }

    public static async Task<Result<IEnumerable<T>, Exception>> RetryQueryAsResultAsync<T>(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        try
        {
            var results = await retryPolicy.ExecuteAsync(
                async (_) => await cnn.QueryAsync<T>(command),
                cancellationToken).NoCap();
            return Result.Ok<IEnumerable<T>, Exception>(results);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static async Task<Result<IEnumerable<T>, Exception>> RetryQueryAsResultAsync<T>(
        this IDbConnection cnn,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        var query = builder.Build();
        if (query.IsError)
            return query.UnwrapError();

        var (sql, param) = query.Unwrap();
        retryPolicy ??= SqlRetryPolicy.Default;
        try
        {
            var results = await retryPolicy.ExecuteAsync(
                async (_) => await cnn.QueryAsync<T>(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType),
                cancellationToken);
            return Result.Ok<IEnumerable<T>, Exception>(results);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static async Task<Result<IEnumerable<T>, Exception>> RetryQueryAsResultAsync<T>(
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
            var results = await retryPolicy.ExecuteAsync(
                async (_) => await cnn.QueryAsync<T>(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType),
                cancellationToken);
            return Result.Ok<IEnumerable<T>, Exception>(results);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static async Task<Result<IEnumerable<T>, Exception>> RetryQueryAsResultAsync<T>(
        this IDbConnection cnn,
        string sql,
        Type[] types,
        Func<object[], T> map,
        object? param = null,
        IDbTransaction? transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        try
        {
            var results = await retryPolicy.ExecuteAsync(
                async (_) => await cnn.QueryAsync<T>(
                    sql,
                    types,
                    map,
                    param,
                    transaction,
                    buffered,
                    splitOn,
                    commandTimeout,
                    commandType),
                cancellationToken);
            return Result.Ok<IEnumerable<T>, Exception>(results);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}