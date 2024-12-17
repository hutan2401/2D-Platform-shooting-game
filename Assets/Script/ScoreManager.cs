using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : SingleTon<ScoreManager>
{
    private TMP_Text scoreText;
    private int currentScore = 0;
    const string SCORE_AMOUNT_TEXT = "txtScorePoint";
    public void UpdateScore(int amount)
    {
        currentScore += amount;
        Debug.Log("Score:" +currentScore);
        if (scoreText == null)
        {
            scoreText = GameObject.Find(SCORE_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }

        scoreText.text = currentScore.ToString("D3");
    }

}
