using UnityEngine;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace SMF.Extensions
{
	/// <summary>
	///
	/// </summary>
	[AddComponentMenu("Physics/ArcCollider")]
	public class ArcCollider : MonoBehaviour
	{
		#if ODIN_INSPECTOR
		/// <summary>
		///
		/// </summary>
		[FoldoutGroup("References to gameobjects")]
		[SerializeField]
		private Transform colliderRoot;

		/// <summary>
		///
		/// </summary>
		[FoldoutGroup("References to gameobjects")]
		[SerializeField]
		private List<BoxCollider> colliders;

		/// <summary>
		///
		/// </summary>
		public enum ArcPivotPositionType
		{
			OnPivotCenter,
			OnArcCenter,
			OnArcStart
		}

		/// <summary>
		///
		/// </summary>
		[Header("Common option")]
		[OnValueChanged("OnChangePivotOnArcBegin")]
		[SerializeField]
		private ArcPivotPositionType arcPivotType = ArcPivotPositionType.OnPivotCenter;

		/// <summary>
		///
		/// </summary>
		[OnValueChanged("OnBoxCountChange")]
		[SerializeField]
		[Range(0f, 360f)]
		private float arcAngle = 90;

		/// <summary>
		///
		/// </summary>
		[OnValueChanged("OnCutHalfCollidersOnArcEndsChange")]
		[SerializeField]
		private bool cutHalfsOfColliderOnArcEnds = true;

		/// <summary>
		///
		/// </summary>
		[OnValueChanged("GenerateCollider")]
		[SerializeField]
		[Min(3)]
		private int boxCollidersCount = 3;

		/// <summary>
		///
		/// </summary>
		[OnValueChanged("OnOffsetChange")]
		[SerializeField]
		private Vector3 offset;

		/// <summary>
		///
		/// </summary>
		[OnValueChanged("OnInnerRadisuChange")]
		[SerializeField]
		[Min(0f)]
		private float arcWidth = 1f;

		/// <summary>
		///
		/// </summary>
		[OnValueChanged("OnChangearcHeight")]
		[SerializeField]
		[Min(0f)]
		private float arcHeight = 1;

		/// <summary>
		///
		/// </summary>
		[OnValueChanged("OnChangeRadius")]
		[SerializeField]
		[Min(0f)]
		private float maxRadius = 1;

		/// <summary>
		///
		/// </summary>
		[Button]
		private void GenerateCollider()
		{
			// destroying exisiting objects
			ClearColliders();

			Transform parentTransform = transform;
			GameObject colliderRootGO = new GameObject("ArcColliderRoot");
			colliderRoot = colliderRootGO.transform;

			colliderRoot.parent = parentTransform;

			colliderRoot.localPosition = offset;
			colliderRoot.localRotation = Quaternion.identity;

			if (boxCollidersCount < 3)
			{
				return;
			}

			float minRadius = maxRadius - arcWidth;

			float collidersOnFullArc = 360f / arcAngle * (boxCollidersCount - 1);
			float faceLength = 2f * Mathf.Max(maxRadius, minRadius) * Mathf.Tan(Mathf.PI / collidersOnFullArc);
			float width = Mathf.Max(maxRadius, minRadius) - Mathf.Min(maxRadius, minRadius);

			float forwardOffset = Mathf.Min(maxRadius, minRadius) + width / 2f;

			Vector3 size = Vector3.up * arcHeight + Vector3.forward * width + Vector3.right * faceLength;

			float angle = arcAngle / (boxCollidersCount - 1);

			for (int i = 0; i < boxCollidersCount; i++)
			{
				GameObject colliderGO = new GameObject($"Box_{i}");
				colliderGO.transform.parent = colliderRoot;

				BoxCollider collider = colliderGO.AddComponent<BoxCollider>();
				collider.size = size;

				ChangeColliderPositionBastedOnPivotType(collider, angle * i, forwardOffset);

				colliders.Add(collider);
			}

			BoxCollider first = colliders[0];
			BoxCollider last = colliders[colliders.Count - 1];

			if (cutHalfsOfColliderOnArcEnds)
			{
				Vector3 sizeF = first.size;
				sizeF.x = faceLength / 2f;
				Vector3 centerF = first.center;
				centerF.x = sizeF.x / 2f;

				first.size = sizeF;
				first.center = centerF;

				Vector3 sizeL = last.size;
				sizeL.x = faceLength / 2f;
				Vector3 centerL = last.center;
				centerL.x = -sizeL.x / 2f;

				last.size = sizeL;
				last.center = centerL;
			}
			else
			{
				first.center = Vector3.zero;
				last.center = Vector3.zero;
			}
		}

		/// <summary>
		///
		/// </summary>
		private void ChangeColliderPositionBastedOnPivotType(Collider col, float angle, float forwardOffset)
		{
			float minRadius = maxRadius - arcWidth;

			switch (arcPivotType)
			{
				case ArcPivotPositionType.OnPivotCenter:
					{
						Quaternion newRot = Quaternion.AngleAxis(angle, Vector3.up);
						Vector3 correctPos = newRot * Vector3.forward * forwardOffset;
						col.transform.localPosition = correctPos;
						col.transform.localRotation = newRot;
					}
					break;

				case ArcPivotPositionType.OnArcCenter:
					{
						Quaternion newRot = Quaternion.AngleAxis(angle - arcAngle / 2f, Vector3.up);
						Vector3 correctPos = newRot * Vector3.forward * forwardOffset;
						col.transform.localPosition = correctPos;
						col.transform.localRotation = newRot;
					}
					break;

				case ArcPivotPositionType.OnArcStart:
					{
						Quaternion newRot = Quaternion.AngleAxis(angle, Vector3.up);
						Vector3 correctPos = newRot * Vector3.forward * forwardOffset;
						col.transform.localPosition = correctPos - Vector3.forward * ((maxRadius - minRadius) / 2f + minRadius);
						col.transform.localRotation = newRot;
					}
					break;
			}
		}

		/// <summary>
		///
		/// </summary>
		[Button]
		private void ClearColliders()
		{
			if (colliderRoot != null)
			{
				DestroyImmediate(colliderRoot.gameObject);
			}

			if (colliders != null)
			{
				colliders.Clear();
			}
			else
			{
				colliders = new List<BoxCollider>();
			}
		}

		/// <summary>
		///
		/// </summary>
		private void OnBoxCountChange() => GenerateCollider();

		/// <summary>
		///
		/// </summary>
		private void OnChangeRadius()
		{
			if (colliders != null)
			{
				float minRadius = maxRadius - arcWidth;

				float collidersOnFullArc = 360f / arcAngle * (boxCollidersCount - 1);
				float faceLength = 2f * Mathf.Max(maxRadius, minRadius) * Mathf.Tan(Mathf.PI / (collidersOnFullArc));
				float width = Mathf.Max(maxRadius, minRadius) - Mathf.Min(maxRadius, minRadius);

				float forwardOffset = Mathf.Min(maxRadius, minRadius) + width / 2f;

				Vector3 size = Vector3.up * arcHeight + Vector3.forward * width + Vector3.right * faceLength;

				float angle = arcAngle / (boxCollidersCount - 1);

				int index = 0;
				foreach (BoxCollider col in colliders)
				{
					Quaternion newRot = Quaternion.AngleAxis(angle * index, Vector3.up);

					Vector3 correctPos = newRot * Vector3.forward * forwardOffset;

					col.size = size;
					col.transform.localPosition = correctPos;

					ChangeColliderPositionBastedOnPivotType(col, angle * index, forwardOffset);

					index++;
				}

				BoxCollider first = colliders[0];
				BoxCollider last = colliders[colliders.Count - 1];
				if (cutHalfsOfColliderOnArcEnds)
				{
					Vector3 sizeF = first.size;
					sizeF.x = faceLength / 2f;
					Vector3 centerF = first.center;
					centerF.x = sizeF.x / 2f;

					first.size = sizeF;
					first.center = centerF;

					Vector3 sizeL = last.size;
					sizeL.x = faceLength / 2f;
					Vector3 centerL = last.center;
					centerL.x = -sizeL.x / 2f;

					last.size = sizeL;
					last.center = centerL;
				}
				else
				{
					first.center = Vector3.zero;
					last.center = Vector3.zero;
				}
			}
		}

		/// <summary>
		///
		/// </summary>
		private void OnChangearcHeight()
		{
			if (colliders == null)
			{
				return;
			}

			foreach (BoxCollider col in colliders)
			{
				Vector3 size = col.size;
				size.y = arcHeight;
				col.size = size;
			}
		}

		/// <summary>
		///
		/// </summary>
		private void OnOffsetChange()
		{
			if (colliderRoot == null)
			{
				return;
			}

			colliderRoot.localPosition = offset;
		}

		/// <summary>
		///
		/// </summary>
		private void OnInnerRadisuChange() => OnChangeRadius();

		/// <summary>
		///
		/// </summary>
		private void OnCutHalfCollidersOnArcEndsChange() => OnChangeRadius();

		/// <summary>
		///
		/// </summary>
		private void OnChangePivotOnArcBegin() => OnChangeRadius();
		#endif
	}
}