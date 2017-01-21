using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private AudioManager audioManager;
    private AudioGun audioGun;

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
            playersOnKey.Remove(selectedKey.playerScript);
        }
        else if (!playerWasOnThisKey && playerIsOnThisKey)
        {
            playersOnKey.Add(selectedKey.playerScript);
            audioManager.playPiano(keyState.keyNoteData.pianoSound, selectedKey.IsLocalPlayer ? SfxOrigin.LocalPlayer : SfxOrigin.RemotePlayer);
        }
    }

    public void Update()
    {
        if (playersOnKey.Any(p => p.IsPressed))
        {
            pressed.SetActive(true);
            unpressed.SetActive(false);

            bool isLocalPlayerActivatingGun = playersOnKey.Any(p => p.hasAuthority);
            audioGun.activateGun(getKeyData(), isLocalPlayerActivatingGun ? SfxOrigin.LocalPlayer : SfxOrigin.RemotePlayer);
        }
        else
        {
            unpressed.SetActive(true);
            pressed.SetActive(false);
            audioGun.deactivateGun();
        }
    }

    public KeyNoteData getKeyData()
    {
        return keyState.keyNoteData;
    }
}
