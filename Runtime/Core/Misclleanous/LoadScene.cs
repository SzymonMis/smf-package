/*
* Copyright (C) 2023
* by Szymon Miś
* All rights reserved;
*/

using UnityEngine;
using UnityEngine.SceneManagement;

namespace SMF.Core
{
	public class LoadScene : MonoBehaviour
	{
		public LoadSceneMode loadSceneMode = LoadSceneMode.Single;

		/// <summary>
		/// Load level by name
		/// </summary>
		/// <param name="sceneName"></param>
		public void LoadLevel(string sceneName) => SceneManager.LoadScene(sceneName, loadSceneMode);

		/// <summary>
		/// Load level by build index
		/// </summary>
		/// <param name="sceneIndex"></param>
		public void LoadLevel(int sceneIndex) => SceneManager.LoadScene(sceneIndex, loadSceneMode);
	}
}