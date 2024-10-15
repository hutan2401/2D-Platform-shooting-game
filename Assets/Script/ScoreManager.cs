using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : SingleTon<ScoreManager>
{
    private int currentScore = 0;
    public void UpdateScore()
    {
        currentScore += 10;
        Debug.Log("Score:" +currentScore);
    }

}
