﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : FireInteractor<Fire>
{
    private Rail rail;
    private PathCreation.VertexPath path;
    private Cart cart;
    private PlayerCharacterController character;

    float cart_progress = 0f;
    public float cart_velocity = 1.0f;
    bool ended = false;

    private Queue<WorkMachine> workMachines;

    public AudioSource lightAmbience;
    public AudioSource darknessAmbiance;

    [SerializeField] private IMachine activeMachine;

    void Start()
    {
        GameObject find_obj = GameObject.Find("/Rail");
        rail = find_obj.GetComponent<Rail>();
        path = rail.path_creator.path;

        find_obj = GameObject.Find("/Cart");
        cart = find_obj.GetComponent<Cart>();

        find_obj = GameObject.Find("/Player_Character");
        character = find_obj.GetComponent<PlayerCharacterController>();

        SetupMachines();
    }

    void Update()
    {
        if (ended) { return; }


        if (!FireStillAlive()) {
            OnFireWentOut();
        }
        else if (CharacterDead()) {
            OnCharacterDies();
        }
        else if (CartReachedEnd()) {
            WinLevel();
        }
        else if (ReachedWorkMachine()) {
            OnReachWorkMachine();
        }
        else if (activeMachine != null){
            if (activeMachine.Finished()) {
                activeMachine = null;
            }
        }
        else {
            MoveCart();
        }

        float darkFraction = Mathf.Clamp01(character.DistanceToFire() / Mathf.Max(fire.CurrentRange, 1e-5f)); // Avoid NaN
        float lightVol = Mathf.Cos(darkFraction * Mathf.PI) * 0.5f + 0.5f;

        if (lightAmbience)
            lightAmbience.volume = lightVol;

        if (darknessAmbiance)
            darknessAmbiance.volume = 1.0f - lightVol;
    }

    void SetupMachines() {
        WorkMachine[] workMachinesArray = GameObject.FindObjectsOfType<WorkMachine>();
        foreach (WorkMachine m in workMachinesArray) {
            m.PutOnRail(rail);
        }
        Array.Sort(workMachinesArray, WorkMachine.CompareByPositionOnRail);
        workMachines = new Queue<WorkMachine>(workMachinesArray);
    }

    void MoveCart() {
        cart_progress += cart_velocity * Time.deltaTime;
        Vector3 position = path.GetPointAtDistance(cart_progress);
        Vector3 direction = path.GetDirectionAtDistance(cart_progress);
        
        cart.transform.SetPositionAndRotation(position, Quaternion.LookRotation(direction, Vector3.up));
        cart.StartRattleSound();
    }

    bool FireStillAlive() {
        return fire.power > 0;
    }

    bool CharacterDead() {
        if (!character) {
            character = GameObject.Find("/Player_Character").GetComponent<PlayerCharacterController>();
        }
        return character.Dead;
    }

    bool CartReachedEnd() {
        return cart_progress >= path.length*0.99;
    }

    bool ReachedWorkMachine() {
        if(workMachines.Count > 0) {
            return cart_progress >= workMachines.Peek().GetPositionOnRail();
        }
        return false;
    }

    void OnFireWentOut() {
        Debug.Log("The fire, it is off.");
        GameOver();
    }

    void OnCharacterDies() {
        Debug.Log("You stupid?");
        GameOver();
    }

    void OnReachWorkMachine() {
        WorkMachine m = workMachines.Dequeue();
        activeMachine = m;
        m.StartWork();
        cart.StopRattleSound();
    }

    void GameOver() {
        // Gameover stuff
        ended = true;
    }

    void WinLevel() {
        // Win the level
        Debug.Log("Winner winner, nothing blub.");
        ended = true;
    }
}
