using UnityEngine;
using UnityEngine.Networking;

public class ServerAddedPlayer
{
	public ServerAddedPlayer(GameObject player)
	{
		Player = player;
	}

	public GameObject Player { get; private set; }
}

public class ChordOfDutyNetworkManager : NetworkManager
{
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

		EventDispatcher.Dispatch(new ServerAddedPlayer(player));

		print("?");
	}

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        EventDispatcher.Dispatch(new ServerAddedPlayer(player));

		print("!");
    }
}