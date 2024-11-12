using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speedBullet = 2f;

    private Rigidbody2D bulletRb;
    private GameObject target;
    void Start()
    {
        bulletRb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        Vector2 moveDir = (target.transform.position - transform.forward).normalized * speedBullet;
        bulletRb.velocity = new Vector2(moveDir.x, moveDir.y);
        Destroy(gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
