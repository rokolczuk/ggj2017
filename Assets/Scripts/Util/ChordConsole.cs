using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public class ChordConsole : MonoBehaviour
    {
        Dictionary<string, string> lines = new Dictionary<string, string>();

        public void Add(string key, string value)
        {
            lines.Add(key, value);
        }

        public void Remove(string key)
        {
            lines.Remove(key);
        }

        public void OnGUI()
        {
            int y = 0;
            const int LINEHEIGHT = 20;
            foreach (var item in lines)
            {
                GUI.Label(new Rect(new Vector2(0, y), new Vector2(Screen.width, LINEHEIGHT)), item.Key + ": " + item.Value);
                y += LINEHEIGHT;
            }
        }
    }
}
