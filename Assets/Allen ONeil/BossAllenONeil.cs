using UnityEngine;

public class BossAllenONeil : MonoBehaviour
{
    public Transform player; // Tham chiếu đến người chơi
    public Animator animator; // Animator của Allen
    public GameObject bulletPrefab; // Prefab của đạn
    public GameObject grenadePrefab; // Prefab của lựu đạn
    public Transform shootPoint; // Vị trí bắn đạn
    public Transform grenadePoint; // Vị trí ném lựu đạn

    public float health = 100f; // Máu của boss
    public float moveSpeed = 2f; // Tốc độ di chuyển của boss
    public float shootRange = 10f; // Tầm bắn súng
    public float meleeRange = 2f; // Tầm cận chiến
    public float retreatDistance = 5f; // Khoảng cách boss sẽ lùi ra để ném lựu đạn
    public float meleeDamage = 20f; // Sát thương của đòn cận chiến
    public float grenadeCooldownPhase1 = 5f; // Thời gian hồi chiêu của lựu đạn (Phase 1)
    public float shootCooldownPhase1 = 1f; // Thời gian hồi chiêu của bắn súng (Phase 1)
    public float grenadeCooldownPhase2 = 2.5f; // Thời gian hồi chiêu của lựu đạn (Phase 2)
    public float shootCooldownPhase2 = 0.5f; // Thời gian hồi chiêu của bắn súng (Phase 2)

    private float currentGrenadeCooldown; // Thời gian hồi chiêu hiện tại của lựu đạn
    private float currentShootCooldown; // Thời gian hồi chiêu hiện tại của bắn súng
    private float lastGrenadeTime;
    private float lastShootTime;
    private bool isDead = false;
    private bool isPhase2 = false; // Kiểm tra nếu boss đang ở Phase 2

    private void Start()
    {
        // Thiết lập cooldown cho Phase 1
        currentGrenadeCooldown = grenadeCooldownPhase1;
        currentShootCooldown = shootCooldownPhase1;
    }

    private void Update()
    {
        if (isDead) return;

        // Chuyển sang Phase 2 nếu máu dưới 40%
        if (!isPhase2 && health <= 40f)
        {
            EnterPhase2();
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > shootRange)
        {
            animator.Play("Idle");
        }

        // Di chuyển theo hướng người chơi
        if (distanceToPlayer > meleeRange && distanceToPlayer <= shootRange)
        {
            MoveTowardsPlayer();
        }

        // Nếu người chơi nhảy qua đầu boss, boss sẽ hướng về phía người chơi
        if (player.position.y > transform.position.y + 1f)
        {
            AimAtPlayer();
        }

        // Bắn súng
        if (distanceToPlayer <= shootRange && distanceToPlayer > meleeRange && Time.time > lastShootTime + currentShootCooldown)
        {
            Shoot();
        }

        // Tấn công cận chiến
        if (distanceToPlayer <= meleeRange)
        {
            MeleeAttack();
            RetreatFromPlayer(); // Sau đòn cận chiến, boss sẽ lùi lại để ném lựu đạn
        }

        // Ném lựu đạn khi ở khoảng cách an toàn
        if (distanceToPlayer > meleeRange && distanceToPlayer <= shootRange && Time.time > lastGrenadeTime + currentGrenadeCooldown)
        {
            ThrowGrenade();
        }
    }

    private void EnterPhase2()
    {
        isPhase2 = true;
        currentGrenadeCooldown = grenadeCooldownPhase2;
        currentShootCooldown = shootCooldownPhase2;
        animator.Play("Laugh"); // Animation khi chuyển sang Phase 2
    }

    private void MoveTowardsPlayer()
    {
        animator.Play("Running");
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    private void AimAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Hướng sang phải
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1); // Hướng sang trái
        }
    }

    private void Shoot()
    {
        animator.Play("Shooting");
        lastShootTime = Time.time;
        Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity); // Bắn đạn từ vị trí bắn
    }

    private void MeleeAttack()
    {
        animator.Play("MeleeAttack");

        // Kiểm tra xem người chơi có đang ở trong tầm cận chiến không
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= meleeRange)
        {
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(meleeDamage);
            }
        }
    }

    private void RetreatFromPlayer()
    {
        Vector2 direction = (transform.position - player.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3)direction, retreatDistance * Time.deltaTime);
    }

    private void ThrowGrenade()
    {
        animator.Play("GrenadeThrow");
        lastGrenadeTime = Time.time;

        GameObject grenade = Instantiate(grenadePrefab, grenadePoint.position, Quaternion.identity);
        Vector2 direction = (player.position - grenadePoint.position).normalized;
        Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * 5f; // Tốc độ ném lựu đạn về phía người chơi
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        animator.Play("Death");
        Destroy(gameObject, 2f); // Xóa boss sau khi chết
    }
}
