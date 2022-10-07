namespace EvoSC.Common.Events.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class Subscribe : Attribute
{
    /// <summary>
    /// The name of the event to subscribe to.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Whether to run the callback asynchronous. Enabling this will essentially have the callback run in
    /// it's own thread.
    /// </summary>
    public bool IsAsync { get; set; }
    /// <summary>
    /// The callback priority. Subscriptions with a higher priority will be called first.
    /// </summary>
    public EventPriority Priority { get; set; }

    public Subscribe(string name, EventPriority priority = EventPriority.Medium, bool isAsync = false)
    {
        Name = name;
        IsAsync = isAsync;
    }
}
