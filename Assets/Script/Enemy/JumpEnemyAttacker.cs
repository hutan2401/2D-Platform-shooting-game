using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingAIEnemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float circleRadius = 0.2f;
    [SerializeField] private Transform wallCheckPoint;
    private float moveDirection = 1;
    private bool isFacingRight = true;
    private bool isGrounded;

    [Header("Jump Attack Settings")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 5f;
    private bool isPlayerDetected;

    [Header("Other Settings")]
    private Rigidbody2D rb;
    private float minFlipDistance = 0.5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Check if the enemy is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, circleRadius, groundLayer);

        // Detect player within range
        DetectPlayer();

        if (isPlayerDetected && isGrounded)
        {
            JumpTowardsPlayer();
        }
        else
        {
            Patrol();
        }
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
        if (!isGrounded || hittingWall)
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

        //// Flip towards player before jumping
        //if (Mathf.Abs(directionToPlayer) > minFlipDistance)
        //{
        //    FlipTowardPlayer();
        //}

        // Jump towards the player
        rb.AddForce(new Vector2(Mathf.Sign(directionToPlayer) , jumpForce), ForceMode2D.Impulse);
    }

    // Flip the enemy to change its direction
    private void Flip()
    {
        moveDirection *= -1;
        isFacingRight = !isFacingRight;

        // Rotate the sprite by 180 degrees on the y-axis to face the opposite direction
        transform.Rotate(0f, 180f, 0f);
    }
    private void FlipTowardPlayer()
    {
        float playerPosition = player.position.x - transform.position.x;

        // Only flip when the enemy is facing the wrong direction and is grounded
        if (playerPosition > 0 && isFacingRight )
        {
            Flip();
        }
        else if (playerPosition < 0 && !isFacingRight)
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
