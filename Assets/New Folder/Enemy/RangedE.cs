using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class RangedE : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 3f;

    public float speed;
    public float lineOfSite;
    public float shootingRange;
    public float fireRate = 1f;
    public float nextFireTime;
    public GameObject dan;
    public GameObject danParent;
    private Transform player;
    private Animator anim;

    //public float health;
    //public float maxHealth;
    public Image healthBarFill;
    public bool isFlipped = false;
    private void Start()
    {
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer < lineOfSite && distanceFromPlayer > shootingRange)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
            //anim.SetTrigger("Run");
        }
        else if (distanceFromPlayer <= shootingRange && nextFireTime < Time.time)
        {
            Instantiate(dan, danParent.transform.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate;
            //anim.SetTrigger("Idle");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBullet")
        {
            //GetDamage(1);
            Destroy(collision.gameObject);
        }
    }
    //public void AddHealth(int number)
    //{
    //    if (health + number < maxHealth)
    //    {
    //        health += number;
    //    }
    //    else
    //    {
    //        health = maxHealth;
    //    }

    //    healthBarFill.fillAmount = health / maxHealth;
    //}

    //public void GetDamage(int number)
    //{
    //    health -= number;
    //    if (health < 1)
    //    {
    //        Destroy(gameObject);
    //    }

    //    healthBarFill.fillAmount = health / maxHealth;
    //}

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if(health <= 0)
        {
            Destroy(gameObject);
        }
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
}
