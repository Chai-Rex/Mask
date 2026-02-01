using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PickupableItem : BaseTimeEvent, IInteractable {
    [SerializeField] private string _iItemName = "Item";
    [SerializeField] private string _iPickupVerb = "Pickup";

    private string _stateString;

    private Rigidbody _rigidbody;

    public string InteractionVerb => _iPickupVerb;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();

        _stateString = "is" + _iItemName + "PickedUp";
        StoryStateSO.RegisterInitialState(new StateVariable(_stateString, false, false));
    }

    public async void OnInteract(GameObject interactor) {
        PickupHandler pickupHandler = interactor.GetComponent<PickupHandler>();

        await pickupHandler.PickupItem(gameObject);
    }

    public void OnLookEnter(GameObject looker) {

    }

    public void OnLookExit(GameObject looker) {

    }

    public Transform GetTransform() {
        return transform;
    }

    virtual public void SetItemPickedUp(bool i_isPickedUp)
    {
        StoryStateSO.Instance.SetValue(_stateString, i_isPickedUp);
    }
}