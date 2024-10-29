using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAmmoBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ProjectTile projectTile = collision.gameObject.GetComponent<ProjectTile>();
        ExplodeBomb grenade = collision.gameObject.GetComponent<ExplodeBomb>();
        if (projectTile || grenade)
        {
            GetComponent<DropAmmo>().DropAmmoBox();
            Destroy(gameObject);
        }
    }
}
