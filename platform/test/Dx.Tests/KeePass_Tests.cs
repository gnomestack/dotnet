using GnomeStack.Dx.KpcLib;

namespace Test;

public class KeePass_Tests
{
    [UnitTest]
    public void WorkWithEntries()
    {
        var db = new KpDatabase();
        var g1 = new KpGroup("g1", db.Root)
        {
            new KpEntry()
            {
                Title = "e1",
                UserName = "u1",
                Password = "p1",
            },

            new KpGroup("g2")
            {
                new KpEntry()
                {
                    Title = "e2",
                    UserName = "u2",
                    Password = "p2",
                },
            },
        };
        db.Root.Add(g1);

        var e1 = db.FindEntry("g1/e1");

        Assert.True(e1.IsSome);
        var e1v = e1.Unwrap();
        Assert.Equal("e1", e1v.Title);
        Assert.Equal("u1", e1v.UserName);
        Assert.Equal("p1", e1v.ReadPassword());

        var e2 = db.FindEntry("g1/g2/e2");
        Assert.True(e2.IsSome);
        var e2v = e2.Unwrap();
        Assert.Equal("e2", e2v.Title);
        Assert.Equal("u2", e2v.UserName);
        Assert.Equal("p2", e2v.ReadPassword());

        var e3 = db.FindEntry("g1/g2/e3");
        Assert.True(e3.IsNone);

        var e4 = db.GetOrCreateEntry("g1/g3/e4");
        Assert.Equal("e4", e4.Title);

        var g3 = db.FindGroup("g1/g3");
        Assert.True(g3.IsSome);

        var groups = db.EnumerateGroupNames().ToArray();
        Assert.Equal(4, groups.Length);
        Assert.Equal("root", groups[0]);
        Assert.Equal("root/g1", groups[1]);
        Assert.Equal("root/g1/g2", groups[2]);
        Assert.Equal("root/g1/g3", groups[3]);

        var entries = db.EnumerateEntryNames().ToArray();
        Assert.Equal(3, entries.Length);
        Assert.Equal("root/g1/e1", entries[0]);
        Assert.Equal("root/g1/g2/e2", entries[1]);
        Assert.Equal("root/g1/g3/e4", entries[2]);

        e4.Delete();

        entries = db.EnumerateEntryNames().ToArray();
        Assert.Equal(2, entries.Length);
        Assert.Equal("root/g1/e1", entries[0]);
        Assert.Equal("root/g1/g2/e2", entries[1]);
    }

    [IntegrationTest]
    public void CreateSaveDeleteDatabase()
    {
        var path = Path.Combine(Path.GetTempPath(), "test.kdbx");
        if (File.Exists(path))
            File.Delete(path);

        var result = KpDatabase.Create(path, "test", "weakest pw ever");
        Assert.False(result.IsError);
        Assert.True(File.Exists(path));

        var db = result.Unwrap();
        Assert.Equal("test", db.Root.Name);
        var g1 = new KpGroup("g1", db.Root)
        {
            new KpEntry()
            {
                Title = "e1",
                UserName = "u1",
                Password = "p1",
            },

            new KpGroup("g2")
            {
                new KpEntry()
                {
                    Title = "e2",
                    UserName = "u2",
                    Password = "p2",
                },
            },
        };

        db.Save();
        var r2 = KpDatabase.Open(path, "weakest pw ever");
        Assert.False(r2.IsError);
        var db2 = r2.Unwrap();
        Assert.Equal("test", db2.Root.Name);
        var e1 = db2.FindEntry("g1/e1");
        Assert.True(e1.IsSome);
        var e1v = e1.Unwrap();
        Assert.Equal("e1", e1v.Title);
        Assert.Equal("u1", e1v.UserName);
        Assert.Equal("p1", e1v.ReadPassword());

        File.Delete(path);
    }
}