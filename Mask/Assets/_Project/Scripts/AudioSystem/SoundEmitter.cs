using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AudioSystem {
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour {

        public SoundData Data { get; private set; }
        public LinkedListNode<SoundEmitter> Node { get; set; }

        private AudioSource audioSource;
        Coroutine playingCoroutine;

        private void Awake() {
            audioSource = gameObject.GetComponent<AudioSource>();
        }
        public void Initialize(SoundData data) {
            Data = data;
            audioSource.clip = data.Clip;
            audioSource.outputAudioMixerGroup = data.MixerGroup;
            audioSource.loop = data.Loop;
            audioSource.playOnAwake = data.PlayOnAwake;

            audioSource.mute = data.Mute;
            audioSource.bypassEffects = data.BypassEffects;
            audioSource.bypassListenerEffects = data.BypassListenerEffects;
            audioSource.bypassReverbZones = data.BypassReverbZones;

            audioSource.priority = data.Priority;
            audioSource.volume = data.Volume;
            audioSource.pitch = data.Pitch;
            audioSource.panStereo = data.PanStereo;
            audioSource.spatialBlend = data.SpatialBlend;
            audioSource.reverbZoneMix = data.ReverbZoneMix;
            audioSource.dopplerLevel = data.DopplerLevel;
            audioSource.spread = data.Spread;

            audioSource.minDistance = data.MinDistance;
            audioSource.maxDistance = data.MaxDistance;

            audioSource.ignoreListenerVolume = data.IgnoreListenerVolume;
            audioSource.ignoreListenerPause = data.IgnoreListenerPause;

            audioSource.rolloffMode = data.rolloffMode;
        }

        public void Play() {
            if (playingCoroutine != null) {
                StopCoroutine(playingCoroutine);
            }

            audioSource.Play();
            playingCoroutine = StartCoroutine(WaitForSoundToEnd());
        }

        public void Stop() {
            if (playingCoroutine != null) {
                StopCoroutine(playingCoroutine);
                playingCoroutine = null;
            }

            audioSource.Stop();
            SoundManager.Instance.ReturnToPool(this);
        }

        IEnumerator WaitForSoundToEnd() {
            yield return new WaitWhile(() => audioSource.isPlaying);
            Stop();
        }

        public void WithRandomPitch(float min = -0.05f, float max = 0.05f) {
            audioSource.pitch += Random.Range(min, max);
        }
    }
}

