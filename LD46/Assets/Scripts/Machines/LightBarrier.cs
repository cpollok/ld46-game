using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBarrier : FireInteractor<Fire>, IMachine
{
    public AudioSource dingSound;
    public float power_req = 0.95f;
    bool startedWork = false;

    public GameObject beam;

    private PathCreation.VertexPath path;
    private float position_on_rail;

    public float GetPositionOnRail() {
        return position_on_rail;
    }
    
    void Start()
    {
        if (!dingSound) {
            dingSound = GetComponent<AudioSource>();
        }
    }
    
    void Update()
    {
        if (startedWork && Finished()) {
            dingSound.Play();
            RaiseBeam();
            startedWork = false;
        }
    }

    public void StartWork() {
        startedWork = true;
    }

    private void RaiseBeam() {
        LeanTween.rotateAroundLocal(beam, Vector3.up, 75, 1f).setEaseInElastic().setEaseOutBounce();
    }

    public void PutOnRail(Rail rail) {
        path = rail.path_creator.path;
        transform.position = path.GetClosestPointOnPath(transform.position);
        position_on_rail = path.GetClosestDistanceAlongPath(transform.position);
        transform.rotation = Quaternion.LookRotation(path.GetDirectionAtDistance(position_on_rail), Vector3.up);
    }

    public bool Finished() {
        return fire.power >= power_req;
    }
}
