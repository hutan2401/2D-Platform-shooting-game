using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int enemyHealth =1;
    [SerializeField] private float timingDestroy = 1f;
    private int currentHealth;
    private Collider2D enemyCollider;
    public UnityEvent OnEnemyDeath;
    public int CurrentHealth => currentHealth; 
    public int MaxHealth => enemyHealth;
    private Flash flash;
    private void Start()
    {
        flash = GetComponent<Flash>();
        currentHealth = enemyHealth;
        enemyCollider = GetComponent<Collider2D>();
        if (enemyCollider == null)
        {
            Debug.LogError("EnemyCollider is missing!");
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (flash != null)
        {
            StartCoroutine(flash.FlashRoutine());
        }
        if(currentHealth <= 0 )
        {
            Die();
        }
    }
   
    private void Die()
    {
        OnEnemyDeath?.Invoke();
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero; // Dừng mọi chuyển động
        }
        Destroy(gameObject, timingDestroy);
    }
}
