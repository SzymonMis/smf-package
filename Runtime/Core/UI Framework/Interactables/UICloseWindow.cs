using UnityEngine;

namespace SMF.Core
{
	[RequireComponent(typeof(UIInteractionHandler))]
	public class UICloseWindow : MonoBehaviour, IInteractable
	{
		public void Clicked() => UIWindowManager.instance.CloseWindow();

		public void Hovered()
		{
		}

		public void Selected()
		{
		}

		public void Submit() => UIWindowManager.instance.CloseWindow();

		public void Unhovered()
		{
		}

		public void Unselected()
		{
		}
	}
}