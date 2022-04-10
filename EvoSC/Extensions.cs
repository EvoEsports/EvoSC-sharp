using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC;

public static class Extensions
{
    public static IServiceCollection Remove<T>(this IServiceCollection serviceCollection)
    {
        var serviceDescriptor = serviceCollection.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(T));
        if (serviceDescriptor != null)
        {
            serviceCollection.Remove(serviceDescriptor);
        }

        return serviceCollection;
    }
}
