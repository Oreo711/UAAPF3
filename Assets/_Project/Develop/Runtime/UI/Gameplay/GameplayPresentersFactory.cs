using _Project.Develop.Runtime.Gameplay.Infrastructure;
using _Project.Develop.Runtime.Infrastructure.DI;


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
	}
}
