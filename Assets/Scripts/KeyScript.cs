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

    [SerializeField]
    private SpriteRenderer unpressed;
    [SerializeField]
    private SpriteRenderer pressed;

    [SyncVar]
    bool active;

	private void Awake()
	{
		EventDispatcher.AddEventListener<SelectedKeyChanged>(OnPlayerSelectedKey);
	}

	private void OnPlayerSelectedKey(SelectedKeyChanged selectedKey)
	{
		if (playersOnKey.Contains(selectedKey.playerScript) && selectedKey.keyScript != this)
		{
			playersOnKey.Remove(selectedKey.playerScript);

            if (playersOnKey.Count == 0)
                active = false;
		}
		else if (!playersOnKey.Contains(selectedKey.playerScript) && selectedKey.keyScript == this)
		{
			playersOnKey.Add(selectedKey.playerScript);
            active = true;
		}
	}

    public void Update()
    {
        //TEMP CODE FOR TESTING WHAT IT LOOKS LIKE
        if (active)
        {
            pressed.gameObject.SetActive(true);
            unpressed.gameObject.SetActive(false);
        }
        else
        {
            pressed.gameObject.SetActive(false);
            unpressed.gameObject.SetActive(true);
        }
    }

	public void fireKey()
	{
		print("StR8 fYr");
        
	}
}
