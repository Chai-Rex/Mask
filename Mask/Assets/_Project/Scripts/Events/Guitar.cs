using UnityEngine;

public class Guitar : PickupableItem
{
    [SerializeField] RoomSoundEmitter _guitarAudioEmitter;

    StateVariable isPickedUp = new StateVariable("isGuitarPickedUp", false);

    public override void SetItemPickedUp(bool i_isPickedUp)
    {
        base.SetItemPickedUp(i_isPickedUp);
        isPickedUp.SetValueAndUpdateBlackboard(i_isPickedUp);

        if (i_isPickedUp)
        {
            if (soundClips.Count != 0 && soundClips.Count >= 3)
            {
                int randomIndex = Random.Range(0, soundClips.Count);
                _eventAudioData.Clip = soundClips[randomIndex];
            }

            PlayTriggerSound();
            ResetSoundTriggered();
        }
    }
}
