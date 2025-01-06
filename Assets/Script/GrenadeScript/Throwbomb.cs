using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Throwbomb : MonoBehaviour
{
    [SerializeField] private float throwForce = 6f;
    [SerializeField] private Transform transFormPointGrenade;
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private int maxGrenade = 10;
    [SerializeField] private TMP_Text grenadeText;
    private static int currentGrenade;
    private Animator animator;

    private PlayerController playerController;
    private bool isCrouch;

    private void Awake()
    {
        playerController = new PlayerController();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (currentGrenade == 0)
        {
            currentGrenade = maxGrenade;
        }
        UpdateGrenadeUI();
        playerController.Player.ThrowGrenade.performed += _ => ThrowGrenade();
        playerController.Player.Crouching.performed += _ => SetCrouch(true);
        playerController.Player.Crouching.canceled += _ => SetCrouch(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        playerController.Enable();
    }

    private void SetCrouch(bool crouch)
    {
        isCrouch = crouch;
    }

    public void ThrowGrenade()
    {
       if(currentGrenade >0)
        {
            GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
            Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
            var direction = transform.right + Vector3.up;
            rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
            currentGrenade--;
            if (isCrouch)
            {
                animator.SetTrigger("CrouchThrow");
            }
            else
            {
                animator.SetTrigger("throwBomb");
            }
            
            Debug.Log("grenade: "+ currentGrenade);
            UpdateGrenadeUI();
        }
    }

    public void AddAGrenade()
    {
        currentGrenade += 10;
        Debug.Log("Add" +currentGrenade);
        UpdateGrenadeUI();
    }

    private void UpdateGrenadeUI()
    {
        if(grenadeText != null)
        {
            grenadeText.text = ":"+ currentGrenade;
        }
    }
    public void AssignGrenadeText(TMP_Text newGrenadeText)
    {
        grenadeText = newGrenadeText;
        UpdateGrenadeUI();
    }
}
