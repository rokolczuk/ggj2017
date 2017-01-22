using UnityEngine;
using System.Net;
using System.Net.Sockets;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkHUDMenu : MonoBehaviour
{
	public GameObject canvas;
	public Text playerCountText;
	public GameObject startGameLayout, startGameButt, serverButt, clientButt, broadButt, waitButt, waitingForHostGO;

	private NetworkDiscovery networkDiscovery;
	private NetworkManager networkManager;
	private bool pressedDown = false;
	private bool showHud = true;

	public int minPlayerCount = 2;
	
	private void Awake()
	{
		EventDispatcher.AddEventListener<GameStartedEvent>(GameStarted);
		canvas.SetActive(true);
		networkDiscovery = GetComponent<OverriddenNetworkDiscovery>();
		networkManager = GetComponent<NetworkManager>();

		stopBroadcasting();
	}

	private void Update()
	{
#if UNITY_STANDALONE || UNITY_EDITOR
		if (Input.GetButtonDown("Cancel"))
		{
			if (!pressedDown)
			{
				pressed();
			}
		}
		if (Input.GetButtonUp("Cancel"))
		{
			pressedDown = false;
		}
#endif
#if UNITY_IOS || UNITY_ANDROID
		if (Input.touchCount > 2)
		{
			if (!pressedDown)
			{
				pressed();
			}
		}
		else
		{
			pressedDown = false;
		}
#endif

		var players = FindObjectsOfType<PlayerScript>();

		if (startGameLayout.activeSelf)
		{
			updatePlayerCount(players.Length);

			if (players.Length >= minPlayerCount)
			{
				displayStartGame();
			}
		}
	}

	private void pressed()
	{
		pressedDown = true;
		showHud = !showHud;

		updateHud();
	}

	private void updateHud()
	{
		canvas.SetActive(showHud);
	}

	private void startBroadcasting()
	{
		serverButt.SetActive(false);
		clientButt.SetActive(false);
		broadButt.SetActive(true);
	}

	public void stopBroadcasting()
	{
		serverButt.SetActive(true);
		clientButt.SetActive(true);
		broadButt.SetActive(false);
		waitButt.SetActive(false);
		startGameLayout.SetActive(false);
		startGameButt.SetActive(false);
		waitingForHostGO.SetActive(false);
	}

	private void startWaiting()
	{
		broadButt.SetActive(false);
		waitButt.SetActive(true);
		displayPlayerCountOnly();
	}

	public void stopWaiting()
	{
		networkManager.StopHost();
		StopSearching();
	}

	public void ServerStart()
	{
		networkManager.networkAddress = LocalIPAddress();
		networkManager.StartHost();
		networkDiscovery.Initialize();
		networkDiscovery.StartAsServer();

		startBroadcasting();
		startWaiting();
	}

	public string LocalIPAddress()
	{
		IPHostEntry host;
		string localIP = "";
		host = Dns.GetHostEntry(Dns.GetHostName());
		foreach (IPAddress ip in host.AddressList)
		{
			if (ip.AddressFamily == AddressFamily.InterNetwork)
			{
				localIP = ip.ToString();
				break;
			}
		}
		return localIP;
	}

	public void ClientStart()
	{
		networkDiscovery.Initialize();
		networkDiscovery.StartAsClient();

		startBroadcasting();
	}

	public void StopSearching()
	{
		if (networkDiscovery.running)
		{
			try
			{
				networkDiscovery.StopBroadcast();
			}
			catch
			{
			}
		}
		
		stopBroadcasting();
	}
	
	public void OnConnect()
	{
		StopSearching();

		serverButt.SetActive(false);
		clientButt.SetActive(false);
		waitingForHostGO.SetActive(true);
	}
	
	private void GameStarted(GameStartedEvent gameS)
	{
		GameStarted();
	}

	public void GameStarted()
	{
		OnConnect();
		waitingForHostGO.SetActive(false);
	}

	public void displayStartGame()
	{
		startGameLayout.SetActive(true);
		startGameButt.SetActive(true);
	}

	public void displayPlayerCountOnly()
	{
		startGameLayout.SetActive(true);
		startGameButt.SetActive(false);
	}

	public void updatePlayerCount(int count)
	{
		 playerCountText.text = string.Format("Player Count : {0}", count);
	}
}