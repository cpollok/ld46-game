using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkMachine : MonoBehaviour, IMachine {
    public static int CompareByPositionOnRail(IMachine x, IMachine y) {
        if (x.GetPositionOnRail() > y.GetPositionOnRail()) {
            return 1;
        }
        else if(x.GetPositionOnRail() == y.GetPositionOnRail()) {
            return 0;
        }
        else {
            return -1;
        }
    }

    [SerializeField] private float work_duration = 5f;
    private float work_start = -1f;
    public float Progress { get { return work_start <= 0 ? 0f : Mathf.Min(1f, (Time.time - work_start) / work_duration); } }

    private float position_on_rail;
    private PathCreation.VertexPath path;

    public float GetPositionOnRail() {
        return position_on_rail;
    }

    private void Update() {
        ShowProgress();
    }

    public void PutOnRail(Rail rail) {
        path = rail.path_creator.path;
        transform.position = path.GetClosestPointOnPath(transform.position);
        position_on_rail = path.GetClosestDistanceAlongPath(transform.position);
        transform.rotation = Quaternion.LookRotation(path.GetDirectionAtDistance(position_on_rail), Vector3.up);
    }

    public void StartWork() {
        work_start = Time.time;
    }

    void ShowProgress() {
        if (Progress > 0f && Progress < 1f) {
            Debug.Log("Show progress: " + (Progress * 100).ToString() + "%");
        }
    }

    public bool Finished() {
        return Progress >= 1f;
    }
}
