using DotNet.Testcontainers.Builders;

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
        this.Writer.WriteLine("Initialize Called");
        await this.container.StartAsync();
        await Task.Delay(500);
    }

    public Task DisposeAsync()
    {
        this.Writer.WriteLine("Dispose Called");
        return this.container.DisposeAsync().AsTask();
    }
}