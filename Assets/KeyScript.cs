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
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private KeyState keyState;

	[SerializeField]
	private List<PlayerScript> playersOnKey = new List<PlayerScript>();
	
	private void Awake()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		EventDispatcher.AddEventListener<SelectedKeyChanged>(OnPlayerSelectedKey);
	}
	
	private void Update()
	{
		renderKeyState();
	}

	private void OnPlayerSelectedKey(SelectedKeyChanged selectedKey)
	{
		if (playersOnKey.Contains(selectedKey.playerScript) && selectedKey.keyScript != this)
		{
			//shut up
			playersOnKey.Remove(selectedKey.playerScript);	
		}
		else if (!playersOnKey.Contains(selectedKey.playerScript) && selectedKey.keyScript == this)
		{
			playersOnKey.Add(selectedKey.playerScript);
		}
	}
	
	private void renderKeyState()
	{
		if (playersOnKey.Count > 0)
		{
			spriteRenderer.color = keyState.activedColor;
		}
		else
		{
			spriteRenderer.color = keyState.deactivatedColor;
		}
	}

	public void fireKey()
	{
		print("StR8 fYr");
	}
}
