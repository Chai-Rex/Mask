using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Revolver : PickupableItem
{
    [SerializeField] private float dischargeDelay = 0.5f;
    [SerializeField] private float rotateDuration = 0.15f;
    public override void SetItemPickedUp(bool i_isPickedUp)
    {
        base.SetItemPickedUp(i_isPickedUp);

        if (!i_isPickedUp)
        {
            OnRotateGun();
        }
    }

    private IEnumerator OnDischargeRevolver()
    {
        yield return new WaitForSeconds(dischargeDelay);

        PlayTriggerSound();
        DeathManager.Instance.Die("P-FERSONA!!!", "Revolver");
    }

    private void OnRotateGun()
    {
        Vector3 targetLocation = LevelManager.Instance.GetPlayerObject().transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetLocation);

        transform.DORotateQuaternion(targetRotation, rotateDuration)
            .OnComplete(() =>
            {
                StartCoroutine(OnDischargeRevolver());
            });

    }
}
