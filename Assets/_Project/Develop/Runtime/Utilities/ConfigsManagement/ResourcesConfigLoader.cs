using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Develop.Runtime.Configs.Meta.Wallet;
using _Project.Develop.Runtime.Gameplay.Configs;
using _Project.Develop.Runtime.Utilities.AssetsManagement;
using UnityEngine;


namespace _Project.Develop.Runtime.Utilities.ConfigsManagement
{
    public class ResourcesConfigLoader : IConfigLoader
    {
        private readonly ResourcesAssetsLoader _resources;

        private readonly Dictionary<Type, string> _configsResourcesPaths = new()
        {
            {typeof(StartWalletConfig), "Configs/Meta/Wallet/StartWalletConfig" },
            {typeof(GameplayConfig), "Configs/Gameplay/GameplayConfig"},
            {typeof(CurrencyIconsConfig), "Configs/Meta/Wallet/CurrencyIconsConfig"}
        };

        public ResourcesConfigLoader(ResourcesAssetsLoader resources)
        {
            _resources = resources;
        }

        public IEnumerator LoadAsync(Action<Dictionary<Type, object>> onConfigsLoaded)
        {
            Dictionary<Type, object> loadedConfigs = new();

            foreach (KeyValuePair<Type, string> configResourcesPath in _configsResourcesPaths)
            {
                ScriptableObject config = _resources.Load<ScriptableObject>(configResourcesPath.Value);
                loadedConfigs.Add(configResourcesPath.Key, config);
                yield return null;
            }

            onConfigsLoaded?.Invoke(loadedConfigs);
        }
    }
}
