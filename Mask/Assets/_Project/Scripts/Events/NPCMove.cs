using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ENPCLocationState
{
    None,
    PointOne,
    PointTwo,
    PointThree,
    PointFour,
    PointFive,
    PointSix,
    PointSeven,
}

[System.Serializable]
public class NPCLocationPoints
{
    public ENPCLocationState npcLocationState;
    public List<Transform> npcTransforms;
}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ProceduralWalkAnimation))]
public class NPCMove : BaseTimeEvent
{
    [SerializeField] private ProceduralWalkAnimation _iProceduralWalkAnimation;

    [SerializeField] private float moveSpeed = 2.5f;

    [SerializeField] private List<NPCLocationPoints> npcLocationPoints = new List<NPCLocationPoints>();
    protected Dictionary<ENPCLocationState, List<Transform>> npcLocationDictionary = new Dictionary<ENPCLocationState, List<Transform>>();
    protected ENPCLocationState currentNPCLocationState = ENPCLocationState.PointOne;

    protected Coroutine collectionPointsCoroutine;
    protected Coroutine pointCoroutine;
    protected Coroutine npcMoveCoroutine;

    protected NavMeshAgent navMeshAgent;

    protected virtual void Awake()
    {
        if (_iProceduralWalkAnimation == null) _iProceduralWalkAnimation = GetComponent<ProceduralWalkAnimation>();

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

            if (npcLocationDictionary.Count != 0 && npcLocationDictionary.ContainsKey(currentNPCLocationState))
            {
                npcMoveCoroutine = StartCoroutine(OnNPCMove());
            }
        }
    }

    protected virtual IEnumerator OnNPCMove()
    {
        yield return collectionPointsCoroutine = StartCoroutine(OnNPCMoveThroughCollectionOfPoints());

        currentNPCLocationState++;

        switch (currentNPCLocationState)
        {
            case ENPCLocationState.PointOne:
                currentNPCLocationState = ENPCLocationState.PointTwo;
                break;
            case ENPCLocationState.PointTwo:
                currentNPCLocationState = ENPCLocationState.PointThree;
                break;
            case ENPCLocationState.PointThree:
                currentNPCLocationState = ENPCLocationState.PointFour;
                break;
            case ENPCLocationState.PointFour:
                currentNPCLocationState = ENPCLocationState.PointFive;
                break;
            case ENPCLocationState.PointFive:
                currentNPCLocationState = ENPCLocationState.PointSix;
                break;
            case ENPCLocationState.PointSix:
                currentNPCLocationState = ENPCLocationState.PointSeven;
                break;
        }
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

        // move animation start
        _iProceduralWalkAnimation.StartWalking(moveSpeed);

        while (navMeshAgent.pathPending)
        {
            yield return null;
        }

        while (navMeshAgent.remainingDistance > 0.001f)
        {
            yield return null;
        }

        navMeshAgent.isStopped = true;

        // move animation end
        _iProceduralWalkAnimation.StopWalking();

    }
}
