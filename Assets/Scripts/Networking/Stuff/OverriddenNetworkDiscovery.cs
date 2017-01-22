using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class OverriddenNetworkDiscovery : NetworkDiscovery
{
	public override void OnReceivedBroadcast(string fromAddress, string data)
	{
		if (NetworkManager.singleton != null && NetworkManager.singleton.client == null)
		{
			NetworkManager.singleton.networkAddress = fromAddress;
			NetworkManager.singleton.StartClient();
		}
	}
}
