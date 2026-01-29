using System.Collections;
using UnityEngine;

public class NPCDaffodil : NPCMove
{
    /*
     * Character Starting at Ballroom
     * Point One - Room 1 - 9:00PM
     * Point Two - Alcove - 10:00PM
     * Point Three - Ballroom - 11:00PM
     * Point Four - Ballroom - 12:00AM
     * Point Five - Library - 1:00AM
     * Point Six - Tea Room - 2:00AM
     * Point Seven - Billards - 3:00AM
     */

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        currentNPCLocationState = ENPCLocationState.PointOne;

        // Move to the Room 1 - 9:00PM
        TimeManager.Instance.ScheduleAt(60.0f, ActivateTimeEvent);

        // Move to the Alcove - 10:00PM
        TimeManager.Instance.ScheduleAt(120.0f, ActivateTimeEvent);

        // Move to the Ballroom - 11:00PM
        TimeManager.Instance.ScheduleAt(180.0f, ActivateTimeEvent);

        // Move to the Ballroom - 12:00AM
        TimeManager.Instance.ScheduleAt(240.0f, ActivateTimeEvent);

        // Move to the Library - 1:00AM
        TimeManager.Instance.ScheduleAt(300.0f, ActivateTimeEvent);

        // Move to the Tea Room - 2:00AM
        TimeManager.Instance.ScheduleAt(360.0f, ActivateTimeEvent);

        // Move to the Billards - 3:00AM
        TimeManager.Instance.ScheduleAt(420.0f, ActivateTimeEvent);
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();
    }
}
