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

    [Header("Ranged Throw Attack Settings")]
    [SerializeField] private Transform pointThrow;
    [SerializeField] private float throwRange = 5f;
    [SerializeField] private float thrownCooldown = 2.0f;
    [SerializeField] private GameObject grenadePrefab;
    private float cooldownTimerThrow = 0f;

    [Header("Shooting Attack Settings")]
    [SerializeField] private Transform pointShooting;
    [SerializeField] private float shootingRange = 5f;
    [SerializeField] private float fireCooldown = 1.0f;
    [SerializeField] private GameObject bulletPrefab;
    private float cooldownTimeShooting = 0f;

    [Header("Attack Settings")]
    [SerializeField] private float randomAttackCooldown = 2f;
    private float randomAttackTimer = 0f; // Timer to manage random attack intervals

    private bool isDead = false;

    private void Update()
    {
        if (isDead) return;

        // Update cooldown timers
        if (cooldownTimeShooting > 0) cooldownTimeShooting -= Time.deltaTime;
        if (cooldownTimerThrow > 0) cooldownTimerThrow -= Time.deltaTime;
        if (randomAttackTimer > 0) randomAttackTimer -= Time.deltaTime;

        // Check if player is in range
        float playerDistance = Vector2.Distance(transform.position, findPlayer.position);
        bool inThrowRange = playerDistance <= throwRange;
        bool inShootingRange = playerDistance <= shootingRange;

        if (inThrowRange || inShootingRange)
        {
            // Face the player
            FacePlayer();

            if (randomAttackTimer <= 0)
            {
                PerformRandomAttack(inThrowRange, inShootingRange);
                randomAttackTimer = randomAttackCooldown;
            }
        }
        else
        {
            // Patrol if not in range
            PatrolMovement();
        }
    }

    private void FacePlayer()
    {
        if (findPlayer.position.x > transform.position.x && facingLeft)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            facingLeft = false;
        }
        else if (findPlayer.position.x < transform.position.x && !facingLeft)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            facingLeft = true;
        }
    }

    private void PerformRandomAttack(bool inThrowRange, bool inShootingRange)
    {
        // Randomly choose an attack
        int attackType = Random.Range(0, 2); // 0 for ThrowBomb, 1 for FireBullet

        if (attackType == 0 && inThrowRange && cooldownTimerThrow <= 0)
        {
            ThrowBomb();
            cooldownTimerThrow = thrownCooldown;
        }
        else if (attackType == 1 && inShootingRange && cooldownTimeShooting <= 0)
        {
            FireBullet();
            cooldownTimeShooting = fireCooldown;
        }
    }

    private void ThrowBomb()
    {
        Debug.Log("Boss throws a bomb!");
        Instantiate(grenadePrefab, pointThrow.position, Quaternion.identity);
    }

    private void FireBullet()
    {
        Debug.Log("Boss fires a bullet!");
        GameObject newBullet = Instantiate(bulletPrefab, pointShooting.position, Quaternion.identity);
        newBullet.GetComponent<EnemyProjectile>()?.UpdateMoveSpeed(5f);
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
