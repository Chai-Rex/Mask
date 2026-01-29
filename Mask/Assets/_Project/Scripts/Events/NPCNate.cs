using System.Collections;
using UnityEngine;

public class NPCNate : NPCMove
{
    /* 
     * Character Starting at Ballroom
     * Point One - Dining Room - 9:00PM
     * Point Two - Tea Room - 10:00PM
     * Point Three - Ballroom - 11:00PM
     * Point Four - Room 1 - 12:00AM
     * Point Five - Sitting Room - 1:00AM
     * Point Six - Reception - 2:00AM
     * Point Seven - Ballroom - 3:00AM
    */

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        currentNPCLocationState = ENPCLocationState.PointOne;

        // Move to the Dining Room - 9:00PM
        TimeManager.Instance.ScheduleAt(60.0f, ActivateTimeEvent);

        // Move to the Tea Room - 10:00PM
        TimeManager.Instance.ScheduleAt(120.0f, ActivateTimeEvent);

        // Move to the Ballroom - 11:00PM
        TimeManager.Instance.ScheduleAt(180.0f, ActivateTimeEvent);

        // Move to the Room 1 - 12:00AM
        TimeManager.Instance.ScheduleAt(240.0f, ActivateTimeEvent);

        // Move to the Sitting Room - 1:00AM
        TimeManager.Instance.ScheduleAt(300.0f, ActivateTimeEvent);

        // Move to the Reception - 2:00AM
        TimeManager.Instance.ScheduleAt(360.0f, ActivateTimeEvent);

        // Move to the Ballroom - 3:00AM
        TimeManager.Instance.ScheduleAt(420.0f, ActivateTimeEvent);
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();
    }
}
