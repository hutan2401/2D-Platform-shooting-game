using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : SingleTon<PlayerControls>
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
    //private SpriteRenderer mySprite;
    private bool isFacing = true;
    //crouch
    private bool isCrouch;
    public float crouchPercentofHeihgt = 0.5f;

    private int currentLayer;

    //private Pistol pistol;

    protected override void Awake()
    {
        base.Awake();

        playerController = new PlayerController();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //pistol = GetComponent<Pistol>();
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
        //if(Input.GetKeyDown(KeyCode.C))
        //{
        //    ChangeLayer();
        //}
    }

    private void OnEnable()
    {
        playerController.Enable();
        playerController.Player.Crouching.started += _ => Crouch();
        playerController.Player.Crouching.canceled += _ => StandUp();
    }
    //private void OnDisable()
    //{
    //    if (playerController != null)
    //    {
    //        playerController.Disable();
    //    }
    //}

    private void PlayerInput()
    {
        movement = playerController.Player.Move.ReadValue<Vector2>();
        animator.SetFloat("xVelocity",Math.Abs(movement.x));
        
    }

    
    private void Move()
    {
        if (PlayerHealth.Instance.isDead)
        {
            movement = Vector2.zero;
            return;

        }
        //rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
        if (isCrouch)
        {
            rb.velocity = new Vector2(movement.x * moveSpeed * 0.5f, rb.velocity.y);
            //animator.SetFloat("crouchMoving", Mathf.Abs(rb.velocity.x));
        }
        else
        {
            rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y); 
        }
    }
    private void Jump()
    {
        if (CheckGrounded())
        {
            rb.AddForce(new Vector2(0f, jumpingPower), ForceMode2D.Impulse);
            animator.SetFloat("yVelocity", movement.y);
        }
    }
    private void Crouch()
    {
        isCrouch = true;
        animator.SetBool("isCrouch", isCrouch);
    }
    private void StandUp()
    {
        isCrouch = false;
        animator.SetBool("isCrouch", isCrouch);
    }

    private bool CheckGrounded()
    {
        bool isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        animator.SetBool("isGround",!isGround);
        return isGround;
    }

    public void FlipSprite()
    {
        if ((movement.x > 0 && !isFacing) || (movement.x < 0 && isFacing))
        {
            isFacing = !isFacing;
            transform.Rotate(0f, 180f, 0f);
        }
            
    }
    public bool IsFacingRight()
    {
        return isFacing; // Assuming 'isFacing' is true when the player is facing right.
    }

    public void ChangeLayer()
    {
        if (currentLayer == 0)
        {
            currentLayer += 1;
            animator.SetLayerWeight(currentLayer - 1, 0);
            animator.SetLayerWeight(currentLayer, 1);   
        }
        else
        {
            currentLayer -= 1;
            animator.SetLayerWeight(currentLayer + 1, 0);
            animator.SetLayerWeight(currentLayer, 0);
        }
    }

}
