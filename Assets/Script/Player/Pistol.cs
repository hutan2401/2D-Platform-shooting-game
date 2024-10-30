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
    [SerializeField] private Transform crouchPostion;

    public Transform damageCollider; // Collider for melee attack
    public int damageAmount = 10;     // Damage for the melee attack
    public float distance = 1.5f;    // Melee attack range

    private BulletType currentBulletType;
    private int currentAmmo;


    private PlayerControls playerControls;
    private PlayerController playerController;
    private Animator animator;
    private bool isLookUp;
    private bool isCrouch;


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

        playerController.Player.Crouching.performed += _ => SetCrouchp(true);
        playerController.Player.Crouching.canceled += _ => SetCrouchp(false);
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
        if (EnemyInMeleeRange())
        {
            TriggerMeleeAttack();
        }else
        {
            TriggerRangedAttack();
        }
    }
    public void ChangeLayer(int currentLayer)
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
    private void SwitchToDefaultWeapon()
    {
        currentBulletType = defaultBulletType;
        currentAmmo = int.MaxValue;
    }
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
    private void SetCrouchp(bool crouch)
    {
        isCrouch = crouch;
    }
    private bool EnemyInMeleeRange()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(damageCollider.position, distance);
        foreach (var enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy")) 
            {
                return true;
            }
        }
        return false;
    }
    private void TriggerMeleeAttack()
    {
        animator.SetTrigger("MeleeAttack"); // Trigger knife animation
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(damageCollider.position, distance);
        foreach (var enemy in hitEnemies)
        {
            var enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
            }
        }
    }
    private void TriggerRangedAttack()
    {
        Debug.Log("Shooting");
        animator.SetTrigger("Attack");
        if (isLookUp)
        {
            bulletSpawnPoint.position = headPosition.position;
            bulletSpawnPoint.rotation = Quaternion.Euler(0, 0, 90);
            animator.SetTrigger("ShootingUp");
        }
        else if (isCrouch)
        {
            if (playerControls.IsFacingRight())
            {
                bulletSpawnPoint.position = crouchPostion.position;
                bulletSpawnPoint.rotation = Quaternion.identity;
            }
            else
            {
                bulletSpawnPoint.position = crouchPostion.position;
                bulletSpawnPoint.rotation = Quaternion.Euler(0, 0, 180); // Rotate to shoot left
            }
            animator.SetTrigger("ShootingCrouch");
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
        AudioManager.Instance.PlayShootingSound(currentBulletType.bulletTypeName);
        //GameObject newBullet = Instantiate(currentBulletType.bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        //ProjectTile bulletScript = newBullet.GetComponent<ProjectTile>();
        //bulletScript.Initialize(currentBulletType.speed, currentBulletType.projectTileRange, currentBulletType.damage);
        if (currentBulletType.burstCount > 1)
        {
            StartCoroutine(BurstFire());
        }
        else
        {
            FireBullet();
        }
        if (!currentBulletType.isUnlimited)
        {
            currentAmmo--;
            {
                if (currentAmmo <= 0)
                {
                    SwitchToDefaultWeapon();
                    ChangeLayer(1);
                }
            }
        }
    }
    private void FireBullet()
    {
        GameObject newBullet = Instantiate(currentBulletType.bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        ProjectTile bulletScript = newBullet.GetComponent<ProjectTile>();
        bulletScript.Initialize(currentBulletType.speed, currentBulletType.projectTileRange, currentBulletType.damage);
    }
    private IEnumerator BurstFire()
    {
        for(int i = 0; i < currentBulletType.burstCount; i++)
        {
            FireBullet();
            yield return new WaitForSeconds(currentBulletType.burstDelayTime);
        }
    }
}
