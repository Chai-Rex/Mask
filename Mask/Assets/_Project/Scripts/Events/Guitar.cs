using UnityEngine;

public class Guitar : PickupableItem
{
    public override void SetItemPickedUp(bool i_isPickedUp)
    {
        base.SetItemPickedUp(i_isPickedUp);

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
