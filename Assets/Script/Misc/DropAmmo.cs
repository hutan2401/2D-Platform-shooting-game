using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropAmmo : MonoBehaviour
{
    [SerializeField] private GameObject ammoBox;
    
    
    public void DropAmmoBox()
    {
        Instantiate(ammoBox,transform.position,Quaternion.identity);
    }
}
