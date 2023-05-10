/*
* Copyright (C) 2023
* by Szymon Miś
* All rights reserved;
*/

using UnityEngine;
using UnityEditor;

namespace SMF.Editor.Tools
{	
	public static class RemoveMissingScripts
	{
		#if UNITY_EDITOR
		/// <summary>
		/// This function removes missing scripts in all of selected game objects
		/// </summary>
		[MenuItem("Edit/SMF/Remove Missing Scripts Recursively")]
		[MenuItem("GameObject/SMF/Remove Missing Scripts Recursively", false, 0)]
		private static void RemoveMissingScriptsInSelectedGameObjects()
		{
			var deepSelection = EditorUtility.CollectDeepHierarchy(Selection.gameObjects);
			int compCount = 0;
			int goCount = 0;
			foreach (var o in deepSelection)
			{
				if (o is GameObject go)
				{
					int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);
					if (count > 0)
					{
						// Edit: use undo record object, since undo destroy wont work with missing
						Undo.RegisterCompleteObjectUndo(go, "Remove missing scripts");
						GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
						compCount += count;
						goCount++;
					}
				}
			}

			Debug.Log($"Found and removed {compCount} missing scripts from {goCount} GameObjects");
		}
		#endif
	}
}