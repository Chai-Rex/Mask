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

        if (meshFilter && drinkEmpty)
        {
            meshFilter.mesh = drinkEmpty;
            isEmpty = true;
        }

        if (isPoison)
        {
            if (!isPoisoned && !isActive.Value) { return; }

            PlayTriggerSound();

            DeathManager.Instance.Die("Drink was Poisoned");
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
}
