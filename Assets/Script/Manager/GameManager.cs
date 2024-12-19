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
    public float bossDefeatDelay = 3f;
    //private bool isGameActive = false;
    [Header("Stage Names")]
    public List<string> stageNames;
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
    #region Public methos
    public void StartGame()
    {
        //isGameActive = true;
        LoadSceneByName(stageNames[0]);
    }
    public void OpenSettings()
    {
        LoadSceneByName(settingsScene);
    }

    // Triggered when a stage is complete
    public void OnStageComplete()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Check if the current stage is the last gameplay stage
        if (IsFinalStage(currentSceneName))
        {
            LoadSceneByName(endGameScene);
        }
        else
        {
            int nextStageIndex = stageNames.IndexOf(currentSceneName) + 1;
            if (nextStageIndex < stageNames.Count)
            {
                LoadSceneByName(stageNames[nextStageIndex]);
            }
        }
    }
    // Triggered when the player defeats the boss
    public void OnBossDefeated()
    {
        Debug.Log("Boss defeated!");

        // Check if it's the final stage
        StartCoroutine(HandleBossDefeat());
    }

    // Return to the Main Menu
    public void ReturnToMainMenu()
    {
        LoadSceneByName(mainMenuScene);
    }

    public void ExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
        Application.Quit();
    }

#endregion

    #region Private Methods
    private bool IsFinalStage(string currentSceneName)
    {
        // Assuming "Stage3" is the final stage
        return currentSceneName == stageNames[stageNames.Count - 1];
    }

    // Load a scene by its name
    private void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator HandleBossDefeat()
    {
        yield return new WaitForSeconds(bossDefeatDelay);
        OnStageComplete();
    }

    #endregion

}
