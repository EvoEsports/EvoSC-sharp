using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Core.Contracts;

public interface IPluginFactory
{
    void Configure(IServiceCollection services);
}
