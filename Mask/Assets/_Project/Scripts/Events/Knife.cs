using AudioSystem;
using DG.Tweening;
using System;
using UnityEngine;

public class Knife : BaseTimeEvent
{
    private bool isActive = false;
    private bool isPlayerNear = false;
    private Action currentCallback = null;
    [SerializeField] private float disappearTime = 145.0f;
    [SerializeField] private float deathDelay = 3.0f;

    [SerializeField]
    private SoundData _lightSwitchAudio;
    [SerializeField]
    private SoundData _footstepsAudio;

    private MeshRenderer meshRenderer;

    [SerializeField] private GameObject boxLid;
    [SerializeField] private float boxLidDuration = 1.5f;

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
            OnBoxOpen();
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
                SoundManager.Instance.CreateSound()
                .WithPosition(gameObject.transform.position)
                .Play(_lightSwitchAudio);
                SoundManager.Instance.CreateSound()
                .WithPosition(gameObject.transform.position)
                .Play(_footstepsAudio);
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
                SoundManager.Instance.CreateSound()
                .WithPosition(gameObject.transform.position)
                .Play(_lightSwitchAudio);
                PlayTriggerSound();
                LightManager.Instance.TurnOnLights();
                TimeManager.Instance.CancelScheduled(currentCallback);
                currentCallback = null;
            }
        }
    }

    public void OnKnifeDisappear()
    {
        isActive = true;
        meshRenderer.enabled = false;
    }

    public void OnBoxOpen()
    {
        if (boxLid)
        {
            boxLid.transform.DORotate(new Vector3(-90.0f, 0.0f, 0.0f), boxLidDuration)
                .OnComplete(() =>
                {
                    OnKnifeDisappear();
                });
        }
    }

    public void OnKnifeStab()
    {
        if (isPlayerNear)
        {
            // Stab Sound
            LightManager.Instance.TurnOnLights();
            DeathManager.Instance.Die("You were Stabbed...", "Knife");
        }
    }
}
