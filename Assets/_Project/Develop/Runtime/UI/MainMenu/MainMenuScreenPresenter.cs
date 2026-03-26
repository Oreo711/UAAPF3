using System.Collections;
using System.Collections.Generic;
using _Project.Develop.Runtime.Configs.Meta;
using _Project.Develop.Runtime.Gameplay.Infrastructure;
using _Project.Develop.Runtime.Meta;
using _Project.Develop.Runtime.Meta.Features.Stats;
using _Project.Develop.Runtime.Meta.Features.Wallet;
using _Project.Develop.Runtime.UI.Core;
using _Project.Develop.Runtime.UI.MainMenu.Stats;
using _Project.Develop.Runtime.UI.Wallet;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using _Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using _Project.Develop.Runtime.Utilities.SceneManagement;
using UnityEngine;


namespace _Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenPresenter : IPresenter
    {
        private readonly SceneSwitcherService _sceneSwitcher;
        private readonly ICoroutinePerformer  _coroutinePerformer;
        private readonly WalletService _wallet;
        private readonly StatsService _stats;
        private readonly StatsResetCostConfig _resetCostConfig;
        private readonly PlayerDataProvider _playerDataProvider;

        private readonly MainMenuScreenView _screen;
        private readonly ProjectPresentersFactory _projectPresentersFactory;
        private readonly List<IPresenter> _childPresenters = new();

        public MainMenuScreenPresenter(
            MainMenuScreenView screen,
            ProjectPresentersFactory projectPresentersFactory,
            SceneSwitcherService sceneSwitcher,
            ICoroutinePerformer coroutinePerformer,
            WalletService wallet,
            StatsService stats,
            StatsResetCostConfig resetCostConfig,
            PlayerDataProvider playerDataProvider
        )
        {
            _screen                   = screen;
            _projectPresentersFactory = projectPresentersFactory;
            _sceneSwitcher            = sceneSwitcher;
            _coroutinePerformer       = coroutinePerformer;
            _wallet                   = wallet;
            _stats                    = stats;
            _resetCostConfig          = resetCostConfig;
            _playerDataProvider             = playerDataProvider;
        }

        public void Initialize()
        {
            CreateWallet();
            CreateStats();

            foreach (IPresenter presenter in _childPresenters)
                presenter.Initialize();

            _screen.PlayButtonNumbersClicked += HandlePlayButtonNumbersClicked;
            _screen.PlayButtonLettersClicked += HandlePlayButtonLettersClicked;
            _screen.StatsResetButtonClicked  += HandleStatsResetButtonClicked;
        }

        private void HandleStatsResetButtonClicked ()
        {
            if (_wallet.Enough(CurrencyTypes.Gold, _resetCostConfig.ResetCost))
            {
                _stats.Reset();
                _wallet.Spend(CurrencyTypes.Gold, _resetCostConfig.ResetCost);
                _coroutinePerformer.StartCoroutine(_playerDataProvider.Save());

                return;
            }

            Debug.Log("Insufficient Gold.");
        }

        private void HandlePlayButtonLettersClicked ()
        {
            _coroutinePerformer.StartCoroutine(_sceneSwitcher.SwitchToAsync(Scenes.Gameplay, new GameplayInputArgs(GameMode.letters)));
        }

        private void HandlePlayButtonNumbersClicked ()
        {
            _coroutinePerformer.StartCoroutine(_sceneSwitcher.SwitchToAsync(Scenes.Gameplay, new GameplayInputArgs(GameMode.numbers)));
        }

        public void Dispose()
        {
            _screen.PlayButtonNumbersClicked -= HandlePlayButtonNumbersClicked;
            _screen.PlayButtonLettersClicked -= HandlePlayButtonLettersClicked;
            _screen.StatsResetButtonClicked  -= HandleStatsResetButtonClicked;

            foreach (IPresenter presenter in _childPresenters)
                presenter.Dispose();

            _childPresenters.Clear();
        }

        private void CreateWallet()
        {
            WalletPresenter walletPresenter = _projectPresentersFactory.CreateWalletPresenter(_screen.WalletView);

            _childPresenters.Add(walletPresenter);
        }

        private void CreateStats ()
        {
            StatsPresenter statsPresenter = _projectPresentersFactory.CreateStatsPresenter(_screen.StatsView);

            _childPresenters.Add(statsPresenter);
        }
    }
}
