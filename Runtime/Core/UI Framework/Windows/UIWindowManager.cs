/*
* Copyright (C) 2023
* by Szymon Miś
* All rights reserved;
*/

using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR

using Sirenix.OdinInspector;

#endif

namespace SMF.Core
{
	/// <summary>
	/// The UIWindowManager maintains a list of windows and a stack for tracking the opened windows.
	/// </summary>
	public class UIWindowManager : MonoBehaviour
	{
		[Header("Windows Setup")]
		// List of UIWindow instances to manage
		public List<UIWindow> windows;

		[Header("Startup Settings")]
		public bool openWindowOnStart;

#if ODIN_INSPECTOR

		[ShowIf("openWindowOnStart")]
#endif
		public UIWindow startupWindow;

		// Stack to keep track of the opened UIWindow instances
		private Stack<UIWindow> windowStack;

		public static UIWindowManager instance;

		// Initialize the windowStack during windows
		private void Awake()
		{
			windowStack = new Stack<UIWindow>();
			instance = this;
		}

		// Open the first window on start
		private void Start()
		{
			if (openWindowOnStart && startupWindow != null)
			{
				OpenWindow(startupWindow);
			}
		}

		/// <summary>
		/// Open the specified UIWindow and close the currently opened window if any
		/// </summary>
		public void OpenWindow(UIWindow window)
		{
			// If the window parameter is null, do not proceed
			if (window == null)
			{
				return;
			}

			// If there are windows in the stack, close the window on the top
			if (windowStack.Count > 0)
			{
				windowStack.Peek().Close();
			}

			// Push the new window onto the stack and open it
			windowStack.Push(window);
			window.Open();
		}

		/// <summary>
		/// Close the currently opened UIWindow, and open the parent or the previous window if any
		/// </summary>
		public void CloseWindow()
		{
			// If there are no windows in the stack, do not proceed
			if (windowStack.Count == 0)
			{
				return;
			}

			// Pop the top window from the stack and close it
			UIWindow topWindow = windowStack.Pop();
			topWindow.Close();

			// If the closed window has a parent, push the parent onto the stack and open it
			if (topWindow.parent != null)
			{
				windowStack.Push(topWindow.parent);
				topWindow.parent.Open();
			}
			// If there are other windows in the stack, open the window on the top
			else if (windowStack.Count > 0)
			{
				windowStack.Peek().Open();
			}
		}
	}
}