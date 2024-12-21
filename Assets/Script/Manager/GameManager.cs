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
    [Header("Player Respawn")]
    public Vector3 respawnPosition = Vector3.zero; // Default respawn position
    public string respawnScene; // Scene where the player should respawn
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
        SetRespawn(SceneManager.GetActiveScene().name, respawnPosition);
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

        if (IsFinalStage(currentSceneName))
        {
            LoadSceneByName(endGameScene);
        }
        else
        {
            int nextStageIndex = stageNames.IndexOf(currentSceneName) + 1;
            if (nextStageIndex < stageNames.Count)
            {
                SetRespawn(stageNames[nextStageIndex], respawnPosition); // Update respawn for the next stage
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
    public void SetRespawn(string sceneName, Vector3 position)
    {
        respawnScene = sceneName;
        respawnPosition = position;
    }
    public void RespawnPlayer(GameObject player)
    {
        if (SceneManager.GetActiveScene().name == respawnScene && player != null)
        {
            player.transform.position = respawnPosition;
        }
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
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator HandleBossDefeat()
    {
        yield return new WaitForSeconds(bossDefeatDelay);
        OnStageComplete();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from sceneLoaded event

        // Respawn the player in the correct position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            RespawnPlayer(player);
        }
        CameraController.Instance.SetPlayerCameraFollow();
    }

    #endregion

}
