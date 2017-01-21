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
		
	public void playLaser(KeyNoteData data){
		pool.playTrack (data.synthSound, true);
	}

	public void stopLaser(KeyNoteData data){
		pool.stopTrack (data.synthSound);
	}
		
	// Update is called once per frame
	void Update () {
		
	}
}
