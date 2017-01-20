using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGun : MonoBehaviour 
{
	private KeyNote currentNote;

	private bool mouseButtonPressed = false;

	public void SetKeyNote(KeyNote keyNote)
	{
		currentNote = keyNote;
	}

	// Update is called once per frame
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
			RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, raycastDirection, 30f, 1 << 15);

			if(hit.collider != null)
			{
				Debug.DrawLine(transform.position, hit.point);
			}
		}
	}
}
