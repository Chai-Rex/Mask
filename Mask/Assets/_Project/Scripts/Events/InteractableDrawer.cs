using AudioSystem;
using DG.Tweening;
using System;
using UnityEngine;

public class InteractableDrawer : BaseTimeEvent, IInteractable
{
    [SerializeField] private string verb = "Close";
    [SerializeField] private string verbWhenOpen = "Open";
    [SerializeField] private bool isDeathDrawer = false;

    [SerializeField]
    private SoundData _openDrawerAudio;

    private bool isActive = false;
    private bool isOpen = false;

    private StateVariable _deathDrawerActive = new StateVariable("", false, false);

    private Vector3 startLocation;
    private Vector3 endLocation;

    [SerializeField] private float drawerDuration = 3.0f;
    [SerializeField] private float deathDelay = 0.5f;
    private Action currentCallback;

    public string InteractionVerb => isOpen ? verbWhenOpen : verb;

    private void Awake()
    {
        startLocation = transform.position;
        endLocation = transform.position + new Vector3(0.0f, 0.0f, 100.0f);

        if (isDeathDrawer)
        {
            _deathDrawerActive = new StateVariable("isDeathDrawerActive", false);
        }
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        if (isActive && isDeathDrawer)
        {
            PlayTriggerSound();
            DeathManager.Instance.Die("Got Blasted");
        }
    }

    public void OnInteract(GameObject interactor)
    {
        OnDrawerOpen();
    }

    private void OnDrawerOpen()
    {
        if (_openDrawerAudio != null && _openDrawerAudio.Clip != null)
        {
            SoundManager.Instance.CreateSound()
                .WithPosition(gameObject.transform.position)
                .Play(_openDrawerAudio);
        }

        if (isOpen)
        {
            transform.DOMove(startLocation, drawerDuration)
                .OnComplete(() =>
                {
                    isOpen = false;
                    isActive = false;

                    if (currentCallback != null && isDeathDrawer)
                    {
                        _deathDrawerActive.SetValueAndUpdateBlackboard(false);
                        TimeManager.Instance.CancelScheduled(currentCallback);
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

                    if (currentCallback == null && isDeathDrawer)
                    {
                        _deathDrawerActive.SetValueAndUpdateBlackboard(true);
                        currentCallback = ActivateTimeEvent;
                        TimeManager.Instance.ScheduleAfter(deathDelay, currentCallback);
                    }
                });
        }
    }
}
