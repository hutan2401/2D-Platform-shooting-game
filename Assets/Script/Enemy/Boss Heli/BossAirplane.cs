using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAirplane : MonoBehaviour
{
    [Header("Point Shooting System")]
    [SerializeField] private List<PointShooting1> pointShootings; 
    [SerializeField] private float distance = 5f;
    [SerializeField] private float attackCooldown = 3f;
    private bool canAttack = true; 
    private int currentAttackIndex = 0;

    [SerializeField] private Transform pointCurve;
    [SerializeField] private Transform pointCurve2;
    [SerializeField] private GameObject RocketPrefab;
    [SerializeField] private float rocketCooldown = 5f; // Cooldown between rockets
    private bool canShootRocket = true;
    private Animator animator;
    private bool isDead = false;
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

        if (Vector2.Distance(transform.position, PlayerControls.Instance.transform.position) <= distance)
        {
            if (canAttack)
            {
                StartCoroutine(ManagerAttack());
                ShootRocket();
            }
            if (canShootRocket)
            {
                StartCoroutine(RandomRocket());
            }
        }
    }

    private IEnumerator ManagerAttack()
    {
        canAttack = false;
        if (pointShootings == null || pointShootings.Count == 0)
        {
            Debug.LogError("PointShooting list is null or empty!");
            yield break;
        }
        // Chọn ngẫu nhiên hoặc theo trình tự
        //int attackIndex = Random.Range(0, pointShootings.Count);
        // Nếu muốn tấn công theo trình tự, bỏ dòng trên và dùng dòng dưới
        int attackIndex = currentAttackIndex++;

        if (attackIndex >= pointShootings.Count)
        {
            currentAttackIndex = 0;
            attackIndex = 0;
        }
        if (pointShootings[attackIndex] == null)
        {
            Debug.LogError($"PointShooting at index {attackIndex} is null!");
            yield break;
        }

        //// Gọi hàm tấn công của `PointShooting1`
        ///

        PointShooting1 point = pointShootings[attackIndex].GetComponent<PointShooting1>();
        if (point == null)
        {
            Debug.LogError($"GameObject at index {attackIndex} does not have PointShooting1 component!");
            yield break;
        }
        point.TriggerAttack();
        yield return new WaitForSeconds(attackCooldown); // Chờ cooldown trước khi cho phép tấn công tiếp
        canAttack = true;
    }

    private void ShootRocket()
    {
        Instantiate(RocketPrefab, pointCurve.position, Quaternion.identity);
    }
    private void ShootRocket2()
    {
        Instantiate(RocketPrefab, pointCurve2.position, Quaternion.identity);
    }
    private IEnumerator RandomRocket()
    {
        canShootRocket = false;

        // Randomly choose which rocket function to call
        int randomChoice = Random.Range(0, 2); // Generates 0 or 1
        if (randomChoice == 0)
        {
            ShootRocket();
        }
        else
        {
            ShootRocket2();
        }

        // Wait for cooldown before allowing another rocket shot
        yield return new WaitForSeconds(rocketCooldown);
        canShootRocket = true;
    }
    private void BossDeath()
    {
        isDead = true;
        Debug.Log("Boss is dead!");
        animator.SetTrigger("Die");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}
