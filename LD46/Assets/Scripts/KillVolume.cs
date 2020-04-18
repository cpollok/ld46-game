using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillVolume : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        Debug.Log("test");
        Debug.Log(other.gameObject);
        if (other.transform.parent) {
            PlayerCharacterController controller = other.transform.parent.GetComponent<PlayerCharacterController>();
            if (controller) {
                controller.Die();
            }

        }
    }
}
