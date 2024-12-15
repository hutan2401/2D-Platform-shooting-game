using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemy;
    public float respawnTime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawner());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator EnemySpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime);
            SpawnEnemy();
        }
        
    }
    void SpawnEnemy()
    {
        Instantiate(enemy[0], transform.position, Quaternion.identity);
    }
}
