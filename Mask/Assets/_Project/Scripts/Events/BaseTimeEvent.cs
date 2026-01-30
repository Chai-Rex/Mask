using AudioSystem;
using UnityEngine;

public class BaseTimeEvent : MonoBehaviour
{
    [SerializeField]
    private SoundData _eventAudioData;

    protected virtual void ActivateTimeEvent()
    {
        if(_eventAudioData != null && _eventAudioData.Clip != null)
        {
            SoundManager.Instance.CreateSound()
                .WithPosition(gameObject.transform.position)
                .Play(_eventAudioData);
        }

       
    }
}
