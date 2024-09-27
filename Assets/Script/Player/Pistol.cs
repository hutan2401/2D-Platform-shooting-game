using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    private PlayerController playerController;

    private void Awake()
    {
        playerController = new PlayerController();
    }

    private void Start()
    {
        playerController.Player.Fire.performed += _ => Attack();
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
        GameObject newBullet = Instantiate(bulletPrefab,bulletSpawnPoint.position, bulletSpawnPoint.rotation);
       // newBullet.GetComponent<ProjectTile>().UpdateProjectTileRange(/*weaponInfo*/); 
    }
   

}
