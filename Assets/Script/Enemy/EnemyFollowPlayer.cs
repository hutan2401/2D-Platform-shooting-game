using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    [SerializeField] private float speed =0.5f;
    [SerializeField] private float lineOfSite = 3f;
    [SerializeField] private float shootingRange = 5f;
    [SerializeField] private float coolDown = 1f;
    [SerializeField] private float fireRate = 1f;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform pointShooting;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        FindPlayer();
    }

    public void FindPlayer()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if(distanceFromPlayer < lineOfSite && distanceFromPlayer >shootingRange)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
        }
        else if( distanceFromPlayer <= shootingRange && coolDown < Time.time )
        {
            Vector2 direction = (player.position - pointShooting.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Instantiate(bulletPrefab, pointShooting.transform.position, Quaternion.Euler(0, 0, angle));
            coolDown = Time.time + fireRate;
        }
    }

    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.color= Color.red;
        Gizmos.DrawWireSphere (transform.position, shootingRange);
    }

}
