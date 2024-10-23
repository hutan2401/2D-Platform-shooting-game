using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAmmoBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ProjectTile>())
        {
            GetComponent<DropAmmo>().DropAmmoBox();
            Destroy(gameObject);
        }
    }
}
