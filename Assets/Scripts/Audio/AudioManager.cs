using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class AudioManager : MonoBehaviour {

	const string laserModifier = "_synth";
	const string keyChangeModifier = "_piano";
	AudioPool pool;

	// Use this for initialization
	void Start () {
		pool = FindObjectOfType<AudioPool> ();
	}

	TrackName generateTrackname(KeyNoteData note, string modifer) {
		StringBuilder builder = new StringBuilder (note.ToString ());
		//builder.Append (note .ToString());
		builder.Append(modifer);
		TrackName trackName = (TrackName) Enum.Parse(typeof(TrackName), builder.ToString(), true);
		return trackName;
	}

	public void transitionLaser(KeyNoteData currentNote, KeyNoteData newNote){
		var currentTrackname = generateTrackname (currentNote, laserModifier); 
		pool.stopTrack (currentTrackname);
		var newTrackname = generateTrackname (newNote, laserModifier);
		pool.playTrack (newTrackname, true);
	}

	public void playLaser(KeyNote note, int octave){
		//TrackName track = generateTrackname (note, octave, laserModifier);
		//pool.playTrack (track);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
