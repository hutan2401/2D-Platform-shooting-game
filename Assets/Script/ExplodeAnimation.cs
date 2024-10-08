using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeAnimation : MonoBehaviour
{
    public void ExplodeDone()
    {
        Destroy(gameObject);
    }
}
