using DG.Tweening;
using System;
using UnityEngine;

public class LaundryShoot : BaseTimeEvent, IInteractable
{
    private StateVariable isActive = new StateVariable("isLaundryShootActive", false);
    [SerializeField] private string verb = "Close";
    [SerializeField] private string verbWhenOpen = "Open";

    private Vector3 startLocation;
    private Vector3 endLocation;

    private StateVariable isOpen = new StateVariable("isLaundryShootOpen", false);

    [SerializeField] private float shootDuration = 3.0f;
    [SerializeField] private float deathDelay = 5.0f;
    private Action currentCallback;

    public string InteractionVerb => isOpen.Value ? verbWhenOpen : verb;

    private void Awake()
    {
        startLocation = transform.position;
        endLocation = transform.position + new Vector3(0.0f, 200.0f, 0.0f);
    }

    public void OnInteract(GameObject interactor)
    {
        OnLaudryShootOpen();
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        if (isActive.Value)
        {
            PlayTriggerSound();
            // Player Death
        }
    }

    void OnLaudryShootOpen()
    {
        if (isOpen.Value)
        {
            transform.DOMove(startLocation, shootDuration)
                .OnComplete(() =>
                {
                    isOpen.SetValueAndUpdateBlackboard(false);
                    isActive.SetValueAndUpdateBlackboard(false);

                    if (currentCallback == null)
                    {
                        currentCallback = ActivateTimeEvent;
                        TimeManager.Instance.ScheduleAfter(deathDelay, currentCallback);
                    }
                    
                });
        }
        else
        {
            transform.DOMove(endLocation, shootDuration)
                .OnComplete(() =>
                {
                    isOpen.SetValueAndUpdateBlackboard(true);
                    isActive.SetValueAndUpdateBlackboard(true);

                    if (currentCallback != null)
                    {
                        TimeManager.Instance.CancelScheduled(currentCallback);
                    }
                });
        }
    }
}
