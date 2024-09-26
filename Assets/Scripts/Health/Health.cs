using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get;  set; }
    private bool dead;
    private void Awake()
    {
        currentHealth = startingHealth;
    }
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if(currentHealth > 0)
        {

        }
        else
        {
            if(!dead)
            {
                GetComponent<PlayerMovement>().enabled = false;
                dead = true;
            }
            
        }
    }
   
}
