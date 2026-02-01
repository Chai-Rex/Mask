using UnityEngine;

public class Bed : BaseTimeEvent, IInteractable
{
    private StateVariable _hasNapActive = new StateVariable("", false, false);
    [SerializeField] private string verb = "Nap";
    [SerializeField] private string verbNotSleepy = "Not Sleepy";

    public string InteractionVerb => StoryStateSO.Instance.GetValue("isNapActive") ? verbNotSleepy : verb;

    private void Awake()
    {
        _hasNapActive = new StateVariable("isNapActive", false);
    }

    public void OnInteract(GameObject interactor)
    {
        if (!StoryStateSO.Instance.GetValue("isNapActive"))
        {
            _hasNapActive.SetValueAndUpdateBlackboard(true);
            PlayTriggerSound();
            DeathManager.Instance.Sleep();
        }
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();
    }
}
