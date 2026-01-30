using UnityEngine;

namespace AudioSystem {
    public class SoundBuilder {
        private readonly SoundManager soundManager;
        Vector3 position = Vector3.zero;
        bool randomPitch;
        float minPitchMod = -0.05f;
        float maxPitchMod = 0.05f;

        public SoundBuilder(SoundManager soundManager) {
            this.soundManager = soundManager;
        }

        public SoundBuilder ResetFields() {
            position = Vector3.zero;
            randomPitch = false;
            return this;
        }


        public SoundBuilder WithPosition(Vector3 position) {
            this.position = position;
            return this;
        }

        public SoundBuilder WithRandomPitch(float min = -0.05f, float max = 0.05f) {
            this.randomPitch = true;
            this.minPitchMod = min;
            this.maxPitchMod = max;
            return this;
        }

        public SoundEmitter Play(SoundData soundData) {
            if (soundData == null) {
                Debug.LogError("SoundData is null");
                return null;
            }

            if (!soundManager.CanPlaySound(soundData)) return null;

            SoundEmitter soundEmitter = soundManager.Get();
            soundEmitter.Initialize(soundData);
            soundEmitter.transform.position = position;
            soundEmitter.transform.parent = soundManager.transform;

            if (randomPitch) {
                soundEmitter.WithRandomPitch(minPitchMod, maxPitchMod);
            }

            if (soundData.FrequentSound) {
                soundEmitter.Node = soundManager.FrequentSoundEmitters.AddLast(soundEmitter);
            }

            soundEmitter.Play();

            return soundEmitter;
        }
    }
}