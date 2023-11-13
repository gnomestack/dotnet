using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

using Dapper;

using GnomeStack.Functional;

using Polly;

using ExecuteResult = GnomeStack.Functional.Result<int, System.Exception>;

namespace GnomeStack.Data;

[SuppressMessage("ReSharper", "SuggestVarOrType_DeconstructionDeclarations")]
public static partial class GsDapperExtensions
{
    public static int Exec(
        this IDbConnection connection,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        return connection.Execute(sql, param, transaction, commandTimeout, commandType);
    }

    public static int Exec(
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
        return connection.Execute(sql, param, transaction, commandTimeout, commandType);
    }

    public static ExecuteResult ExecAsResult(
        this IDbConnection connection,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        CancellationToken cancellationToken = default)
    {
        return Result.Try(() =>
        {
            return connection.Execute(
                sql,
                param,
                transaction,
                commandTimeout,
                commandType);
        });
    }

    public static ExecuteResult ExecAsResult(
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
        return Result.Try(() =>
        {
            return connection.Execute(
                sql,
                param,
                transaction,
                commandTimeout,
                commandType);
        });
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
    public static int RetryExec(
        this IDbConnection cnn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return retryPolicy.Execute(() => cnn.Execute(sql, param, transaction, commandTimeout, commandType));
    }

    public static int RetryExec(
        this IDbConnection cnn,
        ISqlStatementBuilder builder,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        ResiliencePipeline? retryPolicy = null)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return retryPolicy.Execute(() => cnn.Exec(builder, transaction, commandTimeout, commandType));
    }

    /// <summary>
    /// Execute parameterized SQL.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="command">The command to execute on this connection.</param>
    /// <param name="retryPolicy">The retry policy.</param>
    /// <returns>The number of rows affected.</returns>
    public static int RetryExec(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return retryPolicy.Execute(() => cnn.Execute(command));
    }

    public static ExecuteResult RetryExecAsResult(
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
            return retryPolicy.Execute(() => cnn.Exec(builder, transaction, commandTimeout, commandType));
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static ExecuteResult RetryExecAsResult(
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
            return retryPolicy.Execute(() => cnn.Execute(sql, param, transaction, commandTimeout, commandType));
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}