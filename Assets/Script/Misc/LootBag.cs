using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<LootItem> lootList = new List<LootItem>();

   

    LootItem GetDroppedItem()
    {
        int random = Random.Range(1, 101);
        List<LootItem> possibleItems = new List<LootItem>();
        foreach (LootItem item in lootList)
        {
            if(random <= item.dropChance)
            {
                possibleItems.Add(item);

            }
        }
        if(possibleItems.Count > 0)
        {
            LootItem droppedItem = possibleItems[Random.Range(0,possibleItems.Count)];
            return droppedItem;
        }
        return null;

    }
    public void DropItems(Vector3 spawPostion)
    {
        LootItem droppedItem = GetDroppedItem();      
        if (droppedItem != null)
        {
            GameObject lootGameObject = Instantiate(droppedItemPrefab,spawPostion,Quaternion.identity);
            lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.loopSprite;
        }
    }

}
