/*
* Copyright (C) 2023
* by Szymon Miœ
* All rights reserved;
*/

using UnityEngine;

namespace SMF.Core
{
	/// <summary>
	/// Subwindow
	/// </summary>
	public class UISubWindow : UIWindow
	{
		[Header("Parent Setup")]
		public UIMainWindow parentWindow;

		public void InitSubWindow(UIMainWindow parentWindow) => this.parentWindow = parentWindow;
	}
}