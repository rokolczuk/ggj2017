using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGun : MonoBehaviour 
{
	private KeyNoteData currentNote;

	private bool mouseButtonPressed = false;

	private AudioManager audioManager;
	private Enemy enemy;
	private bool active;

	private void Awake()
	{
		audioManager = FindObjectOfType<AudioManager> ();
		active = false;
	}

	public void ActivateGun(bool active, KeyNoteData data){
		this.active = active;
		this.currentNote = data;
		if (!active && mouseButtonPressed) {
			audioManager.stopLaser (currentNote.synthSound);
			mouseButtonPressed = false;
		}
	}

	void Update () 
	{
		if(!active){
			return;
		}

		if(Input.GetMouseButtonDown(0))
		{
			audioManager.playLaser (currentNote.synthSound);
			mouseButtonPressed = true;
		}
		if(Input.GetMouseButtonUp(0))
		{
			audioManager.stopLaser (currentNote.synthSound);
			mouseButtonPressed = false;
		}
			
		if(mouseButtonPressed)
		{
			Vector2 raycastOrigin = new Vector2(transform.position.x, transform.position.y);
			Vector2 raycastDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, raycastDirection - raycastOrigin, 1000f, Layers.GetLayerMask(Layers.Enemies));

			//TODO replace this with motherfucking lazers
			Debug.DrawLine(transform.position, raycastDirection);

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
