using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class LaserStartEvent
{
    public KeyNote Note;
    public bool IsLocal;

    public LaserStartEvent(KeyNote note, bool isLocal)
    {
        Note = note;
        IsLocal = isLocal;
    }
}

public class LaserStopEvent
{
    public KeyNote Note;
    //public bool IsLocal;

    public LaserStopEvent(KeyNote note)
    {
        Note = note;
        // IsLocal = isLocal;
    }
}

public class LaserHitEvent
{
    public KeyNote Note;
    
    public LaserHitEvent(KeyNote note)
    {
        Note = note;
    }
}

public class LaserStopHitEvent
{
    public KeyNote Note;

    public LaserStopHitEvent(KeyNote note)
    {
        Note = note;
    }
}

public class LaserState
{
    public bool Active; // being played
    public bool IsLocal; // played by local player? only valid if Active == true
    public bool HittingEnemy;

    public AudioClip CurrentActiveClip;
}

public class LaserAudioManager : MonoBehaviour
{
    public List<KeyNoteData> Keys;
    List<LaserState> keyStates;

    ChordConsole console;
    AudioManager audioManager;

    void Awake()
    {
        console = FindObjectOfType<ChordConsole>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Start()
    {
        keyStates = new List<LaserState>();
        for (int i = 0; i < Enum.GetNames(typeof(KeyNote)).Length; i++)
        {
            keyStates.Add(new LaserState());
        }

        EventDispatcher.AddEventListener<LaserStartEvent>(OnLaserStart);
        EventDispatcher.AddEventListener<LaserStopEvent>(OnLaserStop);
        EventDispatcher.AddEventListener<LaserHitEvent>(OnLaserHit);
        EventDispatcher.AddEventListener<LaserStopHitEvent>(OnLaserStopHit);
    }

    void Print(KeyNote note)
    {
        var state = keyStates[(int)note];
        if (state.Active)
        {
            string s = state.IsLocal ? "local" : "remote";
            if (state.HittingEnemy)
                s += " (HIT!)";
            console.Set(note.ToString(), s);
        }
        else
        {
            console.Remove(note.ToString());
        }
    }

    void UpdateNoteSfx(KeyNote note)
    {
        Print(note);

        const float FULL_VOLUME = 1;
        const float MED_VOLUME = 0.3f;
        const float LOW_VOLUME = 0.13f;

        var state = keyStates[(int)note];
        var data = Keys.FirstOrDefault(k => k.keyNote == note);

        if (state.Active)
        {
            // hit target
            if (state.HittingEnemy)
            {
                playLaserWithVolume(FULL_VOLUME, note);
            }

            // miss target local player
            if (!state.HittingEnemy && state.IsLocal)
            {
                playLaserWithVolume(MED_VOLUME, note);
            }

            // miss target remote player
            if (!state.HittingEnemy && !state.IsLocal)
            {
                playLaserWithVolume(LOW_VOLUME, note);
            }
        }
        else
        {
            audioManager.stopLaser(data.synthSound);
            state.CurrentActiveClip = null;
        }
    }

    private void playLaserWithVolume(float FULL_VOLUME, KeyNote note)
    {
        var state = keyStates[(int)note];
        var data = Keys.FirstOrDefault(k => k.keyNote == note);

        if (state.CurrentActiveClip != null && state.CurrentActiveClip == data.synthSound)
        {
            audioManager.setVolume(state.CurrentActiveClip, FULL_VOLUME);
        }
        else
        {
            // stop previous sfx if any
            if (state.CurrentActiveClip != null)
                audioManager.stopLaser(state.CurrentActiveClip);

            audioManager.playLaser(data.synthSound, FULL_VOLUME);
            state.CurrentActiveClip = data.synthSound;
        }
    }

    private void OnLaserStopHit(LaserStopHitEvent e)
    {
        keyStates[(int)e.Note].HittingEnemy = false;
        UpdateNoteSfx(e.Note);
    }

    private void OnLaserHit(LaserHitEvent e)
    {
        keyStates[(int)e.Note].HittingEnemy = true;
        UpdateNoteSfx(e.Note);
    }

    private void OnLaserStart(LaserStartEvent e)
    {
        keyStates[(int)e.Note].Active = true;
        keyStates[(int)e.Note].IsLocal = e.IsLocal;

        UpdateNoteSfx(e.Note);
    }

    private void OnLaserStop(LaserStopEvent e)
    {
        keyStates[(int)e.Note].Active = false;
        UpdateNoteSfx(e.Note);
    }
}