using UnityEngine;

namespace SMF.Core
{
	[RequireComponent(typeof(UIInteractionHandler))]
	public class UIOpenWindow : MonoBehaviour, IInteractable
	{
		public UIWindow window;

		public void Clicked() => window.Open();

		public void Hovered()
		{
		}

		public void Selected()
		{
		}

		public void Submit()
		{
		}

		public void Unhovered()
		{
		}

		public void Unselected()
		{
		}
	}
}