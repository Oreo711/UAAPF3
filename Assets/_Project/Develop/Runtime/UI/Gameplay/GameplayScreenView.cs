using System;
using _Project.Develop.Runtime.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace _Project.Develop.Runtime.UI.Gameplay
{
	public class GameplayScreenView : MonoBehaviour, IView
	{
		public event Action<string> InputChanged;

		[SerializeField] private TMP_Text _requestedSequence;
		[SerializeField] private TMP_InputField _sequenceInput;

		private void OnEnable ()
		{
			_sequenceInput.onValueChanged.AddListener(OnInputChanged);
		}

		private void OnDisable ()
		{
			_sequenceInput.onValueChanged.RemoveListener(OnInputChanged);
		}

		private void OnInputChanged (string newValue)
		{
			InputChanged?.Invoke(newValue);
		}

		public void SetRequestedSequence (string sequence)
		{
			_requestedSequence.text = sequence;
		}
	}
}
