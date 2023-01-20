using UnityEngine;

namespace SMF.Core
{
	public class UnparentOnEnable : MonoBehaviour
	{
		private void OnEnable() => transform.SetParent(null);
	}
}