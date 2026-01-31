using System;
using UnityEngine;

public class Knife : BaseTimeEvent
{
    private bool isActive = false;
    private bool isPlayerNear = false;
    private Action currentCallback = null;
    [SerializeField] private float disappearTime = 145.0f;
    [SerializeField] private float deathDelay = 3.0f; 

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        TimeManager.Instance.ScheduleAt(disappearTime, ActivateTimeEvent);
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        if (!isActive)
        {
            OnKnifeDisappear();
        }
        else
        {
            OnKnifeStab();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (isActive && isPlayerNear && currentCallback == null)
            {
                currentCallback = ActivateTimeEvent;
                LightManager.Instance.TurnOffLights();
                TimeManager.Instance.ScheduleAfter(deathDelay, currentCallback);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPlayerNear = false;

            if (isActive && currentCallback != null)
            {
                LightManager.Instance.TurnOnLights();
                TimeManager.Instance.CancelScheduled(currentCallback);
                currentCallback = null;
            }
        }
    }

    public void OnKnifeDisappear()
    {
        if (!isPlayerNear)
        {
            isActive = true;
            meshRenderer.enabled = false;
        }
    }

    public void OnKnifeStab()
    {
        if (isPlayerNear)
        {
            // Stab Sound
            LightManager.Instance.TurnOnLights();
            DeathManager.Instance.Die("You were Stabbed...");
        }
    }
}
