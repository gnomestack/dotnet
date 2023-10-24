using System.Buffers;
using System.Text;
using System.Xml;

using GnomeStack.Extra.Arrays;
using GnomeStack.KeePass.Cryptography;

namespace GnomeStack.KeePass;

public class KpKeyFileFragment : KpKeyFragment
{
    public KpKeyFileFragment(string path)
    {
        if (path.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(path));

        this.Path = path;

        if (!File.Exists(path))
            throw new System.IO.FileNotFoundException(path);

        var data = GetData(path);
        this.SetData(data);
    }

    public string Path { get; }

    public static void Generate(string path, byte[] entropy)
    {
        var key = KpRng.Default.NextBytes(32);
        using var ms = new MemoryStream();
        ms.Write(entropy);
        ms.Write(key);

        ReadOnlySpan<byte> hash = ms.ToArray().ToSha256();

        CreateFile(path, hash);
    }

    private static ReadOnlySpan<byte> GetData(string path)
    {
        var bytes = GetDataFromFile(path);
        switch (bytes.Length)
        {
            case 32:
                return bytes;
            case 64:
                var hex = bytes.ToHexString();
                return Enumerable.Range(0, hex.Length / 2)
                    .Select(x => Convert.ToByte(hex.Substring(x * 2, 2), 16))
                    .ToArray();
            default:
                return bytes.ToSha256();
        }
    }

    private static byte[] GetDataFromFile(string path)
    {
        using (var fs = new FileStream(path, FileMode.Open))
        {
            var doc = new XmlDocument();
            doc.Load(fs);

            var el = doc.DocumentElement;
            if (el == null)
                return Array.Empty<byte>();

            if (!el.Name.Equals("KeyFile", StringComparison.OrdinalIgnoreCase))
                return Array.Empty<byte>();

            foreach (XmlNode child in el.ChildNodes)
            {
                if (child.Name != "Key")
                    continue;

                foreach (XmlNode subChild in child.ChildNodes)
                {
                    if (subChild.Name != "Data")
                        continue;

                    return Convert.FromBase64String(subChild.InnerText);
                }
            }
        }

        return Array.Empty<byte>();
    }

    private static void CreateFile(string path, ReadOnlySpan<byte> data)
    {
        var settings = new XmlWriterSettings()
        {
            Indent = true,
            IndentChars = "  ",
            Encoding = new UTF8Encoding(false, false),
        };

        var rental = ArrayPool<byte>.Shared.Rent(data.Length);
        data.CopyTo(rental);
        var base64 = Convert.ToBase64String(rental);
        ArrayPool<byte>.Shared.Return(rental, true);
        using var fs = new FileStream(path, FileMode.OpenOrCreate);
        using var xmlWriter = XmlWriter.Create(fs, settings);
        xmlWriter.WriteStartDocument();
        xmlWriter.WriteStartElement("KeyFile");
        xmlWriter.WriteStartElement("Meta");
        xmlWriter.WriteStartElement("Version");
        xmlWriter.WriteString("1.00");
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("Data");
        xmlWriter.WriteString(base64);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndDocument();
    }

    private byte[] Copy(byte[] source, int offset, int length = 4)
    {
        byte[] copy = new byte[length];
        Array.Copy(source, offset, copy, 0, length);
        return copy;
    }
}