using UnityEngine;

public class BetweenDoor : MonoBehaviour
{
    [SerializeField] private Door door;

    private void OnTriggerExit(Collider other)
    {
        // If NPC Or Player is not in Between

        if (door != null)
        {
            door.OnDoorClose();
        }
    }
}
