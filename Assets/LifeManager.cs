using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour {

    [SerializeField]
    private GameManager gm;
    [SerializeField]
    private Text Life1;
    [SerializeField]
    private Text Life2;
    [SerializeField]
    private Text Life3;
    [SerializeField]
    private Color startColour;
    [SerializeField]
    private Color deadColour;

    void Update()
    {
        switch (gm.LivesLeft)
        {
            case 3:
                Life3.color = startColour;
                Life2.color = startColour;
                Life1.color = startColour;
                break;
            case 2:
                Life3.color = deadColour;
                Life2.color = startColour;
                Life1.color = startColour;
                break;
            case 1:
                Life3.color = deadColour;
                Life2.color = deadColour;
                Life1.color = startColour;
                break;
            case 0:
                Life3.color = deadColour;
                Life2.color = deadColour;
                Life1.color = deadColour;
                break;
        }
    }
}
