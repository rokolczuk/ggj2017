using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour {

	public List<KeyScript> KeyList;

    private static KeyManager instance;
    public static KeyManager Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
    }
}
