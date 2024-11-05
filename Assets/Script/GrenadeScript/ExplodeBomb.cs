using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeBomb : MonoBehaviour
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private int damageGrenade = 2;
    [SerializeField] private GameObject explodeEffect;

    private AudioHitSound hitSound;

    private void Awake()
    {
        hitSound = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioHitSound>();
    }

    private void Start()
    {
        Collider2D playerCollider = GameObject.FindWithTag("Player").GetComponent<Collider2D>();
        Collider2D bombCollider = GetComponent<Collider2D>();

        // Ignore collision between the bomb and the player
        Physics2D.IgnoreCollision(playerCollider, bombCollider);
    }
    public void Explode()
    {
        if (radius > 0)
        {
            // Detect enemies within the explosion radius
            var hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);

            foreach (var hitCollider in hitColliders)
            {
                var enemy = hitCollider.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    // Calculate damage based on proximity to the explosion
                    var closestPoint = hitCollider.ClosestPoint(transform.position);
                    var distance = Vector3.Distance(closestPoint, transform.position);
                    var damagePercent = Mathf.InverseLerp(radius, 0, distance);
                    var totalDamage = (int)(damagePercent * damageGrenade);

                    // Apply damage to the enemy
                    Debug.Log(totalDamage);
                    enemy.TakeDamage(totalDamage);      
                }
            }
            Destroy(gameObject); 
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destructible destruct = collision.gameObject.GetComponent<Destructible>();
        if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy") || destruct)
        {
            Instantiate(explodeEffect, transform.position,Quaternion.identity);
            hitSound.PlaySFX(hitSound.hitBombSoundSFX);
            Explode();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
