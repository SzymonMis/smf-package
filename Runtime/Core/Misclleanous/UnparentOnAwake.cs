using UnityEngine;

namespace SMF.Core
{
	public class UnparentOnAwake : MonoBehaviour
	{
		private void Awake() => transform.SetParent(null);
	}
}