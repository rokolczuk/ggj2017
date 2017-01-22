using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkHUDMenuHider : NetworkBehaviour
{
	public override void OnStartClient()
	{
		if (!isServer)
		{
			FindObjectOfType<NetworkHUDMenu>().OnConnect();
		}
	}
}
