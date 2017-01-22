using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BackgroundEqualizer : MonoBehaviour {

	private float[] spectrum = new float[256];
	private float[] buckets = new float[8];

	[SerializeField]
	private SpriteRenderer backgroundSprite;

	[SerializeField]
	private int bucketIndex;

	[SerializeField]
	private float minLerpValue;

	[SerializeField]
	private float maxLerpValue;

	[SerializeField]
	private float bucketValueMultiplier;

	private bool gameStarted;

	private void Start()
	{
		SetAlpha(0f);
		EventDispatcher.AddEventListener<GameStartedEvent>(OnGameStarted);
		EventDispatcher.AddEventListener<GameOverEvent>(OnGameOver);
		EventDispatcher.AddEventListener<GameRestartEvent>(OnGameRestarted);
	}

	private void OnDestroy()
	{
		EventDispatcher.RemoveEventListener<GameStartedEvent>(OnGameStarted);
		EventDispatcher.RemoveEventListener<GameOverEvent>(OnGameOver);
		EventDispatcher.RemoveEventListener<GameRestartEvent>(OnGameRestarted);
	}
	private void OnGameStarted(GameStartedEvent e)
	{
		gameStarted = true;
	}

	private void OnGameOver(GameOverEvent e)
	{
		gameStarted = false;
	}

	void OnGameRestarted(GameRestartEvent e)
	{
		gameStarted = true;
	}

	void Update () {

		if(!gameStarted)
		{
			return;
		}

		int scale = spectrum.Length / buckets.Length;


		AudioListener.GetSpectrumData( spectrum, 0, FFTWindow.Hamming );

		if(AudioListener.pause)

		{
			Debug.Log("paused");
		}

		for(int i = 0; i < buckets.Length; i++)
		{
			buckets[i] = 0;
		}
			
		float maxBucket = 0;

		for( int i = 1; i < spectrum.Length-1; i++ )
		{
			int bucketIndex = Mathf.FloorToInt(i / scale);
			buckets[bucketIndex] += spectrum[i];
			maxBucket = Mathf.Max(maxBucket, buckets[bucketIndex]);
		}

		for(int i = 0; i < buckets.Length; i++)
		{
			buckets[i] /= maxBucket;
		}
			
		SetAlpha(Mathf.Max(minLerpValue, buckets[bucketIndex] * bucketValueMultiplier));

	}

	private void SetAlpha(float alpha)
	{
		Color c = backgroundSprite.color ;
		c.a = alpha;
		backgroundSprite.color = c;

	}
}
