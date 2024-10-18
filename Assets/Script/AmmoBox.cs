using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    [SerializeField] private BulletType bulletType;

    public BulletType GetBulletType()
    {
        return bulletType;
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
