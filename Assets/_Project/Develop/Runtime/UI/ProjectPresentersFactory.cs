using _Project.Develop.Runtime.Configs.Meta.Wallet;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Meta.Features.Stats;
using _Project.Develop.Runtime.Meta.Features.Wallet;
using _Project.Develop.Runtime.UI.CommonViews;
using _Project.Develop.Runtime.UI.Core;
using _Project.Develop.Runtime.UI.Core.TestPopup;
using _Project.Develop.Runtime.UI.MainMenu.Stats;
using _Project.Develop.Runtime.UI.Wallet;
using _Project.Develop.Runtime.Utilities.ConfigsManagement;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using _Project.Develop.Runtime.Utilities.Reactive;
using _Project.Develop.Runtime.Utilities.SceneManagement;


namespace _Project.Develop.Runtime.UI
{
    public class ProjectPresentersFactory
    {
        private readonly DIContainer _container;

        public ProjectPresentersFactory(DIContainer container)
        {
            _container = container;
        }

        public CurrencyPresenter CreateCurrencyPresenter(
            IconTextView view,
            IReadOnlyVariable<int> currency,
            CurrencyTypes currencyType)
        {
            return new CurrencyPresenter(
                currency,
                currencyType,
                _container.Resolve<ConfigProviderService>().GetConfig<CurrencyIconsConfig>(),
                view);
        }

        public WalletPresenter CreateWalletPresenter(IconTextListView view)
        {
            return new WalletPresenter(
                _container.Resolve<WalletService>(),
                this,
                _container.Resolve<ViewsFactory>(),
                view);
        }

        public StatsPresenter CreateStatsPresenter(RatioView view)
        {
            return new StatsPresenter(
                view,
                _container.Resolve<StatsService>());
        }

        public TestPopupPresenter CreateTestPopupPresenter(TestPopupView view)
        {
            return new TestPopupPresenter(
                view,
                _container.Resolve<ICoroutinePerformer>());
        }
    }
}
