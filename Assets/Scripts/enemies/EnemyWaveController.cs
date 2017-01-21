
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
		[SerializeField]
		float currentTime;
		public int enemiesPerWave = 1;

		public GameObject oneChordPrefab;
		public GameObject twoChordPrefab;

		string chordFilename = "";
		List<Chord> loadedChords;
		int chordIndex = 0;
	

		void Awake(){
			currentTime = timeBetweenWaves;
			chordFilename = Application.dataPath + "/WaveData/TwoPlayer.txt";
			preloadChords ();
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
			loadedChords = new List<Chord> ();
			string line = "";
			StreamReader reader = new StreamReader (chordFilename, System.Text.Encoding.Default);
			using (reader) {
				line = reader.ReadLine ();
				while (line != null) {
					var chord = convertStringToChord (line);
					loadedChords.Add (chord);
					line = reader.ReadLine ();
				}
				reader.Close();
			}
		}

		void Update(){
			print ("Current " + currentTime.ToString());
			print ("DT" + Time.deltaTime.ToString());
			currentTime -= Time.deltaTime;
		}

		Chord getNextChordData(){
			var c = loadedChords [chordIndex];
			chordIndex++;
			if (chordIndex >= loadedChords.Count) {
				chordIndex = 0;
			}
			return c;
		}

		GameObject getPrefabForChord(Chord chord){
			switch (chord.notesInChord.Count) {
			case 1:
				return oneChordPrefab;
			case 2:
				return twoChordPrefab;
			default:
				Debug.Assert (false, "Invalid chord count");
				return null;
			}
		}
			

		public List<Enemy> SpawnEnemies(){
			List<Enemy> enemies = new List<Enemy> ();
			if (currentTime >= 0) {
				return enemies;
			}
			print ("creating enemy");
			currentTime = timeBetweenWaves;
			print ("Current: " + currentTime.ToString());
			for (int i = 0; i < enemiesPerWave; i++) {
				var chord = getNextChordData ();
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

