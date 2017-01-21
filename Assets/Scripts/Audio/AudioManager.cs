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
	AudioPool pool;

	private List<AudioClip> activeSounds = new List<AudioClip>();

	// Use this for initialization
	void Start () {
		pool = FindObjectOfType<AudioPool> ();
	}
		
	public void playLaser(AudioClip clip, SfxOrigin origin)
    {
		if(!activeSounds.Contains(clip))
		{
			pool.playTrack (clip, true, 1.0f);
			activeSounds.Add(clip);
		}
	}

	public void playPiano(AudioClip clip, SfxOrigin origin)
    {
        float volume;
        if (origin == SfxOrigin.LocalPlayer)
            volume = 0.5f;
        else
            volume = 0.1f;

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

	public void PlayEffect(AudioClip clip){
	{
		 pool.playTrack(clip, false, 1f);
	}

	}
}