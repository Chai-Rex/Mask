using UnityEngine;

public class ElectricCable : BaseTimeEvent
{
    [SerializeField] private WaterSpill waterSpill;
    private bool isActive = false;

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        SetIsElectricCableActive(true);
    }

    public void SetIsElectricCableActive(bool _isActive)
    {
        isActive = _isActive;

        if (isActive)
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
