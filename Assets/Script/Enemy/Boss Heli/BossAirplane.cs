using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BossExplosionController;

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

    [Header("Victory Show UI")]
    [SerializeField] private GameObject victoryUI;
    [SerializeField] private Animator victoryAnim;
    [SerializeField] private float delayTime = 10;

    [Header("Explosion Points")]
    [SerializeField] private List<ExplosionPoint> explosionPoints;

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
        GameObject menuObject = GameObject.Find("VictoryUI");
        if (menuObject != null)
        {
            victoryAnim = menuObject.GetComponent<Animator>();
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
        ManagerAudioSound.Instance.PlayHitSound("FireRocketSoundSFX");
    }
    private void ShootRocket2()
    {
        Instantiate(RocketPrefab, pointCurve2.position, Quaternion.identity);
        ManagerAudioSound.Instance.PlayHitSound("FireRocketSoundSFX");
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
        animator.SetTrigger("Die");
        ManagerAudioSound.Instance.PlayExplodeSound("ExplodeBossAirPlane");
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}
