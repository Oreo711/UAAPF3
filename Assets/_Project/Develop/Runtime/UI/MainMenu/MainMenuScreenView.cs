using System;
using _Project.Develop.Runtime.UI.CommonViews;
using _Project.Develop.Runtime.UI.Core;
using UnityEngine;
using UnityEngine.UI;


namespace _Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenView : MonoBehaviour, IView
    {
        public event Action PlayButtonNumbersClicked;
        public event Action PlayButtonLettersClicked;

        [field: SerializeField] public IconTextListView WalletView { get; private set; }
        [field: SerializeField] public RatioView StatsView { get; private set; }

        [SerializeField] private Button _playButtonNumbers;
        [SerializeField] private Button _playButtonLetters;

        private void OnEnable()
        {
            _playButtonNumbers.onClick.AddListener(OnPlayButtonNumbersClicked);
            _playButtonLetters.onClick.AddListener(OnPlayButtonLettersClicked);
        }

        private void OnDisable()
        {
            _playButtonNumbers.onClick.RemoveListener(OnPlayButtonNumbersClicked);
            _playButtonLetters.onClick.RemoveListener(OnPlayButtonLettersClicked);
        }

        private void OnPlayButtonNumbersClicked() => PlayButtonNumbersClicked?.Invoke();
        private void OnPlayButtonLettersClicked() => PlayButtonLettersClicked?.Invoke();
    }
}
