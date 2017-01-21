using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    public Color baseColor;
    LaserMesh[] lasers;

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
        foreach (LaserMesh laser in lasers)
        {
            laser.SetBeginEnd(transform, target);
        }
        
    }
}
