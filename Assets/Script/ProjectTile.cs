using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectTile : MonoBehaviour
{
    [SerializeField] private float speed = 22f;
    [SerializeField] private float projectTileRange = 10f;
    [SerializeField] private int damage = 1;

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

        if (!other.isTrigger && enemyHealth )
        {
            enemyHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
