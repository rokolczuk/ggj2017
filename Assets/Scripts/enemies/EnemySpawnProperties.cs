using System;
using UnityEngine;

[Serializable]
public class EnemySpawnProperties 
{
	public float minSpawnTime;
	[Range(0f, 1f)]
	public float spawnProbability;
	public GameObject prefab;
}
