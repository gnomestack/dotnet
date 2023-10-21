using System.Buffers;
using System.Buffers.Binary;

using static System.Buffers.Binary.BinaryPrimitives;

namespace GnomeStack.KeePass.Collections;

public class KpMap
{
    private const ushort Version = 0x0100;
    private const ushort Critical = 0xFF00;
    private const ushort Info = 0x00FF;

    private readonly Dictionary<string, object> values = new(StringComparer.OrdinalIgnoreCase);

    public int Count => this.values.Count;

    public object? this[string key]
    {
        get
        {
            if (this.values.TryGetValue(key, out var value))
                return value;
            return null;
        }

        set
        {
            if (value is null)
                this.values.Remove(key);
            else
                this.values[key] = value;
        }
    }

    public static KpMap FromBytes(byte[] bytes)
    {
        var map = new KpMap();
        using var ms = new MemoryStream(bytes);
        using var br = new BinaryReader(ms);

        // keepass is le
        var version = ReadUInt16LittleEndian(br.ReadBytes(2));
        if ((version & Critical) > (Version & Critical))
            throw new InvalidOperationException("Invalid KpMap version");

        while (true)
        {
            var type = br.ReadByte();
            var dataType = (DataTypes)type;
            if (dataType == DataTypes.None)
                break;

            var keyLength = ReadInt32LittleEndian(br.ReadBytes(4));
            var keyBytes = br.ReadBytes(keyLength);
            var key = Utils.Utf8NoBom.GetString(keyBytes);

            int valueLength = ReadInt32LittleEndian(br.ReadBytes(4));
            var valueBytes = br.ReadBytes(valueLength);

            switch (dataType)
            {
                case DataTypes.Boolean:
                    map.Add(key, valueBytes[0] == 1);
                    break;
                case DataTypes.ByteArray:
                    map.Add(key, valueBytes);
                    break;
                case DataTypes.Int32:
                    map.Add(key, ReadInt32LittleEndian(valueBytes));
                    break;
                case DataTypes.Int64:
                    map.Add(key, ReadInt64LittleEndian(valueBytes));
                    break;
                case DataTypes.String:
                    map.Add(key, Utils.Utf8NoBom.GetString(valueBytes));
                    break;
                case DataTypes.UInt32:
                    map.Add(key, ReadUInt32LittleEndian(valueBytes));
                    break;
                case DataTypes.UInt64:
                    map.Add(key, ReadUInt64LittleEndian(valueBytes));
                    break;
                default:
                    throw new InvalidOperationException($"Unknown data type {dataType}");
            }
        }

        return map;
    }

    public void Add(string key, object value)
    {
        this.values.Add(key, value);
    }

    public bool GetBoolean(string key)
    {
        if (this.values.TryGetValue(key, out var value)
            && value is bool next)
            return next;

        throw new InvalidOperationException($"Key '{key}' is not a boolean");
    }

    public byte[] GetByteArray(string key)
    {
        if (this.values.TryGetValue(key, out var value)
            && value is byte[] next)
            return next;

        throw new InvalidOperationException($"Key '{key}' is not a byte array");
    }

    public int GetInt32(string key)
    {
        if (this.values.TryGetValue(key, out var value)
            && value is int next)
            return next;

        throw new InvalidOperationException($"Key '{key}' is not an int32");
    }

    public string GetString(string key)
    {
        if (this.values.TryGetValue(key, out var value)
            && value is string next)
            return next;

        throw new InvalidOperationException($"Key '{key}' is not a string");
    }

    [CLSCompliant(false)]
    public uint GetUInt32(string key)
    {
        if (this.values.TryGetValue(key, out var value)
            && value is uint next)
            return next;

        throw new InvalidOperationException($"Key '{key}' is not a uint32");
    }

    [CLSCompliant(false)]
    public ulong GetUInt64(string key)
    {
        if (this.values.TryGetValue(key, out var value)
            && value is ulong next)
            return next;

        throw new InvalidOperationException($"Key '{key}' is not a uint64");
    }

    public long GetInt64(string key)
    {
        if (this.values.TryGetValue(key, out var value)
            && value is long next)
            return next;

        throw new InvalidOperationException($"Key '{key}' is not an int64");
    }

    public Type? GetValueType(string key)
    {
        if (this.values.TryGetValue(key, out var value) && value is not null)
            return value.GetType();

        return null;
    }

    public KpMap SetBoolean(string key, bool value)
    {
        this.values[key] = value;
        return this;
    }

    public KpMap SetByteArray(string key, byte[] value)
    {
        this.values[key] = value;
        return this;
    }

    public KpMap SetInt32(string key, int value)
    {
        this.values[key] = value;
        return this;
    }

    public KpMap SetInt64(string key, long value)
    {
        this.values[key] = value;
        return this;
    }

    public KpMap SetString(string key, string value)
    {
        this.values[key] = value;
        return this;
    }

