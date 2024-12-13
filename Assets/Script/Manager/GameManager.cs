using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Scene Management")]
    public string mainMenuScene = "MainMenu";
    public string settingsScene = "Settings";
    public string endGameScene = "EndGame";

    //private bool isGameActive = false;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep GameManager across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager instances
        }
    }
    private void Start()
    {
        // Optional: Start in the main menu
        if (SceneManager.GetActiveScene().name != mainMenuScene)
        {
            ReturnToMainMenu();
        }
    }

    // Start the game from Stage1
    public void StartGame()
    {
        //isGameActive = true;
        LoadSceneByName("Stage1");
    }

    // Triggered when a stage is complete
    public void OnStageComplete()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Check if the current stage is the last gameplay stage
        if (IsFinalStage(currentSceneIndex))
        {
            LoadSceneByName(endGameScene);
        }
        else
        {
            // Load the next stage
            LoadSceneByIndex(currentSceneIndex + 1);
        }
    }

    // Triggered when the player defeats the boss
    public void OnBossDefeated()
    {
        Debug.Log("Boss defeated!");

        // Check if it's the final stage
        if (IsFinalStage(SceneManager.GetActiveScene().buildIndex))
        {
            // Show victory screen for the end game
            LoadSceneByName(endGameScene);
        }
        else
        {
            // Proceed to the next stage
            OnStageComplete();
        }
    }

    // Return to the Main Menu
    public void ReturnToMainMenu()
    {
        //isGameActive = false;
        LoadSceneByName(mainMenuScene);
    }

    // Open Settings
    public void OpenSettings()
    {
        LoadSceneByName(settingsScene);
    }

    // Check if the current stage is the final one
    private bool IsFinalStage(int currentSceneIndex)
    {
        // Assuming "Stage3" is the final stage
        return SceneManager.GetSceneByBuildIndex(currentSceneIndex).name == "Stage3";
    }

    // Load a scene by its name
    private void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Load a scene by its build index
    private void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
}
