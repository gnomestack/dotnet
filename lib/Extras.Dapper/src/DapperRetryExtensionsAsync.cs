using System.Data;
using System.Data.Common;

using Polly;

using static Dapper.SqlMapper;

namespace Dapper;

#pragma warning disable S4136
public static class DapperRetryExtensionsAsync
{
    /// <summary>
    /// Execute a query asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>Note: each row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;.</remarks>
    /// <returns>A sequence of dynamic objects with properties matching the columns.</returns>
    public static async Task<IEnumerable<dynamic>> RetryQueryAsync(
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
            async (_) => await cnn.QueryAsync(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a query asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="command">The command used to query on this connection.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>Note: each row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;.</remarks>
    /// <returns>A sequence of dynamic objects with properties matching the columns.</returns>
    public static async Task<IEnumerable<dynamic>> RetryQueryAsync(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QueryAsync(
                    command)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="command">The command used to query on this connection.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>Note: the row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;.</remarks>
    /// <returns>A dynamic object with properties matching the columns.</returns>
    public static async Task<dynamic> RetryQueryFirstAsync(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QueryFirstAsync(
                    command)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="command">The command used to query on this connection.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>Note: the row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;.</remarks>
    /// <returns>A dynamic object with properties matching the columns.</returns>
    public static async Task<dynamic> RetryQueryFirstOrDefaultAsync(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QueryFirstOrDefaultAsync(
                    command)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="command">The command used to query on this connection.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>Note: the row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;.</remarks>
    /// <returns>A dynamic object with properties matching the columns.</returns>
    public static async Task<dynamic> RetryQuerySingleAsync(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QuerySingleAsync(
                    command)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="command">The command used to query on this connection.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>Note: the row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;.</remarks>
    /// <returns>A dynamic object with properties matching the columns.</returns>
    public static async Task<dynamic?> RetryQuerySingleOrDefaultAsync(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QuerySingleOrDefaultAsync(command).ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a query asynchronously using Task.
    /// </summary>
    /// <typeparam name="T">The type of results to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A sequence of data of <typeparamref name="T"/>; if a basic type (int, string, etc) is queried then the data from the first column in assumed, otherwise an instance is
    /// created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
    /// </returns>
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
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <typeparam name="T">The type of result to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A single instance of <typeparamref name="T"/>; if a basic type (int, string, etc) is queried then the data from the first column in assumed.</returns>
    public static async Task<T> RetryQueryFirstAsync<T>(
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
            async (_) => await cnn.QueryFirstAsync<T>(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <typeparam name="T">The type of result to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A single instance of <typeparamref name="T"/>; if a basic type (int, string, etc) is queried then the data from the first column in assumed.</returns>
    public static async Task<T> RetryQueryFirstOrDefaultAsync<T>(
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
            async (_) => await cnn.QueryFirstOrDefaultAsync<T>(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <typeparam name="T">The type of result to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A single instance of <typeparamref name="T"/>; if a basic type (int, string, etc) is queried then the data from the first column in assumed.</returns>
    public static async Task<T> RetryQuerySingleAsync<T>(
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
            async (_) => await cnn.QuerySingleAsync<T>(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="retryPolicy"> The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A single instance of <typeparamref name="T"/>; if a basic
    /// type (int, string, etc) is queried then the data from the first column
    /// in assumed.</returns>
    public static async Task<T> RetryQuerySingleOrDefaultAsync<T>(
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
            async (_) => await cnn.QuerySingleOrDefaultAsync<T>(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns> A dynamic object with properties matching the columns.</returns>
    public static async Task<dynamic> RetryQueryFirstAsync(
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
            async (_) => await cnn.QueryFirstAsync(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns> A dynamic object with properties matching the columns.</returns>
    public static async Task<dynamic> RetryQueryFirstOrDefaultAsync(
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
            async (_) => await cnn.QueryFirstOrDefaultAsync(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns> A dynamic object with properties matching the columns.</returns>
    public static async Task<dynamic> RetryQuerySingleAsync(
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
            async (_) => await cnn.QuerySingleAsync(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns> A dynamic object with properties matching the columns.</returns>
    public static async Task<dynamic> RetryQuerySingleOrDefaultAsync(
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
            async (_) => await cnn.QuerySingleOrDefaultAsync(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a query asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="type">The type to return.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="retryPolicy"> The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <c>null</c>.</exception>
    /// <returns> A sequence of data of <paramref name="type"/>; if a basic
    /// type (int, string, etc) is queried then the data from the first
    /// column in assumed, otherwise an instance is created per row, and a
    /// direct column-name===member-name mapping is assumed (case insensitive).</returns>
    public static async Task<IEnumerable<object>> RetryQueryAsync(
        this IDbConnection cnn,
        Type type,
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
            async (_) => await cnn.QueryAsync(
                    type,
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="type">The type to return.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="retryPolicy"> The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <c>null</c>.</exception>
    /// <returns> A dynamic object with properties matching the columns.</returns>
    public static async Task<object> RetryQueryFirstAsync(
        this IDbConnection cnn,
        Type type,
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
            async (_) => await cnn.QueryFirstAsync(
                    type,
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="type">The type to return.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="retryPolicy"> The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <c>null</c>.</exception>
    /// <returns> A dynamic object with properties matching the columns.</returns>
    public static async Task<object> RetryQueryFirstOrDefaultAsync(
        this IDbConnection cnn,
        Type type,
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
            async (_) => await cnn.QueryFirstOrDefaultAsync(
                    type,
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="type">The type to return.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="retryPolicy"> The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <c>null</c>.</exception>
    /// <returns> A dynamic object with properties matching the columns.</returns>
    public static async Task<object> RetryQuerySingleAsync(
        this IDbConnection cnn,
        Type type,
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
            async (_) => await cnn.QuerySingleAsync(
                    type,
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="type">The type to return.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="retryPolicy"> The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <c>null</c>.</exception>
    /// <returns> A dynamic object with properties matching the columns.</returns>
    public static async Task<object> RetryQuerySingleOrDefaultAsync(
        this IDbConnection cnn,
        Type type,
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
            async (_) => await cnn.QuerySingleOrDefaultAsync(
                    type,
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a query asynchronously using Task.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="command">The command used to query on this connection.</param>
    /// <param name="retryPolicy"> The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A sequence of data of <typeparamref name="T"/>; if a basic type (int, string, etc) is queried then the data from the first column in assumed, otherwise an instance is
    /// created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
    /// </returns>
    public static async Task<IEnumerable<T>> RetryQueryAsync<T>(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QueryAsync<T>(
                    command)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a query asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="type">The type to return.</param>
    /// <param name="command">The command used to query on this connection.</param>
    /// <param name="retryPolicy"> The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns> A sequence of data of <paramref name="type"/>; if a basic
    /// type (int, string, etc) is queried then the data from the first column
    /// in assumed, otherwise an instance is created per row, and a direct
    /// column-name===member-name mapping is assumed (case insensitive).</returns>
    public static async Task<IEnumerable<object>> RetryQueryAsync(
        this IDbConnection cnn,
        Type type,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QueryAsync(type, command).ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="type">The type to return.</param>
    /// <param name="command">The command used to query on this connection.</param>
    /// <param name="retryPolicy"> The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns> A dynamic object with properties matching the columns.</returns>
    public static async Task<object> RetryQueryFirstAsync(
        this IDbConnection cnn,
        Type type,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QueryFirstAsync(type, command).ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="command">The command used to query on this connection.</param>
    /// <param name="retryPolicy"> The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns> A single instance of <typeparamref name="T"/>; if a basic
    /// type (int, string, etc) is queried then the data from the first column
    /// in assumed.
    /// </returns>
    public static async Task<T> RetryQueryFirstAsync<T>(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QueryFirstAsync<T>(command).ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="type">The type to return.</param>
    /// <param name="command">The command used to query on this connection.</param>
    /// <param name="retryPolicy"> The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns> A dynamic object with properties matching the columns.</returns>
    public static async Task<object> RetryQueryFirstOrDefaultAsync(
        this IDbConnection cnn,
        Type type,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QueryFirstOrDefaultAsync(type, command).ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="command">The command used to query on this connection.</param>
    /// <param name="retryPolicy"> The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns> A single instance of <typeparamref name="T"/>; if a basic
    /// type (int, string, etc) is queried then the data from the first column
    /// in assumed.</returns>
    public static async Task<T> RetryQueryFirstOrDefaultAsync<T>(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QueryFirstOrDefaultAsync<T>(command).ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="type">The type to return.</param>
    /// <param name="command">The command used to query on this connection.</param>
    /// <param name="retryPolicy"> The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A dynamic object with properties matching the columns.</returns>
    public static async Task<object> RetryQuerySingleAsync(
        this IDbConnection cnn,
        Type type,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QuerySingleAsync(type, command).ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="command">The command used to query on this connection.</param>
    /// <param name="retryPolicy"> The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A single instance of <typeparamref name="T"/>; if a basic
    /// type (int, string, etc) is queried then the data from the first column
    /// in assumed.</returns>
    public static async Task<T> RetryQuerySingleAsync<T>(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QuerySingleAsync<T>(command).ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="type">The type to return.</param>
    /// <param name="command">The command used to query on this connection.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A dynamic object with properties matching the columns.</returns>
    public static async Task<object> RetryQuerySingleOrDefaultAsync(
        this IDbConnection cnn,
        Type type,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QuerySingleOrDefaultAsync(type, command).ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="command">The command used to query on this connection.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A single instance of <typeparamref name="T"/>; if a basic
    /// type (int, string, etc) is queried then the data from the first column
    /// in assumed.</returns>
    public static async Task<T> RetryQuerySingleOrDefaultAsync<T>(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QuerySingleOrDefaultAsync<T>(command).ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a command asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of rows affected.</returns>
    public static async Task<int> RetryExecuteAsync(
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
            async (_) => await cnn.ExecuteAsync(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a command asynchronously using Task.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="command">The command to execute on this connection.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of rows affected.</returns>
    public static async Task<int> RetryExecuteAsync(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.ExecuteAsync(command).ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 2 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static async Task<IEnumerable<TReturn>> RetryQueryAsync<TFirst, TSecond, TReturn>(
        this IDbConnection cnn,
        string sql,
        Func<TFirst, TSecond, TReturn> map,
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
            async (_) => await cnn.QueryAsync<TFirst, TSecond, TReturn>(
                    sql,
                    map,
                    param,
                    transaction,
                    buffered,
                    splitOn,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 2 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="command">The command to execute.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static async Task<IEnumerable<TReturn>> RetryQueryAsync<TFirst, TSecond, TReturn>(
        this IDbConnection cnn,
        CommandDefinition command,
        Func<TFirst, TSecond, TReturn> map,
        string splitOn = "Id",
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QueryAsync<TFirst, TSecond, TReturn>(
                    command,
                    map,
                    splitOn)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 3 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static async Task<IEnumerable<TReturn>> RetryQueryAsync<TFirst, TSecond, TThird, TReturn>(
        this IDbConnection cnn,
        string sql,
        Func<TFirst, TSecond, TThird, TReturn> map,
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
            async (_) => await cnn.QueryAsync<TFirst, TSecond, TThird, TReturn>(
                    sql,
                    map,
                    param,
                    transaction,
                    buffered,
                    splitOn,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 3 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="command">The command to execute.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static async Task<IEnumerable<TReturn>> RetryQueryAsync<TFirst, TSecond, TThird, TReturn>(
        this IDbConnection cnn,
        CommandDefinition command,
        Func<TFirst, TSecond, TThird, TReturn> map,
        string splitOn = "Id",
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QueryAsync<TFirst, TSecond, TThird, TReturn>(
                    command,
                    map,
                    splitOn)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 4 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static async Task<IEnumerable<TReturn>> RetryQueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(
        this IDbConnection cnn,
        string sql,
        Func<TFirst, TSecond, TThird, TFourth, TReturn> map,
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
            async (_) => await cnn.QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(
                    sql,
                    map,
                    param,
                    transaction,
                    buffered,
                    splitOn,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 4 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="command">The command to execute.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static async Task<IEnumerable<TReturn>> RetryQueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(
        this IDbConnection cnn,
        CommandDefinition command,
        Func<TFirst, TSecond, TThird, TFourth, TReturn> map,
        string splitOn = "Id",
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(
                    command,
                    map,
                    splitOn)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 5 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static async Task<IEnumerable<TReturn>> RetryQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
        this IDbConnection cnn,
        string sql,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map,
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
            async (_) => await cnn.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
                    sql,
                    map,
                    param,
                    transaction,
                    buffered,
                    splitOn,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 5 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="command">The command to execute.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static async Task<IEnumerable<TReturn>> RetryQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
        this IDbConnection cnn,
        CommandDefinition command,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map,
        string splitOn = "Id",
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
                    command,
                    map,
                    splitOn)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 6 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
    /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static async Task<IEnumerable<TReturn>> RetryQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
        this IDbConnection cnn,
        string sql,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map,
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
            async (_) => await cnn.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
                    sql,
                    map,
                    param,
                    transaction,
                    buffered,
                    splitOn,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 6 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
    /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="command">The command to execute.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static async Task<IEnumerable<TReturn>> RetryQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
        this IDbConnection cnn,
        CommandDefinition command,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map,
        string splitOn = "Id",
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
                    command,
                    map,
                    splitOn)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 7 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
    /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
    /// <typeparam name="TSeventh">The seventh type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static async Task<IEnumerable<TReturn>>
        RetryQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
            this IDbConnection cnn,
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
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
            async (_) => await cnn.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
                    sql,
                    map,
                    param,
                    transaction,
                    buffered,
                    splitOn,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 7 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
    /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
    /// <typeparam name="TSeventh">The seventh type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="command">The command to execute.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static async Task<IEnumerable<TReturn>>
        RetryQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
            this IDbConnection cnn,
            CommandDefinition command,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
            string splitOn = "Id",
            ResiliencePipeline? retryPolicy = null,
            CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
                    command,
                    map,
                    splitOn)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Perform an asynchronous multi-mapping query with an arbitrary number of input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="types">Array of types in the recordset.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static async Task<IEnumerable<TReturn>> RetryQueryAsync<TReturn>(
        this IDbConnection cnn,
        string sql,
        Type[] types,
        Func<object[], TReturn> map,
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
            async (_) => await cnn.
                QueryAsync<TReturn>(
                    sql,
                    types,
                    map,
                    param,
                    transaction,
                    buffered,
                    splitOn,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a command that returns multiple result sets, and access each in turn.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A GridReader that can be used to read the results of the query.</returns>
    public static async Task<GridReader> RetryQueryMultipleAsync(
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
            async (_) => await cnn.QueryMultipleAsync(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute a command that returns multiple result sets, and access each in turn.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="command">The command to execute for this query.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A GridReader that can be used to read the results of the query.</returns>
    public static async Task<GridReader> RetryQueryMultipleAsync(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.QueryMultipleAsync(command).ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute parameterized SQL and return an <see cref="IDataReader"/>.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An <see cref="IDataReader"/> that can be used to iterate over the results of the SQL query.</returns>
    /// <remarks>
    /// This is typically used when the results of a query are not processed by Dapper, for example, used to fill a <see cref="DataTable"/>
    /// or <see cref="T:DataSet"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// DataTable table = new DataTable("MyTable");
    /// using (var reader = ExecuteReader(cnn, sql, param))
    /// {
    ///     table.Load(reader);
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public static async Task<IDataReader> RetryExecuteReaderAsync(
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
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute parameterized SQL and return a <see cref="DbDataReader"/>.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns> A <see cref="DbDataReader"/> that can be used to iterate over
    /// the results of the SQL query.
    /// </returns>
    public static async Task<DbDataReader> RetryExecuteReaderAsync(
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
            async (_) => await cnn.ExecuteReaderAsync(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute parameterized SQL and return an <see cref="IDataReader"/>.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="command">The command to execute.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An <see cref="IDataReader"/> that can be used to iterate over the results of the SQL query.</returns>
    /// <remarks>
    /// This is typically used when the results of a query are not processed by Dapper, for example, used to fill a <see cref="DataTable"/>
    /// or <see cref="T:DataSet"/>.
    /// </remarks>
    public static async Task<IDataReader> RetryExecuteReaderAsync(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.ExecuteReaderAsync(command).ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute parameterized SQL and return a <see cref="DbDataReader"/>.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="command">The command to execute.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns> A <see cref="DbDataReader"/> that can be used to iterate over.</returns>
    public static async Task<DbDataReader> RetryExecuteReaderAsync(
        this DbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.ExecuteReaderAsync(command).ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute parameterized SQL and return an <see cref="IDataReader"/>.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="command">The command to execute.</param>
    /// <param name="commandBehavior">The <see cref="CommandBehavior"/> flags for this reader.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An <see cref="IDataReader"/> that can be used to iterate over the results of the SQL query.</returns>
    /// <remarks>
    /// This is typically used when the results of a query are not processed by Dapper, for example, used to fill a <see cref="DataTable"/>
    /// or <see cref="T:DataSet"/>.
    /// </remarks>
    public static async Task<IDataReader> RetryExecuteReaderAsync(
        this IDbConnection cnn,
        CommandDefinition command,
        CommandBehavior commandBehavior,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.ExecuteReaderAsync(command, commandBehavior).ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute parameterized SQL and return a <see cref="DbDataReader"/>.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="command">The command to execute.</param>
    /// <param name="commandBehavior">The <see cref="CommandBehavior"/> flags for this reader.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns> A <see cref="DbDataReader"/> that can be used to iterate over.</returns>
    public static async Task<DbDataReader> RetryExecuteReaderAsync(
        this DbConnection cnn,
        CommandDefinition command,
        CommandBehavior commandBehavior,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;
        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.ExecuteReaderAsync(command, commandBehavior).ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute parameterized SQL that selects a single value.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch.</param>
    /// <param name="retryPolicy">The retry policy.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The first cell returned, as <see cref="object"/>.</returns>
    public static async Task<object> RetryExecuteScalarAsync(
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
            async (_) => await cnn.ExecuteScalarAsync(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute parameterized SQL that selects a single value.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch.</param>
    /// <param name="retryPolicy">The retry policy.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The first cell returned, as <typeparamref name="T"/>.</returns>
    public static async Task<T> RetryExecuteScalarAsync<T>(
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
            async (_) => await cnn.ExecuteScalarAsync<T>(
                    sql,
                    param,
                    transaction,
                    commandTimeout,
                    commandType)
                .ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute parameterized SQL that selects a single value.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="command">The command to execute.</param>
    /// <param name="retryPolicy">The retry policy.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The first cell selected as <see cref="object"/>.</returns>
    public static async Task<object> RetryExecuteScalarAsync(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;

        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.ExecuteScalarAsync(command).ConfigureAwait(false),
            cancellationToken);
    }

    /// <summary>
    /// Execute parameterized SQL that selects a single value.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="command">The command to execute.</param>
    /// <param name="retryPolicy">The retry policy to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The first cell selected as <typeparamref name="T"/>.</returns>
    public static async Task<T> RetryExecuteScalarAsync<T>(
        this IDbConnection cnn,
        CommandDefinition command,
        ResiliencePipeline? retryPolicy = null,
        CancellationToken cancellationToken = default)
    {
        retryPolicy ??= SqlRetryPolicy.Default;

        return await retryPolicy.ExecuteAsync(
            async (_) => await cnn.ExecuteScalarAsync<T>(command).ConfigureAwait(false),
            cancellationToken);
    }
}