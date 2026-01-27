using DG.Tweening;
using UnityEngine;

public class Chandelier : BaseTimeEvent
{
    [SerializeField] private Transform targetLocation;
    [SerializeField] private float fallDuration = 0.5f;

    private bool canFall = true;
    private bool hasFallen = false;

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        if (canFall && !hasFallen)
        {
            ChandelierFall();
        }
    }

    private void ChandelierFall()
    {
        transform.DOMove(targetLocation.position, fallDuration)
            .OnComplete(() =>
            {
                hasFallen = true;
            });
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasFallen) { return; }

        if (collision.gameObject.tag == "Player")
        {
            // Player Death
        }
    }
}
