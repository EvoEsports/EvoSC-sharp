using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Config.Net;
using EvoSC.Common.Config.Mapping;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Config.Stores;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Localization;
using EvoSC.Common.Middleware;
using EvoSC.Common.Middleware.Attributes;
using EvoSC.Common.Permissions.Attributes;
using EvoSC.Common.Permissions.Models;
using EvoSC.Common.Services.Exceptions;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;
using EvoSC.Common.Util.EnumIdentifier;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Models;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Exceptions;
using EvoSC.Modules.Exceptions.ModuleDependency;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Models;
using EvoSC.Modules.Util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;

namespace EvoSC.Modules;

public class ModuleManager : IModuleManager
{
	private readonly ILogger<ModuleManager> _logger;
	private readonly ILogger<LocalizationManager> _localizationLogger;

	private readonly IControllerManager _controllers;

	private readonly IServiceContainerManager _servicesManager;

	private readonly IActionPipelineManager _pipelineManager;

	private readonly IPermissionManager _permissions;

	private readonly IEvoScBaseConfig _config;

	private readonly IConfigStoreRepository _configStoreRepository;

	private readonly IManialinkManager _manialinkManager;

	private readonly IThemeManager _themeManager;

	private readonly Dictionary<Guid, IModuleLoadContext> _loadedModules = new Dictionary<Guid, IModuleLoadContext>();

	private readonly Dictionary<string, Guid> _moduleNameMap = new Dictionary<string, Guid>();

	private readonly ExportAssemblyStore _exportAssemblies;

	public IReadOnlyList<IModuleLoadContext> GetLoadedModules() => _loadedModules.Values.ToList();

	/// <summary>
	/// Orders the loaded modules so that every module follows the ones it depends on. Loading
	/// doesn't require dependencies to be loaded first, so load order says nothing about this.
	/// </summary>
	public IReadOnlyList<IModuleLoadContext> GetLoadedModulesByDependency()
	{
		var remaining = GetLoadedModules().ToList();
		var ordered = new List<IModuleLoadContext>(remaining.Count);
		var placed = new HashSet<Guid>();

		while (remaining.Count > 0)
		{
			IModuleLoadContext? next = null;

			foreach (var module in remaining)
			{
				if (module.LoadedDependencies.All(placed.Contains))
				{
					next = module;
					break;
				}
			}

			if (next == null)
			{
				// What is left depends on itself, so no order satisfies it. Hand out the rest in
				// load order rather than dropping them.
				ordered.AddRange(remaining);
				break;
			}

			remaining.Remove(next);
			placed.Add(next.LoadId);
			ordered.Add(next);
		}

		return ordered;
	}

	internal ExportAssemblyStore ExportAssemblies => _exportAssemblies;

	public ModuleManager(ILogger<ModuleManager> logger, ILogger<LocalizationManager> localizationLogger,
        IEvoScBaseConfig config, IControllerManager controllers, IServiceContainerManager servicesManager, IActionPipelineManager pipelineManager, IPermissionManager permissions, IConfigStoreRepository configStoreRepository, IManialinkManager manialinkManager, IThemeManager themeManager)
	{
		_logger = logger;
		_localizationLogger = localizationLogger;
		_config = config;
		_controllers = controllers;
		_servicesManager = servicesManager;
		_pipelineManager = pipelineManager;
		_permissions = permissions;
		_configStoreRepository = configStoreRepository;
		_manialinkManager = manialinkManager;
		_themeManager = themeManager;
		_exportAssemblies = new ExportAssemblyStore();
		WarnForDisabledVerification();
	}

