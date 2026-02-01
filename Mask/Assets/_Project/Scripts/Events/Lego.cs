using System;
using UnityEngine;

public class Lego : BaseTimeEvent
{
    private StateVariable isLegoActive = new StateVariable("isLegoActive", false);
    [SerializeField] private float legoActivateTime = 120.0f;

    private void Start()
    {
        DeathManager.Instance.HasSlept += SetLegoIsActive;
        // TimeManager.Instance.ScheduleAt(legoActivateTime, ActivateTimeEvent);
        gameObject.SetActive(false);
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        //SetLegoIsActive();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isLegoActive.Value) { return; }

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayTriggerSound();
            DeathManager.Instance.Die("Stepped On Baby Building Block", "Block");
        }
    }

    public void SetLegoIsActive()
    {
        isLegoActive.SetValueAndUpdateBlackboard(true);

        gameObject.SetActive(true);

        DeathManager.Instance.HasSlept -= SetLegoIsActive;
    }

    private void OnDestroy()
    {
        DeathManager.Instance.HasSlept -= SetLegoIsActive;
    }
}
