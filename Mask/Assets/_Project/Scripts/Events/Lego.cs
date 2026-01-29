using UnityEngine;

public class Lego : BaseTimeEvent
{
    private bool isActive = false;

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        SetLegoIsActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isActive) { return; }

        if (collision.gameObject.tag == "Player")
        {
            // Player Death
        }
    }

    public void SetLegoIsActive(bool _isActive)
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
