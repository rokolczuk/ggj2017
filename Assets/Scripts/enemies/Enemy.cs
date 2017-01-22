using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Collider2D))]
public class Enemy : NetworkBehaviour 
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

    [SerializeField]
    private float DeathBoundary;

	[SerializeField]
	protected List<SpriteRenderer> enemyMaterialsSprites = new List<SpriteRenderer>();

	private List<Material> enemyMaterials = new List<Material>(); 

	private Vector3 speedVector;

	private const int maxParticleEmission = 60;

	public bool IsDead {get { return dead; }}
	public bool IsDeathAnimationCompleted {get { return !deathAnimationPlaying; }}

	private bool dying;
	private bool dead;
	private bool deathAnimationPlaying;

	private float dyingTime;

	private Chord currentChord = new Chord();

	public Chord killerChord;

	//private LaserGun trackingGun;

	public void SetKillerChord(Chord chord)
	{
		killerChord = chord;
	}

	[ClientRpc]
	public void RpcSetKillaCord(int[] list)
	{
		List<KeyNote> keyNotes = new List<KeyNote> ();

		for (int i = 0; i < list.Length; i++) {
			keyNotes.Add((KeyNote)list[i]);
		}

		killerChord.notesInChord = keyNotes;
		GetComponent<EnemySkinProvider>().SetSkin(killerChord);
	}
		
	private void Awake()
	{
		speedVector = new Vector3(0, speed, 0);

		for(int i = 0; i < enemyMaterialsSprites.Count; i++)
		{
			enemyMaterials.Add(enemyMaterialsSprites[i].material);
		}
	}


	public void AddActiveNote(KeyNote n, LaserGun trackingGun)
	{
		if(!currentChord.notesInChord.Contains(n))
		{
			currentChord.notesInChord.Add(n);
			//Debug.Log("curr: " + currentChord.ToString() + " / " + killerChord.ToString());
			dying = hasKillerChord();
			if (particles != null) {
				particles.gameObject.SetActive (dying);
			}
		}
	}

	public void RemoveActiveNote(KeyNote n)
	{
		if(currentChord.notesInChord.Contains(n))
		{
			currentChord.notesInChord.Remove(n);
			dying = hasKillerChord();

			if(particles != null)
			{
				particles.gameObject.SetActive(dying);
			}
		}
	}

	private bool hasKillerChord()
	{
		return currentChord == killerChord;
	}

	private void Update()
	{
		transform.position += speedVector * Time.deltaTime;

        CheckIfHitPlayers();

		if(dying)
		{
			dyingTime+= Time.deltaTime;
			float dyingProgress = dyingTime / timeToKill;

			for(int i = 0; i < enemyMaterials.Count; i++)
			{
				enemyMaterials[i].SetFloat("_Whiteness", dyingProgress); 
			}

			var em = particles.emission;
			em.rateOverTime = dyingProgress * maxParticleEmission;

			if(dyingTime >= timeToKill)
			{
				if(!dead)
				{
					dead = true;
					deathAnimationPlaying = true;
					deathAnimation.Play();

					GameObject.FindObjectOfType<AudioManager>().PlayEffect(dieSoundEffect);
					EventDispatcher.Dispatch<EnemyDiedEvent>(new EnemyDiedEvent(this));

					particles.transform.SetParent(transform.parent, true);
					em.rateOverTime = 0;

					EventDispatcher.Dispatch(new ParticlesDetachedEvent(particles));
					particles = null;
					enabled = false;

				}
			}
		}
	}

    private void CheckIfHitPlayers()
    {
        if (transform.position.y < DeathBoundary)
        {
            dead = true;
            deathAnimationPlaying = true;
            deathAnimation.Play();

            GameObject.FindObjectOfType<AudioManager>().PlayEffect(dieSoundEffect);
            EventDispatcher.Dispatch<EnemyCrashedEvent>(new EnemyCrashedEvent());

            particles.transform.SetParent(transform.parent, true);
            var em = particles.emission;
            em.rateOverTime = 0;

            EventDispatcher.Dispatch(new ParticlesDetachedEvent(particles));
            particles = null;
            enabled = false;
        }
    }

    public void OnDeathAnimationCompleted()
	{
		deathAnimationPlaying = false;

	}
}
