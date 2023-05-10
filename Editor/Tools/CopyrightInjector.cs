namespace SMF.Editor.Tools
{
	using System.IO;
	using UnityEditor;
	using UnityEngine;

	public class CopyrightInjector : EditorWindow
	{
		private string folderPath = "";
		private string copyrightTemplate = "Copyright (C)";
		private string licenseTemplate = "All rights reserved;";
		private string authorName = "[Your Name]";
		private int copyrightYear = System.DateTime.Now.Year;
		private string copyrightNotice;
		private string injectedNotice;

		[MenuItem("SMF Tools/Copyright Injector")]
		public static void ShowWindow()
		{
			GetWindow<CopyrightInjector>("Copyright Injector");
		}

		private void OnGUI()
		{
			GUILayout.Label("Enter the folder path containing C# scripts", EditorStyles.boldLabel);
			folderPath = EditorGUILayout.TextField("Folder Path:", folderPath);

			GUILayout.Label("Enter the author name", EditorStyles.boldLabel);
			authorName = EditorGUILayout.TextField("Author Name:", authorName);

			GUILayout.Label("Enter the copyright year", EditorStyles.boldLabel);
			copyrightYear = EditorGUILayout.IntField("Year:", copyrightYear);

			GUILayout.Label("Enter the copyright template", EditorStyles.boldLabel);
			copyrightTemplate = EditorGUILayout.TextField("Template:", copyrightTemplate);

			GUILayout.Label("Enter the copyright license", EditorStyles.boldLabel);
			licenseTemplate = EditorGUILayout.TextField("Template:", licenseTemplate);

			if (GUILayout.Button("Inject Copyright"))
			{
				copyrightNotice = $"/*\n* {copyrightTemplate} {copyrightYear}\n* by {authorName}\n* {licenseTemplate}\n */";
				InjectCopyright();
			}

			if (GUILayout.Button("Revoke Copyright"))
			{
				copyrightNotice = $"/*\n* {copyrightTemplate} {copyrightYear}\n* by {authorName}\n* {licenseTemplate}\n */";
				RevokeCopyright();
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

				if (!fileContent.StartsWith(copyrightNotice))
				{
					string newFileContent = copyrightNotice + fileContent;
					File.WriteAllText(file.FullName, newFileContent);
				}
			}

			Debug.Log("Copyright notice injected.");
		}

		private void RevokeCopyright()
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

				if (fileContent.StartsWith(copyrightNotice))
				{
					string newFileContent = fileContent.Replace(copyrightNotice, "");
					File.WriteAllText(file.FullName, newFileContent);
				}
			}

			Debug.Log("Copyright notice revoked.");
		}
	}
}