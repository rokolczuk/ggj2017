using System;
using UnityEngine;
using System.Collections.Generic;


public class EnemiesManager: MonoBehaviour
{
	[SerializeField]
	private EnemiesFactory enemiesFactory;

	private float timePassed = 0f;

	private List<Enemy> activeEnemies = new List<Enemy>();


	private void SpawnEnemies()
	{
		EnemySpawnResult spawnResult = enemiesFactory.TrySpawn(timePassed);

		if(spawnResult.HasSpawned)
		{
			Debug.Log("enemy spawned");
		}
	}

	public void Update()
	{
		timePassed+= Time.deltaTime;
		SpawnEnemies();
	}
}


