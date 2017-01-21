using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour {

	public Color baseColor;
	LaserMesh[] lasers;
	Transform mouseTransform;

	void Awake() {
		mouseTransform = FindObjectOfType<TrackMouse> ().gameObject.transform;
		lasers = GetComponentsInChildren<LaserMesh> ();
		foreach (LaserMesh laser in lasers) {
			laser.SetBeginEnd (transform, mouseTransform);
		}

		KeyScript script = GetComponentInParent<KeyScript> ();
		if (script) {
			lasers [1].color = script.getKeyData ().activeColor;
			lasers [2].color = lasers [1].color + new Color (0.1f, 0.1f, 0.1f, 0);
		}
	}

	public void SetTarget(Transform target){	
		if (target != null) {
			foreach (LaserMesh laser in lasers) {
				laser.SetBeginEnd (transform, target);
			}
		} 
		clearTransform();
	}

	public void clearTransform(){
		foreach (LaserMesh laser in lasers) {
			laser.SetBeginEnd (transform, mouseTransform);
		}
	}
}