    [CLSCompliant(false)]
    public KpMap SetUInt32(string key, uint value)
    {
        this.values[key] = value;
        return this;
    }

    [CLSCompliant(false)]
    public KpMap SetUInt64(string key, ulong value)
    {
        this.values[key] = value;
        return this;
    }

    public virtual byte[] ToBytes()
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);
        var pool = ArrayPool<byte>.Shared;
        var buffer = pool.Rent(2);
        WriteUInt16BigEndian(buffer, Version);
        bw.Write(buffer, 0, 2);
        pool.Return(buffer);

        foreach (var kvp in this.values)
        {
            var key = kvp.Key;
            var value = kvp.Value;
            var keyBytes = Utils.Utf8NoBom.GetBytes(key);
            buffer = pool.Rent(1);
            buffer[0] = (byte)DataTypes.None;
            bw.Write(buffer, 0, 1);
            pool.Return(buffer);

            buffer = pool.Rent(4);
            WriteInt32BigEndian(buffer, keyBytes.Length);
            bw.Write(buffer, 0, 4);
            pool.Return(buffer);

            bw.Write(keyBytes);

            buffer = pool.Rent(4);
            switch (value)
            {
                case bool b:
                    buffer[0] = (byte)DataTypes.Boolean;
                    buffer[1] = b ? (byte)1 : (byte)0;
                    bw.Write(buffer, 0, 2);
                    break;
                case byte[] bytes:
                    buffer[0] = (byte)DataTypes.ByteArray;
                    WriteInt32BigEndian(buffer, bytes.Length);
                    bw.Write(buffer, 0, 4);
                    bw.Write(bytes);
                    break;
                case int i:
                    buffer[0] = (byte)DataTypes.Int32;
                    WriteInt32BigEndian(buffer, i);
                    bw.Write(buffer, 0, 4);
                    break;
                case long l:
                    buffer[0] = (byte)DataTypes.Int64;
                    WriteInt64BigEndian(buffer, l);
                    bw.Write(buffer, 0, 8);
                    break;
                case string s:
                    buffer[0] = (byte)DataTypes.String;
                    var sBytes = Utils.Utf8NoBom.GetBytes(s);
                    WriteInt32BigEndian(buffer, sBytes.Length);
                    bw.Write(buffer, 0, 4);
                    bw.Write(sBytes);
                    break;
                case uint ui:
                    buffer[0] = (byte)DataTypes.UInt32;
                    WriteUInt32BigEndian(buffer, ui);
                    bw.Write(buffer, 0, 4);
                    break;
                case ulong ul:
                    buffer[0] = (byte)DataTypes.UInt64;
                    WriteUInt64BigEndian(buffer, ul);
                    bw.Write(buffer, 0, 8);
                    break;
                default:
                    throw new InvalidOperationException($"Unknown data type {value.GetType()}");
            }

            pool.Return(buffer);
        }

        return ms.ToArray();
    }

    public bool TryGetBoolean(string key, out bool value)
    {
        value = false;
        if (this.values.TryGetValue(key, out var v) && v is bool next)
        {
            value = next;
            return true;
        }

        return false;
    }

    public bool TryGetByteArray(string key, out byte[] value)
    {
        value = Array.Empty<byte>();
        if (this.values.TryGetValue(key, out var v) && v is byte[] next)
        {
            value = next;
            return true;
        }

        return false;
    }

    public bool TryGetInt32(string key, out int value)
    {
        value = 0;
        if (this.values.TryGetValue(key, out var v) && v is int next)
        {
            value = next;
            return true;
        }

        return false;
    }

    public bool TryGetInt64(string key, out long value)
    {
        value = 0;
        if (this.values.TryGetValue(key, out var v) && v is long next)
        {
            value = next;
            return true;
        }

        return false;
    }

    public bool TryGetString(string key, out string value)
    {
        value = string.Empty;
        if (this.values.TryGetValue(key, out var v) && v is string next)
        {
            value = next;
            return true;
        }

        return false;
    }

    [CLSCompliant(false)]
    public bool TryGetUInt32(string key, out uint value)
    {
        value = 0;
        if (this.values.TryGetValue(key, out var v) && v is uint next)
        {
            value = next;
            return true;
        }

        return false;
    }

    [CLSCompliant(false)]
    public bool TryGetUInt64(string key, out ulong value)
    {
        value = 0;
        if (this.values.TryGetValue(key, out var v) && v is ulong next)
        {
            value = next;
            return true;
        }

        return false;
    }

    public bool Remove(string key)
    {
        return this.values.Remove(key);
    }

    #pragma warning disable SA1201
    private enum DataTypes : byte
    {
        None = 0x0,
        UInt32 = 0x04,
        UInt64 = 0x05,
        Boolean = 0x08,
        Int32 = 0x0C,
        Int64 = 0x0D,
        String = 0x18,
        ByteArray = 0x42,
    }
}