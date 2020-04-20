using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : FireInteractor<Fire> {
    public TextMeshProUGUI text;

    public GameManager gm;
    public Rail rail;
    private PathCreation.VertexPath path;
    public Cart cart;

    public LightBarrier firstLightBarrier;
    public WorkMachine firstMachine;
    public Transform firstWind;
    public LightBarrier secondLightBarrier;
    public Transform secondWind;
    public Transform onYourOwn;

    [SerializeField] private int step = 0;

    private void Start() {
        path = rail.path_creator.path;
        StartCoroutine(PlayIntroSequence());
    }

    private void Update() {
        Debug.Log("Step: " + step.ToString());
        switch (step) {
            case 1:
                StartCoroutine(WaitForStoke());
                break;
            case 2:
                if (NearCheckpoint(firstLightBarrier, 1f)) {
                    StartCoroutine(PlayLightBarrierSequence());
                }
                break;
            case 3:
                if(NearCheckpoint(firstWind, 6f)) {
                    StartCoroutine(PlayFirstWindSequence());
                }
                break;
            case 4:
                if (NearCheckpoint(firstMachine, 3f)) {
                    StartCoroutine(PlayMachineSequence());
                }
                break;
            case 5:
                if(NearCheckpoint(secondLightBarrier, 3f)) {
                    StartCoroutine(PlaySecondLightBarrierSequence());
                }
                break;
            case 6:
                if(NearCheckpoint(secondWind, 4.5f)) {
                    StartCoroutine(PlaySecondWindSequence());
                }
                break;
            case 7:
                if(PastCheckpoint(onYourOwn)) {
                    StartCoroutine(PlayOnYourOwnSequence());
                }
                break;
            default:
                break;
        }
    }

    private bool NearCheckpoint(Transform otherTF, float threshold) {
        float otherDistOnPath = path.GetClosestDistanceAlongPath(otherTF.position);
        return otherDistOnPath - gm.cart_progress <= threshold;
    }

    private bool PastCheckpoint(Transform otherTF) {
        float otherDistOnPath = path.GetClosestDistanceAlongPath(otherTF.position);
        return otherDistOnPath - gm.cart_progress <= 0;
    }

    private bool NearCheckpoint(IMachine machine, float threshold) {
        Debug.Log("Called near checkpoint " + (machine.GetPositionOnRail() - gm.cart_progress).ToString());
        return machine.GetPositionOnRail() - gm.cart_progress <= threshold;
    }

    private bool PastCheckpoint(IMachine machine) {
        return machine.GetPositionOnRail() - gm.cart_progress <= 0;
    }

    private IEnumerator PlayIntroSequence() {
        text.text = "Welcome to these barren lands. \n" +
            "You have to keep the flame alive or be forever lost to the darkness.";
        yield return new WaitForSeconds(5);
        text.text = "Collect branches from the dead trees and feed them to the fire.";
        yield return new WaitForSeconds(4);
        text.text = "";
        step++;
    }

    private IEnumerator WaitForStoke() {
        step++;
        float start_power = fire.power;
        while (fire.power <= start_power) {
            yield return null;
        }
        fire.famish_per_s = 0.03f;
        cart.StartRattleSound();
        gm.cart_velocity = 1f;
    }

    private IEnumerator PlayLightBarrierSequence() {
        step++;
        text.text = "This is a Light Barrier. It will only open if the flame burns bright enough.\n" +
            "So feed it!";
        yield return new WaitForSeconds(4);
        text.text = "";
    }

    private IEnumerator PlayFirstWindSequence() {
        step++;
        text.text = "Watch out for wind currents. They will shorten the life of your flame!\n" +
            "Turn the wind protector by pushing the handle at the cart to shield the flame.";
        while (!PastCheckpoint(firstWind)) {
            yield return null;
        }
        yield return new WaitForSeconds(1);
        if(fire.power > 0) {
            text.text = "Great work! But there will be stronger currents than this.";
            yield return new WaitForSeconds(4);
        }
        text.text = "";
    }

    private IEnumerator PlayMachineSequence() {
        step++;
        text.text = "There is a machine coming up. These constructs used to power these lands.\n" +
            "But nowadays they lie desolated.\n" +
            "Watch the flame give it new life!";
        yield return new WaitForSeconds(5);
        text.text = "";
    }

    private IEnumerator PlaySecondLightBarrierSequence() {
        step++;
        text.text = "Another light barrier...\n" +
            "Take this opportunity to use the refinery next to it to turn your collected branches into something more rich in fuel.";
        while (fire.power <= secondLightBarrier.power_req) {
            yield return null;
        }
        text.text = "";
    }

    private IEnumerator PlaySecondWindSequence() {
        step++;
        text.text = "A strong wind current lies ahead. Be careful to shield the flame!";
        while (!PastCheckpoint(secondWind)) {
            yield return null;
        }
        text.text = "";
        yield return new WaitForSeconds(3);
        if(fire.power > 0f) {
            text.text = "That was dangerous. But you are almost through this part of the land.";
            yield return new WaitForSeconds(3);
        }
        text.text = "";
    }

    private IEnumerator PlayOnYourOwnSequence() {
        step++;
        text.text = "I see you adapted quickly to your new existence.\n" +
            "From now on you are on your own. Keep the flame alive!";
        yield return new WaitForSeconds(5);
        text.text = "";
    }
}
