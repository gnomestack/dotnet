using GnomeStack.Data.Management;
using GnomeStack.Functional;
using GnomeStack.Text;

namespace GnomeStack.Data.SqlServer.Management;

public class CreateMssqlDatabase : CreateDatabase
{
    public string? ElasticPoolName { get; set; }

    public string? ElasticPoolEdition { get; set; }

    public List<SqlDbFileSpec> DataFiles { get; set; } = new();

    public List<SqlDbFileSpec> LogFiles { get; set; } = new();

    public AzDbServiceObjective? ServiceObjective { get; set; }

    public AzSqlBackupRedundancy? BackupRedundancy { get; set; }

    public string? CopyFromDatabase { get; set; }

    public static implicit operator string(CreateMssqlDatabase cmd)
        => cmd.Build().Unwrap();

    public override Result<string, Exception> Build()
    {
        if (!Validate.Identifier(this.Name.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid database name {this.Name}");

        if (!this.CopyFromDatabase.IsNullOrWhiteSpace() && !Validate.Identifier(this.CopyFromDatabase.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid database name {this.CopyFromDatabase}");

        if (!this.ElasticPoolName.IsNullOrWhiteSpace() && !Validate.ElasticPoolName(this.ElasticPoolName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid elastic pool name {this.ElasticPoolName}");

        var sb = StringBuilderCache.Acquire();
        sb.Append("CREATE DATABASE [")
            .Append(this.Name)
            .Append(']');

        if (!this.Collation.IsNullOrWhiteSpace())
        {
            sb.Append(" COLLATE ")
                .Append(this.Collation);
        }

        if (this.DataFiles.Count > 0)
        {
            void AppendFileSpec(List<SqlDbFileSpec> sqlDbFileSpecs)
            {
                foreach (var file in sqlDbFileSpecs)
                {
                    sb.AppendLine();
                    sb.Append("    (NAME = N'")
                        .Append(file.Name)
                        .Append("',")
                        .AppendLine()
                        .Append("    FILENAME = N'")
                        .Append(file.FileName)
                        .Append("\'");

                    if (!file.Size.IsNullOrWhiteSpace())
                    {
                        sb.AppendLine(",");
                        sb.Append("    SIZE = ")
                            .Append(file.Size);
                    }

                    if (!file.MaxSize.IsNullOrWhiteSpace())
                    {
                        sb.AppendLine(",")
                            .Append("    MAXSIZE = ")
                            .Append(file.MaxSize);
                    }

                    if (!file.FileGrowth.IsNullOrWhiteSpace())
                    {
                        sb.AppendLine(",")
                            .Append("    FILEGROWTH = ")
                            .Append(file.FileGrowth);
                    }

                    sb.Append(")");
                }
            }

            var fileGroups = new Dictionary<string, List<SqlDbFileSpec>>();

            foreach (var file in this.DataFiles)
            {
                if (file.FileGroup.IsNullOrWhiteSpace())
                    file.FileGroup = "PRIMARY";

                if (!fileGroups.ContainsKey(file.FileGroup))
                    fileGroups.Add(file.FileGroup, new List<SqlDbFileSpec>());

                fileGroups[file.FileGroup].Add(file);
            }

            if (fileGroups.TryGetValue("PRIMARY", out List<SqlDbFileSpec>? list))
            {
                sb.Append(" ON PRIMARY ");
                AppendFileSpec(list);
            }

            foreach (var kvp in fileGroups)
            {
                if (kvp.Key == "PRIMARY")
                    continue;

                sb.AppendLine(",");
                sb.Append("FILEGROUP ")
                    .Append(kvp.Key)
                    .AppendLine();
                AppendFileSpec(kvp.Value);
            }

            if (this.LogFiles.Count > 0)
            {
                sb.AppendLine(",");
                sb.Append("LOG ON ");
                AppendFileSpec(this.LogFiles);
            }

            sb.AppendLine(";");
            return StringBuilderCache.GetStringAndRelease(sb);
        }

        if (!this.CopyFromDatabase.IsNullOrWhiteSpace())
        {
            sb.Append(" AS COPY OF ")
                .Append(this.CopyFromDatabase);
        }

        var hasPoolName = false;
        if (!this.ElasticPoolName.IsNullOrWhiteSpace())
        {
            hasPoolName = true;
            sb.Append(" (SERVICE_OBJECTIVE = ELASTIC_POOL( name = ")
                .Append(this.ElasticPoolName)
                .Append(" ) )");
        }

        if (!hasPoolName && this.ServiceObjective is not null)
        {
            sb.Append(" (SERVICE_OBJECTIVE = ")
                .Append(this.ServiceObjective.ToString())
                .Append(" )");
        }

        if (this.BackupRedundancy != null)
        {
            sb.Append(" WITH_BACKUP_REDUNDANCY = ")
                .Append(this.BackupRedundancy.Value.ToString().ToUpperInvariant());
        }

        return StringBuilderCache.GetStringAndRelease(sb);
    }
}