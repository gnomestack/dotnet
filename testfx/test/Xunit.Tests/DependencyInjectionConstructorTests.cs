using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Tests;

[UseServiceProviderFactory(typeof(DIFactoryA))]
public class DependencyInjectionConstructorTests
{
    private readonly IAssert assert;

    private readonly ICustomService service;

    private readonly ITestOutputHelper writer;

    public DependencyInjectionConstructorTests(IAssert assert, ICustomService service, ITestOutputHelper writer)
    {
        this.assert = assert;
        this.service = service;
        this.writer = writer;
    }

    [UnitTest]
    public void Verify_ConstructorInjection()
    {
        if (this.assert is null)
            throw new NullException(this.assert);

        this.assert.NotNull(this.service);
        this.assert.NotNull(this.writer);
    }
}