using _Project.Develop.Runtime.UI.Core;
using _Project.Develop.Runtime.UI.Core.LossPopup;
using UnityEngine;


namespace _Project.Develop.Runtime.UI.Gameplay
{
	public class GameplayPopupService : PopupService
	{
		private readonly GameplayUIRoot _uiRoot;

		private readonly GameplayPresentersFactory _presentersFactory;

		public GameplayPopupService (
			ViewsFactory viewsFactory,
			ProjectPresentersFactory presentersFactory,
			GameplayPresentersFactory gameplayPresentersFactory,
			GameplayUIRoot uiRoot
		)
			: base(viewsFactory, presentersFactory)
		{
			_presentersFactory = gameplayPresentersFactory;
			_uiRoot = uiRoot;
		}

		protected override Transform PopupLayer => _uiRoot.PopupsLayer;

		public LossPopupPresenter OpenLossPopup()
		{
			LossPopupView view = ViewsFactory.Create<LossPopupView>(ViewIDs.LossPopup, PopupLayer);

			LossPopupPresenter presenter = _presentersFactory.CreateLossPopup(view);

			OnPopupCreated(presenter, view);

			return presenter;
		}
	}
}
