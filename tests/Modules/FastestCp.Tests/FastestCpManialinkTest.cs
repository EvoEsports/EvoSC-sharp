using EvoSC.Modules.Official.FastestCp.Models;
using ManiaTemplates;

namespace EvoSC.Modules.Official.FastestCp.Tests;

public class FastestCpManialinkTest
{
    private const string Key = "FastestCp.FastestCp";

    private readonly ManiaTemplateEngine _maniaTemplateEngine = new();

    public FastestCpManialinkTest()
    {
        var file = typeof(FastestCpModule).Assembly.GetManifestResourceStream("EvoSC.Modules.Official.FastestCp.Templates.FastestCp.mt");
        var reader = new StreamReader(file!);
        _maniaTemplateEngine.AddTemplateFromString(Key, reader.ReadToEnd());
    }

    [Fact]
    public async void Should_Render_Empty_Widget()
    {
        var result = await _maniaTemplateEngine.RenderAsync(Key, new { times = Array.Empty<PlayerCpTime>() },
            new[] { typeof(FastestCpModule).Assembly });
        var expected = await File.ReadAllTextAsync("Manialinks/Empty.xml");

        Assert.Equal(result, expected, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public async void Should_Render_Filled_Widget()
    {
        var result = await _maniaTemplateEngine.RenderAsync(Key, new { times = GetCpData() },
            new[] { typeof(FastestCpModule).Assembly });
        var expected = await File.ReadAllTextAsync("Manialinks/Filled.xml");

        Assert.Equal(result, expected, ignoreLineEndingDifferences: true);
    }

    private static PlayerCpTime[] GetCpData()
    {
        const string Name = "VeryLongName";
        return Enumerable.Range(0, 7)
            .Select(i => new PlayerCpTime(Name[..i], i + 1, TimeSpan.FromSeconds(13 * i))).ToArray();
    }
}
