using UnityEngine;

public class ChandelierRopeCutTrigger : MonoBehaviour
{
    [SerializeField] private Chandelier chandelier;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (chandelier)
            {
                chandelier.StartFalling();
            }
        }
    }
}
