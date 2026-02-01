using AudioSystem;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class InteractableDrawer : BaseTimeEvent, IInteractable
{
    [SerializeField] private string verb = "Open";
    [SerializeField] private string verbWhenOpen = "Close";
    [SerializeField] private bool isDeathDrawer = false;

    [SerializeField]
    private SoundData _openDrawerAudio;

    private bool isActive = false;
    private bool isOpen = false;

    private StateVariable _deathDrawerActive = new StateVariable("", false, false);

    private Vector3 startLocation;

    [SerializeField] private float drawerDuration = 0.25f;
    [SerializeField] private float deathDelay = 1.0f;
    [SerializeField] private float timeToActivate = 120.0f;

    [SerializeField] private Transform endLocation;
    [SerializeField] private GameObject revolver;

    private Coroutine currentBlastCoroutine;

    public string InteractionVerb => isOpen ? verbWhenOpen : verb;

    private void Awake()
    {
        startLocation = transform.position;

        if (isDeathDrawer)
        {
            _deathDrawerActive = new StateVariable("isDeathDrawerActive", false);
        }
    }

    private void Start()
    {
        TimeManager.Instance.ScheduleAt(timeToActivate, ActivateTimeEvent);
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();
        isActive = true;    
    }

    private IEnumerator OnGettingBlasted()
    {
        float currentDelay = 0.0f;

        while (currentDelay < deathDelay)
        {
            if (!isOpen)
            {
                break;
            }

            currentDelay += Time.deltaTime;
            yield return null;
        }

        if (isOpen && isActive && isDeathDrawer)
        {
            PlayTriggerSound();
            DeathManager.Instance.Die("Got Blasted", "Drawer");
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

                    if (isDeathDrawer)
                    {
                        _deathDrawerActive.SetValueAndUpdateBlackboard(false);

                        if (currentBlastCoroutine != null)
                        {
                            StopAllCoroutines();
                            currentBlastCoroutine = null;
                        }
                    }

                });
        }
        else
        {
            if (endLocation == null) { return; }

            transform.DOMove(endLocation.position, drawerDuration)
                .OnComplete(() =>
                {
                    isOpen = true;

                    if (isDeathDrawer && isActive)
                    {
                        _deathDrawerActive.SetValueAndUpdateBlackboard(true);

                        if (currentBlastCoroutine == null)
                        {
                            currentBlastCoroutine = StartCoroutine(OnGettingBlasted());
                        }
                    }
                });
        }
    }
}
