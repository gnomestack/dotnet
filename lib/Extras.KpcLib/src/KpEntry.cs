using System.Text;

using KeePassLib;
using KeePassLib.Security;

namespace GnomeStack.Extras.KpcLib;

public class KpEntry
{
    private readonly PwEntry entry;

    private readonly Lazy<List<KpEntry>> histories;

    public KpEntry()
        : this(true, true)
    {
    }

    public KpEntry(bool generateUuid)
        : this(new PwEntry(generateUuid, true))
    {
    }

    public KpEntry(bool generateUuid, bool setTimes)
        : this(new PwEntry(generateUuid, setTimes))
    {
    }

    internal KpEntry(PwEntry entry, KpGroup? group = null)
    {
        this.Parent = group;
        this.entry = entry;
        this.Tags = new KpTags(this.entry.Tags);
        this.histories = new(() =>
        {
            var set = new List<KpEntry>();
            foreach (var n in this.entry.History)
            {
                set.Add(new KpEntry(n));
            }

            return set;
        });
    }

    public KpTags Tags { get; }

    public KpGroup? Parent { get; internal set; }

    public IReadOnlyList<KpEntry> Histories => this.histories.Value;

    public string Title
    {
        get => this.GetValue(PwDefs.TitleField);
        set => this.SetValue(PwDefs.TitleField, value);
    }

    public string UserName
    {
        get => this.GetValue(PwDefs.UserNameField);
        set => this.SetValue(PwDefs.UserNameField, value);
    }

    public string Url
    {
        get => this.GetValue(PwDefs.UrlField);
        set => this.SetValue(PwDefs.UrlField, value);
    }

    public string Notes
    {
        get => this.GetValue(PwDefs.NotesField);
        set => this.SetValue(PwDefs.NotesField, value);
    }

    public string Password
    {
        get => "*********";
        set => this.SetValue(PwDefs.PasswordField, value);
    }

    [CLSCompliant(false)]
    public static implicit operator PwEntry(KpEntry entry)
        => entry.entry;

    public void Delete()
        => this.Parent?.Remove(this);

    public string ReadPassword()
        => this.entry.Strings.ReadSafe(PwDefs.PasswordField);

    public ReadOnlySpan<char> ReadPasswordAsSpan()
        => this.entry.Strings.GetSafe(PwDefs.PasswordField).ReadChars();

    public byte[] GetBinary(string name)
    {
        var pb = this.entry.Binaries.Get(name);
        if (pb is null)
            return Array.Empty<byte>();

        return pb.ReadData();
    }

    public string GetBinaryAsText(string name, Encoding? encoding = null)
    {
        var pb = this.entry.Binaries.Get(name);
        if (pb is null)
            return string.Empty;

        var bytes = pb.ReadData();
        encoding ??= System.Text.Encoding.UTF8;
        return encoding.GetString(bytes);
    }

    public string GetValue(string name)
        => this.entry.Strings.ReadSafe(name);

    public byte[] GetValueAsBytes(string name)
        => this.entry.Strings.GetSafe(name).ReadUtf8();

    public char[] GetValueAsChars(string name)
        => this.entry.Strings.GetSafe(name).ReadChars();

    public KpEntry SetBinary(string name, byte[] value, bool encrypt = true)
    {
        this.entry.Binaries.Set(name, new ProtectedBinary(encrypt, value));
        return this;
    }

    public KpEntry SetValue(string name, string value, bool encrypt = true)
    {
        this.entry.Strings.Set(name, new ProtectedString(encrypt, value));
        return this;
    }

    public KpEntry SetValue(string name, byte[] value, bool encrypt = true)
    {
        this.entry.Strings.Set(name, new ProtectedString(encrypt, value));
        return this;
    }

    public KpEntry SetValue(string name, ReadOnlySpan<byte> value, bool encrypt = true)
    {
        var temp = new byte[value.Length];
        try
        {
            value.CopyTo(temp);
            this.entry.Strings.Set(name, new ProtectedString(encrypt, temp));
        }
        finally
        {
            Array.Clear(temp, 0, temp.Length);
        }

        return this;
    }

    public KpEntry SetValue(string name, Span<byte> value, bool encrypt = true)
    {
        var temp = new byte[value.Length];
        try
        {
            value.CopyTo(temp);
            this.entry.Strings.Set(name, new ProtectedString(encrypt, temp));
        }
        finally
        {
            Array.Clear(temp, 0, temp.Length);
        }

        return this;
    }

    public KpEntry SetValue(string name, ReadOnlySpan<char> value, bool encrypt = true)
    {
        var tmp = value.ToArray();
        var bytes = System.Text.Encoding.UTF8.GetBytes(tmp);
        try
        {
            this.entry.Strings.Set(name, new ProtectedString(encrypt, bytes));
        }
        finally
        {
            Array.Clear(tmp, 0, tmp.Length);
            Array.Clear(bytes, 0, bytes.Length);
        }

        return this;
    }
}