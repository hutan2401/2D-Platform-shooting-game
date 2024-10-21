using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawItem : MonoBehaviour
{
    [SerializeField] private GameObject itemScore, healthGlobe;
    public void DropItems()
    {
        int randomNum = Random.Range(1, 2);

        if (randomNum == 2)
        {
            Instantiate(healthGlobe, transform.position, Quaternion.identity);
        }


        if (randomNum == 1)
        {
            Instantiate(itemScore, transform.position, Quaternion.identity);
            //int randomAmountOfGold = Random.Range(1, 4);

            //for (int i = 0; i < randomAmountOfGold; i++)
            //{
            //    Instantiate(itemScore, transform.position, Quaternion.identity);
            //}
        }
    }
}
