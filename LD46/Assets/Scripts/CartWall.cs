using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartWall : MonoBehaviour {

    public Vector3 handle_dir = Vector3.right;
    private Vector3 forward_dir = Vector3.forward;

    private LTDescr latestAnim = null;

    // Start is called before the first frame update
    void Start() {
        forward_dir = Vector3.Cross(Vector3.up, handle_dir);
    }

    public void ProcessHandleCollision(Vector3 contact_normal) {
        if (latestAnim != null)
            return;

        Vector3 lnormal = transform.InverseTransformVector(contact_normal);
        float dot = Vector3.Dot(forward_dir, lnormal);
        if (dot >= 0.2f) {
            latestAnim = LeanTween.rotateAroundLocal(gameObject, Vector3.up, 90, 0.2f);
        } else if (dot <= -0.2f)  {
            latestAnim = LeanTween.rotateAroundLocal(gameObject, Vector3.up, -90, 0.2f);
        }
    }

    // Update is called once per frame
    void Update() {
        if (latestAnim != null && !LeanTween.isTweening(latestAnim.id))
            latestAnim = null;
    }
}
