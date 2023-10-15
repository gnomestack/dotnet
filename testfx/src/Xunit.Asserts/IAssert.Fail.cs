namespace Xunit;

public partial interface IAssert
{
    /// <summary>
    /// Fails the test.
    /// </summary>
    /// <param name="message">The message of why the test case failed.</param>
    void Fail(string message);
}