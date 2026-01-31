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

        if (npcTimePoints.Count == 0) { return; }

        foreach (NPCTimePoints npcTimePoint in npcTimePoints)
        {
            TimeManager.Instance.ScheduleAt(npcTimePoint.npcLocationTime, ActivateTimeEvent);
        }

        // Move to the Room 1 - 9:00PM

        // Move to the Billards - 10:00PM

        // Move to the Billards - 11:00PM

        // Move to the Library - 12:00AM

        // Move to the Sitting Room - 1:00AM

        // Move to the Ballroom - 2:00AM

        // Move to the Ballroom - 3:00AM
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();
    }
}
