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
        public event Action StatsResetButtonClicked;

        [field: SerializeField] public IconTextListView WalletView       { get; private set; }
        [field: SerializeField] public RatioView        StatsView        { get; private set; }

        [SerializeField] private Button _playButtonNumbers;
        [SerializeField] private Button _playButtonLetters;
        [SerializeField] private Button _statsResetButton;

        private void OnEnable()
        {
            _playButtonNumbers.onClick.AddListener(OnPlayButtonNumbersClicked);
            _playButtonLetters.onClick.AddListener(OnPlayButtonLettersClicked);
            _statsResetButton.onClick.AddListener(OnStatsResetButtonClicked);
        }


        private void OnDisable()
        {
            _playButtonNumbers.onClick.RemoveListener(OnPlayButtonNumbersClicked);
            _playButtonLetters.onClick.RemoveListener(OnPlayButtonLettersClicked);
            _statsResetButton.onClick.RemoveListener(OnStatsResetButtonClicked);
        }

        private void OnPlayButtonNumbersClicked() => PlayButtonNumbersClicked?.Invoke();
        private void OnPlayButtonLettersClicked() => PlayButtonLettersClicked?.Invoke();
        private void OnStatsResetButtonClicked () => StatsResetButtonClicked?.Invoke();
    }
}
