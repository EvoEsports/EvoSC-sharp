using System.IO;
using System.Text.Json;
using DefaultEcs;
using EvoSC.Core.Configuration;
using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection.Dependencies;
using GameHost.V3.IO.Storage;

namespace EvoSC.Core.Systems
{
    /// <summary>
    /// Create configuration from files that can be accessed via <see cref="World.Get{T}()"/>
    /// </summary>
    public class SpawnConfigurationSystem : AppSystem
    {
        private World _world;
        private IStorage _configStorage;

        public SpawnConfigurationSystem(Scope scope) : base(scope)
        {
            Dependencies.AddRef(() => ref _world);
            Dependencies.AddRef(() => ref _configStorage);
        }

        protected override void OnInit()
        {
            // see bottom for questions
            Load<ServerConnectionConfig>("server.json", true);
            Load<ModuleListConfig>("modules.json", false);
        }

        private bool Load<T>(string path, bool required)
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

            var result = JsonSerializer.Deserialize<T>(byteList.Span);
            _world.Set(result);

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
