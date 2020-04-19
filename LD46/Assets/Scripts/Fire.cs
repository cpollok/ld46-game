using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

    public GameObject flame;
    public float power = 1.0f;
    public float max_power = 1.0f;
    public float famish_per_s = 0.1f;
    public float stoke_amount = 0.2f;
    public float super_stoke_amount = 0.5f;
    public float audioScaleFactor =10.0f;

    public Light lamp;
    public float max_light_range = 10.0f;
    public float CurrentRange { get { return power / max_power * max_light_range; } }
    private float lamp_normal_intesity = 1.0f;

    public Collider flame_collider;

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start() {
        if (lamp)
            lamp_normal_intesity = lamp.intensity;

        if (flame_collider == null)
            flame_collider = GetComponent<Collider>();

        if (!audioSource)
            audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        Famish(Time.deltaTime * famish_per_s);

        float u1 = (float)1.0 - Random.value; //uniform(0,1] random floats
        float u2 = (float)1.0 - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2); //random normal(0,1)
        float randNormal = 1.0f + 0.01f * randStdNormal; //random normal(mean,stdDev^2)
        
        float scale = power * randNormal;

        flame.transform.localScale = new Vector3(scale, scale, scale);

        lamp.range = CurrentRange;
        lamp.intensity = lamp_normal_intesity + 0.02f * randStdNormal;

        if (audioSource) {
            audioSource.volume = power / max_power;
            audioSource.maxDistance = CurrentRange * audioScaleFactor;
        }
    }

    public void Famish(float amount) {
        power = Mathf.Max(0, power - amount);
    }

    // Stoke with Wood
    public void Stoke() {
        power += stoke_amount;
        if (power > max_power) {
            power = max_power;
        }
    }

    public void SuperStoke() {
        power = Mathf.Min(power + super_stoke_amount, max_power);
    }
}
