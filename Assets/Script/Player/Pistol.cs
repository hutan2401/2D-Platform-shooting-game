using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pistol : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private BulletType defaultBulletType;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Transform headPosition; 
    [SerializeField] private Transform defaultPosition;
    [SerializeField] private Transform crouchPostion;
    [Header("Other Settings")]
    [SerializeField] private TMP_Text bulletAmmoText;

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

    private AudioHitSound hitSound;

    private void Awake()
    {
        playerController = new PlayerController();
        animator = GetComponent<Animator>();
        hitSound = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioHitSound>();

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
        UpdateAmmoUI();
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
        UpdateAmmoUI();
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
        if (isCrouch)
        {
            // Trigger crouching melee attack animation
            animator.SetTrigger("CrouchingMeleeAttack");
        }
        else
        {
            // Trigger regular melee attack animation
            animator.SetTrigger("MeleeAttack");
        }
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(damageCollider.position, distance);
        
        foreach (var enemy in hitEnemies)
        {
            var enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
                hitSound.PlaySFX(hitSound.KnifeHitSoundSFX);
            }
        }
    }
    private void TriggerRangedAttack()
    {
        //Debug.Log("Shooting");

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
            currentAmmo--;
        }
        animator.SetTrigger("Attack");
        if (!currentBulletType.isUnlimited)
        {
            //currentAmmo--;
            {
                if (currentAmmo <= 0)
                {
                    SwitchToDefaultWeapon();
                    ChangeLayer(1);
                }
                UpdateAmmoUI();
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
        //for(int i = 0; i < currentBulletType.burstCount; i++)
        //{
        //    FireBullet();
        //    yield return new WaitForSeconds(currentBulletType.burstDelayTime);
        //}
        int shotsFired = 0;
        for (int i = 0; i < currentBulletType.burstCount; i++)
        {
            // Check if there's enough ammo before each shot
            if (currentAmmo <= 0)
            {
                SwitchToDefaultWeapon();
                ChangeLayer(1);
                yield break;  // Exit the burst if out of ammo
            }

            FireBullet();
            shotsFired++;
            yield return new WaitForSeconds(currentBulletType.burstDelayTime);
        }

        // Deduct ammo based on the number of shots fired in the burst
        currentAmmo -= shotsFired;

        // Check if we've run out of ammo after the burst
        if (currentAmmo <= 0)
        {
            SwitchToDefaultWeapon();
            ChangeLayer(1);
        }
    }
    private void UpdateAmmoUI()
    {
        if (currentBulletType.isUnlimited)
        {
            bulletAmmoText.text = "∞";
        }
        else
        {
            bulletAmmoText.text =currentAmmo.ToString();
        }
    }
}
