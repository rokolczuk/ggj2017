using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour 
{
	[SerializeField]
	private Vector3 speed;

	private List<Note> currentChord = new List<Note>();

	private List<Note> killerChord = new List<Note>();

	public void AddActiveNote(Note n)
	{
		currentChord.Add(n);
		CheckKillCondition();
	}

	public void RemoveActiveNote(Note n)
	{
		currentChord.Add(n);
	}

	private void CheckKillCondition()
	{
		
	}
}
