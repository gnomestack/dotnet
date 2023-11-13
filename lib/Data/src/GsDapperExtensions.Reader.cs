using System.Data;
using System.Data.Common;

using Dapper;

using GnomeStack.Functional;

using Polly;

using ReaderResult = GnomeStack.Functional.Result<System.Data.IDataReader, System.Exception>;

namespace GnomeStack.Data;

public static partial class GsDapperExtensions
{
    public static IDataReader Reader(
        this IDbConnection connection,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        return connection.ExecuteReader(sql, param, transaction, commandTimeout, commandType);
    }

    public static IDataReader Reader(
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
        return connection.ExecuteReader(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType);
    }

    public static IDataReader Reader(
        this IDbConnection connection,
        CommandDefinition command)
    {
        return connection.ExecuteReader(command);
    }

    public static ReaderResult ReaderAsResult(
        this IDbConnection connection,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        CancellationToken cancellationToken = default)
    {
        return Result.Try(() => connection.ExecuteReader(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType));
    }

    public static ReaderResult ReaderAsResult(
        this IDbConnection connection,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        CancellationToken cancellationToken = default)
    {
        var query = builder.Build();
        if (query.IsError)
            return query.UnwrapError();

        var (sql, param) = query.Unwrap();
        return Result.Try(() => connection.ExecuteReader(
            sql,
            param,
            transaction,
            commandTimeout,
            commandType));
    }

    public static ReaderResult ReaderAsResult(
        this IDbConnection connection,
        CommandDefinition command)
    {
        return Result.Try(() => connection.ExecuteReader(command));
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
    public static IDataReader RetryReader(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return retryPolicy.Execute(() => cnn.ExecuteReader(sql, param, transaction, commandTimeout, commandType));
    }

    public static IDataReader RetryReader(
        this IDbConnection cnn,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return retryPolicy.Execute(() => cnn.Reader(builder, transaction, commandTimeout, commandType));
    }

    /// <summary>
    /// Execute parameterized SQL.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="command">The command to execute on this connection.</param>
    /// <param name="retryPolicy">The retry policy.</param>
    /// <returns>The data reader.</returns>
    public static IDataReader RetryReader(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return retryPolicy.Execute(() => cnn.ExecuteReader(command));
    }

    public static ReaderResult RetryReaderAsResult(
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
            var dr = retryPolicy.Execute(() => cnn.Reader(builder, transaction, commandTimeout, commandType));
            return ReaderResult.Ok(dr);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static ReaderResult RetryReaderAsResult(
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
            var dr = retryPolicy.Execute(() => cnn.ExecuteReader(sql, param, transaction, commandTimeout, commandType));
            return ReaderResult.Ok(dr);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static ReaderResult RetryReaderAsResult(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        try
        {
            var dr = retryPolicy.Execute(() => cnn.ExecuteReader(command));
            return ReaderResult.Ok(dr);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}