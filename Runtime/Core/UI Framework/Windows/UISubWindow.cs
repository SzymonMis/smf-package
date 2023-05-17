/*
* Copyright (C) 2023
* by Szymon Miœ
* All rights reserved;
*/

namespace SMF.Core
{
	/// <summary>
	/// Subwindow
	/// </summary>
	public class UISubWindow : UIWindow
	{
		public UIMainWindow parentWindow;

		public void InitSubWindow(UIMainWindow parentWindow) => this.parentWindow = parentWindow;
	}
}