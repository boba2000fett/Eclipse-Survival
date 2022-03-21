using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

using static Constants;

public class DayNightCycle : MonoBehaviour
{
    static public DayNightCycle DNC;
    public bool IsDaytime {get; set;}
    public int Hour { get; set; }
    public bool NeedToUpdateClockUI { get; set; }

    [Header("Set in Inspector")]
    public Light2D[] outdoorLights;
    public float startingBrightness;
    public bool startWithLightIncreasing;

    [Header("Set Dynamically")]   
    public float timeRemainingInRound;
    public float timeRemainingInHour;
    public float lightIncrement;
    public float intensity = 0.5f;
    public float roundsElapsed = 0;
    private bool lightIncreasing;
    private float inGameHour;

    float timer1 = 1;
    float timer1TimeRemaining;

    // Start is called before the first frame update
    void Start()
    {
        if (DNC == null)
        {
            DNC = this;           
        }
        else if (DNC != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        InitializeRound();
    }

    // Update is called once per frame
    void Update()
    {
        timeRemainingInRound -= Time.deltaTime;
        timeRemainingInHour -= Time.deltaTime;
        timer1TimeRemaining -= Time.deltaTime;

        if (timer1TimeRemaining <= 0f) // Do this code at determined increment (timer1) to conserve GPU
        {
            // Michael's Note: Added Case for No Outdoor Lights
            if(outdoorLights.Length != 0) intensity = outdoorLights[0].intensity;
           
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

            // Check for end of the hour
            if (timeRemainingInHour <= 0)
            {
                timeRemainingInHour = inGameHour;
                Hour++;
                NeedToUpdateClockUI = true;
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
        Hour = 0;

        lightIncreasing = startWithLightIncreasing;
        timeRemainingInRound = LIGHT_DECREASE_DURATION + LIGHT_INCREASE_DURATION;
        inGameHour = timeRemainingInRound / 24;
        timeRemainingInHour = inGameHour;

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

        GamePlayManager.GPM.NewCycle((int)roundsElapsed);
    }
}
