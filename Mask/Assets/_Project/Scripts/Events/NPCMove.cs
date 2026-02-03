using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* 
    * Claudette
    * Character Starting at Ballroom
    * Point One - Ballroom - 9:00PM
    * Point Two - Tea Room - 10:00PM
    * Point Three - Reception - 11:00PM
    * Point Four - Dining - 12:00AM
    * Point Five - Library - 1:00AM
    * Point Six - Ballroom - 2:00AM
    * Point Seven - Room 2 - 3:00AM
    * 
    * Nate
    * Character Starting at Ballroom
    * Point One - Dining Room - 9:00PM
    * Point Two - Tea Room - 10:00PM
    * Point Three - Ballroom - 11:00PM
    * Point Four - Room 1 - 12:00AM
    * Point Five - Sitting Room - 1:00AM
    * Point Six - Reception - 2:00AM
    * Point Seven - Ballroom - 3:00AM
    * 
    * Daffodil
    * Character Starting at Ballroom
    * Point One - Room 1 - 9:00PM
    * Point Two - Alcove - 10:00PM
    * Point Three - Ballroom - 11:00PM
    * Point Four - Ballroom - 12:00AM
    * Point Five - Library - 1:00AM
    * Point Six - Tea Room - 2:00AM
    * Point Seven - Billards - 3:00AM
    * 
    * Callum
    * Character Starting at Ballroom
    * Point One - Room 1 - 9:00PM
    * Point Two - Billards - 10:00PM
    * Point Three - Billards - 11:00PM
    * Point Four - Library - 12:00AM
    * Point Five - Sitting Room - 1:00AM
    * Point Six - Ballroom - 2:00AM
    * Point Seven - Ballroom - 3:00AM
*/

public enum ENPCLocationState
{
    PointOne,
    PointTwo,
    PointThree,
    PointFour,
    PointFive,
    PointSix,
    PointSeven,
    PointEight
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

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ProceduralWalkAnimation))]
public class NPCMove : BaseTimeEvent
{
    [SerializeField] private ProceduralWalkAnimation _iProceduralWalkAnimation;
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

    [Header("IS BUTLER: If Loop is Enabled, Ignores NPC Time Points and Just Loops Through NPC Location Points")]
    [SerializeField] private bool isButler = false;
    [SerializeField] private bool isLoop = false;
    [SerializeField] private float startMoveTime = 5.0f;
    [SerializeField] private float delayBetweenMoves = 5.0f;
    [SerializeField] private NPCLocationPoints butlerFinalLocation = new NPCLocationPoints();
    [SerializeField] private GameObject PlatterWithDrinks;
    [SerializeField] private GameObject RightHand;

    private bool isPausedNPCMovement = false;

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

    private void Start()
    {
        LevelManager.Instance.GetDialogueHandler().AddNPCMovementComponent(this);

        if (isButler)
        {
            TimeManager.Instance.ScheduleAt(480.0f, OnButlerFinalMove);
        }

        currentNPCLocationState = ENPCLocationState.PointOne;

        if (npcTimePoints.Count == 0) { return; }

        if (isLoop)
        {
            TimeManager.Instance.ScheduleAt(startMoveTime, ActivateTimeEvent);
        }
        else
        {
            foreach (NPCTimePoints npcTimePoint in npcTimePoints)
            {
                TimeManager.Instance.ScheduleAt(npcTimePoint.npcLocationTime, ActivateTimeEvent);
            }
        }
    }

    private void OnButlerFinalMove()
    {
        StopAllCoroutines();
        collectionPointsCoroutine = null;
        pointCoroutine = null;
        npcMoveCoroutine = null;

        isLoop = false;
        npcLocationDictionary.Clear();
        npcLocationDictionary.Add(butlerFinalLocation.npcLocationState, butlerFinalLocation.npcTransforms);
        currentNPCTimePointIndex = 0;
        currentNPCLocationState = npcTimePoints[currentNPCTimePointIndex].npcLocationState;
        if (PlatterWithDrinks && RightHand)
        {
            PlatterWithDrinks.SetActive(false);
            RightHand.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            RightHand.transform.localRotation = Quaternion.identity;
        }

        ActivateTimeEvent();
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

        if (isLoop)
        {
            TimeManager.Instance.ScheduleAfter(delayBetweenMoves, ActivateTimeEvent);
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

    public void SetIsPausedNPCMovement(bool _isPausedNPCMovement)
    {
        isPausedNPCMovement = _isPausedNPCMovement;
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
            if (isPausedNPCMovement) { break; }

            yield return null;
        }

        navMeshAgent.isStopped = true;

        // move animation end
        _iProceduralWalkAnimation.StopWalking();

        if (isPausedNPCMovement)
        {
            yield return OnNPCIsTalking(_currentPoint);
        }
    }

    protected virtual IEnumerator OnNPCIsTalking(int _currentPoint)
    {
        yield return new WaitUntil(() => !isPausedNPCMovement);

        if (_currentPoint < npcLocationDictionary[currentNPCLocationState].Count)
        {
            yield return OnNPCMoveToPoint(_currentPoint);
        }
    }
}
