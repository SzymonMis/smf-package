#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SMF.Editor.Tools
{
	[ExecuteInEditMode]
	public class BoxSelection : MonoBehaviour
	{		
		private BoxCollider selectionArea; // Reference to the box collider used for selection

		public LayerMask selectableLayers; // Layers to include in selection

		public List<GameObject> selectedObjects = new List<GameObject>(); // List of selected objects

		public Transform newParent;

		public Color volumeColor;

		[Button("1. Setup")]
		[ExecuteInEditMode]
		private void Setup()
		{
			if (selectionArea == null)
			{
				selectionArea = gameObject.AddComponent<BoxCollider>();
			}
		}

		[Button("2. Select GameObjects")]
		[ExecuteInEditMode]
		private void SelectGameObjects()
		{
			// Select all objects in the selectable layers within the selection area
			GameObject[] allObjects = FindObjectsOfType<GameObject>();

			foreach (GameObject selectedObject in allObjects)
			{
				GameObject obj = selectedObject;
				if (!selectedObjects.Contains(obj) && selectionArea.bounds.Contains(obj.transform.position) && obj != gameObject)
				{
					selectedObjects.Add(obj);
				}
			}
		}

		[Button("3. Set Parent")]
		[ExecuteInEditMode]
		private void SetParent()
		{
			foreach (var selectedObject in selectedObjects)
			{
				if (PrefabUtility.IsPartOfPrefabInstance(selectedObject))
				{
					GameObject root = PrefabUtility.GetOutermostPrefabInstanceRoot(selectedObject);

					if (newParent == null)
					{
						root.transform.SetParent(transform);
					}
					else
					{
						root.transform.SetParent(newParent);
					}
				}
				else
				{
					if (newParent == null)
					{
						selectedObject.transform.SetParent(transform);
					}
					else
					{
						selectedObject.transform.SetParent(newParent);
					}
				}
			}

			ClearGameObjectSelection();
		}

		[Button("4. Clear GameObjects Selection")]
		[ExecuteInEditMode]
		private void ClearGameObjectSelection() => selectedObjects.Clear();

		[Button("5. Cleanup")]
		[ExecuteInEditMode]
		private void CleanupSelector()
		{
			if(newParent == null)
			{
				DestroyImmediate(selectionArea);
				DestroyImmediate(this);
			}
			else
			{
				DestroyImmediate(gameObject);
			}
		}

		public void OnDrawGizmos()
		{
			if (selectionArea == null)
			{
				return;
			}

			Matrix4x4 rotationMatrix = Matrix4x4.TRS(selectionArea.transform.position, selectionArea.transform.rotation, selectionArea.transform.lossyScale);
			Gizmos.color = volumeColor;
			Gizmos.matrix = rotationMatrix;
			Gizmos.DrawCube(selectionArea.center, selectionArea.size);
		}
	}
}
#endif