using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingAIEnemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Transform wallCheckPoint;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float circleRadius = 0.2f;
    private float moveDirection = 1;
    private bool isFacingRight = true;
    private bool checkingGround;

    [Header("Jump Attack Settings")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private Transform player;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private float detectionRange = 5f;
    private bool isGrounded;
    private bool isPlayerDetected;

    [Header("Other Settings")]
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        checkingGround = Physics2D.OverlapCircle(groundCheckPoint.position, circleRadius,groundLayer);  
        isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize,0, groundLayer);

        // Detect player within range
        DetectPlayer();

        if (isPlayerDetected && isGrounded)
        {
            JumpTowardsPlayer();
            FlipTowardPlayer();
        }
        //else
        //{
        //    Patrol();
        //}
    }

    // Detect the player within a certain range
    private void DetectPlayer()
    {
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceToPlayer < detectionRange)
        {
            isPlayerDetected = true;
        }
        else
        {
            isPlayerDetected = false;
        }
    }

    // Patrol the area by walking left and right
    private void Patrol()
    {
        bool hittingWall = Physics2D.OverlapCircle(wallCheckPoint.position, circleRadius, groundLayer);

        // Flip direction if hitting a wall or reaching the edge
        if (!checkingGround || hittingWall)
        {
            Flip();
        }

        // Patrol movement
        rb.velocity = new Vector2(moveSpeed * moveDirection, rb.velocity.y);
    }

    // Make the enemy jump toward the player
    private void JumpTowardsPlayer()
    {
        float directionToPlayer = player.position.x - transform.position.x;
        // Jump towards the player
        if (isGrounded)
        {
            rb.AddForce(new Vector2(Mathf.Sign(directionToPlayer), jumpForce), ForceMode2D.Impulse);
        }
    }

    // Flip the enemy to change its direction
    private void Flip()
    {
        moveDirection *= -1;
        isFacingRight = !isFacingRight;

        transform.Rotate(0f, 180f, 0f);
    }
    private void FlipTowardPlayer()
    {
        //float playerPosition = player.position.x - transform.position.x;

        // Only flip when the enemy is facing the wrong direction and is grounded
        if (this.transform.position.x > player.position.x && isFacingRight )
        {
            Flip();
        }
        else if (this.transform.position.x < player.position.x && !isFacingRight)
        {
            Flip();
        }
    }

    // Visualize the detection range and patrol area for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Detection range visualization
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheckPoint.position, circleRadius); // Ground check visualization
        Gizmos.DrawWireSphere(wallCheckPoint.position, circleRadius); // Wall check visualization
    }
}