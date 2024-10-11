using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Transform headPosition; 
    [SerializeField] private Transform defaultPosition;

    private PlayerControls playerControls;
    private PlayerController playerController;
    private Animator animator;
    private bool isLookUp;

    private void Awake()
    {
        playerController = new PlayerController();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        playerControls = PlayerControls.Instance;

        playerController.Player.Fire.performed += _ => Attack();
        playerController.Player.changeRotation.performed += _=> SetLookUp(true);
        playerController.Player.changeRotation.canceled += _ => SetLookUp(false);
    }

    private void OnEnable()
    {
        playerController.Enable();
    }

    private void OnDisable()
    {
        playerController.Disable();
    }

    public void Attack()
    {
        Debug.Log("Shooting");
        animator.SetTrigger("Attack");
        if (isLookUp)
        {
            bulletSpawnPoint.position = headPosition.position;
            bulletSpawnPoint.rotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            if (playerControls.IsFacingRight()) // Check the facing direction from PlayerControls
            {
                bulletSpawnPoint.position = defaultPosition.position;
                bulletSpawnPoint.rotation = Quaternion.identity;               
            }
            else
            {
                bulletSpawnPoint.position = defaultPosition.position;
                bulletSpawnPoint.rotation = Quaternion.Euler(0, 0, 180); // Rotate to shoot left
            }
        }
        GameObject newBullet = Instantiate(bulletPrefab,bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        // newBullet.GetComponent<ProjectTile>().UpdateProjectTileRange(/*weaponInfo*/); 
    }
    private void SetLookUp(bool lookUp)
    {
        isLookUp = lookUp;
    }
}
