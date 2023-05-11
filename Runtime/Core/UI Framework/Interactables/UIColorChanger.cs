/*
* Copyright (C) 2023
* by Szymon Miś
* All rights reserved;
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SMF.Core
{
	[RequireComponent(typeof(UIInteractionHandler))]
	public class UIColorChanger : UITransition, IInteractable
	{
		[SerializeField] private Color normalColor = Color.white;
		[SerializeField] private Color hoverColor = Color.gray;
		[SerializeField] private Color clickColor = Color.green;

		// List of target Graphic components whose colors will change based on interactions
		[SerializeField] private List<Graphic> targetGraphics;

		private Dictionary<InteractionType, Color> colorStates;

		/// <summary>
		/// Initializes the color states for different interaction types.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

			colorStates = new Dictionary<InteractionType, Color>()
			{
				{ InteractionType.None, normalColor },
				{ InteractionType.Hovered, hoverColor },
				{ InteractionType.Clicked, clickColor },
				{ InteractionType.Selected, hoverColor },
				{ InteractionType.Unhovered, normalColor },
				{ InteractionType.Unselected, normalColor },
				{ InteractionType.Submit, clickColor }
			};
		}

		/// <summary>
		/// Coroutine for changing the color of the target Graphic components.
		/// </summary>
		/// <param name="targetColor">The target color to change to.</param>
		/// <returns></returns>
		private IEnumerator ChangeColor(Color targetColor)
		{
			float time = 0;
			AnimationCurve animationCurve = GetTansition(transitionType);

			while (time < 1)
			{
				time += Time.deltaTime * transitionSpeed;
				float t = animationCurve.Evaluate(time);

				foreach (Graphic graphic in targetGraphics)
				{
					graphic.color = Color.Lerp(graphic.color, targetColor, t);
				}

				yield return null;
			}
		}

		/// <summary>
		/// Changes the color of the target Graphic components based on the specified interaction type.
		/// </summary>
		/// <param name="interactionType">The interaction type to process.</param>
		public void ProcessEvent(InteractionType interactionType)
		{
			if (!gameObject.activeInHierarchy)
			{
				foreach (Graphic graphic in targetGraphics)
				{
					graphic.color = normalColor;
				}
				return;
			}

			StartCoroutine(ChangeColor(colorStates[interactionType]));
		}

		public void Clicked() => ProcessEvent(InteractionType.Clicked);

		public void Hovered() => ProcessEvent(InteractionType.Hovered);

		public void Selected() => ProcessEvent(InteractionType.Selected);

		public void Unhovered() => ProcessEvent(InteractionType.Unhovered);

		public void Unselected() => ProcessEvent(InteractionType.Unselected);

		public void Submit() => ProcessEvent(InteractionType.Submit);
	}
}