using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    private Animator animator;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        moveDir = Vector2.right;
    }

    private void FixedUpdate()
    {
        
        rb.velocity = new Vector2(enemySpeed *moveDir.x , rb.velocity.y);
        isGround = Physics2D.OverlapCircle(groundCheck.transform.position,circleRadius,groundLayer);
        if(!isGround )
        {
            Flip();
        }
    }

    private void Flip()
    {
        
        moveDir *= -1;
        movingRight = !movingRight;

        transform.Rotate(0f, 180f, 0f);
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

        //rb.MovePosition(rb.position + moveDir * (enemySpeed * Time.fixedDeltaTime));

        //movingRight = !movingRight;
        //float rotationAngle = movingRight ? 0 : 180;
        //transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);

        //moveDir = movingRight ? Vector2.right : Vector2.left;
        //spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
