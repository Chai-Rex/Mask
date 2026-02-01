using UnityEngine;

public class Guitar : PickupableItem
{
    [SerializeField] RoomSoundEmitter _guitarAudioEmitter;

    private void Start()
    {
        if (_guitarAudioEmitter != null)
        {
            _guitarAudioEmitter.gameObject.SetActive(false);
        }
    }

    public override void SetItemPickedUp(bool i_isPickedUp)
    {
        base.SetItemPickedUp(i_isPickedUp);

        if (_guitarAudioEmitter != null)
        {
            _guitarAudioEmitter.gameObject.SetActive(i_isPickedUp);
        }

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
