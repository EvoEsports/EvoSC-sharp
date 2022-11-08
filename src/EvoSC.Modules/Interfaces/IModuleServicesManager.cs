using SimpleInjector;

namespace EvoSC.Modules.Interfaces;

public interface IModuleServicesManager
{
    public void AddContainer(Guid moduleId, Container container);
    public void RemoveContainer(Guid moduleId);
}
