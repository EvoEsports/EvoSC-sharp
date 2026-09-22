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

	private readonly ExportAssemblyStore _exportAssemblies = new ExportAssemblyStore();

	public IReadOnlyList<IModuleLoadContext> LoadedModules => _loadedModules.Values.ToList();

	internal ExportAssemblyStore ExportAssemblies => _exportAssemblies;

	public ModuleManager(ILogger<ModuleManager> logger, IEvoScBaseConfig config, IControllerManager controllers, IServiceContainerManager servicesManager, IActionPipelineManager pipelineManager, IPermissionManager permissions, IConfigStoreRepository configStoreRepository, IManialinkManager manialinkManager, IThemeManager themeManager)
	{
		_logger = logger;
		_config = config;
		_controllers = controllers;
		_servicesManager = servicesManager;
		_pipelineManager = pipelineManager;
		_permissions = permissions;
		_configStoreRepository = configStoreRepository;
		_manialinkManager = manialinkManager;
		_themeManager = themeManager;
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
		if (loadId == Guid.Empty || !_loadedModules.ContainsKey(loadId))
		{
			throw new EvoScModuleException($"Module with Id {loadId} does not exist.");
		}
		return _loadedModules[loadId];
	}

	private Task RegisterPermissionsAsync(IModuleLoadContext loadContext)
	{
		foreach (Assembly assembly in loadContext.Assemblies)
		{
			foreach (Type item in assembly.AssemblyTypesWithAttribute<PermissionGroupAttribute>())
			{
				string name = item.Name;
				IdentifierAttribute customAttribute = item.GetCustomAttribute<IdentifierAttribute>();
				if (customAttribute != null)
				{
					name = customAttribute.Name;
				}
				FieldInfo[] fields = item.GetFields();
				foreach (FieldInfo fieldInfo in fields)
				{
					if (!(fieldInfo.FieldType != item))
					{
						string text = fieldInfo.GetCustomAttribute<IdentifierAttribute>()?.Name ?? fieldInfo.Name;
						loadContext.Permissions.Add(new Permission
						{
							Name = name + "." + text,
							Description = (fieldInfo.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "")
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
			IPermission existingPermission = await _permissions.GetPermissionAsync(permission.Name);
			if (existingPermission != null)
			{
				_logger.LogDebug("Wont install permission '{Name}' as it already exists", permission.Name);
				identifiedPermissions.Add(existingPermission);
				continue;
			}
			_logger.LogDebug("Installing permission: {Name}", permission.Name);
			await _permissions.AddPermissionAsync(permission);
			IPermission identifiedPermission = await _permissions.GetPermissionAsync(permission.Name);
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
				MiddlewareAttribute customAttribute = item.GetCustomAttribute<MiddlewareAttribute>();
				moduleContext.Pipelines[customAttribute.For].AddComponent(item, moduleContext.Services);
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
				Stream resourceStream = assembly.GetManifestResourceStream(resourceName);
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
		int i;
		for (i = 0; i < namespaceParts.Length && nameComponents[i].Equals(namespaceParts[i], StringComparison.Ordinal); i++)
		{
		}
		if (nameComponents[i].Equals("Templates", StringComparison.Ordinal))
		{
			i++;
		}
		return loadContext.ModuleInfo.Name + "." + string.Join(".", nameComponents[i..^1]);
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

	private Task DisableManialinkTemplatesAsync(IModuleLoadContext moduleContext)
	{
		foreach (IModuleManialinkTemplate manialinkTemplate in moduleContext.ManialinkTemplates)
		{
			switch (manialinkTemplate.Type)
			{
			case ManialinkTemplateType.Script:
				_manialinkManager.RemoveManiaScript(manialinkTemplate.Name);
				break;
			case ManialinkTemplateType.Template:
				_manialinkManager.RemoveAndHideTemplateAsync(manialinkTemplate.Name);
				break;
			}
		}
		return Task.CompletedTask;
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
					ControllerAttribute customAttribute = item.GetCustomAttribute<ControllerAttribute>();
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
		try
		{
			foreach (Assembly assembly in assemblies)
			{
				foreach (Type type in assembly.AssemblyTypesWithAttribute<SettingsAttribute>())
				{
					SettingsAttribute configAttr = type.GetCustomAttribute<SettingsAttribute>();
					if (configAttr != null)
					{
						if (!type.IsInterface)
						{
							_logger.LogError("Settings type {Type} must be an interface", type);
							throw new ServicesException($"Settings type {type} must be an interface.");
						}
						object config = CreateConfigInstance(type, await CreateModuleConfigStoreAsync(moduleInfo.Name, type));
						if (config == null)
						{
							_logger.LogError("An instance of the module config {Type} could not be created", type);
							throw new InvalidOperationException("Failed to create module config instance.");
						}
						container.RegisterInstance(type, config);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			_logger.LogError(ex2, "Failed to add module config");
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
		object obj = ReflectionUtils.CreateGenericInstance(typeof(ConfigurationBuilder<>), configInterface);
		if (obj == null)
		{
			throw new InvalidOperationException("Failed to create module config builder.");
		}
		ReflectionUtils.CallMethod(obj, "UseConfigStore", store);
		ReflectionUtils.CallMethod(obj, "UseTypeParser", new TextColorTypeParser());
		ReflectionUtils.CallMethod(obj, "UseTypeParser", new VersionParser());
		return ReflectionUtils.CallMethod(obj, "Build");
	}

	private bool VerifyExternalModule(IExternalModuleInfo moduleInfo)
	{
		return !_config.Modules.RequireSignatureVerification || moduleInfo.ModuleFiles.All((IModuleFile file) => file.VerifySignature());
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private (Type?, AssemblyLoadContext?) CreateAssemblyLoadContext(Guid loadId, IExternalModuleInfo moduleInfo)
	{
		string[] array = moduleInfo.AssemblyFiles.Select((IModuleFile f) => f.File.FullName).ToArray();
		string[] array2 = array.Where((string f) => Path.GetFileNameWithoutExtension(f).EndsWith(".Exports", StringComparison.OrdinalIgnoreCase)).ToArray();
		string[] array3 = array.Except<string>(array2, StringComparer.OrdinalIgnoreCase).ToArray();
		if (array3.Length == 0)
		{
			_logger.LogError("No assemblies found in module directory for '{Name}'. The module will not load", moduleInfo.Name);
			return (null, null);
		}
		string[] array4 = array2;
		foreach (string text in array4)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
			_exportAssemblies.RegisterExportPath(fileNameWithoutExtension, text);
			_exportAssemblies.AcquireExportForModule(loadId, fileNameWithoutExtension);
			_logger.LogDebug("Registered export assembly '{Export}' for module {Module}", fileNameWithoutExtension, moduleInfo.Name);
		}
		EvoScModuleLoadContext evoScModuleLoadContext = new EvoScModuleLoadContext(array3[0], (string name) => _exportAssemblies.ResolveExportForModule(loadId, name));
		Type type = null;
		string[] array5 = array3;
		foreach (string path in array5)
		{
			Assembly assembly = evoScModuleLoadContext.LoadModuleAssembly(path);
			if ((object)type == null)
			{
				type = assembly.AssemblyTypesWithAttribute<ModuleAttribute>().FirstOrDefault();
			}
		}
		return (type, evoScModuleLoadContext);
	}

	private IModuleLoadContext? GetLoadedDependency(IModuleDependency dependency)
	{
		IModuleLoadContext moduleLoadContext = _loadedModules.Values.FirstOrDefault((IModuleLoadContext m) => m.ModuleInfo.Name.Equals(dependency.Name));
		if (moduleLoadContext == null)
		{
			throw new InvalidOperationException("Tried to get module " + dependency.Name + " a loaded dependency, but it is not loaded.");
		}
		return moduleLoadContext;
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

	private Dictionary<PipelineType, IActionPipeline> CreateDefaultPipelines()
	{
		return new Dictionary<PipelineType, IActionPipeline>
		{
			{
				PipelineType.ChatRouter,
				new ActionPipeline()
			},
			{
				PipelineType.ControllerAction,
				new ActionPipeline()
			}
		};
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private async Task<IModuleLoadContext> CreateModuleLoadContextAsync(Guid loadId, Type mainClass, AssemblyLoadContext? asmLoadContext, IModuleInfo moduleInfo)
	{
		IReadOnlyList<Assembly> assemblies = asmLoadContext is EvoScModuleLoadContext moduleAlc
			? moduleAlc.ModuleAssemblies
			: new Assembly[1] { mainClass.Assembly };
		string rootNamespace = mainClass.Namespace ?? throw new InvalidOperationException("Failed to detect root namespace for module.");
		List<Guid> loadedDependencies = GetLoadedDependencies(moduleInfo);
		var moduleServices = _servicesManager.NewContainer(loadId, assemblies, loadedDependencies);
		moduleServices.RegisterInstance(moduleInfo);

		var localization = GetModuleLocalization(mainClass.Assembly, rootNamespace, moduleInfo);

		if (localization != null)
		{
			moduleServices.RegisterInstance(typeof(ILocalizationManager), localization);
			moduleServices.Register<Locale, LocaleResource>(Lifestyle.Scoped);
		}

		var themes = GetModuleThemes(assemblies);

		await RegisterModuleConfigAsync(assemblies, moduleServices, moduleInfo);
		var moduleInstance = CreateModuleInstance(mainClass, moduleServices);
		return new ModuleLoadContext
		{
			Instance = moduleInstance,
			Services = moduleServices,
			AsmLoadContext = asmLoadContext,
			LoadId = loadId,
			MainClass = mainClass,
			ModuleInfo = moduleInfo,
			Assemblies = assemblies,
			Pipelines = CreateDefaultPipelines(),
			Permissions = new List<IPermission>(),
			LoadedDependencies = loadedDependencies,
			ManialinkTemplates = new List<IModuleManialinkTemplate>(),
			RootNamespace = rootNamespace,
			Localization = localization,
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
			var localization = new LocalizationManager(assembly, rootNamespace + ".Localization");
			_logger.LogDebug("Registered localization for module {Module}", moduleInfo.Name);
			return localization;
		}
		catch (Exception exception)
		{
			_logger.LogDebug(exception, "Localization not found for module {Module}", moduleInfo.Name);
		}
		return null;
	}

	private List<Guid> GetLoadedDependencies(IModuleInfo moduleInfo)
	{
		var dependencies = new List<Guid>();
		foreach (IModuleDependency dependency in moduleInfo.Dependencies)
		{
			IModuleLoadContext loadedDependency = GetLoadedDependency(dependency);
			dependencies.Add(loadedDependency.LoadId);
		}
		return dependencies;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private async Task LoadInternalAsync(Guid loadId, IModuleInfo moduleInfo, Type mainClass, AssemblyLoadContext? asmLoadContext)
	{
		if (_moduleNameMap.ContainsKey(moduleInfo.Name))
		{
			_logger.LogError("A module with the identifier '{Name}' is already loaded. Will not load again", moduleInfo.Name);
			return;
		}
		_logger.LogDebug("Loading module '{Name}' as load ID '{LoadId}'", moduleInfo.Name, loadId);
		IModuleLoadContext loadContext = await CreateModuleLoadContextAsync(loadId, mainClass, asmLoadContext, moduleInfo);
		await RegisterMiddlewaresAsync(loadContext);
		await RegisterPermissionsAsync(loadContext);
		await RegisterManialinksTemplatesAsync(loadContext);

		_loadedModules.Add(loadId, loadContext);
		_moduleNameMap[moduleInfo.Name] = loadId;
		_logger.LogDebug("External Module '{Name}' loaded with ID: {LoadId}", moduleInfo.Name, loadId);

		await InstallAsync(loadId);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public async Task EnableAsync(Guid loadId)
	{
		IModuleLoadContext moduleContext = GetModule(loadId);
		await EnsureDependenciesEnabledAsync(moduleContext);
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

	private async Task EnsureDependenciesEnabledAsync(IModuleLoadContext moduleContext)
	{
		foreach (Guid dependencyId in moduleContext.LoadedDependencies)
		{
			IModuleLoadContext dependency = GetModule(dependencyId);
			if (!dependency.IsEnabled)
			{
				throw new EvoScModuleException($"Module '{moduleContext.ModuleInfo.Name}' cannot be enabled: dependency '{dependency.ModuleInfo.Name}' is not enabled.");
			}
		}
		await Task.CompletedTask;
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
		foreach (IModuleLoadContext module in LoadedModules)
		{
			if (_config.Modules.DisabledModules.Contains(module.ModuleInfo.Name))
			{
				_logger.LogDebug("Module {Name} is disabled", module.ModuleInfo.Name);
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
	public Task LoadAsync(IExternalModuleInfo moduleInfo)
	{
		if (!VerifyExternalModule(moduleInfo))
		{
			_logger.LogError("File signature verification failed for module {Name}. The module will not load", moduleInfo.Name);
			return Task.CompletedTask;
		}
		Guid loadId = Guid.NewGuid();
		var (type, asmLoadContext) = CreateAssemblyLoadContext(loadId, moduleInfo);
		if (type != null)
		{
			return LoadInternalAsync(loadId, moduleInfo, type, asmLoadContext);
		}
		_logger.LogError("Failed to find the module main class for module {Name}. The module will not load", moduleInfo.Name);
		return Task.CompletedTask;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Task LoadAsync(Assembly assembly)
	{
		IInternalModuleInfo internalModuleInfo = ModuleInfoUtils.CreateFromAssembly(assembly);
		Guid loadId = Guid.NewGuid();
		Type type = internalModuleInfo.Assembly.AssemblyTypesWithAttribute<ModuleAttribute>().FirstOrDefault();
		if (type != null)
		{
			return LoadInternalAsync(loadId, internalModuleInfo, type, null);
		}
		_logger.LogError("Failed to find the module main class for module {Name}. The module will not load", internalModuleInfo.Name);
		return Task.CompletedTask;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public async Task LoadAsync(IModuleCollection<IExternalModuleInfo> collection)
	{
		foreach (IExternalModuleInfo module in collection)
		{
			await LoadAsync(module);
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
		foreach (IModuleLoadContext module in LoadedModules)
		{
			if (module.LoadedDependencies.Any((Guid d) => d == loadId))
			{
				await UnloadAsync(module.LoadId);
			}
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
		_moduleNameMap.Remove(moduleContext.ModuleInfo.Name);
		_exportAssemblies.ReleaseModule(loadId);
		GC.AddMemoryPressure(50000000L);
		WeakReference alcWeakRef = DetachLoadContext(moduleContext.AsmLoadContext);
		return (Instance: instanceWeakRef, LoadContext: alcWeakRef);
	}

	private static async Task DisposeModuleInstanceAsync(IModuleLoadContext moduleContext)
	{
		IEvoScModule instance = moduleContext.Instance;
		if (instance is IAsyncDisposable asyncDisposable)
		{
			await asyncDisposable.DisposeAsync();
			return;
		}
		instance = moduleContext.Instance;
		if (instance is IDisposable disposable)
		{
			disposable.Dispose();
		}
	}

	private static WeakReference? DetachLoadContext(AssemblyLoadContext? loadContext)
	{
		if (!(loadContext is EvoScModuleLoadContext evoScModuleLoadContext))
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
		ForceCollection(instanceWeakRef);
		if (instanceWeakRef.IsAlive)
		{
			_logger.LogWarning("Some references for module '{LoadId}' are still alive", loadId);
		}
		if (alcWeakRef != null)
		{
			ForceCollection(alcWeakRef);
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
		_logger.LogDebug("Reloading module '{Name}' ({LoadId})", moduleContext.ModuleInfo.Name, loadId);
		await UnloadAsync(loadId);
		await LoadAsync(directory.FullName);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ForceCollection(WeakReference? weak)
	{
		if (weak == null)
		{
			return;
		}
		for (int i = 0; i < 10; i++)
		{
			if (!weak.IsAlive)
			{
				break;
			}
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
	}
}
