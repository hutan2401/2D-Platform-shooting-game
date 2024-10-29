using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    [SerializeField] private BulletType bulletType;

    //public BulletType GetBulletType()
    //{
    //    return bulletType;
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Pistol pistol = collision.gameObject.GetComponent<Pistol>();
            if (pistol != null)
            {
                pistol.ChangeLayer(0);
                pistol.SwitchWeapon(bulletType);
                Destroy(gameObject);  // Ammo box disappears after pickup
            }
        }
    }
    /* public int weaponIndex; // Index of the weapon to switch to

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WeaponSwitcher weaponSwitcher = other.GetComponent<WeaponSwitcher>();
            if (weaponSwitcher != null)
            {
                weaponSwitcher.SwitchWeapon(weaponIndex);
            }
        }
    }*/
}
