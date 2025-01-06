using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeBomb : MonoBehaviour
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private int damageGrenade = 2;
    [SerializeField] private GameObject explodeEffect;

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
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);

            foreach (Collider2D hitCollider in hitColliders)
            {
                EnemyHealth enemy = hitCollider.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    // Calculate damage based on proximity to the explosion
                    Vector2 closestPoint = hitCollider.ClosestPoint(transform.position);
                    float distance = Vector3.Distance(closestPoint, transform.position);
                    float damagePercent = Mathf.InverseLerp(radius, 0, distance);
                    int totalDamage = (int)(damagePercent * damageGrenade);
                    enemy.TakeDamage(totalDamage);      
                }
            }
            Destroy(gameObject); 
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destructible destruct = collision.gameObject.GetComponent<Destructible>();
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if(collision.gameObject.CompareTag("Ground") || enemy || destruct)
        {
            Instantiate(explodeEffect, transform.position,Quaternion.identity);
            ManagerAudioSound.Instance.PlayHitSound("HitBombSoundSFX");
            Explode();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
