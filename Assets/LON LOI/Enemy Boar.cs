using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;        // Tốc độ di chuyển của enemy
    public float chaseRange = 5f;       // Khoảng cách để enemy bắt đầu đuổi theo player
    public float attackRange = 1f;      // Khoảng cách để enemy tấn công player
    public Vector2 patrolStart;         // Điểm bắt đầu tuần tra
    public Vector2 patrolEnd;           // Điểm kết thúc tuần tra
    public float attackCooldown = 1.5f; // Thời gian hồi chiêu sau mỗi lần tấn công

    private Vector2 targetPoint;        // Điểm đích mà enemy sẽ di chuyển đến
    private bool movingToStart = true;  // Enemy đang di chuyển về điểm bắt đầu tuần tra
    private Transform player;           // Tham chiếu đến đối tượng player
    private Animator animator;          // Tham chiếu đến Animator của enemy
    private bool isAttacking = false;   // Kiểm tra trạng thái tấn công
    private float lastAttackTime;       // Thời gian của lần tấn công cuối cùng
    private bool facingRight = true;    // Kiểm tra trạng thái hướng của enemy

    // Start is called before the first frame update
    void Start()
    {
        // Lấy đối tượng player dựa trên tag "Player"
        player = GameObject.FindGameObjectWithTag("Player").transform;
        targetPoint = patrolStart;
        animator = GetComponent<Animator>();  // Lấy component Animator của enemy
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Nếu player trong phạm vi đuổi theo
        if (distanceToPlayer <= chaseRange && distanceToPlayer > attackRange)
        {
            ChasePlayer();
        }
        // Nếu player trong phạm vi tấn công và không trong trạng thái tấn công
        else if (distanceToPlayer <= attackRange && Time.time > lastAttackTime + attackCooldown)
        {
            AttackPlayer();
        }
        else
        {
            Patrol();
        }
    }

    // Enemy tuần tra giữa 2 điểm
    void Patrol()
    {
        if (movingToStart)
        {
            MoveTo(patrolStart);
            if (Vector2.Distance(transform.position, patrolStart) < 0.1f)
            {
                movingToStart = false;
                targetPoint = patrolEnd;
            }
        }
        else
        {
            MoveTo(patrolEnd);
            if (Vector2.Distance(transform.position, patrolEnd) < 0.1f)
            {
                movingToStart = true;
                targetPoint = patrolStart;
            }
        }

        // Kích hoạt animation di chuyển nếu enemy di chuyển
        animator.SetBool("isMoving", true);
        FlipEnemy(targetPoint);  // Lật hình khi thay đổi hướng
    }

    // Enemy đuổi theo player
    void ChasePlayer()
    {
        MoveTo(player.position);

        // Kích hoạt animation di chuyển khi đuổi theo player
        animator.SetBool("isMoving", true);
        FlipEnemy(player.position);  // Lật hình khi đuổi theo player
    }

    // Enemy tấn công player
    void AttackPlayer()
    {
        // Kích hoạt animation tấn công nếu có
        animator.SetBool("isAttacking", true);
        isAttacking = true;
        lastAttackTime = Time.time; // Cập nhật thời gian tấn công cuối cùng
        Debug.Log("Enemy tấn công player!");

        // Sau khi tấn công, reset animation sau một chút thời gian
        StartCoroutine(ResetAttackAnimation());
    }

    // Di chuyển enemy đến một điểm nào đó
    void MoveTo(Vector2 target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    // Lật hình enemy khi di chuyển
    void FlipEnemy(Vector2 target)
    {
        if ((target.x > transform.position.x && !facingRight) || (target.x < transform.position.x && facingRight))
        {
            facingRight = !facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;  // Lật hình bằng cách thay đổi trục x
            transform.localScale = localScale;
        }
    }

    // Coroutine để reset animation tấn công sau khi kết thúc
    IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(0.5f); // Đợi nửa giây trước khi reset
        animator.SetBool("isAttacking", false);         // Reset animation tấn công
        isAttacking = false;                   // Đặt trạng thái tấn công thành false
    }

    // Hàm này để hiển thị vùng phát hiện của enemy trong editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
