using System.Collections;
using _Project.Develop.Runtime.Gameplay.Infrastructure;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using _Project.Develop.Runtime.Utilities.SceneManagement;
using UnityEngine;


namespace _Project.Develop.Runtime.Meta
{
	public class GameModeSelectionService
	{
		private readonly ICoroutinePerformer  _coroutinePerformer;
		private readonly SceneSwitcherService _sceneSwitcher;

		private bool _inputReceived;

		public GameModeSelectionService (ICoroutinePerformer coroutinePerformer, SceneSwitcherService sceneSwitcher)
		{
			_coroutinePerformer = coroutinePerformer;
			_sceneSwitcher      = sceneSwitcher;
		}

		public void Run ()
		{
			_coroutinePerformer.StartCoroutine(ProcessInput());
		}

		private IEnumerator ProcessInput()
		{
			while (!_inputReceived)
			{
				if (Input.GetKeyDown(KeyCode.Alpha1))
				{
					_inputReceived = true;
					_coroutinePerformer.StartCoroutine(ProcessStartGameplay(GameMode.numbers));
				}
				else if (Input.GetKeyDown(KeyCode.Alpha2))
				{
					_inputReceived = true;
					_coroutinePerformer.StartCoroutine(ProcessStartGameplay(GameMode.letters));
				}

				yield return null;
			}
		}

		private IEnumerator ProcessStartGameplay (GameMode gameMode)
		{
			yield return _sceneSwitcher.SwitchToAsync(Scenes.Gameplay, new GameplayInputArgs(gameMode));
		}
	}
}
