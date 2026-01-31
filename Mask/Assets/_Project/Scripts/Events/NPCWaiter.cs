using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWaiter : NPCMove
{
    /*
     * Cycles Through All Points
     */

    [SerializeField] private float waiterMoveDelay = 7.5f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        // First Schedule To Point Two
        TimeManager.Instance.ScheduleAt(5.0f, ActivateTimeEvent);
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();
    }

    protected override IEnumerator OnNPCMove()
    {
        yield return collectionPointsCoroutine = StartCoroutine(OnNPCMoveThroughCollectionOfPoints());

        switch (currentNPCLocationState)
        {
            case ENPCLocationState.PointOne:
                currentNPCLocationState = ENPCLocationState.PointTwo;
                break;
            case ENPCLocationState.PointTwo:
                currentNPCLocationState = ENPCLocationState.PointThree;
                break;
            case ENPCLocationState.PointThree:
                currentNPCLocationState = ENPCLocationState.PointFour;
                break;
            case ENPCLocationState.PointFour:
                currentNPCLocationState = ENPCLocationState.PointFive;
                break;
            case ENPCLocationState.PointFive:
                currentNPCLocationState = ENPCLocationState.PointSix;
                break;
            case ENPCLocationState.PointSix:
                currentNPCLocationState = ENPCLocationState.PointSeven;
                break;
            default:
                currentNPCLocationState = ENPCLocationState.PointOne;
                break;
        }

        TimeManager.Instance.ScheduleAfter(waiterMoveDelay, ActivateTimeEvent);
    }
}
