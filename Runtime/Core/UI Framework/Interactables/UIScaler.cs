using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SMF.Core
{
	/// <summary>
	/// UIScaler is a MonoBehaviour class that scales UI elements using different interaction types.
	/// </summary>
	[RequireComponent(typeof(Image))]
	[RequireComponent(typeof(UIInteractionHandler))]
	[RequireComponent(typeof(Canvas))]
	[RequireComponent(typeof(GraphicRaycaster))]
	public class UIScaler : UITransition, IInteractable
	{
		[SerializeField] private int hoverSortingOrder = 100;
		[SerializeField] private float transitionSpeed = 20f;
		[SerializeField] private float targetHoverScale = 1.2f;
		[SerializeField] private float targetClickScale = 1.0f;

		private Canvas canvas;
		private Vector3 originalScale;
		private int originalSortingOrder;
		private Dictionary<InteractionType, Vector3> scaleTargets;

		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// It initializes the required components and sets up the scaling targets for different interaction types.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

			canvas = GetComponent<Canvas>();
			originalSortingOrder = canvas.sortingOrder;
			originalScale = transform.localScale;

			scaleTargets = new Dictionary<InteractionType, Vector3>()
			{
				{ InteractionType.Clicked, originalScale * targetClickScale },
				{ InteractionType.Hovered, originalScale * targetHoverScale },
				{ InteractionType.Selected, originalScale * targetHoverScale },
				{ InteractionType.Unhovered, originalScale },
				{ InteractionType.Unselected, originalScale },
				{ InteractionType.Submit, originalScale * targetClickScale }
			};
		}

		/// <summary>
		/// Coroutine that scales the UI element based on the specified interaction type.
		/// </summary>
		/// <param name="interactionType">The interaction type for which to scale the UI element.</param>
		private IEnumerator ScaleImage(InteractionType interactionType)
		{
			Vector3 targetScale = scaleTargets[interactionType];
			float time = 0;
			AnimationCurve animationCurve = GetTansition(transitionType);

			while (time < 1)
			{
				time += Time.deltaTime * transitionSpeed;
				float t = animationCurve.Evaluate(time);
				transform.localScale = Vector3.Lerp(transform.localScale, targetScale, t);
				yield return null;
			}
		}

		/// <summary>
		/// Sets the canvas sorting order to the specified value.
		/// </summary>
		/// <param name="sortingOrder">The sorting order to set.</param>
		private void SetCanvasSortingOrder(int sortingOrder) => canvas.sortingOrder = sortingOrder;

		/// <summary>
		/// Handles the scaling and sorting order update for the specified interaction type.
		/// </summary>
		/// <param name="interactionType">The interaction type to handle.</param>
		public void HandleInteraction(InteractionType interactionType)
		{
			if (gameObject.activeInHierarchy)
			{
				StartCoroutine(ScaleImage(interactionType));
			}
			else
			{
				transform.localScale = originalScale;
			}

			SetCanvasSortingOrder(interactionType == InteractionType.Unhovered || interactionType == InteractionType.Unselected ? originalSortingOrder : hoverSortingOrder);
		}

		// IInteractable implementation
		public void Clicked() => HandleInteraction(InteractionType.Clicked);

		public void Hovered() => HandleInteraction(InteractionType.Hovered);

		public void Selected() => HandleInteraction(InteractionType.Selected);

		public void Unhovered() => HandleInteraction(InteractionType.Unhovered);

		public void Unselected() => HandleInteraction(InteractionType.Unselected);

		public void Submit() => HandleInteraction(InteractionType.Submit);
	}
}