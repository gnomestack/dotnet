using System.Collections;
using System.Security;

namespace GnomeStack.KeePass;

public class KpKey : IEnumerable<KpKeyFragment>
{
    private readonly List<KpKeyFragment> fragments = new();

    public void Add(KpKeyFragment fragment)
    {
        this.fragments.Add(fragment);
    }

    public KpKey AddPassword(string password)
    {
        this.Add(new KpSecretFragment(password));
        return this;
    }

    public KpKey AddPassword(ReadOnlySpan<char> password)
    {
        this.Add(new KpSecretFragment(password));
        return this;
    }

    public KpKey AddPassword(byte[] password)
    {
        this.Add(new KpSecretFragment(password));
        return this;
    }

    public KpKey AddPassword(ReadOnlySpan<byte> password)
    {
        this.Add(new KpSecretFragment(password));
        return this;
    }

    public KpKey AddPassword(SecureString password)
    {
        this.Add(new KpSecretFragment(password));
        return this;
    }

    public KpKey AddKeyFile(string keyFile)
    {
        this.Add(new KpKeyFileFragment(keyFile));
        return this;
    }

    public KpKey AddUserAccount(string? keyLocation)
    {
        this.Add(new KpUserAccountFragment(keyLocation));
        return this;
    }

    public void Clear()
    {
        this.fragments.Clear();
    }

    public IEnumerator<KpKeyFragment> GetEnumerator()
    {
        return this.fragments.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}