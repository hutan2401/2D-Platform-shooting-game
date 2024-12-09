using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : SingleTon<PlayerHealth>
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float damageRecoveryTime = 1f;

    [Header("Lives Settings")]
    [SerializeField] private int maxLives = 3; // Maximum lives
    [SerializeField] private Text textLives;
    private int currentLives;
    [Header("Respawn Point Settings")]
    [SerializeField] private GameObject respawnPoint;

    [Header("Death Menu Settings")]
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private Animator showMenuDeathAnimator;

    private int currentHealth;
    private bool canTakeDamage = true;
    private bool isRespawning = false;
    private Slider healthSlider;

    private Animator deathAnimator;
   

   // public Transform lastSafePosition;
    public bool isDead { get; private set; }
    private Vector3 respawnPosition;
    private void Start()
    {
        currentHealth = maxHealth;
        currentLives = maxLives;
        isDead = false;
        isRespawning = false;
        UpdateUITextLive();
        deathAnimator = GetComponent<Animator>();
        GameObject menuObject = GameObject.Find("DeathMenu"); // Replace with the correct name of your GameObject
        if (menuObject != null)
        {
            showMenuDeathAnimator = menuObject.GetComponent<Animator>();
        }
    }
        // Update is called once per frame
        void Update()
    {
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }
    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage || isDead) { return; }

   
        //StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        Debug.Log("heal:" +currentHealth);
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        if (currentHealth <= 0)
        {
            CheckIfPlayerDeath();
        }
    }
    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
            UpdateHealthSlider();
        }
    }

    private void CheckIfPlayerDeath()
    {
        if (isRespawning || currentHealth > 0) return;

        currentHealth = 0;
        currentLives--;
        UpdateUITextLive();
        isDead = true;

        if (currentLives > 0)
        {
            // Player respawns
            Debug.Log($"Player died. Remaining lives: {currentLives}");
            deathAnimator.SetTrigger("isDead");
            StartCoroutine(RespawnRoutine());
        }
        else
        {
            // Game Over
            Debug.Log("Game Over! No lives remaining.");
            deathAnimator.SetTrigger("isDead");
            StartCoroutine(GameOverRoutine());
        }
    }
    
    private void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find("HealthBar").GetComponent<Slider>(); 
        }
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

    }
    private void UpdateUITextLive()
    {
        if(currentLives <=0) return;
        if (textLives != null)
        {
            textLives.text = "x" + currentLives;
        }
    }

    
    private void OnCollisionStay2D(Collision2D collision)
    {
        EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
        if (enemy)
        {
            TakeDamage(1, collision.transform);
        }
    }

    private IEnumerator DeathLoadSceneRoutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //transform.position = lastSafePosition.position;
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }
    private IEnumerator RespawnRoutine()
    {
        isRespawning = true;
        yield return new WaitForSeconds(2f);

        currentHealth = maxHealth; // Reset health on respawn
        isDead = false;
        isRespawning = false;

        transform.position = new Vector2(respawnPoint.transform.position.x,0); // Change to your respawn point
        Debug.Log("Player respawned.");
    }
    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(2f);
        if (deathMenu != null) // Check if the GameObject is assigned
        {
            deathMenu.SetActive(true); // Show the death menu
            if (showMenuDeathAnimator != null) // Optional: Animate the menu
            {
                showMenuDeathAnimator.SetTrigger("ShowMenu");
            }
        }

        Debug.Log("Returning to Main Menu...");
        //SceneManager.LoadScene("MainMenu"); // Load your main menu or game over scene
    }
   
}
