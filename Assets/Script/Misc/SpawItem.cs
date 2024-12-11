using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawItem : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnableItems;
    public void DropItems()
    {
        int randomIndex = Random.Range(0, spawnableItems.Count);
        GameObject selectedItem = spawnableItems[randomIndex];

        if (selectedItem != null)
        {
            Instantiate(selectedItem, transform.position, Quaternion.identity);
        }
    }
}
