using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public enum SfxOrigin
{
    LocalPlayer,
    RemotePlayer
}

public class AudioManager : MonoBehaviour {

	[SerializeField]
	private AudioClip music;

	[SerializeField]
	private float musicSlowMoPitch = 0.15f;

	[SerializeField]
	private float musicSlowMoDecreaseSpeed = 0.01f;

	AudioPool pool;

	AudioSource musicAudioSource;

	private List<AudioClip> activeSounds = new List<AudioClip>();

    void Awake()
    {
        EventDispatcher.AddEventListener<GameStartedEvent>(OnGameStarted);
    }

	// Use this for initialization
	void Start () {
		pool = FindObjectOfType<AudioPool> ();
	}

    private void OnGameStarted(GameStartedEvent e)
    {
        StartMusic();
    }

    public void setVolume(AudioClip clip, float volume)
    {
        if (activeSounds.Contains(clip))
        {
            pool.setVolume(clip, volume);
        }
    }

	public void playLaser(AudioClip clip, float volume)
    {
		if(!activeSounds.Contains(clip))
		{
			pool.playTrack (clip, true, volume);
			activeSounds.Add(clip);
		}
	}

	public void playPiano(AudioClip clip, SfxOrigin origin)
    {
        float volume;
        if (origin == SfxOrigin.LocalPlayer)
            volume = 0.5f;
        else
            volume = 0.20f;

		pool.playTrack (clip, false, volume);
		activeSounds.Add(clip);
	}

	public void stopLaser(AudioClip clip){
		if(activeSounds.Contains(clip))
		{
			pool.stopTrack (clip);
			activeSounds.Remove(clip);
		}
	}

	public void stopPiano(AudioClip clip){
		pool.stopTrack (clip);
		activeSounds.Remove(clip);
	}

	public void PlayEffect(AudioClip clip)
	{
		 pool.playTrack(clip, false, 1f);
	}

	public void StartMusic()
	{
		musicAudioSource = pool.playTrack(music, true, 0.35f);
	}

	public void StopMusic()
	{
		pool.stopTrack(music);
	}

	public void SlowDownMusic()
	{
		StartCoroutine(InternalSlowDownMusic());

	}

	public void SpeedUpMusic()
	{
		StartCoroutine(InternalSpeedUpMusic());

	}

	private IEnumerator InternalSlowDownMusic()
	{
		while(musicAudioSource.pitch > musicSlowMoPitch)
		{
			musicAudioSource.pitch -= musicSlowMoDecreaseSpeed;
			yield return new WaitForEndOfFrame();
		}
	}

	private IEnumerator InternalSpeedUpMusic()
	{
		while(musicAudioSource.pitch < 1)
		{
			musicAudioSource.pitch += musicSlowMoDecreaseSpeed;
			yield return new WaitForEndOfFrame();
		}

		musicAudioSource.pitch = 1f;
	}
}