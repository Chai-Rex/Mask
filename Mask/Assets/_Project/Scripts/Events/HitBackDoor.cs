using UnityEngine;

public class HitBackDoor : MonoBehaviour
{
    [SerializeField] private Door door;

    private void OnTriggerEnter(Collider other)
    {
        // Has NPC or Pllayer Touched Back of Door

        if (door != null && other.gameObject.layer == LayerMask.NameToLayer("Player") ||
            other.gameObject.tag == "NPC")
        {
            door.OnDoorOpen(false);
        }     
    }
}