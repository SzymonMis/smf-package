using UnityEngine;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace SMF.Core
{
	/// <summary>
	/// Component based on OdinInspector which allows for creating stairs colliders
	/// </summary>
	[AddComponentMenu("Physics/NormalStairsCollider")]
	public class NormalStairsCollider : MonoBehaviour
	{
	#if ODIN_INSPECTOR

		/// <summary>
		/// Parent of objects with stair colliders
		/// </summary>
		[Tooltip("Parent of objects with stair colliders")]
		[FoldoutGroup("Game objects references")]
		[SerializeField]		
		private Transform collidersRoot;

		/// <summary>
		/// All colliders of which the stairs are made of
		/// </summary>
		[Tooltip("All colliders of which the stairs are made of")]
		[FoldoutGroup("Game objects references")]
		[SerializeField]
		private List<BoxCollider> colliders;

		/// <summary>
		/// Generates stairs
		/// </summary>
		[Tooltip("Generates stairs")]
		[OnValueChanged("GenerateColliders")]
		[FoldoutGroup("Stairs configuration")]
		[SerializeField]
		[Min(0)]
		private int stepsCount = 10;

		/// <summary>
		/// Height of one step
		/// </summary>
		[Tooltip("Height of one step")]
		[OnValueChanged("OnStepsSettingsChange")]
		[FoldoutGroup("Stairs configuration")]
		[SerializeField]
		[Min(0)]
		private float stepHeight;

		/// <summary>
		/// Width of one step
		/// </summary>
		[Tooltip("Width of one step")]
		[OnValueChanged("OnStepsSettingsChange")]
		[FoldoutGroup("Stairs configuration")]
		[SerializeField]
		[Min(0)]
		private float stepWidth;

		/// <summary>
		/// Depth of one step
		/// </summary>
		[Tooltip("Depth of one step")]
		[OnValueChanged("OnStepsSettingsChange")]
		[FoldoutGroup("Stairs configuration")]
		[SerializeField]
		[Min(0)]
		private float stepDepth;

		/// <summary>
		/// The distance by which the stair colliders overlap
		/// </summary>
		[Tooltip("The distance by which the stair colliders overlap")]
		[OnValueChanged("OnStepsSettingsChange")]
		[FoldoutGroup("Stairs configuration")]
		[SerializeField]
		[Min(0f)]
		private float overlapDepth = 0f;

		[FoldoutGroup("Stairs configuration")]
		[SerializeField]
		private Color colliderGizmoColor = new Color(1, 0.08627451f, 0, 0.5019608f);

		/// <summary>
		/// Generates stairs colliders for game object
		/// </summary>
		[Button]
		private void GenerateColliders()
		{			
			RemoveColliders();

			GameObject collidersRootGO = new GameObject("CollidersRoot");
			collidersRoot = collidersRootGO.transform;
			collidersRoot.parent = transform;
			collidersRoot.localPosition = Vector3.zero;
			collidersRoot.rotation = Quaternion.identity;

			float stepsDepth = stepsCount * stepDepth;


			Vector3 size = new Vector3(stepWidth, stepHeight, stepDepth + overlapDepth);

			float stepStartHeight = 0f;

			for (int i = 0; i < stepsCount; i++)
			{
				GameObject go = new GameObject($"Step_{i}");
				BoxCollider collider = go.AddComponent<BoxCollider>();
				collider.transform.parent = collidersRoot;

				colliders.Add(collider);

				Vector3 newColLocalPos = Vector3.zero;
				newColLocalPos.x = 0;

				newColLocalPos.y = stepStartHeight + stepHeight / 2f;
				stepStartHeight = stepHeight * (i + 1);

				newColLocalPos.z = stepsDepth - stepDepth / 2f - i * stepDepth - overlapDepth;

				collider.transform.localPosition = newColLocalPos;
				collider.transform.rotation = Quaternion.identity;

				collider.size = size;
			}
		}

		/// <summary>
		/// Removes Colliders from collidersRoot
		/// </summary>
		[Button]
		private void RemoveColliders()
		{
			if (collidersRoot != null)
			{
				DestroyImmediate(collidersRoot.gameObject);
			}

			if (colliders == null)
			{
				colliders = new List<BoxCollider>();
			}
			else
			{
				colliders.Clear();
			}
		}

		/// <summary>
		/// Recalculates colliders when values in inspector are changed
		/// </summary>
		private void OnStepsSettingsChange()
		{
			float stepsDepth = stepsCount * stepDepth;
			float stepStartHeight = 0f;
			Vector3 size = new Vector3(stepWidth, stepHeight, stepDepth + overlapDepth);


			if (colliders != null && colliders.Count == stepsCount)
			{
				for (int i = 0; i < stepsCount; i++)
				{
					BoxCollider collider = colliders[i];

					Vector3 newColLocalPos = Vector3.zero;
					newColLocalPos.x = 0;

					newColLocalPos.y = stepStartHeight + stepHeight / 2f;
					stepStartHeight = stepHeight * (i + 1);

					newColLocalPos.z = stepsDepth - stepDepth / 2f - i * stepDepth - overlapDepth / 2f;

					collider.transform.localPosition = newColLocalPos;
					collider.transform.rotation = Quaternion.identity;

					collider.size = size;
				}
			}
		}

		/// <summary>
		/// Draws gizmos for stairs colliders in editor
		/// </summary>
		private void OnDrawGizmos()
		{
			Gizmos.color = colliderGizmoColor;

			if (colliders == null)
				return;

			foreach (var collider in colliders)
			{
				Gizmos.DrawCube(collider.transform.position, collider.size);
			}			
		}
		#endif
	}
}