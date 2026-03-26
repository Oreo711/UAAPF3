using _Project.Develop.Runtime.Gameplay.Infrastructure;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using _Project.Develop.Runtime.Utilities.SceneManagement;


namespace _Project.Develop.Runtime.UI.Core.LossPopup
{
    public class LossPopupPresenter : PopupPresenterBase
    {
        private readonly LossPopupView        _view;
        private readonly SceneSwitcherService _sceneSwitcher;
        private readonly ICoroutinePerformer _coroutinePerformer;
        private readonly GameplayProcess _gameplay;

        public LossPopupPresenter(
            LossPopupView view,
            ICoroutinePerformer coroutinePerformer,
            SceneSwitcherService sceneSwitcher,
            GameplayProcess gameplay)
            : base (coroutinePerformer)
        {
            _view = view;
            _sceneSwitcher = sceneSwitcher;
            _coroutinePerformer = coroutinePerformer;
            _gameplay = gameplay;
        }

        protected override PopupViewBase PopupView => _view;

        public override void Initialize()
        {
            base.Initialize();

            _view.AcceptButtonClicked += HandleAcceptButtonClicked;
        }

        private void HandleAcceptButtonClicked ()
        {
            _coroutinePerformer.StartCoroutine(_sceneSwitcher.SwitchToAsync(
                Scenes.Gameplay,
                new GameplayInputArgs(_gameplay.GameMode)));
        }

        public override void Dispose ()
        {
            _view.AcceptButtonClicked -= HandleAcceptButtonClicked;

            base.Dispose();
        }
    }
}
