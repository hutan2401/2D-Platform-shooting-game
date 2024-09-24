using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectTile : MonoBehaviour
{
    [SerializeField] private float speed = 22f;
    [SerializeField] private float projectTileRange = 10f;
    //[SerializeField] private GameObject particleOnHitPrefabVFX;
    //[SerializeField] private bool isEnemyProjecttile = false;

    private Vector3 startPostion;

    void Start()
    {
        startPostion = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        MoveProjectTile();
        DectectFireDistance();
    }

    public void UpdateProjectTileRange(float projectTileRange)
    {
        this.projectTileRange = projectTileRange;
    }    
    private void MoveProjectTile()
    {
        transform.Translate(Vector3.right *speed* Time.deltaTime);
    }

    private void DectectFireDistance()
    {
        if(Vector3.Distance(transform.position, startPostion) > projectTileRange)
        {
            Destroy(gameObject);
        }
    }

}