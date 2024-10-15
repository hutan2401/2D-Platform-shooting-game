using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : SingleTon<ScoreManager>
{
    private Text scoreText;
    private int currentScore = 0;
    const string SCORE_AMOUNT_TEXT = "ScorePoint";
    public void UpdateScore()
    {
        currentScore += 10;
        Debug.Log("Score:" +currentScore);
        if (scoreText == null)
        {
            scoreText = GameObject.Find(SCORE_AMOUNT_TEXT).GetComponent<Text>();
        }

        scoreText.text = currentScore.ToString("D3");
    }

}
