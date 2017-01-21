using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour {

	public List<KeyScript> KeyList;

	[SerializeField]
	private List<KeyNoteData> KeyNotesConfiguration;

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
    }
}
