using UnityEngine;

public class HitBackDoor : MonoBehaviour
{
    [SerializeField] private Door door;

    private void OnTriggerEnter(Collider other)
    {
        // Has NPC or Pllayer Touched Back of Door

        if (door != null)
        {
            door.OnDoorOpen(false);
        }     
    }
}