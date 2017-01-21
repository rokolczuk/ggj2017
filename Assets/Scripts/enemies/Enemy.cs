using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour 
{
	[SerializeField]
	private float speed;

	[SerializeField]
	private float timeToKill;

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private ParticleSystem particles;

	[SerializeField]
	private AudioClip dieSoundEffect;

	[SerializeField]
	private Animation deathAnimation;


	private Material enemyMaterial;


	private Vector3 speedVector;

	private const int maxParticleEmission = 60;

	public bool IsDead {get { return dead; }}
	public bool IsDeathAnimationCompleted {get { return !deathAnimationPlaying; }}

	private bool dying;
	private bool dead;
	private bool deathAnimationPlaying;

	private float dyingTime;

	private Chord currentChord = new Chord();
	private Chord killerChord;

	private LaserGun trackingGun;

	public void SetKillerChord(Chord chord)
	{
		killerChord = chord;
		GetComponent<EnemySkinProvider>().SetSkin(chord);

	}
		
	private void Awake()
	{
		speedVector = new Vector3(0, speed, 0);
		enemyMaterial = spriteRenderer.material;
	}

	public void AddActiveNote(KeyNote n, LaserGun trackingGun)
	{
		if(!currentChord.notesInChord.Contains(n))
		{
			currentChord.notesInChord.Add(n);
			Debug.Log("curr: " + currentChord.ToString() + " / " + killerChord.ToString());
			dying = hasKillerChord();
			particles.gameObject.SetActive(dying);
		}
		this.trackingGun = trackingGun;
	}

	public void clean(){
		if (trackingGun != null) {
			trackingGun.clearTransform ();
		}
	}

	public void RemoveActiveNote(KeyNote n)
	{
		if(currentChord.notesInChord.Contains(n))
		{
			currentChord.notesInChord.Remove(n);
			dying = hasKillerChord();
			particles.gameObject.SetActive(dying);
		}
	}

	private bool hasKillerChord()
	{
		return currentChord == killerChord;
	}

	private void Update()
	{
		transform.position += speedVector;


		if(dying)
		{
			dyingTime+= Time.deltaTime;
			float dyingProgress = dyingTime / timeToKill;
			enemyMaterial.SetFloat("_Whiteness", dyingProgress); 

			var em = particles.emission;
			em .rateOverTime = dyingProgress * maxParticleEmission;

			if(dyingTime >= timeToKill)
			{
				if(!dead)
				{
					dead = true;
					deathAnimationPlaying = true;
					deathAnimation.Play();
					GameObject.FindObjectOfType<AudioManager>().PlayEffect(dieSoundEffect);
					EventDispatcher.Dispatch<EnemyDiedEvent>(new EnemyDiedEvent(this));

				}
			}
		}
	}

	public void OnDeathAnimationCompleted()
	{
		deathAnimationPlaying = false;

	}
}
