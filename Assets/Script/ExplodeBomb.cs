using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeBomb : MonoBehaviour
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private int damageGrenade = 2;
    [SerializeField] private GameObject explode;
    

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
        if(collision.gameObject.CompareTag("Ground"))
        {
            Instantiate(explode,transform.position,Quaternion.identity);
            Explode();
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
