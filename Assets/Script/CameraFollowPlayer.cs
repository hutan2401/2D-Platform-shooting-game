using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform target; 
    public float smoothSpeed = 0.25f;
    private Vector3 offset = new Vector3(0f,0f,-10f);
    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        Vector3 targert = target.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targert, ref velocity, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
