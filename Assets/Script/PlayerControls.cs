using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpingPower = 15f;
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private  PlayerController playerController;
    private Vector2 movement;

    private void Awake()
    {
        playerController = new PlayerController();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        playerController.Player.Jump.performed += _ => Jump();
    }
    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        Move();
        CheckGrounded();
    }

    private void OnEnable()
    {
        playerController.Enable();

    }
    private void OnDisable()
    {
        playerController.Disable();
    }

    private void PlayerInput()
    {
        movement = playerController.Player.Move.ReadValue<Vector2>();
    }
    private void Move()
    {
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
    }
    private void Jump()
    {
        if (CheckGrounded())
        {
            rb.AddForce(new Vector2(0f, jumpingPower), ForceMode2D.Impulse);
        }
    }
    private bool CheckGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}
