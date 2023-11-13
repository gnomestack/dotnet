using GnomeStack.Collections.Generic;
using GnomeStack.Diagnostics;

namespace GnomeStack.Apt;

public abstract class AptCmd : PsCommand
{
    protected override string GetExecutablePath()
        => "/usr/bin/apt-get";
}