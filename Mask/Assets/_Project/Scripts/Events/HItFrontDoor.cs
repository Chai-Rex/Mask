using UnityEngine;

public class HitFrontDoor : MonoBehaviour
{
    [SerializeField] private Door door;

    private void OnTriggerEnter(Collider other)
    {
        // Has NPC or Pllayer Touched Front of Door

        if (door != null && other.gameObject.layer == LayerMask.NameToLayer("Player") ||
            other.gameObject.tag == "NPC")
        {
            door.OnDoorOpen(true);
        }    
    }
}
