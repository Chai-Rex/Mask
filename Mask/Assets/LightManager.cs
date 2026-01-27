using System.Collections.Generic;
using UnityEngine;

public class LightManager : BaseTimeEvent
{
    public static LightManager Instance;

    [SerializeField] private List<Light> lights = new List<Light>();

    private bool isLightsOn = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        if (isLightsOn)
        {
            TurnOffLights();
        }
        else
        {
            TurnOnLights();
        }
    }

    private void TurnOnLights()
    {
        isLightsOn = true;
        foreach (Light light in lights)
        {
            light.enabled = true;
        }
    }

    private void TurnOffLights()
    {
        isLightsOn = false;
        foreach (Light light in lights)
        {
            light.enabled = false;
        }
    }

}
