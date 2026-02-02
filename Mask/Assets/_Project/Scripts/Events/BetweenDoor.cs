using UnityEngine;

public class BetweenDoor : MonoBehaviour
{
    private Door door;

    private void Awake()
    {
        door = GetComponent<Door>();
    }

    private void OnTriggerExit(Collider other)
    {
        // If NPC Or Player is not in Between

        if (door != null && other.gameObject.layer == LayerMask.NameToLayer("Player") ||
            other.gameObject.tag == "NPC")
        {
            door.OnDoorClose();
        }
    }
}
