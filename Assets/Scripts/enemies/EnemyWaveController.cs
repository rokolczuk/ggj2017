
using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AssemblyCSharp
{
	public class EnemyWaveController : MonoBehaviour
	{
		public float difficultyModifier = 1;
		public float timeBetweenWaves = 3.0f;
		public int wavesBetweenDifficultyBump = 4;
		public int difficultyBumpsBeforeEnemyAmountIncrease = 3;
		public int enemiesPerWave = 1;

		public GameObject oneChordPrefab;
		public GameObject twoChordPrefab;
		public GameObject threeChordPrefab;

		public List<LevelData> waveData;
		int currentLevel;

		float currentTime;
		int currentWave;
		int totalDifficultyIncreases;

		const string oneChordFilename = "OneChord";
		int oneChordIndex = 0;
		const string twoChordFilename = "TwoChords";
		int twoChordIndex = 0;
		const string threeChordFilename = "ThreeChords";
		int threeChordIndex = 0;
		List<List<Chord>> loadedChords;	

		void Awake(){
			preloadChords ();
			preloadLevelData ();
			EventDispatcher.AddEventListener<GameRestartEvent> (OnGameRestart);
			currentWave = 0;
			oneChordIndex = 0;
			twoChordIndex = 0;
			threeChordIndex = 0;
			currentLevel = 0;
			currentTime = waveData [currentLevel].timeBetweenWaves;
		}

		private void OnGameRestart(GameRestartEvent eventData)
		{
			currentWave = 0;
			oneChordIndex = 0;
			twoChordIndex = 0;
			threeChordIndex = 0;
			currentLevel = 0;
			currentTime = waveData [currentLevel].timeBetweenWaves;
		}

		Chord convertStringToChord(string chordString){
			List<KeyNote> notes = new List<KeyNote>();
			List<string> splitChords = chordString.Split (',').ToList();
			foreach (string s in splitChords) {
				KeyNote note = (KeyNote) Enum.Parse(typeof(KeyNote), s, true);
				notes.Add (note);
			}
			Chord c = new Chord ();
			c.notesInChord = notes;
			return c;
		}

		void preloadChords(){
			loadedChords = new List<List<Chord>> ();
			loadedChords.Add (loadChordList (oneChordFilename));
			loadedChords.Add (loadChordList (twoChordFilename));
			loadedChords.Add (loadChordList (threeChordFilename));
		}

		List<Chord> loadChordList(string file){
			List<Chord> chordList = new List<Chord>();
			TextAsset asset = Resources.Load(file) as TextAsset;
			List<string> splitLines = asset.text.Split ('\n').ToList();
			splitLines  = splitLines.Where(s => !string.IsNullOrEmpty(s)).ToList();
			foreach(var line in splitLines){
				var chord = convertStringToChord (line);
				chordList.Add (chord);
			}
			return chordList;
		}

		void preloadLevelData(){
			waveData = new List<LevelData> ();
			waveData.Add(new LevelData("Level2_waves"));
			waveData.Add(new LevelData("Level2_waves"));
			waveData.Add(new LevelData("Level3_waves"));
			waveData.Add(new LevelData("Level4_waves"));
			waveData.Add(new LevelData("Level5_waves"));
		}

		void Update(){
			currentTime -= Time.deltaTime;
		}

		List<Chord> getListForChordType(EnemyTypes type){
			switch (type) {
			case EnemyTypes.ONE:
				return loadedChords [0];
			case EnemyTypes.TWO:
				return loadedChords [1];
			case EnemyTypes.THREE:
				return loadedChords [2];
			}

			return null;
		}

		int getIndexForChordType(List<Chord> chordList, EnemyTypes type){
			//increment the index if we need to otherwise set it to 0
			int returnVal = 0;
			switch (type) {
			case EnemyTypes.ONE:
				returnVal = oneChordIndex;
				oneChordIndex++;
				if (oneChordIndex >= chordList.Count) {
					oneChordIndex = 0;
				}
				break;
			case EnemyTypes.TWO:
				returnVal = twoChordIndex;
				twoChordIndex++;
				if (twoChordIndex >= chordList.Count) {
					twoChordIndex = 0;
				}
				break;
			case EnemyTypes.THREE:
				returnVal = threeChordIndex;
				threeChordIndex++;
				if (threeChordIndex >= chordList.Count) {
					threeChordIndex = 0;
				}
				break;
			}
			return returnVal;
		}

		Chord newGetNextChordData(EnemyTypes type){
			var chordList = getListForChordType (type);
			var index = getIndexForChordType (chordList, type);
			return chordList[index];
		}

		GameObject getPrefabForChord(Chord chord){
			switch (chord.notesInChord.Count) {
			case 1:
				return oneChordPrefab;
			case 2:
				return twoChordPrefab;
			default:
				return threeChordPrefab;
			}
		}

		void handleWaveParams(){
			currentTime = waveData[currentLevel].timeBetweenWaves;
			waveData [currentLevel].incrementWave();
			if (waveData [currentLevel].isDone()) {
				currentLevel++;
			}
		}
			

		public List<Enemy> SpawnEnemies(){
			List<Enemy> enemies = new List<Enemy> ();
			if (currentTime >= 0) {
				return enemies;
			}
			handleWaveParams ();
			for (int i = 0; i < enemiesPerWave; i++) {
				var chord = newGetNextChordData (waveData[currentLevel].getNextEnemyType());
				var prefab = getPrefabForChord (chord);
				GameObject enemyGameObject = GameObject.Instantiate(prefab) as GameObject;
				Enemy enemy = enemyGameObject.GetComponent<Enemy>();
				enemy.SetKillerChord (chord);
				enemies.Add (enemy);
			}
			return enemies;
		}
	}
}

