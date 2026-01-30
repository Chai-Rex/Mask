using UnityEngine;

public class ElectricCable : BaseTimeEvent
{
    [SerializeField] private WaterSpill waterSpill;
    private StateVariable isActive = new StateVariable("isElectricCableActive", false);

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        SetIsElectricCableActive(true);
    }

    public void SetIsElectricCableActive(bool _isActive)
    {
        isActive.SetValueAndUpdateBlackboard(_isActive);

        if (isActive.Value)
        {
            gameObject.SetActive(true);
            if (waterSpill != null)
            {
                waterSpill.SetIsCableTouching(true);
            }
        }
        else
        {
            gameObject.SetActive(false);
            if (waterSpill != null)
            {
                waterSpill.SetIsCableTouching(false);
            }
        }
    }
}