	private void WarnForDisabledVerification()
	{
		if (!_config.Modules.RequireSignatureVerification)
		{
			_logger.LogWarning("Signature verification for modules is disabled");
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public async Task InstallAsync(Guid loadId)
	{
		IModuleLoadContext moduleContext = GetModule(loadId);
		await InstallPermissionsAsync(moduleContext);
		await TryCallModuleInstallAsync(moduleContext);
		_logger.LogDebug("Module {Type}({Module}) was installed", moduleContext.MainClass, loadId);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public async Task UninstallAsync(Guid loadId)
	{
		IModuleLoadContext moduleContext = GetModule(loadId);
		await UninstallPermissionsAsync(moduleContext);
		await TryCallModuleUninstallAsync(moduleContext);
		_logger.LogDebug("Module {Type}({Module}) was uninstalled", moduleContext.MainClass, loadId);
	}

	public IModuleLoadContext GetModule(Guid loadId)
	{
		if (loadId == Guid.Empty || !_loadedModules.TryGetValue(loadId, out var module))
		{
			throw new EvoScModuleException($"Module with Id {loadId} does not exist.");
		}
		return module;
	}

	private Task RegisterPermissionsAsync(IModuleLoadContext loadContext)
	{
		foreach (Assembly assembly in loadContext.Assemblies)
		{
			foreach (Type permissionGroup in assembly.AssemblyTypesWithAttribute<PermissionGroupAttribute>())
			{
				string groupName = permissionGroup.Name;
				IdentifierAttribute? customAttribute = permissionGroup.GetCustomAttribute<IdentifierAttribute>();
				if (customAttribute != null)
				{
					groupName = customAttribute.Name;
				}
				FieldInfo[] fields = permissionGroup.GetFields();
				foreach (FieldInfo fieldInfo in fields)
				{
					if (fieldInfo.FieldType == permissionGroup)
					{
						string permissionName = fieldInfo.GetCustomAttribute<IdentifierAttribute>()?.Name ?? fieldInfo.Name;
						loadContext.Permissions.Add(new Permission
						{
							Name = groupName + "." + permissionName,
							Description = fieldInfo.GetCustomAttribute<DescriptionAttribute>()?.Description ?? ""
						});
					}
				}
			}
		}
		return Task.CompletedTask;
	}

	private async Task InstallPermissionsAsync(IModuleLoadContext moduleContext)
	{
		List<IPermission> identifiedPermissions = new List<IPermission>();
		foreach (IPermission permission in moduleContext.Permissions)
		{
			IPermission? existingPermission = await _permissions.GetPermissionAsync(permission.Name);
			if (existingPermission != null)
			{
				_logger.LogDebug("Wont install permission '{Name}' as it already exists", permission.Name);
				identifiedPermissions.Add(existingPermission);
				continue;
			}
			_logger.LogDebug("Installing permission: {Name}", permission.Name);
			await _permissions.AddPermissionAsync(permission);
			IPermission? identifiedPermission = await _permissions.GetPermissionAsync(permission.Name);
			if (identifiedPermission == null)
			{
				_logger.LogError("Could not identify permission '{Name}' after installing it. Was it not added to the database?", permission.Name);
			}
			else
			{
				identifiedPermissions.Add(identifiedPermission);
			}
		}
		moduleContext.Permissions = identifiedPermissions;
	}

	private async Task UninstallPermissionsAsync(IModuleLoadContext moduleContext)
	{
		foreach (IPermission permission in moduleContext.Permissions)
		{
			await _permissions.RemovePermissionAsync(permission);
		}
	}

	private Task EnableMiddlewaresAsync(IModuleLoadContext moduleContext)
	{
		_pipelineManager.AddPipeline(PipelineType.ChatRouter, moduleContext.LoadId, moduleContext.Pipelines[PipelineType.ChatRouter]);
		_pipelineManager.AddPipeline(PipelineType.ControllerAction, moduleContext.LoadId, moduleContext.Pipelines[PipelineType.ControllerAction]);
		return Task.CompletedTask;
	}

	private Task DisableMiddlewaresAsync(IModuleLoadContext moduleContext)
	{
		_pipelineManager.RemovePipeline(PipelineType.ChatRouter, moduleContext.LoadId);
		_pipelineManager.RemovePipeline(PipelineType.ControllerAction, moduleContext.LoadId);
		return Task.CompletedTask;
	}

	private Task RegisterMiddlewaresAsync(IModuleLoadContext moduleContext)
	{
		foreach (Assembly assembly in moduleContext.Assemblies)
		{
			foreach (Type item in assembly.AssemblyTypesWithAttribute<MiddlewareAttribute>())
			{
				MiddlewareAttribute? customAttribute = item.GetCustomAttribute<MiddlewareAttribute>();
				if (customAttribute is not null)
				{
					moduleContext.Pipelines[customAttribute.For].AddComponent(item, moduleContext.Services);
				}
			}
		}
		return Task.CompletedTask;
	}

	private async Task RegisterManialinksTemplatesAsync(IModuleLoadContext loadContext)
	{
		string[] namespaceParts = loadContext.RootNamespace.Split(".");
		foreach (Assembly assembly in loadContext.Assemblies)
		{
			string[] manifestResourceNames = assembly.GetManifestResourceNames();
			foreach (string resourceName in manifestResourceNames)
			{
				string[] nameComponents = resourceName.Split('.');
				if (nameComponents.Length <= 1)
				{
					continue;
				}
				string extension = nameComponents[^1];
				ManialinkTemplateType? templateType = extension.ToEnumValue<ManialinkTemplateType>();
				if (!templateType.HasValue)
				{
					continue;
				}
				Stream? resourceStream = assembly.GetManifestResourceStream(resourceName);
				if (resourceStream != null)
				{
					using StreamReader streamReader = new StreamReader(resourceStream);
					string contents = await streamReader.ReadToEndAsync();
					string templateName = GetManialinkTemplateName(loadContext, namespaceParts, nameComponents);
					loadContext.ManialinkTemplates.Add(new ModuleManialinkTemplate
					{
						Content = contents,
						Name = templateName,
						Type = templateType.Value
					});
				}
			}
		}
	}

	private static string GetManialinkTemplateName(IModuleLoadContext loadContext, string[] namespaceParts, string[] nameComponents)
	{
		int i = 0;
		while (i < namespaceParts.Length && nameComponents[i].Equals(namespaceParts[i], StringComparison.Ordinal))
		{
			i++;
		}
		if (nameComponents[i].Equals("Templates", StringComparison.Ordinal))
		{
			i++;
		}
		return loadContext.ModuleInfo.Id + "." + string.Join(".", nameComponents[i..^1]);
	}

	private Task EnableManialinkTemplatesAsync(IModuleLoadContext moduleContext)
	{
		foreach (IModuleManialinkTemplate manialinkTemplate in moduleContext.ManialinkTemplates)
		{
			switch (manialinkTemplate.Type)
			{
			case ManialinkTemplateType.Script:
				_manialinkManager.AddManiaScript(new ManiaScriptInfo
				{
					Name = manialinkTemplate.Name,
					Content = manialinkTemplate.Content
				});
				break;
			case ManialinkTemplateType.Template:
				_manialinkManager.AddTemplate(new ManialinkTemplateInfo
				{
					Assemblies = moduleContext.Assemblies,
					Name = manialinkTemplate.Name,
					Content = manialinkTemplate.Content
				});
				break;
			}
		}
		return Task.CompletedTask;
	}

	private async Task DisableManialinkTemplatesAsync(IModuleLoadContext moduleContext)
	{
		foreach (IModuleManialinkTemplate manialinkTemplate in moduleContext.ManialinkTemplates)
		{
			switch (manialinkTemplate.Type)
			{
				case ManialinkTemplateType.Script:
					_manialinkManager.RemoveManiaScript(manialinkTemplate.Name);
					break;
				case ManialinkTemplateType.Template:
					await _manialinkManager.RemoveAndHideTemplateAsync(manialinkTemplate.Name);
					break;
			}
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Task TryCallModuleEnableAsync(IModuleLoadContext moduleContext)
	{
		if (moduleContext.Instance is IToggleable toggleable)
		{
			return toggleable.EnableAsync();
		}
		return Task.CompletedTask;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Task TryCallModuleDisableAsync(IModuleLoadContext moduleContext)
	{
		if (moduleContext.Instance is IToggleable toggleable)
		{
			return toggleable.DisableAsync();
		}
		return Task.CompletedTask;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Task TryCallModuleInstallAsync(IModuleLoadContext moduleContext)
	{
		if (moduleContext.Instance is IInstallable installable)
		{
			return installable.InstallAsync();
		}
		return Task.CompletedTask;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Task TryCallModuleUninstallAsync(IModuleLoadContext moduleContext)
	{
		if (moduleContext.Instance is IInstallable installable)
		{
			return installable.UninstallAsync();
		}
		return Task.CompletedTask;
	}

	private Task EnableControllersAsync(IModuleLoadContext moduleContext)
	{
		try
		{
			foreach (Assembly assembly in moduleContext.Assemblies)
			{
				foreach (Type item in assembly.AssemblyTypesWithAttribute<ControllerAttribute>())
				{
					ControllerAttribute? customAttribute = item.GetCustomAttribute<ControllerAttribute>();
					if (customAttribute != null)
					{
						_controllers.AddController(item, moduleContext.LoadId, moduleContext.Services);
					}
				}
			}
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "Failed to add controller");
			throw;
		}
		return Task.CompletedTask;
	}

	private Task DisableControllersAsync(IModuleLoadContext moduleContext)
	{
		_controllers.RemoveModuleControllers(moduleContext.LoadId);
		return Task.CompletedTask;
	}

	private async Task RegisterModuleConfigAsync(IEnumerable<Assembly> assemblies, SimpleInjector.Container container, IModuleInfo moduleInfo)
	{
		foreach (Assembly assembly in assemblies)
		{
			foreach (Type type in assembly.AssemblyTypesWithAttribute<SettingsAttribute>())
			{
				SettingsAttribute? configAttr = type.GetCustomAttribute<SettingsAttribute>();
				if (configAttr == null)
				{
					continue;
				}

				if (!type.IsInterface)
				{
					_logger.LogError("Settings type {Type} must be an interface", type);
					throw new ServicesException($"Settings type {type} must be an interface.");
				}

				// A module whose settings cannot be built must not load: it would fail later on with an
				// unrelated error, as a service cannot be resolved without its settings.
				object? config = CreateConfigInstance(type, await CreateModuleConfigStoreAsync(moduleInfo.Id, type));
				if (config == null)
				{
					_logger.LogError("An instance of the module config {Type} could not be created", type);
					throw new InvalidOperationException("Failed to create module config instance.");
				}

				container.RegisterInstance(type, config);
			}
		}
	}

	private async Task<IConfigStore> CreateModuleConfigStoreAsync(string name, Type configInterface)
	{
		DatabaseStore dbStore = new DatabaseStore(name, configInterface, _configStoreRepository);
		await dbStore.SetupDefaultSettingsAsync();
		return new EvSCModuleConfigStore(name, dbStore);
	}

	private object? CreateConfigInstance(Type configInterface, IConfigStore store)
	{
		object? configBuilder = ReflectionUtils.CreateGenericInstance(typeof(ConfigurationBuilder<>), configInterface);
		if (configBuilder == null)
		{
			throw new InvalidOperationException("Failed to create module config builder.");
		}
		ReflectionUtils.CallMethod(configBuilder, "UseConfigStore", store);
		ReflectionUtils.CallMethod(configBuilder, "UseTypeParser", new TextColorTypeParser());
		ReflectionUtils.CallMethod(configBuilder, "UseTypeParser", new VersionParser());

		// Config.Net emits the settings object with Castle, which cannot be used from a collectible
		// load context. The configuration is still Config.Net's, only the emitted type differs.
		if (configInterface.Assembly.IsCollectible)
		{
			return CollectibleConfigProxy.Create(configInterface, ConfigNetInterceptor.Create(configBuilder, configInterface));
		}

		return ReflectionUtils.CallMethod(configBuilder, "Build");
	}

	private bool VerifyExternalModule(IExternalModuleInfo moduleInfo)
	{
		return !_config.Modules.RequireSignatureVerification || moduleInfo.ModuleFiles.All((IModuleFile file) => file.VerifySignature());
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private (Type?, AssemblyLoadContext?) CreateAssemblyLoadContext(Guid loadId, IExternalModuleInfo moduleInfo)
	{
		string[] assemblyPaths = moduleInfo.AssemblyFiles.Select((IModuleFile f) => f.File.FullName).ToArray();
		string[] exportPaths = assemblyPaths.Where((string f) => Path.GetFileNameWithoutExtension(f).EndsWith(".Exports", StringComparison.OrdinalIgnoreCase)).ToArray();

		// A dependency's export must never be shipped inside the depending module's directory —
		// it is loaded dynamically from the provider. Skip stale copies so they neither register
		// as the module's own export nor load as a private copy.
		var foreignExports = exportPaths
			.Where(exportPath => !_exportAssemblies.IsOwnExportPath(exportPath))
			.ToArray();
		foreach (string foreignPath in foreignExports)
		{
			_logger.LogWarning(
				"Module '{Module}' ships an export assembly '{Export}' that belongs to another module. " +
				"The file will be ignored; the export is loaded dynamically from its provider.",
				moduleInfo.Id, Path.GetFileName(foreignPath));
		}

		string[] loadablePaths = assemblyPaths.Except<string>(exportPaths, StringComparer.OrdinalIgnoreCase).ToArray();
		if (loadablePaths.Length == 0)
		{
			_logger.LogError("No assemblies found in module directory for '{Name}'. The module will not load", moduleInfo.Id);
			return (null, null);
		}
		string[] ownedExports = exportPaths.Except<string>(foreignExports, StringComparer.OrdinalIgnoreCase).ToArray();
		foreach (string exportPath in ownedExports)
		{
			_exportAssemblies.RegisterExportPath(moduleInfo.Id, exportPath);
			_exportAssemblies.AcquireExportForModule(loadId, moduleInfo.Id);
			_logger.LogDebug("Registered export assembly '{Export}' for module {Module}", Path.GetFileName(exportPath), moduleInfo.Id);
		}
		EvoScModuleLoadContext loadContext = new EvoScModuleLoadContext(loadablePaths[0], _exportAssemblies.CreateExportResolver(loadId));
		Type? mainType = null;
		foreach (string path in loadablePaths)
		{
			Assembly assembly = loadContext.LoadModuleAssembly(path);
			if (mainType == null)
			{
				mainType = assembly.AssemblyTypesWithAttribute<ModuleAttribute>().FirstOrDefault();
			}
		}
		return (mainType, loadContext);
	}

	/// <summary>
	/// A module classifies itself with [Module(IsInternal = true)]. The application also knows which
	/// modules it registered as internal, so either declaration is enough to protect a module from
	/// being unloaded or reloaded.
	/// </summary>
	private void ApplyModuleDeclaration(IExternalModuleInfo moduleInfo, Type mainClass)
	{
		if (mainClass.GetCustomAttribute<ModuleAttribute>() is not { IsInternal: true })
		{
			return;
		}

		if (moduleInfo is ExternalModuleInfo module)
		{
			module.IsInternal = true;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnsureExportDependenciesLoaded(Guid ownerLoadId, IModuleInfo moduleInfo)
	{
		// Exports are registered by module id when the provider's load context is created, so the
		// provider does not have to be loaded before its consumers. This is what allows two modules
		// to depend on each other.
		foreach (string providerModuleId in moduleInfo.Dependencies.Select(dependency => dependency.Name).Where(_exportAssemblies.HasExport))
		{
			// Load the dependency's export now so the module binds against the shared export
			// assembly. Binary incompatibility is enforced by the runtime binder.
			_exportAssemblies.AcquireExportForModule(ownerLoadId, providerModuleId);
			_logger.LogDebug("Loaded export assembly for dependency '{Dependency}'", providerModuleId);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private IEvoScModule CreateModuleInstance(Type mainClass, SimpleInjector.Container moduleServices)
	{
		try
		{
			return (IEvoScModule)ActivatorUtilities.CreateInstance(moduleServices, mainClass);
		}
		catch (Exception innerException)
		{
			throw new InvalidOperationException("Failed to create module instance.", innerException);
		}
	}

	private Dictionary<PipelineType, IActionPipeline> CreateDefaultPipelines() => new()
	{
		[PipelineType.ChatRouter] = new ActionPipeline(),
		[PipelineType.ControllerAction] = new ActionPipeline()
	};

	[MethodImpl(MethodImplOptions.NoInlining)]
	private async Task<IModuleLoadContext> CreateModuleLoadContextAsync(Guid loadId, Type mainClass, AssemblyLoadContext? asmLoadContext, IModuleInfo moduleInfo)
	{
		IReadOnlyList<Assembly> assemblies = asmLoadContext is EvoScModuleLoadContext moduleAlc
			? moduleAlc.GetModuleAssemblies()
			: new Assembly[1] { mainClass.Assembly };
		string rootNamespace = mainClass.Namespace ?? throw new InvalidOperationException("Failed to detect root namespace for module.");
		List<Guid> loadedDependencies = [];
		var moduleServices = _servicesManager.NewContainer(loadId, assemblies, loadedDependencies);
		moduleServices.RegisterInstance(moduleInfo);

		// A module's own export assembly holds types the module declares itself, such as a
		// [PermissionGroup] enum it shares with other modules, so it takes part in the same
		// metadata scans as the module's own assemblies. It is deliberately left out of the
		// service registrations above: an export only contains contracts and data, and
		// registering those as services would be meaningless.
		IReadOnlyList<Assembly> declaredAssemblies = _exportAssemblies.TryGetLoadedExport(moduleInfo.Id, out var ownExport)
			? [.. assemblies, ownExport]
			: assemblies;

		var localization = GetModuleLocalization(mainClass.Assembly, rootNamespace, moduleInfo);

		if (localization != null)
		{
			moduleServices.RegisterInstance(typeof(ILocalizationManager), localization);
			moduleServices.Register<Locale, LocaleResource>(Lifestyle.Scoped);
		}

		var themes = GetModuleThemes(declaredAssemblies);

		// A module's own constructor may take services from a module it depends on, so its
		// container has to be connected to those dependencies before the instance exists.
		LinkLoadedDependencies(loadId, moduleInfo, loadedDependencies);

		await RegisterModuleConfigAsync(declaredAssemblies, moduleServices, moduleInfo);
		var moduleInstance = CreateModuleInstance(mainClass, moduleServices);
		return new ModuleLoadContext
		{
			Instance = moduleInstance,
			Services = moduleServices,
			AsmLoadContext = asmLoadContext,
			LoadId = loadId,
			MainClass = mainClass,
			ModuleInfo = moduleInfo,
			Assemblies = declaredAssemblies,
			Pipelines = CreateDefaultPipelines(),
			Permissions = new List<IPermission>(),
			LoadedDependencies = loadedDependencies,
			ManialinkTemplates = new List<IModuleManialinkTemplate>(),
			RootNamespace = rootNamespace,
			Themes = themes
		};
	}

	private IReadOnlyList<Type> GetModuleThemes(IEnumerable<Assembly> assemblies)
	{
		var themes = new List<Type>();
		foreach (Assembly assembly in assemblies)
		{
			themes.AddRange(assembly.AssemblyTypesWithAttribute<ThemeAttribute>());
		}
		return themes;
	}

	private ILocalizationManager? GetModuleLocalization(Assembly assembly, string rootNamespace, IModuleInfo moduleInfo)
	{
		try
		{
			var localization = new LocalizationManager(assembly, rootNamespace + ".Localization", _localizationLogger);
			_logger.LogDebug("Registered localization for module {Module}", moduleInfo.Id);
			return localization;
		}
		catch (Exception exception)
		{
			_logger.LogDebug(exception, "Localization not found for module {Module}", moduleInfo.Id);
		}
		return null;
	}

	/// <summary>
	/// Connects a module that is being created to the dependencies that are already loaded, so
	/// services of those modules can be resolved while the instance is constructed. A dependency
	/// that is not loaded yet is left to <see cref="LinkModuleAsync"/>, which runs once every
	/// module of the load operation exists.
	/// </summary>
	private void LinkLoadedDependencies(Guid loadId, IModuleInfo moduleInfo, List<Guid> loadedDependencies)
	{
		foreach (string dependencyId in moduleInfo.Dependencies.Select(dependency => dependency.Name))
		{
			if (!_moduleNameMap.TryGetValue(dependencyId, out Guid dependencyLoadId) ||
			    loadedDependencies.Contains(dependencyLoadId))
			{
				continue;
			}

			loadedDependencies.Add(dependencyLoadId);
			_servicesManager.RegisterDependency(loadId, dependencyLoadId);
			_logger.LogDebug("Module '{Name}' is linked to dependency '{Dependency}'", moduleInfo.Id, dependencyId);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private async Task<IModuleLoadContext?> RegisterModuleAsync(Guid loadId, IModuleInfo moduleInfo, Type mainClass, AssemblyLoadContext? asmLoadContext)
	{
		if (_moduleNameMap.ContainsKey(moduleInfo.Id))
		{
			_logger.LogError("A module with the identifier '{Name}' is already loaded. Will not load again", moduleInfo.Id);
			return null;
		}
		_logger.LogDebug("Loading module '{Name}' as load ID '{LoadId}'", moduleInfo.Id, loadId);
		IModuleLoadContext loadContext = await CreateModuleLoadContextAsync(loadId, mainClass, asmLoadContext, moduleInfo);

		_loadedModules.Add(loadId, loadContext);
		_moduleNameMap[moduleInfo.Id] = loadId;
		_logger.LogDebug("Module '{Name}' loaded with ID: {LoadId}", moduleInfo.Id, loadId);

		return loadContext;
	}

	/// <summary>
	/// Connects a loaded module to the modules it depends on and registers what it contributes.
	/// This runs after every module of a load operation has been loaded, which is what allows two
	/// modules to depend on each other: no module has to be loaded before another one, they only
	/// have to be linked once all of them exist.
	/// </summary>
	[MethodImpl(MethodImplOptions.NoInlining)]
	private async Task LinkModuleAsync(IModuleLoadContext moduleContext)
	{
		IModuleInfo moduleInfo = moduleContext.ModuleInfo;

		if (moduleContext.AsmLoadContext is EvoScModuleLoadContext)
		{
			EnsureExportDependenciesLoaded(moduleContext.LoadId, moduleInfo);
		}

		foreach (string dependencyId in moduleInfo.Dependencies.Select(dependency => dependency.Name))
		{
			if (!_moduleNameMap.TryGetValue(dependencyId, out Guid dependencyLoadId))
			{
				throw new DependencyNotFoundException(moduleInfo.Id, dependencyId);
			}

			// A dependency that was already available when the instance was created is linked then.
			if (moduleContext.LoadedDependencies.Contains(dependencyLoadId))
			{
				continue;
			}

			moduleContext.LoadedDependencies.Add(dependencyLoadId);
			_servicesManager.RegisterDependency(moduleContext.LoadId, dependencyLoadId);
			_logger.LogDebug("Module '{Name}' is linked to dependency '{Dependency}'", moduleInfo.Id, dependencyId);
		}

		await RegisterMiddlewaresAsync(moduleContext);
		await RegisterPermissionsAsync(moduleContext);
		await RegisterManialinksTemplatesAsync(moduleContext);
	}

	/// <summary>
	/// Undoes the registration of a module that could not be linked or installed, so a failed
	/// dependency does not leave a half-loaded module behind.
	/// </summary>
	private void RollbackModule(IModuleLoadContext moduleContext)
	{
		_loadedModules.Remove(moduleContext.LoadId);
		_moduleNameMap.Remove(moduleContext.ModuleInfo.Id);

		try
		{
			_servicesManager.RemoveContainer(moduleContext.LoadId);
		}
		catch (ServicesException exception)
		{
			_logger.LogWarning(exception, "Failed to remove service container for module {LoadId}", moduleContext.LoadId);
		}

		_exportAssemblies.ReleaseModule(moduleContext.LoadId);
		_logger.LogDebug("Rolled back module '{Name}' after a failed load", moduleContext.ModuleInfo.Id);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Task EnableAsync(Guid loadId) => EnableAsync(loadId, new HashSet<Guid>());

	private async Task EnableAsync(Guid loadId, HashSet<Guid> enabling)
	{
		// A module that is already being enabled further up the stack is a cyclic dependency:
		// it will be enabled once the enable it is waiting on has finished.
		if (!enabling.Add(loadId))
		{
			return;
		}

		try
		{
			IModuleLoadContext moduleContext = GetModule(loadId);
			if (moduleContext.IsEnabled)
			{
				return;
			}
			await EnableDependenciesAsync(moduleContext, enabling);
			await EnableThemesAsync(moduleContext);
			await EnableControllersAsync(moduleContext);
			await EnableMiddlewaresAsync(moduleContext);
			await EnableManialinkTemplatesAsync(moduleContext);
			await StartBackgroundServicesAsync(moduleContext);
			await TryCallModuleEnableAsync(moduleContext);
			moduleContext.SetEnabled(enabled: true);
			moduleContext.SetStatus(ModuleStatus.Enabled);
			_logger.LogDebug("Module {Type}({Module}) was enabled", moduleContext.MainClass, loadId);
		}
		finally
		{
			enabling.Remove(loadId);
		}
	}

	/// <summary>
	/// Enables the modules this one depends on. Dependencies are enabled instead of required to be
	/// enabled already, because two modules that depend on each other can never satisfy that.
	/// </summary>
	private async Task EnableDependenciesAsync(IModuleLoadContext moduleContext, HashSet<Guid> enabling)
	{
		foreach (Guid dependencyId in moduleContext.LoadedDependencies)
		{
			IModuleLoadContext dependency = GetModule(dependencyId);
			if (dependency.IsEnabled || enabling.Contains(dependencyId))
			{
				continue;
			}

			if (_config.Modules.DisabledModules.Contains(dependency.ModuleInfo.Id))
			{
				throw new EvoScModuleException($"Module '{moduleContext.ModuleInfo.Id}' cannot be enabled: dependency '{dependency.ModuleInfo.Id}' is disabled in the configuration.");
			}

			_logger.LogDebug("Enabling dependency '{Dependency}' of module '{Module}'", dependency.ModuleInfo.Id, moduleContext.ModuleInfo.Id);
			await EnableAsync(dependencyId, enabling);
		}
	}

	private async Task EnableThemesAsync(IModuleLoadContext moduleContext)
	{
		foreach (Type theme in moduleContext.Themes)
		{
			await _themeManager.AddThemeAsync(theme, moduleContext.LoadId);
		}
	}

	private async Task StartBackgroundServicesAsync(IModuleLoadContext moduleContext)
	{
		foreach (IBackgroundService service in moduleContext.Services.GetAllInstances<IBackgroundService>())
		{
			await service.StartAsync();
		}
	}

	public async Task EnableModulesAsync()
	{
		foreach (IModuleLoadContext module in GetLoadedModules())
		{
			if (_config.Modules.DisabledModules.Contains(module.ModuleInfo.Id))
			{
				_logger.LogDebug("Module {Name} is disabled", module.ModuleInfo.Id);
			}
			else
			{
				await EnableAsync(module.LoadId);
			}
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public async Task DisableAsync(Guid loadId)
	{
		IModuleLoadContext moduleContext = GetModule(loadId);
		await DisableManialinkTemplatesAsync(moduleContext);
		await DisableControllersAsync(moduleContext);
		await DisableMiddlewaresAsync(moduleContext);
		await DisableThemesAsync(moduleContext);
		await StopBackgroundServicesAsync(moduleContext);
		await TryCallModuleDisableAsync(moduleContext);
		moduleContext.SetEnabled(enabled: false);
		moduleContext.SetStatus(ModuleStatus.Disabled);
		_logger.LogDebug("Module {Type}({Module}) was disabled", moduleContext.MainClass, loadId);
	}

	private async Task DisableThemesAsync(IModuleLoadContext moduleContext)
	{
		await _themeManager.RemoveThemesForModuleAsync(moduleContext.LoadId);
	}

	private async Task StopBackgroundServicesAsync(IModuleLoadContext moduleContext)
	{
		foreach (IBackgroundService service in moduleContext.Services.GetAllInstances<IBackgroundService>())
		{
			await service.StopAsync();
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Task LoadAsync(string directory)
	{
		if (!Directory.Exists(directory))
		{
			throw new DirectoryNotFoundException("The module directory was not found at: " + directory);
		}
		IExternalModuleInfo moduleInfo = ModuleInfoUtils.CreateFromDirectory(new DirectoryInfo(directory));
		return LoadAsync(moduleInfo);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public async Task LoadAsync(IExternalModuleInfo moduleInfo, bool install = true)
	{
		if (!VerifyExternalModule(moduleInfo))
		{
			_logger.LogError("File signature verification failed for module {Name}. The module will not load", moduleInfo.Id);
			return;
		}
		Guid loadId = Guid.NewGuid();
		var (type, asmLoadContext) = CreateAssemblyLoadContext(loadId, moduleInfo);
		if (type != null)
		{
			ApplyModuleDeclaration(moduleInfo, type);
			await LoadAndLinkAsync(moduleInfo, type, asmLoadContext, loadId, install);
			return;
		}
		_logger.LogError("Failed to find the module main class for module {Name}. The module will not load", moduleInfo.Id);
	}

	/// <summary>
	/// Loads the modules that ship with EvoSC. They come from a module directory like any other
	/// module, but are picked by id from a list the application controls rather than by discovery,
	/// so that what is part of EvoSC is decided in code and a module missing from the deployment
	/// is reported instead of silently ignored.
	/// </summary>
	[MethodImpl(MethodImplOptions.NoInlining)]
	public async Task LoadInternalModulesAsync(IEnumerable<string> moduleIds, string directory)
	{
		if (!Directory.Exists(directory))
		{
			throw new DirectoryNotFoundException("The internal module directory was not found at: " + directory);
		}

		var available = ModuleDirectoryUtils.FindModulesIn(directory).ToDictionary(module => module.Id);
		var internalModules = new SortedModuleCollection<IExternalModuleInfo>();
		var missing = new List<string>();

		foreach (string moduleId in moduleIds)
		{
			if (!available.TryGetValue(moduleId, out var moduleInfo))
			{
				missing.Add(moduleId);
				continue;
			}

			internalModules.Add(new ExternalModuleInfo
			{
				Id = moduleInfo.Id,
				Name = moduleInfo.Name,
				Summary = moduleInfo.Summary,
				Version = moduleInfo.Version,
				Author = moduleInfo.Author,
				Dependencies = moduleInfo.Dependencies,
				Directory = moduleInfo.Directory,
				ModuleFiles = moduleInfo.ModuleFiles,
				IsInternal = true
			});
		}

		if (missing.Count > 0)
		{
			throw new EvoScModuleException(
				$"The following internal modules were not found in '{directory}': {string.Join(", ", missing)}");
		}

		if (internalModules.Count == 0)
		{
			return;
		}

		await LoadAsync(internalModules);
	}

	/// <summary>
	/// Loads a set of modules in phases: every load context is created first, then every module is
	/// loaded, then they are linked to each other and installed. Dependencies therefore do not
	/// dictate the order modules are loaded in, and modules may depend on each other in a cycle.
	/// </summary>
	[MethodImpl(MethodImplOptions.NoInlining)]
	public async Task LoadAsync(IModuleCollection<IExternalModuleInfo> collection)
	{
		var prepared = new List<(IExternalModuleInfo ModuleInfo, Type Type, AssemblyLoadContext? AsmLoadContext, Guid LoadId)>();

		// Create all load contexts up front so that every module's export assembly is registered
		// before any module type is loaded.
		foreach (IExternalModuleInfo module in collection)
		{
			if (!VerifyExternalModule(module))
			{
				_logger.LogError("File signature verification failed for module {Name}. The module will not load", module.Id);
				continue;
			}
			Guid loadId = Guid.NewGuid();
			var (type, asmLoadContext) = CreateAssemblyLoadContext(loadId, module);
			if (type == null)
			{
				_logger.LogError("Failed to find the module main class for module {Name}. The module will not load", module.Id);
				continue;
			}
			ApplyModuleDeclaration(module, type);
			prepared.Add((module, type, asmLoadContext, loadId));
		}

		var loaded = new List<IModuleLoadContext>();
		foreach (var (moduleInfo, type, asmLoadContext, loadId) in prepared)
		{
			IModuleLoadContext? loadContext = await RegisterModuleAsync(loadId, moduleInfo, type, asmLoadContext);
			if (loadContext != null)
			{
				loaded.Add(loadContext);
			}
		}

		foreach (IModuleLoadContext loadContext in loaded)
		{
			try
			{
				await LinkModuleAsync(loadContext);
			}
			catch
			{
				// A dependency that could not be resolved fails the whole load operation, but none
				// of the modules it registered are left behind.
				foreach (IModuleLoadContext registered in loaded)
				{
					RollbackModule(registered);
				}
				throw;
			}
		}

		foreach (IModuleLoadContext loadContext in loaded)
		{
			await InstallAsync(loadContext.LoadId);
		}
	}

	private async Task LoadAndLinkAsync(IModuleInfo moduleInfo, Type type, AssemblyLoadContext? asmLoadContext, Guid loadId, bool install = true)
	{
		IModuleLoadContext? loadContext = await RegisterModuleAsync(loadId, moduleInfo, type, asmLoadContext);

		if (loadContext == null)
		{
			return;
		}

		try
		{
			await LinkModuleAsync(loadContext);

			if (install)
			{
				await InstallAsync(loadId);
			}
		}
		catch
		{
			RollbackModule(loadContext);
			throw;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private async Task<(WeakReference Instance, WeakReference? LoadContext)> UnloadInternalAsync(Guid loadId)
	{
		IModuleLoadContext moduleContext = GetModule(loadId);
		if (moduleContext.ModuleInfo.IsInternal)
		{
			throw new EvoScModuleException($"Attempted to unload internal module '{loadId}' but this is not allowed");
		}
		var dependentModules = GetLoadedModules()
			.Where(m => m.LoadedDependencies.Contains(loadId))
			.Select(m => m.LoadId)
			.ToArray();

		foreach (var dependentLoadId in dependentModules)
		{
			await UnloadAsync(dependentLoadId);
		}
		if (moduleContext.IsEnabled)
		{
			await DisableAsync(loadId);
		}
		WeakReference instanceWeakRef = new WeakReference(moduleContext.Instance);
		await DisposeModuleInstanceAsync(moduleContext);
		try
		{
			_servicesManager.RemoveContainer(loadId);
		}
		catch (ServicesException exception)
		{
			_logger.LogWarning(exception, "Failed to remove service container for module {LoadId}", loadId);
		}
		_loadedModules.Remove(loadId);
		_moduleNameMap.Remove(moduleContext.ModuleInfo.Id);
		_exportAssemblies.ReleaseModule(loadId);
		GC.AddMemoryPressure(50000000L);
		WeakReference? alcWeakRef = DetachLoadContext(moduleContext.AsmLoadContext);
		return (Instance: instanceWeakRef, LoadContext: alcWeakRef);
	}

	private static async Task DisposeModuleInstanceAsync(IModuleLoadContext moduleContext)
	{
		IEvoScModule? instance = moduleContext.Instance;
		if (instance is IAsyncDisposable asyncDisposable)
		{
			await asyncDisposable.DisposeAsync();
			return;
		}
		if (instance is IDisposable disposable)
		{
			disposable.Dispose();
		}
	}

	private static WeakReference? DetachLoadContext(AssemblyLoadContext? loadContext)
	{
		if (loadContext is not EvoScModuleLoadContext evoScModuleLoadContext)
		{
			return null;
		}
		WeakReference result = new WeakReference(evoScModuleLoadContext);
		evoScModuleLoadContext.Unload();
		return result;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public async Task UnloadAsync(Guid loadId)
	{
		var (instanceWeakRef, alcWeakRef) = await UnloadInternalAsync(loadId);
		CollectibleLoadContext.WaitForUnload(instanceWeakRef);
		if (instanceWeakRef.IsAlive)
		{
			_logger.LogWarning("Some references for module '{LoadId}' are still alive", loadId);
		}
		if (alcWeakRef != null)
		{
			CollectibleLoadContext.WaitForUnload(alcWeakRef);
			if (alcWeakRef.IsAlive)
			{
				_logger.LogWarning("The load context for module '{LoadId}' is still alive after unload", loadId);
			}
		}
		_logger.LogDebug("Module '{LoadId}' was unloaded", loadId);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public async Task ReloadAsync(Guid loadId)
	{
		IModuleLoadContext moduleContext = GetModule(loadId);
		if (moduleContext.ModuleInfo.IsInternal)
		{
			throw new EvoScModuleException($"Attempted to reload internal module '{loadId}' but this is not allowed");
		}
		DirectoryInfo directory = ((IExternalModuleInfo)moduleContext.ModuleInfo).Directory;
		_logger.LogDebug("Reloading module '{Name}' ({LoadId})", moduleContext.ModuleInfo.Id, loadId);
		await UnloadAsync(loadId);
		await LoadAsync(directory.FullName);
	}
}
