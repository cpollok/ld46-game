using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullshitPhysicsTrigger : MonoBehaviour
{
    private Vector3 origin;
    // Start is called before the first frame update
    void Start() {
        origin = transform.position;
    }

    // Update is called once per frame
    void Update() {
        transform.position = origin + new Vector3(0, Mathf.Sin(Time.fixedTime) * 0.01f, 0);
    }
}
