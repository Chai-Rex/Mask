using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public enum ENPCSplineState
{
    None,
    AtStart,
    AtEnd
}

[System.Serializable]
public class NPCSpline
{
    public ENPCSplineState npcSplineState;
    public SplineContainer splineContainer;
}

public class NPCMove : BaseTimeEvent
{
    [SerializeField] private float moveDuration = 5.0f;
    private SplineAnimate splineAnimate;

    [SerializeField] private List<NPCSpline> npcSplines = new List<NPCSpline>();
    private Dictionary<ENPCSplineState, SplineContainer> npcSplineDictionary = new Dictionary<ENPCSplineState, SplineContainer>();
    private ENPCSplineState currentNPCSplineState = ENPCSplineState.AtStart;

    private void Awake()
    {
        splineAnimate = GetComponent<SplineAnimate>();
        splineAnimate.PlayOnAwake = false;
        splineAnimate.Loop = SplineAnimate.LoopMode.Once;
        splineAnimate.Duration = moveDuration;

        foreach (NPCSpline npcSpine in npcSplines)
        {
            npcSplineDictionary[npcSpine.npcSplineState] = npcSpine.splineContainer;
        }
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        if (!splineAnimate.IsPlaying)
        {
            NPCMoveOnSpline();
        }
    }

    private void NPCMoveOnSpline()
    {
        splineAnimate.Container = npcSplineDictionary[currentNPCSplineState];
        splineAnimate.Restart(false);
        splineAnimate.Play();

        switch (currentNPCSplineState)
        {
            case ENPCSplineState.AtStart:
                currentNPCSplineState = ENPCSplineState.AtEnd;
                break;
            case ENPCSplineState.AtEnd:
                currentNPCSplineState = ENPCSplineState.AtStart;
                break;
        }
    }
}
