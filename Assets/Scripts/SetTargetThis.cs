using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetThis : MonoBehaviour {

	// Use this for initialization
	void Start () {
		FindObjectOfType<LaserGun> ().SetTarget (transform);
	}
	

}
