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
	private float enemyDiePositionY;

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
		}
	}

	private void HandleEnemyCreation(Enemy spawnedEnemy)
	{
		spawnedEnemy.transform.position = new Vector2(UnityEngine.Random.Range(minEnemySpawnPositionX, maxEnemySpawnPositionX), spawnEnemyPositionY);
		activeEnemies.Add(spawnedEnemy);
	}

	public void Update()
	{
		timePassed+= Time.deltaTime;

		SpawnEnemies();
		RemoveDeadEnemies();
	}

	void RemoveDeadEnemies()
	{
		for(int i = 0; i < activeEnemies.Count; i++)
		{
			if(activeEnemies[i].IsDead || activeEnemies[i].transform.position.y <= enemyDiePositionY)
			{
				Enemy enemy = activeEnemies[i];
				activeEnemies.Remove(enemy);
				Destroy(enemy.gameObject);
			}
		}
	}
}


