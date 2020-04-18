using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : FireInteractor<Fire>
{
    private Rail rail;
    private PathCreation.VertexPath path;
    private Transform cart;
    private PlayerCharacterController character;

    float cart_progress = 0f;
    public float cart_velocity = 1.0f;
    bool ended = false;

    void Start()
    {
        GameObject find_obj = GameObject.Find("/Rail");
        rail = find_obj.GetComponent<Rail>();
        path = rail.path_creator.path;

        find_obj = GameObject.Find("/Cart");
        cart = find_obj.transform;

        find_obj = GameObject.Find("/Player_Character");
        character = find_obj.GetComponent<PlayerCharacterController>();
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
        else {
            MoveCart();
        }
    }

    void MoveCart() {
        cart_progress += cart_velocity * Time.deltaTime;
        Vector3 position = path.GetPointAtDistance(cart_progress);
        Vector3 direction = path.GetDirectionAtDistance(cart_progress);
        
        cart.SetPositionAndRotation(position, Quaternion.LookRotation(direction, Vector3.up));
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
        return cart_progress >= path.length;
    }

    void OnFireWentOut() {
        Debug.Log("The fire, it is off.");
        GameOver();
    }

    void OnCharacterDies() {
        Debug.Log("You stupid?");
        GameOver();
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
