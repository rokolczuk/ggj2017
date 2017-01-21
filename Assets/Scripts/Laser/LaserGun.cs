using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour {

	public Transform begin;
	public Transform end;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Transform child in transform) {
			child.GetComponent<LaserMesh> ().begin = begin;
			child.GetComponent<LaserMesh> ().end = end;
		}
	}
}
