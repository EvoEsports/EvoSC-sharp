using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Util;

public static class ModuleDirectoryUtils
{
    /// <summary>
    /// Find all modules within a given directory.
    /// </summary>
    /// <param name="directory">A directory containing module directories.</param>
    /// <returns></returns>
    public static SortedModuleCollection<IExternalModuleInfo> FindModulesFromDirectory(string directory)
    {
        var modules = new SortedModuleCollection<IExternalModuleInfo>();
        FindModulesFromDirectory(directory, modules);
        return modules;
    }

    /// <summary>
    /// Find all modules within a given directory and add them to an existing collection.
    /// </summary>
    /// <param name="directory">A directory containing module directories.</param>
    /// <param name="modules">Collection to add the modules to.</param>
    /// <returns></returns>
    public static void FindModulesFromDirectory(string directory, SortedModuleCollection<IExternalModuleInfo> modules)
    {
        foreach (var dir in Directory.GetDirectories(Path.GetFullPath(directory)))
        {
            var infoFile = Path.Combine(dir, "info.toml");

            if (!File.Exists(infoFile))
            {
                continue;
            }

            var moduleInfo = ModuleInfoUtils.CreateFromDirectory(new DirectoryInfo(dir));
            modules.Add(moduleInfo);
        }
    }
}
