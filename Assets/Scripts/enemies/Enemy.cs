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

	private Vector3 speedVector;

	public bool IsDead {get { return dead; }}

	private bool dying;
	private bool dead;

	private float dyingTime;

	private Chord currentChord = new Chord();
	private Chord killerChord;

	public void SetKillerChord(Chord chord)
	{
		killerChord = chord;
		UpdateColor();
	}

	private void UpdateColor()
	{
		spriteRenderer.color = KeyManager.Instance.GetKeyData(killerChord.notesInChord[0]).activeColor;
	}

	private void Awake()
	{
		speedVector = new Vector3(0, speed, 0);
	}

	public void AddActiveNote(KeyNote n)
	{
		if(!currentChord.notesInChord.Contains(n))
		{
			currentChord.notesInChord.Add(n);
			dying = hasKillerChord();
		}
	}

	public void RemoveActiveNote(KeyNote n)
	{
		if(currentChord.notesInChord.Contains(n))
		{
			currentChord.notesInChord.Remove(n);
			dying = hasKillerChord();

			if(!dying)
			{
				dyingTime = 0;
			}
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

			if(dyingTime >= timeToKill)
			{
				Debug.Log(dyingTime+ " >= " + timeToKill);
				if(!dead)
				{
					dead = true;
					EventDispatcher.Dispatch<EnemyDiedEvent>(new EnemyDiedEvent(this));
				}
			}
		}
	}
}
