using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyTank : MonoBehaviour
{
    [SerializeField] private Transform findPlayer;
    [SerializeField] private float enemyMoveSpeed = 2f;
    [SerializeField] private float distance = 1f;
    [SerializeField] private Transform checkPoint;
    public LayerMask groundLayer;
    public bool facingLeft = true;

    [Header("Ranged Curves Attack Settings")]
    [SerializeField] private Transform pointThrow;
    [SerializeField] private float throwRange = 5f;
    [SerializeField] private float thrownCooldown = 2.0f;
    [SerializeField] private GameObject grenadePrefab;
    //private float cooldownTimerThrow = 0f;

    [Header("Shooting Attack Settings")]
    [SerializeField] private Transform pointShooting;
    [SerializeField] private float shootingRange = 5f;
    [SerializeField] private float fireCooldown = 1.0f;
    [SerializeField] private GameObject bulletPrefab;
    //private float cooldownTimeShooting = 0f;

    [Header("Special Attack Settings")]
    [SerializeField] private float specialAttackThreshold = 0.5f; // 40% health
    [SerializeField] private float specialAttackCooldown = 3f;
    [SerializeField] private GameObject specialAttackPrefab;
    [SerializeField] private Transform specialAttackPoint;

    [Header("Attack Settings")]
    [SerializeField] private float randomAttackCooldown = 2f;
    private float randomAttackTimer = 0f;
    private float thrownCooldownTimer = 0f;
    private float shootingCooldownTimer = 0f;
    private float specialAttackTimer = 0f;
    private bool isSpecialAttackActive = false;

    private bool isDead = false;
    private EnemyHealth enemyHealth;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.OnEnemyDeath.AddListener(BossDeath);
        }
    }

    private void Update()
    {
        if (isDead) return;

        // Update cooldown timers
        UpdateCooldowns();

        if (!isSpecialAttackActive && enemyHealth != null)
        {
            float currentHealth = enemyHealth.CurrentHealth;
            float maxHealth = enemyHealth.MaxHealth;

            Debug.Log($"Boss Health: {currentHealth}/{maxHealth}");

            if (currentHealth / maxHealth <= specialAttackThreshold)
            {
                isSpecialAttackActive = true;
                Debug.Log("Special Attack Activated!");
            }
        }
        ManagerAttack();
    }

    private void UpdateCooldowns()
    {
        if (randomAttackTimer > 0) randomAttackTimer -= Time.deltaTime;
        if (thrownCooldownTimer > 0) thrownCooldownTimer -= Time.deltaTime;
        if (shootingCooldownTimer > 0) shootingCooldownTimer -= Time.deltaTime;
        if (specialAttackTimer > 0) specialAttackTimer -= Time.deltaTime;
    }

    private void ManagerAttack()
    {
        if (isSpecialAttackActive)
        {
            // Perform special attack with cooldown
            if (specialAttackTimer <= 0)
            {
                PerformSpecialAttack();
                specialAttackTimer = specialAttackCooldown;
            }
            return;
        }

        // Normal random attack management
        if (randomAttackTimer <= 0)
        {
            PerformRandomAttack();
            randomAttackTimer = randomAttackCooldown;
        }
    }

    private void PerformRandomAttack()
    {
        // Check if the player is in range
        float playerDistance = Vector2.Distance(transform.position, findPlayer.position);
        bool inThrowRange = playerDistance <= throwRange;
        bool inShootingRange = playerDistance <= shootingRange;

        if (inThrowRange || inShootingRange)
        {
            // Randomly choose an attack
            int attackType = Random.Range(0, 2); // 0 for ThrowBomb, 1 for FireBullet

            if (attackType == 0 && inThrowRange && thrownCooldownTimer <= 0)
            {
                ThrowBomb();
                thrownCooldownTimer = thrownCooldown;
            }
            else if (attackType == 1 && inShootingRange && shootingCooldownTimer <= 0)
            {
                FireBullet();
                shootingCooldownTimer = fireCooldown;
            }
        }
        else
        {
            PatrolMovement();
        }
    }

    private void PerformSpecialAttack()
    {
        Debug.Log("Boss performs a special attack!");
        Instantiate(specialAttackPrefab, specialAttackPoint.position, Quaternion.identity);
    }

    private void ThrowBomb()
    {
       // Debug.Log("Boss throws a bomb!");
        Instantiate(grenadePrefab, pointThrow.position, Quaternion.identity);
    }

    private void FireBullet()
    {
        //Debug.Log("Boss fires a bullet!");
        GameObject newBullet = Instantiate(bulletPrefab, pointShooting.position, Quaternion.identity);
        newBullet.GetComponent<EnemyProjectile>()?.UpdateMoveSpeed(5f);
    }


    private void PatrolMovement()
    {
        float speed = 0f;
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
        speed = enemyMoveSpeed;
        animator.SetFloat("Speed",speed);
    }
    private void BossDeath()
    {
        isDead = true;
        Debug.Log("Boss is dead!");
        animator.SetTrigger("Die");
    }
    private void OnDrawGizmosSelected()
    {
        // Ground check
        if (checkPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);
        }

        // Ranged attack ranges
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, throwRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
