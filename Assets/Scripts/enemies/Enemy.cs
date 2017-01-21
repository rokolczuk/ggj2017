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

	private Material enemyMaterial;


	private Vector3 speedVector;

	private const int maxParticleEmission = 60;

	public bool IsDead {get { return dead; }}

	private bool dying;
	private bool dead;

	private float dyingTime;

	private Chord currentChord = new Chord();
	private Chord killerChord;

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

	public void AddActiveNote(KeyNote n)
	{
		if(!currentChord.notesInChord.Contains(n))
		{
			currentChord.notesInChord.Add(n);
			dying = hasKillerChord();
			particles.gameObject.SetActive(dying);
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
				Debug.Log(dyingTime+ " >= " + timeToKill);
				if(!dead)
				{
					dead = true;
					GameObject.FindObjectOfType<AudioManager>().PlayEffect(dieSoundEffect);
					EventDispatcher.Dispatch<EnemyDiedEvent>(new EnemyDiedEvent(this));
				}
			}
		}
	}
}
