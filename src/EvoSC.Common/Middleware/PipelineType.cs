namespace EvoSC.Common.Middleware;

public enum PipelineType
{
    /// <summary>
    /// Middleware pipeline executed for the chat router. This is executed before
    /// forwarding any chat messages.
    /// </summary>
    ChatRouter,
    /// <summary>
    /// Middleware pipeline executed before calling an action handler in controllers.
    /// </summary>
    ControllerAction
}
