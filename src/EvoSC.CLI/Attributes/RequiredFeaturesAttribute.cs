using EvoSC.Common.Application;

namespace EvoSC.CLI.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class RequiredFeaturesAttribute :  Attribute
{
    public AppFeature[] Features { get; init; }

    public RequiredFeaturesAttribute(AppFeature feature, params AppFeature[] features)
    {
        var requiredFeatures = new List<AppFeature>();
        
        if (feature == AppFeature.All)
        {
            requiredFeatures.AddRange(Enum.GetValues<AppFeature>().Where(f => f != AppFeature.All));
        }
        else
        {
            requiredFeatures.Add(feature);
            requiredFeatures.AddRange(features);
        }

        Features = requiredFeatures.ToArray();
    }
}
