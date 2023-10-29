namespace GnomeStack.Standard;

#if DFX_CORE
public
#else
internal
#endif
    class EnvExpandException : SystemException
{
    public EnvExpandException()
        : base()
    {
    }

    public EnvExpandException(string message)
        : base(message)
    {
    }

    public EnvExpandException(string message, Exception inner)
        : base(message, inner)
    {
    }
}