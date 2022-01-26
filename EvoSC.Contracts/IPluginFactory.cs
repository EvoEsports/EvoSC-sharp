using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Contracts;

public interface IPluginFactory
{
    void Configure(IServiceCollection services);
}
