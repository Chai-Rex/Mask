using UnityEngine;

public class ChandelierRopeCutTrigger : MonoBehaviour
{
    [SerializeField] private Chandelier chandelier;

    private BoxCollider deathCollider;

    private void Awake()
    {
        deathCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        deathCollider.enabled = false;
    }
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
