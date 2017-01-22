using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour
{
	[SerializeField]
	private AudioManager audioManager;

    [SyncVar]
    public int LivesLeft = 3;

    [SyncVar]
    private bool gameOver = false;

	PrefabManager prefabManager;

	[SyncVar (hook = "OnGameStartedChanged")]
    bool gameStarted = false;

	public void Awake()
	{
		prefabManager = FindObjectOfType<PrefabManager>();
		EventDispatcher.AddEventListener<ServerAddedPlayer>(OnServerAddedPlayer);
        EventDispatcher.AddEventListener<EnemyCrashedEvent>(OnEnemyCrashed);
    }

    private void OnServerAddedPlayer(ServerAddedPlayer eventData)
	{
		PlayerScript player = prefabManager.Instantiate<PlayerScript>();
		NetworkServer.SpawnWithClientAuthority(player.gameObject, eventData.Player);

        TrackMouse mouse = prefabManager.Instantiate<TrackMouse>();
        NetworkServer.SpawnWithClientAuthority(mouse.gameObject, eventData.Player);
		
	}

    private void OnEnemyCrashed(EnemyCrashedEvent e)
    {
        if (isServer)
        {
            if (!gameOver)
            {
                LivesLeft--;
                if (LivesLeft <= 0)
                {
                    gameOver = true;
                }
            }
        }
    }

    public void StartButtonClicked()
    {
        if (!gameStarted)
        {
            gameStarted = true;
			//audioManager.StartMusic();
        }
    }

	private void OnGameStartedChanged(bool started)
	{
		gameStarted = started;
		if (gameStarted)
			EventDispatcher.Dispatch(new GameStartedEvent());
	}
}