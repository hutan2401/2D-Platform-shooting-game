using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectTile : MonoBehaviour
{
    [SerializeField] private float speed = 22f;
    [SerializeField] private float projectTileRange = 10f;
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject particleOnHitPrefabVFX;

    [SerializeField] private bool isCanExplode = false; // New boolean to check if projectile can explode
    [SerializeField] private float explosionRadius = 2f;
    //[SerializeField] private int explosionDamage = 2;

    private Vector3 startPostion;

    public void Initialize(float bulletSpeed, float bulletRange, int bulletDamage)
    {
        speed = bulletSpeed;
        projectTileRange = bulletRange;
        damage = bulletDamage;
    }
    void Start()
    {
        startPostion = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        MoveProjectTile();
        DectectFireDistance();
    }

    public void UpdateProjectTileRange(float projectTileRange)
    {
        this.projectTileRange = projectTileRange;
    }    
    private void MoveProjectTile()
    {
        transform.Translate(Vector3.right *speed* Time.deltaTime);
    }
    public void UpdateMoveSpeed(float speed)
    {
        this.speed = speed;
    }
    private void DectectFireDistance()
    {
        if(Vector3.Distance(transform.position, startPostion) > projectTileRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        Destructible destruct = other.gameObject.GetComponent<Destructible>();
        if (!other.isTrigger && enemyHealth)
        {
            if(isCanExplode)
            {
                Explode();
            }
            else
            {
                enemyHealth.TakeDamage(damage);
                if (particleOnHitPrefabVFX != null)
                {
                    Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
                }
                Destroy(gameObject);
            }           
        }
        else if (other.isTrigger && destruct)
        {
            if (particleOnHitPrefabVFX != null)
            {
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
            }
            Destroy(gameObject) ;
        }
    }

    private void Explode()
    {
        if (explosionRadius > 0)
        {
            var hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

            foreach (var hitCollider in hitColliders)
            {
                var enemy = hitCollider.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    var closestPoint = hitCollider.ClosestPoint(transform.position);
                    var distance = Vector3.Distance(closestPoint, transform.position);
                    var damagePercent = Mathf.InverseLerp(explosionRadius, 0, distance);
                    var totalDamage = (int)(damagePercent + damage);

                    Debug.Log("Damage:" + totalDamage);
                    enemy.TakeDamage(totalDamage);
                }
            }

            if (particleOnHitPrefabVFX != null)
            {
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }
    private void OnDrawGizmos()
    {
        if (isCanExplode)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
