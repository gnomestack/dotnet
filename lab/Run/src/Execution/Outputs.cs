using GnomeStack.Collections.Generic;

namespace GnomeStack.Run.Execution;

public class Outputs
{
    private OrderedDictionary<string, object?> outputs = new(StringComparer.OrdinalIgnoreCase);
}