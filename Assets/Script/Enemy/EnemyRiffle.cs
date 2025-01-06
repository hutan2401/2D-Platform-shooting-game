using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyRiffle : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float enemyMoveSpeed = 2f;
    [SerializeField] private float distance = 1f;
    [SerializeField] private Transform checkPoint;
    [SerializeField] private LayerMask groundLayer;
    public bool facingLeft = true;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform pointShooting;
    [SerializeField] private float shootingRange = 5f;
    [SerializeField] private float fireCooldown = 1.0f;

    [Header("Points Settings")]
    [SerializeField] private int score = 5;
    private float cooldownTimer = 0f;

    [Header("Sounds Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClipSoundEnemyDie;

    public bool inRange = false;
    private Animator animator;
    private bool isDead = false;
    private void Start()
    {
       // explosionController = GetComponent<BossExplosionController>();
        animator = GetComponent<Animator>();
        EnemyHealth healthComponent = GetComponent<EnemyHealth>();
        if (healthComponent != null)
        {
            healthComponent.OnEnemyDeath.AddListener(DeathAnimation);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        float speed = 0f;
        Vector3 playerPosition = PlayerControls.Instance.transform.position;
        if (isDead) { return; }
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (Vector2.Distance(transform.position, playerPosition) <= shootingRange)
        {
            inRange = true;
        }
        else { inRange = false; }

        if (inRange)
        {

            if (playerPosition.x > transform.position.x && facingLeft)
            {
                FLip();
            }
            else if (playerPosition.x < transform.position.x && !facingLeft)
            {
                FLip();
            }
            if (cooldownTimer <= 0)
            {
                animator.SetTrigger("Shooting");
                ManagerAudioSound.Instance.PlayHitSound("RifleGunSoundSFX");
                FireBullet();
                cooldownTimer = fireCooldown;
            }
        }
        else
        {
            transform.Translate(Vector2.left * Time.deltaTime * enemyMoveSpeed);
            RaycastHit2D hitGround = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, groundLayer);

            if (hitGround == false && facingLeft )
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingLeft = false;
            }
            else if (hitGround == false && facingLeft == false)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingLeft = true;
            }
            speed = enemyMoveSpeed;
        }
        animator.SetFloat("xSpeed",speed);
    }

    private void FireBullet()
    {
        GameObject newBullet = Instantiate(bulletPrefab, pointShooting.position, pointShooting.rotation);
        EnemyProjectile bulletScript = newBullet.GetComponent<EnemyProjectile>();
        newBullet.GetComponent<EnemyProjectile>().UpdateMoveSpeed(5f);
    }

    public void DeathAnimation()
    {
        if(isDead) return;
        isDead = true;
        if (audioSource != null && audioClipSoundEnemyDie != null)
        {
            audioSource.PlayOneShot(audioClipSoundEnemyDie);
        }
        ScoreManager.Instance.UpdateScore(score);
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
    }
    private void FLip()
    {
        facingLeft = !facingLeft;
        float rotationY = facingLeft ? 0 : -180;
        transform.eulerAngles = new Vector3(0, y: rotationY, 0);
    }
    private void OnDrawGizmosSelected()
    {
        if (checkPoint == null)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
   }
