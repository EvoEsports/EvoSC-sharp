using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using DefaultEcs;
using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection;
using GameHost.V3.Injection.Dependencies;
using GameHost.V3.IO.Storage;

namespace EvoSC.Core.Systems
{
    /// <summary>
    /// Create configuration from files that can be accessed via <see cref="HostRunnerScope.Context"/>
    /// </summary>
    public class SpawnConfigurationSystem : AppSystem
    {
        private World _world;
        private IStorage _configStorage;

        private readonly HostRunnerScope _hostScope;
        
        public SpawnConfigurationSystem(Scope scope) : base(scope)
        {
            if (!scope.Context.TryGet(out _hostScope))
                throw new InvalidOperationException($"Expected to have {nameof(HostRunnerScope)}");
            
            _hostScope.Context.Register(this);

            Dependencies.AddRef(() => ref _world);
            Dependencies.AddRef(() => ref _configStorage);
        }

        protected override void OnInit()
        {
            // All JSON struct must be referenced in JsonContext.cs
            var context = JsonContext.Default;

            // see bottom for questions
            Load("server.json", context.ServerConnectionConfig, true);
            Load("modules.json", context.ModuleListConfig, false);
            Load("config.json", context.ControllerConfig, false);
        }

        private bool Load<T>(string path, JsonTypeInfo<T> typeInfo, bool required)
        {
            using var fileList = _configStorage.GetPooledFiles(path);
            if (fileList.Count == 0)
            {
                if (required)
                {
                    throw new FileNotFoundException(
                        $"Configuration '{typeof(T).Name}' file {_configStorage.CurrentPath}/{path} not found."
                    );
                }

                return false;
            }

            // Get from the latest file
            using var byteList = fileList[^1].GetPooledBytes();

            var result = JsonSerializer.Deserialize(byteList.Span, typeInfo);
            _hostScope.Context.Register(result);

            return true;
        }

        // QUESTIONS
        //
        // =========================================================================================
        // "server.json"
        // =========================================================================================
        // If it don't exist, should we throw?
        // should we create a default configuration at runtime?
        // should the file be already be included when you download the controller?
        //
    }
}
