using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class Chord 
{
	public List<KeyNote> notesInChord = new List<KeyNote>();

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

	public int[] notesToNetworkForm()
	{
		int[] shindler = new int[notesInChord.Count];

		for (int i = 0; i < notesInChord.Count; i++) {
			shindler [i] = (int)notesInChord[i];
		}

		return shindler;
	}
		
	public static bool operator !=(Chord x, Chord y) 
	{
		return !(x == y);
	}

	public string ToString()
	{
		string str = string.Empty;

		for(int i = 0; i < notesInChord.Count; i++)
		{
			str+=notesInChord[i];

			if(i + 1 < notesInChord.Count)
			{
				str += ",";
			}
		}
		return str;
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

