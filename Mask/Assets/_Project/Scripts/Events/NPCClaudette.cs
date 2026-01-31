using System.Collections;
using UnityEngine;

public class NPCClaudette : NPCMove
{
    /* 
     * Character Starting at Ballroom
     * Point One - Ballroom - 9:00PM
     * Point Two - Team Room - 10:00PM
     * Point Three - Reception - 11:00PM
     * Point Four - Dining - 12:00AM
     * Point Five - Library - 1:00AM
     * Point Six - Ballroom - 2:00AM
     * Point Seven - Room 2 - 3:00AM
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

        // Move to the Ballroom - 9:00PM

        // Move to the Team Room - 10:00PM

        // Move to the Reception - 11:00PM

        // Move to the Dining - 12:00AM

        // Move to the Library - 1:00AM

        // Move to the Ballroom - 2:00AM

        // Move to the Room 2 - 3:00AM
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();
    }
}
