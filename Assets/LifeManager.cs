using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour {

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

    private int LifeCount = 3;

    void Awake()
    {
        EventDispatcher.AddEventListener<LifeLostEvent>(OnLifeLost);
    }

    void Update()
    {
        //TESTING  
        if (Input.GetKeyDown(KeyCode.T))
            EventDispatcher.Dispatch(new LifeLostEvent());
    }

	void OnLifeLost(LifeLostEvent e)
    {
        LifeCount--;
        switch (LifeCount)
        {
            case 2:
                Life3.color = deadColour;
                break;
            case 1:
                Life3.color = deadColour;
                Life2.color = deadColour;
                break;
            case 0:
                Life3.color = deadColour;
                Life2.color = deadColour;
                Life1.color = deadColour;
                EventDispatcher.Dispatch(new GameOverEvent());
                break;
        }
    }
}
