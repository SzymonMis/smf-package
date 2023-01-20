using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace SMF.Core
{
	public class BehaviourSystem : MonoBehaviour
	{
		public int Order { get; set; }

		public BehaviourSystem()
		{
			fixedUpdateInvoker = new FixedUpdateSystemInvoker();
			updateInvoker = new UpdateSystemInvoker();
			lateUpdateInvoker = new LateUpdateSystemInvoker();
		}

		public FixedUpdateSystemInvoker fixedUpdateInvoker;
		public UpdateSystemInvoker updateInvoker;
		public LateUpdateSystemInvoker lateUpdateInvoker;

		public void OnFixedUpdate(float fixedDeltaTime)
		{
			if (fixedUpdateInvoker.updateInvokers.Count > 0)
			{
				#if UNITY_EDITOR
				Profiler.BeginSample(fixedUpdateInvoker.ProfilerSampleName);
				#endif
				fixedUpdateInvoker.StretchFreeSpaces();
				fixedUpdateInvoker.Update(fixedDeltaTime);

				#if UNITY_EDITOR
				Profiler.EndSample();
				#endif
			}
		}

		public void OnUpdate(float deltaTime)
		{
			if (updateInvoker.updateInvokers.Count > 0)
			{
				#if UNITY_EDITOR
				Profiler.BeginSample(updateInvoker.ProfilerSampleName);
				#endif

				updateInvoker.StretchFreeSpaces();
				updateInvoker.Update(deltaTime);

				#if UNITY_EDITOR
				Profiler.EndSample();
				#endif
			}
		}

		public void OnLateUpdate(float deltaTime)
		{
			if (lateUpdateInvoker.updateInvokers.Count > 0)
			{
				#if UNITY_EDITOR
				Profiler.BeginSample(lateUpdateInvoker.ProfilerSampleName);
				#endif

				lateUpdateInvoker.StretchFreeSpaces();
				lateUpdateInvoker.Update(deltaTime);

				#if UNITY_EDITOR
				Profiler.EndSample();
				#endif
			}
		}

		public void AddMonoBehaviour(MonoBehaviour behaviour)
		{
			fixedUpdateInvoker.AddBehaviour(behaviour);
			updateInvoker.AddBehaviour(behaviour);
			lateUpdateInvoker.AddBehaviour(behaviour);
		}

		public void RemoveMonoBehaviour(MonoBehaviour behaviour)
		{
			fixedUpdateInvoker.RemoveBehavior(behaviour);
			updateInvoker.RemoveBehavior(behaviour);
			lateUpdateInvoker.RemoveBehavior(behaviour);
		}

		public void SetBehaviourEnabled(MonoBehaviour behaviour, bool enabled)
		{
			fixedUpdateInvoker.SetBehaviourEnabled(behaviour, enabled);
			updateInvoker.SetBehaviourEnabled(behaviour, enabled);
			lateUpdateInvoker.SetBehaviourEnabled(behaviour, enabled);
		}

		#if UNITY_EDITOR
		public void InitializeProfilersSampleNames(string systemName)
		{
			fixedUpdateInvoker.ProfilerSampleName = $"{systemName}::FixedUpdate()";
			updateInvoker.ProfilerSampleName = $"{systemName}::Update()"; ;
			lateUpdateInvoker.ProfilerSampleName = $"{systemName}::LateUpdate()";
		}
		#endif
	}

	public struct UpdatePair<T>
	{
		public MonoBehaviour Behaviour;
		public T Invoker;
		public bool IsValid;
		public bool IsEnabled;

		public UpdatePair(MonoBehaviour behaviour, T invoker)
		{
			IsValid = behaviour != null;
			IsEnabled = IsValid ? behaviour.enabled : false;
			Behaviour = behaviour;
			Invoker = invoker;
		}
	}

	public abstract class UpdateSystemInvokers<I> where I : class
	{
		protected internal List<UpdatePair<I>> updateInvokers;
		protected internal Dictionary<MonoBehaviour, int> updateIndexes;
		protected internal List<int> freeIndexes;

		protected UpdateSystemInvokers()
		{
			updateInvokers = new List<UpdatePair<I>>();
			updateIndexes = new Dictionary<MonoBehaviour, int>();
			freeIndexes = new List<int>();
		}

		public abstract void Update(float deltaTime);

		public void AddBehaviour(MonoBehaviour behaviour)
		{
			I invoker = behaviour as I;

			if (invoker != null)
			{
				if (!updateIndexes.ContainsKey(behaviour))
				{
					if (freeIndexes.Count > 0)
					{
						int lastIndexOfFreeIndexes = freeIndexes.Count - 1;

						int newIndex = freeIndexes[lastIndexOfFreeIndexes];
						updateInvokers[newIndex] = new UpdatePair<I>(behaviour, invoker);
						updateIndexes.Add(behaviour, newIndex);
						freeIndexes.RemoveAt(lastIndexOfFreeIndexes);
					}
					else
					{
						int index = updateInvokers.Count;
						updateInvokers.Add(new UpdatePair<I>(behaviour, invoker));
						updateIndexes.Add(behaviour, index);
					}
				}
			}
		}

		public void RemoveBehavior(MonoBehaviour behaviour)
		{
			int index;
			if (updateIndexes.TryGetValue(behaviour, out index))
			{
				updateIndexes.Remove(behaviour);
				updateInvokers[index] = new UpdatePair<I>(null, null);
				freeIndexes.Add(index);
			}
		}

		public void StretchFreeSpaces()
		{
			if (freeIndexes.Count < updateInvokers.Count / 2)
			{
				return;
			}

			for (int i = 0; i < freeIndexes.Count; i++)
			{
			}

			for (int i = updateInvokers.Count - 1; i >= 0; i--)
			{
				UpdatePair<I> pair = updateInvokers[i];
				if (pair.Behaviour == null)
				{
					updateInvokers.RemoveAt(i);
				}
			}

			freeIndexes.Clear();
			updateIndexes.Clear();

			for (int i = 0; i < updateInvokers.Count; i++)
			{
				updateIndexes.Add(updateInvokers[i].Behaviour, i);
			}
		}

		#if UNITY_EDITOR
		public string ProfilerSampleName;
		#endif

		public void SetBehaviourEnabled(MonoBehaviour behaviour, bool enabled)
		{
			I invoker = behaviour as I;
			if (invoker == null)
			{
				return;
			}

			if (updateIndexes.TryGetValue(behaviour, out int index))
			{
				UpdatePair<I> pair = updateInvokers[index];
				pair.IsEnabled = enabled;
				updateInvokers[index] = pair;
			}
		}
	}

	public class FixedUpdateSystemInvoker : UpdateSystemInvokers<IFixedUpdateInvoker>
	{
		public override void Update(float deltaTime)
		{
			int size = updateInvokers.Count;
			for (int i = 0; i < size; i++)
			{
				UpdatePair<IFixedUpdateInvoker> pair = updateInvokers[i];

				if (pair.IsValid && pair.IsEnabled)
				{
					pair.Invoker.OnFixedUpdate(deltaTime);
				}
			}
		}
	}

	public class UpdateSystemInvoker : UpdateSystemInvokers<IUpdateInvoker>
	{
		public override void Update(float deltaTime)
		{
			int size = updateInvokers.Count;
			for (int i = 0; i < size; i++)
			{
				UpdatePair<IUpdateInvoker> pair = updateInvokers[i];

				if (pair.IsValid && pair.IsEnabled)
				{
					pair.Invoker.OnUpdate(deltaTime);
				}
			}
		}
	}

	public class LateUpdateSystemInvoker : UpdateSystemInvokers<ILateUpdateInvoker>
	{
		public override void Update(float deltaTime)
		{
			int size = updateInvokers.Count;
			for (int i = 0; i < size; i++)
			{
				UpdatePair<ILateUpdateInvoker> pair = updateInvokers[i];

				if (pair.IsValid && pair.IsEnabled)
				{
					pair.Invoker.OnLateUpdate(deltaTime);
				}
			}
		}
	}
}