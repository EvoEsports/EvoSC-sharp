using System.Dynamic;
using System.Resources;

namespace EvoSC.Common.Interfaces.Localization;

public abstract class ILocale : DynamicObject
{
    public abstract string this[string name, params object[] args] { get; }
    public abstract ILocale PlayerLanguage { get; }
    public abstract ResourceSet? GetResourceSet();
}
