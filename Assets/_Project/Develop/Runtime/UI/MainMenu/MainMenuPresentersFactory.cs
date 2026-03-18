using _Project.Develop.Runtime.Configs.Meta;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Meta.Features.Stats;
using _Project.Develop.Runtime.Meta.Features.Wallet;
using _Project.Develop.Runtime.Utilities.ConfigsManagement;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using _Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using _Project.Develop.Runtime.Utilities.SceneManagement;


namespace _Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuPresentersFactory
    {
        private readonly DIContainer _container;

        public MainMenuPresentersFactory(DIContainer container)
        {
            _container = container;
        }

        public MainMenuScreenPresenter CreateMainMenuScreen(MainMenuScreenView view)
        {
            MainMenuScreenPresenter presenter = new MainMenuScreenPresenter(
                view,
                _container.Resolve<ProjectPresentersFactory>(),
                _container.Resolve<SceneSwitcherService>(),
                _container.Resolve<ICoroutinePerformer>(),
                _container.Resolve<WalletService>(),
                _container.Resolve<StatsService>(),
                _container.Resolve<ConfigProviderService>()
                          .GetConfig<StatsResetCostConfig>(),
                _container.Resolve<PlayerDataProvider>());

            presenter.Initialize();

            return presenter;
        }
    }
}
