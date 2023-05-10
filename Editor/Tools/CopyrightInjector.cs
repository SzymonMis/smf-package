using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace SMF.Editor.Tools
{
	public class CopyrightInjector : EditorWindow
	{
		private string folderPath = "";
		private string copyrightNotice = "Copyright (C) [Year] [Your Name]";

		[MenuItem("SMF Tools/Copyright Injector")]
		public static void ShowWindow()
		{
			GetWindow<CopyrightInjector>("Copyright Injector");
		}

		private void OnGUI()
		{
			GUILayout.Label("Enter the folder path containing C# scripts", EditorStyles.boldLabel);
			folderPath = EditorGUILayout.TextField("Folder Path:", folderPath);

			GUILayout.Label("Enter the copyright notice", EditorStyles.boldLabel);
			copyrightNotice = EditorGUILayout.TextArea(copyrightNotice, GUILayout.Height(60));

			if (GUILayout.Button("Inject Copyright"))
			{
				InjectCopyright();
			}
		}

		private void InjectCopyright()
		{
			if (string.IsNullOrEmpty(folderPath))
			{
				Debug.LogError("Invalid folder path.");
				return;
			}

			DirectoryInfo directory = new DirectoryInfo(folderPath);
			if (!directory.Exists)
			{
				Debug.LogError("Directory does not exist.");
				return;
			}

			FileInfo[] files = directory.GetFiles("*.cs", SearchOption.AllDirectories);
			foreach (FileInfo file in files)
			{
				string fileContent = File.ReadAllText(file.FullName);
				string classPattern = @"public( sealed| abstract)? class [\w]+";

				Match classMatch = Regex.Match(fileContent, classPattern);
				if (classMatch.Success)
				{
					int insertIndex = classMatch.Index;
					string newFileContent = fileContent.Insert(insertIndex, "// " + copyrightNotice + "\n\n");
					File.WriteAllText(file.FullName, newFileContent);
				}
			}

			Debug.Log("Copyright notice injected.");
		}
	}
}