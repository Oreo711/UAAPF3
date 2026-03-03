using System;
using System.Collections;
using _Project.Develop.Runtime.Gameplay.Configs;
using _Project.Develop.Runtime.Infrastructure;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Meta.Features.Stats;
using _Project.Develop.Runtime.Meta.Features.Wallet;
using _Project.Develop.Runtime.Utilities.ConfigsManagement;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using _Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using _Project.Develop.Runtime.Utilities.SceneManagement;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Infrastructure
{
	public class GameplayBootstrap : SceneBootstrap
	{
		[SerializeField] private Gameplay _gameplay;

		private DIContainer       _container;
		private GameplayInputArgs _inputArgs;

		public override void ProcessRegistrations (DIContainer container, IInputSceneArgs sceneArgs = null)
		{
			_container = container;

			if (sceneArgs is not GameplayInputArgs gameplayInputArgs)
				throw new ArgumentException($"{nameof(sceneArgs)} does not match with {typeof(GameplayInputArgs)} type");

			_inputArgs = gameplayInputArgs;

			GameplayContextRegistrations.Process(_container, _inputArgs);
		}

		public override IEnumerator Initialize ()
		{
			ConfigProviderService configProvider = _container.Resolve<ConfigProviderService>();

			GameplayConfig config = configProvider.GetConfig<GameplayConfig>();

			_gameplay.Initialize(
				_inputArgs.GameMode,
				config,
				_container.Resolve<SceneSwitcherService>(),
				_container.Resolve<ICoroutinePerformer>(),
				_container.Resolve<WalletService>(),
				_container.Resolve<StatsService>(),
				_container.Resolve<PlayerDataProvider>());
			_gameplay.Setup();

			yield break;
		}

		public override void Run () {}
	}
}
