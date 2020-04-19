using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refinery : Interactable, IMachine
{
    private float position_on_rail;
    private PathCreation.VertexPath path;

    public float GetPositionOnRail() {
        return position_on_rail;
    }

    public void PutOnRail(Rail rail) {
        path = rail.path_creator.path;
        transform.position = path.GetClosestPointOnPath(transform.position);
        position_on_rail = path.GetClosestDistanceAlongPath(transform.position);
        transform.rotation = Quaternion.LookRotation(path.GetDirectionAtDistance(position_on_rail), Vector3.up);
    }

    public bool Finished() {
        return false;
    }
}
