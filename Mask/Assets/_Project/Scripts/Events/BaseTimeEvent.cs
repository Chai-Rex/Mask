using AudioSystem;
using UnityEngine;

public class BaseTimeEvent : MonoBehaviour
{
    [SerializeField]
    private SoundData _eventAudioData;
    bool _soundTriggered = false;

    protected virtual void ActivateTimeEvent()
    {
        
    }

    protected void PlayTriggerSound()
    {
        if (!_soundTriggered && _eventAudioData != null && _eventAudioData.Clip != null)
        {
            _soundTriggered = true;
            SoundManager.Instance.CreateSound()
                .WithPosition(gameObject.transform.position)
                .Play(_eventAudioData);
        }
    }
}
