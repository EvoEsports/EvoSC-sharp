namespace EvoSC.Common.Interfaces.Config.Mapping;

public interface ITomlMappingManager
{
    public void AddMapper<T>(ITomlTypeMapper<T> mapper);

    public void SetupDefaultMappers();
}
