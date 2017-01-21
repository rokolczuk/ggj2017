using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour {

	public GameObject end;

	// Update is called once per frame
	void Update () {

		var lasers = GetComponentsInChildren<LaserMesh> ();

		foreach (LaserMesh laser in lasers) {
			laser.begin = transform;
			laser.end = end.transform;
		}
	}
}
