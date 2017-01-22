using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BackgroundEqualizer : MonoBehaviour {

	private float[] spectrum = new float[256];

	[SerializeField]
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


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		int scale = spectrum.Length / buckets.Length;

		AudioListener.GetSpectrumData( spectrum, 0, FFTWindow.Hamming );

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

		backgroundSprite.color = Color.Lerp(Color.black, Color.white, Mathf.Min(maxLerpValue, Mathf.Max(minLerpValue, buckets[bucketIndex] * bucketValueMultiplier)));
	}
}
