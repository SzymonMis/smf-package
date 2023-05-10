using UnityEngine;

namespace SMF.Core
{
	public class OpenLink : MonoBehaviour
	{
		public void LoadLink(string link) => Application.OpenURL(link);
	}
}