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
	public class UIDefaultWindow : UIWindow
	{
		[Header("Subwindows config")]
		public List<UIWindow> subwindows = new List<UIWindow>();

		protected override void OpenWindow()
		{
			base.OpenWindow();

			subwindows?.ForEach(x => x.Open());
		}

		protected override void CloseWindow()
		{
			base.CloseWindow();

			subwindows?.ForEach(x => x.Close());
		}
	}
}