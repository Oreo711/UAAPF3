using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Develop.Runtime.Gameplay.Configs;
using _Project.Develop.Runtime.Meta;
using _Project.Develop.Runtime.Meta.Features.Stats;
using _Project.Develop.Runtime.Meta.Features.Wallet;
using _Project.Develop.Runtime.UI.Gameplay;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using _Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using _Project.Develop.Runtime.Utilities.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;


namespace _Project.Develop.Runtime.Gameplay.Infrastructure
{
	public class GameplayProcess
	{
		public event Action HasSetup;

		private readonly GameplayConfig       _config;
		private readonly SceneSwitcherService _sceneSwitcher;
		private readonly ICoroutinePerformer  _coroutinePerformer;
		private readonly WalletService        _wallet;
		private readonly StatsService         _stats;
		private readonly PlayerDataProvider   _playerDataProvider;
		private readonly GameplayPopupService _popupService;

		private List<char> _chars;
		private char[]     _sequence;
		private GameMode   _gameMode;

		public GameplayProcess (
			GameplayConfig config,
			SceneSwitcherService sceneSwitcher,
			ICoroutinePerformer coroutinePerformer,
			WalletService wallet,
			StatsService stats,
			PlayerDataProvider playerDataProvider,
			GameplayPopupService popupService
		)
		{
			_config             = config;
			_sceneSwitcher      = sceneSwitcher;
			_coroutinePerformer = coroutinePerformer;
			_wallet             = wallet;
			_stats              = stats;
			_playerDataProvider = playerDataProvider;
			_popupService       = popupService;
		}

		public char[] Sequence => _sequence;
		public GameMode GameMode => _gameMode;

		public void Setup (GameMode gameMode)
		{
			_gameMode = gameMode;

			_chars = GetChars();

			_sequence = new char[_config.SequenceLength];

			for (int i = 0; i < _config.SequenceLength; i++)
			{
				int charIndex = Random.Range(0, _chars.Count);
				_sequence[i] = _chars[charIndex];
			}

			HasSetup?.Invoke();
		}

		private List<char> GetChars ()
		{
			char[] chars;

			switch (_gameMode)
			{
				case GameMode.numbers:
					chars = _config.Numbers;
					break;
				case GameMode.letters:
					chars = _config.Letters;
					break;
				default:
					throw new InvalidOperationException();
			}

			return chars.ToList();
		}

		public void ValidateInput (string input)
		{
			_coroutinePerformer.StartCoroutine(ProcessValidateInput(input));
		}

		private IEnumerator ProcessValidateInput (string input)
		{
			bool isValidInput = _sequence.SequenceEqual(input);

			if (isValidInput)
			{
				_wallet.Add(CurrencyTypes.Gold, 10);
				_stats.IncrementWins();

				yield return _playerDataProvider.Save();

				yield return _sceneSwitcher.SwitchToAsync(Scenes.MainMenu);

				yield break;
			}

			if (_wallet.Enough(CurrencyTypes.Gold, 10))
			{
				_wallet.Spend(CurrencyTypes.Gold, 10);
			} else
			{
				_wallet.Spend(CurrencyTypes.Gold, _wallet.GetCurrency(CurrencyTypes.Gold).Value);
			}

			_stats.IncrementLosses();
			yield return _playerDataProvider.Save();

			_popupService.OpenLossPopup();
		}
	}
}
