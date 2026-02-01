using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Chandelier : BaseTimeEvent
{
    [SerializeField] private Transform targetLocation;
    [SerializeField] private float fallDuration = 0.5f;

    private StateVariable canFall = new StateVariable("canChandelierFall", false);
    private StateVariable hasFallen = new StateVariable("hasChandelierFallen", false);

    [SerializeField] private float chandelierFallTime = 60.0f;
    private BoxCollider boxCollider;

    [SerializeField] private float beforeFallDelay = 2.0f;
    [SerializeField] private float interFallDelay = 2.0f;

    private Coroutine beforeFallCoroutine;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    private void Start()
    {
        TimeManager.Instance.ScheduleAt(chandelierFallTime, ActivateTimeEvent);
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        canFall.SetValueAndUpdateBlackboard(true);
        if (canFall.Value && !hasFallen.Value)
        {
            if (beforeFallCoroutine == null)
            {
                beforeFallCoroutine = StartCoroutine(OnBeforeFall());
            }
        }
    }

    private IEnumerator OnBeforeFall()
    {
        if (soundClips.Count != 0 && soundClips.Count >= 3)
        {
            _eventAudioData.Clip = soundClips[0];
        }

        PlayTriggerSound();
        ResetSoundTriggered();
        
        yield return new WaitForSeconds(beforeFallDelay);

        if (canFall.Value && !hasFallen.Value)
        {
            ChandelierFall();
        }
    }

    private void ChandelierFall()
    {
        if (soundClips.Count != 0 && soundClips.Count >= 3)
        {
            _eventAudioData.Clip = soundClips[1];
        }

        PlayTriggerSound();
        ResetSoundTriggered();

        transform.DOMove(targetLocation.position, fallDuration)
            .OnComplete(() =>
            {
                if (soundClips.Count != 0 && soundClips.Count >= 3)
                {
                    _eventAudioData.Clip = soundClips[2];
                }

                PlayTriggerSound();

                hasFallen.SetValueAndUpdateBlackboard(true);
                if (boxCollider)
                {
                    boxCollider.isTrigger = false;
                }
            });

        beforeFallCoroutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasFallen.Value) { return; }

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Player Death
            DeathManager.Instance.Die("Chandelier Bonked you on the head", "Chandelier");
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        beforeFallCoroutine = null;
    }
}
