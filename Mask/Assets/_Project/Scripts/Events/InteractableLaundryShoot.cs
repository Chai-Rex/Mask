using DG.Tweening;
using System;
using UnityEngine;

public class LaundryShoot : BaseTimeEvent, IInteractable
{
    private bool isActive = false;
    [SerializeField] private string verb = "Close";
    [SerializeField] private string verbWhenOpen = "Open";

    private Vector3 startLocation;
    private Vector3 endLocation;

    private bool isOpen = false;

    [SerializeField] private float shootDuration = 3.0f;
    [SerializeField] private float deathDelay = 5.0f;
    private Action currentCallback;

    public string InteractionVerb => isOpen ? verbWhenOpen : verb;

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

        if (isActive)
        {
            // Player Death
        }
    }

    void OnLaudryShootOpen()
    {
        if (isOpen)
        {
            transform.DOMove(startLocation, shootDuration)
                .OnComplete(() =>
                {
                    isOpen = false;
                    isActive = false;

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
                    isOpen = true;
                    isActive = true;

                    if (currentCallback != null)
                    {
                        TimeManager.Instance.CancelScheduled(currentCallback);
                    }
                });
        }
    }
}
