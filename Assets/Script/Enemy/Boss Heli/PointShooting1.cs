using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointShooting1 : MonoBehaviour
{
    [Header("Point Shooting 1")]
    
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float distance = 5f;

    private bool canAttack = true;
    private void Update()
    {
        if (Vector2.Distance(transform.position, PlayerControls.Instance.transform.position) <= distance)
        {
            if (canAttack)
            {
                TriggerAttack();
            }
        }
    }

    public void TriggerAttack()
    {
        if (canAttack && enemyType is IEnemy enemy)
        {
            enemy.Attack(); // Gọi hàm Attack của `Shooter`
            StartCoroutine(AttackCooldownRoutine());
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}
