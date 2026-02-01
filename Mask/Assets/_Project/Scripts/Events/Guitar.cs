using UnityEngine;

public class Guitar : PickupableItem
{
    public override void SetItemPickedUp(bool i_isPickedUp)
    {
        base.SetItemPickedUp(i_isPickedUp);

        if (i_isPickedUp)
        {
            PlayTriggerSound();
        }
    }
}
