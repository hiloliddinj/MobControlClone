using UnityEngine;
using UnityEngine.AI;

public class RedCharacterController : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;

    private NavMeshAgent _navMeshAgent;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        _navMeshAgent.SetDestination(_targetTransform.position);
    }

}
