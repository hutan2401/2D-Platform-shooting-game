using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;

    private bool canAttack = true;

    private enum State
    {
        Roaming,
        Attacking
    }

    private Vector2 roamPosstion;
    private float timeRoaming = 0f;

    private State state;
    private EnemyPathFinding enemyPathFinding;

    private void Awake()
    {
        enemyPathFinding = GetComponent<EnemyPathFinding>();
        state = State.Roaming;
    }
    private void Start()
    {
        roamPosstion = GetRoamingPositon();
    }

    private void Update()
    {
        MovementStatControl();
    }

    private void MovementStatControl()
    {
        switch (state)
        {
            default:
            case State.Roaming:
                Roaming();
                break;

            case State.Attacking:
                Attacking();
                break;
        }
    }

    private void Attacking()
    {
        if (Vector2.Distance(transform.position, PlayerControls.Instance.transform.position) > attackRange)
        {
            state = State.Roaming;
        }
        if (attackRange != 0 && canAttack)
        {
            canAttack = false;
            (enemyType as IEnemy).Attack();

            if (stopMovingWhileAttacking)
            {
                enemyPathFinding.StopMoving();
            }
            else
            {
                enemyPathFinding.MoveTo(roamPosstion);
            }
            StartCoroutine(AttackCooldownRoutine());
        }
    }

    private void Roaming()
    {
        timeRoaming += Time.deltaTime;
        enemyPathFinding.MoveTo(roamPosstion);

        if (Vector2.Distance(transform.position, PlayerControls.Instance.transform.position) < attackRange)
        {
            state = State.Attacking;
        }

        if (timeRoaming > roamChangeDirFloat)
        {
            roamPosstion = GetRoamingPositon();
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private Vector2 GetRoamingPositon()
    {
        timeRoaming = 0f;
        float randomX = UnityEngine.Random.Range(-1f, 1f);
        float fixedYPosition = transform.position.y;
        return new Vector2(transform.position.x + randomX, fixedYPosition).normalized;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
