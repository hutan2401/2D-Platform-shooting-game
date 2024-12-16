using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class EHeathBar : MonoBehaviour
{
    public Text text;
    public Image healthBarFill;

    public float health;
    public float maxHealth;
    void Start()
    {
        text.text = health + "/" + maxHealth;
        healthBarFill.fillAmount = health / maxHealth;
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
        text.text = health + "/" + maxHealth;
        healthBarFill.fillAmount = health / maxHealth;
    }
   
    public void GetDamage(int number)
    {
        health -= number;
        if(health < 1)
        {

        }
        text.text = health + "/" + maxHealth;
        healthBarFill.fillAmount = health / maxHealth;
    }
}
