namespace EvoSC.Modules.Exceptions.ModuleDependency;

public class DependencyException : EvoScModuleException
{
    public DependencyException(){}
    public DependencyException(string message) : base(message){}
    public DependencyException(string message, Exception innerException) : base(message, innerException){}
}
