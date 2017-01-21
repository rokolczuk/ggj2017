using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour {

	AudioPool audioPool;

	// Use this for initialization
	void Start () {
		audioPool = FindObjectOfType(typeof(AudioPool)) as AudioPool;
		//audioPool.playTrack (AudioPool.TrackName.A1_Synth);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonUp("Jump"))
		{
			audioPool.playTrack (AudioPool.TrackName.A1_Synth);	
			audioPool.playTrack (AudioPool.TrackName.A2_Synth);	
			audioPool.playTrack (AudioPool.TrackName.C1_Synth);	
			audioPool.playTrack (AudioPool.TrackName.C2_Synth);	
			audioPool.playTrack (AudioPool.TrackName.D1_Synth);	

		}
	}
}
