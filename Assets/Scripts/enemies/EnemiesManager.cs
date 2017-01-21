using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

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

    bool activated;

    public void Awake()
    {
        EventDispatcher.AddEventListener<GameStartedEvent>(OnGameStarted);
    }

    private void OnGameStarted(GameStartedEvent e)
    {
        activated = true;
    }

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

        NetworkServer.Spawn(spawnedEnemy.gameObject);
	}

	public void Update()
	{
        // note: server only object

        if (!activated)
            return;

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
				NetworkServer.Destroy(enemy.gameObject);
			}
		}
	}
}


