using UnityEngine;
using UnityEngine.Networking;

public class AudioGun : MonoBehaviour
{
    private KeyNoteData currentNote;

    private AudioManager audioManager;
    private Enemy enemy;
	private LaserGun laserGun;

    SfxOrigin activator;
    private bool active;

    private void Awake()
    {
		laserGun = GetComponentInChildren<LaserGun> (true);
        audioManager = FindObjectOfType<AudioManager>();
        active = false;

		laserGun.gameObject.SetActive(false);
	}

    public void activateGun(KeyNoteData data, SfxOrigin origin)
    {
        if (active && activator == origin)
            return;

        if (active && activator != origin)
            audioManager.stopLaser(currentNote.synthSound);

        this.activator = origin;
        this.active = true;
        this.currentNote = data;

        audioManager.playLaser(currentNote.synthSound, origin);
		laserGun.gameObject.SetActive (true);
    }

    public void deactivateGun()
    {
        if (!active)
            return;

        active = false;
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

		var raycastOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		var dir = Vector3.forward;

		Debug.DrawRay (raycastOrigin, dir);

		RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector3.forward, 1000f, Layers.GetLayerMask(Layers.Enemies));

        if (hit.collider != null)
        {
            enemy = hit.collider.GetComponent<Enemy>();
			enemy.AddActiveNote(currentNote.keyNote, laserGun);

			laserGun.SetTarget (enemy.transform);
		} 
		else if (enemy != null)
		{
			enemy.RemoveActiveNote(currentNote.keyNote);
			enemy = null;
		}

		if(hit.collider == null)
        {
			laserGun.SetTarget (null);
        }
	}
}
