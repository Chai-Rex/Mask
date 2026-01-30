using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class RoomSoundEmitter : MonoBehaviour
{
    [SerializeField]
    private float fadeTime = 1.0f;
    private float volumeLevel = 0f;
    private float startFadeTime = 0f;

    [SerializeField]
    private AudioSource MusicSource;
    [SerializeField]
    private List<AudioSource> AmbianceSource;

    private void Start()
    {
        MusicSource.loop = true;
        foreach(AudioSource source in AmbianceSource)
        {
            source.loop = true;
        }
    }

    IEnumerator FadeAudioIn()
    {
        while(volumeLevel < 1.0f)
        {
            volumeLevel = Mathf.Clamp((Time.time - startFadeTime) / fadeTime, 0, 1);
            SetVolumes(volumeLevel);

            yield return null;
        }
        
    }

    IEnumerator FadeAudioOut()
    {
        while (volumeLevel > 0f)
        {
            volumeLevel = Mathf.Clamp(1f - ((Time.time - startFadeTime) / fadeTime), 0, 1);
            SetVolumes(volumeLevel);

            yield return null;
        }

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
            MusicSource.volume = i_volume;
        }

        foreach (AudioSource source in AmbianceSource)
        {
            if (source != null && source.clip != null)
            {
                source.volume = i_volume;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (MusicSource != null && MusicSource.clip != null)
            {
                MusicSource.Play();
            }

            foreach(AudioSource source in AmbianceSource)
            {
                if(source != null && source.clip != null)
                {
                    source.Play();
                }
            }

            startFadeTime = Time.time;
            StartCoroutine(FadeAudioIn());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            startFadeTime = Time.time;
            StartCoroutine(FadeAudioOut());
        }
    }
}
