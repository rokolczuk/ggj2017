using UnityEngine;
using UnityEngine.Networking;

public class ServerAddedPlayer : IGameEvent
{
	public ServerAddedPlayer(GameObject player)
	{
		Player = player;
	}

	public GameObject Player { get; private set; }
}

public class SpaceKingsNetworkManager : NetworkManager
{
	private EventDispatcher eventDispatcher;

	public void Awake()
	{
		eventDispatcher = FindObjectOfType<EventDispatcher>();
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

		eventDispatcher.Dispatch(new ServerAddedPlayer(player));
	}
}