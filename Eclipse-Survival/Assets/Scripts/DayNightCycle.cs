using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    public Light2D[] outdoorLights;
    bool lightIncresaing = false;
    const float lightIncrement = 0.0001f;
    const float maxBright = 0.6f;
    const float minBright = 0.01f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float intensity = outdoorLights[0].intensity;
        if (lightIncresaing)
        {
            intensity += lightIncrement;
        }
        else if (!lightIncresaing)
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
        
        if (lightIncresaing && intensity >= maxBright)
        {
            lightIncresaing = false;
        }

        if (!lightIncresaing && intensity <= minBright)
        {
            lightIncresaing = true;
        }

    }
}
