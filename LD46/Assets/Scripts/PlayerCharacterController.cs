using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : MonoBehaviour
{
    GameObject wooden_Log;
    Rigidbody rb;

    public float speed = 5f;
    public float wood_Duration = 2f;

    Interactable interactable;

    Vector2 dir;
    bool interact = false;
    bool holding_Wood = false;

    private float wood_Time_Start = -1f;

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        wooden_Log = transform.Find("Wooden_Log").gameObject;
    }

    void Update()
    {
        Interact();
    }

    void FixedUpdate() {
        if (!interact) {
            Move();
            if (dir != Vector2.zero) {
                Turn();
            }
        }
    }

    public void Move(){
        rb.MovePosition(transform.position + new Vector3(dir.x, 0, dir.y) * speed * Time.deltaTime);
    }

    void Turn(){
        transform.LookAt(transform.position + new Vector3(dir.x, 0, dir.y));
    }

    void Interact() {
        if (interact && interactable != null) {
            if (!holding_Wood) {
                if (interactable is WoodSite) {
                    Debug.Log("interacting since " + (Time.time - wood_Time_Start).ToString());
                    if (wood_Time_Start < 0) {
                        wood_Time_Start = Time.time;
                    }
                    if (Time.time - wood_Time_Start >= wood_Duration) {
                        GetWood();
                        wood_Time_Start = -1f;
                    }
                }
            }
            else {

            }
            
            // if interactable is Machine
            // if interactable is Cart/Fire
        }
    }

    void GetWood() {
        holding_Wood = true;
        wooden_Log.SetActive(true);
    }

    void LoseWood() {
        holding_Wood = false;
        wooden_Log.SetActive(false);
    }

    public void OnMove(InputAction.CallbackContext ctx){
        dir = ctx.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext ctx) {
        interact = ctx.ReadValue<float>() > 0 ? true : false;
    }

    private void OnTriggerEnter(Collider other) {
        Interactable other_Interactable = other.GetComponent<Interactable>();
        if (other_Interactable) {
            Debug.Log("Enter Interactable");
            interactable = other_Interactable;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<Interactable>()) {
            Debug.Log("Exit Interactable");
            interactable = null;
        }
    }
}
