/*
* Copyright (C) 2023
* by Szymon Miś
* All rights reserved;
*/

using UnityEngine;

namespace SMF.Core
{
	public class Unparent : MonoBehaviour
	{
		private bool onAwake;
		private bool onStart;
		private bool onEnable;
		private bool onDisable;
		private bool onDestroy;

		private void Awake()
		{
			if(onAwake)
			{
				transform.SetParent(null);
			}
		}

		private void Start()
		{
			if(onStart)
			{
				transform.SetParent(null);
			}
		}

		private void OnEnable()
		{
			if(onEnable)
			{
				transform.SetParent(null);
			}
		}

		private void OnDisable()
		{
			if(onDisable)
			{
				transform.SetParent(null);
			}
		}

		private void OnDestroy()
		{
			if(onDestroy)
			{
				transform.SetParent(null);
			}
		}
	}
}