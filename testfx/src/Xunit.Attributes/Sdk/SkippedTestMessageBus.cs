using System;
using System.Collections.Generic;
using System.Text;

using Xunit.Abstractions;

namespace Xunit.Sdk;

/// <summary>
/// The SkippedTestMessageBus is based on Skippable Fact, DotNet Arcade, and Xunit 3
/// where the test is skipped due to a string token found in the exception message.
/// </summary>
public class SkippedTestMessageBus : IMessageBus
{
    private readonly IMessageBus innerBus;

    public SkippedTestMessageBus(IMessageBus innerBus)
    {
        this.innerBus = innerBus;
    }

    public int SkippedTestCount { get; private set; }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public bool QueueMessage(IMessageSinkMessage message)
    {
        if (message is not ITestFailed testFailed)
            return this.innerBus.QueueMessage(message);

        foreach (var exceptionMessage in testFailed.Messages)
        {
            if (!exceptionMessage.StartsWith(DynamicSkipToken.Value))
                continue;

            this.SkippedTestCount++;
            if (exceptionMessage.Length <= DynamicSkipToken.Value.Length)
                continue;

            var skipReason = exceptionMessage.Substring(DynamicSkipToken.Value.Length);

            return this.innerBus.QueueMessage(new TestSkipped(testFailed.Test, skipReason));
        }

        return this.innerBus.QueueMessage(message);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
            return;

        this.innerBus?.Dispose();
    }
}