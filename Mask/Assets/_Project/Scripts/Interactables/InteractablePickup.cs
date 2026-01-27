using UnityEngine;

public class InteractablePickup : MonoBehaviour, IInteractable {

    [SerializeField] private string verb = "Pickup";
    [SerializeField] private string itemName = "Key";

    public string InteractionVerb => verb;

    public void OnInteract(GameObject i_interactor) {
        Debug.Log($"Picked up {itemName}");

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