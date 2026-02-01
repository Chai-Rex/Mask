using System.Collections.Generic;
using UnityEngine;

public class ElectricCable : BaseTimeEvent
{
    [SerializeField] private List<WaterSpill> waterSpills = new List<WaterSpill>();
    private StateVariable isActive = new StateVariable("isElectricCableActive", false);
    [SerializeField] private float electricCableActivateTime = 60.0f;
    [SerializeField] private ParticleSystem sparkParticles;

    [SerializeField] RoomSoundEmitter _electricAudioEmitter;

    private void Start()
    {
        sparkParticles.gameObject.SetActive(false);
        _electricAudioEmitter.volumeMultiplier = 0;
        TimeManager.Instance.ScheduleAt(electricCableActivateTime, ActivateTimeEvent);
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        SetIsElectricCableActive(true);
    }

    public void SetIsElectricCableActive(bool _isActive)
    {
        isActive.SetValueAndUpdateBlackboard(_isActive);
        _electricAudioEmitter.volumeMultiplier = _isActive ? 1 : 0;

        if (sparkParticles)
        {
            sparkParticles.gameObject.SetActive(true);
            sparkParticles.Play();
        }

        if (isActive.Value)
        {
            gameObject.SetActive(true);
            if (waterSpills != null && waterSpills.Count != 0)
            {
                foreach (WaterSpill waterSpill in waterSpills)
                {
                    if (waterSpill)
                    {
                        waterSpill.SetIsCableTouching(true);
                    }
                }
            }
        }
        else
        {
            gameObject.SetActive(false);
            if (waterSpills != null && waterSpills.Count != 0)
            {
                foreach (WaterSpill waterSpill in waterSpills)
                {
                    if (waterSpill)
                    {
                        waterSpill.SetIsCableTouching(false);
                    }
                }
            }
        }
    }
}
