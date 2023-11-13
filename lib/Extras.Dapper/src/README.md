# GnomeStack.Extras.Dapper

Retry extensions for Dapper using Polly. Each retry method has a polly 8+
`ResiliencePipeline? retryPolicy = null,` parameter that can be used to
customize the retry policy, otherwise the default policy is used.

GnomeStack.Data will have similar methods

## Usage

```csharp
using Dapper;

var connection = new SqlConnection("...");
connection.RetryExecute("CREATE DATABASE [Test]");
```

MIT License
