using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;
using UtilitySingletons;

namespace AudioSystem {
    public class SoundManager : PersistentSingleton<SoundManager> {

        private SoundBuilder _soundBuilder;
        private IObjectPool<SoundEmitter> _soundEmitterPool;
        private readonly List<SoundEmitter> activeSoundEmitters = new();
        public readonly LinkedList<SoundEmitter> FrequentSoundEmitters = new();


        [SerializeField] private SoundEmitter soundEmitterPrefab;
        [SerializeField] private bool collectionCheck = true;
        [SerializeField] private int defaultCapacity = 10;
        [SerializeField] private int maxPoolSize = 100;
        [SerializeField] private int maxSoundInstances = 30;


        protected override void Awake() {
            base.Awake();
            InitializePool();
            _soundBuilder = new SoundBuilder(this);
        }

        /// <summary>
        /// Example Usage:
        /// SoundManager.Instance.CreateSound()
        /// .WithSoundData(soundData.OnCollisionSoundData)
        /// .WithRandomPitch()
        /// .WithPosition(transform.postion)
        /// .Play();
        /// </summary>
        public SoundBuilder CreateSound() => _soundBuilder.ResetFields();

        public bool CanPlaySound(SoundData data) {
            if (!data.FrequentSound) return true;

            if (FrequentSoundEmitters.Count >= maxSoundInstances) {
                try {
                    FrequentSoundEmitters.First.Value.Stop();
                    return true;
                } catch {
                    Debug.Log("SoundEmitter is already released");
                }
                return false;
            }
            return true;
        }

        public SoundEmitter Get() {
            return _soundEmitterPool.Get();
        }

        public void ReturnToPool(SoundEmitter soundEmitter) {
            _soundEmitterPool.Release(soundEmitter);
        }

        private void OnDestroyPoolObject(SoundEmitter soundEmitter) {
            Destroy(soundEmitter.gameObject);
        }

        private void OnReturnedToPool(SoundEmitter soundEmitter) {
            if (soundEmitter.Node != null) {
                FrequentSoundEmitters.Remove(soundEmitter.Node);
                soundEmitter.Node = null;
            }
        }

        private void OnTakeFromPool(SoundEmitter soundEmitter) {
            soundEmitter.gameObject.SetActive(true);
            activeSoundEmitters.Add(soundEmitter);
        }

        private SoundEmitter CreateSoundEmitter() {
            var soundEmitter = Instantiate(soundEmitterPrefab);
            soundEmitter.gameObject.SetActive(false);
            return soundEmitter;
        }

        private void InitializePool() {
            _soundEmitterPool = new ObjectPool<SoundEmitter>(
                CreateSoundEmitter,
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                collectionCheck,
                defaultCapacity,
                maxPoolSize
            );
        }
    }


}

