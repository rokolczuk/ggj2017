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
}

public class LaserAudioManager : MonoBehaviour
{
    public List<KeyNoteData> Keys;
    List<LaserState> keyStates;

    ChordConsole console;

    void Awake()
    {
        console = FindObjectOfType<ChordConsole>();
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

    private void OnLaserStopHit(LaserStopHitEvent e)
    {
        keyStates[(int)e.Note].HittingEnemy = false;
        Print(e.Note);
    }

    private void OnLaserHit(LaserHitEvent e)
    {
        keyStates[(int)e.Note].HittingEnemy = true;
        Print(e.Note);
    }

    private void OnLaserStart(LaserStartEvent e)
    {
        keyStates[(int)e.Note].Active = true;
        keyStates[(int)e.Note].IsLocal = e.IsLocal;

        Print(e.Note);
    }

    private void OnLaserStop(LaserStopEvent e)
    {
        keyStates[(int)e.Note].Active = false;
        Print(e.Note);
    }
}