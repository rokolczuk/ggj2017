using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    public Color baseColor;
	public GameObject[] nodes;
    LaserMesh[] lasers;

	List<Transform> controlPoints = new List<Transform>();

    void Awake()
    {
        lasers = GetComponentsInChildren<LaserMesh>();

        KeyScript script = GetComponentInParent<KeyScript>();
        if (script)
        {
            lasers[1].color = script.getKeyData().activeColor;
            lasers[2].color = lasers[1].color + new Color(0.1f, 0.1f, 0.1f, 0);
        }
    }

    public void SetTarget(Transform target)
    {
		
		CreateControlPoints (target);
		Debug.Log ("Setting Target...");
		if (target != null) {
			SpringJoint2D joint = target.GetComponent<SpringJoint2D> ();
			var lastNode = nodes [nodes.Length-1];
			joint.connectedBody = lastNode.GetComponent<Rigidbody2D> ();
		}

        foreach (LaserMesh laser in lasers)
        {
			laser.SetControlPoints(controlPoints);
        }
    }

	void CreateControlPoints(Transform target){
		controlPoints.Clear ();
		if (target != null) {				
			for (int i = 0; i < nodes.Length; ++i) {
				controlPoints.Add (nodes [i].transform);
			}
			controlPoints.Add (target);
		}
	}
}
