using System.Collections;
using UnityEngine;

public class BathHeat : BaseTimeEvent
{
    private StateVariable isPlayerIn = new StateVariable("isPlayerInBath", false);
    private StateVariable isActive = new StateVariable("isBathActive", false);

    [SerializeField] private Door doorToLock;
    [SerializeField] private float heatDuration = 5.0f;

    [SerializeField] private float doorLockTime = 120.0f;

    private bool isPlayerDead = false;
    private Coroutine bathCoroutine;

    private void Start()
    {
        TimeManager.Instance.ScheduleAt(doorLockTime, ActivateTimeEvent);
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        SetBathHeatIsActive(true);
    }

    IEnumerator BathHeating()
    {
        float elapsedHeat = 0.0f;

        while (elapsedHeat < heatDuration)
        {
            elapsedHeat += Time.deltaTime;

            yield return null;
        }

        isPlayerDead = true;
        bathCoroutine = null;

        DeathManager.Instance.Die("Bath Heat...", "Bath");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPlayerIn.Value = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (bathCoroutine == null && !isPlayerDead)
            {
                TurnOnBathHeat();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPlayerIn.Value = false;
        }
    }

    public void TurnOnBathHeat()
    {
        if (isActive.Value && isPlayerIn.Value)
        {
            doorToLock.SetIsDoorLocked(true);
            PlayTriggerSound();
            bathCoroutine = StartCoroutine(BathHeating());

        }
    }

    public void SetBathHeatIsActive(bool _isActive)
    {
        isActive.SetValueAndUpdateBlackboard(_isActive);
    }

}
