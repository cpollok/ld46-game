using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cart : Interactable {
    float position = 0.0f;
    public float velocity = 1.0f;

    public Rail rail;

    // Start is called before the first frame update
    void Start() {
        
    }

    void Update() {
        position += velocity * Time.deltaTime;

        SetPathLocation(rail.path_creator.path, position);
    }

    public void SetPathLocation(PathCreation.VertexPath path, float distance) {
        Vector3 position = path.GetPointAtDistance(distance);
        Vector3 dir      = path.GetDirectionAtDistance(distance);

        transform.position = position;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }
}
