using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : FireInteractor<Fire> {

    public float famish_per_s = 0.2f;
    public ParticleSystem[] particle_systems;

    private BoxCollider area_of_effect;

    // Start is called before the first frame update
    void Start() {
        area_of_effect = GetComponent<BoxCollider>();

        foreach (ParticleSystem s in particle_systems) {

            float avg_particle_size = 0.5f * (s.main.startSize.constantMin + s.main.startSize.constantMax);

            var shape = s.shape;
            shape.scale = new Vector3(area_of_effect.size.x - avg_particle_size,
                                        area_of_effect.size.y - avg_particle_size, 1);
        }
    }

    // Update is called once per frame
    void Update() {
        Vector3 fire_on_plane = transform.InverseTransformPoint(fire.flame_collider.bounds.center);
        fire_on_plane.z = 0;
        if (Mathf.Abs(fire_on_plane.x) <= area_of_effect.size.x * 0.5f &&
            Mathf.Abs(fire_on_plane.y) <= area_of_effect.size.y * 0.5f) {

            Vector3 raycast_origin = transform.TransformPoint(fire_on_plane);
            Vector3 raycast_dir    = transform.forward;

            RaycastHit hit;
            if (Physics.Raycast(raycast_origin + raycast_dir * 0.1f, raycast_dir, out hit, area_of_effect.size.z * 2)) {

                Debug.DrawRay(raycast_origin, raycast_dir * hit.distance, Color.cyan);
                if (hit.collider == fire.flame_collider) {
                    fire.Famish(famish_per_s * Time.deltaTime);
                }
            }
        }
    }
}
