using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ENPCLocationState
{
    PointOne,
    PointTwo,
    PointThree,
    PointFour,
    PointFive,
    PointSix,
    PointSeven,
}

[System.Serializable]
public struct NPCLocationPoints
{
    public ENPCLocationState npcLocationState;
    public List<Transform> npcTransforms;
}

[System.Serializable]
public struct NPCTimePoints
{
    public ENPCLocationState npcLocationState;
    public float npcLocationTime;
}

    public class NPCMove : BaseTimeEvent
{
    [SerializeField] private float moveSpeed = 2.5f;

    [SerializeField] protected List<NPCLocationPoints> npcLocationPoints = new List<NPCLocationPoints>();
    [SerializeField] protected List<NPCTimePoints> npcTimePoints = new List<NPCTimePoints>();
    protected Dictionary<ENPCLocationState, List<Transform>> npcLocationDictionary = new Dictionary<ENPCLocationState, List<Transform>>();
    protected ENPCLocationState currentNPCLocationState = ENPCLocationState.PointOne;
    protected int currentNPCTimePointIndex = 0;

    protected Coroutine collectionPointsCoroutine;
    protected Coroutine pointCoroutine;
    protected Coroutine npcMoveCoroutine;

    protected NavMeshAgent navMeshAgent;

    protected virtual void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;

        foreach (NPCLocationPoints npcLocationPoint in npcLocationPoints)
        {
            npcLocationDictionary[npcLocationPoint.npcLocationState] = npcLocationPoint.npcTransforms;
        }
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        if (navMeshAgent)
        {
            StopAllCoroutines();
            collectionPointsCoroutine = null;
            pointCoroutine = null;
            npcMoveCoroutine = null;

            if (npcLocationDictionary.Count != 0 && npcLocationDictionary.ContainsKey(currentNPCLocationState) && npcTimePoints.Count != 0)
            {
                npcMoveCoroutine = StartCoroutine(OnNPCMove());
            }
        }
    }

    protected virtual IEnumerator OnNPCMove()
    {
        yield return collectionPointsCoroutine = StartCoroutine(OnNPCMoveThroughCollectionOfPoints());

        currentNPCTimePointIndex++;

        if (currentNPCTimePointIndex >= npcTimePoints.Count)
        {
            currentNPCTimePointIndex = 0;
        }

        currentNPCLocationState = npcTimePoints[currentNPCTimePointIndex].npcLocationState;
    }

    protected virtual IEnumerator OnNPCMoveThroughCollectionOfPoints()
    {
        int currentPoint = 0;

        while (currentPoint < npcLocationDictionary[currentNPCLocationState].Count)
        {
            yield return pointCoroutine = StartCoroutine(OnNPCMoveToPoint(currentPoint));
            currentPoint++;
        }
    }

    protected virtual IEnumerator OnNPCMoveToPoint(int _currentPoint)
    {
        navMeshAgent.isStopped = false;

        navMeshAgent.SetDestination(npcLocationDictionary[currentNPCLocationState][_currentPoint].position);      

        while (navMeshAgent.pathPending)
        {
            yield return null;
        }

        while (navMeshAgent.remainingDistance > 0.001f)
        {
            yield return null;
        }

        navMeshAgent.isStopped = true;
    }
}
