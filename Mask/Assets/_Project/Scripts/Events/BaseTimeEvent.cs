using AudioSystem;
using System.Collections.Generic;
using UnityEngine;

public class BaseTimeEvent : MonoBehaviour
{
    [SerializeField] protected SoundData _eventAudioData;
    protected bool _soundTriggered = false;

    [SerializeField] protected List<AudioClip> soundClips = new List<AudioClip>();

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

    protected void ResetSoundTriggered()
    {
        _soundTriggered = false;
    }
}
