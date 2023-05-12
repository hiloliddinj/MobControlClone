using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class BlueCharacterController : MonoBehaviour
{
    public GameObject target;
    public bool isGunGenerate = false;
    public bool collidedToMultiplier = false;

    private NavMeshAgent _navMeshAgent;

    public void MoveAfterGunGenerate()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        if (isGunGenerate)
        {
            transform.DOMove(new Vector3(
                transform.position.x,
                transform.position.y - 0.6f,
                transform.position.z + 2), 0.5f);

            DOVirtual.DelayedCall(0.5f, () =>
            {
                _navMeshAgent.SetDestination(target.transform.position);
            });

        } else
        {
            _navMeshAgent.SetDestination(target.transform.position);
        }
    }

    public void Collided()
    {
        collidedToMultiplier = true;
        DOVirtual.DelayedCall(1.0f, () =>
        {
            collidedToMultiplier = false;
        });
    }

    public void Die()
    {
        isGunGenerate = false;
        collidedToMultiplier = false;
        gameObject.SetActive(false);
    }
}
