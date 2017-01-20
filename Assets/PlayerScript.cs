using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	private KeyScript keyScript;

	void Update()
	{
		float input = Input.GetAxis("Horizontal");
		if (input != 0)
		{
			move(input*Vector3.right);
		}
	}

	public void setActiveKey(KeyScript kScript)
	{
		keyScript = kScript;
	}
	
	private void move(Vector3 direction)
	{
		transform.position += direction;

		keyScript = null;
	}
}
