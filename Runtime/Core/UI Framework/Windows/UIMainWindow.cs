/*
* Copyright (C) 2023
* by Szymon Miś
* All rights reserved;
*/

using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SMF.Core
{
	/// <summary>
	/// Default implementation of UIWindow
	/// </summary>
	public class UIMainWindow : UIWindow
	{
		[Header("Subwindows config")]
		public bool processSubWindows = true;

		[ShowIf("processSubWindows")]
		public List<UISubWindow> subwindows = new List<UISubWindow>();

		[Header("Event Support Settings")]
		public bool firstSelectedSupport = true;

		[ShowIf("firstSelectedSupport")]
		public GameObject firstSelectedObject;

		protected override void Awake()
		{
			base.Awake();

			//Initializing sub windows by passing the parent
			if (processSubWindows)
			{
				subwindows?.ForEach(x => x.InitSubWindow(this));
			}
		}

		protected override void OpenWindow()
		{
			base.OpenWindow();

			//Handle opening sub windows
			if (processSubWindows)
			{
				subwindows?.ForEach(x => x.Open());
			}

			if (firstSelectedSupport)
			{
				EventSystem.current.SetSelectedGameObject(firstSelectedObject);
			}
		}

		protected override void CloseWindow()
		{
			base.CloseWindow();

			//Handle closing sub windows
			if (processSubWindows)
			{
				subwindows?.ForEach(x => x.Close());
			}
		}
	}
}