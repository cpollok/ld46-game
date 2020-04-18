using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartWallHandle : MonoBehaviour {
    public CartWall wall;

    void OnCollisionEnter(Collision other) {
        Debug.Log("Collision detected.");
        
        Vector3 normal = other.contacts[0].normal;
        wall.ProcessHandleCollision(normal);
    }
}
