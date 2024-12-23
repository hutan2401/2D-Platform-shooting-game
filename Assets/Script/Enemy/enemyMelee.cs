using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class enemyMelee : MonoBehaviour
{
    [SerializeField] private float enemyMoveSpeed = 2f;
    [SerializeField] private float distance = 1f;
    [SerializeField] private Transform checkPoint;
    public LayerMask groundLayer;
    public bool facingLeft = true;

    [Header("Find Player")]
    public bool inRange = false;
    [SerializeField] private float attackRange = 10f;
    public float retrieveDistance = 2.0f;
    public float chaseSpeed = 2f;

    [Header("Attack ")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float radius;
    [SerializeField] private float delayAttackAnimation = 1.5f;
    [SerializeField] private int damage =1;
    [SerializeField] private LayerMask playerLayer;
    [Header("Points Settings")]
    [SerializeField] private int score = 5;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClipSoundEnemyDie;
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
        if(isDead) { return; }
        Vector3 playerPosition = PlayerControls.Instance.transform.position;
        if (Vector2.Distance(transform.position, playerPosition) <= attackRange)
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

            if (Vector2.Distance(transform.position, playerPosition) > retrieveDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, playerPosition, chaseSpeed * Time.fixedDeltaTime);

                speed = chaseSpeed;
            }
            else
            {
                //Debug.Log("Attack");
                TriggerAttack();
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
        animator.SetFloat("xSpeed", speed);
    }

    public void Attack()
    {
        Collider2D collHit = Physics2D.OverlapCircle(attackPoint.position, radius, playerLayer);
        if (collHit)
        {
            PlayerHealth playerHealth = collHit.gameObject.GetComponent<PlayerHealth>();

                playerHealth.TakeDamage(damage, transform);
        }
    }
    private void TriggerAttack()
    {
        StartCoroutine(DelayedAttack());
    }

    private IEnumerator DelayedAttack()
    {
        animator.SetTrigger("Attack"); 
        yield return new WaitForSeconds(delayAttackAnimation); 

    }

    public void DeathAnimation()
    {
        if (isDead) return;
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
        //if (checkPoint == null)
        //{
        //    return;
        //}
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(attackPoint.position, radius);
    }

}
