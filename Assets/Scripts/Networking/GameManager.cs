using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
	[SerializeField]
	private AudioManager audioManager;

    [SyncVar]
    public int LivesLeft = 3;

    [SyncVar (hook = "OnGameOverChanged")]
    private bool gameOver = false;

	PrefabManager prefabManager;

	[SyncVar (hook = "OnGameStartedChanged")]
    bool gameStarted = false;

	public GameObject gameOverText;
	public GameObject restartButt;

	public void Awake()
	{
		prefabManager = FindObjectOfType<PrefabManager>();
		EventDispatcher.AddEventListener<ServerAddedPlayer>(OnServerAddedPlayer);
        EventDispatcher.AddEventListener<EnemyCrashedEvent>(OnEnemyCrashed);

		gameOverText.SetActive(false);
		restartButt.SetActive(false);
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
        EventDispatcher.Dispatch(new CameraShakeEvent());
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

	private void OnGameOverChanged(bool over)
	{
		gameOver = over;

		if (gameOver)
		{
			EventDispatcher.Dispatch(new GameOverEvent());
			OnGameOver();
		}
	}

	private void OnGameOver()
	{
		Time.timeScale = 0;

		audioManager.SlowDownMusic();
		gameOverText.SetActive(true);

		if (isServer)
			restartButt.SetActive(true);
	}

	public void restart()
	{
		RpcRestart();
	}

	[ClientRpc]
	private void RpcRestart()
	{
		Time.timeScale = 1;

		EventDispatcher.Dispatch(new GameRestartEvent());
		LivesLeft = 3;
		gameOver = false;
		gameStarted = false;

		audioManager.SpeedUpMusic();

        gameOverText.SetActive(false);
		restartButt.SetActive(false);
	}

	public void StartButtonClicked()
    {
        if (!gameStarted)
        {
            gameStarted = true;
			
        }
    }

	private void OnGameStartedChanged(bool started)
	{
		gameStarted = started;
		if (gameStarted)
			EventDispatcher.Dispatch(new GameStartedEvent());
	}
}