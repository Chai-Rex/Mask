using DG.Tweening;
using UnityEngine;

public class Chandelier : BaseTimeEvent
{
    [SerializeField] private Transform targetLocation;
    [SerializeField] private float fallDuration = 0.5f;

    private StateVariable canFall = new StateVariable("canChandelierFall", false);
    private StateVariable hasFallen = new StateVariable("hasChandelierFallen", false);

    [SerializeField] private float chandelierFallTime = 60.0f;

    private void Start()
    {
        TimeManager.Instance.ScheduleAt(chandelierFallTime, ActivateTimeEvent);
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        if (canFall.Value && !hasFallen.Value)
        {
            ChandelierFall();
        }
    }

    private void ChandelierFall()
    {
        PlayTriggerSound();
        transform.DOMove(targetLocation.position, fallDuration)
            .OnComplete(() =>
            {
                hasFallen.SetValueAndUpdateBlackboard(true);
            });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasFallen.Value) { return; }

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Player Death
            DeathManager.Instance.Die("Chandelier Bonked you on the head");
        }
    }
}
