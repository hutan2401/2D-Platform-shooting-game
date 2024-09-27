using System;
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

    //Animator
    private Animator animator;
    private SpriteRenderer mySprite;
    //private bool isFacing = true;

    private void Awake()
    {
        playerController = new PlayerController();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
        FlipSprite();
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
        animator.SetFloat("xVelocity",Math.Abs(movement.x));
        
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
            animator.SetFloat("yVelocity", movement.y);
        }
    }
    private bool CheckGrounded()
    {
        bool isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        animator.SetBool("isGround",!isGround);
        return isGround;
    }

    private void FlipSprite()
    {
        if (movement.x != 0) // Only flip when the character is moving horizontally
        {
            transform.localScale = new Vector3(Mathf.Sign(movement.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
