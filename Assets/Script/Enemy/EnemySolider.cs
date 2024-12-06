using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySolider : MonoBehaviour
{
    [SerializeField] private float enemyMoveSpeed = 2f;
    [SerializeField] private float distance = 1f;
    [SerializeField] private Transform checkPoint;
    public LayerMask groundLayer;
    public bool facingLeft = true;

    [Header("Ranged Attack Settings")]
    [SerializeField] private Transform pointShooting;
    [SerializeField] private float throwRange = 5f;
    [SerializeField] private float thrownCooldown = 2.0f;
    private float cooldownTimer = 0f;

    [Header("Melee Attack Settings")]
    [SerializeField] private float meleeRange = 1.5f;
    [SerializeField] private float delayAttackAnimation = 1.5f;
    [SerializeField] private int meleeDamage = 1;
    [SerializeField] private Transform meleeAttackPoint;
    [SerializeField] private float meleeRadius = 0.5f;

    public GameObject grenadePrefab;

    public bool inRange = false;

    private Animator animator;
    private bool isDead = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
        EnemyHealth healthComponent = GetComponent<EnemyHealth>();
        if (healthComponent != null)
        {
            healthComponent.OnEnemyDeath.AddListener(DeathAnimation);
        }
    }
    private void Update()
    {
        float speed = 0f;
        Vector3 playerPosition = PlayerControls.Instance.transform.position;
        if (isDead) { return; }
        // Cooldowns
        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
       

        // Check distance to player
        float playerDistance = Vector2.Distance(transform.position, playerPosition);
        inRange = playerDistance <= throwRange;

        if (inRange)
        {
            // Face the player
            if (playerPosition.x > transform.position.x && facingLeft)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingLeft = false;
            }
            else if (playerPosition.x < transform.position.x && !facingLeft)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingLeft = true;
            }

            if (playerDistance <= meleeRange)
            {
                TriggerAttack();
            }
            else if (playerDistance > meleeRange && cooldownTimer <= 0)
            {
                // Perform ranged attack if outside melee range
                Triggerthrow();
                cooldownTimer = thrownCooldown;
            }
        }
        else
        {
            // Patrol
            PatrolMovement();
            speed = enemyMoveSpeed;
            animator.SetFloat("xSpeed",speed);
        }
    }

    public void MeleeAttack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(meleeAttackPoint.position, meleeRadius);
        foreach (Collider2D hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<PlayerHealth>()?.TakeDamage(meleeDamage,transform);
            }
        }

    }
    private void TriggerAttack()
    {
        StartCoroutine(DelayedAttack());
    }

    private IEnumerator DelayedAttack()
    {
        animator.SetTrigger("MeleeAttack");
        yield return new WaitForSeconds(delayAttackAnimation);

    }

    public void ThrowBomb()
    {
        Instantiate(grenadePrefab, pointShooting.position, Quaternion.identity);
        
    }
    private void Triggerthrow()
    {
        StartCoroutine(DelayedThrow());
    }

    private IEnumerator DelayedThrow()
    {
        animator.SetTrigger("throwBomb");
        yield return null;

    }

    private void PatrolMovement()
    {
        transform.Translate(Vector2.left * Time.deltaTime * enemyMoveSpeed);
        RaycastHit2D hitGround = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, groundLayer);

        if (!hitGround && facingLeft)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            facingLeft = false;
        }
        else if (!hitGround && !facingLeft)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            facingLeft = true;
        }
    }
    public void DeathAnimation()
    {
        isDead = true;
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
    }
    private void OnDrawGizmosSelected()
    {
        // Ground check
        if (checkPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);
        }

        // Shooting range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, throwRange);

        // Melee range
        if (meleeAttackPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(meleeAttackPoint.position, meleeRadius);
        }
    }
}
