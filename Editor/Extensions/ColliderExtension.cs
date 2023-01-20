using UnityEngine;
using UnityEditor;
using SMF.Extensions;

namespace SMF.Editor.Extension
{
	/// <summary>
	/// Creates a new option in the menu that allows the creation of colliders themselves
	/// </summary>
	public class CollidersExtension
	{
		#if UNITY_EDITOR
		/// <summary>
		/// Creates Box Collider
		/// </summary>
		[MenuItem("GameObject/Colliders/Box", false, 0)]
		private static void CreateBoxCollider()
		{
			GameObject colliderObject = new GameObject();
			colliderObject.AddComponent<BoxCollider>();
			SetStartPosition(colliderObject);
			colliderObject.name = "New Box Collider";
		}

		/// <summary>
		/// Creates Sphere Collider
		/// </summary>
		[MenuItem("GameObject/Colliders/Sphere", false, 0)]
		private static void CreateSphereCollider()
		{
			GameObject colliderObject = new GameObject();
			colliderObject.AddComponent<SphereCollider>();
			SetStartPosition(colliderObject);
			colliderObject.name = "New Sphere Collider";
		}

		/// <summary>
		/// Creates Capsule Collider
		/// </summary>
		[MenuItem("GameObject/Colliders/Capsule", false, 0)]
		private static void CreateCapsuleCollider()
		{
			GameObject colliderObject = new GameObject();
			colliderObject.AddComponent<CapsuleCollider>();
			SetStartPosition(colliderObject);
			colliderObject.name = "New Capsule Collider";
		}

		/// <summary>
		/// Creates Arc Collider
		/// </summary>
		[MenuItem("GameObject/Colliders/Arc", false, 0)]
		private static void CreateArcCollider()
		{
			GameObject colliderObject = new GameObject();
			colliderObject.AddComponent<ArcCollider>();
			SetStartPosition(colliderObject);
			colliderObject.name = "New Arc Collider";
		}

		/// <summary>
		/// Creates Cylinder Collider
		/// </summary>
		[MenuItem("GameObject/Colliders/Cylinder", false, 0)]
		private static void CreateCylinderCollider()
		{
			GameObject colliderObject = new GameObject();
			colliderObject.AddComponent<CylinderCollider>();
			SetStartPosition(colliderObject);
			colliderObject.name = "New Cylinder Collider";
		}

		/// <summary>
		/// Creates Stairs Collider
		/// </summary>
		[MenuItem("GameObject/Colliders/Stairs", false, 0)]
		private static void CreateNormalStairsCollider()
		{
			GameObject colliderObject = new GameObject();
			colliderObject.AddComponent<NormalStairsCollider>();
			SetStartPosition(colliderObject);
			colliderObject.name = "New Stairs Collider";
		}

		/// <summary>
		/// Sets starting position of game object
		/// </summary>
		/// <param name="target"></param>
		private static void SetStartPosition(GameObject target)
		{
			if (Selection.activeTransform != null)
			{
				target.transform.parent = Selection.activeTransform;
				target.transform.Translate(Selection.activeTransform.position);

				Selection.activeTransform.parent = target.transform;
			}
		}
		#endif
	}
}