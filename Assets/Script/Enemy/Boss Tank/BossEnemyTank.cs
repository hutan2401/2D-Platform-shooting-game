using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExplosionPoint
{
    public GameObject explosionPrefab;
    public Transform point;
    public float delay;
}

public class BossEnemyTank : MonoBehaviour
{
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
    //public BossExplosionController explode;

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

    [Header("Victory Show UI")]
    [SerializeField] private GameObject victoryUI;
    [SerializeField] private Animator victoryAnim;
    [SerializeField] private float delayTime = 10;
    [Header("Explosion Points")]
    [SerializeField] private List<ExplosionPoint> explosionPoints;


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
        GameObject menuObject = GameObject.Find("VictoryUI");
        if (menuObject != null)
        {
            victoryAnim = menuObject.GetComponent<Animator>();
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
        Vector3 playerPosition = PlayerControls.Instance.transform.position;
        float playerDistance = Vector2.Distance(transform.position, playerPosition);
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
        ManagerAudioSound.Instance.PlayHitSound("FireRocketSoundSFX");
    }

    private void ThrowBomb()
    {
        Instantiate(grenadePrefab, pointThrow.position, Quaternion.identity);
        ManagerAudioSound.Instance.PlayHitSound("GunTankCurveSoundSFX");
    }

    private void FireBullet()
    {
        //Debug.Log("Boss fires a bullet!");
        GameObject newBullet = Instantiate(bulletPrefab, pointShooting.position, Quaternion.identity);
        newBullet.GetComponent<EnemyProjectile>()?.UpdateMoveSpeed(5f);
        ManagerAudioSound.Instance.PlayHitSound("GunTankFireSoundSFX");
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
        animator.SetTrigger("Die");
        ManagerAudioSound.Instance.PlayExplodeSound("ExplodeBossTank");
        StartCoroutine(Explosion());
        StartCoroutine(ShowUI());
        GameManager.Instance.OnBossDefeated();

    }
    private IEnumerator ShowUI()
    {
        if (victoryUI != null)
        {
            victoryUI.SetActive(true);
            Debug.Log("Victory UI activated.");

            if (victoryAnim != null)
            {
                victoryAnim.SetTrigger("ShowUI");
                Debug.Log("Victory animation triggered.");
            }

            yield return new WaitForSeconds(delayTime);
            victoryAnim.SetTrigger("hide");
            victoryUI.SetActive(false);

            Debug.Log("Victory UI deactivated.");
        }
        else
        {
            Debug.LogError("Victory UI is not assigned!");
        }
    }
    private IEnumerator Explosion()
    {

        foreach (var explosionPoint in explosionPoints)
        {
            Instantiate(explosionPoint.explosionPrefab, explosionPoint.point.position, Quaternion.identity);
            yield return new WaitForSeconds(explosionPoint.delay);
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
