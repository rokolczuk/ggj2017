using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    public Color baseColor;

    LaserMesh[] lasers;

	List<GameObject> nodes = new List<GameObject>();
	List<Transform> controlPoints = new List<Transform>();

    void Awake()
    {
        lasers = GetComponentsInChildren<LaserMesh>();

		var nodeParent = transform.GetChild (3);
		foreach (Transform t in nodeParent.transform) {
			nodes.Add (t.gameObject);
		}

        KeyScript script = GetComponentInParent<KeyScript>();
        if (script)
        {
            lasers[1].color = script.getKeyData().activeColor;
            lasers[2].color = lasers[1].color + new Color(0.1f, 0.1f, 0.1f, 0);
        }
    }

	void Update(){
		if (controlPoints.Count > 0) {
			float dist = (controlPoints [controlPoints.Count - 1].position - controlPoints [0].position).magnitude / 2.0f;
			foreach (Transform t in controlPoints) {				
				var spring = t.GetComponent<SpringJoint2D> ();
				if (spring != null) {
					spring.distance = dist / (controlPoints.Count - 1);
				}
			}
		}
	}

    public void SetTarget(Transform target)
    {
		CreateControlPoints (target);
		if (target != null) {
			SpringJoint2D joint = target.GetComponent<SpringJoint2D> ();
			var lastNode = nodes [nodes.Count-1];
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
			for (int i = 0; i < nodes.Count; ++i) {
				controlPoints.Add (nodes [i].transform);
			}
			controlPoints.Add (target);
		}
	}
}
