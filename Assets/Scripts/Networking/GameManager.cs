using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour
{
    public GameObject startButton;
	PrefabManager prefabManager;

    bool gameStarted = false;
    bool gameOver = false;

	public void Awake()
	{
		prefabManager = FindObjectOfType<PrefabManager>();
		EventDispatcher.AddEventListener<ServerAddedPlayer>(OnServerAddedPlayer);
        EventDispatcher.AddEventListener<GameOverEvent>(OnGameOver);
    }

    private void OnServerAddedPlayer(ServerAddedPlayer eventData)
	{
		PlayerScript player = prefabManager.Instantiate<PlayerScript>();
		NetworkServer.SpawnWithClientAuthority(player.gameObject, eventData.Player);

        if (!gameStarted)
            startButton.SetActive(true);
	}

    private void OnGameOver(GameOverEvent e)
    {
        if (gameOver)
            return;

        gameOver = true;
        Debug.Log("GAME OVER BRAH");
    }

    public void StartButtonClicked()
    {
        if (!gameStarted)
        {
            startButton.SetActive(false);
            gameStarted = true;
            EventDispatcher.Dispatch(new GameStartedEvent());
        }
    }
}