using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    public bool IsDaytime {get; set;}

    [Header("Set in Inspector")]
    public Light2D[] outdoorLights;
    public float startingBrightness;
    public bool startWithLightIncreasing;

    [Header("Set Dynamically")]   
    public float timeRemainingInRound;
    public float lightIncrement;
    public float intensity;
    public float roundsElapsed = 0;
    private bool lightIncreasing;

    // Constants
    const float MAX_LIGHT_INTENSITY = 0.6f;
    const float MIN_LIGHT_INTENSITY = 0.05f;
    const float LIGHT_DECREASE_DURATION = 450f; // in seconds
    const float LIGHT_INCREASE_DURATION = 450f; // in seconds
    const float DAY_NIGHT_CUTOFF = 0.3f; // light intensity at which IsDaytime is toggled

    float timer1 = 1;
    float timer1TimeRemaining;

    // Start is called before the first frame update
    void Start()
    {
        InitializeRound();
    }

    // Update is called once per frame
    void Update()
    {
        timeRemainingInRound -= Time.deltaTime;
        timer1TimeRemaining -= Time.deltaTime;

        if (timer1TimeRemaining <= 0f) // Do this code at determined increment (timer1) to conserve GPU
        {
            intensity = outdoorLights[0].intensity;
            if (lightIncreasing)
            {
                intensity += lightIncrement;
            }
            else if (!lightIncreasing)
            {
                intensity -= lightIncrement;
            }

            if (intensity < 0)
            {
                intensity = 0;
            }

            foreach (var light in outdoorLights)
            {
                light.intensity = intensity;
            }

            if (lightIncreasing && intensity >= MAX_LIGHT_INTENSITY)
            {
                lightIncreasing = false;
                lightIncrement = (MAX_LIGHT_INTENSITY - startingBrightness) / LIGHT_INCREASE_DURATION;
            }

            if (!lightIncreasing && intensity <= MIN_LIGHT_INTENSITY)
            {
                lightIncreasing = true;
                lightIncrement = (startingBrightness - MIN_LIGHT_INTENSITY) / LIGHT_DECREASE_DURATION;
            }

            // Toggle Day/Night
            if ((IsDaytime && intensity < DAY_NIGHT_CUTOFF) || (!IsDaytime && intensity > DAY_NIGHT_CUTOFF))
            {
                IsDaytime = !IsDaytime;
            }

            // Reset Timer
            timer1TimeRemaining = timer1;
        }
        
        // Reset for next round
        if (timeRemainingInRound <= 0)
        {
            roundsElapsed++;
            InitializeRound();
        }
    }

    private void InitializeRound()
    {
        lightIncreasing = startWithLightIncreasing;
        timeRemainingInRound = LIGHT_DECREASE_DURATION + LIGHT_INCREASE_DURATION;
        intensity = startingBrightness;
        foreach (var light in outdoorLights)
        {
            light.intensity = intensity;
        }

        if (lightIncreasing)
        {
            lightIncrement = (MAX_LIGHT_INTENSITY - startingBrightness) / LIGHT_INCREASE_DURATION;
        }
        else
        {
            lightIncrement = (startingBrightness - MIN_LIGHT_INTENSITY) / LIGHT_DECREASE_DURATION;
        }
    }
}
