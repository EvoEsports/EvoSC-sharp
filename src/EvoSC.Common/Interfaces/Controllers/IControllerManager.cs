using EvoSC.Common.Controllers;

namespace EvoSC.Common.Interfaces.Controllers;

public interface IControllerManager
{
    /// <summary>
    /// A list of all registered controllers.
    /// </summary>
    public IEnumerable<ControllerInfo> Controllers { get; }

    /// <summary>
    /// Add a new controller.
    /// </summary>
    /// <param name="controllerType">The type representing the controller's class.</param>
    /// <param name="moduleId">The ID of the module that registered this controller.</param>
    public void AddController(Type controllerType, Guid moduleId);
    /// <summary>
    /// Add a new controller.
    /// </summary>
    /// <param name="moduleId">The ID of the module that registered this controller.</param>
    /// <typeparam name="TController">The type representing the controller's class.</typeparam>
    public void AddController<TController>(Guid moduleId) where TController : IController;
    /// <summary>
    /// Add a new registry that can register controller actions.
    /// </summary>
    /// <param name="registry"></param>
    public void AddControllerActionRegistry(IControllerActionRegistry registry);
    /// <summary>
    /// Get info about a registered controller.
    /// </summary>
    /// <param name="controllerType"></param>
    /// <returns></returns>
    public ControllerInfo GetInfo(Type controllerType);

    /// <summary>
    /// Create a new scoped instance of a controller with a context.
    /// </summary>
    /// <param name="controllerType"></param>
    /// <returns></returns>
    public IController<IControllerContext> CreateInstance(Type controllerType);
    /// <summary>
    /// Create a new scoped instance of a controller with a context.
    /// </summary>
    /// <param name="controllerType"></param>
    /// <returns></returns>
    public IController<TContext> CreateInstance<TContext>(Type controllerType) where TContext : IControllerContext;
    
}
