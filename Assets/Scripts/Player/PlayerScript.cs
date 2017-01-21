using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SelectedKeyChanged
{
	public PlayerScript playerScript;
	public KeyScript keyScript;
    public bool IsLocalPlayer;

	public SelectedKeyChanged(PlayerScript playerScript, KeyScript keyScript, bool localPlayer)
	{
		this.playerScript = playerScript;
		this.keyScript = keyScript;
        this.IsLocalPlayer = localPlayer;
	}
}

public class PlayerScript : NetworkBehaviour
{
    [SerializeField]
    private float MoveTime = 1;
	private List<KeyScript> KeyArray;
    [SerializeField]
    private Vector3 PlayerKeyPositionOffset;

    private bool isMoving = false;
    private Vector3 startPos;
    private Vector3 targetPos;

    private int _currentKeyIndex = 0;

	public GameObject keyRayCast;
    private KeyScript keyScript;

    [SyncVar]
    public bool IsPressed;

    void Awake()
	{
    }

    void Start()
    {
        KeyArray = KeyManager.Instance.KeyList;
        transform.position = KeyArray[_currentKeyIndex].transform.position + PlayerKeyPositionOffset;
    }

	void Update()
	{
		checkForKey();
        MovementChecks();

        if (hasAuthority)
            IsPressed = Input.GetMouseButton(0);

        if (IsPressed)
        { 
			//Shoot laser!
		} // some animation?  
	}
	
    private void MovementChecks()
    {
		if (!hasAuthority)
			return;

		if (isMoving)
            return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal != 0)
        {
            _currentKeyIndex += (int)horizontal;
            _currentKeyIndex = Mathf.Clamp(_currentKeyIndex, 0, KeyArray.Count - 1);
            StartCoroutine(MoveToNewPosition(KeyArray[_currentKeyIndex].transform.position + PlayerKeyPositionOffset, MoveTime));
        }
    }

    private IEnumerator MoveToNewPosition(Vector3 newPosition, float time)
    {
        isMoving = true;
        float elapsedTime = 0;
        var startingPosition = transform.position;
        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startingPosition, newPosition, Mathf.SmoothStep(0, 1, (elapsedTime/ time)));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        isMoving = false;
    }

	private void checkForKey()
	{
		RaycastHit2D[] raycastHit = Physics2D.RaycastAll(transform.position, keyRayCast.transform.localPosition);

		bool newKey = false;

		for (int i = 0; i < raycastHit.Length; i++)
		{
			if (raycastHit[i].collider != null)
			{
				if (raycastHit[i].collider.CompareTag("Key"))
				{
					newKey = true;
					KeyScript newKeyScript = raycastHit[i].collider.GetComponent<KeyScript>();
					if (keyScript != newKeyScript)
					{
                        EventDispatcher.Dispatch(new SelectedKeyChanged(this, newKeyScript, hasAuthority));
						keyScript = newKeyScript;
					}
				}
			}
		}

		if(!newKey)
		{
            EventDispatcher.Dispatch(new SelectedKeyChanged(this, null, hasAuthority));
		}
	}
}
