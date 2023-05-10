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
		private AudioSource audioSource;

		/// <summary>
		/// A dictionary that maps SoundType enum values to corresponding audio clips.
		/// </summary>
		[ShowInInspector]
		[Tooltip("Map sound types to corresponding audio clips")]
		private Dictionary<InteractionType, AudioClip> soundEffects = new Dictionary<InteractionType, AudioClip>();

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
		private void PlaySound(InteractionType soundType)
		{
			if (soundEffects.TryGetValue(soundType, out AudioClip clip))
			{
				audioSource.Stop();
				audioSource.PlayOneShot(clip);
			}
			else
			{
				return;
			}
		}

		public void Clicked() => PlaySound(InteractionType.Clicked);

		public void Hovered() => PlaySound(InteractionType.Hovered);

		public void Selected() => PlaySound(InteractionType.Selected);

		public void Unhovered() => PlaySound(InteractionType.Unhovered);

		public void Unselected() => PlaySound(InteractionType.Unselected);

		public void Submit() => PlaySound(InteractionType.Submit);

		[Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
		private void InitializeDictionary()
		{
			soundEffects.Clear();
			soundEffects.Add(InteractionType.Clicked, null);
			soundEffects.Add(InteractionType.Hovered, null);
			soundEffects.Add(InteractionType.Selected, null);
			soundEffects.Add(InteractionType.Submit, null);
		}
	}
}

#endif