using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using AssemblyCSharp;
using System;

public enum TrackName {
	A1_Synth,
	A2_Synth,
	B1_Synth,
	B2_Synth,
	C1_Synth,
	C2_Synth,
	D1_Synth,
	D2_Synth,
	E1_Synth,
	E2_Synth,
	F1_Synth,
	F2_Synth,
	G1_Synth,
	G2_Synth,
	A2_Piano,
	B2_Piano,
	C2_Piano,
	D2_Piano,
	E2_Piano,
	F2_Piano,
	G2_Piano,
	C0_Bass
}

public class AudioPool : MonoBehaviour {
	public int poolSize = 20;

	List<GameObject> pool;
	List<AudioTimer> playingSounds;

	// Use this for initialization
	void Awake () {
		playingSounds = new List<AudioTimer> ();
		preAllocPool ();
	}

	void preAllocPool(){
		pool = new List<GameObject> (poolSize);
		for (int i = 0; i < poolSize; i++) {
			GameObject obj = new GameObject();
			obj.AddComponent<AudioSource> ();
			AudioSource source = obj.GetComponent<AudioSource> ();
			source.Stop ();
			source.playOnAwake = false;
			source.loop = false;
			pool.Add(obj);
		}
	}

	AudioSource getFreeSource(){
		GameObject source = pool.FirstOrDefault(i => !i.GetComponent<AudioSource>().isPlaying);
		Debug.Assert (source != null, "AUDIO OBJECT POOL IS EMPTY, ALLOCATE MORE");
		return source.GetComponent<AudioSource>();
	}

	void onSoundEnd (AudioSource source, AudioTimer timer){
		if (!source.isPlaying) {
			playingSounds.Remove (timer);
		}
	}

	void trackPlayingSound(AudioSource source){
		AudioTimer timer = new AudioTimer(source, source.clip.length, onSoundEnd);
		playingSounds.Add (timer);
	}

	public void playTrack(AudioClip clip, bool looping){
		AudioSource source = getFreeSource ();
		source.clip = clip;
		source.loop = looping;
		source.Play();
		trackPlayingSound (source);
	}

	public void stopTrack(AudioClip clip){
		for (int i = 0; i < playingSounds.Count; i++) {
			var source = playingSounds [i].getAudioSource ();
			if (source.clip == clip){
				source.Stop ();
				playingSounds.Remove (playingSounds[i]);
				break;
			}
		}
	}

	void handlePlayingSounds(){
		float dt = Time.deltaTime;
		for (int i = 0; i < playingSounds.Count; i++) {
			playingSounds[i].Tick (dt);
		}
	}

	void Update () {
		handlePlayingSounds ();	
	}
}
