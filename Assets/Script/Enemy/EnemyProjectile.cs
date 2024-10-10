using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
   
    [SerializeField] private float speed = 22f;
    [SerializeField] private float projectTileRange = 10f;
    //[SerializeField] private bool isEnemyProjecttile = false;
    [SerializeField] private int damage = 1;


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

    public void UpdateProjectTileRange(float projectTileRange)
    {
        this.projectTileRange = projectTileRange;
    }
    public void UpdateMoveSpeed(float speed)
    {
        this.speed = speed;
    }
    private void MoveProjectTile()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
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
        Debug.Log(collision.name);
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (!collision.isTrigger && player)
        {
            player.TakeDamage(damage, transform);
            Destroy(gameObject);
        }
       
    }
}
