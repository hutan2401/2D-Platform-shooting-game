using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    private TMP_Text scoreText;
    private int currentScore = 0;
    private Dictionary<string, int> levelScores = new Dictionary<string, int>(); 
    private int totalScore = 0; 
    const string SCORE_AMOUNT_TEXT = "txtScorePoint";
    private TMP_Text finalScore;
    const string FINAL_TOTAL_SCORE_TEXT = "FinalScoreText";
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
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
    public void SaveScoreForCurrentLevel(string levelName)
    {
        if (!levelScores.ContainsKey(levelName))
        {
            levelScores.Add(levelName, currentScore);
        }
        else
        {
            levelScores[levelName] = currentScore;
        }

        Debug.Log($"Score for {levelName}: {currentScore}");
    }

    public void ResetScoreForNextLevel()
    {
        currentScore = 0;
    }

    public int GetTotalScore()
    {
        totalScore = 0;
        foreach (var score in levelScores.Values)
        {
            totalScore += score;         
        }
        Debug.Log("Total Score Calculated: " + totalScore);
        return totalScore;
    }
    public void DisplayFinalScore()
    {
        //if (finalScore == null)
        //{
        //    finalScore = GameObject.Find(FINAL_TOTAL_SCORE_TEXT).GetComponent<TMP_Text>();
        //}

        //if (finalScore != null)
        //{
        //    int calculatedTotalScore = GetTotalScore();
        //    finalScore.text ="Score: "+ calculatedTotalScore.ToString("D3"); // Hiển thị tổng điểm trên màn EndGame
        //}
        if (finalScore == null)
        {
            finalScore = GameObject.Find(FINAL_TOTAL_SCORE_TEXT)?.GetComponent<TMP_Text>();
            if (finalScore == null)
            {
                Debug.LogError("FinalScoreText not found in EndScene!");
                return;
            }
        }

        int calculatedTotalScore = GetTotalScore();
        finalScore.text = "Score: " + calculatedTotalScore.ToString("D3");
    }
    public Dictionary<string, int> GetLevelScores()
    {
        return new Dictionary<string, int>(levelScores);
    }

    public void ResertAllScores()
    {
        PlayerPrefs.DeleteAll();
    }
    public void SaveTotalScoreToPrefs()
    {
        int calculatedTotalScore = GetTotalScore();
        PlayerPrefs.SetInt("TotalScore", calculatedTotalScore);
        PlayerPrefs.Save();
        Debug.Log("Saved Total Score: " + calculatedTotalScore);
    }
}
