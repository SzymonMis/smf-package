/*
* Copyright (C) 2023
* by Szymon Miś
* All rights reserved;
*/

using UnityEngine;

#if ODIN_INSPECTOR

using Sirenix.OdinInspector;

#endif

namespace SMF.Core
{
	/// <summary>
	/// You can inherit from UIWindow whenever you create your own UI window.
	/// It is not necessary tho, the UI window supports functions such as transitions,
	/// fading, opening and closing the window
	/// </summary>
	[RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
	public abstract class UIWindow : MonoBehaviour
	{
		[Tooltip("Parent is optional")]
		public UIWindow parent;

		[Header("Window Transitions")]
		// Variables to define the open and close directions and distance
		public bool enableTransition = true;

#if ODIN_INSPECTOR

		[ShowIf("enableTransition")]
#endif
		public Direction openDirection = Direction.Down;

#if ODIN_INSPECTOR

		[ShowIf("enableTransition")]
#endif
		public Direction closeDirection = Direction.Down;

#if ODIN_INSPECTOR

		[ShowIf("enableTransition")]
#endif
		public float transitionOffset = 0f;

		[Header("Alpha Fade")]
		// Determine if the window should fade in/out when opening and closing
		public bool enableAlphaFade = true;

		[Header("Other Settings")]
		// Duration of the opening and closing transition
		public float transitionDuration = 0.5f;

		private CanvasGroup canvasGroup;
		private RectTransform rectTransform;
		private Vector3 openedPosition;
		private Vector3 closedPosition;
		private float targetAlpha;

		/// <summary>
		/// Set up the initial state for the UIWindow
		/// </summary>
		protected virtual void Awake()
		{
			// Get the CanvasGroup and RectTransform components
			canvasGroup = GetComponent<CanvasGroup>();
			rectTransform = GetComponent<RectTransform>();

			// If there is no CanvasGroup component, add one
			if (canvasGroup == null)
			{
				canvasGroup = gameObject.AddComponent<CanvasGroup>();
			}

			// If there is no RectTransform component, add one
			if (rectTransform == null)
			{
				rectTransform = gameObject.AddComponent<RectTransform>();
			}

			// Set the initial alpha value to 0 and deactivate the game object
			canvasGroup.alpha = 0;
			gameObject.SetActive(false);

			// Store the open and closed positions of the UIWindow
			openedPosition = rectTransform.anchoredPosition;
			closedPosition = CalculatePosition(closeDirection);
		}

		/// <summary>
		/// Calculate the position of the UIWindow based on the given direction
		/// </summary>
		/// <param name="direction"></param>
		/// <returns></returns>
		private Vector3 CalculatePosition(Direction direction)
		{
			Vector3 offset = Vector3.zero;

			switch (direction)
			{
				case Direction.Up:
					offset = new Vector3(0, Screen.height + transitionOffset, 0);
					break;

				case Direction.Down:
					offset = new Vector3(0, -Screen.height - transitionOffset, 0);
					break;

				case Direction.Left:
					offset = new Vector3(-Screen.width - transitionOffset, 0, 0);
					break;

				case Direction.Right:
					offset = new Vector3(Screen.width + transitionOffset, 0, 0);
					break;
			}

			return openedPosition + offset;
		}

		/// <summary>
		/// Open the UIWindow with the specified transition
		/// </summary>
		protected virtual void OpenWindow()
		{
			gameObject.SetActive(true);

			// Calculate transition if enabled
			if (enableTransition)
			{
				rectTransform.anchoredPosition = CalculatePosition(openDirection);
			}
			else
			{
				rectTransform.anchoredPosition = openedPosition;
			}

			// Perform an alpha fade if enabled, otherwise set alpha to 1
			if (enableAlphaFade)
			{
				LeanTween.alphaCanvas(canvasGroup, 1f, transitionDuration).setEaseInOutQuad();
			}
			else
			{
				canvasGroup.alpha = 1;
			}

			// Move the UIWindow to the open position with the specified transition if enabled
			if (enableTransition)
			{
				LeanTween.move(rectTransform, openedPosition, transitionDuration).setEaseInOutQuad();
			}
		}

		/// <summary>
		/// Close the UIWindow with the specified transition
		/// </summary>
		protected virtual void CloseWindow()
		{
			// Perform an alpha fade to 0 with the specified transition if enabled
			if (enableAlphaFade)
			{
				LeanTween.alphaCanvas(canvasGroup, 0f, transitionDuration).setEaseInOutQuad();
			}

			// Move the UIWindow to the closed position with the specified transition if enabled and deactivate it when complete
			if (enableTransition)
			{
				LeanTween.move(rectTransform, closedPosition, transitionDuration).setEaseInOutQuad().setOnComplete(() => gameObject.SetActive(false));
			}
		}

		/// <summary>
		/// Public method to open the UIWindow
		/// </summary>
#if ODIN_INSPECTOR

		[HorizontalGroup("Split", 0.5f)]
		[Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
#endif
		public void Open() => OpenWindow();

		/// <summary>
		/// Public method to close the UIWindow
		/// </summary>
#if ODIN_INSPECTOR

		[VerticalGroup("Split/right")]
		[Button(ButtonSizes.Large), GUIColor(1, 0, 0)]
#endif
		public void Close() => CloseWindow();
	}

	public enum Direction
	{
		Up,
		Down,
		Left,
		Right
	}
}