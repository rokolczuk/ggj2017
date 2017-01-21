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
	public KeyNoteData keyNoteData;
}

public class KeyScript : NetworkBehaviour
{
	[SerializeField]
	private KeyState keyState;

	[SerializeField]
	private List<PlayerScript> playersOnKey = new List<PlayerScript>();

    [SerializeField]
    private GameObject unpressed;
    [SerializeField]
    private GameObject pressed;

    [SyncVar]
    bool active;

	private AudioGun audioGun;

	private void Awake()
	{
        EventDispatcher.AddEventListener<SelectedKeyChanged>(OnPlayerSelectedKey);
		audioGun = gameObject.GetComponent<AudioGun> ();
	}
	
	private void OnPlayerSelectedKey(SelectedKeyChanged selectedKey)
	{
		if (playersOnKey.Contains(selectedKey.playerScript) && selectedKey.keyScript != this) //wtf
		{
			playersOnKey.Remove(selectedKey.playerScript);
			if (playersOnKey.Count == 0) {
				active = false;
				pressed.SetActive(false);
				unpressed.SetActive(true);
			}
		}
		else if (!playersOnKey.Contains(selectedKey.playerScript) && selectedKey.keyScript == this) //wtf
		{
			playersOnKey.Add(selectedKey.playerScript);
            active = true;
			pressed.SetActive(true);
			unpressed.SetActive(false);
		}
	}

    public void Update()
    {
    }

	public KeyNoteData getKeyData(){
		return keyState.keyNoteData;
	}
}
