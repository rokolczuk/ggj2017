using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public enum KeyNote
{
	A1, B1, C1, D1, E1, F1, G1, A2, B2, C2, D2, E2, F2, G2, A3, B3, C3
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

	private AudioManager audioManager;
	private AudioGun audioGun;

	private void Awake()
	{
        EventDispatcher.AddEventListener<SelectedKeyChanged>(OnPlayerSelectedKey);
		audioManager = FindObjectOfType<AudioManager> ();
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
				audioGun.deactivateGun (false);
				unpressed.SetActive(true);
			}
		}
		else if (!playersOnKey.Contains(selectedKey.playerScript) && selectedKey.keyScript == this) //wtf
		{
			playersOnKey.Add(selectedKey.playerScript);
            active = true;
			pressed.SetActive(true);
			audioGun.activateGun (true, keyState.keyNoteData);
			unpressed.SetActive(false);
			audioManager.playPiano (keyState.keyNoteData.pianoSound);
		}
	}

    public void Update()
    {
    }

	public KeyNoteData getKeyData(){
		return keyState.keyNoteData;
	}
}
