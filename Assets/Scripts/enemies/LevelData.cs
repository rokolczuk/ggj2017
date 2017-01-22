using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AssemblyCSharp
{
	public enum EnemyTypes{
		ONE = 1,
		TWO = 2,
		THREE = 3
	}
		
	public class LevelData
	{
		const int totalWavesIndex = 0;
		const int amountOfEnemiesSpawnedIndex = 1;
		const int timeBetweenWavesIndex = 2;

		public int amountOfEnemiesSpawned;
		public float timeBetweenWaves;
		public int totalWaves;

		public List<EnemyTypes> enemyTypes;
		int currentWave = -1;


		public LevelData (string dataFile)
		{
			enemyTypes = new List<EnemyTypes> ();
			loaData (dataFile);
		}

		void loaData(string file){
			TextAsset asset = Resources.Load(file) as TextAsset;
			List<string> splitLines = asset.text.Split ('\n').ToList();
			totalWaves = int.Parse(splitLines [totalWavesIndex].Split (':') [1].Trim ());
			amountOfEnemiesSpawned = int.Parse(splitLines [amountOfEnemiesSpawnedIndex].Split (':') [1].Trim ());
			timeBetweenWaves = float.Parse(splitLines [timeBetweenWavesIndex].Split (':') [1].Trim ());
			splitLines.RemoveRange (0, 3);
			splitLines  = splitLines.Where(s => !string.IsNullOrEmpty(s)).ToList();
			foreach(var line in splitLines){
				EnemyTypes type = (EnemyTypes)int.Parse (line);
				enemyTypes.Add (type);
			}
		}

		public EnemyTypes getNextEnemyType(){
			if (currentWave >= enemyTypes.Count) {
				currentWave = 0;
			}
			return enemyTypes [currentWave];
		}

		public void incrementWave(){
			currentWave++;
		}

		public bool isDone(){
			return currentWave >= totalWaves;
		}
	}
}

