using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : FireInteractor<Fire>
{
    GameObject wooden_log;
    GameObject coal_lumps;
    Rigidbody rb;

    public GameObject progressBarPrefab;
    ProgressBar progressBar;

    public float speed = 5f;
    public float wood_duration = 2f;
    public float coal_duration = 3f;

    Interactable interactable;

    Vector2 dir;
    bool interact = false;
    bool holding_wood = false;
    bool holding_coal = false;

    private float wood_time_start = -1f;
    private float coal_time_start = -1f;

    private bool dead = false;
    public bool Dead { get => dead; set => dead = value; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        wooden_log = transform.Find("Body").Find("Wooden_Log").gameObject;
        coal_lumps = transform.Find("Body").Find("Coal_Lumps").gameObject;
    }

    void Update()
    {
        if (dead) {
            return;
        }
        Interact();
    }

    void FixedUpdate() {
        if (dead) {
            return;
        }
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
        if (InFireRange() && interact && interactable) {
            if(!holding_wood && !holding_coal && interactable is WoodSite) {
                Debug.Log("Hacking Wood " + (Time.time - wood_time_start).ToString());
                SpawnProgressBar();
                if (wood_time_start < 0) {
                    wood_time_start = Time.time;
                }
                progressBar.SetFill((Time.time - wood_time_start) / wood_duration);
                if (Time.time - wood_time_start >= wood_duration) {
                    GetWood();
                    wood_time_start = -1f;
                    DestroyProgressBar();
                }
            }
            else if(holding_wood && interactable is Refinery) {
                Debug.Log("Processing Wood to Coal " + (Time.time - coal_time_start).ToString());
                SpawnProgressBar();
                if (coal_time_start < 0) {
                    coal_time_start = Time.time;
                }
                progressBar.SetFill((Time.time - coal_time_start) / coal_duration);
                if (Time.time - coal_time_start >= coal_duration) {
                    GetCoal();
                    coal_time_start = -1f;
                    DestroyProgressBar();
                }
            }
            else if(interactable is CartInteractable) {
                if (holding_wood) {
                    Debug.Log("Stoking fire!");
                    LoseWood();
                    fire.Stoke();
                }
                else if (holding_coal) {
                    Debug.Log("SuperStoking fire!");
                    LoseCoal();
                    fire.SuperStoke();
                }
            }
        }
        else {
            wood_time_start = -1f;
            coal_time_start = -1f;
            DestroyProgressBar();
        }
    }

    void SpawnProgressBar() {
        if (!progressBar) {
            progressBar = Instantiate(progressBarPrefab).GetComponent<ProgressBar>();
            progressBar.gameObject.GetComponent<CameraFollow>().follow = transform;
        }
    }

    void DestroyProgressBar() {
        if (progressBar) {
            Destroy(progressBar.gameObject);
            progressBar = null;
        }
    }

    public float DistanceToFire() {
        Vector3 delta = (transform.position - fire.transform.position);
        delta.y = 0;  // 2D Distance
        return delta.magnitude;
    }

    bool InFireRange() {
        return DistanceToFire() <= fire.CurrentRange;
    }

    void GetWood() {
        holding_wood = true;
        wooden_log.SetActive(true);
    }

    void LoseWood() {
        holding_wood = false;
        wooden_log.SetActive(false);
    }

    void GetCoal() {
        holding_wood = false;
        holding_coal = true;
        wooden_log.SetActive(false);
        coal_lumps.SetActive(true);
    }

    void LoseCoal() {
        holding_coal = false;
        coal_lumps.SetActive(false);
    }

    public void OnMove(InputAction.CallbackContext ctx){
        dir = ctx.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext ctx) {
        interact = ctx.ReadValue<float>() > 0 ? true : false;
    }

    public void Die() {
        dead = true;
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
