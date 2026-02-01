using System.Collections;
using UnityEngine;

public class Revolver : PickupableItem
{
    [SerializeField] private float dischargeDelay = 0.5f;
    public override void SetItemPickedUp(bool i_isPickedUp)
    {
        base.SetItemPickedUp(i_isPickedUp);

        if (!i_isPickedUp)
        {
            StartCoroutine(OnDischargeRevolver());
        }
    }

    private IEnumerator OnDischargeRevolver()
    {
        yield return new WaitForSeconds(dischargeDelay);
        DeathManager.Instance.Die("Discharged... To Getting Blasted", "Discharge");
    }
}
