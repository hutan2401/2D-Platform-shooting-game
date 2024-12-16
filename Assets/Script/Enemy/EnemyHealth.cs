using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int enemyHealths = 1;
    [SerializeField] private float timingDestroy = 1f;
    private int currentHealth;

    public UnityEvent OnEnemyDeath;
    public int CurrentHealth => currentHealth;
    public int MaxHealth => enemyHealths;
    private void Start()
    {
        currentHealth = enemyHealths;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnEnemyDeath?.Invoke();
        Destroy(gameObject, timingDestroy);
    }
}
