using UnityEngine;
using UnityEngine.Networking;

public class AudioGun : MonoBehaviour
{
    private KeyNoteData currentNote;

    private AudioManager audioManager;
    private Enemy enemy;
	private LaserGun laserGun;

    PlayerScript activator;
    private bool active;

    private void Awake()
    {
		laserGun = GetComponentInChildren<LaserGun> (true);
        audioManager = FindObjectOfType<AudioManager>();
        active = false;

		laserGun.gameObject.SetActive(false);
	}

    public void activateGun(KeyNoteData data, PlayerScript playerScript)
    {
        if (active && activator == playerScript)
            return;

        if (active && activator != playerScript)
        {
            EventDispatcher.Dispatch(new LaserStopEvent(currentNote.keyNote));
            audioManager.stopLaser(currentNote.synthSound);
        }

        this.activator = playerScript;
        this.active = true;
        this.currentNote = data;

        EventDispatcher.Dispatch(new LaserStartEvent(currentNote.keyNote, playerScript.hasAuthority));
        audioManager.playLaser(currentNote.synthSound, playerScript.hasAuthority ? SfxOrigin.LocalPlayer : SfxOrigin.RemotePlayer);
		laserGun.gameObject.SetActive(true);
        laserGun.SetTarget(playerScript.Mouse.transform);
    }

    public void deactivateGun()
    {
        if (!active)
            return;

        active = false;
        EventDispatcher.Dispatch(new LaserStopEvent(currentNote.keyNote));
        audioManager.stopLaser(currentNote.synthSound);
		laserGun.gameObject.SetActive (false);

        if (enemy != null)
        {
            enemy.RemoveActiveNote(currentNote.keyNote);
            enemy = null;
        }
    }

    void Update()
    {
        if (!active)
        {
            return;
        }

		var raycastOrigin = activator.Mouse.transform.position;
		var dir = Vector3.forward;

		Debug.DrawRay (raycastOrigin, dir);

		RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector3.forward, 1000f, Layers.GetLayerMask(Layers.Enemies));

        if (hit.collider != null)
        {
            enemy = hit.collider.GetComponent<Enemy>();
			enemy.AddActiveNote(currentNote.keyNote, laserGun);
		} 
		else if (enemy != null)
		{
			enemy.RemoveActiveNote(currentNote.keyNote);
			enemy = null;
		}

	}
}
