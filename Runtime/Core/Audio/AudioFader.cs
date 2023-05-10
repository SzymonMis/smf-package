/*
* Copyright (C) 2023
* by Szymon Miś
* All rights reserved;
*/

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioFader : MonoBehaviour
{
	private AudioSource audioSource;

	[SerializeField] private float fadeInTime = 1.0f;
	[SerializeField] private float fadeOutTime = 1.0f;
	[SerializeField] private bool autoFadeIn;
	[SerializeField] private bool autoFadeOut;
	private float currentVolume = 0.0f;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		audioSource.playOnAwake = false;
		audioSource.loop = false;
	}

	private void OnEnable()
	{
		if (autoFadeIn)
		{
			audioSource.volume = 0.0f;
			PlayFadeIn();
		}
		else
		{
			audioSource.volume = 1.0f;
			audioSource.Play();
		}		

		if (autoFadeOut)
		{
			PlayFadeOutDelayed(audioSource.clip.length - fadeOutTime);
		}
	}

	private void OnDisable()
	{
		audioSource.Stop();
		StopAllCoroutines();
	}

	/// <summary>
	/// Playing audio volume linear fade in
	/// </summary>
	public void PlayFadeIn()	
	{
		StartCoroutine(FadeIn());
	}

	/// <summary>
	/// Playing audio volume linear fade out delayed
	/// </summary>
	/// <param name="delay">Seconds to play from execution</param>
	public void PlayFadeOutDelayed(float delay)
	{
		StartCoroutine(FadeOutDelayed(delay));
	}

	/// <summary>
	/// Playing audio volume linear fade out 
	/// </summary>
	public void PlayFadeOut()
	{
		StartCoroutine(FadeOut());
	}

	private IEnumerator FadeIn()
	{
		audioSource.Play();

		while (currentVolume < 1.0f)
		{
			currentVolume += Time.deltaTime / fadeInTime;
			audioSource.volume = currentVolume;
			yield return null;
		}
	}

	private IEnumerator FadeOut()
	{
		while (currentVolume > 0.0f)
		{
			currentVolume -= Time.deltaTime / fadeOutTime;
			audioSource.volume = currentVolume;
			yield return null;
		}

		audioSource.Stop();
	}

	private IEnumerator FadeOutDelayed(float delay)
	{
		yield return new WaitForSeconds(delay);
		StartCoroutine(FadeOut());
	}
}