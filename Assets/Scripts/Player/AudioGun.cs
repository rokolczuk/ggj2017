using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGun : MonoBehaviour 
{
	private KeyNote currentNote;

	private bool mouseButtonPressed = false;

	private Enemy enemy;

	public void SetKeyNote(KeyNote keyNote)
	{
		if(enemy != null)
		{
			enemy.RemoveActiveNote(keyNote);
		}

		currentNote = keyNote;

		if(enemy != null)
		{
			enemy.AddActiveNote(currentNote);
		}
	}

	private void Awake()
	{
		//TEMP
		SetKeyNote(KeyNote.A);
	}

	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			mouseButtonPressed = true;
		}
		if(Input.GetMouseButtonUp(0))
		{
			mouseButtonPressed = false;
		}
			
		if(mouseButtonPressed)
		{
			Vector2 raycastOrigin = new Vector2(transform.position.x, transform.position.y);
			Vector2 raycastDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, raycastDirection - raycastOrigin, 1000f, Layers.GetLayerMask(Layers.Enemies));

			//TODO replace this with motherfucking lazers
			Debug.DrawLine(transform.position, Input.mousePosition);

			if(hit.collider != null)
			{
				//TODO replace this with motherfucking lazers
				Debug.DrawLine(transform.position, hit.point);

				enemy = hit.collider.GetComponent<Enemy>();
				enemy.AddActiveNote(currentNote);
			}
			else if(enemy != null)
			{
				enemy.RemoveActiveNote(currentNote);
				enemy = null;
			}
		}
	}
}
