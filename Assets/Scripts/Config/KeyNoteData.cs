
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Key Notes Configuration", menuName = "Key Notes/Key", order = 1)]

public class KeyNoteData: ScriptableObject
{
	public Color activeColor;
	public KeyNote keyNote;
	public AudioClip pianoSound;
	public AudioClip synthSound;
	[Range(1,3)]
	public int octave;
}
