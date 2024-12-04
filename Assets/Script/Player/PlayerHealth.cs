using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : SingleTon<PlayerHealth>
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float damageRecoveryTime = 1f;

    private int currentHealth;
    private bool canTakeDamage = true;
    private Slider healthSlider;

    public Transform lastSafePosition;
    public bool isDead { get; private set; }
    private Vector3 respawnPosition;
    private void Start()
    {
        currentHealth = maxHealth;
        isDead = false;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }
    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage) { return; }

   
        //StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        Debug.Log("heal:" +currentHealth);
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        //CheckIfPlayerDeath();
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
        if(currentHealth <= 0)
        {
            isDead = true;
            currentHealth = 0;

            respawnPosition = transform.position;

            GetComponent<Animator>().SetTrigger("isDead");
            StartCoroutine(DeathLoadSceneRoutine());
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
        // Delay for death animation or effects
        yield return new WaitForSeconds(2f);

        // Reset the player's state and health
        isDead = false;
        currentHealth = maxHealth;
        UpdateHealthSlider();

        // Respawn the player at the saved position
        transform.position = respawnPosition;

        // Reactivate any necessary components (like movement or collision)
        canTakeDamage = true;
        Debug.Log("Player respawned at position: " + respawnPosition);
    }
}
