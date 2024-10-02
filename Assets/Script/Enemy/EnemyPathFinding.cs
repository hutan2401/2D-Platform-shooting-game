using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFinding : MonoBehaviour
{
    [SerializeField] private float enemySpeed = 2f;

    private Rigidbody2D rb;
    private Vector2 moveDir;

    public GameObject groundCheck;
    public LayerMask groundLayer;
    public float circleRadius;
    public bool isGround;
    private bool movingRight = true;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        moveDir = Vector2.right;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDir * (enemySpeed * Time.fixedDeltaTime));
        isGround = Physics2D.OverlapCircle(groundCheck.transform.position,circleRadius,groundLayer);
        if(!isGround )
        {
            Flip();
        }
     
    }

    private void Flip()
    {
        movingRight = !movingRight;
        float rotationAngle = movingRight ? 0 : 180;
        transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);

        moveDir = movingRight ? Vector2.right : Vector2.left;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
    public void MoveTo(Vector2 targetPosition)
    {
        moveDir = targetPosition;
    }
    public void StopMoving()
    {
        moveDir = Vector3.zero;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
       
        Gizmos.DrawWireSphere(groundCheck.transform.position,circleRadius);
    }
}
