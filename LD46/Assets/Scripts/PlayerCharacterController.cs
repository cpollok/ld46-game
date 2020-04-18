using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : FireInteractor<Fire>
{
    GameObject wooden_log;
    Rigidbody rb;

    public float speed = 5f;
    public float wood_duration = 2f;

    Interactable interactable;

    Vector2 dir;
    bool interact = false;
    bool holding_wood = false;

    private float wood_time_start = -1f;

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        wooden_log = transform.Find("Body").Find("Wooden_Log").gameObject;
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
            if (!holding_wood) {
                if (interactable is WoodSite) {
                    Debug.Log("Hacking Wood " + (Time.time - wood_time_start).ToString());
                    if (wood_time_start < 0) {
                        wood_time_start = Time.time;
                    }
                    if (Time.time - wood_time_start >= wood_duration) {
                        GetWood();
                        wood_time_start = -1f;
                    }
                }
            }
            else {
                if (interactable is CartInteractable) {
                    Debug.Log("Interacting with fire cart");
                    LoseWood();
                    fire.Stoke();
                }
            }
            
            // if interactable is Machine
            // if interactable is Cart/Fire
        }
    }

    void GetWood() {
        holding_wood = true;
        wooden_log.SetActive(true);
    }

    void LoseWood() {
        holding_wood = false;
        wooden_log.SetActive(false);
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
