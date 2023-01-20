using SMF.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace SMF.Core
{
	public sealed class GlobalBehaviourUpdateManager : GenericSingleton<GlobalBehaviourUpdateManager>
	{
		private List<BehaviourSystem> behaviourSystems = new List<BehaviourSystem>();

		private Dictionary<string, BehaviourSystem> systemsDicitonary = new Dictionary<string, BehaviourSystem>();

		protected override void SingletonAwake()
		{

		}

		private void FixedUpdate()
		{
			float fixedDeltaTime = Time.fixedDeltaTime;

			if (fixedDeltaTime == 0)
			{
				return;
			}

			int size = behaviourSystems.Count;
			for (int i = 0; i < size; i++)
			{
				BehaviourSystem system = behaviourSystems[i];
				if (system.enabled)
				{
					system.OnFixedUpdate(fixedDeltaTime);
				}
			}
		}

		private void Update()
		{
			float deltaTime = Time.deltaTime;

			if (deltaTime == 0)
			{
				return;
			}

			int size = behaviourSystems.Count;
			for (int i = 0; i < size; i++)
			{
				BehaviourSystem system = behaviourSystems[i];
				if (system.enabled)
				{
					system.OnUpdate(deltaTime);
				}
			}
		}

		private void LateUpdate()
		{
			float deltaTime = Time.deltaTime;

			if (deltaTime == 0)
			{
				return;
			}

			int size = behaviourSystems.Count;
			for (int i = 0; i < size; i++)
			{
				BehaviourSystem system = behaviourSystems[i];
				if (system.enabled)
				{
					system.OnLateUpdate(deltaTime);
				}
			}
		}

		private void AddBehaviorSystem_I(BehaviourSystem system)
		{
			if (behaviourSystems == null)
			{
				behaviourSystems = new List<BehaviourSystem>();
			}

			if (behaviourSystems.Count == 0)
			{
				behaviourSystems.Add(system);
			}
			else
			{
				for (int i = 0; i < behaviourSystems.Count; i++)
				{
					if (behaviourSystems[i].Order > system.Order)
					{
						behaviourSystems.Insert(i, system);
						return;
					}
				}

				behaviourSystems.Add(system);
			}
		}

		private bool RemoveBehaviorSystem_I(BehaviourSystem system)
		{
			if (behaviourSystems == null)
			{
				return false;
			}

			systemsDicitonary.Remove(system.name);
			return behaviourSystems.Remove(system);
		}

		private BehaviourSystem AddBehaviour_I(MonoBehaviour behavior, string systemName, int order)
		{
			BehaviourSystem system;
			if (!systemsDicitonary.TryGetValue(systemName, out system))
			{
				GameObject newSystemGameObject = new GameObject(systemName);
				newSystemGameObject.transform.parent = transform;
				newSystemGameObject.transform.localPosition = Vector3.zero;
				newSystemGameObject.transform.localRotation = Quaternion.identity;

				system = newSystemGameObject.AddComponent<BehaviourSystem>();
				system.Order = order;

				AddBehaviorSystem_I(system);
				systemsDicitonary.Add(systemName, system);

				#if UNITY_EDITOR
				system.InitializeProfilersSampleNames(systemName);
				#endif
			}

			system.AddMonoBehaviour(behavior);

			return system;
		}

		private void RemoveBehavior_I(MonoBehaviour behavior, string systemName)
		{
			BehaviourSystem system;
			if (systemsDicitonary.TryGetValue(systemName, out system))
			{
				system.RemoveMonoBehaviour(behavior);
			}
		}

		public static BehaviourSystem AddBehaviour(MonoBehaviour behavior, string systemName, int order)
		{
			if (Instance == null)
			{
				return null;
			}

			return Instance.AddBehaviour_I(behavior, systemName, order);
		}

		public static void RemoveBehaviour(MonoBehaviour behavior, string systemName)
		{
			if (Instance == null)
			{
				return;
			}

			Instance.RemoveBehavior_I(behavior, systemName);
		}
	}
}