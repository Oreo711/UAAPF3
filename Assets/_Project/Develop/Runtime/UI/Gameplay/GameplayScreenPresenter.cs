using System;
using _Project.Develop.Runtime.Gameplay.Infrastructure;
using _Project.Develop.Runtime.UI.Core;


namespace _Project.Develop.Runtime.UI.Gameplay
{
	public class GameplayScreenPresenter : IPresenter
	{
		private readonly GameplayScreenView _screen;
		private readonly GameplayProcess _gameplayProcess;

		public GameplayScreenPresenter (GameplayScreenView screen, GameplayProcess gameplayProcess)
		{
			_screen = screen;
			_gameplayProcess = gameplayProcess;
		}

		public void Initialize ()
		{
			_screen.InputChanged      += ProcessAndValidate;
			_gameplayProcess.HasSetup += SetSequenceText;
		}

		private void SetSequenceText ()
		{
			_screen.SetRequestedSequence(new string(_gameplayProcess.Sequence));
		}

		private void ProcessAndValidate (string newInput)
		{
			bool isMatchingSequenceLength = _gameplayProcess.Sequence.Length == newInput.Length;

			if (isMatchingSequenceLength)
			{
				_gameplayProcess.ValidateInput(newInput);
			}
		}

		public void Dispose ()
		{
			_screen.InputChanged      -= ProcessAndValidate;
			_gameplayProcess.HasSetup -= SetSequenceText;
		}
	}
}
