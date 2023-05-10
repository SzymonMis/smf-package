using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SMF.Core
{
	public class UIInteractionHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler, ISubmitHandler
	{
		private List<IInteractable> interactables = new List<IInteractable>();

		public List<IInteractable> Interactables => interactables;

		private void OnEnable() => interactables = new List<IInteractable>(GetComponentsInChildren<IInteractable>());

		private void OnDisable()
		{
			// Create a copy of the Interactables list
			var tempInteractables = new List<IInteractable>(Interactables);

			// Clear the original Interactables list
			interactables.Clear();

			// Call the Unselected method on all the interactables in the copy
			tempInteractables.ForEach(x => x.Unselected());
		}

		/// <summary>
		/// Handling OnClick event for all selected interactables
		/// </summary>
		/// <param name="eventData"></param>
		public void OnPointerClick(PointerEventData eventData)
		{
			var tempInteractables = new List<IInteractable>(Interactables);
			tempInteractables?.ForEach(x => x.Clicked());
		}

		/// <summary>
		/// Handling OnHover event for all selected interactables
		/// </summary>
		/// <param name="eventData"></param>
		public void OnPointerEnter(PointerEventData eventData)
		{
			var tempInteractables = new List<IInteractable>(Interactables);
			tempInteractables?.ForEach(x => x.Hovered());
		}

		/// <summary>
		/// Handling OnUnhover event for all selected interactables
		/// </summary>
		/// <param name="eventData"></param>
		public void OnPointerExit(PointerEventData eventData) => Interactables?.ForEach(x => x.Unhovered());

		/// <summary>
		/// Handling OnSelected event for all selected interactables
		/// </summary>
		public void OnSelect(BaseEventData eventData)
		{
			var tempInteractables = new List<IInteractable>(Interactables);
			tempInteractables?.ForEach(x => x.Selected());
		}

		/// <summary>
		/// Handling OnUnselected event for all selected interactables
		/// </summary>
		public void OnDeselect(BaseEventData eventData)
		{
			var tempInteractables = new List<IInteractable>(Interactables);
			tempInteractables?.ForEach(x => x.Unselected());
		}

		/// <summary>
		/// Handling OnSubmit event for all selected interactables
		/// </summary>
		public void OnSubmit(BaseEventData eventData)
		{
			var tempInteractables = new List<IInteractable>(Interactables);
			tempInteractables?.ForEach(x => x.Submit());
		}
	}
}