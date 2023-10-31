using System;
using System.Collections.Generic;
using System.Linq;

namespace GnomeStack.Security.Cryptography
{
    public static class NonceFactory
    {
        private static readonly List<byte[]> Nonces = new List<byte[]>();
        private static readonly object SyncLock = new object();
        private static readonly Csrng RandomNumberGenerator = new Csrng();

        public static void Clear(bool clearArray = false)
        {
            lock (SyncLock)
            {
                if (clearArray)
                {
                    foreach (var iv in Nonces)
                    {
                        Array.Clear(iv, 0, iv.Length);
                    }
                }

                Nonces.Clear();
            }
        }

        public static bool Remove(byte[] iv)
        {
            lock (SyncLock)
            {
                var index = Nonces.FindIndex(o => o.SequenceEqual(iv));
                if (index > -1)
                {
                    Nonces.RemoveAt(index);
                    return true;
                }

                return false;
            }
        }

        public static byte[] Generate(int size = 8)
        {
            lock (SyncLock)
            {
                unsafe
                {
                    var set = new byte[size];
                    RandomNumberGenerator.GetBytes(set);

                    while (Nonces.Any(o => o.SequenceEqual(set)))
                    {
                        RandomNumberGenerator.GetBytes(set);
                    }

                    Nonces.Add(set);

                    return set;
                }
            }
        }
    }
}