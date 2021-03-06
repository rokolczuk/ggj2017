﻿using UnityEngine;
using UnityEngine.Networking;

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
		//Ship ship = prefabManager.Instantiate<Ship>();
		//NetworkServer.SpawnWithClientAuthority(ship.gameObject, eventData.Player);
	}
}