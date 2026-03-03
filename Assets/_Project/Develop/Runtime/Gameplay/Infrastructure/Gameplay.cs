using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Develop.Runtime.Gameplay.Configs;
using _Project.Develop.Runtime.Meta;
using _Project.Develop.Runtime.Meta.Features.Stats;
using _Project.Develop.Runtime.Meta.Features.Wallet;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using _Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using _Project.Develop.Runtime.Utilities.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;


namespace _Project.Develop.Runtime.Gameplay.Infrastructure
{
	public class Gameplay : MonoBehaviour
	{
		private GameplayConfig       _config;
		private SceneSwitcherService _sceneSwitcher;
		private ICoroutinePerformer  _coroutinePerformer;
		private WalletService        _wallet;
		private StatsService         _stats;
		private PlayerDataProvider   _playerDataProvider;

		private List<char> _chars;
		private char[]     _sequence;
		private bool       _isSetup;
		private GameMode   _gameMode;

		private readonly List<char> _input = new();

		public void Initialize (
			GameMode gameMode,
			GameplayConfig config,
			SceneSwitcherService sceneSwitcher,
			ICoroutinePerformer coroutinePerformer,
			WalletService wallet,
			StatsService stats,
			PlayerDataProvider playerDataProvider
		)
		{
			_gameMode           = gameMode;
			_config             = config;
			_sceneSwitcher      = sceneSwitcher;
			_coroutinePerformer = coroutinePerformer;
			_wallet             = wallet;
			_stats              = stats;
			_playerDataProvider = playerDataProvider;
		}

		public void Setup ()
		{
			_chars = GetChars();

			_sequence = new char[_config.SequenceLength];

			for (int i = 0; i < _config.SequenceLength; i++)
			{
				int charIndex = Random.Range(0, _chars.Count);
				_sequence[i] = _chars[charIndex];
			}

			Debug.Log(new string(_sequence));

			_isSetup = true;
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

		private void Update ()
		{
			if (!_isSetup)
				return;

			if (!Input.anyKeyDown)
				return;

			string inputString = Input.inputString;

			if (inputString.Length == 1 && _chars.Contains(inputString[0]))
			{
				_input.Add(inputString[0]);

				Debug.Log(inputString[0].ToString());

				if (_sequence.Length == _input.Count)
				{
					_coroutinePerformer.StartCoroutine(ValidateInput());
				}
			}
		}

		private IEnumerator ValidateInput ()
		{
			bool isValidInput = _sequence.SequenceEqual(_input);

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

			yield return _sceneSwitcher.SwitchToAsync(Scenes.Gameplay, new GameplayInputArgs(_gameMode));
		}
	}
}
