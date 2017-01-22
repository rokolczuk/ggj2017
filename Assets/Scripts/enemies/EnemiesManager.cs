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
	private float spawnEnemyMinPositionY;

	[SerializeField]
	private float spawnEnemyMaxPositionY;


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

	private Vector2 GetRandomPosition()
	{
		return 	new Vector2(UnityEngine.Random.Range(minEnemySpawnPositionX, maxEnemySpawnPositionX), UnityEngine.Random.Range(spawnEnemyMinPositionY, spawnEnemyMaxPositionY));
	}

	private void HandleEnemyCreation(Enemy spawnedEnemy)
	{
		bool isIntersectingWithOtherEnemies = activeEnemies.Count > 0;

		int maxPlacementAttempts = 10;
		int currentPlacementAttemptIndex = 0;
	
		Collider2D spawnedEnemyCollider = spawnedEnemy.GetComponent<Collider2D>();

		spawnedEnemy.transform.position = GetRandomPosition();


		while(isIntersectingWithOtherEnemies)
		{
			spawnedEnemy.transform.position = GetRandomPosition();
			isIntersectingWithOtherEnemies = false;

			for(int i = 0; i < activeEnemies.Count; i++)
			{
				Collider2D other = activeEnemies[i].GetComponent<Collider2D>();
									
				if(spawnedEnemyCollider.bounds.Intersects(other.bounds))
				{
					isIntersectingWithOtherEnemies = true;
					break;
				}
			}

			if(++currentPlacementAttemptIndex > maxPlacementAttempts)
			{
				break;
			}
		}

		if(isIntersectingWithOtherEnemies)
		{
			Debug.LogWarning("failed to place the enemy after " + currentPlacementAttemptIndex + " attempts");
			Destroy(spawnedEnemy.gameObject);
		}
		else 
		{
			activeEnemies.Add(spawnedEnemy);
	        NetworkServer.Spawn(spawnedEnemy.gameObject);
			spawnedEnemy.RpcSetKillaCord (spawnedEnemy.killerChord.notesToNetworkForm());
		}
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
				NetworkServer.Destroy(enemy.gameObject);
			}
		}
	}
}


