using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour
{
	PrefabManager prefabManager;

	public void Awake()
	{
		prefabManager = FindObjectOfType<PrefabManager>();

		EventDispatcher.AddEventListener<ServerAddedPlayer>(OnServerAddedPlayer);
	}

	// Use this for initialization
	private void Start()
	{
	}

	// Update is called once per frame
	private void Update()
	{
	}

	private void OnServerAddedPlayer(ServerAddedPlayer eventData)
	{
		//DUNT
		//PlayerScript player = prefabManager.Instantiate<PlayerScript>();
		//NetworkServer.SpawnWithClientAuthority(player.gameObject, eventData.Player);
	}
}