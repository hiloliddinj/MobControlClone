using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlueCharacterController : MonoBehaviour
{
    public GameObject target;

    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        
        //_navMeshAgent.destination = target.transform.position;
    }

    private void LateUpdate()
    {
        _navMeshAgent.SetDestination(target.transform.position);
    }
}
