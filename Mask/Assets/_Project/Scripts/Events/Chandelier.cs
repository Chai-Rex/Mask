using DG.Tweening;
using UnityEngine;

public class Chandelier : BaseTimeEvent
{
    [SerializeField] private Transform targetLocation;
    [SerializeField] private float fallDuration = 0.5f;

    private StateVariable canFall = new StateVariable("canChandelierFall", false);
    private StateVariable hasFallen = new StateVariable("hasChandelierFallen", false);

    private void Start()
    {
        TimeManager.Instance.ScheduleAt(5.0f, ActivateTimeEvent);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (hasFallen.Value) { return; }

        if (collision.gameObject.tag == "Player")
        {
            // Player Death
            DeathManager.Instance.Die("Chandelier Bonked you on the head");
        }
    }
}
