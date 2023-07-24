using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] List<Light> lights;

    List<float> lightsDefaultIntensity;
    float ambientDefaultIntensity;
    float ambientDefaultReflection;

    bool isNightVisionOn = false;
    bool isPlayerInDark = false;

    void Awake()
    {
        this.StoreDefaultIntensity();
    }

    void StoreDefaultIntensity()
    {
        this.lightsDefaultIntensity = new List<float>();

        foreach (Light light in lights)
        {
            this.lightsDefaultIntensity.Add(light.intensity);
        }

        this.ambientDefaultIntensity = RenderSettings.ambientIntensity;
        this.ambientDefaultReflection = RenderSettings.reflectionIntensity;
    }

    public void PlayerEnterDarkness()
    {
        isPlayerInDark = true;

        if(!isNightVisionOn)
        {
            LightsOff();
        }
    }

    public void PlayerLeavesDarkness()
    {
        isPlayerInDark = false;

        LightsOn();
    }

    public void ActivateNightVision()
    {
        isNightVisionOn = true;

        if(isPlayerInDark)
        {
            LightsOn();
        }
    }

    public void DeactivateNightVision()
    {
        isNightVisionOn = false;

        if (isPlayerInDark)
        {
            LightsOff();
        }
    }

    void LightsOff()
    {
        foreach (Light light in lights)
        {
            light.intensity = 0;
        }
        RenderSettings.ambientIntensity = 0;
        RenderSettings.reflectionIntensity = 0;
    }

    void LightsOn()
    {
        int i = 0;
        foreach (Light light in lights)
        {
            light.intensity = lightsDefaultIntensity[i];
            i++;
        }
        RenderSettings.ambientIntensity = this.ambientDefaultIntensity;
        RenderSettings.reflectionIntensity = this.ambientDefaultReflection;
    }
}
