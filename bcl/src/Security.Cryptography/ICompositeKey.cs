using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GnomeStack.Security.Cryptography
{
    [SuppressMessage(
        "Microsoft.Naming",
        "CA1710: Identifiers should have correct suffix",
        Justification = "By Design")]
    public interface ICompositeKey : IEnumerable<ICompositeKeyFragment>, IDisposable
    {
        int Count { get; }

        void Add(ICompositeKeyFragment fragment);

        void Remove(ICompositeKeyFragment fragment);

        void Clear();

        ReadOnlySpan<byte> AssembleKey();
    }
}