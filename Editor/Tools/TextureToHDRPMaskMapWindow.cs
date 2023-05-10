using UnityEditor;
using UnityEngine;

namespace SMF.Editor.Tools
{
	public class TextureToHDRPMaskMapWindow : EditorWindow
	{
		private Texture2D metallicTexture;
		private Texture2D smoothnessTexture;
		private Texture2D aoTexture;

		[MenuItem("SMF Tools/Convert Textures to HDRP Mask Map")]
		public static void ShowWindow()
		{
			EditorWindow.GetWindow(typeof(TextureToHDRPMaskMapWindow), false, "Convert Textures to HDRP Mask Map");
		}

		private void OnGUI()
		{
			GUILayout.Label("Source Textures", EditorStyles.boldLabel);
			metallicTexture = (Texture2D)EditorGUILayout.ObjectField("Metallic Texture", metallicTexture, typeof(Texture2D), false);
			smoothnessTexture = (Texture2D)EditorGUILayout.ObjectField("Smoothness Texture", smoothnessTexture, typeof(Texture2D), false);
			aoTexture = (Texture2D)EditorGUILayout.ObjectField("AO Texture", aoTexture, typeof(Texture2D), false);

			if (GUILayout.Button("Convert Textures"))
			{
				if (metallicTexture == null || smoothnessTexture == null || aoTexture == null)
				{
					EditorUtility.DisplayDialog("Error", "Please select all source textures.", "OK");
					return;
				}
				ConvertTexturesToHDRPMaskMap();
			}
		}

		private void ConvertTexturesToHDRPMaskMap()
		{
			Texture2D maskMapTexture = new Texture2D(metallicTexture.width, metallicTexture.height, TextureFormat.RGBA32, true);

			for (int x = 0; x < metallicTexture.width; x++)
			{
				for (int y = 0; y < metallicTexture.height; y++)
				{
					Color metallicColor = metallicTexture.GetPixel(x, y);
					Color smoothnessColor = smoothnessTexture.GetPixel(x, y);
					Color aoColor = aoTexture.GetPixel(x, y);

					Color maskMapColor = new Color(
						metallicColor.r,
						aoColor.r,
						1 - smoothnessColor.r,
						0
					);

					maskMapTexture.SetPixel(x, y, maskMapColor);
				}
			}

			maskMapTexture.Apply();

			string path = EditorUtility.SaveFilePanel("Save Mask Map", "", "MaskMap", "png");
			if (!string.IsNullOrEmpty(path))
			{
				byte[] pngData = maskMapTexture.EncodeToPNG();
				System.IO.File.WriteAllBytes(path, pngData);
				AssetDatabase.Refresh();
			}
		}
	}
}