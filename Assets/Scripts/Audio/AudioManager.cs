﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class AudioManager : MonoBehaviour {
	AudioPool pool;

	private List<AudioClip> activeSounds = new List<AudioClip>();

	// Use this for initialization
	void Start () {
		pool = FindObjectOfType<AudioPool> ();
	}
		
	public void playLaser(AudioClip clip){
		if(!activeSounds.Contains(clip))
		{
			pool.playTrack (clip, true, 1.0f);
			activeSounds.Add(clip);
		}
	}

	public void playPiano(AudioClip clip){
		pool.playTrack (clip, false, 0.5f);
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
		
	// Update is called once per frame
	void Update () {
		
	}
}
