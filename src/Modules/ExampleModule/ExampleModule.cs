using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Attributes;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.ExampleModule;

[Module(Name = "ExampleModule", Description = "An example module to get people started.", IsInternal = true)]
public class ExampleModule : EvoScModule
{
}
