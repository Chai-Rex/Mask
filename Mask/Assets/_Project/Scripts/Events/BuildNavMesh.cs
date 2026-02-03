using Unity.AI.Navigation;
using UnityEngine;

public class BuildNavMesh : MonoBehaviour
{
    [SerializeField] private NavMeshSurface currentNavMesh;

    private void Awake()
    {
        if (!currentNavMesh)
        { 
            currentNavMesh = GetComponent<NavMeshSurface>();
        }
    }

    private void Start()
    {
        currentNavMesh.BuildNavMesh();
    }
}
