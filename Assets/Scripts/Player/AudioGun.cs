using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public enum SfxOrigin
{
    LocalPlayer,
    RemotePlayer
}

public class AudioGun : NetworkBehaviour
{
    private KeyNoteData currentNote;

    private AudioManager audioManager;
    private Enemy enemy;

    SfxOrigin activator;
    private bool active;

    private void Awake()
    {
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

        audioManager.playLaser(currentNote.synthSound, isLocalPlayer ? SfxOrigin.LocalPlayer : SfxOrigin.RemotePlayer);
    }

    public void deactivateGun()
    {
        if (!active)
            return;

        active = false;
        audioManager.stopLaser(currentNote.synthSound);

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

        if (Input.GetMouseButtonDown(0))
        {
            audioManager.playLaser(currentNote.synthSound, isLocalPlayer ? SfxOrigin.LocalPlayer : SfxOrigin.RemotePlayer);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            audioManager.stopLaser(currentNote.synthSound);
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
