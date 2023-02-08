using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace SMF.Editor.Extension
{
	[CustomEditor(typeof(Transform)), CanEditMultipleObjects]
	public class WorldTransformExtension : UIBaseAlignmentInspector
	{
		bool unfold = false;
		SerializedProperty worldPosition;
		SerializedProperty worldRotation;
		SerializedProperty worldScale;
		Transform worldTransform;

		public WorldTransformExtension() : base("TransformInspector") { }

		void OnEnable()
		{
			worldPosition = serializedObject.FindProperty("m_LocalPosition");
			worldRotation = serializedObject.FindProperty("m_LocalRotation");
			worldScale = serializedObject.FindProperty("m_LocalScale");
			worldTransform = target as Transform;

			if (EditorPrefs.HasKey("CustomWordTransformUnfold"))
				unfold = EditorPrefs.GetString("CustomWordTransformUnfold") == "True";
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (worldPosition.vector3Value != worldTransform.position || worldScale.vector3Value != worldTransform.transform.lossyScale)
			{				
				string originLabel = "Word Transform";
				unfold = EditorGUILayout.Foldout(unfold, originLabel);
				EditorPrefs.SetString("CustomWordTransformUnfold", unfold.ToString());
				if (unfold)
				{
					EditorGUILayout.Vector3Field("Position", RoundTo5th(worldTransform.position));
					EditorGUILayout.Vector3Field("Rotation", worldTransform.rotation.eulerAngles);
					EditorGUILayout.Vector3Field("Scale", RoundTo5th(worldTransform.lossyScale));
				}
			}
		}

		Vector3 RoundTo5th(Vector3 vector3) => new Vector3((float)System.Math.Round(vector3.x, 5), (float)System.Math.Round(vector3.y, 5), (float)System.Math.Round(vector3.z, 5));
	}

	public abstract class UIBaseAlignmentInspector : UnityEditor.Editor
	{
		private static readonly object[] EMPTY_ARRAY = new object[0];
		private System.Type AlignmentInspectorType;
		private System.Type editedObjectType;
		private UnityEditor.Editor editorInstance;

		private static Dictionary<string, MethodInfo> decoratedMethods = new Dictionary<string, MethodInfo>();
		private static Assembly editorAssembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));

		protected UnityEditor.Editor EditorInstance
		{
			get
			{
				if (editorInstance == null && targets != null && targets.Length > 0)
				{
					editorInstance = UnityEditor.Editor.CreateEditor(targets, AlignmentInspectorType);
				}
				if (editorInstance == null)
				{
					Debug.LogError("Could not create editor !");
				}

				return editorInstance;
			}
		}

		public UIBaseAlignmentInspector(string editorTypeName)
		{
			AlignmentInspectorType = editorAssembly.GetTypes().Where(t => t.Name == editorTypeName).FirstOrDefault();
			Init();
			var originalEditedType = GetCustomEditorType(AlignmentInspectorType);
			if (originalEditedType != editedObjectType)
			{
				throw new System.ArgumentException(
					 string.Format("Type {0} does not match the editor {1} type {2}",
								  editedObjectType, editorTypeName, originalEditedType));
			}
		}

		private System.Type GetCustomEditorType(System.Type type)
		{
			var flags = BindingFlags.NonPublic | BindingFlags.Instance;
			var attributes = type.GetCustomAttributes(typeof(CustomEditor), true) as CustomEditor[];
			var field = attributes.Select(editor => editor.GetType().GetField("m_InspectedType", flags)).First();
			return field.GetValue(attributes[0]) as System.Type;
		}

		private void Init()
		{
			var flags = BindingFlags.NonPublic | BindingFlags.Instance;
			var attributes = this.GetType().GetCustomAttributes(typeof(CustomEditor), true) as CustomEditor[];
			var field = attributes.Select(editor => editor.GetType().GetField("m_InspectedType", flags)).First();
			editedObjectType = field.GetValue(attributes[0]) as System.Type;
		}

		void OnDisable()
		{
			if (editorInstance != null)
			{
				DestroyImmediate(editorInstance);
			}
		}

		protected void CallInspectorMethod(string methodName)
		{
			MethodInfo method = null;
			if (!decoratedMethods.ContainsKey(methodName))
			{
				var flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
				method = AlignmentInspectorType.GetMethod(methodName, flags);
				if (method != null)
				{
					decoratedMethods[methodName] = method;
				}
				else
				{
					Debug.LogError(string.Format("Could not find method {0}", methodName));
				}
			}
			else
			{
				method = decoratedMethods[methodName];
			}
			if (method != null)
			{
				method.Invoke(EditorInstance, EMPTY_ARRAY);
			}
		}

		protected override void OnHeaderGUI() => CallInspectorMethod("OnHeaderGUI");
		public override void OnInspectorGUI() => EditorInstance.OnInspectorGUI();
		public override void DrawPreview(Rect previewArea) => EditorInstance.DrawPreview(previewArea);
		public override string GetInfoString() => EditorInstance.GetInfoString();
		public override GUIContent GetPreviewTitle() => EditorInstance.GetPreviewTitle();
		public override bool HasPreviewGUI() => EditorInstance.HasPreviewGUI();
		public override void OnInteractivePreviewGUI(Rect r, GUIStyle background) => EditorInstance.OnInteractivePreviewGUI(r, background);
		public override void OnPreviewGUI(Rect rect, GUIStyle guiStyle) => EditorInstance.OnPreviewGUI(rect, guiStyle);
		public override void OnPreviewSettings() => EditorInstance.OnPreviewSettings();
		public override void ReloadPreviewInstances() => EditorInstance.ReloadPreviewInstances();
		public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height) => EditorInstance.RenderStaticPreview(assetPath, subAssets, width, height);
		public override bool RequiresConstantRepaint() => EditorInstance.RequiresConstantRepaint();
		public override bool UseDefaultMargins() => EditorInstance.UseDefaultMargins();
	}
}