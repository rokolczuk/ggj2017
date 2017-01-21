using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using AssemblyCSharp;

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

	EnemyWaveController waveController;

	private float timePassed = 0f;

	private List<Enemy> activeEnemies = new List<Enemy>();

    bool activated;

    public void Awake()
    {
        EventDispatcher.AddEventListener<GameStartedEvent>(OnGameStarted);
		waveController = gameObject.GetComponent<EnemyWaveController> ();
    }

    private void OnGameStarted(GameStartedEvent e)
    {
        activated = true;
    }

    private void SpawnEnemies()
	{
		var enemies = waveController.SpawnEnemies ();	
		foreach (Enemy e in enemies) {
			HandleEnemyCreation (e);
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
			if((activeEnemies[i].IsDead && activeEnemies[i].IsDeathAnimationCompleted) || activeEnemies[i].transform.position.y <= enemyDiePositionY)
			{
				Enemy enemy = activeEnemies[i];
				activeEnemies.Remove(enemy);
				enemy.clean ();
				NetworkServer.Destroy(enemy.gameObject);
			}
		}
	}
}


