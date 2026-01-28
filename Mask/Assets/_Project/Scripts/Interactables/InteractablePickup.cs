using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PickupableItem : MonoBehaviour, IInteractable {
    [SerializeField] private string _iItemName = "Item";
    [SerializeField] private string _iPickupVerb = "Pickup";

    private Rigidbody _rigidbody;

    public string InteractionVerb => _iPickupVerb;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
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
}