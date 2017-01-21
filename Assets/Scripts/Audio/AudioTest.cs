using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour {

	AudioPool audioPool;

	// Use this for initialization
	void Start () {
		audioPool = new AudioPool ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonUp("Jump"))
		{

		}
	}
}
