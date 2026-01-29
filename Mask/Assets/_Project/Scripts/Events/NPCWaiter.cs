using System.Collections;
using UnityEngine;

public class NPCWaiter : NPCMove
{
    [SerializeField] private GameObject platter;
    [SerializeField] private InteractablePoisonDrink poisonDrink;

    private void Start()
    {
        // First Schedule To Point Two
        TimeManager.Instance.ScheduleAt(5.0f, ActivateTimeEvent);

        // This can be changed if the waiter goes and gets drinks
        poisonDrink.SetPoisonDrinkIsActive(true);
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();
    }
}
