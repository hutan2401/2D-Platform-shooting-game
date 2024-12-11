using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Boss : MonoBehaviour
{
    //public int health;
    //public int damage;
    private float timeBtwDamage = 1.5f;
    public Transform player;

    public float health;
    public float maxHealth;
    public Image healthBarFill;

    public bool isFlipped = false;


    void Start()
    {
        maxHealth = 20;
        healthBarFill.fillAmount = health / maxHealth;
    }




    public void Update()
    {
        if (timeBtwDamage > 0)
        {
            timeBtwDamage -= Time.deltaTime;
        }
        //healthBar.value = health;
    }
    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

    public void AddHealth(int number)
    {
        if (health + number < maxHealth)
        {
            health += number;
        }
        else
        {
            health = maxHealth;
        }
        
        healthBarFill.fillAmount = health / maxHealth;
    }

    public void GetDamage(int number)
    {
        health -= number;
        if (health < 1)
        {
            Destroy(gameObject) ;
        }
        
        healthBarFill.fillAmount = health / maxHealth;
    }
}
