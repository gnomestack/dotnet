#if NET6_0_OR_GREATER

using System.Data.Common;

using Azure.Core;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GnomeStack.EntityFramework.SqlServer;

public class AzureAdAuthenticationInterceptor : DbConnectionInterceptor
{
    private readonly TokenCredential credential;

    public AzureAdAuthenticationInterceptor(TokenCredential credential)
    {
        this.credential = credential;
    }

    public override InterceptionResult ConnectionOpening(
        DbConnection connection,
        ConnectionEventData eventData,
        InterceptionResult result)
    {
        var sqlConnection = (SqlConnection)connection;
        sqlConnection.SetAccessToken(this.credential);

        return result;
    }

    public override async ValueTask<InterceptionResult> ConnectionOpeningAsync(
        DbConnection connection,
        ConnectionEventData eventData,
        InterceptionResult result,
        CancellationToken cancellationToken = default)
    {
        var sqlConnection = (SqlConnection)connection;

        await sqlConnection.SetAccessTokenAsync(this.credential, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return result;
    }
}
#endif