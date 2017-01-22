using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour {

    [SerializeField]
    private GameManager gm;
    [SerializeField]
    private Image Life1;
    [SerializeField]
    private Image Life2;
    [SerializeField]
    private Image Life3;
    [SerializeField]
    private Sprite startSprite;
    [SerializeField]
    private Sprite deadSprite;

    void Update()
    {
        switch (gm.LivesLeft)
        {
            case 3:
                Life3.sprite = startSprite;
                Life2.sprite = startSprite;
                Life1.sprite = startSprite;
                break;
            case 2:
                Life3.sprite = deadSprite;
                Life2.sprite = startSprite;
                Life1.sprite = startSprite;
                break;
            case 1:
                Life3.sprite = deadSprite;
                Life2.sprite = deadSprite;
                Life1.sprite = startSprite;
                break;
            case 0:
                Life3.sprite = deadSprite;
                Life2.sprite = deadSprite;
                Life1.sprite = deadSprite;
                break;
        }
    }
}
