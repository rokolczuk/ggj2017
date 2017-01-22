using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkHUDMenuHider : NetworkBehaviour
{
    void Awake()
    {
        EventDispatcher.AddEventListener<GameRestartEvent>(OnGameRestart);
    }

	public override void OnStartClient()
	{
		if (!isServer)
		{
			FindObjectOfType<NetworkHUDMenu>().OnConnect();
		}
	}

    void OnGameRestart(GameRestartEvent e)
    {
        if (isServer)
            FindObjectOfType<NetworkHUDMenu>().startWaiting();
    }
}
