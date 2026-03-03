using System.Collections;
using _Project.Develop.Runtime.Infrastructure;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Meta.Features.Stats;
using _Project.Develop.Runtime.Meta.Features.Wallet;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using _Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using _Project.Develop.Runtime.Utilities.SceneManagement;
using UnityEngine;


namespace _Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuBootstrap : SceneBootstrap
    {
        private DIContainer _container;

        private WalletService _walletService;
        private StatsService  _statsService;

        private PlayerDataProvider       _playerDataProvider;
        private ICoroutinePerformer      _coroutinePerformer;
        private GameModeSelectionService _gameModeSelectionService;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            MainMenuContextRegistrations.Process(_container);
        }

        public override IEnumerator Initialize()
        {
            _walletService = _container.Resolve<WalletService>();
            _statsService = _container.Resolve<StatsService>();

            _playerDataProvider       = _container.Resolve<PlayerDataProvider>();
            _coroutinePerformer       = _container.Resolve<ICoroutinePerformer>();
            _gameModeSelectionService = _container.Resolve<GameModeSelectionService>();

            yield break;
        }

        public override void Run()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                _coroutinePerformer.StartCoroutine(_playerDataProvider.Save());
                Debug.Log("Saved!");
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                _coroutinePerformer.StartCoroutine(_playerDataProvider.Load());
                Debug.Log("Loaded!");
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log($"Current balance: {_walletService.GetCurrency(CurrencyTypes.Gold).Value.ToString()}");
                Debug.Log($"Current wins: {_statsService.Wins.ToString()}; Current losses: {_statsService.Losses.ToString()}");
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                if (_walletService.Enough(CurrencyTypes.Gold, _statsService.ResetCost))
                {
                    _walletService.Spend(CurrencyTypes.Gold, _statsService.ResetCost);
                    _statsService.Reset();

                    _coroutinePerformer.StartCoroutine(_playerDataProvider.Save());

                    Debug.Log("Stats have been reset.");
                }
                else
                {
                    Debug.Log("Insufficient gold");
                }
            }
        }
    }
}
