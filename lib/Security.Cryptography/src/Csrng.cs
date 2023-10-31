using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace GnomeStack.Security.Cryptography;

public class Csrng : RandomNumberGenerator
{
    private bool disposed;

    private RandomNumberGenerator? rng;

    public byte[] NextBytes(int length)
    {
        if (this.disposed)
            throw new ObjectDisposedException(this.GetType().FullName);

        switch (length)
        {
            case 0:
                return Array.Empty<byte>();
            case < 0:
                throw new ArgumentOutOfRangeException(nameof(length));
        }

        var bytes = new byte[length];
        this.GetBytes(bytes);
        return bytes;
    }

    public override void GetBytes(byte[] data)
    {
        if (this.disposed)
            throw new ObjectDisposedException(this.GetType().FullName);

        this.rng ??= RandomNumberGenerator.Create();
        this.rng.GetBytes(data);
    }

#if NETSTANDARD2_0 || NETFRAMEWORK
    public virtual void GetBytes(Span<byte> data)
    {
        if (this.disposed)
            throw new ObjectDisposedException(this.GetType().FullName);

        var rental = ArrayPool<byte>.Shared.Rent(data.Length);
        try
        {
            this.rng ??= RandomNumberGenerator.Create();
            this.rng.GetBytes(rental);
            new ReadOnlySpan<byte>(rental, 0, data.Length).CopyTo(data);
        }
        finally
        {
            Array.Clear(rental, 0, rental.Length);
            ArrayPool<byte>.Shared.Return(rental);
        }
    }

#endif

    public override void GetNonZeroBytes(byte[] data)
    {
        if (this.disposed)
            throw new ObjectDisposedException(this.GetType().FullName);

        this.GetNonZeroBytesSpan(data);
    }

    public short NextInt16()
    {
        if (this.disposed)
            throw new ObjectDisposedException(this.GetType().FullName);

        var rental = ArrayPool<byte>.Shared.Rent(sizeof(short));
        try
        {
            this.GetBytes(rental);
            return BitConverter.ToInt16(rental, 0);
        }
        finally
        {
            Array.Clear(rental, 0, rental.Length);
            ArrayPool<byte>.Shared.Return(rental);
        }
    }

    public int NextInt32()
    {
        if (this.disposed)
            throw new ObjectDisposedException(this.GetType().FullName);

        var rental = ArrayPool<byte>.Shared.Rent(sizeof(int));
        try
        {
            this.GetBytes(rental);
            return BitConverter.ToInt32(rental, 0);
        }
        finally
        {
            Array.Clear(rental, 0, rental.Length);
            ArrayPool<byte>.Shared.Return(rental);
        }
    }

    public long NextInt64()
    {
        if (this.disposed)
            throw new ObjectDisposedException(this.GetType().FullName);

        var rental = ArrayPool<byte>.Shared.Rent(sizeof(long));
        try
        {
            this.GetBytes(rental);
            return BitConverter.ToInt64(rental, 0);
        }
        finally
        {
            Array.Clear(rental, 0, rental.Length);
            ArrayPool<byte>.Shared.Return(rental);
        }
    }

    [CLSCompliant(false)]
    public ushort NextUInt16()
    {
        if (this.disposed)
            throw new ObjectDisposedException(this.GetType().FullName);

        var rental = ArrayPool<byte>.Shared.Rent(sizeof(ushort));
        try
        {
            this.GetBytes(rental);
            return BitConverter.ToUInt16(rental, 0);
        }
        finally
        {
            Array.Clear(rental, 0, rental.Length);
            ArrayPool<byte>.Shared.Return(rental);
        }
    }

    [CLSCompliant(false)]
    public ulong NextUInt64(ulong max)
    {
        if (this.disposed)
            throw new ObjectDisposedException(this.GetType().FullName);

        if (max == 0)
            return 0;

        ulong next;
        ulong mod;
        ulong exclusive = ulong.MaxValue - (max - 1UL);
        do
        {
            next = this.NextUInt64();
            mod = next % max;
        }
        while ((next - mod) > exclusive);

        return next;
    }

    [CLSCompliant(false)]
    public ulong NextUInt64()
    {
        if (this.disposed)
            throw new ObjectDisposedException(this.GetType().FullName);

        var rental = ArrayPool<byte>.Shared.Rent(sizeof(ulong));
        try
        {
            this.GetBytes(rental);
            return BitConverter.ToUInt64(rental, 0);
        }
        finally
        {
            Array.Clear(rental, 0, rental.Length);
            ArrayPool<byte>.Shared.Return(rental);
        }
    }

    public ReadOnlySpan<byte> NextSpan(int count)
    {
        if (this.disposed)
            throw new ObjectDisposedException(this.GetType().FullName);

        return this.NextBytes(count);
    }

    [SuppressMessage("Major Code Smell", "S4144:Methods should not have identical implementations")]
    public ReadOnlyMemory<byte> NextMemory(int count)
    {
        if (this.disposed)
            throw new ObjectDisposedException(this.GetType().FullName);

        return this.NextBytes(count);
    }

    protected override void Dispose(bool disposing)
    {
        if (this.disposed || !disposing)
            return;

        this.rng?.Dispose();
        this.disposed = true;

        base.Dispose(disposing);
    }

    private void GetNonZeroBytesSpan(Span<byte> data)
    {
        while (data.Length > 0)
        {
            // Fill the remaining portion of the span with random bytes.
            this.GetBytes(data);

            // Find the first zero in the remaining portion.
            int indexOfFirst0Byte = data.Length;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == 0)
                {
                    indexOfFirst0Byte = i;
                    break;
                }
            }

            // If there were any zeros, shift down all non-zeros.
            for (int i = indexOfFirst0Byte + 1; i < data.Length; i++)
            {
                if (data[i] != 0)
                {
                    data[indexOfFirst0Byte++] = data[i];
                }
            }

            // Request new random bytes if necessary; dont re-use
            // existing bytes since they were shifted down.
            data = data.Slice(indexOfFirst0Byte);
        }
    }
}