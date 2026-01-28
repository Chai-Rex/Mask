using UnityEngine;

public class InteractablePickup : MonoBehaviour, IInteractable {

    [SerializeField] private string _iVerb = "Pickup";
    [SerializeField] private string _iItemName = "Key";

    public string InteractionVerb => _iVerb;

    public void OnInteract(GameObject i_interactor) {
        Debug.Log($"Picked up {_iItemName}");

        // animate hand moving to item and picking it up.
        // i_interactor.GetComponent<typeof(hands)>()
        // hands.PickupItem(this)

        Destroy(gameObject);
    }

    public void OnLookEnter(GameObject i_looker) {
        // Could add a glow effect or outline here
    }

    public void OnLookExit(GameObject i_looker) {
        // Remove glow effect
    }

    public Transform GetTransform() {
        return transform;
    }
}