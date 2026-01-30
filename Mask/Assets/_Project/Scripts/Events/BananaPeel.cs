using UnityEngine;

public class BananaPeel : BaseTimeEvent
{
    private StateVariable isActive = new StateVariable("isBananaPeelActive", false);

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        SetBananaPeelIsActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isActive.Value) { return; }

        if (collision.gameObject.tag == "Player")
        {
            PlayTriggerSound();
            // Player Death
        }
    }

    public void SetBananaPeelIsActive(bool _isActive)
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
}
