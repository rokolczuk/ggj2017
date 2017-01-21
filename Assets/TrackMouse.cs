using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMouse : MonoBehaviour {

	void Update () {
		Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		pos.z = transform.position.z;
		transform.position = pos;
	}
}
