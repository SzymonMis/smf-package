/*
* Copyright (C) 2023
* by Szymon Miś
* All rights reserved;
*/

using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SMF.Editor.Tools
{
	public class CodeLineCounterEditor : EditorWindow
	{
		private string folderPath = "Assets/";
		private int totalLines;

		[MenuItem("SMF Tools/Code Line Counter")]
		public static void ShowWindow()
		{
			GetWindow<CodeLineCounterEditor>("Code Line Counter");
		}

		private void OnGUI()
		{
			GUILayout.Label("Enter the folder path containing C# scripts", EditorStyles.boldLabel);
			folderPath = EditorGUILayout.TextField("Folder Path:", folderPath);

			if (GUILayout.Button("Count Lines"))
			{
				CountLines();
			}

			GUILayout.Label("Total Lines of Code: " + totalLines);
		}

		private void CountLines()
		{
			totalLines = 0;

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
				string[] lines = File.ReadAllLines(file.FullName);
				totalLines += lines.Count(line => !string.IsNullOrWhiteSpace(line) && !line.Trim().StartsWith("//"));
			}

			Debug.Log("Total lines of code: " + totalLines);
		}
	}
}