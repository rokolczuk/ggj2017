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

        /*
        if (mouseButtonPressed)
        {
            Vector2 raycastOrigin = new Vector2(transform.position.x, transform.position.y);
            Vector2 raycastDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, raycastDirection - raycastOrigin, 1000f, Layers.GetLayerMask(Layers.Enemies));

            if (hit.collider != null)
            {
                //TODO replace this with motherfucking lazers
                Debug.DrawLine(transform.position, hit.point);

                enemy = hit.collider.GetComponent<Enemy>();
                enemy.AddActiveNote(currentNote.keyNote);
            }
            else if (enemy != null)
            {
                enemy.RemoveActiveNote(currentNote.keyNote);
                enemy = null;
            }
        }
        */
    }
}
