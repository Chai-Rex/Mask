using System.Collections;
using UnityEngine;

public class NPCCallum : NPCMove
{
    /* 
     * Character Starting at Ballroom
     * Point One - Room 1 - 9:00PM
     * Point Two - Billards - 10:00PM
     * Point Three - Billards - 11:00PM
     * Point Four - Library - 12:00AM
     * Point Five - Sitting Room - 1:00AM
     * Point Six - Ballroom - 2:00AM
     * Point Seven - Ballroom - 3:00AM
    */

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        currentNPCLocationState = ENPCLocationState.PointOne;

        // Move to the Room 1 - 9:00PM
        TimeManager.Instance.ScheduleAt(5.0f, ActivateTimeEvent);

        // Move to the Billards - 10:00PM
        TimeManager.Instance.ScheduleAt(60.0f, ActivateTimeEvent);

        // Move to the Billards - 11:00PM
        TimeManager.Instance.ScheduleAt(140.0f, ActivateTimeEvent);

        // Move to the Library - 12:00AM
        TimeManager.Instance.ScheduleAt(165.0f, ActivateTimeEvent);

        // Move to the Sitting Room - 1:00AM
        TimeManager.Instance.ScheduleAt(185.0f, ActivateTimeEvent);

        // Move to the Ballroom - 2:00AM
        TimeManager.Instance.ScheduleAt(230.0f, ActivateTimeEvent);

        // Move to the Ballroom - 3:00AM
        TimeManager.Instance.ScheduleAt(270.0f, ActivateTimeEvent);
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();
    }
}
