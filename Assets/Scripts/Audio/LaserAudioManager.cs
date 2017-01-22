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

    public class LaserState
    {
        public bool Active; // being played
        public bool IsLocal; // played by local player? only valid if Active == true
        public bool HittingEnemy;
    }

    public class LaserAudioManager: MonoBehaviour
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
        }

        private void OnLaserStart(LaserStartEvent e)
        {
            keyStates[(int)e.Note].Active = true;
            keyStates[(int)e.Note].IsLocal = e.IsLocal;

            console.Add(e.Note.ToString(), e.IsLocal ? "local" : "remote");
        }

        private void OnLaserStop(LaserStopEvent e)
        {
            keyStates[(int)e.Note].Active = false;
            console.Remove(e.Note.ToString());
        }
    }