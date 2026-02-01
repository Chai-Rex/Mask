using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static Unity.VisualScripting.Member;

public class RoomSoundEmitter : MonoBehaviour
{
    [SerializeField]
    private float fadeTime = 1.0f;
    [SerializeField]
    public float volumeMultiplier = 1.0f;
    private float volumeLevel = 0f;
    private float startFadeTime = 0f;
    private bool fadeIn = false;
    private bool fadeOut = false;

    [SerializeField]
    private AudioSource MusicSource;
    [SerializeField]
    private List<AudioSource> AmbianceSource;

    [SerializeField] private bool hasCheckValue = false;
    [SerializeField] private string checkValueName = string.Empty;
    [SerializeField] private bool checkValue = true;
    private bool playerInArea = false;

    [SerializeField] private bool triggerValueOnFirstPlay = false;
    [SerializeField] private string triggerValueName = string.Empty;
    [SerializeField] private bool triggerValue = true;
    private bool hasTriggered = false;

    private void Start()
    {
        MusicSource.loop = true;
        foreach(AudioSource source in AmbianceSource)
        {
            if (source != null)
            {
                source.loop = true;
            }
        }

        if(hasCheckValue)
        {
            StoryStateSO.Instance.RegisterCallback(checkValueName, SetCheckValue);
        }
        
    }

    IEnumerator FadeAudioIn()
    {
        
        fadeOut = false;
        fadeIn = true;
        while(volumeLevel < 1.0f && !fadeOut)
        {
            volumeLevel = Mathf.Clamp((Time.time - startFadeTime) / fadeTime, 0, 1);
            SetVolumes(volumeLevel);

            yield return null;
        }
        fadeIn = false;

        if(triggerValueOnFirstPlay && !hasTriggered)
        {
            StoryStateSO.Instance.SetValue(triggerValueName, triggerValue);
        }
    }

    IEnumerator FadeAudioOut()
    {
        fadeIn = false;
        fadeOut = true;
        while (volumeLevel > 0f && !fadeIn)
        {
            volumeLevel = Mathf.Clamp(1f - ((Time.time - startFadeTime) / fadeTime), 0, 1);
            SetVolumes(volumeLevel);

            yield return null;
        }

        fadeOut = false;
        if (fadeIn) { yield break; }

        if (MusicSource != null && MusicSource.clip != null)
        {
            MusicSource.Pause();
        }

        foreach (AudioSource source in AmbianceSource)
        {
            if (source != null && source.clip != null)
            {
                source.Pause();
            }
        }
    }

    private void SetVolumes(float i_volume)
    {
        if (MusicSource != null && MusicSource.clip != null)
        {
            MusicSource.volume = i_volume * volumeMultiplier;
        }

        foreach (AudioSource source in AmbianceSource)
        {
            if (source != null && source.clip != null)
            {
                source.volume = i_volume * volumeMultiplier;
            }
        }
    }

    private void SetCheckValue(bool value)
    {
        if (!playerInArea) return;

        if(value == checkValue)
        {
            StartFadeIn();
        }
        else
        {
            StartFadeOut();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInArea = true;
            if (!hasCheckValue || StoryStateSO.Instance.GetValue(checkValueName) == checkValue)
            {
                StartFadeIn();
            }
        }
    }

    public void StartFadeIn()
    {

        if(!fadeIn)
        {
            if (MusicSource != null && MusicSource.clip != null)
            {
                if (!MusicSource.isPlaying)
                {
                    if (MusicSource.time != 0)
                    {
                        MusicSource.UnPause();
                    }
                    else
                    {
                        MusicSource.Play();
                    }
                }

            }

            foreach (AudioSource source in AmbianceSource)
            {
                if (source != null && source.clip != null)
                {
                    if (!source.isPlaying)
                    {
                        if (source.time != 0)
                        {
                            source.UnPause();
                        }
                        else
                        {
                            source.Play();
                        }
                    }
                }
            }

            startFadeTime = Time.time - (volumeLevel * fadeTime);
            StartCoroutine(FadeAudioIn());
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInArea = false;

            if (!hasCheckValue || StoryStateSO.Instance.GetValue(checkValueName) == checkValue)
            {
                StartFadeOut();
            }
            
        }
    }

    public void StartFadeOut()
    {

        if (!fadeOut)
        {
            startFadeTime = Time.time - ((1 - volumeLevel) * fadeTime);
            StartCoroutine(FadeAudioOut());
        }

    }
}
