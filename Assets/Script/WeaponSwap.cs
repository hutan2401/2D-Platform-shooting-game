using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    public Transform weaponSlot;
    public GameObject activeWeapon;

    void Start()
    {
        var weapon = Instantiate(activeWeapon, weaponSlot.transform.position, weaponSlot.transform.rotation);
        weapon.transform.parent = weaponSlot.transform;
    }

    private void WeaponSwapParent()
    {
        var weapon = Instantiate(activeWeapon,weaponSlot.transform.position,weaponSlot.transform.rotation);
        weapon.transform.parent = weaponSlot.transform;
    }

    public void UpdateWeapon(GameObject newWeapon)
    {
        activeWeapon = newWeapon;
        
        var weapon = Instantiate(activeWeapon,weaponSlot.transform.position,weaponSlot.transform.rotation);
        weapon.transform.parent = weaponSlot.transform;
    }
    /*public GameObject[] weapons;
      private int currentWeaponIndex = 0;

      void Start()
      {
          SelectWeapon();
      }

      public void SwitchWeapon(int weaponIndex)
      {
          currentWeaponIndex = weaponIndex;
          SelectWeapon();
      }

      void SelectWeapon()
      {
          for (int i = 0; i < weapons.Length; i++)
          {
              weapons[i].SetActive(i == currentWeaponIndex);
          }
      }*/
}
