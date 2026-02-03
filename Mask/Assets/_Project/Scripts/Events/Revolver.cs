using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Revolver : PickupableItem
{
    [SerializeField] private float dischargeDelay = 0.5f;
    [SerializeField] private float rotateDuration = 0.15f;
    [SerializeField] private GameObject playerObject;

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
        if (playerObject == null) 
        {
            StartCoroutine(OnDischargeRevolver());
            return;
        }

        Vector3 targetLocation = playerObject.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetLocation);

        transform.DORotateQuaternion(targetRotation, rotateDuration)
            .OnComplete(() =>
            {
                StartCoroutine(OnDischargeRevolver());
            });

    }
}
