using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Weapon
{
    public void Attack();
    public ManagerWeaponInfo GetWeaponInfo();
}
