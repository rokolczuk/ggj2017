using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIController : MonoBehaviour {

	// Use this for initialization
	void Start () {

		EventDispatcher.AddEventListener<GameStartedEvent>(OnGameStarted);
		gameObject.SetActive(false);
	}

	void OnGameStarted(GameStartedEvent e)
	{
		gameObject.SetActive(true);
	}

	private void OnDestroy()
	{
		EventDispatcher.RemoveEventListener<GameStartedEvent>(OnGameStarted);
	}
}
