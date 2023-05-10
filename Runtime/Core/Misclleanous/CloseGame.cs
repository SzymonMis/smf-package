using UnityEngine;

namespace SMF.Core
{
	public class CloseGame : MonoBehaviour
	{
#if UNITY_EDITOR

		public void Quit() => UnityEditor.EditorApplication.isPlaying = false;

#else

		public void Quit() => Application.Quit();

#endif
	}
}