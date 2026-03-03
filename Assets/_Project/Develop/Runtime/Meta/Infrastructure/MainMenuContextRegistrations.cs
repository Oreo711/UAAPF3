using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.UI;
using _Project.Develop.Runtime.UI.Core;
using _Project.Develop.Runtime.UI.MainMenu;
using _Project.Develop.Runtime.Utilities.AssetsManagement;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using _Project.Develop.Runtime.Utilities.SceneManagement;
using UnityEngine;


namespace _Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuContextRegistrations
    {
        public static void Process(DIContainer container)
        {
            container.RegisterAsSingle(CreateGameModeSelectionService);
            container.RegisterAsSingle(CreateMainMenuUIRoot).NonLazy();
            container.RegisterAsSingle(CreateMainMenuPresentersFactory);
            container.RegisterAsSingle(CreateMainMenuScreenPresenter).NonLazy();
            container.RegisterAsSingle(CreateMainMenuPopupService);
        }

        private static MainMenuPopupService CreateMainMenuPopupService(DIContainer c)
        {
            return new MainMenuPopupService(
                c.Resolve<ViewsFactory>(),
                c.Resolve<ProjectPresentersFactory>(),
                c.Resolve<MainMenuUIRoot>());
        }

        private static GameModeSelectionService CreateGameModeSelectionService (DIContainer c)
        {
            return new GameModeSelectionService(c.Resolve<ICoroutinePerformer>(), c.Resolve<SceneSwitcherService>());
        }

        private static MainMenuUIRoot CreateMainMenuUIRoot(DIContainer c)
        {
            ResourcesAssetsLoader resourcesAssetsLoader = c.Resolve<ResourcesAssetsLoader>();

            MainMenuUIRoot mainMenuUIRootPrefab = resourcesAssetsLoader
                .Load<MainMenuUIRoot>("UI/MainMenu/MainMenuUIRoot");

            return Object.Instantiate(mainMenuUIRootPrefab);
        }

        private static MainMenuPresentersFactory CreateMainMenuPresentersFactory(DIContainer c)
        {
            return new MainMenuPresentersFactory(c);
        }

        private static MainMenuScreenPresenter CreateMainMenuScreenPresenter(DIContainer c)
        {
            MainMenuUIRoot uiRoot = c.Resolve<MainMenuUIRoot>();

            MainMenuScreenView view = c
                                      .Resolve<ViewsFactory>()
                                      .Create<MainMenuScreenView>(ViewIDs.MainMenuScreen, uiRoot.HUDLayer);

            MainMenuScreenPresenter presenter = c
                                                .Resolve<MainMenuPresentersFactory>()
                                                .CreateMainMenuScreen(view);

            return presenter;
        }
    }
}
