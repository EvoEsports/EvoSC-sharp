using EvoSC.Common.Application;
using EvoSC.Common.Util.EnumIdentifier;
using SimpleInjector;

namespace EvoSC.Common.Interfaces;

public interface IStartupPipeline
{
    /// <summary>
    /// The service container managed and built by this startup pipeline.
    /// </summary>
    public Container ServiceContainer { get; }
    
    /// <summary>
    /// Add a step to configure services.
    /// </summary>
    /// <param name="name">Name of the step.</param>
    /// <param name="servicesConfig">Action to configure the needed services.</param>
    /// <param name="dependencies">Dependencies required to run this step.</param>
    /// <returns></returns>
    public IStartupPipeline Services(string name, Action<ServicesBuilder> servicesConfig, params string[] dependencies);

    /// <summary>
    /// Add a step to configure services.
    /// </summary>
    /// <param name="name">Name of the step.</param>
    /// <param name="servicesConfig">Action to configure the needed services.</param>
    /// <param name="dependencies">Dependencies required to run this step.</param>
    /// <returns></returns>
    public IStartupPipeline Services(Enum name, Action<ServicesBuilder> servicesConfig, params string[] dependencies) =>
        Services(name.GetIdentifier(), servicesConfig, dependencies);
    
    /// <summary>
    /// Add a step to execute a generic action.
    /// </summary>
    /// <param name="name">Name of the action.</param>
    /// <param name="actionFunc">The action to be executed.</param>
    /// <param name="dependencies">Dependencies required to run this action.</param>
    /// <returns></returns>
    public IStartupPipeline Action(string name, Action<ServicesBuilder> actionFunc, params string[] dependencies);

    /// <summary>
    /// Add a step to execute a generic action.
    /// </summary>
    /// <param name="name">Name of the action.</param>
    /// <param name="actionFunc">The action to be executed.</param>
    /// <param name="dependencies">Dependencies required to run this action.</param>
    /// <returns></returns>
    public IStartupPipeline Action(Enum name, Action<ServicesBuilder> actionFunc, params string[] dependencies) =>
        Action(name.GetIdentifier(), actionFunc, dependencies);
    
    /// <summary>
    /// Add a step to execute a generic async action.
    /// </summary>
    /// <param name="name">Name of the action.</param>
    /// <param name="actionFunc">The action to be executed.</param>
    /// <param name="dependencies">Dependencies required to run this action.</param>
    /// <returns></returns>
    public IStartupPipeline AsyncAction(string name, Func<ServicesBuilder, Task> actionFunc, params string[] dependencies);

    /// <summary>
    /// Add a step to execute a generic async action.
    /// </summary>
    /// <param name="name">Name of the action.</param>
    /// <param name="actionFunc">The action to be executed.</param>
    /// <param name="dependencies">Dependencies required to run this action.</param>
    /// <returns></returns>
    public IStartupPipeline AsyncAction(Enum name, Func<ServicesBuilder, Task> actionFunc, params string[] dependencies) =>
        AsyncAction(name.GetIdentifier(), actionFunc, dependencies);

    /// <summary>
    /// Execute the given components/steps in the pipeline.
    /// </summary>
    /// <param name="components">The components to execute.</param>
    /// <returns></returns>
    public Task ExecuteAsync(params string[] components);
    
    /// <summary>
    /// Execute all components in the pipeline in the defined order.
    /// </summary>
    /// <returns></returns>
    public Task ExecuteAllAsync();
}
