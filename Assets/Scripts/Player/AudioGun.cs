using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioGun : MonoBehaviour 
{
	private KeyNoteData currentNote;

	[SerializeField]
	private bool mouseButtonPressed;

	private AudioManager audioManager;
	private Enemy enemy;
	private bool active;

	private void Awake()
	{
		audioManager = FindObjectOfType<AudioManager> ();
		mouseButtonPressed = false;
		active = false;
	}

	public void activateGun(bool active, KeyNoteData data){
		this.active = active;
		this.currentNote = data;

		if (Input.GetMouseButton (0)) {
			audioManager.playLaser (currentNote.synthSound);
		}
	}

	public void deactivateGun(bool active){
		audioManager.stopLaser (currentNote.synthSound);
	}

	public void toggleMouseDown(){
		if (Input.GetMouseButtonDown (0)) {
			mouseButtonPressed = true;
		}  else if (Input.GetMouseButtonUp (0)) {
			mouseButtonPressed = false;
		}
	}

	void Update () 
	{
		toggleMouseDown ();

		if(!active){
			return;
		}

		if (Input.GetMouseButtonDown (0)) {
			audioManager.playLaser (currentNote.synthSound);
		} else if (Input.GetMouseButtonUp (0)) {
			audioManager.stopLaser (currentNote.synthSound);
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
