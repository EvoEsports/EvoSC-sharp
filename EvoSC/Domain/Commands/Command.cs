using System;
using System.Collections.Generic;
using System.Reflection;

namespace EvoSC.Domain.Commands;

#nullable enable
public class Command
{
    public MethodInfo CmdMethod;
    public Type CmdType;
    public string Description;
    public string Name;
    public Dictionary<string, string>? Parameters;
    public string? Permission;
}
