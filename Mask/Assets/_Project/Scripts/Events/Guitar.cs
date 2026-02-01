using UnityEngine;

public class Guitar : PickupableItem
{

    public void Start()
    {
        TimeManager.Instance.ScheduleAt(60, SetIntroFinished);
        TimeManager.Instance.ScheduleAt(540, SetOutroStarted);
    }

    public void SetIntroFinished()
    {
        StoryStateSO.Instance.SetValue("isIntroFinished", true);
    }

    public void SetOutroStarted()
    {
        StoryStateSO.Instance.SetValue("hasOutroStarted", true);
    }

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
