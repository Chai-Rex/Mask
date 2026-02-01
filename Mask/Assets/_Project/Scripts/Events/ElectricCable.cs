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
        if(_electricAudioEmitter != null)
        {
            _electricAudioEmitter.gameObject.SetActive(false);
        }
        
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
        if (_electricAudioEmitter != null)
        {
            _electricAudioEmitter.gameObject.SetActive(_isActive);
        }

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
                        waterSpill.SetWaterSpillActive(true);
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
                        waterSpill.SetWaterSpillActive(false);
                        waterSpill.SetIsCableTouching(false);
                    }
                }
            }
        }
    }
}
