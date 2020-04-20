using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : FireInteractor<Fire> {

    public float famish_per_s = 0.2f;
    public ParticleSystem[] particle_systems;

    public Camera mainCamera;

    private BoxCollider area_of_effect;

    private int raycastMask = 0;

    // Start is called before the first frame update
    void Start() {
        area_of_effect = GetComponent<BoxCollider>();

        mainCamera = GameObject.FindObjectOfType<Camera>();

        foreach (ParticleSystem s in particle_systems) {

            float avg_particle_size = 0.5f * (s.main.startSize.constantMin + s.main.startSize.constantMax);

            var shape = s.shape;
            shape.scale = new Vector3(Mathf.Max(0.5f * area_of_effect.size.x, area_of_effect.size.x - avg_particle_size),
                                      Mathf.Max(0.5f * area_of_effect.size.y, area_of_effect.size.y - avg_particle_size), 1);
        }

        raycastMask = LayerMask.GetMask("WindColliders", "Fire");
    }

    // Update is called once per frame
    void Update() {
        if (mainCamera) {
            Vector3 eInWorld = area_of_effect.transform.TransformVector(area_of_effect.size) * 0.5f;
            Debug.DrawLine(area_of_effect.transform.position + eInWorld,
                           area_of_effect.transform.position - eInWorld, Color.red);

            bool onScreen = false;
            for (float x = -1.0f; x <= 1.0f && !onScreen; x += 0.5f / eInWorld.x) {
                for (float z = -1.0f; z <= 1.0f && !onScreen; z += 0.5f / eInWorld.z) {
                    Vector3 worldSamplePoint = new Vector3(eInWorld.x * x, 0, eInWorld.z * z) + area_of_effect.transform.position;
                    Vector3 vpPoint = mainCamera.WorldToViewportPoint(worldSamplePoint);
                    onScreen = vpPoint.x > 0 && vpPoint.x < 1 && vpPoint.y > 0 && vpPoint.y < 1;
                    Debug.DrawRay(worldSamplePoint, Vector3.up * 0.2f);
                }
            }

            if (onScreen) {
                foreach (ParticleSystem p in particle_systems)
                    if (!p.isPlaying)
                        p.Play();
            } else {
                foreach (ParticleSystem p in particle_systems)
                    if (p.isPlaying)
                        p.Pause();
            }
        }

        Vector3 fire_on_plane = transform.InverseTransformPoint(fire.flame_collider.bounds.center);
        fire_on_plane.z = 0;
        if (Mathf.Abs(fire_on_plane.x) <= area_of_effect.size.x * 0.5f &&
            Mathf.Abs(fire_on_plane.y) <= area_of_effect.size.y * 0.5f) {

            Vector3 raycast_origin = transform.TransformPoint(fire_on_plane);
            Vector3 raycast_dir    = transform.forward;

            RaycastHit hit;
            if (Physics.Raycast(raycast_origin + raycast_dir * 0.1f,
                                raycast_dir, out hit, area_of_effect.size.z * 2,
                                raycastMask, QueryTriggerInteraction.Ignore)) {

                Debug.DrawRay(raycast_origin, raycast_dir * hit.distance, Color.cyan);
                if (hit.collider == fire.flame_collider) {
                    fire.Famish(famish_per_s * Time.deltaTime);
                }
            }
        }
    }
}
