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

	public bool IsDead {get { return dying; }}

	private bool dying;
	private bool dead;

	private float dyingTime;

	private List<KeyNote> currentChord = new List<KeyNote>();
	private List<KeyNote> killerChord = new List<KeyNote>();

	public void AddToKillerChord(KeyNote keyNote)
	{
		killerChord.Add(keyNote);
	}

	private void Awake()
	{
		speedVector = new Vector3(0, speed, 0);
	}

	public void AddActiveNote(KeyNote n)
	{
		if(!currentChord.Contains(n))
		{
			currentChord.Add(n);
			dying = hasKillerChord();
			Debug.Log("Enemy add note: " + n);
		}
	}

	public void RemoveActiveNote(KeyNote n)
	{
		if(currentChord.Contains(n))
		{
			currentChord.Remove(n);
			dying = hasKillerChord();

			if(!dying)
			{
				dyingTime = 0;
			}

			Debug.Log("Enemy remove note: " + n);
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

		Debug.Log("Enemy killer chord matched: " + currentChord);


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
					Debug.Log("Enemy died");

					EventDispatcher.Dispatch<EnemyDiedEvent>(new EnemyDiedEvent(this));
				}
			}
		}
	}
}
