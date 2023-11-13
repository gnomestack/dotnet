using System.Data;

using Dapper;

using GnomeStack.Functional;

using Polly;

using ScalarResult = GnomeStack.Functional.Result<object, System.Exception>;

namespace GnomeStack.Data;

public static partial class GsDapperExtensions
{
    public static Task<object?> ScalarAsync(
        this IDbConnection connection,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        return connection.ExecuteScalarAsync(sql, param, transaction, commandTimeout, commandType);
    }

    public static Task<object?> ScalarAsync(
        this IDbConnection connection,
        CommandDefinition command)
    {
        return connection.ExecuteScalarAsync(command);
    }

    public static Task<object?> ScalarAsync(
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
        return connection.ExecuteScalarAsync(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType);
    }

    public static Task<T> ScalarAsync<T>(
        this IDbConnection connection,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        return connection.ExecuteScalarAsync<T>(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType);
    }

    public static Task<T> ScalarAsync<T>(
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
        return connection.ExecuteScalarAsync<T>(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType);
    }

    public static Task<T> ScalarAsync<T>(
        this IDbConnection connection,
        CommandDefinition command)
    {
        return connection.ExecuteScalarAsync<T>(command);
    }

    public static Task<ScalarResult> ScalarAsResultAsync(
        this IDbConnection connection,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        return Result.TryAsync<object, Exception>(() => connection.ExecuteScalarAsync(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType));
    }

    public static Task<ScalarResult> ScalarAsResultAsync(
        this IDbConnection connection,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        var query = builder.Build();
        if (query.IsError)
            return Result.Error<object>(query.UnwrapError());

        var (sql, param) = query.Unwrap();
        return Result.TryAsync(() => connection.ExecuteScalarAsync(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType));
    }

    public static Task<ScalarResult> ScalarAsResultAsync(
        this IDbConnection connection,
        CommandDefinition command)
    {
        return Result.TryAsync(() => connection.ExecuteScalarAsync(command));
    }

    public static Task<Result<T, Exception>> ScalarAsResultAsync<T>(
        this IDbConnection connection,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
        where T : notnull
    {
        return Result.TryAsync(() => connection.ExecuteScalarAsync<T>(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType));
    }

    public static Task<Result<T, Exception>> ScalarAsResultAsync<T>(
        this IDbConnection connection,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
        where T : notnull
    {
        var query = builder.Build();
        if (query.IsError)
            return Result.Error<T>(query.UnwrapError());

        var (sql, param) = query.Unwrap();
        return Result.TryAsync(() => connection.ExecuteScalarAsync<T>(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType));
    }

    public static Task<Result<T, Exception>> ScalarAsResultAsync<T>(
        this IDbConnection connection,
        CommandDefinition command)
        where T : notnull
    {
        return Result.TryAsync(() => connection.ExecuteScalarAsync<T>(command));
    }

    public static async Task<object?> RetryScalarAsync(
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
            async (_) => await cnn.ScalarAsync(sql, param, transaction, commandTimeout, commandType),
            cancellationToken).NoCap();
    }

    public static async Task<object?> RetryScalarAsync(
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
            async (_) =>
                await cnn.ScalarAsync(
                    builder,
                    transaction,
                    commandTimeout,
                    commandType),
            cancellationToken);
    }

    public static async Task<object?> RetryScalarAsync(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_)
                => await cnn.ExecuteScalarAsync(command),
            cancellationToken);
    }

    public static async Task<T> RetryScalarAsync<T>(
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
            async (_)
                => await cnn.ScalarAsync<T>(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType),
            cancellationToken);
    }

    public static async Task<T> RetryScalarAsync<T>(
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
            async (_) =>
                await cnn.ScalarAsync<T>(
                    builder,
                    transaction,
                    commandTimeout,
                    commandType),
            cancellationToken);
    }

    public static async Task<T> RetryScalarAsync<T>(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.ScalarAsync<T>(command),
            cancellationToken);
    }

    public static async Task<ScalarResult> RetryScalarAsResultTask(
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
            var result = await retryPolicy.ExecuteAsync(
                async (_) => await cnn.ScalarAsync(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType),
                cancellationToken);
            if (result is not null)
                return result;

            return new NullReferenceException("Scalar result is null");
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static async Task<ScalarResult> RetryScalarAsResultAsync(
        this IDbConnection cnn,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        try
        {
            var result = retryPolicy.Execute(() => cnn.Scalar(builder, transaction, commandTimeout, commandType));
            if (result is not null)
                return result;

            return new NullReferenceException("Scalar result is null");
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static async Task<ScalarResult> RetryScalarAsResultAsync(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        try
        {
            var result = await retryPolicy.ExecuteAsync(
                async (_) => await cnn.ScalarAsync(command),
                cancellationToken);
            if (result is not null)
                return result;

            return new NullReferenceException("Scalar result is null");
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static async Task<Result<T, Exception>> RetryScalarAsResultAsync<T>(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
        where T : notnull
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        try
        {
            return await retryPolicy.ExecuteAsync(
                async (_) => await cnn.ScalarAsync<T>(
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

    public static async Task<Result<T, Exception>> RetryScalarAsResultAsync<T>(
        this IDbConnection cnn,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
        where T : notnull
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        try
        {
            return await retryPolicy.ExecuteAsync(
                async (_) => await cnn.ScalarAsync<T>(
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

    public static async Task<Result<T, Exception>> RetryScalarAsResultAsync<T>(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
        where T : notnull
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        try
        {
            return await retryPolicy.ExecuteAsync(
                async (_) => await cnn.ScalarAsync<T>(command),
                cancellationToken);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}