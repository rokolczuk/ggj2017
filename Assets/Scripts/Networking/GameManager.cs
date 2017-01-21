using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour
{
    public GameObject startButton;
	PrefabManager prefabManager;

    bool gameStarted = false;

	public void Awake()
	{
		prefabManager = FindObjectOfType<PrefabManager>();
		EventDispatcher.AddEventListener<ServerAddedPlayer>(OnServerAddedPlayer);
	}

	// Use this for initialization
	private void Start()
	{
	}

	// Update is called once per frame
	private void Update()
	{
	}

	private void OnServerAddedPlayer(ServerAddedPlayer eventData)
	{
		PlayerScript player = prefabManager.Instantiate<PlayerScript>();
		NetworkServer.SpawnWithClientAuthority(player.gameObject, eventData.Player);

        if (!gameStarted)
            startButton.SetActive(true);
	}

    public void StartButtonClicked()
    {
        startButton.SetActive(false);
        gameStarted = true;
        EventDispatcher.Dispatch(new GameStartedEvent());
    }
}