﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour {

	public Color baseColor;

	void Awake() {	
		var	begin = transform;
		var end = FindObjectOfType<TrackMouse> ().gameObject.transform;
		var lasers = GetComponentsInChildren<LaserMesh> ();
		foreach (LaserMesh laser in lasers) {
			laser.SetBeginEnd (begin, end);
		}


		lasers [1].color = GetComponentInParent<KeyScript> ().getKeyData ().activeColor;
		lasers [2].color = lasers [1].color + new Color (0.1f, 0.1f, 0.1f, 0);
	}	
}
