using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;

public interface INatsJetstreamService
{
    /// <summary>
    /// Publishes a message to a NATS subject.
    /// </summary>
    /// <param name="subject">The subject will be prefixed with a message group, which can be set from the main config.</param>
    /// <param name="message">The message to be published.</param>
    /// <typeparam name="T">The type of the data object sent as a message</typeparam>
    /// <returns></returns>
    Task PublishMessageAsync<T>(string subject, T message);
}
