#if ODIN_INSPECTOR

using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace SMF.Core
{
	/// <summary>
	/// This clasc handle sounds for Interactables
	/// Works only with odin inspector due to asigning dictionary in inspector
	/// </summary>
	[RequireComponent(typeof(UIInteractionHandler))]
	[RequireComponent(typeof(AudioSource))]
	public class UISounds : MonoBehaviour, IInteractable
	{
		public enum SoundType
		{
			Clicked,
			Hovered,
			Selected,
			Submit,
			None
		}

		private AudioSource audioSource;

		/// <summary>
		/// A dictionary that maps SoundType enum values to corresponding audio clips.
		/// </summary>
		[ShowInInspector]
		[Tooltip("Map sound types to corresponding audio clips")]
		private Dictionary<SoundType, AudioClip> soundEffects = new Dictionary<SoundType, AudioClip>();

		private void Awake()
		{
			audioSource = GetComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.loop = false;
			audioSource.Stop();
		}

		/// <summary>
		/// Plays the sound effect associated with the specified sound type.
		/// </summary>
		/// <param name="soundType">The type of sound to play.</param>
		private void PlaySound(SoundType soundType)
		{
			if (soundType == SoundType.None)
			{
				return;
			}

			if (soundEffects.TryGetValue(soundType, out AudioClip clip))
			{
				audioSource.Stop();
				audioSource.PlayOneShot(clip);
			}
		}

		public void Clicked() => PlaySound(SoundType.Clicked);

		public void Hovered() => PlaySound(SoundType.Hovered);

		public void Selected() => PlaySound(SoundType.Selected);

		public void Unhovered() => PlaySound(SoundType.None);

		public void Unselected() => PlaySound(SoundType.None);

		public void Submit() => PlaySound(SoundType.Submit);

		[Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
		private void InitializeDictionary()
		{
			soundEffects.Clear();
			soundEffects.Add(SoundType.Clicked, null);
			soundEffects.Add(SoundType.Hovered, null);
			soundEffects.Add(SoundType.Selected, null);
			soundEffects.Add(SoundType.Submit, null);
		}
	}
}

#endif