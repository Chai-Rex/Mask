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

        if (npcTimePoints.Count == 0) { return; }

        foreach (NPCTimePoints npcTimePoint in npcTimePoints)
        {
            TimeManager.Instance.ScheduleAt(npcTimePoint.npcLocationTime, ActivateTimeEvent);
        }

        // Move to the Dining Room - 9:00PM

        // Move to the Tea Room - 10:00PM

        // Move to the Ballroom - 11:00PM

        // Move to the Room 1 - 12:00AM

        // Move to the Sitting Room - 1:00AM

        // Move to the Reception - 2:00AM

        // Move to the Ballroom - 3:00AM
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();
    }
}
