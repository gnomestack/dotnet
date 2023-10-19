using Azure.Core;

namespace GnomeStack.Data.SqlServer;

public static class ConnectionExtensions
{
    [CLSCompliant(false)]
    public static void AddAccessToken(
        this Microsoft.Data.SqlClient.SqlConnection connection,
        TokenCredential credential,
        string scope = "https://database.windows.net/.default",
        CancellationToken cancellationToken = default)
    {
        if (connection == null)
            throw new ArgumentNullException(nameof(connection));

        if (credential == null)
            throw new ArgumentNullException(nameof(credential));

        var token = credential.GetToken(new TokenRequestContext(new[] { scope }), cancellationToken);
        connection.AccessToken = token.Token;
    }

    [CLSCompliant(false)]
    public static Task AddAccessTokenAsync(
        this Microsoft.Data.SqlClient.SqlConnection connection,
        TokenCredential credential,
        string scope = "https://database.windows.net/.default",
        CancellationToken cancellationToken = default)
    {
        if (connection == null)
            throw new ArgumentNullException(nameof(connection));

        if (credential == null)
            throw new ArgumentNullException(nameof(credential));

        return connection.AssignAccessTokeAsync(credential, scope, cancellationToken);
    }

    private static async Task AssignAccessTokeAsync(
        this Microsoft.Data.SqlClient.SqlConnection connection,
        TokenCredential credential,
        string scope = "https://database.windows.net/.default",
        CancellationToken cancellationToken = default)
    {
        var token = await credential.GetTokenAsync(new TokenRequestContext(new[] { scope }), cancellationToken);
        connection.AccessToken = token.Token;
    }
}