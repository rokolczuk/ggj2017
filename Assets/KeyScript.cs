using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum KeyNote
{
	A,
	B,
	C,
	D,
	E,
	F,
	G,
}

[Serializable]
public class KeyState
{
	public int octave;
	public KeyNote keyNote;
	public Color color;
}

public class KeyScript : MonoBehaviour
{
	[SerializeField]
	private KeyState keyState;

	private bool activated = false;


}
