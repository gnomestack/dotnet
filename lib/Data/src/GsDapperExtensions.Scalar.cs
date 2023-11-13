using System.Data;

using Dapper;

using GnomeStack.Functional;

using Polly;

using ScalarResult = GnomeStack.Functional.Result<object, System.Exception>;

namespace GnomeStack.Data;

public static partial class GsDapperExtensions
{
    public static object? Scalar(
        this IDbConnection connection,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        return connection.ExecuteScalar(sql, param, transaction, commandTimeout, commandType);
    }

    public static object? Scalar(
        this IDbConnection connection,
        CommandDefinition command)
    {
        return connection.ExecuteScalar(command);
    }

    public static object? Scalar(
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
        return connection.ExecuteScalar(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType);
    }

    public static T Scalar<T>(
        this IDbConnection connection,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        return connection.ExecuteScalar<T>(sql, param, transaction, commandTimeout, commandType);
    }

    public static T Scalar<T>(
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
        return connection.ExecuteScalar<T>(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType);
    }

    public static T Scalar<T>(
        this IDbConnection connection,
        CommandDefinition command)
    {
        return connection.ExecuteScalar<T>(command);
    }

    public static ScalarResult ScalarAsResult(
        this IDbConnection connection,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        return Result.Try<object, Exception>(() => connection.ExecuteScalar(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType));
    }

    public static ScalarResult ScalarAsResult(
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
        return Result.Try(() => connection.ExecuteScalar(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType));
    }

    public static ScalarResult ScalarAsResult(
        this IDbConnection connection,
        CommandDefinition command)
    {
        return Result.Try(() => connection.ExecuteScalar(command));
    }

    public static Result<T, Exception> ScalarAsResult<T>(
        this IDbConnection connection,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
        where T : notnull
    {
        return Result.Try(() => connection.ExecuteScalar<T>(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType));
    }

    public static Result<T, Exception> ScalarAsResult<T>(
        this IDbConnection connection,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
        where T : notnull
    {
        var query = builder.Build();
        if (query.IsError)
            return query.UnwrapError();

        var (sql, param) = query.Unwrap();
        return Result.Try(() => connection.ExecuteScalar<T>(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType));
    }

    public static Result<T, Exception> ScalarAsResult<T>(
        this IDbConnection connection,
        CommandDefinition command)
        where T : notnull
    {
        return Result.Try(() => connection.ExecuteScalar<T>(command));
    }

    /// <summary>
    /// Execute parameterized SQL.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch.</param>
    /// <param name="retryPolicy">The retry policy.</param>
    /// <returns>The number of rows affected.</returns>
    public static object? RetryScalar(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return retryPolicy.Execute(() => cnn.Scalar(sql, param, transaction, commandTimeout, commandType));
    }

    public static object? RetryScalar(
        this IDbConnection cnn,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return retryPolicy.Execute(() => cnn.Scalar(builder, transaction, commandTimeout, commandType));
    }

    /// <summary>
    /// Execute parameterized SQL.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="command">The command to execute on this connection.</param>
    /// <param name="retryPolicy">The retry policy.</param>
    /// <returns>The number of rows affected.</returns>
    public static object? RetryScalar(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return retryPolicy.Execute(() => cnn.ExecuteScalar(command));
    }

    public static T RetryScalar<T>(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return retryPolicy.Execute(() => cnn.Scalar<T>(sql, param, transaction, commandTimeout, commandType));
    }

    public static T RetryScalar<T>(
        this IDbConnection cnn,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return retryPolicy.Execute(() => cnn.Scalar<T>(builder, transaction, commandTimeout, commandType));
    }

    public static T RetryScalar<T>(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return retryPolicy.Execute(() => cnn.Scalar<T>(command));
    }

    public static ScalarResult RetryScalarAsResult(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        try
        {
            var result = retryPolicy.Execute(() => cnn.Scalar(sql, param, transaction, commandTimeout, commandType));
            if (result is not null)
                return result;

            return new NullReferenceException("Scalar result is null");
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static ScalarResult RetryScalarAsResult(
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

    public static ScalarResult RetryScalarAsResult(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        try
        {
            var result = retryPolicy.Execute(() => cnn.Scalar(command));
            if (result is not null)
                return result;

            return new NullReferenceException("Scalar result is null");
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static Result<T, Exception> RetryScalarAsResult<T>(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null)
        where T : notnull
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        try
        {
            return retryPolicy.Execute(() => cnn.Scalar<T>(sql, param, transaction, commandTimeout, commandType));
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static Result<T, Exception> RetryScalarAsResult<T>(
        this IDbConnection cnn,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null)
        where T : notnull
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        try
        {
            return retryPolicy.Execute(() => cnn.Scalar<T>(builder, transaction, commandTimeout, commandType));
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static Result<T, Exception> RetryScalarAsResult<T>(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null)
        where T : notnull
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        try
        {
            return retryPolicy.Execute(() => cnn.Scalar<T>(command));
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}