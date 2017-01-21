using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class AudioManager : MonoBehaviour {
	AudioPool pool;

	// Use this for initialization
	void Start () {
		pool = FindObjectOfType<AudioPool> ();
	}
		
	public void playLaser(AudioClip clip){
		pool.playTrack (clip, true);
	}

	public void stopLaser(AudioClip clip){
		pool.stopTrack (clip);
	}
		
	// Update is called once per frame
	void Update () {
		
	}
}
