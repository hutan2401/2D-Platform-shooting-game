using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;


    void Start()
    {
        
    }

   public void Attack()
    {
        GameObject newBullet = Instantiate(bulletPrefab,bulletSpawnPoint.position,transform.rotation);
       // newBullet.GetComponent<ProjectTile>().UpdateProjectTileRange(/*weaponInfo*/); 
    }
}
