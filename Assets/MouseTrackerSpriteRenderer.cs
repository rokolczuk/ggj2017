using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MouseTrackerSpriteRenderer : NetworkBehaviour
{
	private SpriteRenderer spriteRenderer;

	public override void OnStartAuthority()
	{
		base.OnStartAuthority();

		Color color = Color.white;
		
		spriteRenderer.color = color;
	}

	private void Awake()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();

		Cursor.visible = false;

		Color color = Color.white;
		color.a = 0.3f;

		spriteRenderer.color = color;
	}
}
