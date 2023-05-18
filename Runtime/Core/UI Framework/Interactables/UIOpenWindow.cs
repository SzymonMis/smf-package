using UnityEngine;

namespace SMF.Core
{
	[RequireComponent(typeof(UIInteractionHandler))]
	public class UIOpenWindow : MonoBehaviour, IInteractable
	{
		public UIWindow window;

		public void Clicked() => UIWindowManager.instance.OpenWindow(window);

		public void Hovered()
		{
		}

		public void Selected()
		{
		}

		public void Submit() => UIWindowManager.instance.OpenWindow(window);

		public void Unhovered()
		{
		}

		public void Unselected()
		{
		}
	}
}