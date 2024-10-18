using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    //[SerializeField] private GameObject bulletPrefab;
    [SerializeField] private BulletType defaultBulletType;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Transform headPosition; 
    [SerializeField] private Transform defaultPosition;

    private BulletType currentBulletType;
    private int currentAmmo;


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

        currentBulletType = defaultBulletType;
        currentAmmo = currentBulletType.isUnlimited ? int.MaxValue : currentBulletType.maxAmmo;

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
            animator.SetTrigger("ShootingUp");
        }
        else
        {
            if (playerControls.IsFacingRight()) 
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
        GameObject newBullet = Instantiate(currentBulletType.bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        ProjectTile bulletScript = newBullet.GetComponent<ProjectTile>();
        bulletScript.Initialize(currentBulletType.speed, currentBulletType.projectTileRange, currentBulletType.damage);
        if(!currentBulletType.isUnlimited)
        {
            currentAmmo--;
            {
                if (currentAmmo <= 0)
                {
                    SwitchToDefaultWeapon();
                }
            }
        }
    }
    private void SwitchToDefaultWeapon()
    {
        currentBulletType = defaultBulletType;
        currentAmmo = int.MaxValue;
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("AmmoBox"))
    //    {
    //        // Assume AmmoBox has a reference to a BulletType
    //        BulletType newBulletType = collision.gameObject.GetComponent<AmmoBox>().GetBulletType();
    //        SwitchBullet(newBulletType);
    //        Destroy(collision.gameObject); // Destroy the ammo box after pickup
    //    }
    //}

    public void SwitchWeapon(BulletType newBulletType)
    {
        currentBulletType = newBulletType;
        currentAmmo = currentBulletType.isUnlimited ? int.MaxValue : currentBulletType.maxAmmo;
        Debug.Log("Switched to new bullet type: " + currentBulletType.name);
    }
    private void SetLookUp(bool lookUp)
    {
        isLookUp = lookUp;
    }

}
