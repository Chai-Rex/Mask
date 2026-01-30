using UnityEngine;

public class InteractablePoisonDrink : BaseTimeEvent, IInteractable
{
    private StateVariable isActive = new StateVariable("isPoisonDrinkActive", false);
    [SerializeField] private string verb = "Drink";

    public string InteractionVerb => verb;

    public void OnInteract(GameObject interactor)
    {
        if (!isActive.Value) { return; }

        // Player Death
        DeathManager.Instance.Die("Drink was Poisoned");

        Destroy(gameObject);
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        SetPoisonDrinkIsActive(true);
    }

    public void SetPoisonDrinkIsActive(bool _isActive)
    {
        isActive.SetValueAndUpdateBlackboard(_isActive);

        if (isActive.Value)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
