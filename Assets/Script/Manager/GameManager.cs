using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField]private BulletType bullet;
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
        ScoreManager.Instance.SaveScoreForCurrentLevel(currentSceneName);
        ScoreManager.Instance.ResetScoreForNextLevel();
        if (IsFinalStage(currentSceneName))
        {
            Debug.Log("load end scene");
            int totalScore = ScoreManager.Instance.GetTotalScore();
            PlayerPrefs.SetInt("TotalScore", totalScore);
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
        SceneManager.LoadScene(mainMenuScene);
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
        return currentSceneName == stageNames[stageNames.Count - 1];
    }

    // Load a scene by its name
    private void LoadSceneByName(string sceneName)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
        if (scene.name == endGameScene)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Destroy(player); // Xóa Player trong EndScene
            }
            return;
        }
        else
        {
            // Respawn the player in the correct position
            GameObject respawnPlayer = GameObject.FindGameObjectWithTag("Player");
            if (respawnPlayer != null)
            {
                RespawnPlayer(respawnPlayer);
     
                PlayerHealth playerHealth = respawnPlayer.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.HealPlayer(playerHealth.MaxHealth);
                }
                Pistol pistol = respawnPlayer.GetComponent<Pistol>();
                if (pistol != null)
                {
                   pistol.ResetWeapon();
                }

                Throwbomb throwBombScript = respawnPlayer.GetComponent<Throwbomb>();
                if (throwBombScript != null)
                {
                    TMP_Text newGrenadeText = GameObject.Find("grenadeAmmoText")?.GetComponent<TMP_Text>(); // Replace "GrenadeText" with your UI element's name
                    throwBombScript.AssignGrenadeText(newGrenadeText);
                }
                CameraController.Instance.SetPlayerCameraFollow();
            }
        }
    }

    #endregion

}
