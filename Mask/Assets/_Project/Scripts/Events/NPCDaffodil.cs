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

        if (npcLocationPoints.Count == 0) { return; }

        if (npcTimePoints.Count == 0) { return; }

        foreach (NPCTimePoints npcTimePoint in npcTimePoints)
        {
            TimeManager.Instance.ScheduleAt(npcTimePoint.npcLocationTime, ActivateTimeEvent);
        }

        // Move to the Room 1 - 9:00PM

        // Move to the Alcove - 10:00PM

        // Move to the Ballroom - 11:00PM

        // Move to the Ballroom - 12:00AM

        // Move to the Library - 1:00AM

        // Move to the Tea Room - 2:00AM

        // Move to the Billards - 3:00AM
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();
    }
}
