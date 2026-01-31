using System.Collections.Generic;
using UnityEngine;
using UtilitySingletons;

public class LightManager : Singleton<LightManager>
{
    [SerializeField] private GameObject lightsOutCanvas;

    protected override void Awake()
    {
        base.Awake();
    }

    public void TurnOnLights()
    {
        if (lightsOutCanvas)
        {
            lightsOutCanvas.SetActive(false);
        }
    }

    public void TurnOffLights()
    {
        if (lightsOutCanvas)
        {
            lightsOutCanvas.SetActive(true);
        }
    }

}
