/*
* Copyright (C) 2023
* by Szymon Miś
* All rights reserved;
*/

using UnityEngine;

namespace SMF.Core
{
	public abstract class SystemicBehaviour<T> : MonoBehaviour where T : SystemicBehaviour<T>
	{
		protected virtual int ExecutionOrder { get => 0; }

		private static string systemName = null;

		private static string SystemName
		{
			get
			{
				if (systemName == null)
				{
					systemName = $"{typeof(T).Name}_US";
				}
				return systemName;
			}
		}

		private BehaviourSystem ownSystem = null;

		protected virtual void Start()
		{
			ownSystem = GlobalBehaviourUpdateManager.AddBehaviour(this, SystemName, ExecutionOrder);
		}

		protected virtual void OnDestroy()
		{
			if (ownSystem != null)
			{
				ownSystem.RemoveMonoBehaviour(this);
			}
		}

		protected virtual void OnEnable()
		{
			if (ownSystem != null)
			{
				ownSystem.SetBehaviourEnabled(this, true);
			}
		}

		protected virtual void OnDisable()
		{
			if (ownSystem != null)
			{
				ownSystem.SetBehaviourEnabled(this, false);
			}
		}
	}
}