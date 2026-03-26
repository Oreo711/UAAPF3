using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace _Project.Develop.Runtime.UI.Core.LossPopup
{
    public class LossPopupView : PopupViewBase
    {
        public event Action AcceptButtonClicked;

        [SerializeField] private Button _acceptButton;

        private void OnEnable ()
        {
            _acceptButton.onClick.AddListener(OnAcceptButtonClicked);
        }

        private void OnAcceptButtonClicked ()
        {
            AcceptButtonClicked?.Invoke();
        }

        private void OnDisable ()
        {
            _acceptButton.onClick.RemoveListener(OnAcceptButtonClicked);
        }
    }
}
