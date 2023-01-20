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
	[AddComponentMenu("Physics/CylinderCollider")]
	public class CylinderCollider : MonoBehaviour
	{
		#if ODIN_INSPECTOR
		/// <summary>
		/// 
		/// </summary>
		[Tooltip("")]
		[FoldoutGroup("References to gameobjects")]
		[SerializeField]
		Transform colliderRoot;

		/// <summary>
		/// 
		/// </summary>
		[Tooltip("")]
		[FoldoutGroup("References to gameobjects")]
		[SerializeField]
		List<BoxCollider> colliders;

		/// <summary>
		/// 
		/// </summary>
		[Tooltip("")]
		[SerializeField]
		[OnValueChanged("OnBoxCountChange")]
		CylinderColliderType colliderType;

		/// <summary>
		/// 
		/// </summary>
		[Tooltip("")]
		[Header("Common option")]
		[OnValueChanged("OnBoxCountChange")]
		[SerializeField]
		[Min(3)]
		int boxCollidersCount = 4;

		/// <summary>
		/// 
		/// </summary>
		[Tooltip("")]
		[OnValueChanged("OnOffsetChange")]
		[SerializeField]
		private Vector3 offset;

		/// <summary>
		/// 
		/// </summary>
		[Tooltip("")]
		[OnValueChanged("OnChangeRadius")]
		[SerializeField]
		[Min(0f)]
		private float radius = 1;

		/// <summary>
		/// 
		/// </summary>
		[Tooltip("")]
		[OnValueChanged("OnChangeHeight")]
		[SerializeField]
		[Min(0f)]
		private float height = 1;

		/// <summary>
		/// 
		/// </summary>
		[Header("Empty inside cylinder:")]
		[OnValueChanged("OnInnerRadisuChange")]
		[SerializeField]
		[Min(0f)]
		private float innerRadius = 1f;

		/// <summary>
		/// 
		/// </summary>
		[Button]
		private void GenerateCollider()
		{
			//Destroying exisiting objects
			ClearColliders();

			Transform parentTransform = transform;
			GameObject colliderRootGO = new GameObject("CylinderColliderRoot");
			colliderRoot = colliderRootGO.transform;

			colliderRoot.parent = parentTransform;
			colliderRoot.localPosition = offset;
			colliderRoot.localRotation = Quaternion.identity;

			///Local variables needed for calculations
			float angle;
			float width;			
			float diameter;
			float faceLength;
			float forwardOffset;			
			Vector3 size;

			switch (colliderType)
			{
				case CylinderColliderType.FullCollider:
					if (boxCollidersCount == 1 || boxCollidersCount == 2)
					{
						GameObject colliderGO = new GameObject("Box");
						colliderGO.transform.parent = colliderRoot;
						colliderGO.transform.localPosition = Vector3.zero;
						colliderGO.transform.localRotation = Quaternion.identity;

						BoxCollider collider = colliderGO.AddComponent<BoxCollider>();
						collider.size = Vector3.up * height + Vector3.right * radius * 2f + Vector3.forward * radius * 2f;

						colliders.Add(collider);
					}
					else
					{
						faceLength = 2 * radius * Mathf.Tan(Mathf.PI / (boxCollidersCount * 2f));
						diameter = 2 * radius;
						size = Vector3.up * height + Vector3.forward * diameter + Vector3.right * faceLength;

						angle = 180f / boxCollidersCount;

						for (int i = 0; i < boxCollidersCount; i++)
						{
							GameObject colliderGO = new GameObject($"Box_{i}");
							colliderGO.transform.parent = colliderRoot;
							colliderGO.transform.localPosition = Vector3.zero;

							BoxCollider collider = colliderGO.AddComponent<BoxCollider>();
							collider.size = size;

							Quaternion newRot = Quaternion.AngleAxis(angle * i, Vector3.up);

							colliderGO.transform.localRotation = newRot;

							colliders.Add(collider);
						}
					}
					break;

				case CylinderColliderType.EmptyInside:					
					if (boxCollidersCount < 4)
					{
						return;
					}

					faceLength = 2f * Mathf.Max(radius, innerRadius) * Mathf.Tan(Mathf.PI / (boxCollidersCount));
					width = Mathf.Max(radius, innerRadius) - Mathf.Min(radius, innerRadius);

					forwardOffset = Mathf.Min(radius, innerRadius) + width / 2f;

					size = Vector3.up * height + Vector3.forward * width + Vector3.right * faceLength;

					angle = 360f / boxCollidersCount;

					for (int i = 0; i < boxCollidersCount; i++)
					{
						GameObject colliderGO = new GameObject($"Box_{i}");
						colliderGO.transform.parent = colliderRoot;

						BoxCollider collider = colliderGO.AddComponent<BoxCollider>();
						collider.size = size;

						Quaternion newRot = Quaternion.AngleAxis(angle * i, Vector3.up);

						Vector3 correctPos = newRot * Vector3.forward * forwardOffset;

						colliderGO.transform.localRotation = newRot;
						colliderGO.transform.localPosition = correctPos;

						colliders.Add(collider);
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
				switch (colliderType)
				{
					case CylinderColliderType.FullCollider:
						{
							float faceLength = 2 * radius * Mathf.Tan(Mathf.PI / (boxCollidersCount * 2f));
							float diameter = 2 * radius;

							foreach (BoxCollider col in colliders)
							{
								if (colliders.Count == 1 || colliders.Count == 2)
								{
									Vector3 size = col.size;
									size.x = radius * 2f;
									size.z = radius * 2f;
									col.size = size;
								}
								else
								{
									Vector3 size = col.size;
									size.x = faceLength;
									size.z = diameter;
									col.size = size;
								}
							}
						}
						break;
					case CylinderColliderType.EmptyInside:
						{
							float faceLength = 2f * Mathf.Max(radius, innerRadius) * Mathf.Tan(Mathf.PI / (boxCollidersCount));
							float width = Mathf.Max(radius, innerRadius) - Mathf.Min(radius, innerRadius);

							float forwardOffset = Mathf.Min(radius, innerRadius) + width / 2f;

							Vector3 size = Vector3.up * height + Vector3.forward * width + Vector3.right * faceLength;

							float angle = 360f / boxCollidersCount;

							int index = 0;
							foreach (BoxCollider col in colliders)
							{
								Quaternion newRot = Quaternion.AngleAxis(angle * index, Vector3.up);

								Vector3 correctPos = newRot * Vector3.forward * forwardOffset;

								col.size = size;
								col.transform.localPosition = correctPos;

								index++;
							}
						}
						break;
				}

			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void OnChangeHeight()
		{
			if (colliders == null)
				return;
			
			foreach (BoxCollider col in colliders)
			{
				Vector3 size = col.size;
				size.y = height;
				col.size = size;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void OnOffsetChange()
		{
			if (colliderRoot == null)
				return;
			
			colliderRoot.localPosition = offset;			
		}

		/// <summary>
		/// 
		/// </summary>
		private void OnInnerRadisuChange()
		{
			if (colliders == null)
				return;

			switch (colliderType)
			{
				case CylinderColliderType.EmptyInside:					
					float faceLength = 2f * Mathf.Max(radius, innerRadius) * Mathf.Tan(Mathf.PI / (boxCollidersCount));
					float width = Mathf.Max(radius, innerRadius) - Mathf.Min(radius, innerRadius);

					float forwardOffset = Mathf.Min(radius, innerRadius) + width / 2f;

					Vector3 size = Vector3.up * height + Vector3.forward * width + Vector3.right * faceLength;

					float angle = 360f / boxCollidersCount;

					int index = 0;
					foreach (BoxCollider col in colliders)
					{
						Quaternion newRot = Quaternion.AngleAxis(angle * index, Vector3.up);

						Vector3 correctPos = newRot * Vector3.forward * forwardOffset;

						col.size = size;
						col.transform.localPosition = correctPos;

						index++;							
					}
					break;				
			}
		}
#endif
	}

	public enum CylinderColliderType
	{
		FullCollider,
		EmptyInside
	}
}