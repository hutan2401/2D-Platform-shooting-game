using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwbomb : MonoBehaviour
{
    [SerializeField] private float throwForce = 6f;
    [SerializeField] private Transform transFormPointGrenade;
    [SerializeField] private GameObject grenadePrefab;

    private PlayerController playerController;

    private void Awake()
    {
        playerController = new PlayerController();
    }

    private void Start()
    {
        playerController.Player.ThrowGrenade.performed += _ => ThrowGrenade();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        playerController.Enable();
    }

    public void ThrowGrenade()
    {
        GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
        Rigidbody2D rb =  grenade.GetComponent<Rigidbody2D>();
        var direction = transform.right + Vector3.up;
        rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
    }
}