﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    private Text scoreText;
    private int currentScore = 0;
    private int actualScore = 0;

    [SerializeField]
    private int increaseAmount = 1;

    void Awake()
    {
        scoreText = GetComponent<Text>();
        EventDispatcher.AddEventListener<EnemyDiedEvent>(OnEnemyDied);
    }

    void Update()
    {
        if (currentScore < actualScore)
        {
            currentScore += increaseAmount;
            if (currentScore > actualScore)
                currentScore = actualScore;
            scoreText.text = currentScore.ToString();
        }
    }

    private void OnEnemyDied(EnemyDiedEvent e)
    {
        actualScore += 100;
    }
}
