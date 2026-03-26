using _Project.Develop.Runtime.Gameplay.Configs;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Meta.Features.Stats;
using _Project.Develop.Runtime.Meta.Features.Wallet;
using _Project.Develop.Runtime.UI;
using _Project.Develop.Runtime.UI.Core;
using _Project.Develop.Runtime.UI.Gameplay;
using _Project.Develop.Runtime.Utilities.AssetsManagement;
using _Project.Develop.Runtime.Utilities.ConfigsManagement;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using _Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using _Project.Develop.Runtime.Utilities.SceneManagement;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayContextRegistrations
    {
        public static void Process(DIContainer container, GameplayInputArgs args)
        {
            container.RegisterAsSingle(CreateGameplay);
            container.RegisterAsSingle(CreateGameplayUIRoot);
            container.RegisterAsSingle(CreateGameplayPresentersFactory);
            container.RegisterAsSingle(CreateGameplayScreenPresenter).NonLazy();
            container.RegisterAsSingle(CreateGameplayPopupService);
        }

        private static GameplayProcess CreateGameplay (DIContainer c)
        {
            GameplayProcess gameplayProcess = new GameplayProcess(
                c.Resolve<ConfigProviderService>().GetConfig<GameplayConfig>(),
                c.Resolve<SceneSwitcherService>(),
                c.Resolve<ICoroutinePerformer>(),
                c.Resolve<WalletService>(),
                c.Resolve<StatsService>(),
                c.Resolve<PlayerDataProvider>(),
                c.Resolve<GameplayPopupService>());

            return gameplayProcess;
        }

        private static GameplayUIRoot CreateGameplayUIRoot(DIContainer c)
        {
            ResourcesAssetsLoader assetLoader = c.Resolve<ResourcesAssetsLoader>();

            GameplayUIRoot uiRootPrefab = assetLoader.Load<GameplayUIRoot>("UI/Gameplay/GameplayUIRoot");

            return Object.Instantiate(uiRootPrefab);
        }

        private static GameplayPresentersFactory CreateGameplayPresentersFactory(DIContainer c)
        {
            return new GameplayPresentersFactory(c);
        }

        private static GameplayScreenPresenter CreateGameplayScreenPresenter(DIContainer c)
        {
            GameplayUIRoot uiRoot = c.Resolve<GameplayUIRoot>();

            GameplayScreenView view = c.Resolve<ViewsFactory>()
                                       .Create<GameplayScreenView>(ViewIDs.GameplayScreen, uiRoot.HUDLayer);

            GameplayScreenPresenter presenter = c.Resolve<GameplayPresentersFactory>()
                                                 .CreateGameplayScreen(view);
            return presenter;
        }

        private static GameplayPopupService CreateGameplayPopupService (DIContainer c)
        {
            GameplayPopupService popupService = new GameplayPopupService(
                c.Resolve<ViewsFactory>(),
                c.Resolve<ProjectPresentersFactory>(),
                c.Resolve<GameplayPresentersFactory>(),
                c.Resolve<GameplayUIRoot>());

            return popupService;
        }
    }
}
