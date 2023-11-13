namespace GnomeStack.Data.Mssql;

public class SqlDbFileSpec
{
    public SqlDbFileSpec(string name, string fileName)
    {
        this.Name = name;
        this.FileName = fileName;
    }

    public string Name { get; set; }

    public string FileName { get; set; }

    public string? FileGroup { get; set; }

    public string? Size { get; set; }

    public string? MaxSize { get; set; }

    public string? FileGrowth { get; set; }
}