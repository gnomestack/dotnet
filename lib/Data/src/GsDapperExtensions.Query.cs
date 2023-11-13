using System.Data;
using System.Data.Common;

using Dapper;

using GnomeStack.Functional;

using Polly;

namespace GnomeStack.Data;

public static partial class GsDapperExtensions
{
    public static IEnumerable<T> Query<T>(
        this IDbConnection cnn,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        bool buffered = true,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        var query = builder.Build();
        if (query.IsError)
            query.ThrowIfError();

        var (sql, param) = query.Unwrap();
        return cnn.Query<T>(
            sql,
            param,
            transaction: transaction,
            buffered,
            commandTimeout,
            commandType);
    }

    public static Result<IEnumerable<T>, Exception> QueryAsResult<T>(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        bool buffered = true,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        var results = cnn.Query<T>(
            sql,
            param,
            transaction,
            buffered,
            commandTimeout,
            commandType);

        return Result.Ok<IEnumerable<T>, Exception>(results);
    }

    public static Result<IEnumerable<T>, Exception> QueryAsResult<T>(
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
        var results = cnn.Query<T>(
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

    public static Result<IEnumerable<T>, Exception> QueryAsResult<T>(
        this IDbConnection cnn,
        CommandDefinition command)
    {
        var results = cnn.Query<T>(command);

        return Result.Ok<IEnumerable<T>, Exception>(results);
    }

    public static Result<IEnumerable<T>, Exception> QueryAsResult<T>(
        this IDbConnection cnn,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        bool buffered = true,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        var query = builder.Build();
        if (query.IsError)
            return query.Map(_ => (IEnumerable<T>)Array.Empty<T>());

        var (sql, param) = query.Unwrap();
        var results = cnn.Query<T>(
            sql,
            param,
            transaction: transaction,
            buffered,
            commandTimeout,
            commandType);

        return Result.Ok<IEnumerable<T>, Exception>(results);
    }

    public static IEnumerable<T> RetryQuery<T>(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        bool buffered = true,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return retryPolicy.Execute(() => cnn.Query<T>(
            sql,
            param,
            transaction,
            buffered,
            commandTimeout,
            commandType));
    }

    public static IEnumerable<T> RetryQuery<T>(
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
        ResiliencePipeline? retryPolicy = null)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return retryPolicy.Execute(() => cnn.Query<T>(
            sql,
            types,
            map,
            param,
            transaction,
            buffered,
            splitOn,
            commandTimeout,
            commandType));
    }

    public static IEnumerable<T> RetryQuery<T>(
        this IDbConnection cnn,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        bool buffered = true,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null)
    {
        var query = builder.Build();
        if (query.IsError)
            query.ThrowIfError();

        var (sql, param) = query.Unwrap();
        retryPolicy ??= SqlRetryPolicy.Default;
        return retryPolicy.Execute(() => cnn.Query<T>(
            sql,
            param,
            transaction,
            buffered,
            commandTimeout,
            commandType));
    }

    public static IEnumerable<T> RetryQuery<T>(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return retryPolicy.Execute(() => cnn.Query<T>(command));
    }

    public static Result<IEnumerable<T>, Exception> RetryQueryAsResult<T>(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        try
        {
            var results = retryPolicy.Execute(() => cnn.Query<T>(command));
            return Result.Ok<IEnumerable<T>, Exception>(results);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static Result<IEnumerable<T>, Exception> RetryQueryAsResult<T>(
        this IDbConnection cnn,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        bool buffered = true,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null)
    {
        var query = builder.Build();
        if (query.IsError)
            return query.Map(_ => (IEnumerable<T>)Array.Empty<T>());

        var (sql, param) = query.Unwrap();
        retryPolicy ??= SqlRetryPolicy.Default;
        try
        {
            var results = retryPolicy.Execute(() => cnn.Query<T>(
                sql,
                param,
                transaction,
                buffered,
                commandTimeout,
                commandType));
            return Result.Ok<IEnumerable<T>, Exception>(results);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static Result<IEnumerable<T>, Exception> RetryQueryAsResult<T>(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        bool buffered = true,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        try
        {
            var results = retryPolicy.Execute(() => cnn.Query<T>(
                sql,
                param,
                transaction,
                buffered,
                commandTimeout,
                commandType));
            return Result.Ok<IEnumerable<T>, Exception>(results);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static Result<IEnumerable<T>, Exception> RetryQueryAsResult<T>(
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
        ResiliencePipeline? retryPolicy = null)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        try
        {
            var results = retryPolicy.Execute(() => cnn.Query<T>(
                sql,
                types,
                map,
                param,
                transaction,
                buffered,
                splitOn,
                commandTimeout,
                commandType));
            return Result.Ok<IEnumerable<T>, Exception>(results);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}