using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet Type")]
public class BulletType : ScriptableObject
{
    public GameObject bulletPrefab; // Prefab for the specific bullet type
    public float speed; // Speed of the bullet
    public int damage; // Damage value of the bullet
    public int projectTileRange;
    public Sprite icon; // Icon for the bullet type (if you want to show it on UI)
}
