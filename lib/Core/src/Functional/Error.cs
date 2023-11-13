namespace GnomeStack.Functional;

public class Error : IError
{
    public Error(string? message, IInnerError? innerError = null)
    {
        this.Message = message ?? "Unknown error";
        this.InnerError = innerError;
    }

    public static Func<Exception, Error> Convert { get; set; } = (ex) =>
    {
        IError? innerError = null;
        if (ex.InnerException is not null)
        {
            #pragma warning disable CS8602
            innerError = Error.Convert(ex.InnerException);
            #pragma warning restore CS8602
        }

        if (ex is ArgumentException argEx)
        {
            return new ArgumentError(argEx.Message, innerError)
            {
                StackTrace = ex.StackTrace,
                ParamName = argEx.ParamName,
                Target = ex.TargetSite is null
                    ? null
                    : (ex.TargetSite.DeclaringType?.FullName + "." + ex.TargetSite?.Name) ?? null,
                Code = ex.GetType().FullName,
            };
        }

        return new Error(ex.Message, innerError)
        {
            StackTrace = ex.StackTrace,
            Target = ex.TargetSite is null
                ? null
                : (ex.TargetSite.DeclaringType?.FullName + "." + ex.TargetSite?.Name) ?? null,
            Code = ex.GetType().FullName,
        };
    };

    public string Message { get; set; }

    public string? Code { get; set; }

    public string? Target { get; set; }

    public string? StackTrace { get; set; }

    public IInnerError? InnerError { get; set; }

    public static implicit operator Error(Exception ex)
        => Convert(ex);

    public override string ToString()
    {
        return this.Message;
    }
}

public class ArgumentError : Error
{
    public ArgumentError(string? message, IInnerError? innerError = null)
        : base(message, innerError)
    {
    }

    public string? ParamName { get; set; }
}