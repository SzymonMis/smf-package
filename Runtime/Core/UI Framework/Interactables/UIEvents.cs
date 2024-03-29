/*
* Copyright (C) 2023
* by Szymon Miś
* All rights reserved;
*/

#if ODIN_INSPECTOR

using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SMF.Core
{
	public class UIElementEvents : SerializedMonoBehaviour, IInteractable
	{
		[Tooltip("Map sound types to corresponding audio clips")]
		[ShowInInspector]
		[OdinSerialize]
		private Dictionary<InteractionType, UnityEvent> eventMap = new Dictionary<InteractionType, UnityEvent>();

		private void InvokeEvent(InteractionType eventType)
		{
			if (eventMap.TryGetValue(eventType, out UnityEvent unityEvent))
			{
				unityEvent?.Invoke();
			}
			else
			{
				return;
			}
		}

		public void Clicked() => InvokeEvent(InteractionType.Clicked);

		public void Hovered() => InvokeEvent(InteractionType.Hovered);

		public void Selected() => InvokeEvent(InteractionType.Selected);

		public void Unhovered() => InvokeEvent(InteractionType.Unhovered);

		public void Unselected() => InvokeEvent(InteractionType.Unselected);

		public void Submit() => InvokeEvent(InteractionType.Submit);

#if UNITY_EDITOR

		[Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
		private void InitializeDictionary()
		{
			eventMap.Clear();
			eventMap.Add(InteractionType.Clicked, new UnityEvent());
			eventMap.Add(InteractionType.Hovered, new UnityEvent());
			eventMap.Add(InteractionType.Selected, new UnityEvent());
			eventMap.Add(InteractionType.Unhovered, new UnityEvent());
			eventMap.Add(InteractionType.Unselected, new UnityEvent());
			eventMap.Add(InteractionType.Submit, new UnityEvent());
		}

#endif
	}
}

#endif