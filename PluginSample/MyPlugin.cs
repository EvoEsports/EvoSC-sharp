using EvoSC.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace PluginSample;

public class MyPlugin
{
    public class MyPlugin1 : IPluginFactory
    {
        public void Configure(IServiceCollection services)
        {
            services.AddTransient<ISampleService, SampleService>();
            GbxCallbacks callbacks = new GbxCallbacks("", 0);
        }
    }
}
