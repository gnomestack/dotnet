namespace GnomeStack.KeePass;

internal static class Extensions
{
    public static string ToHexString(this byte[] bytes)
    {
        char[] chars = new char[bytes.Length * 2];
        for (int i = 0; i < bytes.Length; i++)
        {
            int bit = bytes[i] >> 4;
            chars[i * 2] = (char)(55 + bit + (((bit - 10) >> 31) & -7));
            bit = bytes[i] & 0xF;
            chars[(i * 2) + 1] = (char)(55 + bit + (((bit - 10) >> 31) & -7));
        }

        return new string(chars);
    }
}