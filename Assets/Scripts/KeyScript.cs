using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;



[Serializable]
public enum KeyNote
{
	A1, B1, C1, D1, E1, F1, G1, A2, B2, C2, D2, E2, F2, G2, C3
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

    private AudioManager audioManager;
    private AudioGun audioGun;

    [SyncVar]
    uint pressingPlayerScriptId; // the unique network id of the script what pressed us (0 for none)

    private void Awake()
    {
        EventDispatcher.AddEventListener<SelectedKeyChanged>(OnPlayerSelectedKey);
        audioManager = FindObjectOfType<AudioManager>();
        audioGun = gameObject.GetComponent<AudioGun>();
	}

	private void OnPlayerSelectedKey(SelectedKeyChanged selectedKey)
	{
		bool playerWasOnThisKey = playersOnKey.Contains(selectedKey.playerScript);
		bool playerIsOnThisKey = selectedKey.keyScript == this;

		if (playerWasOnThisKey && !playerIsOnThisKey)
		{
			keyUp(selectedKey);
		}
		else if (!playerWasOnThisKey && playerIsOnThisKey)
		{
			keyDown(selectedKey);
		}
	}

	private void keyUp(SelectedKeyChanged selectedKey)
	{
		playersOnKey.Remove(selectedKey.playerScript);

		unpressed.SetActive(true);
		pressed.SetActive(false);
	}

	private void keyDown(SelectedKeyChanged selectedKey)
	{
		playersOnKey.Add(selectedKey.playerScript);
		audioManager.playPiano(keyState.keyNoteData.pianoSound,
			selectedKey.IsLocalPlayer ? SfxOrigin.LocalPlayer : SfxOrigin.RemotePlayer);

		pressed.SetActive(true);
		unpressed.SetActive(false);
	}

	public void Update()
    {
        playersOnKey.RemoveAll(p => p == null);

        var pressingPlayerScript = playersOnKey.FirstOrDefault(p => p.netId.Value == pressingPlayerScriptId);
        if (pressingPlayerScript == null || !pressingPlayerScript.IsPressed)
        {
            pressingPlayerScript = playersOnKey.FirstOrDefault(p => p.IsPressed);
        }

        if (pressingPlayerScript == null)
        {
            audioGun.deactivateGun();
        }
        else
        {
            pressingPlayerScriptId = pressingPlayerScript.netId.Value;
			
            bool isLocalPlayerActivatingGun = pressingPlayerScript.hasAuthority;
			audioGun.activateGun(getKeyData(), pressingPlayerScript);
        }
    }

    public KeyNoteData getKeyData()
    {
        return keyState.keyNoteData;
    }
}
