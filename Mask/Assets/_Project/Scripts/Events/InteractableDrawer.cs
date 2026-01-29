using DG.Tweening;
using System;
using UnityEngine;

public class InteractableDrawer : BaseTimeEvent, IInteractable
{
    [SerializeField] private string verb = "Close";
    [SerializeField] private string verbWhenOpen = "Open";
    [SerializeField] private bool isDeathDrawer = false;
    private bool isActive = false;
    private bool isOpen = false;

    private Vector3 startLocation;
    private Vector3 endLocation;

    [SerializeField] private float drawerDuration = 3.0f;
    [SerializeField] private float deathDelay = 3.0f;
    private Action currentCallback;

    public string InteractionVerb => isOpen ? verbWhenOpen : verb;

    private void Awake()
    {
        startLocation = transform.position;
        endLocation = transform.position + new Vector3(0.0f, 0.0f, 100.0f);
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        if (isActive && isDeathDrawer)
        {
            // Player Death
        }
    }

    public void OnInteract(GameObject interactor)
    {
        OnDrawerOpen();
    }

    private void OnDrawerOpen()
    {
        if (isOpen)
        {
            transform.DOMove(startLocation, drawerDuration)
                .OnComplete(() =>
                {
                    isOpen = false;
                    isActive = false;

                    if (currentCallback == null && isDeathDrawer)
                    {
                        currentCallback = ActivateTimeEvent;
                        TimeManager.Instance.ScheduleAfter(deathDelay, currentCallback);
                    }
                });
        }
        else
        {
            transform.DOMove(endLocation, drawerDuration)
                .OnComplete(() =>
                {
                    isOpen = true;
                    isActive = true;

                    if (currentCallback != null && isDeathDrawer)
                    {
                        TimeManager.Instance.CancelScheduled(currentCallback);
                    }
                });
        }
    }
}
