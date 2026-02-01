using System.Collections;
using UnityEngine;

public class InteractablePoisonDrink : BaseTimeEvent, IInteractable
{
    private StateVariable isActive = new StateVariable("isPoisonDrinkActive", false);
    [SerializeField] private string verb = "Drink";
    [SerializeField] private string verbWhenEmpty = "Empty";

    [SerializeField] private Mesh drinkFull;
    [SerializeField] private Mesh drinkEmpty;
    private bool isEmpty = false;

    [SerializeField] bool isPoison = true;
    private bool isPoisoned = false;
    [SerializeField] float poisonScheduledTime = 120.0f;
    [SerializeField] float fillDrinkDelay = 5.0f;
    [SerializeField] float deathDelay = 3.0f;
    [SerializeField] float drinkDelay = 2.5f;

    private Coroutine poisonCoroutine;

    private MeshFilter meshFilter;

    public string InteractionVerb => isEmpty ? verbWhenEmpty : verb;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (drinkFull)
        {
            meshFilter.mesh = drinkFull;
        }
    }

    private void Start()
    {
        if (isPoison)
        {
            TimeManager.Instance.ScheduleAt(poisonScheduledTime, ActivateTimeEvent);
        }
    }

    public void OnInteract(GameObject interactor)
    {
        if (meshFilter && drinkEmpty && isEmpty) { return; }

        if (soundClips.Count != 0 && soundClips.Count >= 2)
        {
            _eventAudioData.Clip = soundClips[0];
        }

        PlayTriggerSound();
        ResetSoundTriggered();

        if (meshFilter && drinkEmpty)
        {
            meshFilter.mesh = drinkEmpty;
            isEmpty = true;

            interactor.GetComponent<InteractionHandler>().OverrideInteractionPrompt(verbWhenEmpty);
        }

        if (isPoison)
        {
            if (!isPoisoned && !isActive.Value) { return; }

            if (poisonCoroutine == null)
            {
                poisonCoroutine = StartCoroutine(OnGotPoisoned());
            }

        }
        else
        {
            TimeManager.Instance.ScheduleAfter(fillDrinkDelay, ActivateTimeEvent);
        }
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        if (isPoison)
        {
            SetPoisonDrinkIsActive(true);
        }
        else
        {
            OnFillUpDrink();
        }
    }

    private IEnumerator OnGotPoisoned()
    {
        yield return new WaitForSeconds(drinkDelay);

        if (soundClips.Count != 0 && soundClips.Count >= 2)
        {
            _eventAudioData.Clip = soundClips[1];
        }

        PlayTriggerSound();

        yield return new WaitForSeconds(deathDelay);

        DeathManager.Instance.Die("Drink was Poisoned", "Drink");
    }

    public void OnFillUpDrink()
    {
        if (meshFilter && drinkFull && isEmpty)
        {
            meshFilter.mesh = drinkFull;
            isEmpty = false;
        }
    }

    public void SetPoisonDrinkIsActive(bool _isActive)
    {
        if (isPoison)
        {
            isActive.SetValueAndUpdateBlackboard(_isActive);
            isPoisoned = true;
        }

        OnFillUpDrink();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        poisonCoroutine = null;
    }
}
