using UnityEngine;

public class WaterSpill : BaseTimeEvent
{
    private bool isActive = false;
    private bool isCableTouching = false;

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
        if (!isActive) { return; }
        if (!isCableTouching) { return; }

        if (other.gameObject.tag == "Player")
        {
            // Player Dies
        }
    }

    public void WaterSpillActive(bool _isActive)
    {
        isActive = _isActive;
        if (isActive)
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
        return isActive;
    }

    public void SetIsCableTouching(bool _isCableTouching)
    {
        isCableTouching = _isCableTouching;
    }
}
