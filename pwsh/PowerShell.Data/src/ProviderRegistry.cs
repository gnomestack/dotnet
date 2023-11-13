using System.Data.Common;

using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;

using Npgsql;

namespace GnomeStack.PowerShell.Data;

public static class ProviderRegistry
{
    private static readonly Dictionary<string, DbProviderFactory> s_factories
        = new(StringComparer.OrdinalIgnoreCase)
        {
            ["mssql"] = SqlClientFactory.Instance,
            ["sqlserver"] = SqlClientFactory.Instance,
            ["Microsoft.Data.SqlServer"] = SqlClientFactory.Instance,
            ["sqlite"] = SqliteFactory.Instance,
            ["Microsoft.Data.Sqlite"] = SqliteFactory.Instance,
            ["pg"] = NpgsqlFactory.Instance,
            ["postgres"] = NpgsqlFactory.Instance,
            ["postgresql"] = NpgsqlFactory.Instance,
            ["npgsql"] = NpgsqlFactory.Instance,
        };

    public static string DefaultProviderName { get; set; } = "mssql";

    public static string? DefaultConnectionString { get; set; }

    public static DbConnection? DefaultConnection { get; set; }

    public static void Register(string name, DbProviderFactory factory)
    {
        s_factories[name] = factory;
    }

    public static DbProviderFactory? Get(string name)
    {
        if (s_factories.TryGetValue(name, out var factory))
            return factory;

        return null;
    }
}