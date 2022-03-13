using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

using static Constants;

public class SceneDetail : MonoBehaviour
{
    public Light2D[] outdoorLights;

    private void Start()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("TimeAndLightController");
        foreach (var light in outdoorLights)
        {
            light.intensity = controller.GetComponent<DayNightCycle>().intensity;
        }
        controller.GetComponent<DayNightCycle>().outdoorLights = outdoorLights;        
    }
}
