using System;
using UnityEngine;
using System.Collections.Generic;


public class EnemiesManager: MonoBehaviour
{
	[SerializeField]
	private float minEnemySpawnPositionX;

	[SerializeField]
	private float maxEnemySpawnPositionX;

	[SerializeField]
	private float spawnEnemyPositionY;

	[SerializeField]
	private EnemiesFactory enemiesFactory;

	private float timePassed = 0f;

	private List<Enemy> activeEnemies = new List<Enemy>();


	private void SpawnEnemies()
	{
		EnemySpawnResult spawnResult = enemiesFactory.TrySpawn(timePassed);

		if(spawnResult.HasSpawned)
		{
			HandleEnemyCreation(spawnResult.SpawnedEnemy);
			Debug.Log("enemy spawned");
		}
	}

	private void HandleEnemyCreation(Enemy spawnedEnemy)
	{
		spawnedEnemy.transform.position = new Vector2();
	}

	public void Update()
	{
		timePassed+= Time.deltaTime;
		SpawnEnemies();
	}
}


