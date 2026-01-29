using System.Collections;
using UnityEngine;

public class Waiter : NPCMove
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

    protected override void NPCMoveOnSpline()
    {
        base.NPCMoveOnSpline();

        switch (currentNPCSplineState)
        {
            case ENPCSplineState.PointOne:
                currentNPCSplineState = ENPCSplineState.PointTwo;
                break;
            case ENPCSplineState.PointTwo:
                currentNPCSplineState = ENPCSplineState.PointOne;
                break;
        }

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        currentCoroutine = StartCoroutine(HasNPCFinishedMoving());     
    }

    protected override IEnumerator HasNPCFinishedMoving()
    {
        while (splineAnimate.IsPlaying)
        {
            yield return null;
        }

        switch (currentNPCSplineState)
        {
            case ENPCSplineState.PointOne:
                // Waiter Moves To Point Two
                TimeManager.Instance.ScheduleAfter(5.0f, ActivateTimeEvent);
                break;
            case ENPCSplineState.PointTwo:
                // Waiter Moves To Point One
                TimeManager.Instance.ScheduleAfter(5.0f, ActivateTimeEvent);
                break;
        }


    }
}
