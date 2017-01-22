using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour
{
	[SerializeField]
	private AudioManager audioManager;
	
	PrefabManager prefabManager;

    bool gameStarted = false;
    bool gameOver = false;

	public void Awake()
	{
		prefabManager = FindObjectOfType<PrefabManager>();
		EventDispatcher.AddEventListener<ServerAddedPlayer>(OnServerAddedPlayer);
        EventDispatcher.AddEventListener<EnemyCrashedEvent>(OnEnemyCrashed);
        EventDispatcher.AddEventListener<GameOverEvent>(OnGameOver);
    }

    private void OnServerAddedPlayer(ServerAddedPlayer eventData)
	{
		PlayerScript player = prefabManager.Instantiate<PlayerScript>();
		NetworkServer.SpawnWithClientAuthority(player.gameObject, eventData.Player);

        TrackMouse mouse = prefabManager.Instantiate<TrackMouse>();
        NetworkServer.SpawnWithClientAuthority(mouse.gameObject, eventData.Player);
		
	}

    private void OnGameOver(GameOverEvent e)
    {
        if (gameOver)
            return;

        gameOver = true;
        Debug.Log("GAME OVER BRAH");
    }

    private void OnEnemyCrashed(EnemyCrashedEvent e)
    {
        if (isServer)
        {
            EventDispatcher.Dispatch(new LifeLostEvent());
        }
    }

    public void StartButtonClicked()
    {
        if (!gameStarted)
        {
            gameStarted = true;
			//audioManager.StartMusic();
            EventDispatcher.Dispatch(new GameStartedEvent());
        }
    }
}