/*
* Copyright (C) 2023
* by Szymon Miś
* All rights reserved;
*/

#if ODIN_INSPECTOR

using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

// Serializable class for localizing TextMeshPro font assets
[Serializable]
[DrawWithUnity]
public class LocalizedTMPFont : LocalizedAsset<TMP_FontAsset> { }

// Serializable class for UnityEvent with a TextMeshPro font asset parameter
[Serializable]
public class UnityEventTMPFont : UnityEvent<TMP_FontAsset> { }

// Component that localizes a TextMeshPro font asset and raises an event with the localized asset
[AddComponentMenu("Localization/Asset/Localize TMP Font Event")]
public class LocalizeTMPFontEvent : LocalizedAssetEvent<TMP_FontAsset, LocalizedTMPFont, UnityEventTMPFont>
{
#if UNITY_EDITOR

	// OnValidate is called when the script is loaded or a value is changed in the Inspector
	private void OnValidate()
	{
		// If there are already listeners on the OnUpdateAsset event, exit the method
		if (OnUpdateAsset.GetPersistentEventCount() > 0)
		{
			return;
		}

		// Get the TextMeshProUGUI component on the current game object
		TextMeshProUGUI target = gameObject.GetComponent<TextMeshProUGUI>();
		// Get the setter method for the "font" property of the TextMeshProUGUI component
		var setStringMethod = target.GetType().GetProperty("font").GetSetMethod();
		// Create a delegate for the setter method
		var methodDelegate = Delegate.CreateDelegate(typeof(UnityAction<TMP_FontAsset>), target, setStringMethod) as UnityAction<TMP_FontAsset>;
		// Add the delegate as a persistent listener on the OnUpdateAsset event
		UnityEditor.Events.UnityEventTools.AddPersistentListener(OnUpdateAsset, methodDelegate);
	}

#endif
}

// Serializable class for UnityEvent with a Material parameter
[Serializable]
internal class UnityEventMaterial : UnityEvent<Material>
{
}

// Component that localizes a Material asset and raises an event with the localized asset
internal class LocalizeFontMaterialEvent
	: LocalizedAssetEvent<Material, LocalizedMaterial, UnityEventMaterial>
{
}

#endif