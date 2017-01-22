using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TrackMouse : NetworkBehaviour {

	Rigidbody2D rb2d;
	public float force = 100;

	void Awake() {
		rb2d = GetComponent<Rigidbody2D> ();
	}

	void Update ()
    {
		if (NetworkServer.active) {
			if (hasAuthority) {
				Vector2 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				Vector2 direction = pos - rb2d.position;
				rb2d.AddForce (direction*force);

			}
		} else { // For mashs test scene with no network server
			Vector2 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Vector2 direction = pos - rb2d.position;
			rb2d.AddForce (direction*force);
		}
        
	}
}
