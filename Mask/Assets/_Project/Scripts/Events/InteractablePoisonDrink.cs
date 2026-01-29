using UnityEngine;

public class InteractablePoisonDrink : BaseTimeEvent, IInteractable
{
    private bool isActive = false;
    [SerializeField] private string verb = "Drink";

    public string InteractionVerb => verb;

    public void OnInteract(GameObject interactor)
    {
        if (!isActive) { return; }

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
        isActive = _isActive;

        if (isActive)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
