using UnityEngine;

public class Lego : BaseTimeEvent
{
    private StateVariable isLegoActive = new StateVariable("isLegoActive", false);

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        SetLegoIsActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isLegoActive.Value) { return; }

        if (collision.gameObject.tag == "Player")
        {
            PlayTriggerSound();
            // Player Death
        }
    }

    public void SetLegoIsActive(bool _isActive)
    {
        isLegoActive.SetValueAndUpdateBlackboard(_isActive);

        if (!isLegoActive.Value)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
