using UnityEngine;

public class InteractablePoisonDrink : BaseTimeEvent, IInteractable
{
    private StateVariable isActive = new StateVariable("isPoisonDrinkActive", false);
    [SerializeField] private string verb = "Drink";

    [SerializeField] private Mesh drinkFull;
    [SerializeField] private Mesh drinkEmpty;

    [SerializeField] bool isPoison = true;
    private bool isPoisoned = false;
    [SerializeField] float poisonScheduledTime = 120.0f;
    [SerializeField] float fillDrinkDelay = 5.0f;

    private MeshFilter meshFilter;

    public string InteractionVerb => verb;

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
        if (meshFilter && drinkEmpty && meshFilter.mesh == drinkEmpty) { return; }

        PlayTriggerSound();

        if (meshFilter && drinkEmpty)
        {
            meshFilter.mesh = drinkEmpty;
        }

        if (isPoison)
        {
            if (!isPoisoned && !isActive.Value) { return; }

            DeathManager.Instance.Die("Drink was Poisoned", "Drink");
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
        if (meshFilter && drinkFull && meshFilter.mesh != drinkFull)
        {
            meshFilter.mesh = drinkFull;
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
