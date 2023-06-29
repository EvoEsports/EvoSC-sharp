using SimpleInjector;

namespace EvoSC.Common.Application;

public class ServicesBuilder : Container
{
    public ServicesBuilder DependsOn(params string[] dependencies)
    {
        return this;
    }
}
