using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSpriteScript : NetworkBehaviour
{
	public Sprite[] fingerSprites;

	[SyncVar(hook = "OnSpriteIDChanged")]
	private int spriteID;

	private int localSpriteID = 0;

	private bool gameStarted = false;

	private SpriteRenderer spriteRenderer;

	public override void OnStartClient()
	{
		setSprite();
	}

	private void Awake()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();

		EventDispatcher.AddEventListener<GameStartedEvent>(gameStart);
		EventDispatcher.AddEventListener<GameRestartEvent>(gameRetart);
	}

	// Update is called once per frame
	void Update ()
	{
		if (!gameStarted && hasAuthority)
		{
			if (Input.GetButtonDown("Jump"))
			{
				if (++localSpriteID >= fingerSprites.Length)
				{
					localSpriteID = 0;
				}

				CmdSetSpriteID(localSpriteID);
			}
		}
	}

	private void gameStart(GameStartedEvent e)
	{
		gameStarted = true;
	}

	private void gameRetart(GameRestartEvent e)
	{
		gameStarted = false;
	}

	[Command]
	private void CmdSetSpriteID(int id)
	{
		spriteID = id;
	}

	private void OnSpriteIDChanged(int id)
	{
		spriteID = id;
		setSprite();
	}

	private void setSprite()
	{
		spriteRenderer.sprite = fingerSprites[spriteID];
	}
}
