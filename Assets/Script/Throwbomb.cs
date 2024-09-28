using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwbomb : MonoBehaviour
{
    [SerializeField] private float speedThrow = 4f;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void thrown()
    {
        var direction = transform.right + Vector3.up;
        GetComponent<Rigidbody2D>().AddForce(direction *speedThrow,ForceMode2D.Impulse);
    }
}
