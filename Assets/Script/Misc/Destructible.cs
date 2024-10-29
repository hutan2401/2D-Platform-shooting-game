using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ProjectTile projectTile = collision.gameObject.GetComponent<ProjectTile>();
        ExplodeBomb grenade = collision.gameObject.GetComponent<ExplodeBomb>();
        if (projectTile || grenade )
        {
            GetComponent<SpawItem>().DropItems();
            Destroy(gameObject);
        }
    }
}
