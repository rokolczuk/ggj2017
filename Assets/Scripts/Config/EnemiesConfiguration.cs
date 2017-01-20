using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesConfiguration", menuName = "Enemies/Config", order = 1)]
public class EnemiesConfiguration : ScriptableObject 
{
	[SerializeField]
	private EnemySpawnProperties[] enemies;

	public EnemySpawnProperties[] GetEnemySpawnProperties()
	{
		return enemies;
	}
}
