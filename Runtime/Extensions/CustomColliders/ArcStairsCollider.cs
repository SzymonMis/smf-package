/*
* Copyright (C) 2023
* by Szymon Miś
* All rights reserved;
*/

using UnityEngine;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace SMF.Extensions
{
	/// <summary>
	/// Component in development
	/// </summary>
	//[AddComponentMenu("Physics/ArcStairsCollider")]
//	public class ArcStairsCollider : MonoBehaviour
//	{
//#if ODIN_INSPECTOR

//		[FoldoutGroup("References to gameobjects")]
//		[SerializeField]
//		private Transform colliderRoot;

//		[FoldoutGroup("References to gameobjects")]
//		[SerializeField]
//		private List<BoxCollider> colliders;

//		//[SerializeField]
//		//[OnValueChanged("OnBoxCountChange")]
//		//CylinderColliderType colliderType;

//		[Header("Common option")]
//		[OnValueChanged("GenerateCollider")]
//		[SerializeField]
//		[Range(0f, 360f)]
//		private float arcAngle = 90;

//		[OnValueChanged("GenerateCollider")]
//		[SerializeField]
//		private bool cutHalfsOfColliderOnArcEnds = true;

//		[OnValueChanged("GenerateCollider")]
//		[SerializeField]
//		private Vector3 offset;

//		private enum StairsType
//		{
//			MinRadiusOnTop,
//			MaxRadiusOnTop,
//		}

//		[Header("Steps generation")]
//		[OnValueChanged("GenerateSteps")]
//		[SerializeField]
//		private bool useStepGeneration;

//		[OnValueChanged("GenerateSteps")]
//		[SerializeField]
//		private StairsType stairsType = StairsType.MinRadiusOnTop;

//		[OnValueChanged("GenerateSteps")]
//		[SerializeField]
//		[Min(3)]
//		private int collidersInEachStep = 3;

//		[OnValueChanged("GenerateSteps")]
//		[SerializeField]
//		[Min(2)]
//		private int stepsCount = 2;

//		[OnValueChanged("GenerateSteps")]
//		[SerializeField]
//		[Min(0f)]
//		private float stepsMinRadius = 1;

//		[OnValueChanged("GenerateSteps")]
//		[SerializeField]
//		[Min(0f)]
//		private float stepsMaxRadius = 2;

//		[OnValueChanged("GenerateSteps")]
//		[SerializeField]
//		[Min(0f)]
//		private float stepsHeight = 0.15f;

//		[Space]
//		[SerializeField]
//		[OnValueChanged("GenerateCollider", true)]
//		private List<StepDescriptrion> stepsDescription;

//		[PropertySpace(SpaceBefore = 10)]
//		[Button]
//		private void GenerateCollider()
//		{
//			// destroying exisiting objects
//			ClearColliders();

//			if (stepsDescription == null || stepsDescription.Count == 0)
//			{
//				return;
//			}

//			Transform parentTransform = transform;
//			GameObject colliderRootGO = new GameObject("ArcColliderRoot");
//			colliderRoot = colliderRootGO.transform;

//			colliderRoot.parent = parentTransform;
//			colliderRoot.localPosition = offset;
//			colliderRoot.localRotation = Quaternion.identity;

//			int stepIndex = 0;

//			List<BoxCollider> boxColliders = new List<BoxCollider>();

//			float startHeight = 0f;
//			foreach (StepDescriptrion step in stepsDescription)
//			{
//				int boxCollidersCount = step.BoxCollidersCount;
//				if (boxCollidersCount < 2) { return; }

//				float innerRadius = step.MinRadius;
//				float radius = step.MaxRadius;
//				float height = step.Height;

//				GameObject stepRoot = new GameObject($"StepRoot_{stepIndex}");
//				stepRoot.transform.parent = colliderRoot;

//				Transform stepRootTransform = stepRoot.transform;
//				stepRootTransform.localRotation = Quaternion.identity;
//				stepRootTransform.localPosition = Vector3.up * (startHeight + height / 2f);
//				startHeight += height;

//				float collidersOnFullArc = 360f / arcAngle * (boxCollidersCount - 1);
//				float faceLength = 2f * Mathf.Max(radius, innerRadius) * Mathf.Tan(Mathf.PI / collidersOnFullArc);
//				float width = Mathf.Max(radius, innerRadius) - Mathf.Min(radius, innerRadius);

//				float forwardOffset = Mathf.Min(radius, innerRadius) + width / 2f;

//				Vector3 size = Vector3.up * height + Vector3.forward * width + Vector3.right * faceLength;

//				float angle = arcAngle / (boxCollidersCount - 1);

//				boxColliders.Clear();

//				for (int i = 0; i < boxCollidersCount; i++)
//				{
//					GameObject colliderGO = new GameObject($"Box_{i}");
//					colliderGO.transform.parent = stepRootTransform;

//					BoxCollider collider = colliderGO.AddComponent<BoxCollider>();
//					collider.size = size;

//					Quaternion newRot = Quaternion.AngleAxis(angle * i, Vector3.up);

//					Vector3 correctPos = newRot * Vector3.forward * forwardOffset;

//					colliderGO.transform.localRotation = newRot;
//					colliderGO.transform.localPosition = correctPos;

//					colliders.Add(collider);
//					boxColliders.Add(collider);
//				}

//				BoxCollider first = boxColliders[0];
//				BoxCollider last = boxColliders[boxColliders.Count - 1];

//				if (cutHalfsOfColliderOnArcEnds)
//				{
//					Vector3 sizeF = first.size;
//					sizeF.x = faceLength / 2f;
//					Vector3 centerF = first.center;
//					centerF.x = sizeF.x / 2f;

//					first.size = sizeF;
//					first.center = centerF;

//					Vector3 sizeL = last.size;
//					sizeL.x = faceLength / 2f;
//					Vector3 centerL = last.center;
//					centerL.x = -sizeL.x / 2f;

//					last.size = sizeL;
//					last.center = centerL;
//				}
//				else
//				{
//					first.center = Vector3.zero;
//					last.center = Vector3.zero;
//				}

//				stepIndex += 1;
//			}
//		}

//		[Button]
//		private void ClearColliders()
//		{
//			if (colliderRoot != null)
//			{
//				DestroyImmediate(colliderRoot.gameObject);
//			}

//			if (colliders != null)
//			{
//				colliders.Clear();
//			}
//			else
//			{
//				colliders = new List<BoxCollider>();
//			}
//		}

//		//private void OnBoxCountChange()
//		//{
//		//	GenerateCollider();
//		//}

//		//private void OnChangeRadius()
//		//{
//		//	if (colliders != null)
//		//	{
//		//		float collidersOnFullArc = 360f / arcAngle * (boxCollidersCount - 1);
//		//		float faceLength = 2f * Mathf.Max(radius, innerRadius) * Mathf.Tan(Mathf.PI / (collidersOnFullArc));
//		//		float width = Mathf.Max(radius, innerRadius) - Mathf.Min(radius, innerRadius);

//		//		float forwardOffset = Mathf.Min(radius, innerRadius) + width / 2f;

//		//		Vector3 size = Vector3.up * height + Vector3.forward * width + Vector3.right * faceLength;

//		//		float angle = arcAngle / (boxCollidersCount - 1);

//		//		int index = 0;
//		//		foreach (BoxCollider col in colliders)
//		//		{
//		//			Quaternion newRot = Quaternion.AngleAxis(angle * index, Vector3.up);

//		//			Vector3 correctPos = newRot * Vector3.forward * forwardOffset;

//		//			col.size = size;
//		//			col.transform.localPosition = correctPos;

//		//			index++;
//		//		}

//		//		BoxCollider first = colliders[0];
//		//		BoxCollider last = colliders[colliders.Count - 1];
//		//		if (cutHalfsOfColliderOnArcEnds)
//		//		{
//		//			Vector3 sizeF = first.size;
//		//			sizeF.x = faceLength / 2f;
//		//			Vector3 centerF = first.center;
//		//			centerF.x = sizeF.x / 2f;

//		//			first.size = sizeF;
//		//			first.center = centerF;

//		//			Vector3 sizeL = last.size;
//		//			sizeL.x = faceLength / 2f;
//		//			Vector3 centerL = last.center;
//		//			centerL.x = -sizeL.x / 2f;

//		//			last.size = sizeL;
//		//			last.center = centerL;
//		//		}
//		//		else
//		//		{
//		//			first.center = Vector3.zero;
//		//			last.center = Vector3.zero;
//		//		}
//		//	}
//		//}

//		//private void OnChangeHeight()
//		//{
//		//	if (colliders != null)
//		//	{
//		//		foreach (BoxCollider col in colliders)
//		//		{
//		//			Vector3 size = col.size;
//		//			size.y = height;
//		//			col.size = size;
//		//		}
//		//	}
//		//}

//		//private void OnOffsetChange()
//		//{
//		//	if (colliderRoot != null)
//		//	{
//		//		colliderRoot.localPosition = offset;
//		//	}
//		//}

//		//private void OnInnerRadisuChange()
//		//{
//		//	OnChangeRadius();
//		//}

//		//private void OnCutHalfCollidersOnArcEndsChange()
//		//{
//		//	OnChangeRadius();
//		//}

//		private void GenerateSteps()
//		{
//			if (useStepGeneration)
//			{
//				if (stepsDescription == null)
//				{
//					stepsDescription = new List<StepDescriptrion>();
//				}
//				else
//				{
//					stepsDescription.Clear();
//				}

//				float stepWidth = (stepsMaxRadius - stepsMinRadius) / (stepsCount);

//				for (int i = 0; i < stepsCount; i++)
//				{
//					float stepMaxRadius;

//					switch (stairsType)
//					{
//						case StairsType.MinRadiusOnTop:
//							{
//								stepMaxRadius = stepsMaxRadius - i * stepWidth;

//								stepsDescription.Add(new StepDescriptrion(
//									collidersInEachStep,
//									stepsHeight,
//									stepMaxRadius - stepWidth,
//									stepMaxRadius
//									));
//							}
//							break;

//						case StairsType.MaxRadiusOnTop:
//							{
//								stepMaxRadius = stepsMinRadius + i * stepWidth;

//								stepsDescription.Add(new StepDescriptrion(
//										collidersInEachStep,
//										stepsHeight,
//										stepMaxRadius - stepWidth,
//										stepMaxRadius
//									));
//							}
//							break;
//					}
//				}

//				GenerateCollider();
//			}
//		}

//#endif
//	}

//	[System.Serializable]
//	public class StepDescriptrion
//	{
//		[SerializeField]
//		[Min(3)]
//		public int BoxCollidersCount = 1;

//		[SerializeField]
//		[Min(0f)]
//		public float Height = 0.1f;

//		[SerializeField]
//		[Min(0f)]
//		public float MinRadius;

//		[SerializeField]
//		[Min(0f)]
//		public float MaxRadius;

//		public StepDescriptrion(int boxCollidersCount, float height, float minRadius, float maxRadius)
//		{
//			BoxCollidersCount = boxCollidersCount;
//			Height = height;
//			MinRadius = minRadius;
//			MaxRadius = maxRadius;
//		}
//	}
}