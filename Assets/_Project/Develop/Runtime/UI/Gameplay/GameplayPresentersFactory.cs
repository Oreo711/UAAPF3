using _Project.Develop.Runtime.Gameplay.Infrastructure;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.UI.Core.LossPopup;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using _Project.Develop.Runtime.Utilities.SceneManagement;


namespace _Project.Develop.Runtime.UI.Gameplay
{
	public class GameplayPresentersFactory
	{
		private DIContainer _container;

		public GameplayPresentersFactory (DIContainer container)
		{
			_container = container;
		}

		public GameplayScreenPresenter CreateGameplayScreen (GameplayScreenView view)
		{
			GameplayScreenPresenter presenter = new GameplayScreenPresenter(
				view,
				_container.Resolve<GameplayProcess>());
			presenter.Initialize();

			return presenter;
		}

		public LossPopupPresenter CreateLossPopup (LossPopupView view)
		{
			LossPopupPresenter presenter = new LossPopupPresenter(
				view,
				_container.Resolve<ICoroutinePerformer>(),
				_container.Resolve<SceneSwitcherService>(),
				_container.Resolve<GameplayProcess>());

			return presenter;
		}
	}
}
