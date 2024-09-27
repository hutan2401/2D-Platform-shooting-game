using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int enemyHealth =1;

    private int currentHealth;

    private void Start()
    {
        currentHealth = enemyHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0 )
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
