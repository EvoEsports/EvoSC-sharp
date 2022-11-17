using System.Reflection;
using EvoSC.Common.Controllers;
using GbxRemoteNet;
using SimpleInjector;

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
    public void AddController(Type controllerType, Guid moduleId, Container services);
    
    /// <summary>
    /// Add a new controller.
    /// </summary>
    /// <param name="moduleId">The ID of the module that registered this controller.</param>
    /// <typeparam name="TController">The type representing the controller's class.</typeparam>
    public void AddController<TController>(Guid moduleId, Container services) where TController : IController;
    
    /// <summary>
    /// Add a new registry that can register controller actions.
    /// </summary>
    /// <param name="registry">An object instance that implements a controller registry.</param>
    public void AddControllerActionRegistry(IControllerActionRegistry registry);
    
    /// <summary>
    /// Remove a controller and dispose any current instances.
    /// </summary>
    /// <param name="controllerType">The type of the controller's class.</param>
    public void RemoveController(Type controllerType);
    
    /// <summary>
    /// Remove all controllers of a certain module.
    /// </summary>
    /// <param name="moduleId">The ID of the module.</param>
    public void RemoveModuleControllers(Guid moduleId);
    
    /// <summary>
    /// Get info about a registered controller.
    /// </summary>
    /// <param name="controllerType">The type of the controller's class.</param>
    /// <returns></returns>
    public ControllerInfo GetInfo(Type controllerType);
    
    /// <summary>
    /// Create a new scoped instance of a controller with a context.
    /// </summary>
    /// <param name="controllerType">The type of the controller's class.</param>
    /// <returns></returns>
    public (IController, IControllerContext) CreateInstance(Type controllerType);
    
    /// <summary>
    /// Invoke a controller action.
    /// </summary>
    /// <param name="context">Context of the action.</param>
    /// <param name="method">The action's callback method.</param>
    /// <param name="args">Arguments for the callback method.</param>
    /// <returns></returns>
    public Task InvokeActionAsync(IControllerContext context, MethodInfo method, params object[] args);
}
