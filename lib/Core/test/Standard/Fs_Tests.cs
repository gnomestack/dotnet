namespace Tests;

public partial class Fs_Tests
{
    [IntegrationTest]
    public void Verify_Attr(IAssert assert)
    {
        var gnomeStackDir = CreateTestDir();
        var textFile = Path.Combine(gnomeStackDir, "alpha", "test.txt");
        var attrs = Fs.Attr(gnomeStackDir);
        assert.True(attrs.HasFlag(FileAttributes.Directory));

        attrs = Fs.Attr(textFile);
        assert.False(attrs.HasFlag(FileAttributes.Directory));
    }

    [IntegrationTest]
    public void Verify_Stat(IAssert assert)
    {
        var gnomeStackDir = CreateTestDir();
        var fsi = Fs.Stat(gnomeStackDir);
        assert.True(fsi is DirectoryInfo);
    }

    [IntegrationTest]
    public void Verify_IsDirectory(IAssert assert)
    {
        var gnomeStackDir = CreateTestDir();
        var textFile = Path.Combine(gnomeStackDir, "alpha", "test.txt");
        assert.True(Fs.IsDirectory(gnomeStackDir));
        assert.False(Fs.IsDirectory(textFile));
    }

    [IntegrationTest]
    public void Verify_IsFile(IAssert assert)
    {
        var gnomeStackDir = CreateTestDir();
        var textFile = Path.Combine(gnomeStackDir, "alpha", "test.txt");
        assert.False(Fs.IsFile(gnomeStackDir));
        assert.True(Fs.IsFile(textFile));
    }

    [IntegrationTest]
    public void Verify_MakeAndDeleteDirectory(IAssert assert)
    {
        var tmp = Path.GetTempPath();
        var dir = Path.Combine(tmp, "foo", "bar");
        var parent = Path.GetDirectoryName(dir)!;
        if (Fs.DirectoryExists(parent))
            Fs.RemoveDirectory(parent, true);

        assert.False(Fs.DirectoryExists(dir));
        Fs.MakeDirectory(dir);
        assert.True(Fs.DirectoryExists(dir));

        Fs.RemoveDirectory(parent, true);
        assert.False(Fs.DirectoryExists(dir));
    }

    [IntegrationTest]
    public void Verify_CopyDirectory(IAssert assert)
    {
        var tmp = Path.GetTempPath();
        var gnomeStackDir = CreateTestDir();
        var dst = Path.Combine(tmp, "dst");
        if (Fs.DirectoryExists(dst))
            Fs.RemoveDirectory(dst, true);

        try
        {
            assert.False(Fs.DirectoryExists(dst));
            Fs.CopyDirectory(gnomeStackDir, dst, true);

            assert.True(Fs.DirectoryExists(dst));
            assert.True(Fs.DirectoryExists(Path.Combine(dst, "alpha")));
            assert.True(Fs.FileExists(Path.Combine(dst, "alpha", "test.txt")));
            assert.True(Fs.DirectoryExists(Path.Combine(dst, "foo")));
            assert.True(Fs.DirectoryExists(Path.Combine(dst, "foo", "bar")));
        }
        finally
        {
            if (Fs.DirectoryExists(dst))
                Fs.RemoveDirectory(dst, true);
        }
    }

    [IntegrationTest]
    public void Verify_Open(IAssert assert)
    {
        var gnomeStackDir = CreateTestDir();
        var textFile = Path.Combine(gnomeStackDir, "alpha", "test.txt");
        using var stream = Fs.OpenFile(textFile);
        assert.True(stream.CanRead);
        assert.True(stream.Length > 0);
    }

    private static string CreateTestDir()
    {
        var tmp = Path.GetTempPath();
        var dir = Path.Combine(tmp, "GnomeStack", "foo", "bar");
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        dir = Path.Combine(tmp, "GnomeStack", "alpha");
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        var testFile = Path.Combine(dir, "test.txt");
        if (!File.Exists(testFile))
            File.WriteAllText(testFile, "test");

        return Path.Combine(tmp, "GnomeStack");
    }
}