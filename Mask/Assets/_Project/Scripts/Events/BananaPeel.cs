using UnityEngine;

public class BananaPeel : BaseTimeEvent
{
    private bool isActive = false;

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        SetBananaPeelIsActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isActive) { return; }

        if (collision.gameObject.tag == "Player")
        {
            // Player Death
        }
    }

    public void SetBananaPeelIsActive(bool _isActive)
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
}
