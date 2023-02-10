using System.Text;
using System.Text.Json;
using EvoSC.Common.Interfaces.Models.Audit;
using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Models.Audit;

/// <summary>
/// General purpose audit action class that can be used instead
/// of defining custom audit actions. Optimized for quick construction.
/// </summary>
public class AuditAction : IAuditAction
{
    public string Id { get; init; }
    public dynamic? Data { get; init; }
    public string? Message { get; init; }

    public AuditAction() : this("<unknown action>") {}
    public AuditAction(string id) => Id = id;
    public AuditAction(string id, string message) : this(id) => Message = message;
    public AuditAction(string id, dynamic data) : this(id) => Data = data;
    public AuditAction(string id, string message, dynamic data) : this(id, message) => Data = data;
    public AuditAction(Enum id) : this(id.GetIdentifier(true)){}
    public AuditAction(Enum id, string message) : this(id.GetIdentifier(true)) => Message = message;
    public AuditAction(Enum id, dynamic data) : this(id.GetIdentifier(true)) => Data = data;
    public AuditAction(Enum id, string message, dynamic data) : this(id.GetIdentifier(true), message) => Data = data;

    public override string ToString()
    {
        if (Message != null)
        {
            return Message;
        }

        var message = new StringBuilder(Id);

        if (Data == null)
        {
            return message.ToString();
        }

        message.Append(' ');
        message.Append(JsonSerializer.Serialize(Data));

        return message.ToString();
    }
}
