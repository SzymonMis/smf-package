using UnityEngine;
using UnityEngine.Localization.Settings;

namespace SMF.Core
{
	public class LocalizationSelector : MonoBehaviour
	{
		/// <summary>
		/// Sets language by letter code eg.: en, de, pl
		/// </summary>
		/// <param name="language"></param>
		public void SetLanguage(string language) => LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(language);
	}
}