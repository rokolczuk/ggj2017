using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct EnemySpawnResult
{
	public readonly bool HasSpawned;

	public readonly Enemy SpawnedEnemy;

	public EnemySpawnResult(bool spawned, Enemy enemy)
	{
		this.SpawnedEnemy = enemy;
		this.HasSpawned = spawned;
		
	}
}

public class EnemiesFactory : MonoBehaviour 
{
	[SerializeField]
	private EnemiesConfiguration configuration;


	public EnemySpawnResult TrySpawn(float timePassed)
	{
		for(int i = 0; i < configuration.GetEnemySpawnProperties().Length; i++)
		{
			EnemySpawnProperties spawnProperties = configuration.GetEnemySpawnProperties()[i];

			if(spawnProperties.minSpawnTime <= timePassed && UnityEngine.Random.Range(0f, 1f) <= spawnProperties.spawnProbability)
			{
				GameObject enemyGameObject = GameObject.Instantiate(spawnProperties.prefab) as GameObject;
				Enemy enemy = enemyGameObject.GetComponent<Enemy>();
				Chord chord = spawnProperties.possibleChords[UnityEngine.Random.Range(0,spawnProperties.possibleChords.Count)];
				enemy.SetKillerChord(chord);
				return new EnemySpawnResult(true, enemy);
			}
		}

		return new EnemySpawnResult(false, null);
	}
}
