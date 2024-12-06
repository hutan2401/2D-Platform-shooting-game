using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
   
    [SerializeField] private float speed = 22f;
    [SerializeField] private float projectTileRange = 10f;
    //[SerializeField] private bool isEnemyProjecttile = false;
    [SerializeField] private int damage =0;
    [SerializeField] private Vector3 direction = Vector3.right;
    [SerializeField] private GameObject effectPrefab;
    private Vector3 startPostion;
    void Start()
    {
        startPostion = transform.position;
    }

    private void Update()
    {
        MoveProjectTile();
        DectectFireDistance();
    }
    public void Initialize(float bulletSpeed, float bulletRange, int bulletDamage)
    {
        speed = bulletSpeed;
        projectTileRange = bulletRange;
        damage = bulletDamage;
    }
    public void UpdateProjectTileRange(float projectTileRange)
    {
        this.projectTileRange = projectTileRange;
    }
    public void UpdateMoveSpeed(float speed)
    {
        this.speed = speed;
    }
    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }
    private void MoveProjectTile()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
    private void DectectFireDistance()
    {
        if (Vector3.Distance(transform.position, startPostion) > projectTileRange)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       // Debug.Log(collision.name);
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
        
        if (!collision.isTrigger && player)
        {
            player.TakeDamage(damage, transform);
            if (effectPrefab != null)
            {
                Instantiate(effectPrefab, transform.position,Quaternion.identity);
            }

            Destroy(gameObject);
        }

    }
}
