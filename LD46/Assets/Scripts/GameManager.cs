using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : FireInteractor<Fire>
{
    private Rail rail;
    [SerializeField] private PlayerCharacterController character;
    float progress = 0f;

    void Start()
    {
        rail = GameObject.Find("Rail").GetComponent<Rail>();
        character = GameObject.Find("Player_Character").GetComponent<PlayerCharacterController>();
    }

    void Update()
    {
        if (!FireStillAlive()) {
            OnFireWentOut();
        }
        if (CharacterDead()) {
            OnCharacterDies();
        }
        if (CartReachedEnd()) {
            WinLevel();
        }
    }

    bool FireStillAlive() {
        return fire.power > 0;
    }

    bool CharacterDead() {
        return character.Dead;
    }

    bool CartReachedEnd() {
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

    void GameOver() {
        // Gameover stuff
    }

    void WinLevel() {
        // Win the level
        Debug.Log("Winner winner, nothing blub.");
    }
}
