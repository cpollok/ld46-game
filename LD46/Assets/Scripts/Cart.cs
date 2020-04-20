using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cart : MonoBehaviour {

    public AudioSource rattleSound;

    private bool soundStarted = false;

    void Start() {
        if (!rattleSound)
            rattleSound = GetComponent<AudioSource>();
    }

    public void StartRattleSound() {
        if (soundStarted)
            return;

        //Debug.Log("Starting Cart rattle sound");
        LeanTween.value(gameObject, UpdateRattleVolume,
                        rattleSound.volume, 1.0f,
                        (1.0f - rattleSound.volume) * 0.2f).setEase(LeanTweenType.easeInOutCubic);
        soundStarted = true;
    }

    public void StopRattleSound() {
        if (!soundStarted)
            return;

        //Debug.Log("Stopping Cart rattle sound");
        LeanTween.value(gameObject, UpdateRattleVolume,
                        rattleSound.volume, 0.0f,
                        (rattleSound.volume) * 0.2f).setEase(LeanTweenType.easeInOutCubic);
        soundStarted = false;
    }

    public void UpdateRattleVolume(float val, float ratio) {
        if (val > 0.01f && !rattleSound.isPlaying)
            rattleSound.Play();
        else if (val <= 0.01f && rattleSound.isPlaying)
            rattleSound.Stop();
            
        rattleSound.volume = val;
    }
}
