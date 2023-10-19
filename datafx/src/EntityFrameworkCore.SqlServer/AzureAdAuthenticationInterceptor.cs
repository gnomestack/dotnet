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
        var ctx = new TokenRequestContext(new[] { "https://database.windows.net//.default" });
        sqlConnection.AccessToken = this.credential.GetToken(ctx, CancellationToken.None).Token;

        return result;
    }

    public override async ValueTask<InterceptionResult> ConnectionOpeningAsync(
        DbConnection connection,
        ConnectionEventData eventData,
        InterceptionResult result,
        CancellationToken cancellationToken = default)
    {
        var sqlConnection = (SqlConnection)connection;

        var ctx = new TokenRequestContext(new[] { "https://database.windows.net//.default" });
        var r = await this.credential.GetTokenAsync(ctx, cancellationToken)
            .ConfigureAwait(false);
        sqlConnection.AccessToken = r.Token;

        return result;
    }
}
#endif