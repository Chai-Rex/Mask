using UnityEngine;

public class Lego : BaseTimeEvent
{
    private StateVariable isLegoActive = new StateVariable("isLegoActive", false);
    [SerializeField] private float legoActivateTime = 120.0f;

    private void Start()
    {
        TimeManager.Instance.ScheduleAt(legoActivateTime, ActivateTimeEvent);
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        SetLegoIsActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isLegoActive.Value) { return; }

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayTriggerSound();
            DeathManager.Instance.Die("Stepped On Baby Building Block");
        }
    }

    public void SetLegoIsActive(bool _isActive)
    {
        isLegoActive.SetValueAndUpdateBlackboard(_isActive);

        if (isLegoActive.Value)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
