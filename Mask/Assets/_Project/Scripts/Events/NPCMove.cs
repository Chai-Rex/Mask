using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public enum ENPCSplineState
{
    None,
    PointOne,
    PointTwo,
    PointThree,
    PointFour,
    PointFive,
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
    protected SplineAnimate splineAnimate;

    [SerializeField] private List<NPCSpline> npcSplines = new List<NPCSpline>();
    private Dictionary<ENPCSplineState, SplineContainer> npcSplineDictionary = new Dictionary<ENPCSplineState, SplineContainer>();
    protected ENPCSplineState currentNPCSplineState = ENPCSplineState.PointOne;

    protected Coroutine currentCoroutine;

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

    protected virtual void NPCMoveOnSpline()
    {
        splineAnimate.Container = npcSplineDictionary[currentNPCSplineState];
        splineAnimate.Restart(false);
        splineAnimate.Play();
    }

    protected virtual IEnumerator HasNPCFinishedMoving()
    {
        yield return null;
    }
}
