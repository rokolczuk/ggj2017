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

	private Vector3 speedVector;

	private bool dying;
	private bool dead;

	private float dyingTime;

	private List<KeyNote> currentChord = new List<KeyNote>();
	private List<KeyNote> killerChord = new List<KeyNote>();

	private void Activate()
	{
		speedVector = new Vector3(0, speed, 0);
	}

	public void AddActiveNote(KeyNote n)
	{
		currentChord.Add(n);
		dying = hasKillerChord();
	}

	public void RemoveActiveNote(KeyNote n)
	{
		currentChord.Add(n);
		dying = hasKillerChord();

		if(!dying)
		{
			dyingTime = 0;
		}
	}

	private bool hasKillerChord()
	{
		if(currentChord.Count != killerChord.Count)
		{
			return false;
		}

		for(int i = 0; i < killerChord.Count; i++)
		{
			if(!currentChord.Contains(killerChord[i]))
			{
				return false;
			}
		}

		return true;
	}

	private void Update()
	{
		transform.position += speedVector;

		if(dying)
		{
			dyingTime+= Time.deltaTime;

			if(dyingTime >= timeToKill)
			{
				if(!dead)
				{
					dead = true;
					EventDispatcher.Dispatch<EnemyDiedEvent>(new EnemyDiedEvent(this));
				}
			}
		}
	}
}
