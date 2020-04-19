using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] public Transform follow;
    [SerializeField] private Vector3 positionOffset;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = follow.position + positionOffset;
    }
}
