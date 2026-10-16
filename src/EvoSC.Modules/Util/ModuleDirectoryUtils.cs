using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Models;

namespace EvoSC.Modules.Util;

public static class ModuleDirectoryUtils
{
    /// <summary>
    /// Read the metadata of every module in a directory. A module is any subdirectory holding an
    /// info.toml; the name of that subdirectory is irrelevant.
    /// </summary>
    public static IEnumerable<ExternalModuleInfo> FindModulesIn(string directory)
    {
        foreach (var dir in Directory.GetDirectories(Path.GetFullPath(directory)))
        {
            var infoFile = Path.Combine(dir, "info.toml");

            if (!File.Exists(infoFile))
            {
                continue;
            }

            yield return (ExternalModuleInfo)ModuleInfoUtils.CreateFromDirectory(new DirectoryInfo(dir));
        }
    }

    public static SortedModuleCollection<IExternalModuleInfo> FindModulesFromDirectory(string directory)
    {
        var modules = new SortedModuleCollection<IExternalModuleInfo>();
        FindModulesFromDirectory(directory, modules);
        return modules;
    }

    /// <summary>
    /// Finds every module in a directory and adds it to <paramref name="modules"/>. Ids listed in
    /// <paramref name="excludeIds"/> are left out, so that a module already registered from another
    /// source is not loaded twice.
    /// </summary>
    public static void FindModulesFromDirectory(string directory, SortedModuleCollection<IExternalModuleInfo> modules, IEnumerable<string>? excludeIds = null)
    {
        var excluded = excludeIds?.ToHashSet() ?? [];

        foreach (var module in FindModulesIn(directory))
        {
            if (excluded.Contains(module.Id))
            {
                continue;
            }

            modules.Add(module);
        }
    }
}
