using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum KeyNote
{
	A,
	B,
	C,
	D,
	E,
	F,
	G,
}

[Serializable]
public class KeyState
{
	public int octave;
	public KeyNote keyNote;
	public Color activedColor;
	public Color deactivatedColor;
}

public class KeyScript : MonoBehaviour
{
	public GameObject keyRayCast;

	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private KeyState keyState;

	private bool activated = false;

	private void Awake()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}

	private void Update()
	{
		checkForPlayer();
		renderKeyState();
	}

	private void checkForPlayer()
	{
		RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, keyRayCast.transform.position);
		if (raycastHit.collider != null)
		{
			if (raycastHit.collider.CompareTag("Player"))
			{
				activated = true;
				raycastHit.collider.GetComponent<PlayerScript>().setActiveKey(this);
			}
		}
	}

	private void renderKeyState()
	{
		if (activated)
		{
			spriteRenderer.color = keyState.activedColor;
		}
		else
		{
			spriteRenderer.color = keyState.deactivatedColor;
		}
	}
}
