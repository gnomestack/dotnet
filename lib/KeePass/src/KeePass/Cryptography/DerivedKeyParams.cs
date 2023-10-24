using GnomeStack.KeePass.Collections;

namespace GnomeStack.KeePass.Cryptography;

public class DerivedKeyParams : KpMap
{
    private const string UuidName = "UUID";

    public DerivedKeyParams(Kpid id)
    {
        this.Uuid = id;
        this.SetByteArray(UuidName, id);
    }

    internal DerivedKeyParams()
    {
        this.Uuid = Kpid.Empty;
    }

    public static DerivedKeyParams Empty { get; } = new();

    public Kpid Uuid { get; }

    public bool IsEmpty => this.Uuid == Kpid.Empty;

    public static new DerivedKeyParams FromBytes(byte[] bytes)
    {
        var map = KpMap.FromBytes(bytes);
        var uuid = map.GetByteArray(UuidName);
        return new DerivedKeyParams(uuid);
    }
}