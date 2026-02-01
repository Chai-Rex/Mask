using UnityEngine;

public class WaterSpill : BaseTimeEvent
{
    private StateVariable isActive = new StateVariable("isWaterSpillActive", false);
    private StateVariable isCableTouching = new StateVariable("isCableTouchingSpill", false);

    [SerializeField] private float waterSpillTime = 120.0f;

    private void Start()
    {
        SetWaterSpillActive(false);
        TimeManager.Instance.ScheduleAt(waterSpillTime, ActivateTimeEvent);
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        SetWaterSpillActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive.Value) { return; }
        if (!isCableTouching.Value) { return; }


        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayTriggerSound();
            DeathManager.Instance.Die("Was Electrocuted", "Spill");
        }
    }

    public void SetWaterSpillActive(bool _isActive)
    {
        isActive.SetValueAndUpdateBlackboard(_isActive);

        if (isActive.Value)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }       
    }

    public bool GetWaterSpillActive()
    {
        return isActive.Value;
    }

    public void SetIsCableTouching(bool _isCableTouching)
    {
        isCableTouching.SetValueAndUpdateBlackboard(_isCableTouching);
    }
}
