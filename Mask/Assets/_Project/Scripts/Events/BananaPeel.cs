using UnityEngine;

public class BananaPeel : BaseTimeEvent
{
    private StateVariable isActive = new StateVariable("isBananaPeelActive", false);
    [SerializeField] private float bananaPeelActivateTime = 160.0f;

    private void Start()
    {
        TimeManager.Instance.ScheduleAt(bananaPeelActivateTime, ActivateTimeEvent);
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        SetBananaPeelIsActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (!isActive.Value) { return; }

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("HIT");
            PlayTriggerSound();
            DeathManager.Instance.Die("Slipped On A Banana Peel");
        }
    }

    public void SetBananaPeelIsActive(bool _isActive)
    {
        isActive.SetValueAndUpdateBlackboard(_isActive);

        if (isActive.Value)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
