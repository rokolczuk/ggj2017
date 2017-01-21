using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public enum SfxOrigin
{
    LocalPlayer,
    RemotePlayer,
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
        // TODO: check origin for volume brah

        if (!activeSounds.Contains(clip))
        {
            pool.playTrack(clip, true);
            activeSounds.Add(clip);
        }
	}

	public void playPiano(AudioClip clip, SfxOrigin origin)
    {
        if (origin != SfxOrigin.LocalPlayer)
            return;

		pool.playTrack (clip, false);
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
}
