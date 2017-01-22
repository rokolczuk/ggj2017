using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour {

	public List<KeyScript> KeyList;

	[SerializeField]
	private List<KeyNoteData> KeyNotesConfiguration;

	[SerializeField]
	private GameObject blackKeys;

	public KeyNoteData GetKeyData(KeyNote note)
	{
		for(int i = 0; i < KeyNotesConfiguration.Count; i++)
		{
			if(KeyNotesConfiguration[i].keyNote == note)
			{
				return KeyNotesConfiguration[i];
			}
		}

		return null;
	}

    private static KeyManager instance;
    public static KeyManager Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
		blackKeys.SetActive(false);
		EventDispatcher.AddEventListener<InitializeGameEvent>(OnGameStartedEvent);
    }

	void OnGameStartedEvent(InitializeGameEvent e)
	{
		blackKeys.SetActive(true);
	}

	private void OnDestroy()
	{
		EventDispatcher.RemoveEventListener<InitializeGameEvent>(OnGameStartedEvent);
	}
}
