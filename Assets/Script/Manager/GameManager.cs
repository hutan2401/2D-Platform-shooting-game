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

    //[Header("Victory Show UI")]
    //[SerializeField] private GameObject victoryUI;
    //[SerializeField] private Animator victoryAnim;
    //[SerializeField] private float delayTime = 10;

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
        //GameObject menuObject = GameObject.Find("VictoryUI");
        //if (menuObject != null)
        //{
        //    victoryAnim = menuObject.GetComponent<Animator>();
        //}
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
            Debug.Log("load end scene");
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
       // StartCoroutine(ShowUI());
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
    //private IEnumerator ShowUI()
    //{
    //    if (victoryUI != null)
    //    {
    //        victoryUI.SetActive(true); // Hiển thị UI
    //        Debug.Log("Victory UI activated.");

    //        //if (victoryAnim != null)
    //        //{
    //        //    victoryAnim.SetTrigger("ShowUI"); // Kích hoạt animation
    //        //    Debug.Log("Victory animation triggered.");
    //        //}

    //        // Chờ thời gian hiển thị UI
    //        yield return new WaitForSeconds(delayTime);

    //        victoryUI.SetActive(false); // Ẩn UI sau khi hoàn thành
    //        Debug.Log("Victory UI deactivated.");
    //    }
    //    else
    //    {
    //        Debug.LogError("Victory UI is not assigned!");
    //    }
    //}
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from sceneLoaded event

        // Respawn the player in the correct position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            RespawnPlayer(player);
        }
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.HealPlayer(playerHealth.MaxHealth);
        }
        CameraController.Instance.SetPlayerCameraFollow();
    }

    #endregion

}
