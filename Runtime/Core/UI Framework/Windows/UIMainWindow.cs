/*
* Copyright (C) 2023
* by Szymon Miś
* All rights reserved;
*/

using System.Collections.Generic;
using UnityEngine;

namespace SMF.Core
{
	/// <summary>
	/// Default implementation of UIWindow
	/// </summary>
	public class UIMainWindow : UIWindow
	{
		[Header("Subwindows config")]
		public List<UISubWindow> subwindows = new List<UISubWindow>();

		public bool processSubWindows = true;

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