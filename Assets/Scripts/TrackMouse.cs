using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TrackMouse : NetworkBehaviour {

	Rigidbody2D rb2d;

	void Awake() {
		rb2d = GetComponent<Rigidbody2D> ();
	}

	void Update ()
    {		
		if (hasAuthority) {
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			//pos.z = transform.position.z;
			rb2d.position = pos;
		} 
	}
}
