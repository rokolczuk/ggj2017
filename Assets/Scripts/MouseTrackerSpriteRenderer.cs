using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class MouseTrackerSpriteRenderer : NetworkBehaviour
{
	private SpriteRenderer spriteRenderer;

	public override void OnStartAuthority()
	{
		base.OnStartAuthority();

		Color color = Color.white;
		
		spriteRenderer.color = color;
	}

	private void OnDestroy()
	{
		Cursor.visible = true;
		EventDispatcher.RemoveEventListener<SelectedKeyChanged>(OnSelectedKeyChanged);
	}

	private void OnGameStart(GameStartedEvent e)
	{
		Cursor.visible = false;
	}

	private void OnGameOver(GameOverEvent e)
	{
		Cursor.visible = true;
	}

	private void Awake()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();

		EventDispatcher.AddEventListener<SelectedKeyChanged>(OnSelectedKeyChanged);
		EventDispatcher.AddEventListener<GameStartedEvent>(OnGameStart);
		EventDispatcher.AddEventListener<GameOverEvent>(OnGameOver);

		Color color = Color.white;
		color.a = 0.3f;

		Cursor.visible = false;

		spriteRenderer.color = color;
	}

	private void OnSelectedKeyChanged(SelectedKeyChanged e)
	{
		if (hasAuthority)
		{
			if (e.playerScript.hasAuthority)
			{
				if (e.keyScript == null)
				{
					spriteRenderer.color = Color.white;
				}
				else
				{
					try 
					{
					spriteRenderer.color = e.keyScript.getKeyData().activeColor;
					}

					catch (Exception r)
					{
						Debug.Log("oops");
					}
				}
			}
		}
	}
}
