using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGun : MonoBehaviour 
{
	private KeyNoteData currentNote;

	private bool mouseButtonPressed = false;

	private AudioManager audioManager;
	private Enemy enemy;

	void transitionSound(KeyNoteData currentSound, KeyNoteData newSound){
		audioManager.transitionLaser (currentSound, newSound);
	}

	public void SetKeyNote(KeyNoteData noteData)
	{
		if(enemy != null)
		{
			enemy.RemoveActiveNote(noteData.keyNote);
		}
		transitionSound (currentNote, noteData);
		currentNote = noteData;

		if(enemy != null)
		{
			enemy.AddActiveNote(currentNote.keyNote);
		}
	}

	private void Awake()
	{
		audioManager = FindObjectOfType<AudioManager> ();
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

			if(hit.collider != null)
			{
				//TODO replace this with motherfucking lazers
				Debug.DrawLine(transform.position, hit.point);

				enemy = hit.collider.GetComponent<Enemy>();
				enemy.AddActiveNote(currentNote.keyNote);
			}
			else if(enemy != null)
			{
				enemy.RemoveActiveNote(currentNote.keyNote);
				enemy = null;
			}
		}
	}
}
