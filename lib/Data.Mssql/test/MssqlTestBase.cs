using GnomeStack.Standard;

using Testcontainers.MsSql;

using Xunit.Abstractions;

namespace Tests;

public abstract class MssqlTestBase : IAsyncLifetime
{
    private readonly MsSqlContainer container;

    protected MssqlTestBase(ITestOutputHelper writer)
    {
        this.Writer = writer;
        this.container = new MsSqlBuilder()
            .WithPortBinding(1433, true)
            .Build();
    }

    protected string ConnectString => this.container.GetConnectionString();

    protected bool SkipTest => Env.IsWindows && Env.Get("CI") == "true";

    protected ITestOutputHelper Writer { get; }

    public async Task InitializeAsync()
    {
        if (this.SkipTest)
            return;
        this.Writer.WriteLine("Initialize Called");
        await this.container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        if (this.SkipTest)
            return;

        this.Writer.WriteLine("Dispose Called");
        await this.container.DisposeAsync().AsTask();
    }
}