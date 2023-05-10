/*
* Copyright (C) 2023
* by Szymon Miś
* All rights reserved;
*/

using UnityEngine;

namespace SMF.Core
{
	public class OpenLink : MonoBehaviour
	{
		public void LoadLink(string link) => Application.OpenURL(link);
	}
}