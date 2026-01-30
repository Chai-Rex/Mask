using UnityEngine;

public class WaterSpill : BaseTimeEvent
{
    private StateVariable isActive = new StateVariable("isWaterSpillActive", false);
    private StateVariable isCableTouching = new StateVariable("isCableTouchingSpill", false);

    private void Start()
    {
        WaterSpillActive(false);
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        WaterSpillActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isActive.Value) { return; }
        if (!isCableTouching.Value) { return; }

        if (other.gameObject.tag == "Player")
        {
            PlayTriggerSound();
            // Player Dies

        }
    }

    public void WaterSpillActive(bool _isActive)
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
