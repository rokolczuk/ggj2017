using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public enum KeyNote
{
	A, B, C, D, E, F, G
}

[Serializable]
public class KeyState
{
	public int octave;
	public KeyNoteData keyNoteData;
}

public class KeyScript : NetworkBehaviour
{
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private KeyState keyState;

	[SerializeField]
	private List<PlayerScript> playersOnKey = new List<PlayerScript>();

    [SyncVar]
    bool active;

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
		if (playersOnKey.Contains(selectedKey.playerScript) && selectedKey.keyScript != this) //wtf
		{
			playersOnKey.Remove(selectedKey.playerScript);
            if (playersOnKey.Count == 0)
                active = false;
		}
		else if (!playersOnKey.Contains(selectedKey.playerScript) && selectedKey.keyScript == this) //wtf
		{
			playersOnKey.Add(selectedKey.playerScript);
            active = true;
		}
	}

    private void renderKeyState()
    {
		spriteRenderer.color = active ? keyState.keyNoteData.activeColor : keyState.keyNoteData.inactiveColor;
    }

	public void fireKey()
	{
		AudioManager manager = FindObjectOfType<AudioManager> ();
		manager.playLaser (keyState.keyNoteData.keyNote, keyState.octave);
	}
}
