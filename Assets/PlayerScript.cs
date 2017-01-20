using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedKeyChanged
{
	public PlayerScript playerScript;
	public KeyScript keyScript;

	public SelectedKeyChanged(PlayerScript playerScript, KeyScript keyScript)
	{
		this.playerScript = playerScript;
		this.keyScript = keyScript;
	}
}

public class PlayerScript : MonoBehaviour
{
	public GameObject keyRayCast;

	private KeyScript keyScript;

	void Update()
	{
		Debug.DrawRay(transform.position, keyRayCast.transform.localPosition * Vector3.Distance(transform.position, keyRayCast.transform.position));

		checkForKey();

		float input = Input.GetAxis("Horizontal");
		if (input != 0)
		{
			move(input*Vector3.right);
		}

		if (Input.GetButtonDown("Jump"))
		{
			if (keyScript != null)
			{
				keyScript.fireKey();
			}
		}
	}
	
	private void move(Vector3 direction)
	{
		transform.position += direction;

		keyScript = null;
	}

	private void checkForKey()
	{
		RaycastHit2D[] raycastHit = Physics2D.RaycastAll(transform.position, keyRayCast.transform.localPosition);

		bool newKey = false;

		for (int i = 0; i < raycastHit.Length; i++)
		{
			if (raycastHit[i].collider != null)
			{
				if (raycastHit[i].collider.CompareTag("Key"))
				{
					newKey = true;
					KeyScript newKeyScript = raycastHit[i].collider.GetComponent<KeyScript>();
					if (keyScript != newKeyScript)
					{
						EventDispatcher.Dispatch(new SelectedKeyChanged(this, newKeyScript));
						keyScript = newKeyScript;
					}
				}
			}
		}

		if(!newKey)
		{
			EventDispatcher.Dispatch(new SelectedKeyChanged(this, null));
		}
	}
}
