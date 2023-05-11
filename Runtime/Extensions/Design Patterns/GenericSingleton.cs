/*
* Copyright (C) 2023
* by Szymon Miś
* All rights reserved;
*/

using UnityEngine;

namespace SMF.Extensions
{
	[DisallowMultipleComponent]
	public abstract class GenericSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		/// <summary>
		/// Generic implementation of singleton
		/// </summary>
		#region Singleton
		private static bool isDestroyed = false;
		private static T instance;

		public static T Instance
		{
			get
			{
				if (isDestroyed)
				{
					return null;
				}

				if (instance != null) { return instance; }
				
				if (instance == null)
				{
					T[] existingInstances = FindObjectsOfType<T>();
					if (existingInstances.Length >= 1)
					{
						instance = existingInstances[0];

						for (int i = 1; i < existingInstances.Length; i++)
						{
							#if UNITY_EDITOR
							Debug.LogWarning($"Destroing \"{typeof(T).Name}\" from {existingInstances[i].gameObject.name} game object!");
							#endif
							DestroyImmediate(existingInstances[i]);
						}
					}
					if (instance == null)
					{				
						GameObject go = new GameObject($"{typeof(T).Name}_Instance");
						instance = go.AddComponent<T>();
					}
				}

				return instance;
			}
		}

		public static bool IsDestroyed { get => isDestroyed; private set => isDestroyed = value; }
		public static bool IsInitialized { get => instance != null && !isDestroyed; }
		#endregion

		protected void Awake()
		{
			if (instance == this || instance == null)
			{
				isDestroyed = false;
				instance = this as T;
			}
			else
			{
				Destroy(this);
				return;
			}
			SingletonAwake();
		}

		protected abstract void SingletonAwake();
		
		protected virtual void OnApplicationQuit()
		{
			if (this == instance)
			{
				isDestroyed = true;
			}
		}
	}
}