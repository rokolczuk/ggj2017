using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class Chord 
{
	public List<KeyNote> notesInChord;

	public static bool operator ==(Chord x, Chord y) 
	{
		if(x.notesInChord.Count != y.notesInChord.Count)
		{
			return false;
		}

		for(int i = 0; i < x.notesInChord.Count; i++)
		{
			if(!y.notesInChord.Contains(x.notesInChord[i]))
			{
				return false;
			}
		}

		return true;

	}
		
	public static bool operator !=(Chord x, Chord y) 
	{
		return !(x == y);
	}
}

[Serializable]
public class EnemySpawnProperties 
{
	public float minSpawnTime;
	[Range(0f, 1f)]
	public float spawnProbability;
	public GameObject prefab;
	public List<Chord> possibleChords;
}

